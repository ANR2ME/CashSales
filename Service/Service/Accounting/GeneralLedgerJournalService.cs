using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Service.Service
{
    public class GeneralLedgerJournalService : IGeneralLedgerJournalService
    {
        private IGeneralLedgerJournalRepository _repository;
        private IGeneralLedgerJournalValidator _validator;

        public GeneralLedgerJournalService(IGeneralLedgerJournalRepository _generalLedgerJournalRepository, IGeneralLedgerJournalValidator _generalLedgerJournalValidator)
        {
            _repository = _generalLedgerJournalRepository;
            _validator = _generalLedgerJournalValidator;
        }

        public IGeneralLedgerJournalValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<GeneralLedgerJournal> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<GeneralLedgerJournal> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<GeneralLedgerJournal> GetObjectsByAccountId(int accountId)
        {
            return _repository.GetObjectsByAccountId(accountId);
        }

        public GeneralLedgerJournal GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<GeneralLedgerJournal> GetObjectsBySourceDocument(int accountId, string SourceDocument, int SourceDocumentId)
        {
            return _repository.GetObjectsBySourceDocument(accountId, SourceDocument, SourceDocumentId);
        }

        public GeneralLedgerJournal CreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService)
        {
            generalLedgerJournal.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(generalLedgerJournal, _accountService) ? _repository.CreateObject(generalLedgerJournal) : generalLedgerJournal);
        }

        public GeneralLedgerJournal SoftDeleteObject(GeneralLedgerJournal generalLedgerJournal)
        {
            return (_validator.ValidDeleteObject(generalLedgerJournal) ? _repository.SoftDeleteObject(generalLedgerJournal) : generalLedgerJournal);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            // User Input Memorial
            #region User Input Memorial

            IList<MemorialDetail> details = _memorialDetailService.GetObjectsByMemorialId(memorial.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            foreach (var memorialDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = memorialDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.Memorial,
                    SourceDocumentId = memorial.Id,
                    TransactionDate = (DateTime)memorial.ConfirmationDate,
                    Status = memorialDetail.Status,
                    Amount = memorialDetail.Amount
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForMemorial(Memorial memorial, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            // Use Input Memorial
            #region User Input Memorial

            DateTime UnconfirmationDate = DateTime.Now;
            IList<MemorialDetail> details = _memorialDetailService.GetObjectsByMemorialId(memorial.Id);
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            foreach (var memorialDetail in details)
            {
                GeneralLedgerJournal journal = new GeneralLedgerJournal()
                {
                    AccountId = memorialDetail.AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.Memorial,
                    SourceDocumentId = memorial.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = (memorialDetail.Status == Constant.GeneralLedgerStatus.Debit) ? Constant.GeneralLedgerStatus.Credit : Constant.GeneralLedgerStatus.Debit,
                    Amount = memorialDetail.Amount
                };
                journal = CreateObject(journal, _accountService);
                journals.Add(journal);
            }

            return journals;
            #endregion
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = (DateTime)paymentVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentVoucher.TotalAmount
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = (DateTime)paymentVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentVoucher.TotalAmount
            };
            creditcashbank = CreateObject(creditcashbank, _accountService);

            journals.Add(debitaccountpayable);
            journals.Add(creditcashbank);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForPaymentVoucher(PaymentVoucher paymentVoucher, CashBank cashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = paymentVoucher.TotalAmount
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);

            GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.PaymentVoucher,
                SourceDocumentId = paymentVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = paymentVoucher.TotalAmount
            };
            debitcashbank = CreateObject(debitcashbank, _accountService);

            journals.Add(creditaccountpayable);
            journals.Add(debitcashbank);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime)receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = receiptVoucher.TotalAmount
            };
            debitcashbank = CreateObject(debitcashbank, _accountService);

            GeneralLedgerJournal creditaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = (DateTime)receiptVoucher.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount
            };
            creditaccountreceivable = CreateObject(creditaccountreceivable, _accountService);

            journals.Add(debitcashbank);
            journals.Add(creditaccountreceivable);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForReceiptVoucher(ReceiptVoucher receiptVoucher, CashBank cashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = receiptVoucher.TotalAmount
            };
            creditcashbank = CreateObject(creditcashbank, _accountService);

            GeneralLedgerJournal debitaccountreceivable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.ReceiptVoucher,
                SourceDocumentId = receiptVoucher.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = receiptVoucher.TotalAmount
            };
            debitaccountreceivable = CreateObject(debitaccountreceivable, _accountService);

            journals.Add(creditcashbank);
            journals.Add(debitaccountreceivable);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (cashBankAdjustment.Amount >= 0)
            {
                string LegacyCode = Constant.AccountLegacyCode.CashBank + cashBank.Id.ToString();
                int AccountId = _accountService.GetObjectByLegacyCode(LegacyCode).Id;
                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = AccountId,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = cashBankAdjustment.Amount
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                GeneralLedgerJournal creditcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = cashBankAdjustment.Amount
                };
                creditcashbankequityadjustment = CreateObject(creditcashbankequityadjustment, _accountService);

                journals.Add(debitcashbank);
                journals.Add(creditcashbankequityadjustment);
            }
            else
            {
                GeneralLedgerJournal debitcashbankadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBankAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
                };
                debitcashbankadjustmentexpense = CreateObject(debitcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = (DateTime)cashBankAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                journals.Add(debitcashbankadjustmentexpense);
                journals.Add(creditcashbank);
            }

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankAdjustment(CashBankAdjustment cashBankAdjustment, CashBank cashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            if (cashBankAdjustment.Amount >= 0)
            {
                GeneralLedgerJournal creditcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = cashBankAdjustment.Amount
                };
                creditcashbank = CreateObject(creditcashbank, _accountService);

                GeneralLedgerJournal debitcashbankequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = cashBankAdjustment.GetType().ToString(),
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = cashBankAdjustment.Amount
                };
                debitcashbankequityadjustment = CreateObject(debitcashbankequityadjustment, _accountService);

                journals.Add(creditcashbank);
                journals.Add(debitcashbankequityadjustment);
            }
            else
            {
                GeneralLedgerJournal creditcashbankadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBankAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
                };
                creditcashbankadjustmentexpense = CreateObject(creditcashbankadjustmentexpense, _accountService);

                GeneralLedgerJournal debitcashbank = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id).Id,
                    SourceDocument = Constant.GeneralLedgerSource.CashBankAdjustment,
                    SourceDocumentId = cashBankAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(cashBankAdjustment.Amount)
                };
                debitcashbank = CreateObject(debitcashbank, _accountService);

                journals.Add(creditcashbankadjustmentexpense);
                journals.Add(debitcashbank);
            }

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debittargetcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + targetCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime)cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashBankMutation.Amount
            };
            debittargetcashbank = CreateObject(debittargetcashbank, _accountService);

            GeneralLedgerJournal creditsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = (DateTime)cashBankMutation.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashBankMutation.Amount
            };
            creditsourcecashbank = CreateObject(creditsourcecashbank, _accountService);

            journals.Add(debittargetcashbank);
            journals.Add(creditsourcecashbank);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashBankMutation(CashBankMutation cashBankMutation, CashBank sourceCashBank, CashBank targetCashBank, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal credittargetcashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + targetCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashBankMutation.Amount
            };
            credittargetcashbank = CreateObject(credittargetcashbank, _accountService);

            GeneralLedgerJournal debitsourcecashbank = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank + sourceCashBank.Id).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashBankMutation,
                SourceDocumentId = cashBankMutation.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashBankMutation.Amount
            };
            debitsourcecashbank = CreateObject(debitsourcecashbank, _accountService);

            journals.Add(credittargetcashbank);
            journals.Add(debitsourcecashbank);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitcogs = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = (DateTime)cashSalesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashSalesInvoice.CoGS //Total
            };
            debitcogs = CreateObject(debitcogs, _accountService);

            GeneralLedgerJournal creditinventory = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = (DateTime)cashSalesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashSalesInvoice.CoGS, //Total
            };
            creditinventory = CreateObject(creditinventory, _accountService);

            journals.Add(debitcogs);
            journals.Add(creditinventory);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditcogs = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashSalesInvoice.CoGS //Total
            };
            creditcogs = CreateObject(creditcogs, _accountService);

            GeneralLedgerJournal debitinventory = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashSalesInvoice.CoGS, //Total
            };
            debitinventory = CreateObject(debitinventory, _accountService);

            journals.Add(creditcogs);
            journals.Add(debitinventory);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreatePaidJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debittotal = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = (DateTime)cashSalesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashSalesInvoice.Total,
            };
            debittotal = CreateObject(debittotal, _accountService);

            GeneralLedgerJournal creditrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = (DateTime)cashSalesInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashSalesInvoice.Total, // AmountPaid.GetValueOrDefault()
            };
            creditrevenue = CreateObject(creditrevenue, _accountService);

            journals.Add(debittotal);
            journals.Add(creditrevenue);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnpaidJournalForCashSalesInvoice(CashSalesInvoice cashSalesInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal credittotal = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = cashSalesInvoice.Total,
            };
            credittotal = CreateObject(credittotal, _accountService);

            GeneralLedgerJournal debitrevenue = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id,
                SourceDocument = Constant.GeneralLedgerSource.CashSalesInvoice,
                SourceDocumentId = cashSalesInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = cashSalesInvoice.Total, // AmountPaid.GetValueOrDefault()
            };
            debitrevenue = CreateObject(debitrevenue, _accountService);

            journals.Add(credittotal);
            journals.Add(debitrevenue);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            if (stockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal debitinventory = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = stockAdjustment.Total
                };
                debitinventory = CreateObject(debitinventory, _accountService);

                GeneralLedgerJournal creditstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = stockAdjustment.Total
                };
                creditstockequityadjustment = CreateObject(creditstockequityadjustment, _accountService);

                journals.Add(debitinventory);
                journals.Add(creditstockequityadjustment);
            }
            else
            {
                GeneralLedgerJournal debitstockadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.StockAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                debitstockadjustmentexpense = CreateObject(debitstockadjustmentexpense, _accountService);

                GeneralLedgerJournal creditinventory = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = (DateTime)stockAdjustment.ConfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                creditinventory = CreateObject(creditinventory, _accountService);

                journals.Add(debitstockadjustmentexpense);
                journals.Add(creditinventory);
            }

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForStockAdjustment(StockAdjustment stockAdjustment, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            if (stockAdjustment.Total >= 0)
            {
                GeneralLedgerJournal creditinventory = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = stockAdjustment.Total
                };
                creditinventory = CreateObject(creditinventory, _accountService);

                GeneralLedgerJournal debitstockequityadjustment = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.EquityAdjustment).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = stockAdjustment.Total
                };
                debitstockequityadjustment = CreateObject(debitstockequityadjustment, _accountService);

                journals.Add(creditinventory);
                journals.Add(debitstockequityadjustment);
            }
            else
            {
                GeneralLedgerJournal creditstockadjustmentexpense = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.StockAdjustmentExpense).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Credit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                creditstockadjustmentexpense = CreateObject(creditstockadjustmentexpense, _accountService);

                GeneralLedgerJournal debitinventory = new GeneralLedgerJournal()
                {
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                    SourceDocument = Constant.GeneralLedgerSource.StockAdjustment,
                    SourceDocumentId = stockAdjustment.Id,
                    TransactionDate = UnconfirmationDate,
                    Status = Constant.GeneralLedgerStatus.Debit,
                    Amount = Math.Abs(stockAdjustment.Total)
                };
                debitinventory = CreateObject(debitinventory, _accountService);

                journals.Add(creditstockadjustmentexpense);
                journals.Add(debitinventory);
            }

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateConfirmationJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitinventory = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = (DateTime)customPurchaseInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = customPurchaseInvoice.Total, //CoGS
            };
            debitinventory = CreateObject(debitinventory, _accountService);

            GeneralLedgerJournal creditgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = (DateTime)customPurchaseInvoice.ConfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = customPurchaseInvoice.Total, //CoGS
            };
            creditgoodsPendingclearance = CreateObject(creditgoodsPendingclearance, _accountService);

            journals.Add(debitinventory);
            journals.Add(creditgoodsPendingclearance);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnconfirmationJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditinventory = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Inventory).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = customPurchaseInvoice.Total, //CoGS
            };
            creditinventory = CreateObject(creditinventory, _accountService);

            GeneralLedgerJournal debitgoodsPendingclearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = customPurchaseInvoice.Total, //CoGS
            };
            debitgoodsPendingclearance = CreateObject(debitgoodsPendingclearance, _accountService);

            journals.Add(creditinventory);
            journals.Add(debitgoodsPendingclearance);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreatePaidJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();

            GeneralLedgerJournal debitGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = (DateTime)customPurchaseInvoice.ConfirmationDate, // PaymentDate.GetValueOrDefault() ??
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = customPurchaseInvoice.Total, // AmountPaid.GetValueOrDefault()
            };
            debitGoodsPendingClearance = CreateObject(debitGoodsPendingClearance, _accountService);

            GeneralLedgerJournal creditaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = (DateTime)customPurchaseInvoice.ConfirmationDate, // PaymentDate.GetValueOrDefault() ??
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = customPurchaseInvoice.Total, // AmountPaid.GetValueOrDefault()
            };
            creditaccountpayable = CreateObject(creditaccountpayable, _accountService);

            journals.Add(debitGoodsPendingClearance);
            journals.Add(creditaccountpayable);

            return journals;
        }

        public IList<GeneralLedgerJournal> CreateUnpaidJournalForCustomPurchaseInvoice(CustomPurchaseInvoice customPurchaseInvoice, IAccountService _accountService)
        {
            IList<GeneralLedgerJournal> journals = new List<GeneralLedgerJournal>();
            DateTime UnconfirmationDate = DateTime.Now;

            GeneralLedgerJournal creditGoodsPendingClearance = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GoodsPendingClearance).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Credit,
                Amount = customPurchaseInvoice.Total, // AmountPaid.GetValueOrDefault()
            };
            creditGoodsPendingClearance = CreateObject(creditGoodsPendingClearance, _accountService);

            GeneralLedgerJournal debitaccountpayable = new GeneralLedgerJournal()
            {
                AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable).Id,
                SourceDocument = Constant.GeneralLedgerSource.CustomPurchaseInvoice,
                SourceDocumentId = customPurchaseInvoice.Id,
                TransactionDate = UnconfirmationDate,
                Status = Constant.GeneralLedgerStatus.Debit,
                Amount = customPurchaseInvoice.Total, // AmountPaid.GetValueOrDefault()
            };
            debitaccountpayable = CreateObject(debitaccountpayable, _accountService);

            journals.Add(creditGoodsPendingClearance);
            journals.Add(debitaccountpayable);

            return journals;
        }
    }
}
