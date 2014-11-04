using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPaymentRequestService
    {
        IQueryable<PaymentRequest> GetQueryable();
        IPaymentRequestValidator GetValidator();
        IList<PaymentRequest> GetAll();
        PaymentRequest GetObjectById(int Id);
        IList<PaymentRequest> GetObjectsByContactId(int contactId);
        PaymentRequest CreateObject(int ContactId, string Description, decimal Amount, DateTime RequestDate, DateTime DueDate, IContactService _contactService, IPaymentRequestDetailService _paymentRequestDetailService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PaymentRequest CreateObject(PaymentRequest paymentRequest, IContactService _contactService, IPaymentRequestDetailService _paymentRequestDetailService,
                                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PaymentRequest UpdateObject(PaymentRequest paymentRequest, IContactService _contactService, IPaymentRequestDetailService _paymentRequestDetailService,
                                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest);
        bool DeleteObject(int Id);
        PaymentRequest ConfirmObject(PaymentRequest paymentRequest, DateTime ConfirmationDate, IPayableService _payableService,
                                     IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService,
                                     IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PaymentRequest UnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IPayableService _payableService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}