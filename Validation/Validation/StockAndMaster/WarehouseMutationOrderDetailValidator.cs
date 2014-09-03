using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class WarehouseMutationOrderDetailValidator : IWarehouseMutationOrderDetailValidator
    {
        public WarehouseMutationOrderDetail VHasWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService)
        {
            WarehouseMutationOrder warehouseMutationOrder = _warehouseMutationOrderService.GetObjectById(warehouseMutationOrderDetail.WarehouseMutationOrderId);
            if (warehouseMutationOrder == null)
            {
                warehouseMutationOrderDetail.Errors.Add("WarehouseMutationOrderId", "Tidak terasosiasi dengan Stock Adjustment");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VHasWarehouseItemFrom(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutationOrder warehouseMutationOrder = _warehouseMutationOrderService.GetObjectById(warehouseMutationOrderDetail.WarehouseMutationOrderId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseFromId, warehouseMutationOrderDetail.ItemId);
            if (warehouseItemFrom == null)
            {
                warehouseMutationOrderDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang sebelum");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VHasWarehouseItemTo(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutationOrder warehouseMutationOrder = _warehouseMutationOrderService.GetObjectById(warehouseMutationOrderDetail.WarehouseMutationOrderId);
            WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseToId, warehouseMutationOrderDetail.ItemId);
            if (warehouseItemTo == null)
            {
                warehouseMutationOrderDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang dituju");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VUniqueItem(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrderDetail.WarehouseMutationOrderId);
            foreach (var detail in details)
            {
                if (detail.ItemId == warehouseMutationOrderDetail.ItemId && detail.Id != warehouseMutationOrderDetail.Id)
                {
                     warehouseMutationOrderDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi item dalam 1 Stock Adjustment");
                }
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VNonNegativeNorZeroQuantity(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            if (warehouseMutationOrderDetail.Quantity <= 0)
            {
                warehouseMutationOrderDetail.Errors.Add("Quantity", "Tidak boleh negatif atau 0");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VWarehouseMutationOrderHasBeenConfirmed(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService)
        {
            WarehouseMutationOrder warehouseMutationOrder = _warehouseMutationOrderService.GetObjectById(warehouseMutationOrderDetail.WarehouseMutationOrderId);
            if (!warehouseMutationOrder.IsConfirmed)
            {
                warehouseMutationOrderDetail.Errors.Add("Generic", "WarehouseMutationOrder harus sudah dikonfirmasi");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VHasNotBeenConfirmed(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            if (warehouseMutationOrderDetail.IsConfirmed)
            {
                warehouseMutationOrderDetail.Errors.Add("IsConfirmed", "Tidak boleh sudah selesai.");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VHasBeenConfirmed(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            if (!warehouseMutationOrderDetail.IsConfirmed)
            {
                warehouseMutationOrderDetail.Errors.Add("IsConfirmed", "Harus sudah selesai.");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VNonNegativeStockQuantity(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                                      IItemService _itemService, /*IBarringService _barringService,*/ IWarehouseItemService _warehouseItemService, bool CaseConfirm)
        {
            WarehouseMutationOrder warehouseMutationOrder = _warehouseMutationOrderService.GetObjectById(warehouseMutationOrderDetail.WarehouseMutationOrderId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseFromId, warehouseMutationOrderDetail.ItemId);
            WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseToId, warehouseMutationOrderDetail.ItemId);
            int Quantity = CaseConfirm ? warehouseItemFrom.Quantity : warehouseItemTo.Quantity;
            if ( Quantity < warehouseMutationOrderDetail.Quantity)
            {
                warehouseMutationOrderDetail.Errors.Add("Quantity", "Stock barang tidak boleh kurang dari jumlah mutasi barang");
                return warehouseMutationOrderDetail;
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VCreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                          IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasWarehouseMutationOrder(warehouseMutationOrderDetail, _warehouseMutationOrderService);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VHasWarehouseItemFrom(warehouseMutationOrderDetail, _warehouseMutationOrderService, _warehouseItemService);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VHasWarehouseItemTo(warehouseMutationOrderDetail, _warehouseMutationOrderService, _warehouseItemService);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VNonNegativeNorZeroQuantity(warehouseMutationOrderDetail);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VUniqueItem(warehouseMutationOrderDetail, _warehouseMutationOrderDetailService, _itemService);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VNonNegativeStockQuantity(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, /*_barringService,*/ _warehouseItemService, true);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VUpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                          IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenConfirmed(warehouseMutationOrderDetail);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VCreateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _warehouseItemService);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            VHasNotBeenConfirmed(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VHasConfirmationDate(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            if (warehouseMutationOrderDetail.ConfirmationDate == null)
            {
                warehouseMutationOrderDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VConfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(warehouseMutationOrderDetail);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VNonNegativeStockQuantity(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, /*_barringService,*/ _warehouseItemService, true);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail VUnconfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VWarehouseMutationOrderHasBeenConfirmed(warehouseMutationOrderDetail, _warehouseMutationOrderService);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VHasBeenConfirmed(warehouseMutationOrderDetail);
            if (!isValid(warehouseMutationOrderDetail)) { return warehouseMutationOrderDetail; }
            VNonNegativeStockQuantity(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, /*_barringService,*/ _warehouseItemService, false);
            return warehouseMutationOrderDetail;
        }

        public bool ValidCreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                      IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _warehouseItemService);
            return isValid(warehouseMutationOrderDetail);
        }

        public bool ValidUpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                      IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrderDetail.Errors.Clear();
            VUpdateObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _warehouseItemService);
            return isValid(warehouseMutationOrderDetail);
        }

        public bool ValidDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.Errors.Clear();
            VDeleteObject(warehouseMutationOrderDetail);
            return isValid(warehouseMutationOrderDetail);
        }

        public bool ValidConfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrderDetail.Errors.Clear();
            VConfirmObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService);
            return isValid(warehouseMutationOrderDetail);
        }

        public bool ValidUnconfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                        IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrderDetail.Errors.Clear();
            VUnconfirmObject(warehouseMutationOrderDetail, _warehouseMutationOrderService, _itemService, _barringService, _warehouseItemService);
            return isValid(warehouseMutationOrderDetail);
        }

        public bool isValid(WarehouseMutationOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WarehouseMutationOrderDetail obj)
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