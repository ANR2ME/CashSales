using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Core.Interface.Validation;
using System.Data.Objects;

namespace Service.Service
{
    public class ClosingService : IClosingService
    {
        private IClosingRepository _repository;
        private IClosingValidator _validator;

        public ClosingService(IClosingRepository _closingRepository, IClosingValidator _closingValidator)
        {
            _repository = _closingRepository;
            _validator = _closingValidator;
        }

        public IClosingValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Closing> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Closing> GetAll()
        {
            return _repository.GetAll();
        }

        public Closing GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Closing GetObjectByPeriodAndYear(int Period, int YearPeriod)
        {
            return _repository.GetObjectByPeriodAndYear(Period, YearPeriod);
        }

        public Closing CreateObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService)
        {
            closing.Errors = new Dictionary<String, String>();
            // Create all ValidComb

            if (_validator.ValidCreateObject(closing, this))
            {
                _repository.CreateObject(closing);
                IList<Account> allAccounts = _accountService.GetQueryable().Where(x => !x.IsDeleted).OrderBy(x => x.Code).ToList();
                foreach (var account in allAccounts)
                {
                    ValidComb validComb = new ValidComb()
                    {
                        AccountId = account.Id,
                        ClosingId = closing.Id,
                        Amount = 0
                    };
                    _validCombService.CreateObject(validComb, _accountService, this);
                }
            }
            return closing;
        }

        public Closing CloseObject(Closing closing, IAccountService _accountService,
                                   IGeneralLedgerJournalService _generalLedgerJournalService, IValidCombService _validCombService)
        {
            if (_validator.ValidCloseObject(closing, this))
            {
                // Get Last Closing
                var lastClosing = _repository.GetQueryable().Where(x => EntityFunctions.AddDays(EntityFunctions.TruncateTime(x.EndDatePeriod), 1) == closing.BeginningPeriod).FirstOrDefault();

                // Count ValidComb for each leaf account
                IList<Account> leafAccounts = _accountService.GetLeafObjects();
                foreach(var leaf in leafAccounts)
                {
                    DateTime EndDate = closing.EndDatePeriod.Date.AddDays(1);
                    IList<GeneralLedgerJournal> ledgers = _generalLedgerJournalService.GetQueryable()
                                                          .Where(x => x.AccountId == leaf.Id && 
                                                                 x.TransactionDate >= closing.BeginningPeriod.Date && 
                                                                 x.TransactionDate < EndDate)
                                                          .ToList();
                    decimal totalAmountInLedgers = 0;
                    decimal totalAmountLast = 0;
                    if (lastClosing != null
                        && (leaf.Group != (int)Constant.AccountGroup.Expense && leaf.Group != (int)Constant.AccountGroup.Revenue)
                        )
                    {
                        var lastVC = _validCombService.GetQueryable().
                            Where(x => x.ClosingId == lastClosing.Id && x.AccountId == leaf.Id).FirstOrDefault();
                        totalAmountLast = lastVC != null ? lastVC.Amount : 0;
                    }
                    totalAmountInLedgers += totalAmountLast;
                    foreach(var ledger in ledgers)
                    {
                        Account account = _accountService.GetObjectById(ledger.AccountId);
                        if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                            (account.Group == (int)Constant.AccountGroup.Asset ||
                             account.Group == (int)Constant.AccountGroup.Expense)) ||
                           (ledger.Status == Constant.GeneralLedgerStatus.Credit &&
                            (account.Group == (int)Constant.AccountGroup.Liability ||
                             account.Group == (int)Constant.AccountGroup.Equity ||
                             account.Group == (int)Constant.AccountGroup.Revenue)))
                        {
                            totalAmountInLedgers += ledger.Amount;
                        }
                        else 
                        {
                            totalAmountInLedgers -= ledger.Amount;
                        }
                    }

                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(leaf.Id, closing.Id);
                    validComb.Amount = totalAmountInLedgers;
                    _validCombService.UpdateObject(validComb, _accountService, this);
                }

                var groupNodeAccounts = _accountService.GetQueryable().Where(x => !x.IsLeaf && !x.IsDeleted).OrderByDescending(x => x.Level).ToList();
                foreach (var groupNode in groupNodeAccounts)
                {
                    FillValidComb(groupNode, closing, _accountService, _validCombService);
                }

                // Clearance
                Account Revenue = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue);
                Account Expense = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Expense);
                Account RetainedEarnings = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.RetainedEarnings);
                Account RetainedEarnings2 = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.RetainedEarnings2);
                Account Equity = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Equity);

                ValidComb VCRevenue = _validCombService.FindOrCreateObjectByAccountAndClosing(Revenue.Id, closing.Id);
                ValidComb VCExpense = _validCombService.FindOrCreateObjectByAccountAndClosing(Expense.Id, closing.Id);
                ValidComb VCRetainedEarnings = _validCombService.FindOrCreateObjectByAccountAndClosing(RetainedEarnings.Id, closing.Id);
                ValidComb VCRetainedEarnings2 = _validCombService.FindOrCreateObjectByAccountAndClosing(RetainedEarnings2.Id, closing.Id);
                ValidComb VCEquity = _validCombService.FindOrCreateObjectByAccountAndClosing(Equity.Id, closing.Id);

                VCRetainedEarnings.Amount += (VCRevenue.Amount - VCExpense.Amount); // use += instead of = if Rev & Exp not zeroed
                _validCombService.UpdateObject(VCRetainedEarnings, _accountService, this);
                VCRetainedEarnings2.Amount += (VCRevenue.Amount - VCExpense.Amount);
                _validCombService.UpdateObject(VCRetainedEarnings2, _accountService, this);
                VCEquity.Amount += VCRetainedEarnings.Amount;
                _validCombService.UpdateObject(VCEquity, _accountService, this);
                //VCRevenue.Amount = 0;
                //_validCombService.UpdateObject(VCRevenue, _accountService, this);
                //VCExpense.Amount = 0;
                //_validCombService.UpdateObject(VCExpense, _accountService, this);
                // End of Clearance

                _repository.CloseObject(closing);
            }
            return closing;
        }

        private void FillValidComb(Account nodeAccount, Closing closing, IAccountService _accountService, IValidCombService _validCombService)
        {
            ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(nodeAccount.Id, closing.Id);
            _validCombService.CalculateTotalAmount(validComb, _accountService, this);
        }

        public Closing OpenObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService)
        {
            if (_validator.ValidOpenObject(closing))
            {
                IList<Account> allAccounts = _accountService.GetAll();
                foreach (var account in allAccounts)
                {
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, closing.Id);
                    validComb.Amount = 0;
                    _validCombService.UpdateObject(validComb, _accountService, this);
                }
            }
            return _repository.OpenObject(closing);
        }

        public bool DeleteObject(int Id, IAccountService _accountService, IValidCombService _validCombService)
        {
            Closing closing = GetObjectById(Id);
            if (_validator.ValidDeleteObject(closing))
            {
                IList<Account> allAccounts = _accountService.GetAll();
                foreach (var account in allAccounts)
                {
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, Id);
                    _validCombService.DeleteObject(validComb.Id);
                }
                return _repository.DeleteObject(Id);
            }
            return false;
        }

        public bool IsDateClosed(DateTime DateToCheck)
        {
            DateTime dateEnd = DateToCheck.AddDays(1);
            var ClosedDates = _repository.FindAll(x => x.BeginningPeriod <= DateToCheck && x.EndDatePeriod > dateEnd && x.IsClosed).ToList();
            if (ClosedDates.Any())
            {
                return true;
            }
            return false;
        }
    }
}
