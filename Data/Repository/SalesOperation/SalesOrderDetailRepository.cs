using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesOrderDetailRepository : EfRepository<SalesOrderDetail>, ISalesOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesOrderDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalesOrderDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesOrderDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IQueryable<SalesOrderDetail> GetQueryableObjectsBySalesOrderId(int salesOrderId)
        {
            return FindAll(sod => sod.SalesOrderId == salesOrderId && !sod.IsDeleted);
        }

        public IList<SalesOrderDetail> GetObjectsBySalesOrderId(int salesOrderId)
        {
            return FindAll(sod => sod.SalesOrderId == salesOrderId && !sod.IsDeleted).ToList();
        }

        public IList<SalesOrderDetail> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId && !x.IsDeleted).ToList();
        }

        public SalesOrderDetail GetObjectById(int Id)
        {
            SalesOrderDetail detail = Find(sod => sod.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.SalesOrders
                              where obj.Id == salesOrderDetail.SalesOrderId
                              select obj.Code).FirstOrDefault();
            }
            if (salesOrderDetail.Code == null || salesOrderDetail.Code.Trim() == "")
            salesOrderDetail.Code = SetObjectCode(ParentCode);
            salesOrderDetail.IsConfirmed = false;
            salesOrderDetail.IsDeleted = false;
            salesOrderDetail.CreatedAt = DateTime.Now;
            salesOrderDetail.PendingDeliveryQuantity = salesOrderDetail.Quantity;
            return Create(salesOrderDetail);
        }

        public SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.UpdatedAt = DateTime.Now;
            Update(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.IsDeleted = true;
            salesOrderDetail.DeletedAt = DateTime.Now;
            Update(salesOrderDetail);
            return salesOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesOrderDetail sod = Find(x => x.Id == Id);
            return (Delete(sod) == 1) ? true : false;
        }

        public SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.IsConfirmed = true;
            Update(salesOrderDetail);
            return salesOrderDetail;
        }

        public SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail)
        {
            salesOrderDetail.IsConfirmed = false;
            salesOrderDetail.ConfirmationDate = null;
            UpdateObject(salesOrderDetail);
            return salesOrderDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}