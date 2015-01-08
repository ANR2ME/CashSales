using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class CustomPurchaseInvoiceDetailRepository : EfRepository<CustomPurchaseInvoiceDetail>, ICustomPurchaseInvoiceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CustomPurchaseInvoiceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CustomPurchaseInvoiceDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CustomPurchaseInvoiceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CustomPurchaseInvoiceDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IQueryable<CustomPurchaseInvoiceDetail> GetQueryableObjectsByCustomPurchaseInvoiceId(int CustomPurchaseInvoiceId)
        {
            return FindAll(x => x.CustomPurchaseInvoiceId == CustomPurchaseInvoiceId && !x.IsDeleted);
        }

        public IList<CustomPurchaseInvoiceDetail> GetObjectsByCustomPurchaseInvoiceId(int CustomPurchaseInvoiceId)
        {
            return FindAll(x => x.CustomPurchaseInvoiceId == CustomPurchaseInvoiceId && !x.IsDeleted).ToList();
        }

        public CustomPurchaseInvoiceDetail GetObjectById(int Id)
        {
            CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (customPurchaseInvoiceDetail != null) { customPurchaseInvoiceDetail.Errors = new Dictionary<string, string>(); }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail CreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.CustomPurchaseInvoices
                              where obj.Id == customPurchaseInvoiceDetail.CustomPurchaseInvoiceId
                              select obj.Code).FirstOrDefault();
            }
            if (customPurchaseInvoiceDetail.Code == null || customPurchaseInvoiceDetail.Code.Trim() == "")
            customPurchaseInvoiceDetail.Code = SetObjectCode(ParentCode);
            customPurchaseInvoiceDetail.IsDeleted = false;
            customPurchaseInvoiceDetail.CreatedAt = DateTime.Now;
            return Create(customPurchaseInvoiceDetail);
        }

        public CustomPurchaseInvoiceDetail UpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            customPurchaseInvoiceDetail.UpdatedAt = DateTime.Now;
            Update(customPurchaseInvoiceDetail);
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail ConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            Update(customPurchaseInvoiceDetail);
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail UnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            Update(customPurchaseInvoiceDetail);
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail SoftDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail)
        {
            customPurchaseInvoiceDetail.IsDeleted = true;
            customPurchaseInvoiceDetail.DeletedAt = DateTime.Now;
            Update(customPurchaseInvoiceDetail);
            return customPurchaseInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail = Find(x => x.Id == Id);
            return (Delete(customPurchaseInvoiceDetail) == 1) ? true : false;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}
