using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPaymentRequestRepository : IRepository<PaymentRequest>
    {
        IQueryable<PaymentRequest> GetQueryable();
        IList<PaymentRequest> GetAll();
        IList<PaymentRequest> GetAllByMonthCreated();
        PaymentRequest GetObjectById(int Id);
        IList<PaymentRequest> GetObjectsByContactId(int contactId);
        PaymentRequest CreateObject(PaymentRequest paymentRequest);
        PaymentRequest UpdateObject(PaymentRequest paymentRequest);
        PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest);
        bool DeleteObject(int Id);
        PaymentRequest ConfirmObject(PaymentRequest paymentRequest);
        PaymentRequest UnconfirmObject(PaymentRequest paymentRequest);
        string SetObjectCode();
    }
}