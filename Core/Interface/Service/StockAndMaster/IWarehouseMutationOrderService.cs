using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IWarehouseMutationOrderService
    {
        IQueryable<WarehouseMutationOrder> GetQueryable();
        IWarehouseMutationOrderValidator GetValidator();
        IList<WarehouseMutationOrder> GetAll();
        WarehouseMutationOrder GetObjectById(int Id);
        Warehouse GetWarehouseFrom(WarehouseMutationOrder warehouseMutationOrder);
        Warehouse GetWarehouseTo(WarehouseMutationOrder warehouseMutationOrder);
        WarehouseMutationOrder CreateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService);
        WarehouseMutationOrder UpdateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService);
        WarehouseMutationOrder SoftDeleteObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService);
        bool DeleteObject(int Id);
        WarehouseMutationOrder ConfirmObject(WarehouseMutationOrder warehouseMutationOrder, DateTime ConfirmationDate, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                             IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
        WarehouseMutationOrder UnconfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
    }
}