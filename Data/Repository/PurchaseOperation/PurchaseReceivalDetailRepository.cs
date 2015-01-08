using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseReceivalDetailRepository : EfRepository<PurchaseReceivalDetail>, IPurchaseReceivalDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseReceivalDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseReceivalDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PurchaseReceivalDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseReceivalDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IQueryable<PurchaseReceivalDetail> GetQueryableObjectsByPurchaseReceivalId(int purchaseReceivalId)
        {
            return FindAll(prd => prd.PurchaseReceivalId == purchaseReceivalId && !prd.IsDeleted);
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int purchaseReceivalId)
        {
            return FindAll(prd => prd.PurchaseReceivalId == purchaseReceivalId && !prd.IsDeleted).ToList();
        }

        public PurchaseReceivalDetail GetObjectById(int Id)
        {
            PurchaseReceivalDetail purchaseReceivalDetail = Find(prd => prd.Id == Id && !prd.IsDeleted);
            if (purchaseReceivalDetail != null) { purchaseReceivalDetail.Errors = new Dictionary<string, string>(); }
            return purchaseReceivalDetail;
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseOrderDetailId(int purchaseOrderDetailId)
        {
            return FindAll(prd => prd.PurchaseOrderDetailId == purchaseOrderDetailId && !prd.IsDeleted).ToList();
        }

        public PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PurchaseReceivals
                              where obj.Id == purchaseReceivalDetail.PurchaseReceivalId
                              select obj.Code).FirstOrDefault();
            }
            if (purchaseReceivalDetail.Code == null || purchaseReceivalDetail.Code.Trim() == "")
            purchaseReceivalDetail.Code = SetObjectCode(ParentCode);
            purchaseReceivalDetail.IsConfirmed = false;
            purchaseReceivalDetail.IsDeleted = false;
            purchaseReceivalDetail.IsAllInvoiced = false;
            purchaseReceivalDetail.PendingInvoicedQuantity = purchaseReceivalDetail.Quantity;
            purchaseReceivalDetail.CreatedAt = DateTime.Now;
            return Create(purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.UpdatedAt = DateTime.Now;
            Update(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.IsDeleted = true;
            purchaseReceivalDetail.DeletedAt = DateTime.Now;
            Update(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseReceivalDetail prd = Find(x => x.Id == Id);
            return (Delete(prd) == 1) ? true : false;
        }

        public PurchaseReceivalDetail ConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.IsConfirmed = true;
            Update(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail UnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            purchaseReceivalDetail.IsConfirmed = false;
            UpdateObject(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}