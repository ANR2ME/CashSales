using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPaymentRequestDetailRepository : IRepository<PaymentRequestDetail>
    {
        IQueryable<PaymentRequestDetail> GetQueryable();
        IList<PaymentRequestDetail> GetAll();
        IList<PaymentRequestDetail> GetAllByMonthCreated();
        IList<PaymentRequestDetail> GetObjectsByPaymentRequestId(int paymentRequestId);
        IList<PaymentRequestDetail> GetNonLegacyObjectsByPaymentRequestId(int paymentRequestId);
        PaymentRequestDetail GetLegacyObjectByPaymentRequestId(int paymentRequestId);
        PaymentRequestDetail GetObjectById(int Id);
        PaymentRequestDetail CreateObject(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail UpdateObject(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail paymentRequestDetail);
        bool DeleteObject(int Id);
        PaymentRequestDetail ConfirmObject(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail UnconfirmObject(PaymentRequestDetail paymentRequestDetail);
        string SetObjectCode(string ParentCode);
    }
}