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
                IList<Account> allAccounts = _accountService.GetQueryable().OrderBy(x => x.Code).ToList();
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
                // Count ValidComb for each leaf account
                IList<Account> leafAccounts = _accountService.GetLeafObjects();
                foreach(var leaf in leafAccounts)
                {
                    DateTime EndDate = closing.EndDatePeriod.AddDays(1);
                    IList<GeneralLedgerJournal> ledgers = _generalLedgerJournalService.GetQueryable()
                                                          .Where(x => x.AccountId == leaf.Id && 
                                                                 x.TransactionDate >= closing.BeginningPeriod && 
                                                                 x.TransactionDate < EndDate)
                                                          .ToList();
                    decimal totalAmountInLedgers = 0;
                    foreach(var ledger in ledgers)
                    {
                        Account account = _accountService.GetObjectById(ledger.AccountId);
                        if ((ledger.Status == Constant.GeneralLedgerStatus.Debit &&
                            (account.Group == Constant.AccountGroup.Asset ||
                             account.Group == Constant.AccountGroup.Expense)) ||
                           (ledger.Status == Constant.GeneralLedgerStatus.Credit &&
                            (account.Group == Constant.AccountGroup.Liability ||
                             account.Group == Constant.AccountGroup.Equity ||
                             account.Group == Constant.AccountGroup.Revenue)))
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

                // Count validComb for all parent Nodes
                var groupLeafAccounts = _accountService.GetLeafObjects().GroupBy(x => x.ParentId)
                                                                        .Select(grp => grp.ToList()).ToList();
                FillValidComb(groupLeafAccounts, closing, _accountService, _validCombService);

                _repository.CloseObject(closing);
            }
            return closing;
        }

        private void FillValidComb(IList<List<Account>> nodeAccounts, Closing closing, IAccountService _accountService, IValidCombService _validCombService)
        {
            foreach(var group in nodeAccounts)
            {
                decimal totalNodeAmount = 0;
                foreach(var node in group)
                {
                    ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(node.Id, closing.Id);
                    totalNodeAmount += validComb.Amount;
                }
                if (group.First().ParentId == null) { return; }
                int AccountId = (int)group.First().ParentId ;
                ValidComb nodeValidComb = _validCombService.FindOrCreateObjectByAccountAndClosing(AccountId, closing.Id);
                nodeValidComb.Amount = totalNodeAmount;
                _validCombService.UpdateObject(nodeValidComb, _accountService, this);

                Account currentAccount = _accountService.GetObjectById(AccountId);
                if (currentAccount.ParentId != null)
                {
                    var parentNodeAccounts = _accountService.GetQueryable().Where(x => x.ParentId == currentAccount.ParentId).ToList()
                                                            .GroupBy(x => x.ParentId).Select(grp => grp.ToList()).ToList();

                    FillValidComb(parentNodeAccounts, closing, _accountService, _validCombService);
                }
            }
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
            IList<Account> allAccounts = _accountService.GetAll();
            foreach (var account in allAccounts)
            {
                ValidComb validComb = _validCombService.FindOrCreateObjectByAccountAndClosing(account.Id, Id);
                _validCombService.DeleteObject(validComb.Id);
            }
            return _repository.DeleteObject(Id);
        }

        public bool IsDateClosed(DateTime DateToCheck)
        {
            DateTime dateEnd = DateToCheck.AddDays(1);
            var ClosedDates = _repository.FindAll(x => x.BeginningPeriod <= DateToCheck && x.EndDatePeriod > dateEnd).ToList();
            if (ClosedDates.Any())
            {
                return true;
            }
            return false;
        }
    }
}
