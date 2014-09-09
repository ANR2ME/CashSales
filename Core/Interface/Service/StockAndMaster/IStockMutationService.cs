using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IStockMutationService
    {
        IQueryable<StockMutation> GetQueryable();
         IStockMutationValidator GetValidator();
         IList<StockMutation> GetAll();
         IList<StockMutation> GetObjectsByItemId(int itemId);
         IList<StockMutation> GetObjectsByWarehouseId(int warehouseId);
         IList<StockMutation> GetObjectsByWarehouseItemId(int WarehouseId);
         StockMutation GetObjectById(int Id);
         IList<StockMutation> GetObjectsBySourceDocumentDetailForWarehouseItem(int warehouseItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         IList<StockMutation> GetObjectsBySourceDocumentDetailForItem(int ItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         StockMutation CreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBarringService _barringService);
         StockMutation UpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBarringService _barringService);
         StockMutation SoftDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBarringService _barringService);
         bool DeleteObject(int Id);
         StockMutation CreateStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item, IPurchaseOrderService _purchaseOrderService);
         IList<StockMutation> SoftDeleteStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item);
         IList<StockMutation> CreateStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         IList<StockMutation> SoftDeleteStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         IList<StockMutation> CreateStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem, IStockAdjustmentService _stockAdjustmentService);
         IList<StockMutation> SoftDeleteStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         StockMutation CreateStockMutationForRecoveryOrderCompound(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> SoftDeleteStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForBarringOrder(BarringOrderDetail barringOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> SoftDeleteStockMutationForBarringOrder(BarringOrderDetail barringOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> SoftDeleteStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> CreateStockMutationForWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo, IWarehouseMutationOrderService _warehouseMutationOrderService);
         IList<StockMutation> SoftDeleteStockMutationForWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
         void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
    }
}
