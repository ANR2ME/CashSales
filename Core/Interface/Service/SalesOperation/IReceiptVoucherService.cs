using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IReceiptVoucherService
    {
        IQueryable<ReceiptVoucher> GetQueryable();
        IReceiptVoucherValidator GetValidator();
        IList<ReceiptVoucher> GetAll();
        ReceiptVoucher GetObjectById(int Id);
        IList<ReceiptVoucher> GetObjectsByCashBankId(int cashBankId);
        IList<ReceiptVoucher> GetObjectsByContactId(int contactId);
        ReceiptVoucher CreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher CreateObject(int cashBankId, int contactId, DateTime receiptDate, decimal totalAmount, bool IsGBCH, DateTime DueDate, bool IsBank,
                                    IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher UpdateAmount(ReceiptVoucher receiptVoucher);
        ReceiptVoucher UpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher SoftDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool DeleteObject(int Id);
        ReceiptVoucher ConfirmObject(ReceiptVoucher receiptVoucher, DateTime ConfirmationDate, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                     IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        ReceiptVoucher UnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                       ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        ReceiptVoucher ReconcileObject(ReceiptVoucher receiptVoucher, DateTime ReconciliationDate, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                       ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        ReceiptVoucher UnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                         ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                         IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
    }
}