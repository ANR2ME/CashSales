using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesInvoiceRepository : EfRepository<SalesInvoice>, ISalesInvoiceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesInvoiceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesInvoice> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalesInvoice> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesInvoice> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public SalesInvoice GetObjectById(int Id)
        {
            SalesInvoice salesInvoice = Find(pi => pi.Id == Id && !pi.IsDeleted);
            if (salesInvoice != null) { salesInvoice.Errors = new Dictionary<string, string>(); }
            return salesInvoice;
        }

        public IList<SalesInvoice> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return FindAll(pi => pi.DeliveryOrderId == deliveryOrderId && !pi.IsDeleted).ToList();
        }

        public SalesInvoice CreateObject(SalesInvoice salesInvoice)
        {
            if (salesInvoice.Code == null || salesInvoice.Code.Trim() == "")
            salesInvoice.Code = SetObjectCode();
            salesInvoice.AmountReceivable = 0;
            salesInvoice.IsDeleted = false;
            salesInvoice.IsConfirmed = false;
            salesInvoice.CreatedAt = DateTime.Now;
            return Create(salesInvoice);
        }

        public SalesInvoice UpdateObject(SalesInvoice salesInvoice)
        {
            salesInvoice.UpdatedAt = DateTime.Now;
            Update(salesInvoice);
            return salesInvoice;
        }

        public SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice)
        {
            salesInvoice.IsDeleted = true;
            salesInvoice.DeletedAt = DateTime.Now;
            Update(salesInvoice);
            return salesInvoice;
        }

        public bool DeleteObject(int Id)
        {
            SalesInvoice pi = Find(x => x.Id == Id);
            return (Delete(pi) == 1) ? true : false;
        }

        public SalesInvoice ConfirmObject(SalesInvoice salesInvoice)
        {
            salesInvoice.IsConfirmed = true;
            Update(salesInvoice);
            return salesInvoice;
        }

        public SalesInvoice UnconfirmObject(SalesInvoice salesInvoice)
        {
            salesInvoice.IsConfirmed = false;
            salesInvoice.ConfirmationDate = null;
            UpdateObject(salesInvoice);
            return salesInvoice;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}