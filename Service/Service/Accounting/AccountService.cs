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
            return _repository.GetQueryable().Where(x => x.IsLeaf == true && !x.IsDeleted).ToList();
        }

        public IList<Account> GetLeafObjectsById(int? Id)
        {
            return _repository.GetQueryable().Where(x => x.IsLeaf == true && x.ParentId == Id && !x.IsDeleted).ToList();
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

        public Account CreateObject(Account account)
        {
            account.IsLeaf = true;
            account.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(account, this)) 
            {
                _repository.CreateObject(account);
                // Fix Parent's IsLeaf
                if (account.ParentId.GetValueOrDefault() > 0)
                {
                    Account parent = GetObjectById(account.ParentId.GetValueOrDefault());
                    if (parent != null && parent.IsLeaf)
                    {
                        parent.IsLeaf = false;
                        _repository.UpdateObject(parent);
                    }
                }
            };
            return account;
        }

        public Account CreateLegacyObject(Account account)
        {
            account.IsLegacy = true;
            account.Errors = new Dictionary<String, String>();
            return CreateObject(account);
        }

        public Account FindOrCreateLegacyObject(Account account)
        {
            Account legacyAccount = GetObjectByLegacyCode(account.LegacyCode);
            if (legacyAccount == null)
            {
                legacyAccount = CreateLegacyObject(account);
            }
            return legacyAccount;
        }

        public Account CreateCashBankAccount(Account account)
        {
            account.IsCashBankAccount = true;
            account.Errors = new Dictionary<String, String>();
            return CreateObject(account);
        }

        public Account UpdateObjectWithoutValidation(Account account, int? OldParentId)
        {
            _repository.UpdateObject(account);
            // Fix Old Parent's IsLeaf
            Account oldparent = GetObjectById(OldParentId.GetValueOrDefault());
            if (oldparent != null)
            {
                if (!GetLeafObjectsById(OldParentId).Any())
                {
                    if (!oldparent.IsLeaf)
                    {
                        oldparent.IsLeaf = true;
                        _repository.UpdateObject(oldparent);
                    }
                }
            }
            // Fix New Parent's IsLeaf
            if (account.ParentId.GetValueOrDefault() > 0)
            {
                Account newparent = GetObjectById(account.ParentId.GetValueOrDefault());
                if (newparent != null && newparent.IsLeaf)
                {
                    newparent.IsLeaf = false;
                    _repository.UpdateObject(newparent);
                }
            }
            // Move all childs to the same Group and re-adjust the Level
            if ((oldparent == null && account.ParentId.GetValueOrDefault() > 0) || (oldparent != null && (account.Group != oldparent.Group || account.Level != oldparent.Level + 1)))
            {
                UpdateChildren(account);
            }
            return account;
        }

        public Account UpdateObject(Account account, int? OldParentId)
        {
            if(_validator.ValidUpdateObject(account, this))
            {
                UpdateObjectWithoutValidation(account, OldParentId);
            }
            return account;
        }

        public Account UpdateObjectForCashBank(Account account, int? OldParentId)
        {
            if (_validator.ValidUpdateObjectForCashBank(account, this))
            {
                UpdateObjectWithoutValidation(account, OldParentId);
            }
            return account;
        }

        public Account UpdateObjectForLegacy(Account account, int? OldParentId)
        {
            if (_validator.ValidUpdateObjectForLegacy(account, this))
            {
                UpdateObjectWithoutValidation(account, OldParentId);
            }
            return account;
        }

        public Account SoftDeleteObjectWithoutValidation(Account account)
        {
            _repository.SoftDeleteObject(account);
            // Fix Parent's IsLeaf
            if (account.ParentId.GetValueOrDefault() > 0)
            {
                if (!GetLeafObjectsById(account.ParentId).Any())
                {
                    Account parent = GetObjectById(account.ParentId.GetValueOrDefault());
                    if (parent != null && !parent.IsLeaf)
                    {
                        parent.IsLeaf = true;
                        _repository.UpdateObject(parent);
                    }
                }
            }
            return account;
        }

        public Account SoftDeleteObject(Account account)
        {
            if(_validator.ValidDeleteObject(account))
            {
                SoftDeleteObjectWithoutValidation(account);
            }
            return account;
        }

        public Account SoftDeleteObjectForCashBank(Account account)
        {
            if (_validator.ValidDeleteObjectForCashBank(account))
            {
                SoftDeleteObjectWithoutValidation(account);
            }
            return account;
        }

        public Account SoftDeleteObjectForLegacy(Account account)
        {
            if (_validator.ValidDeleteObjectForLegacy(account))
            {
                SoftDeleteObjectWithoutValidation(account);
            }
            return account;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(Account account)
        {
            return _repository.FindAll(x => !x.IsDeleted && x.Code == account.Code && x.Id != account.Id).Any();
        }

        public int UpdateChildren(Account parent)
        {
            int count = 0;
            foreach(var child in GetQueryable().Where(x => !x.IsDeleted && x.ParentId != null && x.ParentId.Value == parent.Id))
            {
                //child.Errors = new Dictionary<string,string>();
                child.Group = parent.Group;
                child.Level = parent.Level + 1;
                _repository.UpdateObject(child);
                count++;
                if (!child.IsLeaf)
                {
                    count += UpdateChildren(child); //
                }
            }
            return count;
        }

        public string GetGroupName(int Group)
        {
            string name = "";
            switch (Group)
            {
                case 1: name = "Asset"; break;
                case 2: name = "Expense"; break;
                case 3: name = "Liability"; break;
                case 4: name = "Equity"; break;
                case 5: name = "Revenue"; break;
            }
            return name;
        }

    }
}
