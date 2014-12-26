using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class CashSalesInvoiceDetailValidator : ICashSalesInvoiceDetailValidator
    {
        public CashSalesInvoiceDetail VIsParentNotConfirmed(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService)
        {
            CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
            if (cashSalesInvoice != null)
            {
                if (cashSalesInvoice.IsConfirmed)
                {
                    cashSalesInvoiceDetail.Errors.Add("Generic", "CashSalesInvoice tidak boleh terkonfirmasi");
                }
            }
            else
            {
                cashSalesInvoiceDetail.Errors.Add("Generic", "CashSalesInvoice tidak ada");
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VHasCashSalesInvoice(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService)
        {
            CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
            if (cashSalesInvoice == null)
            {
                cashSalesInvoiceDetail.Errors.Add("CashSalesInvoiceId", "Tidak valid");
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VHasItem(CashSalesInvoiceDetail cashSalesInvoiceDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
            if (item == null)
            {
                cashSalesInvoiceDetail.Errors.Add("ItemId", "Tidak valid");
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VUniqueItem(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService)
        {
            IList<CashSalesInvoiceDetail> details = _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoiceDetail.CashSalesInvoiceId);
            foreach (var detail in details)
            {
                if (detail.ItemId == cashSalesInvoiceDetail.ItemId && detail.Id != cashSalesInvoiceDetail.Id)
                {
                    cashSalesInvoiceDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi item dalam 1 CashSalesInvoice");
                }
            }
            return cashSalesInvoiceDetail;
        }

        /*public CashSalesInvoiceDetail VHasQuantityPricing(CashSalesInvoiceDetail cashSalesInvoiceDetail, IItemService _itemService, IQuantityPricingService _quantityPricingService)
        {
            Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
            IList<QuantityPricing> quantityPricings = _quantityPricingService.GetObjectsByItemTypeId(item.ItemTypeId);
            if (quantityPricings.Any())
            {
                cashSalesInvoiceDetail.Errors.Add("Generic", "Tidak ada QuantityPricing terasosiasi");
            }
            return cashSalesInvoiceDetail;
        }*/

        public CashSalesInvoiceDetail VIsValidQuantityOrdered(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IItemService _itemService)
        {
            CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
            Item item = _itemService.GetObjectById(cashSalesInvoiceDetail.ItemId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(cashSalesInvoice.WarehouseId, cashSalesInvoiceDetail.ItemId);
            if (warehouseItem.Quantity - cashSalesInvoiceDetail.Quantity < 0)
            {
                cashSalesInvoiceDetail.Errors.Add("Generic", String.Format("Quantity Item(SKU:{0}) harus lebih kecil atau sama dengan WarehouseItem Quantity", item.Sku));
                return cashSalesInvoiceDetail;
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VIsValidQuantity(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesInvoiceDetail.CashSalesInvoiceId);
            //WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(cashSalesInvoice.WarehouseId, cashSalesInvoiceDetail.ItemId);
            if (cashSalesInvoiceDetail.Quantity <= 0 /*|| cashSalesInvoiceDetail.Quantity > warehouseItem.Quantity*/)
            {
                //cashSalesInvoiceDetail.Errors.Add("Quantity", "Quantity harus lebih besar dari 0 dan lebih kecil atau sama dengan WarehouseItem Quantity");
                cashSalesInvoiceDetail.Errors.Add("Quantity", "Quantity harus lebih besar dari 0");
                return cashSalesInvoiceDetail;
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VIsValidDiscount(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            if (cashSalesInvoiceDetail.Discount < 0 || cashSalesInvoiceDetail.Discount > 100)
            {
                cashSalesInvoiceDetail.Errors.Add("Discount", "Harus antara 0 sampai 100");
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VIsValidAssignedPrice(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            if (cashSalesInvoiceDetail.IsManualPriceAssignment && cashSalesInvoiceDetail.AssignedPrice < 0)
            {
                cashSalesInvoiceDetail.Errors.Add("AssignedPrice", "Harus lebih besar atau sama dengan 0");
            }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IItemService _itemService)
        {
            VIsValidQuantityOrdered(cashSalesInvoiceDetail, _cashSalesInvoiceService, _warehouseItemService, _itemService);
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VUnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VCreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, 
                                                      ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                                      IQuantityPricingService _quantityPricingService)
        {
            VIsParentNotConfirmed(cashSalesInvoiceDetail, _cashSalesInvoiceService);
            if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            VHasCashSalesInvoice(cashSalesInvoiceDetail, _cashSalesInvoiceService);
            if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            VIsValidQuantity(cashSalesInvoiceDetail, _cashSalesInvoiceService, _warehouseItemService);
            if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            VIsValidDiscount(cashSalesInvoiceDetail);
            if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            VIsValidAssignedPrice(cashSalesInvoiceDetail);
            if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            VHasItem(cashSalesInvoiceDetail, _itemService);
            if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            VUniqueItem(cashSalesInvoiceDetail, _cashSalesInvoiceDetailService, _itemService);
            //if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            //VConfirmObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, _warehouseItemService, _itemService);
            //if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            //VHasQuantityPricing(cashSalesInvoiceDetail, _itemService, _quantityPricingService);
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail VUpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                                      ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                                      IQuantityPricingService _quantityPricingService)
        {
            //VIsParentNotConfirmed(cashSalesInvoiceDetail, _cashSalesInvoiceService);
            //if (!isValid(cashSalesInvoiceDetail)) { return cashSalesInvoiceDetail; }
            return VCreateObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, _cashSalesInvoiceDetailService, _itemService, _warehouseItemService, _quantityPricingService);
        }

        public CashSalesInvoiceDetail VDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService)
        {
            VIsParentNotConfirmed(cashSalesInvoiceDetail, _cashSalesInvoiceService);
            return cashSalesInvoiceDetail;
        }

        public bool ValidCreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                      ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                      IQuantityPricingService _quantityPricingService)
        {
            VCreateObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, _cashSalesInvoiceDetailService, _itemService, _warehouseItemService, _quantityPricingService);
            return isValid(cashSalesInvoiceDetail);
        }

        public bool ValidConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IItemService _itemService)
        {
            cashSalesInvoiceDetail.Errors.Clear();
            VConfirmObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, _warehouseItemService, _itemService);
            return isValid(cashSalesInvoiceDetail);
        }

        public bool ValidUnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            cashSalesInvoiceDetail.Errors.Clear();
            VUnconfirmObject(cashSalesInvoiceDetail);
            return isValid(cashSalesInvoiceDetail);
        }

        public bool ValidUpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                      ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                      IQuantityPricingService _quantityPricingService)
        {
            cashSalesInvoiceDetail.Errors.Clear();
            VUpdateObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, _cashSalesInvoiceDetailService, _itemService, _warehouseItemService, _quantityPricingService);
            return isValid(cashSalesInvoiceDetail);
        }

        public bool ValidDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService)
        {
            cashSalesInvoiceDetail.Errors.Clear();
            VDeleteObject(cashSalesInvoiceDetail, _cashSalesInvoiceService);
            return isValid(cashSalesInvoiceDetail);
        }

        public bool isValid(CashSalesInvoiceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashSalesInvoiceDetail obj)
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
