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
    public class RollerWarehouseMutationRepository : EfRepository<RollerWarehouseMutation>, IRollerWarehouseMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RollerWarehouseMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<RollerWarehouseMutation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<RollerWarehouseMutation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<RollerWarehouseMutation> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<RollerWarehouseMutation> GetObjectsByCoreIdentificationId(int coreIdentificationId)
        {
            return FindAll(x => x.CoreIdentificationId == coreIdentificationId && !x.IsDeleted).ToList();
        }

        public RollerWarehouseMutation GetObjectById(int Id)
        {
            RollerWarehouseMutation rollerWarehouseMutation = Find(x => x.Id == Id && !x.IsDeleted);
            if (rollerWarehouseMutation != null) { rollerWarehouseMutation.Errors = new Dictionary<string, string>(); }
            return rollerWarehouseMutation;
        }

        public Warehouse GetWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseFrom =
                    (from obj in db.Warehouses
                     where obj.Id == rollerWarehouseMutation.WarehouseFromId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseFrom;
            }
        }

        public Warehouse GetWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation)
        {
            using (var db = GetContext())
            {
                Warehouse warehouseTo =
                    (from obj in db.Warehouses
                     where obj.Id == rollerWarehouseMutation.WarehouseToId &&
                           !obj.IsDeleted
                     select obj).First();
                return warehouseTo;
            }
        }

        public RollerWarehouseMutation CreateObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            if (rollerWarehouseMutation.Code == null || rollerWarehouseMutation.Code.Trim() == "")
            rollerWarehouseMutation.Code = SetObjectCode();
            rollerWarehouseMutation.IsConfirmed = false;
            rollerWarehouseMutation.IsCompleted = false;
            rollerWarehouseMutation.IsDeleted = false;
            rollerWarehouseMutation.CreatedAt = DateTime.Now;
            return Create(rollerWarehouseMutation);
        }

        public RollerWarehouseMutation UpdateObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            rollerWarehouseMutation.UpdatedAt = DateTime.Now;
            Update(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation SoftDeleteObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            rollerWarehouseMutation.IsDeleted = true;
            rollerWarehouseMutation.DeletedAt = DateTime.Now;
            Update(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation ConfirmObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            rollerWarehouseMutation.IsConfirmed = true;
            Update(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation UnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            rollerWarehouseMutation.IsConfirmed = false;
            rollerWarehouseMutation.ConfirmationDate = null;
            Update(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation CompleteObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            rollerWarehouseMutation.IsCompleted = true;
            rollerWarehouseMutation.UpdatedAt = DateTime.Now;
            Update(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public bool DeleteObject(int Id)
        {
            RollerWarehouseMutation rollerWarehouseMutation =  Find(x => x.Id == Id);
            return (Delete(rollerWarehouseMutation) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}