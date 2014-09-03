using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class CashSalesInvoiceDetailService : ICashSalesInvoiceDetailService
    {
        private ICashSalesInvoiceDetailRepository _repository;
        private ICashSalesInvoiceDetailValidator _validator;
        public CashSalesInvoiceDetailService(ICashSalesInvoiceDetailRepository _cashSalesInvoiceDetailRepository, ICashSalesInvoiceDetailValidator _cashSalesInvoiceDetailValidator)
        {
            _repository = _cashSalesInvoiceDetailRepository;
            _validator = _cashSalesInvoiceDetailValidator;
        }

        public ICashSalesInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CashSalesInvoiceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashSalesInvoiceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<CashSalesInvoiceDetail> GetQueryableObjectsByCashSalesInvoiceId(int CashSalesInvoiceId)
        {
            return _repository.GetQueryableObjectsByCashSalesInvoiceId(CashSalesInvoiceId);
        }

        public IList<CashSalesInvoiceDetail> GetObjectsByCashSalesInvoiceId(int CashSalesInvoiceId)
        {
            return _repository.GetObjectsByCashSalesInvoiceId(CashSalesInvoiceId);
        }

        public CashSalesInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CashSalesInvoiceDetail CreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, 
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, 
                                                     IQuantityPricingService _quantityPricingService)
        {
            cashSalesInvoiceDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, this, _itemService, _warehouseItemService, _quantityPricingService))
            {
                Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
                QuantityPricing quantityPricing = _quantityPricingService.GetObjectByItemTypeIdAndQuantity(item.ItemTypeId, cashSalesInvoiceDetail.Quantity);
                decimal price = item.SellingPrice;
                decimal discount = cashSalesInvoiceDetail.Discount;
                if (cashSalesInvoiceDetail.IsManualPriceAssignment)
                {
                    price = cashSalesInvoiceDetail.AssignedPrice;
                }
                if (quantityPricing != null)
                {
                    if(cashSalesInvoiceDetail.Discount <= 0)
                    {
                        discount = quantityPricing.Discount;
                    };
                    
                }
                cashSalesInvoiceDetail.Amount = (price * (100 - discount) / 100) * cashSalesInvoiceDetail.Quantity;
                cashSalesInvoiceDetail.CoGS = cashSalesInvoiceDetail.Quantity * item.AvgPrice; 

                CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
                cashSalesInvoiceDetail.PriceMutationId = item.PriceMutationId;
                
                cashSalesInvoiceDetail = _repository.CreateObject(cashSalesInvoiceDetail);
                cashSalesInvoice.Total = CalculateTotal(cashSalesInvoice.Id);
                cashSalesInvoice.CoGS = CalculateCoGS(cashSalesInvoice.Id);
                _cashSalesInvoiceService.GetRepository().Update(cashSalesInvoice);
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail UpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                                     IQuantityPricingService _quantityPricingService)
        {
            if (_validator.ValidUpdateObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, this, _itemService, _warehouseItemService, _quantityPricingService))
            {
                Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
                QuantityPricing quantityPricing = _quantityPricingService.GetObjectByItemTypeIdAndQuantity(item.ItemTypeId, cashSalesInvoiceDetail.Quantity);
                decimal price = item.SellingPrice;
                decimal discount = cashSalesInvoiceDetail.Discount;
                if (cashSalesInvoiceDetail.IsManualPriceAssignment)
                {
                    price = cashSalesInvoiceDetail.AssignedPrice;
                }
                if (quantityPricing != null)
                {
                    if (cashSalesInvoiceDetail.Discount <= 0)
                    {
                        discount = quantityPricing.Discount;
                    };

                }
                cashSalesInvoiceDetail.Amount = (price * (100 - discount) / 100) * cashSalesInvoiceDetail.Quantity;
                cashSalesInvoiceDetail.CoGS = cashSalesInvoiceDetail.Quantity * item.AvgPrice; 

                CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
                cashSalesInvoiceDetail.PriceMutationId = item.PriceMutationId;
                cashSalesInvoiceDetail = _repository.UpdateObject(cashSalesInvoiceDetail);
                cashSalesInvoice.Total = CalculateTotal(cashSalesInvoice.Id);
                cashSalesInvoice.CoGS = CalculateCoGS(cashSalesInvoice.Id);
                _cashSalesInvoiceService.GetRepository().Update(cashSalesInvoice);
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail ConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, 
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if(_validator.ValidConfirmObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, _warehouseItemService))
            {
                CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(cashSalesInvoice.WarehouseId, cashSalesInvoiceDetail.ItemId);
                Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
                StockMutation stockMutation = new StockMutation()
                {
                    ItemId = cashSalesInvoiceDetail.ItemId,
                    ItemCase = Core.Constants.Constant.ItemCase.Ready,
                    Status = Core.Constants.Constant.MutationStatus.Deduction,
                    Quantity = cashSalesInvoiceDetail.Quantity,
                    SourceDocumentId = cashSalesInvoice.Id,
                    SourceDocumentType = Core.Constants.Constant.SourceDocumentType.CashSalesInvoice,
                    SourceDocumentDetailId = cashSalesInvoiceDetail.Id,
                    SourceDocumentDetailType = Core.Constants.Constant.SourceDocumentDetailType.CashSalesInvoiceDetail,
                    WarehouseId = cashSalesInvoice.WarehouseId,
                    WarehouseItemId = warehouseItem.Id
                };
                stockMutation = _stockMutationService.CreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                stockMutation.CreatedAt = (DateTime)cashSalesInvoice.ConfirmationDate.GetValueOrDefault();
                _stockMutationService.UpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                cashSalesInvoiceDetail.CoGS = cashSalesInvoiceDetail.Quantity * item.AvgPrice;
                cashSalesInvoiceDetail = _repository.ConfirmObject(cashSalesInvoiceDetail);
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail UnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(cashSalesInvoiceDetail))
            {
                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(cashSalesInvoiceDetail.ItemId, Core.Constants.Constant.SourceDocumentDetailType.CashSalesInvoiceDetail, cashSalesInvoiceDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    stockMutation.Errors = new Dictionary<string, string>();
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    _stockMutationService.SoftDeleteObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                }
                cashSalesInvoiceDetail.CoGS = 0;
                cashSalesInvoiceDetail = _repository.UnconfirmObject(cashSalesInvoiceDetail);
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail SoftDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService)
        {
            if(_validator.ValidDeleteObject(cashSalesInvoiceDetail, _cashSalesInvoiceService)) {
                CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
                _repository.SoftDeleteObject(cashSalesInvoiceDetail);
                cashSalesInvoice.Total = CalculateTotal(cashSalesInvoice.Id);
                cashSalesInvoice.CoGS = CalculateCoGS(cashSalesInvoice.Id);
                _cashSalesInvoiceService.GetRepository().Update(cashSalesInvoice);
            };
            return cashSalesInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public decimal CalculateTotal(int CashSalesInvoiceId)
        {
            IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = GetObjectsByCashSalesInvoiceId(CashSalesInvoiceId);
            decimal Total = 0;
            foreach (var cashSalesInvoiceDetail in cashSalesInvoiceDetails)
            {
                Total += cashSalesInvoiceDetail.Amount;
            }
            return Total;
        }

        public decimal CalculateCoGS(int CashSalesInvoiceId)
        {
            IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = GetObjectsByCashSalesInvoiceId(CashSalesInvoiceId);
            decimal Total = 0;
            foreach (var cashSalesInvoiceDetail in cashSalesInvoiceDetails)
            {
                Total += cashSalesInvoiceDetail.CoGS;
            }
            return Total;
        }

    }
}
