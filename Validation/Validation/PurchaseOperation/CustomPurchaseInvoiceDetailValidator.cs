using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class CustomPurchaseInvoiceDetailValidator : ICustomPurchaseInvoiceDetailValidator
    {
        public CustomPurchaseInvoiceDetail VIsValidDiscount(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            if (customPurchaseInvoiceDetail.Discount < 0 || customPurchaseInvoiceDetail.Discount > 100)
            {
                customPurchaseInvoiceDetail.Errors.Add("Discount", "Harus antara 0 sampai 100");
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VIsValidListedUnitPrice(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            if (customPurchaseInvoiceDetail.ListedUnitPrice < 0)
            {
                customPurchaseInvoiceDetail.Errors.Add("ListedUnitPrice", "Harus lebih besar atau sama dengan 0");
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VIsParentNotConfirmed(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService)
        {
            CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
            if (customPurchaseInvoice != null)
            {
                if (customPurchaseInvoice.IsConfirmed)
                {
                    customPurchaseInvoiceDetail.Errors.Add("Generic", "CustomPurchaseInvoice tidak boleh terkonfirmasi");
                }
            }
            else
            {
                customPurchaseInvoiceDetail.Errors.Add("Generic", "CustomPurchaseInvoice tidak ada");
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VHasCustomPurchaseInvoice(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService)
        {
            CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
            if (customPurchaseInvoice == null)
            {
                customPurchaseInvoiceDetail.Errors.Add("CustomPurchaseInvoiceId", "Tidak valid");
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VHasItem(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
            if (item == null)
            {
                customPurchaseInvoiceDetail.Errors.Add("ItemId", "Tidak valid");
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VUniqueItem(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService)
        {
            IList<CustomPurchaseInvoiceDetail> details = _customPurchaseInvoiceDetailService.GetObjectsByCustomPurchaseInvoiceId(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
            foreach (var detail in details)
            {
                if (detail.ItemId == customPurchaseInvoiceDetail.ItemId && detail.Id != customPurchaseInvoiceDetail.Id)
                {
                    customPurchaseInvoiceDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi item dalam 1 CustomPurchaseInvoice");
                }
            }
            return customPurchaseInvoiceDetail;
        }

        /*public CustomPurchaseInvoiceDetail VIsValidQuantityOrdered(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(customPurchaseInvoice.WarehouseId, customPurchaseInvoiceDetail.ItemId);
            if (warehouseItem.Quantity - customPurchaseInvoiceDetail.Quantity < 0)
            {
                customPurchaseInvoiceDetail.Errors.Add("Generic", "Quantity harus lebih kecil atau sama dengan WarehouseItem Quantity");
                return customPurchaseInvoiceDetail;
            }
            return customPurchaseInvoiceDetail;
        }*/

        public CustomPurchaseInvoiceDetail VIsValidQuantity(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            if (customPurchaseInvoiceDetail.Quantity <= 0)
            {
                customPurchaseInvoiceDetail.Errors.Add("Quantity", "Quantity harus lebih besar dari 0");
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            //VIsValidQuantityOrdered(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _warehouseItemService);
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VUnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VCreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, 
                                                      ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VIsParentNotConfirmed(customPurchaseInvoiceDetail, _customPurchaseInvoiceService);
            if (!isValid(customPurchaseInvoiceDetail)) { return customPurchaseInvoiceDetail; }
            VHasItem(customPurchaseInvoiceDetail, _itemService);
            if (!isValid(customPurchaseInvoiceDetail)) { return customPurchaseInvoiceDetail; }
            VUniqueItem(customPurchaseInvoiceDetail, _customPurchaseInvoiceDetailService, _itemService);
            if (!isValid(customPurchaseInvoiceDetail)) { return customPurchaseInvoiceDetail; }
            VHasCustomPurchaseInvoice(customPurchaseInvoiceDetail, _customPurchaseInvoiceService);
            if (!isValid(customPurchaseInvoiceDetail)) { return customPurchaseInvoiceDetail; }
            VIsValidQuantity(customPurchaseInvoiceDetail);
            if (!isValid(customPurchaseInvoiceDetail)) { return customPurchaseInvoiceDetail; }
            VIsValidListedUnitPrice(customPurchaseInvoiceDetail);
            if (!isValid(customPurchaseInvoiceDetail)) { return customPurchaseInvoiceDetail; }
            VIsValidDiscount(customPurchaseInvoiceDetail);
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail VUpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                                      ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            //VIsNotConfirmed(customPurchaseInvoiceDetail, _customPurchaseInvoiceService);
            //if (!isValid(customPurchaseInvoiceDetail)) { return customPurchaseInvoiceDetail; }
            return VCreateObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _customPurchaseInvoiceDetailService, _itemService, _warehouseItemService);
        }

        public CustomPurchaseInvoiceDetail VDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService)
        {
            VIsParentNotConfirmed(customPurchaseInvoiceDetail, _customPurchaseInvoiceService);
            return customPurchaseInvoiceDetail;
        }

        public bool ValidCreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                      ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _customPurchaseInvoiceDetailService, _itemService, _warehouseItemService);
            return isValid(customPurchaseInvoiceDetail);
        }

        public bool ValidConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            customPurchaseInvoiceDetail.Errors.Clear();
            VConfirmObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _warehouseItemService);
            return isValid(customPurchaseInvoiceDetail);
        }

        public bool ValidUnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            customPurchaseInvoiceDetail.Errors.Clear();
            VUnconfirmObject(customPurchaseInvoiceDetail);
            return isValid(customPurchaseInvoiceDetail);
        }

        public bool ValidUpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                      ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            customPurchaseInvoiceDetail.Errors.Clear();
            VUpdateObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _customPurchaseInvoiceDetailService, _itemService, _warehouseItemService);
            return isValid(customPurchaseInvoiceDetail);
        }

        public bool ValidDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService)
        {
            customPurchaseInvoiceDetail.Errors.Clear();
            VDeleteObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService);
            return isValid(customPurchaseInvoiceDetail);
        }

        public bool isValid(CustomPurchaseInvoiceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CustomPurchaseInvoiceDetail obj)
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
