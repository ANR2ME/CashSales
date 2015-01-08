using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PaymentRequestDetailRepository : EfRepository<PaymentRequestDetail>, IPaymentRequestDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PaymentRequestDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PaymentRequestDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<PaymentRequestDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PaymentRequestDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IList<PaymentRequestDetail> GetObjectsByPaymentRequestId(int paymentRequestId)
        {
            return FindAll(x => x.PaymentRequestId == paymentRequestId && !x.IsDeleted).ToList();
        }

        public IList<PaymentRequestDetail> GetNonLegacyObjectsByPaymentRequestId(int paymentRequestId)
        {
            return FindAll(x => x.PaymentRequestId == paymentRequestId && !x.IsLegacy && !x.IsDeleted).ToList();
        }

        public PaymentRequestDetail GetLegacyObjectByPaymentRequestId(int paymentRequestId)
        {
            PaymentRequestDetail detail = Find(x => x.PaymentRequestId == paymentRequestId && x.IsLegacy && !x.IsDeleted);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PaymentRequestDetail GetObjectById(int Id)
        {
            PaymentRequestDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PaymentRequestDetail CreateObject(PaymentRequestDetail paymentRequestDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PaymentRequests
                              where obj.Id == paymentRequestDetail.PaymentRequestId
                              select obj.Code).FirstOrDefault();
            }
            if (paymentRequestDetail.Code == null || paymentRequestDetail.Code.Trim() == "")
            paymentRequestDetail.Code = SetObjectCode(ParentCode);
            paymentRequestDetail.IsConfirmed = false;
            paymentRequestDetail.IsDeleted = false;
            paymentRequestDetail.CreatedAt = DateTime.Now;
            return Create(paymentRequestDetail);
        }

        public PaymentRequestDetail UpdateObject(PaymentRequestDetail paymentRequestDetail)
        {
            paymentRequestDetail.UpdatedAt = DateTime.Now;
            Update(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail paymentRequestDetail)
        {
            paymentRequestDetail.IsDeleted = true;
            paymentRequestDetail.DeletedAt = DateTime.Now;
            Update(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public bool DeleteObject(int Id)
        {
            PaymentRequestDetail paymentRequestDetail = Find(x => x.Id == Id);
            return (Delete(paymentRequestDetail) == 1) ? true : false;
        }

        public PaymentRequestDetail ConfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            paymentRequestDetail.IsConfirmed = true;
            Update(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail UnconfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            paymentRequestDetail.IsConfirmed = false;
            paymentRequestDetail.ConfirmationDate = null;
            UpdateObject(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}