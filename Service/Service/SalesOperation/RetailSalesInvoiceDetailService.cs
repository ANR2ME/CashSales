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
    public class RetailSalesInvoiceDetailService : IRetailSalesInvoiceDetailService
    {
        private IRetailSalesInvoiceDetailRepository _repository;
        private IRetailSalesInvoiceDetailValidator _validator;
        public RetailSalesInvoiceDetailService(IRetailSalesInvoiceDetailRepository _retailSalesInvoiceDetailRepository, IRetailSalesInvoiceDetailValidator _retailSalesInvoiceDetailValidator)
        {
            _repository = _retailSalesInvoiceDetailRepository;
            _validator = _retailSalesInvoiceDetailValidator;
        }

        public IRetailSalesInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<RetailSalesInvoiceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RetailSalesInvoiceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<RetailSalesInvoiceDetail> GetQueryableObjectsByRetailSalesInvoiceId(int RetailSalesInvoiceId)
        {
            return _repository.GetQueryableObjectsByRetailSalesInvoiceId(RetailSalesInvoiceId);
        }

        public IList<RetailSalesInvoiceDetail> GetObjectsByRetailSalesInvoiceId(int RetailSalesInvoiceId)
        {
            return _repository.GetObjectsByRetailSalesInvoiceId(RetailSalesInvoiceId);
        }

        public RetailSalesInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RetailSalesInvoiceDetail CreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, 
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            retailSalesInvoiceDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(retailSalesInvoiceDetail.ItemId);
                RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);

                if (retailSalesInvoiceDetail.IsManualPriceAssignment)
                {
                    //priceMutation = _priceMutationService.CreateObject(retailSalesInvoiceDetail.ItemId, retailSalesInvoiceDetail.AssignedPrice, DateTime.Now);

                    retailSalesInvoiceDetail.PriceMutationId = priceMutation.Id;
                    retailSalesInvoiceDetail.Amount = (retailSalesInvoiceDetail.AssignedPrice * retailSalesInvoiceDetail.Quantity) * (100 - retailSalesInvoiceDetail.Discount) / 100;
                    //item.PriceMutationId = priceMutation.Id;
                    //_itemService.GetRepository().Update(item);
                }
                else
                {
                    retailSalesInvoiceDetail.PriceMutationId = item.PriceMutationId;
                    retailSalesInvoiceDetail.Amount = (priceMutation.Amount * retailSalesInvoiceDetail.Quantity) * (100 - retailSalesInvoiceDetail.Discount) / 100;
                }
                retailSalesInvoiceDetail.CoGS = retailSalesInvoiceDetail.Quantity * item.AvgPrice; 

                retailSalesInvoiceDetail = _repository.CreateObject(retailSalesInvoiceDetail);
                if (retailSalesInvoiceDetail.IsManualPriceAssignment)
                {
                    priceMutation.CreatedAt = retailSalesInvoiceDetail.CreatedAt;
                    _priceMutationService.GetRepository().Update(priceMutation);
                }

                retailSalesInvoice.Total = CalculateTotal(retailSalesInvoice.Id);
                retailSalesInvoice.Total = CalculateCoGS(retailSalesInvoice.Id);
                _retailSalesInvoiceService.GetRepository().Update(retailSalesInvoice);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail UpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(retailSalesInvoiceDetail.ItemId);
                RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);

                if (retailSalesInvoiceDetail.IsManualPriceAssignment)
                {
                    //priceMutation = _priceMutationService.CreateObject(retailSalesInvoiceDetail.ItemId, retailSalesInvoiceDetail.AssignedPrice, DateTime.Now);

                    retailSalesInvoiceDetail.PriceMutationId = priceMutation.Id;
                    retailSalesInvoiceDetail.Amount = (retailSalesInvoiceDetail.AssignedPrice * retailSalesInvoiceDetail.Quantity) * (100 - retailSalesInvoiceDetail.Discount) / 100;
                    //item.PriceMutationId = priceMutation.Id;
                    //_itemService.GetRepository().Update(item);
                }
                else
                {
                    retailSalesInvoiceDetail.PriceMutationId = item.PriceMutationId;
                    retailSalesInvoiceDetail.Amount = (priceMutation.Amount * retailSalesInvoiceDetail.Quantity) * (100 - retailSalesInvoiceDetail.Discount) / 100;
                }
                retailSalesInvoiceDetail.CoGS = retailSalesInvoiceDetail.Quantity * item.AvgPrice;

                retailSalesInvoiceDetail = _repository.UpdateObject(retailSalesInvoiceDetail);
                if (retailSalesInvoiceDetail.IsManualPriceAssignment)
                {
                    priceMutation.CreatedAt = retailSalesInvoiceDetail.CreatedAt;
                    _priceMutationService.GetRepository().Update(priceMutation);
                }

                retailSalesInvoice.Total = CalculateTotal(retailSalesInvoice.Id);
                retailSalesInvoice.Total = CalculateCoGS(retailSalesInvoice.Id);
                _retailSalesInvoiceService.GetRepository().Update(retailSalesInvoice);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail ConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService, 
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if(_validator.ValidConfirmObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _warehouseItemService))
            {
                RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(retailSalesInvoice.WarehouseId, retailSalesInvoiceDetail.ItemId);
                Item item = _itemService.GetObjectById(retailSalesInvoiceDetail.ItemId);
                StockMutation stockMutation = new StockMutation()
                {
                    ItemId = retailSalesInvoiceDetail.ItemId,
                    ItemCase = Core.Constants.Constant.ItemCase.Ready,
                    Status = Core.Constants.Constant.MutationStatus.Deduction,
                    Quantity = retailSalesInvoiceDetail.Quantity,
                    SourceDocumentCode = retailSalesInvoice.Code,
                    SourceDocumentId = retailSalesInvoice.Id,
                    SourceDocumentType = Core.Constants.Constant.SourceDocumentType.RetailSalesInvoice,
                    SourceDocumentDetailCode = retailSalesInvoiceDetail.Code,
                    SourceDocumentDetailId = retailSalesInvoiceDetail.Id,
                    SourceDocumentDetailType = Core.Constants.Constant.SourceDocumentDetailType.RetailSalesInvoiceDetail,
                    WarehouseId = retailSalesInvoice.WarehouseId,
                    WarehouseItemId = warehouseItem.Id
                };
                stockMutation = _stockMutationService.CreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                stockMutation.CreatedAt = (DateTime)retailSalesInvoice.ConfirmationDate.GetValueOrDefault();
                _stockMutationService.UpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                retailSalesInvoiceDetail.CoGS = retailSalesInvoiceDetail.Quantity * item.AvgPrice;
                retailSalesInvoiceDetail = _repository.ConfirmObject(retailSalesInvoiceDetail);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail UnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(retailSalesInvoiceDetail))
            {
                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(retailSalesInvoiceDetail.ItemId, Core.Constants.Constant.SourceDocumentDetailType.RetailSalesInvoiceDetail, retailSalesInvoiceDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    stockMutation.Errors = new Dictionary<string, string>();
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    _stockMutationService.SoftDeleteObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                }
                retailSalesInvoiceDetail.CoGS = 0;
                retailSalesInvoiceDetail = _repository.UnconfirmObject(retailSalesInvoiceDetail);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail SoftDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService)
        {
            if(_validator.ValidDeleteObject(retailSalesInvoiceDetail, _retailSalesInvoiceService))
            {
                RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
                _repository.SoftDeleteObject(retailSalesInvoiceDetail);
                retailSalesInvoice.Total = CalculateTotal(retailSalesInvoice.Id);
                retailSalesInvoice.Total = CalculateCoGS(retailSalesInvoice.Id);
                _retailSalesInvoiceService.GetRepository().Update(retailSalesInvoice);
            }
            return retailSalesInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public decimal CalculateTotal(int RetailSalesInvoiceId)
        {
            IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = GetObjectsByRetailSalesInvoiceId(RetailSalesInvoiceId);
            decimal Total = 0;
            foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
            {
                Total += retailSalesInvoiceDetail.Amount;
            }
            return Total;
        }

        public decimal CalculateCoGS(int RetailSalesInvoiceId)
        {
            IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = GetObjectsByRetailSalesInvoiceId(RetailSalesInvoiceId);
            decimal Total = 0;
            foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
            {
                Total += retailSalesInvoiceDetail.CoGS;
            }
            return Total;
        }

    }
}
