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
    public class CashSalesReturnDetailService : ICashSalesReturnDetailService
    {
        private ICashSalesReturnDetailRepository _repository;
        private ICashSalesReturnDetailValidator _validator;
        public CashSalesReturnDetailService(ICashSalesReturnDetailRepository _cashSalesReturnDetailRepository, ICashSalesReturnDetailValidator _cashSalesReturnDetailValidator)
        {
            _repository = _cashSalesReturnDetailRepository;
            _validator = _cashSalesReturnDetailValidator;
        }

        public ICashSalesReturnDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CashSalesReturnDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashSalesReturnDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<CashSalesReturnDetail> GetQueryableObjectsByCashSalesReturnId(int CashSalesReturnId)
        {
            return _repository.GetQueryableObjectsByCashSalesReturnId(CashSalesReturnId);
        }

        public IList<CashSalesReturnDetail> GetObjectsByCashSalesReturnId(int CashSalesReturnId)
        {
            return _repository.GetObjectsByCashSalesReturnId(CashSalesReturnId);
        }

        public CashSalesReturnDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<CashSalesReturnDetail> GetObjectsByCashSalesInvoiceDetailId(int CashSalesInvoiceDetailId)
        {
            return _repository.GetObjectsByCashSalesInvoiceDetailId(CashSalesInvoiceDetailId);
        }

        public CashSalesReturnDetail CreateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                                  ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            cashSalesReturnDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(cashSalesReturnDetail, _cashSalesReturnService, this, _cashSalesInvoiceDetailService))
            {
                CashSalesReturn cashSalesReturn = _cashSalesReturnService.GetObjectById(cashSalesReturnDetail.CashSalesReturnId);
                CashSalesInvoiceDetail cashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(cashSalesReturnDetail.CashSalesInvoiceDetailId);
                cashSalesReturnDetail.TotalPrice = (cashSalesInvoiceDetail.Amount / cashSalesInvoiceDetail.Quantity) * cashSalesReturnDetail.Quantity;
                cashSalesReturnDetail.CoGS = (cashSalesInvoiceDetail.CoGS / cashSalesInvoiceDetail.Quantity) * cashSalesReturnDetail.Quantity;
                cashSalesReturnDetail = _repository.CreateObject(cashSalesReturnDetail);
                cashSalesReturn = CalculateTotal(cashSalesReturn);
                _cashSalesReturnService.GetRepository().Update(cashSalesReturn);
            }
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail UpdateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                                  ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            if (_validator.ValidUpdateObject(cashSalesReturnDetail, _cashSalesReturnService, this, _cashSalesInvoiceDetailService))
            {
                CashSalesReturn cashSalesReturn = _cashSalesReturnService.GetObjectById(cashSalesReturnDetail.CashSalesReturnId);
                CashSalesInvoiceDetail cashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(cashSalesReturnDetail.CashSalesInvoiceDetailId);
                cashSalesReturnDetail.TotalPrice = (cashSalesInvoiceDetail.Amount / cashSalesInvoiceDetail.Quantity) * cashSalesReturnDetail.Quantity;
                cashSalesReturnDetail.CoGS = (cashSalesInvoiceDetail.CoGS / cashSalesInvoiceDetail.Quantity) * cashSalesReturnDetail.Quantity;
                cashSalesReturnDetail = _repository.UpdateObject(cashSalesReturnDetail);
                cashSalesReturn = CalculateTotal(cashSalesReturn);
                _cashSalesReturnService.GetRepository().Update(cashSalesReturn);
            }
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail ConfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService, 
                                                   ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                   IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                                   IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            if(_validator.ValidConfirmObject(cashSalesReturnDetail, _cashSalesInvoiceDetailService, _cashSalesReturnDetailService))
            {
                CashSalesReturn cashSalesReturn = _cashSalesReturnService.GetObjectById(cashSalesReturnDetail.CashSalesReturnId);
                CashSalesInvoiceDetail cashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(cashSalesReturnDetail.CashSalesInvoiceDetailId);
                CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesReturn.CashSalesInvoiceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(cashSalesInvoice.WarehouseId, cashSalesInvoiceDetail.ItemId);
                Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
                StockMutation stockMutation = new StockMutation()
                {
                    ItemId = cashSalesInvoiceDetail.ItemId,
                    ItemCase = Core.Constants.Constant.ItemCase.Ready,
                    Status = Core.Constants.Constant.MutationStatus.Addition,
                    Quantity = cashSalesReturnDetail.Quantity,
                    SourceDocumentCode = cashSalesReturn.Code,
                    SourceDocumentId = cashSalesReturn.Id,
                    SourceDocumentType = Core.Constants.Constant.SourceDocumentType.CashSalesReturn,
                    SourceDocumentDetailCode = cashSalesReturnDetail.Code,
                    SourceDocumentDetailId = cashSalesReturnDetail.Id,
                    SourceDocumentDetailType = Core.Constants.Constant.SourceDocumentDetailType.CashSalesReturnDetail,
                    WarehouseId = cashSalesInvoice.WarehouseId,
                    WarehouseItemId = warehouseItem.Id
                };
                stockMutation = _stockMutationService.CreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                stockMutation.CreatedAt = (DateTime)cashSalesReturn.ConfirmationDate.GetValueOrDefault();
                _stockMutationService.UpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, cashSalesReturnDetail.Quantity, (cashSalesInvoiceDetail.CoGS / cashSalesInvoiceDetail.Quantity));
                cashSalesReturnDetail = _repository.ConfirmObject(cashSalesReturnDetail);
            }
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail UnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                     IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, 
                                                     IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(cashSalesReturnDetail))
            {
                CashSalesInvoiceDetail cashSalesInvoiceDetail = _cashSalesInvoiceDetailService.GetObjectById(cashSalesReturnDetail.CashSalesInvoiceDetailId);
                Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(cashSalesInvoiceDetail.ItemId, Core.Constants.Constant.SourceDocumentDetailType.CashSalesReturnDetail, cashSalesReturnDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    stockMutation.Errors = new Dictionary<string, string>();
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    _stockMutationService.SoftDeleteObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                }
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, cashSalesReturnDetail.Quantity * (-1), (cashSalesInvoiceDetail.CoGS / cashSalesInvoiceDetail.Quantity));
                cashSalesReturnDetail = _repository.UnconfirmObject(cashSalesReturnDetail);
            }
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail SoftDeleteObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService)
        {
            if(_validator.ValidDeleteObject(cashSalesReturnDetail, _cashSalesReturnService)) 
            {
                CashSalesReturn cashSalesReturn = _cashSalesReturnService.GetObjectById(cashSalesReturnDetail.CashSalesReturnId);
                _repository.SoftDeleteObject(cashSalesReturnDetail);
                cashSalesReturn = CalculateTotal(cashSalesReturn);
                _cashSalesReturnService.GetRepository().Update(cashSalesReturn);
            }
            return cashSalesReturnDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        //public decimal CalculateTotal(int CashSalesReturnId)
        //{
        //    IList<CashSalesReturnDetail> cashSalesReturnDetails = GetObjectsByCashSalesReturnId(CashSalesReturnId);
        //    decimal Total = 0;
        //    decimal CoGS = 0;
        //    foreach (var cashSalesReturnDetail in cashSalesReturnDetails)
        //    {
        //        Total += cashSalesReturnDetail.TotalPrice;
        //        CoGS += cashSalesReturnDetail.CoGS;
        //    }
        //    return Total;
        //}

        public CashSalesReturn CalculateTotal(CashSalesReturn cashSalesReturn)
        {
            IList<CashSalesReturnDetail> cashSalesReturnDetails = GetObjectsByCashSalesReturnId(cashSalesReturn.Id);
            decimal Total = 0;
            decimal CoGS = 0;
            foreach (var cashSalesReturnDetail in cashSalesReturnDetails)
            {
                Total += cashSalesReturnDetail.TotalPrice;
                CoGS += cashSalesReturnDetail.CoGS;
            }
            cashSalesReturn.Total = Total;
            cashSalesReturn.CoGS = CoGS;
            return cashSalesReturn;
        }

        public int GetTotalQuantityByCashSalesInvoiceDetailId(int Id)
        {
            IList<CashSalesReturnDetail> cashSalesReturnDetails = GetObjectsByCashSalesInvoiceDetailId(Id);
            int Quantity = 0;
            foreach (var cashSalesReturnDetail in cashSalesReturnDetails)
            {
                Quantity += cashSalesReturnDetail.Quantity;
            }
            return Quantity;
        }
    }
}
