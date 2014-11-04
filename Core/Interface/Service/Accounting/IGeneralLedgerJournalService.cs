using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IGeneralLedgerJournalService
    {
        IQueryable<GeneralLedgerJournal> GetQueryable();
        IGeneralLedgerJournalValidator GetValidator();
        IList<GeneralLedgerJournal> GetAll();
        GeneralLedgerJournal GetObjectById(int Id);
        IList<GeneralLedgerJournal> GetObjectsByAccountId(int accountId);
        IList<GeneralLedgerJournal> GetObjectsBySourceDocument(int accountId, string SourceDocumentType, int SourceDocumentId);
        GeneralLedgerJournal CreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService);
        GeneralLedgerJournal SoftDeleteObject(GeneralLedgerJournal generalLedgerJournal);
        bool DeleteObject(int Id);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentRequest(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreatePaidJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnpaidJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCashSalesReturn(CashSalesReturn cashSalesReturn, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashSalesReturn(CashSalesReturn cashSalesReturn, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreatePaidJournalForCashSalesReturn(CashSalesReturn cashSalesReturn, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnpaidJournalForCashSalesReturn(CashSalesReturn cashSalesReturn, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateConfirmationJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreatePaidJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService);
        IList<GeneralLedgerJournal> CreateUnpaidJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService);
    }
}