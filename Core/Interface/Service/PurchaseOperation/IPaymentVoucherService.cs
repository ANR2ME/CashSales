using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPaymentVoucherService
    {
        IQueryable<PaymentVoucher> GetQueryable();
        IPaymentVoucherValidator GetValidator();
        IList<PaymentVoucher> GetAll();
        PaymentVoucher GetObjectById(int Id);
        IList<PaymentVoucher> GetObjectsByCashBankId(int cashBankId);
        IList<PaymentVoucher> GetObjectsByContactId(int contactId);
        PaymentVoucher CreateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        PaymentVoucher CreateObject(int cashBankId, int contactId, DateTime paymentDate, decimal totalAmount, bool IsGBCH, DateTime DueDate, bool IsBank,
                                    IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        PaymentVoucher UpdateAmount(PaymentVoucher paymentVoucher);
        PaymentVoucher UpdateObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        PaymentVoucher SoftDeleteObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool DeleteObject(int Id);
        PaymentVoucher ConfirmObject(PaymentVoucher paymentVoucher, DateTime ConfirmationDate, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                     ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                     IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService);
        PaymentVoucher UnconfirmObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                       ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService);
        PaymentVoucher ReconcileObject(PaymentVoucher paymentVoucher, DateTime ReconciliationDate, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                       ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService);
        PaymentVoucher UnreconcileObject(PaymentVoucher paymentVoucher, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                         ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                         IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService);
    }
}