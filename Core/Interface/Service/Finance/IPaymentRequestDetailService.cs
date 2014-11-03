using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPaymentRequestDetailService
    {
        IPaymentRequestDetailValidator GetValidator();
        IQueryable<PaymentRequestDetail> GetQueryable();
        IList<PaymentRequestDetail> GetAll();
        IList<PaymentRequestDetail> GetObjectsByPaymentRequestId(int paymentRequestId);
        IList<PaymentRequestDetail> GetNonLegacyObjectsByPaymentRequestId(int paymentRequestId);
        PaymentRequestDetail GetLegacyObjectByPaymentRequestId(int paymentRequestId);
        PaymentRequestDetail GetObjectById(int Id);
        PaymentRequestDetail CreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService);
        PaymentRequestDetail UpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService);
        PaymentRequestDetail SoftDeleteObject(PaymentRequestDetail paymentRequestDetail);
        bool DeleteObject(int Id);
        PaymentRequestDetail CreateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService);
        PaymentRequestDetail UpdateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IAccountService _accountService);
        PaymentRequestDetail ConfirmObject(PaymentRequestDetail paymentRequestDetail, DateTime ConfirmationDate, IPaymentRequestService _paymentRequestService);
        PaymentRequestDetail UnconfirmObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService);
    }
}