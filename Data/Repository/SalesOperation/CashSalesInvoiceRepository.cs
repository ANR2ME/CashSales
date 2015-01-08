using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class CashSalesInvoiceRepository : EfRepository<CashSalesInvoice>, ICashSalesInvoiceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CashSalesInvoiceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashSalesInvoice> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CashSalesInvoice> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CashSalesInvoice> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public CashSalesInvoice GetObjectById(int Id)
        {
            CashSalesInvoice cashSalesInvoice = Find(x => x.Id == Id && !x.IsDeleted);
            if (cashSalesInvoice != null) { cashSalesInvoice.Errors = new Dictionary<string, string>(); }
            return cashSalesInvoice;
        }

        public CashSalesInvoice CreateObject(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.Code == null || cashSalesInvoice.Code.Trim() == "")
            cashSalesInvoice.Code = SetObjectCode();
            cashSalesInvoice.IsDeleted = false;
            cashSalesInvoice.CreatedAt = DateTime.Now;
            return Create(cashSalesInvoice);
        }

        public CashSalesInvoice UpdateObject(CashSalesInvoice cashSalesInvoice)
        {
            cashSalesInvoice.UpdatedAt = DateTime.Now;
            Update(cashSalesInvoice);
            return cashSalesInvoice;
        }

        public CashSalesInvoice ConfirmObject(CashSalesInvoice cashSalesInvoice)
        {
            cashSalesInvoice.IsConfirmed = true;
            Update(cashSalesInvoice);
            return cashSalesInvoice;
        }

        public CashSalesInvoice UnconfirmObject(CashSalesInvoice cashSalesInvoice)
        {
            cashSalesInvoice.IsConfirmed = false;
            cashSalesInvoice.ConfirmationDate = null;
            Update(cashSalesInvoice);
            return cashSalesInvoice;
        }

        public CashSalesInvoice PaidObject(CashSalesInvoice cashSalesInvoice)
        {
            cashSalesInvoice.IsPaid = true;
            Update(cashSalesInvoice);
            return cashSalesInvoice;
        }

        public CashSalesInvoice UnpaidObject(CashSalesInvoice cashSalesInvoice)
        {
            cashSalesInvoice.IsPaid = false;
            Update(cashSalesInvoice);
            return cashSalesInvoice;
        }

        public CashSalesInvoice SoftDeleteObject(CashSalesInvoice cashSalesInvoice)
        {
            cashSalesInvoice.IsDeleted = true;
            cashSalesInvoice.DeletedAt = DateTime.Now;
            Update(cashSalesInvoice);
            return cashSalesInvoice;
        }

        public bool DeleteObject(int Id)
        {
            CashSalesInvoice cashSalesInvoice = Find(x => x.Id == Id);
            return (Delete(cashSalesInvoice) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}
