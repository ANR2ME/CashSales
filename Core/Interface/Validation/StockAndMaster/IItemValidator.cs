using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IItemValidator
    {
        Item VHasItemType(Item item, IItemTypeService _itemTypeService);
        Item VHasItemTypeAndNotLegacyItem(Item item, IItemTypeService _itemTypeService);
        Item VHasUniqueSku(Item item, IItemService _itemService);
        Item VHasName(Item item);
        Item VHasCategory(Item item);
        Item VHasUoM(Item item, IUoMService _uomService);
        Item VNonNegativeQuantity(Item item);
        Item VIsValidMargin(Item item);
        Item VWarehouseQuantityMustBeZero(Item item, IWarehouseItemService _warehouseItemService);
        Item VHasNoStockMutations(Item item, IStockMutationService _stockMutationService);
        Item VHasNoPurchaseOrderDetails(Item item, IPurchaseOrderDetailService _purchaseOrderDetailService);
        Item VHasNoStockAdjustmentDetails(Item item, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        Item VHasNoSalesOrderDetails(Item item, ISalesOrderDetailService _salesOrderDetailService);
        
        Item VCreateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VCreateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VUpdateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VUpdateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService);
        Item VDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService);
        Item VAdjustQuantity(Item item);
        Item VAdjustPendingDelivery(Item item);
        Item VAdjustPendingReceival(Item item);

        bool ValidCreateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidCreateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService);
        bool ValidAdjustQuantity(Item item);
        bool ValidAdjustPendingDelivery(Item item);
        bool ValidAdjustPendingReceival(Item item);
        bool isValid(Item item);
        string PrintError(Item item);
    }
}