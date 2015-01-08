using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseOrderDetailRepository : EfRepository<PurchaseOrderDetail>, IPurchaseOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseOrderDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PurchaseOrderDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseOrderDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IQueryable<PurchaseOrderDetail> GetQueryableObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return FindAll(pod => pod.PurchaseOrderId == purchaseOrderId && !pod.IsDeleted);
        }

        public IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return FindAll(pod => pod.PurchaseOrderId == purchaseOrderId && !pod.IsDeleted).ToList();
        }

        public IList<PurchaseOrderDetail> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId && !x.IsDeleted).ToList();
        }

        public PurchaseOrderDetail GetObjectById(int Id)
        {
            PurchaseOrderDetail detail = Find(pod => pod.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PurchaseOrders
                              where obj.Id == purchaseOrderDetail.PurchaseOrderId
                              select obj.Code).FirstOrDefault();
            }
            if (purchaseOrderDetail.Code == null || purchaseOrderDetail.Code.Trim() == "")
            purchaseOrderDetail.Code = SetObjectCode(ParentCode);
            purchaseOrderDetail.IsConfirmed = false;
            purchaseOrderDetail.IsDeleted = false;
            purchaseOrderDetail.CreatedAt = DateTime.Now;
            purchaseOrderDetail.PendingReceivalQuantity = purchaseOrderDetail.Quantity;
            return Create(purchaseOrderDetail);
        }

        public PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.UpdatedAt = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.IsDeleted = true;
            purchaseOrderDetail.DeletedAt = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseOrderDetail pod = Find(x => x.Id == Id);
            return (Delete(pod) == 1) ? true : false;
        }

        public PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.IsConfirmed = true;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.IsConfirmed = false;
            purchaseOrderDetail.ConfirmationDate = null;
            UpdateObject(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}