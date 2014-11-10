﻿using Core.DomainModel;
using Core.Constants;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;

namespace Service.Service
{
    public class CashBankService : ICashBankService
    {
        private ICashBankRepository _repository;
        private ICashBankValidator _validator;
        public CashBankService(ICashBankRepository _cashBankRepository, ICashBankValidator _cashBankValidator)
        {
            _repository = _cashBankRepository;
            _validator = _cashBankValidator;
        }

        public ICashBankValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CashBank> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashBank> GetAll()
        {
            return _repository.GetAll();
        }

        public CashBank GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CashBank GetObjectByName(string Name)
        {
            return _repository.GetObjectByName(Name);
        }

        public CashBank CreateObject(CashBank cashBank, IAccountService _accountService)
        {
            cashBank.Errors = new Dictionary<string, string>();
            if (_validator.ValidCreateObject(cashBank, this))
            {
                _repository.CreateObject(cashBank);
                Account account = _accountService.GetObjectByNameAndParentLegacyCode(Constant.AccountLegacyCode.CashBank, cashBank.Name);
                if (account == null)
                {
                    // Create Leaf Cash Bank Account
                    string Code = GenerateAccountCode(_accountService);
                    account = new Account()
                    {
                        Name = cashBank.Name,
                        Level = 3,
                        Group = Constant.AccountGroup.Asset,
                        Code = Code,
                        IsCashBankAccount = true,
                        IsLeaf = true,
                        ParentId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank).Id,
                        LegacyCode = Constant.AccountLegacyCode.CashBank + cashBank.Id,
                    };
                    _accountService.CreateCashBankAccount(account, _accountService);

                    //account.LegacyCode = Constant.AccountLegacyCode.CashBank + cashBank.Id;
                    //_accountService.UpdateObject(account, _accountService);
                }
            }
            return cashBank;
        }

        public CashBank UpdateObject(CashBank cashBank)
        {
            return (cashBank = _validator.ValidUpdateObject(cashBank, this) ? _repository.UpdateObject(cashBank) : cashBank);
        }

        public CashBank SoftDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService, IAccountService _accountService)
        {
            if(_validator.ValidDeleteObject(cashBank, _cashMutationService))
            {
                _repository.SoftDeleteObject(cashBank);
                Account account = _accountService.GetObjectByNameAndLegacyCode(Constant.AccountLegacyCode.CashBank + cashBank.Id, cashBank.Name);
                if (account != null)
                {
                    _accountService.SoftDeleteObject(account);
                }
            }
            return cashBank;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(CashBank cashBank)
        {
            IQueryable<CashBank> cashbanks = _repository.FindAll(cb => cb.Name == cashBank.Name && !cb.IsDeleted && cb.Id != cashBank.Id);
            return (cashbanks.Count() > 0 ? true : false);
        }

        public decimal GetTotalCashBank()
        {
            IList<CashBank> cashBanks = GetAll();
            decimal Total = 0;
            foreach (var cashBank in cashBanks)
            {
                Total += cashBank.Amount;
            }
            return Total;
        }

        public string GenerateAccountCode(IAccountService _accountService)
        {
            int ParentId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.CashBank).Id;
            string parentCode = _accountService.GetObjectById(ParentId).Code;
            int newId = _accountService.GetQueryable().Where(x => x.ParentId == ParentId && !x.IsDeleted).Count() + 1;
            while (true)
            {
                if (_accountService.GetObjectByLegacyCode(parentCode + newId.ToString()) == null)
                {
                    return parentCode + newId.ToString();
                }
                newId += 1;
            }
        }
    }
}