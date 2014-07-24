﻿using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class WarehouseMutationOrderDetailService : IWarehouseMutationOrderDetailService
    {
        private IWarehouseMutationOrderDetailRepository _repository;
        private IWarehouseMutationOrderDetailValidator _validator;

        public WarehouseMutationOrderDetailService(IWarehouseMutationOrderDetailRepository _warehouseMutationOrderDetailRepository, IWarehouseMutationOrderDetailValidator _warehouseMutationOrderDetailValidator)
        {
            _repository = _warehouseMutationOrderDetailRepository;
            _validator = _warehouseMutationOrderDetailValidator;
        }

        public IWarehouseMutationOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<WarehouseMutationOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId)
        {
            return _repository.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrderId);
        }

        public WarehouseMutationOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                         IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrderDetail.Errors = new Dictionary<String, String>();
            return (warehouseMutationOrderDetail = _validator.ValidCreateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, this, _itemService, _warehouseItemService) ?
                                                   _repository.CreateObject(warehouseMutationOrderDetail) : warehouseMutationOrderDetail);
        }

        public WarehouseMutationOrderDetail CreateObject(int warehouseMutationOrderId, int itemId, int quantity,
                                                    IWarehouseMutationOrderService _warehouseMutationOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutationOrderDetail warehouseMutationOrderDetail = new WarehouseMutationOrderDetail
            {
                WarehouseMutationOrderId = warehouseMutationOrderId,
                ItemId = itemId,
                Quantity = quantity,
                // Price = price
            };
            return this.CreateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _warehouseItemService);
        }

        public WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (warehouseMutationOrderDetail = _validator.ValidUpdateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, this, _itemService, _warehouseItemService) ?
                                                   _repository.UpdateObject(warehouseMutationOrderDetail) : warehouseMutationOrderDetail);
        }

        public WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService)
        {
            return (warehouseMutationOrderDetail = _validator.ValidDeleteObject(warehouseMutationOrderDetail) ? _repository.SoftDeleteObject(warehouseMutationOrderDetail) : warehouseMutationOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public WarehouseMutationOrderDetail FinishObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidFinishObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService))
            {
                WarehouseMutationOrder warehouseMutationOrder = _warehouseMutationOrderService.GetObjectById(warehouseMutationOrderDetail.WarehouseMutationOrderId);

                _repository.FinishObject(warehouseMutationOrderDetail);
                if (_warehouseMutationOrderService.GetValidator().ValidCompleteObject(warehouseMutationOrder, this))
                {
                    _warehouseMutationOrderService.CompleteObject(warehouseMutationOrder, this);
                }

                // deduce warehouseFrom item
                // add warehouseTo item
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseFromId, warehouseMutationOrderDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseToId, warehouseMutationOrderDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForWarehouseMutationOrder(warehouseMutationOrderDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail UnfinishObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnfinishObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService))
            {
                _repository.UnfinishObject(warehouseMutationOrderDetail);

                // reverse mutate item in warehousefrom and warehouseto
                WarehouseMutationOrder warehouseMutationOrder = _warehouseMutationOrderService.GetObjectById(warehouseMutationOrderDetail.WarehouseMutationOrderId);
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseFromId, warehouseMutationOrderDetail.ItemId);
                WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseToId, warehouseMutationOrderDetail.ItemId);

                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForWarehouseMutationOrder(warehouseMutationOrderDetail, warehouseItemFrom, warehouseItemTo);
                foreach (var stockMutation in stockMutations)
                {
                    ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return warehouseMutationOrderDetail;
        }

        public void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Addition) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            _itemService.AdjustQuantity(item, Quantity);
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

        public void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Deduction) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            _itemService.AdjustQuantity(item, Quantity);
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

    }
}