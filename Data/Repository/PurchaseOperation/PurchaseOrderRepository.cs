using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseOrderRepository : EfRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseOrder> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PurchaseOrder> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseOrder> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public PurchaseOrder GetObjectById(int Id)
        {
            PurchaseOrder purchaseOrder = Find(po => po.Id == Id && !po.IsDeleted);
            if (purchaseOrder != null) { purchaseOrder.Errors = new Dictionary<string, string>(); }
            return purchaseOrder;
        }

        public IList<PurchaseOrder> GetObjectsByContactId(int contactId)
        {
            return FindAll(po => po.ContactId == contactId && !po.IsDeleted).ToList();
        }

        public IQueryable<PurchaseOrder> GetQueryableConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted);
        }

        public IList<PurchaseOrder> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public PurchaseOrder CreateObject(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder.Code == null || purchaseOrder.Code.Trim() == "")
            purchaseOrder.Code = SetObjectCode();
            purchaseOrder.IsDeleted = false;
            purchaseOrder.IsConfirmed = false;
            purchaseOrder.CreatedAt = DateTime.Now;
            return Create(purchaseOrder);
        }

        public PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.UpdatedAt = DateTime.Now;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsDeleted = true;
            purchaseOrder.DeletedAt = DateTime.Now;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseOrder po = Find(x => x.Id == Id);
            return (Delete(po) == 1) ? true : false;
        }

        public PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsConfirmed = true;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsConfirmed = false;
            UpdateObject(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder SetReceivalComplete(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsReceivalCompleted = true;
            UpdateObject(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder UnsetReceivalComplete(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsReceivalCompleted = false;
            UpdateObject(purchaseOrder);
            return purchaseOrder;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}