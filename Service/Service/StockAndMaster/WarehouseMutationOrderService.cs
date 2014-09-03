using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class WarehouseMutationOrderService : IWarehouseMutationOrderService
    {
        private IWarehouseMutationOrderRepository _repository;
        private IWarehouseMutationOrderValidator _validator;
        public WarehouseMutationOrderService(IWarehouseMutationOrderRepository _warehouseMutationOrderRepository, IWarehouseMutationOrderValidator _warehouseMutationOrderValidator)
        {
            _repository = _warehouseMutationOrderRepository;
            _validator = _warehouseMutationOrderValidator;
        }

        public IWarehouseMutationOrderValidator GetValidator()
        {
            return _validator;
        }

        public IWarehouseMutationOrderRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<WarehouseMutationOrder> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<WarehouseMutationOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public Warehouse GetWarehouseFrom(WarehouseMutationOrder warehouseMutationOrder)
        {
            return _repository.GetWarehouseFrom(warehouseMutationOrder);
        }

        public Warehouse GetWarehouseTo(WarehouseMutationOrder warehouseMutationOrder)
        {
            return _repository.GetWarehouseFrom(warehouseMutationOrder);
        }

        public WarehouseMutationOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WarehouseMutationOrder CreateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            warehouseMutationOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(warehouseMutationOrder, _warehouseService) ? _repository.CreateObject(warehouseMutationOrder) : warehouseMutationOrder);
        }

        public WarehouseMutationOrder UpdateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            return (warehouseMutationOrder = _validator.ValidUpdateObject(warehouseMutationOrder, _warehouseService) ? _repository.UpdateObject(warehouseMutationOrder) : warehouseMutationOrder);
        }

        public WarehouseMutationOrder SoftDeleteObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService)
        {
            return (warehouseMutationOrder = _validator.ValidDeleteObject(warehouseMutationOrder, _warehouseMutationOrderDetailService) ? _repository.SoftDeleteObject(warehouseMutationOrder) : warehouseMutationOrder);
        }

        public WarehouseMutationOrder ConfirmObject(WarehouseMutationOrder warehouseMutationOrder, DateTime ConfirmationDate, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService)
        {
            warehouseMutationOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(warehouseMutationOrder, this, _warehouseMutationOrderDetailService,
                                              _itemService, _barringService, _warehouseItemService))
            {
                IList<WarehouseMutationOrderDetail> warehouseMutationOrderDetails = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
                foreach (var detail in warehouseMutationOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _warehouseMutationOrderDetailService.ConfirmObject(detail, ConfirmationDate, this, _itemService, _barringService, _warehouseItemService, _stockMutationService);
                }
                _repository.ConfirmObject(warehouseMutationOrder);
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder UnconfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService,
                                                      IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(warehouseMutationOrder, this, _warehouseMutationOrderDetailService,
                                                _itemService, _barringService, _warehouseItemService))
            {
                IList<WarehouseMutationOrderDetail> warehouseMutationOrderDetails = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
                foreach (var detail in warehouseMutationOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _warehouseMutationOrderDetailService.UnconfirmObject(detail, this, _itemService, _barringService, _warehouseItemService, _stockMutationService);
                }
                _repository.UnconfirmObject(warehouseMutationOrder);
            }
            return warehouseMutationOrder;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}