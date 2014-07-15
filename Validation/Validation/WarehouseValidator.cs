﻿using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class WarehouseValidator : IWarehouseValidator
    {
        public Warehouse VHasUniqueCode(Warehouse warehouse, IWarehouseService _warehouseService)
        {
            if (String.IsNullOrEmpty(warehouse.Code) || warehouse.Code.Trim() == "")
            {
                warehouse.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_warehouseService.IsCodeDuplicated(warehouse))
            {
                warehouse.Errors.Add("Code", "Tidak boleh diduplikasi");
            }
            return warehouse;
        }

        public Warehouse VWarehouseQuantityMustBeZero(Warehouse warehouse, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseItem> warehouseItems = _warehouseItemService.GetObjectsByWarehouseId(warehouse.Id);
            foreach (var warehouseItem in warehouseItems)
            {
                if (warehouseItem.Quantity > 0)
                {
                    warehouse.Errors.Add("Generic", "Item quantity di dalam warehouse harus 0");
                    return warehouse;
                }
            }
            return warehouse;
        }

        public Warehouse VIsInCoreIdentification(Warehouse warehouse, ICoreIdentificationService _coreIdentificationService)
        {
            IList<CoreIdentification> coreIdentifications = _coreIdentificationService.GetAllObjectsByWarehouseId(warehouse.Id);
            // TODO, after finishing Roller Warehouse Mutation
            return warehouse;
        }

        public Warehouse VIsInBarringOrderAndIncomplete(Warehouse warehouse, IBarringOrderService _barringOrderService)
        {
            // TODO, to discuss about stock mutation for barring
            return warehouse;
        }

        public Warehouse VCreateObject(Warehouse warehouse, IWarehouseService _warehouseService)
        {
            VHasUniqueCode(warehouse, _warehouseService);
            return warehouse;
        }

        public Warehouse VUpdateObject(Warehouse warehouse, IWarehouseService _warehouseService)
        {
            return VCreateObject(warehouse, _warehouseService);
        }

        public Warehouse VDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService, IBarringOrderService _barringOrderService)
        {
            VWarehouseQuantityMustBeZero(warehouse, _warehouseItemService);
            if (!isValid(warehouse)) { return warehouse; }
            VIsInCoreIdentification(warehouse, _coreIdentificationService);
            if (!isValid(warehouse)) { return warehouse; }
            VIsInBarringOrderAndIncomplete(warehouse, _barringOrderService);
            return warehouse;
        }

        public bool ValidCreateObject(Warehouse warehouse, IWarehouseService _warehouseService)
        {
            VCreateObject(warehouse, _warehouseService);
            return isValid(warehouse);
        }

        public bool ValidUpdateObject(Warehouse warehouse, IWarehouseService _warehouseService)
        {
            warehouse.Errors.Clear();
            VUpdateObject(warehouse, _warehouseService);
            return isValid(warehouse);
        }

        public bool ValidDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService, IBarringOrderService _barringOrderService)
        {
            warehouse.Errors.Clear();
            VDeleteObject(warehouse, _warehouseItemService, _coreIdentificationService, _barringOrderService);
            return isValid(warehouse);
        }
        
        public bool isValid(Warehouse obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Warehouse obj)
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
