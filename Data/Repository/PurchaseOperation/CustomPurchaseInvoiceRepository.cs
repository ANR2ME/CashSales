using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class CustomPurchaseInvoiceRepository : EfRepository<CustomPurchaseInvoice>, ICustomPurchaseInvoiceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CustomPurchaseInvoiceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CustomPurchaseInvoice> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CustomPurchaseInvoice> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CustomPurchaseInvoice> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public CustomPurchaseInvoice GetObjectById(int Id)
        {
            CustomPurchaseInvoice customPurchaseInvoice = Find(x => x.Id == Id && !x.IsDeleted);
            if (customPurchaseInvoice != null) { customPurchaseInvoice.Errors = new Dictionary<string, string>(); }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice CreateObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.Code == null || customPurchaseInvoice.Code.Trim() == "")
            customPurchaseInvoice.Code = SetObjectCode();
            customPurchaseInvoice.IsDeleted = false;
            customPurchaseInvoice.CreatedAt = DateTime.Now;
            return Create(customPurchaseInvoice);
        }

        public CustomPurchaseInvoice UpdateObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            customPurchaseInvoice.UpdatedAt = DateTime.Now;
            Update(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice ConfirmObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            customPurchaseInvoice.IsConfirmed = true;
            Update(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice UnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            customPurchaseInvoice.IsConfirmed = false;
            customPurchaseInvoice.ConfirmationDate = null;
            Update(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice PaidObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            customPurchaseInvoice.IsPaid = true;
            Update(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice UnpaidObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            customPurchaseInvoice.IsPaid = false;
            Update(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice SoftDeleteObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            customPurchaseInvoice.IsDeleted = true;
            customPurchaseInvoice.DeletedAt = DateTime.Now;
            Update(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public bool DeleteObject(int Id)
        {
            CustomPurchaseInvoice customPurchaseInvoice = Find(x => x.Id == Id);
            return (Delete(customPurchaseInvoice) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }

    }
}
