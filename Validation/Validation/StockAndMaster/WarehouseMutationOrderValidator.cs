using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class WarehouseMutationOrderValidator : IWarehouseMutationOrderValidator
    {

        public WarehouseMutationOrder VHasDifferentWarehouse(WarehouseMutationOrder warehouseMutationOrder)
        {
            if (warehouseMutationOrder.WarehouseFromId == warehouseMutationOrder.WarehouseToId)
            {
                warehouseMutationOrder.Errors.Add("Generic", "Warehouse sebelum dan sesudah tidak boleh sama");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasWarehouseFrom(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            Warehouse warehouseFrom = _warehouseService.GetObjectById(warehouseMutationOrder.WarehouseFromId);
            if (warehouseFrom == null)
            {
                warehouseMutationOrder.Errors.Add("WarehouseFromId", "Tidak terasosiasi dengan warehouse");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasWarehouseTo(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            Warehouse warehouseTo = _warehouseService.GetObjectById(warehouseMutationOrder.WarehouseToId);
            if (warehouseTo == null)
            {
                warehouseMutationOrder.Errors.Add("WarehouseToId", "Tidak terasosiasi dengan warehouse");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasWarehouseMutationOrderDetails(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
            if (!details.Any())
            {
                warehouseMutationOrder.Errors.Add("Generic", "Harus memiliki WarehouseMutationOrder Details");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasMutationDate(WarehouseMutationOrder warehouseMutationOrder)
        {
            if (warehouseMutationOrder.MutationDate == null)
            {
                warehouseMutationOrder.Errors.Add("MutationDate", "Tidak boleh kosong");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasNotBeenConfirmed(WarehouseMutationOrder warehouseMutationOrder)
        {
            if (warehouseMutationOrder.IsConfirmed)
            {
                warehouseMutationOrder.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasBeenConfirmed(WarehouseMutationOrder warehouseMutationOrder)
        {
            if (!warehouseMutationOrder.IsConfirmed)
            {
                warehouseMutationOrder.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VDontHaveDetails(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
            if(details.Any())
            {
                warehouseMutationOrder.Errors.Add("Generic", "Tidak boleh memiliki WarehouseMutationOrder Details");
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VDetailsAreVerifiedConfirmable(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseMutationOrderDetail> details = _warehouseMutationOrderDetailService.GetObjectsByWarehouseMutationOrderId(warehouseMutationOrder.Id);
            foreach (var detail in details)
            {
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(warehouseMutationOrder.WarehouseFromId, detail.ItemId);
                if (warehouseItemFrom.Quantity < detail.Quantity)
                {
                    warehouseMutationOrder.Errors.Add("Generic", "Stock barang tidak boleh kurang dari stock yang akan dimutasikan");
                    return warehouseMutationOrder;
                }
            }
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VCreateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            VHasDifferentWarehouse(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasWarehouseFrom(warehouseMutationOrder, _warehouseService);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasWarehouseTo(warehouseMutationOrder, _warehouseService);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasMutationDate(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VUpdateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            VHasNotBeenConfirmed(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VCreateObject(warehouseMutationOrder, _warehouseService);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VDeleteObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService)
        {
            VHasNotBeenConfirmed(warehouseMutationOrder);
            VDontHaveDetails(warehouseMutationOrder, _warehouseMutationOrderDetailService);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VHasConfirmationDate(WarehouseMutationOrder obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public WarehouseMutationOrder VConfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasNotBeenConfirmed(warehouseMutationOrder);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VHasWarehouseMutationOrderDetails(warehouseMutationOrder, _warehouseMutationOrderDetailService);
            if (!isValid(warehouseMutationOrder)) { return warehouseMutationOrder; }
            VDetailsAreVerifiedConfirmable(warehouseMutationOrder, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder VUnconfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasBeenConfirmed(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public bool ValidCreateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            VCreateObject(warehouseMutationOrder, _warehouseService);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidUpdateObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseService _warehouseService)
        {
            warehouseMutationOrder.Errors.Clear();
            VUpdateObject(warehouseMutationOrder, _warehouseService);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidDeleteObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService)
        {
            warehouseMutationOrder.Errors.Clear();
            VDeleteObject(warehouseMutationOrder, _warehouseMutationOrderDetailService);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidConfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrder.Errors.Clear();
            VConfirmObject(warehouseMutationOrder, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(warehouseMutationOrder);
        }

        public bool ValidUnconfirmObject(WarehouseMutationOrder warehouseMutationOrder, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService,
                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            warehouseMutationOrder.Errors.Clear();
            VUnconfirmObject(warehouseMutationOrder, _warehouseMutationOrderService, _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(warehouseMutationOrder);
        }

        public bool isValid(WarehouseMutationOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WarehouseMutationOrder obj)
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