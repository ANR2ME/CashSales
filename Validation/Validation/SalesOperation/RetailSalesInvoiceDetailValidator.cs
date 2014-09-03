using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class RetailSalesInvoiceDetailValidator : IRetailSalesInvoiceDetailValidator
    {
        public RetailSalesInvoiceDetail VIsParentNotConfirmed(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService)
        {
            RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
            if (retailSalesInvoice != null)
            {
                if (retailSalesInvoice.IsConfirmed)
                {
                    retailSalesInvoiceDetail.Errors.Add("Generic", "RetailSalesInvoice tidak boleh terkonfirmasi");
                }
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VHasRetailSalesInvoice(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService)
        {
            RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
            if (retailSalesInvoice == null)
            {
                retailSalesInvoiceDetail.Errors.Add("RetailSalesInvoiceId", "Tidak valid");
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VHasItem(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(retailSalesInvoiceDetail.ItemId);
            if (item == null)
            {
                retailSalesInvoiceDetail.Errors.Add("ItemId", "Tidak valid");
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VUniqueItem(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService)
        {
            IList<RetailSalesInvoiceDetail> details = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoiceDetail.RetailSalesInvoiceId);
            foreach (var detail in details)
            {
                if (detail.ItemId == retailSalesInvoiceDetail.ItemId && detail.Id != retailSalesInvoiceDetail.Id)
                {
                    retailSalesInvoiceDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi item dalam 1 RetailSalesInvoice");
                }
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VIsValidQuantityOrdered(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(retailSalesInvoice.WarehouseId, retailSalesInvoiceDetail.ItemId);
            if (warehouseItem.Quantity - retailSalesInvoiceDetail.Quantity < 0)
            {
                retailSalesInvoiceDetail.Errors.Add("Generic", "Quantity harus lebih kecil atau sama dengan WarehouseItem Quantity");
                return retailSalesInvoiceDetail;
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VIsValidQuantity(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(retailSalesInvoice.WarehouseId, retailSalesInvoiceDetail.ItemId);
            if (retailSalesInvoiceDetail.Quantity <= 0 || retailSalesInvoiceDetail.Quantity > warehouseItem.Quantity)
            {
                retailSalesInvoiceDetail.Errors.Add("Quantity", "Harus lebih besar dari 0 dan lebih kecil atau sama dengan WarehouseItem Quantity");
                return retailSalesInvoiceDetail;
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VIsValidDiscount(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            if (retailSalesInvoiceDetail.Discount < 0 || retailSalesInvoiceDetail.Discount > 100)
            {
                retailSalesInvoiceDetail.Errors.Add("Discount", "Harus antara 0 sampai 100");
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VIsValidAssignedPrice(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            if (retailSalesInvoiceDetail.IsManualPriceAssignment && retailSalesInvoiceDetail.AssignedPrice < 0)
            {
                retailSalesInvoiceDetail.Errors.Add("AssignedPrice", "Harus lebih besar atau sama dengan 0");
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            VIsValidQuantityOrdered(retailSalesInvoiceDetail, _retailSalesInvoiceService, _warehouseItemService);
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VUnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VCreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, 
                                                      IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VIsParentNotConfirmed(retailSalesInvoiceDetail, _retailSalesInvoiceService);
            if (!isValid(retailSalesInvoiceDetail)) { return retailSalesInvoiceDetail; }
            VHasRetailSalesInvoice(retailSalesInvoiceDetail, _retailSalesInvoiceService);
            if (!isValid(retailSalesInvoiceDetail)) { return retailSalesInvoiceDetail; }
            VIsValidQuantity(retailSalesInvoiceDetail, _retailSalesInvoiceService, _warehouseItemService);
            if (!isValid(retailSalesInvoiceDetail)) { return retailSalesInvoiceDetail; }
            VIsValidDiscount(retailSalesInvoiceDetail);
            if (!isValid(retailSalesInvoiceDetail)) { return retailSalesInvoiceDetail; }
            VIsValidAssignedPrice(retailSalesInvoiceDetail);
            if (!isValid(retailSalesInvoiceDetail)) { return retailSalesInvoiceDetail; }
            VHasItem(retailSalesInvoiceDetail, _itemService);
            if (!isValid(retailSalesInvoiceDetail)) { return retailSalesInvoiceDetail; }
            VUniqueItem(retailSalesInvoiceDetail, _retailSalesInvoiceDetailService, _itemService);
            if (!isValid(retailSalesInvoiceDetail)) { return retailSalesInvoiceDetail; }
            VConfirmObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _warehouseItemService);
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail VUpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                                      IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            
            return VCreateObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _retailSalesInvoiceDetailService, _itemService, _warehouseItemService);
        }

        public RetailSalesInvoiceDetail VDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService)
        {
            VIsParentNotConfirmed(retailSalesInvoiceDetail, _retailSalesInvoiceService);
            return retailSalesInvoiceDetail;
        }

        public bool ValidCreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                      IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _retailSalesInvoiceDetailService, _itemService, _warehouseItemService);
            return isValid(retailSalesInvoiceDetail);
        }

        public bool ValidConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            retailSalesInvoiceDetail.Errors.Clear();
            VConfirmObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _warehouseItemService);
            return isValid(retailSalesInvoiceDetail);
        }

        public bool ValidUnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            retailSalesInvoiceDetail.Errors.Clear();
            VUnconfirmObject(retailSalesInvoiceDetail);
            return isValid(retailSalesInvoiceDetail);
        }

        public bool ValidUpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                      IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            retailSalesInvoiceDetail.Errors.Clear();
            VUpdateObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _retailSalesInvoiceDetailService, _itemService, _warehouseItemService);
            return isValid(retailSalesInvoiceDetail);
        }

        public bool ValidDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService)
        {
            retailSalesInvoiceDetail.Errors.Clear();
            VDeleteObject(retailSalesInvoiceDetail, _retailSalesInvoiceService);
            return isValid(retailSalesInvoiceDetail);
        }

        public bool isValid(RetailSalesInvoiceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RetailSalesInvoiceDetail obj)
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
