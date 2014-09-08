using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class ItemValidator : IItemValidator
    {
        public Item VHasItemType(Item item, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
            if (itemType == null)
            {
                item.Errors.Add("ItemType", "Tidak boleh tidak ada");
            }
            return item;
        }

        public Item VHasItemTypeAndNotLegacyItem(Item item, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
            if (itemType == null)
            {
                item.Errors.Add("ItemType", "Tidak boleh tidak ada");
            }
            else if (itemType.IsLegacy)
            {
                item.Errors.Add("ItemType", "Tidak boleh memilih Legacy item type");
            }
            return item;
        }

        public Item VHasUniqueSku(Item item, IItemService _itemService)
        {
            if (String.IsNullOrEmpty(item.Sku) || item.Sku.Trim() == "")
            {
                item.Errors.Add("Sku", "Tidak boleh kosong");
            }
            if (_itemService.IsSkuDuplicated(item))
            {
                item.Errors.Add("Sku", "Tidak boleh diduplikasi");
            }
            return item;
        }

        public Item VHasName(Item item)
        {
            if (String.IsNullOrEmpty(item.Name) || item.Name.Trim() == "")
            {
                item.Errors.Add("Name", "Tidak boleh kosong");
            }
            return item;
        }

        /*public Item VHasCategory(Item item)
        {
            if (String.IsNullOrEmpty(item.Category) || item.Category.Trim() == "")
            {
                item.Errors.Add("Category", "Tidak boleh kosong");
            }
            return item;
        }*/

        public Item VHasUoM(Item item, IUoMService _uomService)
        {
            UoM uom = _uomService.GetObjectById(item.UoMId);
            if (uom == null)
            {
                item.Errors.Add("UoMId", "Tidak terasosiasi dengan Unit of Measurement");
            }
            return item;
        }

        public Item VNonNegativeQuantity(Item item)
        {
            if (item.Quantity < 0)
            {
                item.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            else if (item.PendingDelivery < 0)
            {
                item.Errors.Add("Generic", "Pending delivery tidak boleh negatif");
            }
            else if (item.PendingReceival < 0)
            {
                item.Errors.Add("Generic", "Pending receival tidak boleh negatif");
            }
            return item;
        }

        public Item VNonNegativePrice(Item item)
        {
            if (item.SellingPrice < 0)
            {
                item.Errors.Add("SellingPrice", "Tidak boleh negatif");
            }
            return item;
        }

        public Item VIsValidMargin(Item item)
        {
            if (item.Margin < 0 /*|| item.Margin > 100*/)
            {
                item.Errors.Add("Margin", "Harus lebih besar atau sama dengan 0");
            }
            return item;
        }

        public Item VWarehouseQuantityMustBeZero(Item item, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseItem> warehouseitems = _warehouseItemService.GetObjectsByItemId(item.Id);
            foreach (var warehouseitem in warehouseitems)
            {
                if (warehouseitem.Quantity > 0)
                {
                    item.Errors.Add("Generic", "quantity di semua warehouse harus 0");
                    return item;
                }
            }
            return item;
        }

        public Item VHasNoStockMutations(Item item, IStockMutationService _stockMutationService)
        {
            IList<StockMutation> stockMutations = _stockMutationService.GetObjectsByItemId(item.Id);
            if (stockMutations.Any())
            {
                item.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock mutation");
            }
            return item;
        }

        public Item VHasNoPurchaseOrderDetails(Item item, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByItemId(item.Id);
            if (purchaseOrderDetails.Any())
            {
                item.Errors.Add("Generic", "Tidak boleh terasosiasi dengan purchase order detail");
            }
            return item;
        }

        public Item VHasNoStockAdjustmentDetails(Item item, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByItemId(item.Id);
            if (stockAdjustmentDetails.Any())
            {
                item.Errors.Add("Generic", "Tidak boleh terasosiasi dengan stock adjustment detail");
            }
            return item;
        }

        public Item VHasNoSalesOrderDetails(Item item, ISalesOrderDetailService _salesOrderDetailService)
        {
            IList<SalesOrderDetail> salesOrderDetails = _salesOrderDetailService.GetObjectsByItemId(item.Id);
            if (salesOrderDetails.Any())
            {
                item.Errors.Add("Generic", "Tidak boleh terasosiasi dengan sales order detail");
            }
            return item;
        }

        public Item VCreateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VHasUoM(item, _uomService);
            if (!isValid(item)) { return item; }
            VHasItemTypeAndNotLegacyItem(item, _itemTypeService);
            if (!isValid(item)) { return item; }
            VHasUniqueSku(item, _itemService);
            if (!isValid(item)) { return item; }
            VHasName(item);
            //if (!isValid(item)) { return item; }
            //VHasCategory(item);
            if (!isValid(item)) { return item; }
            VNonNegativePrice(item);
            if (!isValid(item)) { return item; }
            VIsValidMargin(item);
            return item;
        }

        public Item VCreateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VHasUoM(item, _uomService);
            if (!isValid(item)) { return item; }
            VHasItemType(item, _itemTypeService);
            if (!isValid(item)) { return item; }
            VHasUniqueSku(item, _itemService);
            if (!isValid(item)) { return item; }
            VHasName(item);
            //if (!isValid(item)) { return item; }
            //VHasCategory(item);
            if (!isValid(item)) { return item; }
            VNonNegativePrice(item);
            if (!isValid(item)) { return item; }
            VIsValidMargin(item);
            return item;
        }

        public Item VUpdateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            return VCreateObject(item, _uomService, _itemService, _itemTypeService);
        }

        public Item VUpdateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            return VCreateLegacyObject(item, _uomService, _itemService, _itemTypeService);
        }

        public Item VDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                  IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            VHasItemTypeAndNotLegacyItem(item, _itemTypeService);
            if (!isValid(item)) { return item; }
            VHasNoStockMutations(item, _stockMutationService);
            if (!isValid(item)) { return item; }
            VWarehouseQuantityMustBeZero(item, _warehouseItemService);
            if (!isValid(item)) { return item; }
            VHasNoPurchaseOrderDetails(item, _purchaseOrderDetailService);
            if (!isValid(item)) { return item; }
            VHasNoStockAdjustmentDetails(item, _stockAdjustmentDetailService);
            if (!isValid(item)) { return item; }
            VHasNoSalesOrderDetails(item, _salesOrderDetailService);
            return item;
        }

        public Item VDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            VHasNoStockMutations(item, _stockMutationService);
            if (!isValid(item)) { return item; }
            VWarehouseQuantityMustBeZero(item, _warehouseItemService);
            if (!isValid(item)) { return item; }
            VHasNoPurchaseOrderDetails(item, _purchaseOrderDetailService);
            if (!isValid(item)) { return item; }
            VHasNoStockAdjustmentDetails(item, _stockAdjustmentDetailService);
            if (!isValid(item)) { return item; }
            VHasNoSalesOrderDetails(item, _salesOrderDetailService);
            return item;
        }

        public Item VAdjustQuantity(Item item)
        {
            VNonNegativeQuantity(item);
            return item;
        }

        public Item VAdjustPendingDelivery(Item item)
        {
            VNonNegativeQuantity(item);
            return item;
        }

        public Item VAdjustPendingReceival(Item item)
        {
            VNonNegativeQuantity(item);
            return item;
        }

        public bool ValidCreateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VCreateObject(item, _uomService, _itemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidCreateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VCreateLegacyObject(item, _uomService, _itemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidUpdateObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            item.Errors.Clear();
            VUpdateObject(item, _uomService, _itemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidUpdateLegacyObject(Item item, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            item.Errors.Clear();
            VUpdateLegacyObject(item, _uomService, _itemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            item.Errors.Clear();
            VDeleteObject(item, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService);
            return isValid(item);
        }

        public bool ValidDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            item.Errors.Clear();
            VDeleteLegacyObject(item, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService);
            return isValid(item);
        }

        public bool ValidAdjustQuantity(Item item)
        {
            item.Errors.Clear();
            VAdjustQuantity(item);
            return isValid(item);
        }

        public bool ValidAdjustPendingDelivery(Item item)
        {
            item.Errors.Clear();
            VAdjustPendingDelivery(item);
            return isValid(item);
        }

        public bool ValidAdjustPendingReceival(Item item)
        {
            item.Errors.Clear();
            VAdjustPendingReceival(item);
            return isValid(item);
        }

        public bool isValid(Item obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Item obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }
    }
}
