using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class RetailPurchaseInvoiceDetailValidator : IRetailPurchaseInvoiceDetailValidator
    {
        public RetailPurchaseInvoiceDetail VIsParentNotConfirmed(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService)
        {
            RetailPurchaseInvoice retailPurchaseInvoice = _retailPurchaseInvoiceService.GetObjectById(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
            if (retailPurchaseInvoice != null)
            {
                if (retailPurchaseInvoice.IsConfirmed)
                {
                    retailPurchaseInvoiceDetail.Errors.Add("Generic", "RetailPurchaseInvoice tidak boleh terkonfirmasi");
                }
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VHasRetailPurchaseInvoice(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService)
        {
            RetailPurchaseInvoice retailPurchaseInvoice = _retailPurchaseInvoiceService.GetObjectById(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
            if (retailPurchaseInvoice == null)
            {
                retailPurchaseInvoiceDetail.Errors.Add("RetailPurchaseInvoiceId", "Tidak valid");
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VHasItem(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(retailPurchaseInvoiceDetail.ItemId);
            if (item == null)
            {
                retailPurchaseInvoiceDetail.Errors.Add("ItemId", "Tidak valid");
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VUniqueItem(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService)
        {
            IList<RetailPurchaseInvoiceDetail> details = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
            foreach (var detail in details)
            {
                if (detail.ItemId == retailPurchaseInvoiceDetail.ItemId && detail.Id != retailPurchaseInvoiceDetail.Id)
                {
                    retailPurchaseInvoiceDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi item dalam 1 RetailPurchaseInvoice");
                }
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VIsValidQuantityOrdered(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            RetailPurchaseInvoice retailPurchaseInvoice = _retailPurchaseInvoiceService.GetObjectById(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(retailPurchaseInvoice.WarehouseId, retailPurchaseInvoiceDetail.ItemId);
            if (warehouseItem.Quantity - retailPurchaseInvoiceDetail.Quantity < 0)
            {
                retailPurchaseInvoiceDetail.Errors.Add("Generic", "Quantity harus lebih kecil atau sama dengan WarehouseItem Quantity");
                return retailPurchaseInvoiceDetail;
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VIsValidQuantity(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            RetailPurchaseInvoice retailPurchaseInvoice = _retailPurchaseInvoiceService.GetObjectById(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(retailPurchaseInvoice.WarehouseId, retailPurchaseInvoiceDetail.ItemId);
            if (retailPurchaseInvoiceDetail.Quantity <= 0 || retailPurchaseInvoiceDetail.Quantity >= warehouseItem.Quantity)
            {
                retailPurchaseInvoiceDetail.Errors.Add("Quantity", "Quantity harus lebih besar dari 0 dan lebih kecil dari WarehouseItem Quantity");
                return retailPurchaseInvoiceDetail;
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            VIsValidQuantityOrdered(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _warehouseItemService);
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VUnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail)
        {
            
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VCreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, 
                                                      IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VIsParentNotConfirmed(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService);
            if (!isValid(retailPurchaseInvoiceDetail)) { return retailPurchaseInvoiceDetail; }
            VHasRetailPurchaseInvoice(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService);
            if (!isValid(retailPurchaseInvoiceDetail)) { return retailPurchaseInvoiceDetail; }
            VIsValidQuantity(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _warehouseItemService);
            if (!isValid(retailPurchaseInvoiceDetail)) { return retailPurchaseInvoiceDetail; }
            VHasItem(retailPurchaseInvoiceDetail, _itemService);
            if (!isValid(retailPurchaseInvoiceDetail)) { return retailPurchaseInvoiceDetail; }
            VUniqueItem(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceDetailService, _itemService);
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail VUpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                                      IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return VCreateObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _retailPurchaseInvoiceDetailService, _itemService, _warehouseItemService);
        }

        public RetailPurchaseInvoiceDetail VDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService)
        {
            VIsParentNotConfirmed(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService);
            return retailPurchaseInvoiceDetail;
        }

        public bool ValidCreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                      IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _retailPurchaseInvoiceDetailService, _itemService, _warehouseItemService);
            return isValid(retailPurchaseInvoiceDetail);
        }

        public bool ValidConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            retailPurchaseInvoiceDetail.Errors.Clear();
            VConfirmObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _warehouseItemService);
            return isValid(retailPurchaseInvoiceDetail);
        }

        public bool ValidUnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail)
        {
            retailPurchaseInvoiceDetail.Errors.Clear();
            VUnconfirmObject(retailPurchaseInvoiceDetail);
            return isValid(retailPurchaseInvoiceDetail);
        }

        public bool ValidUpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                      IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            retailPurchaseInvoiceDetail.Errors.Clear();
            VUpdateObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _retailPurchaseInvoiceDetailService, _itemService, _warehouseItemService);
            return isValid(retailPurchaseInvoiceDetail);
        }

        public bool ValidDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService)
        {
            retailPurchaseInvoiceDetail.Errors.Clear();
            VDeleteObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService);
            return isValid(retailPurchaseInvoiceDetail);
        }

        public bool isValid(RetailPurchaseInvoiceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RetailPurchaseInvoiceDetail obj)
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
