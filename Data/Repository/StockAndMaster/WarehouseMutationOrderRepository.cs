using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IQueryable<WarehouseMutationOrder> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<WarehouseMutationOrder> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<WarehouseMutationOrder> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
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
            if (warehouseMutationOrder.Code == null || warehouseMutationOrder.Code.Trim() == "")
            warehouseMutationOrder.Code = SetObjectCode();
            warehouseMutationOrder.IsConfirmed = false;
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

        public bool DeleteObject(int Id)
        {
            WarehouseMutationOrder warehouseMutationOrder =  Find(x => x.Id == Id);
            return (Delete(warehouseMutationOrder) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}