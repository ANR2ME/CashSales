using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PaymentRequestRepository : EfRepository<PaymentRequest>, IPaymentRequestRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PaymentRequestRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PaymentRequest> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PaymentRequest> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PaymentRequest> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public PaymentRequest GetObjectById(int Id)
        {
            PaymentRequest paymentRequest = Find(x => x.Id == Id && !x.IsDeleted);
            if (paymentRequest != null) { paymentRequest.Errors = new Dictionary<string, string>(); }
            return paymentRequest;
        }

        public IList<PaymentRequest> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public PaymentRequest CreateObject(PaymentRequest paymentRequest)
        {
            paymentRequest.Code = SetObjectCode();
            paymentRequest.IsDeleted = false;
            paymentRequest.IsConfirmed = false;
            paymentRequest.CreatedAt = DateTime.Now;
            return Create(paymentRequest);
        }

        public PaymentRequest UpdateObject(PaymentRequest paymentRequest)
        {
            paymentRequest.UpdatedAt = DateTime.Now;
            Update(paymentRequest);
            return paymentRequest;
        }

        public PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest)
        {
            paymentRequest.IsDeleted = true;
            paymentRequest.DeletedAt = DateTime.Now;
            Update(paymentRequest);
            return paymentRequest;
        }

        public bool DeleteObject(int Id)
        {
            PaymentRequest paymentRequest = Find(x => x.Id == Id);
            return (Delete(paymentRequest) == 1) ? true : false;
        }

        public PaymentRequest ConfirmObject(PaymentRequest paymentRequest)
        {
            paymentRequest.IsConfirmed = true;
            Update(paymentRequest);
            return paymentRequest;
        }

        public PaymentRequest UnconfirmObject(PaymentRequest paymentRequest)
        {
            paymentRequest.IsConfirmed = false;
            paymentRequest.ConfirmationDate = null;
            UpdateObject(paymentRequest);
            return paymentRequest;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}