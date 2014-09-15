using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class StockAdjustmentDetailService : IStockAdjustmentDetailService
    {
        private IStockAdjustmentDetailRepository _repository;
        private IStockAdjustmentDetailValidator _validator;

        public StockAdjustmentDetailService(IStockAdjustmentDetailRepository _stockAdjustmentDetailRepository, IStockAdjustmentDetailValidator _stockAdjustmentDetailValidator)
        {
            _repository = _stockAdjustmentDetailRepository;
            _validator = _stockAdjustmentDetailValidator;
        }

        public IStockAdjustmentDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<StockAdjustmentDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<StockAdjustmentDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<StockAdjustmentDetail> GetQueryableObjectsByStockAdjustmentId(int stockAdjustmentId)
        {
            return _repository.GetQueryableObjectsByStockAdjustmentId(stockAdjustmentId);
        }

        public IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int stockAdjustmentId)
        {
            return _repository.GetObjectsByStockAdjustmentId(stockAdjustmentId);
        }

        public IList<StockAdjustmentDetail> GetObjectsByItemId(int itemId)
        {
            return _repository.GetObjectsByItemId(itemId);
        }

        public StockAdjustmentDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustmentDetail.Errors = new Dictionary<String, String>();
            return (stockAdjustmentDetail = _validator.ValidCreateObject(stockAdjustmentDetail, this, _stockAdjustmentService, _itemService, _warehouseItemService) ?
                                            _repository.CreateObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail CreateObject(int stockAdjustmentId, int itemId, int quantity, decimal price,
                                                    IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            StockAdjustmentDetail stockAdjustmentDetail = new StockAdjustmentDetail
            {
                StockAdjustmentId = stockAdjustmentId,
                ItemId = itemId,
                Quantity = quantity,
                Price = price
            };
            return this.CreateObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _warehouseItemService);
        }

        public StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (stockAdjustmentDetail = _validator.ValidUpdateObject(stockAdjustmentDetail, this, _stockAdjustmentService, _itemService, _warehouseItemService) ?
                                            _repository.UpdateObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            return (stockAdjustmentDetail = _validator.ValidDeleteObject(stockAdjustmentDetail) ? _repository.SoftDeleteObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, DateTime ConfirmationDate, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                                   IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustmentDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
            {
                stockAdjustmentDetail = _repository.ConfirmObject(stockAdjustmentDetail);
                StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
                Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetail.Price);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(stockAdjustment.WarehouseId, item.Id);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForStockAdjustment(stockAdjustmentDetail, warehouseItem, _stockAdjustmentService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
            }
            return stockAdjustmentDetail;
        }
        public StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
            {
                stockAdjustmentDetail = _repository.UnconfirmObject(stockAdjustmentDetail);
                StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
                Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, stockAdjustmentDetail.Quantity * (-1), stockAdjustmentDetail.Price);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(stockAdjustment.WarehouseId, item.Id);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForStockAdjustment(stockAdjustmentDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return stockAdjustmentDetail;
        }
        

    }
}