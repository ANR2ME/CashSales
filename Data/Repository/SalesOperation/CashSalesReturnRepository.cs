using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class CashSalesReturnRepository : EfRepository<CashSalesReturn>, ICashSalesReturnRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CashSalesReturnRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashSalesReturn> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CashSalesReturn> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CashSalesReturn> GetObjectsByCashSalesInvoiceId(int CashSalesInvoiceId)
        {
            return FindAll(x => x.CashSalesInvoiceId == CashSalesInvoiceId && !x.IsDeleted).ToList();
        }

        public IList<CashSalesReturn> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public CashSalesReturn GetObjectById(int Id)
        {
            CashSalesReturn cashSalesReturn = Find(x => x.Id == Id && !x.IsDeleted);
            if (cashSalesReturn != null) { cashSalesReturn.Errors = new Dictionary<string, string>(); }
            return cashSalesReturn;
        }

        public CashSalesReturn GetObjectByCashSalesInvoiceId(int CashSalesInvoiceId)
        {
            CashSalesReturn cashSalesReturn = Find(x => x.CashSalesInvoiceId == CashSalesInvoiceId && !x.IsDeleted);
            if (cashSalesReturn != null) { cashSalesReturn.Errors = new Dictionary<string, string>(); }
            return cashSalesReturn;
        }

        public CashSalesReturn CreateObject(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.Code == null || cashSalesReturn.Code.Trim() == "")
            cashSalesReturn.Code = SetObjectCode();
            cashSalesReturn.IsDeleted = false;
            cashSalesReturn.CreatedAt = DateTime.Now;
            return Create(cashSalesReturn);
        }

        public CashSalesReturn UpdateObject(CashSalesReturn cashSalesReturn)
        {
            cashSalesReturn.UpdatedAt = DateTime.Now;
            Update(cashSalesReturn);
            return cashSalesReturn;
        }

        public CashSalesReturn ConfirmObject(CashSalesReturn cashSalesReturn)
        {
            cashSalesReturn.IsConfirmed = true;
            Update(cashSalesReturn);
            return cashSalesReturn;
        }

        public CashSalesReturn UnconfirmObject(CashSalesReturn cashSalesReturn)
        {
            cashSalesReturn.IsConfirmed = false;
            cashSalesReturn.ConfirmationDate = null;
            Update(cashSalesReturn);
            return cashSalesReturn;
        }

        public CashSalesReturn PaidObject(CashSalesReturn cashSalesReturn)
        {
            cashSalesReturn.IsPaid = true;
            Update(cashSalesReturn);
            return cashSalesReturn;
        }

        public CashSalesReturn UnpaidObject(CashSalesReturn cashSalesReturn)
        {
            cashSalesReturn.IsPaid = false;
            Update(cashSalesReturn);
            return cashSalesReturn;
        }

        public CashSalesReturn SoftDeleteObject(CashSalesReturn cashSalesReturn)
        {
            cashSalesReturn.IsDeleted = true;
            cashSalesReturn.DeletedAt = DateTime.Now;
            Update(cashSalesReturn);
            return cashSalesReturn;
        }

        public bool DeleteObject(int Id)
        {
            CashSalesReturn cashSalesReturn = Find(x => x.Id == Id);
            return (Delete(cashSalesReturn) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}
