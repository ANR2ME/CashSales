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
    public class AccountService : IAccountService
    {
        private IAccountRepository _repository;
        private IAccountValidator _validator;

        public AccountService(IAccountRepository _accountRepository, IAccountValidator _accountValidator)
        {
            _repository = _accountRepository;
            _validator = _accountValidator;
        }

        public IAccountValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Account> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Account> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Account> GetLeafObjects()
        {
            return _repository.GetQueryable().Where(x => x.IsLeaf == true).ToList();
        }

        public IList<Account> GetLegacyObjects()
        {
            return _repository.FindAll(x => x.IsLegacy == true && !x.IsDeleted).ToList();
        }

        public Account GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Account GetObjectByLegacyCode(string LegacyCode)
        {
            Account account = _repository.FindAll(x => x.LegacyCode == LegacyCode && !x.IsDeleted).FirstOrDefault();
            if (account != null) account.Errors = new Dictionary<string, string>();
            return account;
        }

        public Account GetObjectByNameAndLegacyCode(string LegacyCode, string Name)
        {
            Account account = _repository.FindAll(x => x.LegacyCode == LegacyCode && x.Name == Name && !x.IsDeleted).FirstOrDefault();
            if (account != null) account.Errors = new Dictionary<string, string>();
            return account;
        }

        public Account GetObjectByNameAndParentLegacyCode(string ParentLegacyCode, string Name)
        {
            Account Parent = GetObjectByLegacyCode(ParentLegacyCode);
            Nullable<int> ParentId = null;
            if (Parent != null) ParentId = Parent.Id;
            Account account = _repository.FindAll(x => x.ParentId == ParentId && x.Name == Name && !x.IsDeleted).FirstOrDefault();
            if (account != null) account.Errors = new Dictionary<string, string>();
            return account;
        }

        public Account GetObjectByIsLegacy(bool IsLegacy)
        {
            return _repository.GetObjectByIsLegacy(IsLegacy);
        }

        public Account CreateObject(Account account, IAccountService _accountService)
        {
            account.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(account, _accountService) ? _repository.CreateObject(account) : account);
        }

        public Account CreateLegacyObject(Account account, IAccountService _accountService)
        {
            account.IsLegacy = true;
            account.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(account, _accountService) ? _repository.CreateObject(account) : account);
        }

        public Account FindOrCreateLegacyObject(Account account, IAccountService _accountService)
        {
            Account legacyAccount = GetObjectByLegacyCode(account.LegacyCode);
            if (legacyAccount == null)
            {
                legacyAccount = CreateLegacyObject(account, _accountService);
            }
            //else
            //{
            //    legacyAccount.Errors = new Dictionary<String, String>();
            //}
            return legacyAccount;
        }

        public Account CreateCashBankAccount(Account account, IAccountService _accountService)
        {
            account.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(account, _accountService) ? _repository.CreateObject(account) : account);
        }

        public Account UpdateObject(Account account, IAccountService _accountService)
        {
            return (_validator.ValidUpdateObject(account, _accountService) ? _repository.UpdateObject(account) : account);
        }

        public Account SoftDeleteObject(Account account)
        {
            return (_validator.ValidDeleteObject(account) ? _repository.SoftDeleteObject(account) : account);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
