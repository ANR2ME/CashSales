using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPaymentRequestDetailValidator
    {
        PaymentRequestDetail VHasPaymentRequest(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService);
        PaymentRequestDetail VHasAccount(PaymentRequestDetail paymentRequestDetail, IAccountService _accountService);
        PaymentRequestDetail VHasBeenConfirmed(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VHasNotBeenConfirmed(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VHasNotBeenDeleted(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VAmountIsTheSameWithPaymentRequestAmount(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService);
        PaymentRequestDetail VNonNegativeAmount(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VStatusIsCredit(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VStatusIsDebit(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VNotLegacyObject(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VCreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        PaymentRequestDetail VUpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        PaymentRequestDetail VDeleteObject(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VCreateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        PaymentRequestDetail VUpdateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        PaymentRequestDetail VHasConfirmationDate(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VConfirmObject(PaymentRequestDetail paymentRequestDetail);
        PaymentRequestDetail VUnconfirmObject(PaymentRequestDetail paymentRequestDetail);
        bool ValidCreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        bool ValidUpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        bool ValidDeleteObject(PaymentRequestDetail paymentRequestDetail);
        bool ValidCreateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        bool ValidUpdateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        bool ValidConfirmObject(PaymentRequestDetail paymentRequestDetail);
        bool ValidUnconfirmObject(PaymentRequestDetail paymentRequestDetail);
        bool isValid(PaymentRequestDetail paymentRequestDetail);
        string PrintError(PaymentRequestDetail paymentRequestDetail);
    }
}
