using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class RetailPurchaseInvoiceRepository : EfRepository<RetailPurchaseInvoice>, IRetailPurchaseInvoiceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RetailPurchaseInvoiceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<RetailPurchaseInvoice> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<RetailPurchaseInvoice> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<RetailPurchaseInvoice> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public RetailPurchaseInvoice GetObjectById(int Id)
        {
            RetailPurchaseInvoice retailPurchaseInvoice = Find(x => x.Id == Id && !x.IsDeleted);
            if (retailPurchaseInvoice != null) { retailPurchaseInvoice.Errors = new Dictionary<string, string>(); }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice CreateObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.Code == null || retailPurchaseInvoice.Code.Trim() == "")
            retailPurchaseInvoice.Code = SetObjectCode();
            retailPurchaseInvoice.IsDeleted = false;
            retailPurchaseInvoice.CreatedAt = DateTime.Now;
            return Create(retailPurchaseInvoice);
        }

        public RetailPurchaseInvoice UpdateObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            retailPurchaseInvoice.UpdatedAt = DateTime.Now;
            Update(retailPurchaseInvoice);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice ConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            retailPurchaseInvoice.IsConfirmed = true;
            Update(retailPurchaseInvoice);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice UnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            retailPurchaseInvoice.IsConfirmed = false;
            retailPurchaseInvoice.ConfirmationDate = null;
            Update(retailPurchaseInvoice);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice PaidObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            retailPurchaseInvoice.IsPaid = true;
            Update(retailPurchaseInvoice);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice UnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            retailPurchaseInvoice.IsPaid = false;
            Update(retailPurchaseInvoice);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice SoftDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            retailPurchaseInvoice.IsDeleted = true;
            retailPurchaseInvoice.DeletedAt = DateTime.Now;
            Update(retailPurchaseInvoice);
            return retailPurchaseInvoice;
        }

        public bool DeleteObject(int Id)
        {
            RetailPurchaseInvoice retailPurchaseInvoice = Find(x => x.Id == Id);
            return (Delete(retailPurchaseInvoice) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }

    }
}
