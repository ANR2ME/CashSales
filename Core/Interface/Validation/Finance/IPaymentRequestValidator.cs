using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPaymentRequestValidator
    {
        PaymentRequest VHasContact(PaymentRequest paymentRequest, IContactService _contatService);
        PaymentRequest VIsValidAmount(PaymentRequest paymentRequest);
        PaymentRequest VDebitEqualCreditEqualAmount(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService);
        PaymentRequest VHasNotBeenConfirmed(PaymentRequest paymentRequest);
        PaymentRequest VHasBeenConfirmed(PaymentRequest paymentRequest);
        PaymentRequest VHasNotBeenDeleted(PaymentRequest paymentRequest);
        PaymentRequest VHasConfirmationDate(PaymentRequest paymentRequest);
        PaymentRequest VGeneralLedgerPostingHasNotBeenClosed(PaymentRequest paymentRequest, IClosingService _closingService, int CaseConfirmUnconfirm);

        PaymentRequest VCreateObject(PaymentRequest paymentRequest, IContactService _contactService);
        PaymentRequest VUpdateObject(PaymentRequest paymentRequest, IContactService _contactService);
        PaymentRequest VDeleteObject(PaymentRequest paymentRequest);
        PaymentRequest VConfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService);
        PaymentRequest VUnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService);
        bool ValidCreateObject(PaymentRequest paymentRequest, IContactService _contactService);
        bool ValidUpdateObject(PaymentRequest paymentRequest, IContactService _contactService);
        bool ValidDeleteObject(PaymentRequest paymentRequest);
        bool ValidConfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService);
        bool ValidUnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService);
        bool isValid(PaymentRequest paymentRequest);
        string PrintError(PaymentRequest paymentRequest);
    }
}
