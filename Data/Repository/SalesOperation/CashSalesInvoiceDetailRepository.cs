using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Data.Repository
{
    public class CashSalesInvoiceDetailRepository : EfRepository<CashSalesInvoiceDetail>, ICashSalesInvoiceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CashSalesInvoiceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashSalesInvoiceDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted).Include("Item").Include("CashSalesInvoice").Include(x => x.PriceMutation).Include(x => x.CashSalesReturnDetails);
        }

        public IList<CashSalesInvoiceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).Include("Item").Include("CashSalesInvoice").Include(x => x.PriceMutation).Include(x => x.CashSalesReturnDetails).ToList();
        }

        public IList<CashSalesInvoiceDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IQueryable<CashSalesInvoiceDetail> GetQueryableObjectsByCashSalesInvoiceId(int CashSalesInvoiceId)
        {
            return FindAll(x => x.CashSalesInvoiceId == CashSalesInvoiceId && !x.IsDeleted).Include("Item").Include("CashSalesInvoice").Include(x => x.PriceMutation).Include(x => x.CashSalesReturnDetails);
        }

        public IList<CashSalesInvoiceDetail> GetObjectsByCashSalesInvoiceId(int CashSalesInvoiceId)
        {
            return FindAll(x => x.CashSalesInvoiceId == CashSalesInvoiceId && !x.IsDeleted).Include("Item").Include("CashSalesInvoice").Include(x => x.PriceMutation).Include(x => x.CashSalesReturnDetails).ToList();
        }

        public CashSalesInvoiceDetail GetObjectById(int Id)
        {
            CashSalesInvoiceDetail cashSalesInvoiceDetail = FindAll(x => x.Id == Id && !x.IsDeleted).Include("Item").Include("CashSalesInvoice").Include(x => x.PriceMutation).Include(x => x.CashSalesReturnDetails).FirstOrDefault();
            if (cashSalesInvoiceDetail != null) { cashSalesInvoiceDetail.Errors = new Dictionary<string, string>(); }
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail CreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.CashSalesInvoices
                              where obj.Id == cashSalesInvoiceDetail.CashSalesInvoiceId
                              select obj.Code).FirstOrDefault();
            }
            if (cashSalesInvoiceDetail.Code == null || cashSalesInvoiceDetail.Code.Trim() == "")
            cashSalesInvoiceDetail.Code = SetObjectCode(ParentCode);
            cashSalesInvoiceDetail.IsDeleted = false;
            cashSalesInvoiceDetail.CreatedAt = DateTime.Now;
            return Create(cashSalesInvoiceDetail);
        }

        public CashSalesInvoiceDetail UpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            cashSalesInvoiceDetail.UpdatedAt = DateTime.Now;
            Update(cashSalesInvoiceDetail);
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail ConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            Update(cashSalesInvoiceDetail);
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail UnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            Update(cashSalesInvoiceDetail);
            return cashSalesInvoiceDetail;
        }

        public CashSalesInvoiceDetail SoftDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail)
        {
            cashSalesInvoiceDetail.IsDeleted = true;
            cashSalesInvoiceDetail.DeletedAt = DateTime.Now;
            Update(cashSalesInvoiceDetail);
            return cashSalesInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            CashSalesInvoiceDetail cashSalesInvoiceDetail = Find(x => x.Id == Id);
            return (Delete(cashSalesInvoiceDetail) == 1) ? true : false;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}
