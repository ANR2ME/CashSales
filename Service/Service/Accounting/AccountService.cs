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

        public Account UpdateObject(Account account, int? OldParentId)
        {
            if(_validator.ValidUpdateObject(account, this))
            {
                _repository.UpdateObject(account);
                // Fix Old Parent's IsLeaf
                if (OldParentId.GetValueOrDefault() > 0)
                {
                    if (!GetLeafObjectsById(OldParentId).Any())
                    {
                        Account parent = GetObjectById(OldParentId.GetValueOrDefault());
                        if (parent != null && !parent.IsLeaf)
                        {
                            parent.IsLeaf = true;
                            _repository.UpdateObject(parent);
                        }
                    }
                }
                // Fix New Parent's IsLeaf
                if (account.ParentId.GetValueOrDefault() > 0)
                {
                    Account parent = GetObjectById(account.ParentId.GetValueOrDefault());
                    if (parent != null && parent.IsLeaf)
                    {
                        parent.IsLeaf = false;
                        _repository.UpdateObject(parent);
                    }
                }
                // TODO : Move all childs to the same Group and re-adjust the Level

            };
            return account;
        }

        public Account UpdateObjectForCashBank(Account account, int? OldParentId)
        {
            if (_validator.ValidUpdateObjectForCashBank(account, this))
            {
                _repository.UpdateObject(account);
                // Fix Old Parent's IsLeaf
                if (OldParentId.GetValueOrDefault() > 0)
                {
                    if (!GetLeafObjectsById(OldParentId).Any())
                    {
                        Account parent = GetObjectById(OldParentId.GetValueOrDefault());
                        if (parent != null && !parent.IsLeaf)
                        {
                            parent.IsLeaf = true;
                            _repository.UpdateObject(parent);
                        }
                    }
                }
                // Fix New Parent's IsLeaf
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

        public Account UpdateObjectForLegacy(Account account, int? OldParentId)
        {
            if (_validator.ValidUpdateObjectForLegacy(account, this))
            {
                _repository.UpdateObject(account);
                // Fix Old Parent's IsLeaf
                if (OldParentId.GetValueOrDefault() > 0)
                {
                    if (!GetLeafObjectsById(OldParentId).Any())
                    {
                        Account parent = GetObjectById(OldParentId.GetValueOrDefault());
                        if (parent != null && !parent.IsLeaf)
                        {
                            parent.IsLeaf = true;
                            _repository.UpdateObject(parent);
                        }
                    }
                }
                // Fix New Parent's IsLeaf
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

        public Account SoftDeleteObject(Account account)
        {
            if(_validator.ValidDeleteObject(account))
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
            };
            return account;
        }

        public Account SoftDeleteObjectForCashBank(Account account)
        {
            if (_validator.ValidDeleteObjectForCashBank(account))
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
            };
            return account;
        }

        public Account SoftDeleteObjectForLegacy(Account account)
        {
            if (_validator.ValidDeleteObjectForLegacy(account))
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
            };
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


    }
}
