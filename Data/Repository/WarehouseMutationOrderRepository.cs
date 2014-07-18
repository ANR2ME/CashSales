﻿using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class WarehouseMutationOrderRepository : EfRepository<WarehouseMutationOrder>, IWarehouseMutationOrderRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public WarehouseMutationOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<WarehouseMutationOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public WarehouseMutationOrder GetObjectById(int Id)
        {
            WarehouseMutationOrder warehouseMutationOrder = Find(x => x.Id == Id && !x.IsDeleted);
            if (warehouseMutationOrder != null) { warehouseMutationOrder.Errors = new Dictionary<string, string>(); }
            return warehouseMutationOrder;
        }

        public Warehouse GetWarehouseFrom(WarehouseMutationOrder warehouseMutationOrder)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseFrom =
                    (from obj in db.Warehouses
                     where obj.Id == warehouseMutationOrder.WarehouseFromId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseFrom;
            }
        }

        public Warehouse GetWarehouseTo(WarehouseMutationOrder warehouseMutationOrder)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseTo =
                    (from obj in db.Warehouses
                     where obj.Id == warehouseMutationOrder.WarehouseToId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseTo;
            }
        }

        public WarehouseMutationOrder CreateObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            warehouseMutationOrder.IsDeleted = false;
            warehouseMutationOrder.CreatedAt = DateTime.Now;
            return Create(warehouseMutationOrder);
        }

        public WarehouseMutationOrder UpdateObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            warehouseMutationOrder.UpdatedAt = DateTime.Now;
            Update(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder SoftDeleteObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            warehouseMutationOrder.IsDeleted = true;
            warehouseMutationOrder.DeletedAt = DateTime.Now;
            Update(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder ConfirmObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            warehouseMutationOrder.IsConfirmed = true;
            warehouseMutationOrder.ConfirmationDate = DateTime.Now;
            Update(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder UnconfirmObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            warehouseMutationOrder.IsConfirmed = false;
            warehouseMutationOrder.ConfirmationDate = null;
            Update(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public WarehouseMutationOrder CompleteObject(WarehouseMutationOrder warehouseMutationOrder)
        {
            warehouseMutationOrder.IsCompleted = true;
            warehouseMutationOrder.UpdatedAt = DateTime.Now;
            Update(warehouseMutationOrder);
            return warehouseMutationOrder;
        }

        public bool DeleteObject(int Id)
        {
            WarehouseMutationOrder warehouseMutationOrder =  Find(x => x.Id == Id);
            return (Delete(warehouseMutationOrder) == 1) ? true : false;
        }
    }
}