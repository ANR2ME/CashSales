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
    public class WarehouseMutationOrderDetailRepository : EfRepository<WarehouseMutationOrderDetail>, IWarehouseMutationOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public WarehouseMutationOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<WarehouseMutationOrderDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<WarehouseMutationOrderDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<WarehouseMutationOrderDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IQueryable<WarehouseMutationOrderDetail> GetQueryableObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId)
        {
            return FindAll(x => x.WarehouseMutationOrderId == warehouseMutationOrderId && !x.IsDeleted);
        }

        public IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId)
        {
            return FindAll(x => x.WarehouseMutationOrderId == warehouseMutationOrderId && !x.IsDeleted).ToList();
        }

        public WarehouseMutationOrderDetail GetObjectById(int Id)
        {
            WarehouseMutationOrderDetail warehouseMutationOrderDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (warehouseMutationOrderDetail != null) { warehouseMutationOrderDetail.Errors = new Dictionary<string, string>(); }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.WarehouseMutationOrders
                              where obj.Id == warehouseMutationOrderDetail.WarehouseMutationOrderId
                              select obj.Code).FirstOrDefault();
            }
            if (warehouseMutationOrderDetail.Code == null || warehouseMutationOrderDetail.Code.Trim() == "")
            warehouseMutationOrderDetail.Code = SetObjectCode(ParentCode);
            warehouseMutationOrderDetail.IsConfirmed = false;
            warehouseMutationOrderDetail.IsDeleted = false;
            warehouseMutationOrderDetail.CreatedAt = DateTime.Now;
            return Create(warehouseMutationOrderDetail);
        }

        public WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.UpdatedAt = DateTime.Now;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.IsDeleted = true;
            warehouseMutationOrderDetail.DeletedAt = DateTime.Now;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            WarehouseMutationOrderDetail warehouseMutationOrderDetail =  Find(x => x.Id == Id);
            return (Delete(warehouseMutationOrderDetail) == 1) ? true : false;
        }

        public WarehouseMutationOrderDetail ConfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.IsConfirmed = true;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail UnconfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.IsConfirmed = false;
            warehouseMutationOrderDetail.ConfirmationDate = null;
            warehouseMutationOrderDetail.UpdatedAt = DateTime.Now;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + totalnumberinthemonth;
            return Code;
        } 
    }
}