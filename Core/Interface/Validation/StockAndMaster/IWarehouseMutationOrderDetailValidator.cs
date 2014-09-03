using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IWarehouseMutationOrderDetailValidator
    {
        WarehouseMutationOrderDetail VHasWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService);
        WarehouseMutationOrderDetail VHasWarehouseItemFrom(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail VHasWarehouseItemTo(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail VUniqueItem(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService);
        WarehouseMutationOrderDetail VNonNegativeStockQuantity(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                               IItemService _itemService, /*IBarringService _barringService,*/ IWarehouseItemService _warehouseItemService, bool CaseConfirmOrConfirm);
        WarehouseMutationOrderDetail VWarehouseMutationOrderHasBeenConfirmed(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService);
        WarehouseMutationOrderDetail VHasNotBeenConfirmed(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail VHasBeenConfirmed(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail VCreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                   IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail VUpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                   IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail VDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail VHasConfirmationDate(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail VConfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail VUnconfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                               IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                               IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidConfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        bool isValid(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        string PrintError(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
    }
}