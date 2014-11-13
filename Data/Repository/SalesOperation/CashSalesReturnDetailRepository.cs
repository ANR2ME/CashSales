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
    public class CashSalesReturnDetailRepository : EfRepository<CashSalesReturnDetail>, ICashSalesReturnDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CashSalesReturnDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashSalesReturnDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CashSalesReturnDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CashSalesReturnDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IQueryable<CashSalesReturnDetail> GetQueryableObjectsByCashSalesReturnId(int CashSalesReturnId)
        {
            return FindAll(x => x.CashSalesReturnId == CashSalesReturnId && !x.IsDeleted).Include("CashSalesReturn").Include("CashSalesInvoiceDetail");
        }

        public IQueryable<CashSalesReturnDetail> GetQueryableObjectsByCashSalesInvoiceDetailId(int CashSalesInvoiceDetailId)
        {
            return FindAll(x => x.CashSalesInvoiceDetailId == CashSalesInvoiceDetailId && !x.IsDeleted).Include("CashSalesReturn").Include("CashSalesInvoiceDetail");
        }

        public IList<CashSalesReturnDetail> GetObjectsByCashSalesReturnId(int CashSalesReturnId)
        {
            return FindAll(x => x.CashSalesReturnId == CashSalesReturnId && !x.IsDeleted).Include("CashSalesReturn").Include("CashSalesInvoiceDetail").ToList();
        }

        public CashSalesReturnDetail GetObjectById(int Id)
        {
            CashSalesReturnDetail cashSalesReturnDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (cashSalesReturnDetail != null) { cashSalesReturnDetail.Errors = new Dictionary<string, string>(); }
            return cashSalesReturnDetail;
        }

        public IList<CashSalesReturnDetail> GetObjectsByCashSalesInvoiceDetailId(int CashSalesInvoiceDetailId)
        {
            return FindAll(x => x.CashSalesInvoiceDetailId == CashSalesInvoiceDetailId && !x.IsDeleted).Include("CashSalesReturn").Include("CashSalesInvoiceDetail").ToList();
        }

        public CashSalesReturnDetail CreateObject(CashSalesReturnDetail cashSalesReturnDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.CashSalesReturns
                              where obj.Id == cashSalesReturnDetail.CashSalesReturnId
                              select obj.Code).FirstOrDefault();
            }
            if (cashSalesReturnDetail.Code == null || cashSalesReturnDetail.Code.Trim() == "")
            cashSalesReturnDetail.Code = SetObjectCode(ParentCode);
            cashSalesReturnDetail.IsDeleted = false;
            cashSalesReturnDetail.CreatedAt = DateTime.Now;
            return Create(cashSalesReturnDetail);
        }

        public CashSalesReturnDetail UpdateObject(CashSalesReturnDetail cashSalesReturnDetail)
        {
            cashSalesReturnDetail.UpdatedAt = DateTime.Now;
            Update(cashSalesReturnDetail);
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail ConfirmObject(CashSalesReturnDetail cashSalesReturnDetail)
        {
            Update(cashSalesReturnDetail);
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail UnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail)
        {
            Update(cashSalesReturnDetail);
            return cashSalesReturnDetail;
        }

        public CashSalesReturnDetail SoftDeleteObject(CashSalesReturnDetail cashSalesReturnDetail)
        {
            cashSalesReturnDetail.IsDeleted = true;
            cashSalesReturnDetail.DeletedAt = DateTime.Now;
            Update(cashSalesReturnDetail);
            return cashSalesReturnDetail;
        }

        public bool DeleteObject(int Id)
        {
            CashSalesReturnDetail cashSalesReturnDetail = Find(x => x.Id == Id);
            return (Delete(cashSalesReturnDetail) == 1) ? true : false;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}
