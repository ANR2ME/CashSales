using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesInvoiceDetailRepository : EfRepository<SalesInvoiceDetail>, ISalesInvoiceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesInvoiceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesInvoiceDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalesInvoiceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesInvoiceDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IQueryable<SalesInvoiceDetail> GetQueryableObjectsBySalesInvoiceId(int salesInvoiceId)
        {
            return FindAll(pid => pid.SalesInvoiceId == salesInvoiceId && !pid.IsDeleted);
        }

        public IList<SalesInvoiceDetail> GetObjectsBySalesInvoiceId(int salesInvoiceId)
        {
            return FindAll(pid => pid.SalesInvoiceId == salesInvoiceId && !pid.IsDeleted).ToList();
        }

        public IQueryable<SalesInvoiceDetail> GetQueryableObjectsByDeliveryOrderDetailId(int deliveryOrderDetailId)
        {
            return FindAll(pid => pid.DeliveryOrderDetailId == deliveryOrderDetailId && !pid.IsDeleted);
        }

        public IList<SalesInvoiceDetail> GetObjectsByDeliveryOrderDetailId(int deliveryOrderDetailId)
        {
            return FindAll(pid => pid.DeliveryOrderDetailId == deliveryOrderDetailId && !pid.IsDeleted).ToList();
        }

        public SalesInvoiceDetail GetObjectById(int Id)
        {
            SalesInvoiceDetail detail = Find(pid => pid.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesInvoiceDetail CreateObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.SalesInvoices
                              where obj.Id == salesInvoiceDetail.SalesInvoiceId
                              select obj.Code).FirstOrDefault();
            }
            if (salesInvoiceDetail.Code == null || salesInvoiceDetail.Code.Trim() == "")
            salesInvoiceDetail.Code = SetObjectCode(ParentCode);
            salesInvoiceDetail.IsConfirmed = false;
            salesInvoiceDetail.IsDeleted = false;
            salesInvoiceDetail.CreatedAt = DateTime.Now;
            return Create(salesInvoiceDetail);
        }

        public SalesInvoiceDetail UpdateObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.UpdatedAt = DateTime.Now;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail SoftDeleteObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.IsDeleted = true;
            salesInvoiceDetail.DeletedAt = DateTime.Now;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesInvoiceDetail pid = Find(x => x.Id == Id);
            return (Delete(pid) == 1) ? true : false;
        }

        public SalesInvoiceDetail ConfirmObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.IsConfirmed = true;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail UnconfirmObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.IsConfirmed = false;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}