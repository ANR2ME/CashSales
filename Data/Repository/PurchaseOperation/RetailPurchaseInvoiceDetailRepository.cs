using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class RetailPurchaseInvoiceDetailRepository : EfRepository<RetailPurchaseInvoiceDetail>, IRetailPurchaseInvoiceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RetailPurchaseInvoiceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<RetailPurchaseInvoiceDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<RetailPurchaseInvoiceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<RetailPurchaseInvoiceDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<RetailPurchaseInvoiceDetail> GetObjectsByRetailPurchaseInvoiceId(int RetailPurchaseInvoiceId)
        {
            return FindAll(x => x.RetailPurchaseInvoiceId == RetailPurchaseInvoiceId && !x.IsDeleted).ToList();
        }

        public RetailPurchaseInvoiceDetail GetObjectById(int Id)
        {
            RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (retailPurchaseInvoiceDetail != null) { retailPurchaseInvoiceDetail.Errors = new Dictionary<string, string>(); }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail CreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.RetailPurchaseInvoices
                              where obj.Id == retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId
                              select obj.Code).FirstOrDefault();
            }
            if (retailPurchaseInvoiceDetail.Code == null || retailPurchaseInvoiceDetail.Code.Trim() == "")
            retailPurchaseInvoiceDetail.Code = SetObjectCode(ParentCode);
            retailPurchaseInvoiceDetail.IsDeleted = false;
            retailPurchaseInvoiceDetail.CreatedAt = DateTime.Now;
            return Create(retailPurchaseInvoiceDetail);
        }

        public RetailPurchaseInvoiceDetail UpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail)
        {
            retailPurchaseInvoiceDetail.UpdatedAt = DateTime.Now;
            Update(retailPurchaseInvoiceDetail);
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail ConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail)
        {
            Update(retailPurchaseInvoiceDetail);
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail UnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail)
        {
            Update(retailPurchaseInvoiceDetail);
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail SoftDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail)
        {
            retailPurchaseInvoiceDetail.IsDeleted = true;
            retailPurchaseInvoiceDetail.DeletedAt = DateTime.Now;
            Update(retailPurchaseInvoiceDetail);
            return retailPurchaseInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail = Find(x => x.Id == Id);
            return (Delete(retailPurchaseInvoiceDetail) == 1) ? true : false;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}
