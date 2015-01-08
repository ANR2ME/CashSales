using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseReceivalRepository : EfRepository<PurchaseReceival>, IPurchaseReceivalRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseReceivalRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseReceival> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PurchaseReceival> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseReceival> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public PurchaseReceival GetObjectById(int Id)
        {
            PurchaseReceival purchaseReceival = Find(x => x.Id == Id && !x.IsDeleted);
            if (purchaseReceival != null) { purchaseReceival.Errors = new Dictionary<string, string>(); }
            return purchaseReceival;
        }

        public IList<PurchaseReceival> GetObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return FindAll(x => x.PurchaseOrderId == purchaseOrderId && !x.IsDeleted).ToList();
        }

        public IQueryable<PurchaseReceival> GetQueryableConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted);
        }

        public IList<PurchaseReceival> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public PurchaseReceival CreateObject(PurchaseReceival purchaseReceival)
        {
            if (purchaseReceival.Code == null || purchaseReceival.Code.Trim() == "")
            purchaseReceival.Code = SetObjectCode();
            purchaseReceival.IsDeleted = false;
            purchaseReceival.IsConfirmed = false;
            purchaseReceival.IsInvoiceCompleted = false;
            purchaseReceival.CreatedAt = DateTime.Now;
            return Create(purchaseReceival);
        }

        public PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.UpdatedAt = DateTime.Now;
            Update(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.IsDeleted = true;
            purchaseReceival.DeletedAt = DateTime.Now;
            Update(purchaseReceival);
            return purchaseReceival;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseReceival pr = Find(x => x.Id == Id);
            return (Delete(pr) == 1) ? true : false;
        }

        public PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.IsConfirmed = true;
            Update(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.IsConfirmed = false;
            purchaseReceival.ConfirmationDate = null;
            UpdateObject(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival SetInvoiceComplete(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.IsInvoiceCompleted = true;
            UpdateObject(purchaseReceival);
            return purchaseReceival;
        }

        public PurchaseReceival UnsetInvoiceComplete(PurchaseReceival purchaseReceival)
        {
            purchaseReceival.IsInvoiceCompleted = false;
            UpdateObject(purchaseReceival);
            return purchaseReceival;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}