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

        public IList<Account> GetLegacyObjects()
        {
            return _repository.FindAll(x => x.IsLegacy == true).ToList();
        }

        public Account GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Account GetObjectByLegacyCode(string LegacyCode)
        {
            return _repository.Find(x => x.LegacyCode == LegacyCode);
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

        public Account CreateCashBankAccount(Account account, IAccountService _accountService)
        {
            account.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(account, _accountService) ? _repository.CreateObject(account) : account);
            /*
                string[] group = model.AccountGroup.Split(';');
                MstChartOfAccount mstCOA = new MstChartOfAccount();
                mstCOA.AccountCode = model.AccountCode;
                mstCOA.AccountGroup = group[1];
                mstCOA.AccountLevel = model.AccountLevel;
                mstCOA.AccountParent = !String.IsNullOrEmpty(model.AccountParent) ? model.AccountParent.Trim() : "";
                mstCOA.AccountTitle = !String.IsNullOrEmpty(model.AccountTitle) ? model.AccountTitle.Trim().ToUpper() : "";
                mstCOA.AccountType = model.AccountType;
                mstCOA.CompanyId = AccountModels.GetCompanyId();
                mstCOA.CreateBy = AccountModels.GetUserId();
                mstCOA.CreateOn = DateTime.Now;

                string LastAccount = GetLastAccountCode(model.AccountParent, AccountModels.GetCompanyId(), model.AccountLevel);
                
                if (model.AccountLevel.Value < 2)
                {
                    if (model.AccountParent == null)
                    {
                        mstCOA.AccountCode = group[0];
                    }
                    else
                    {
                        if (LastAccount == null)
                        {
                            mstCOA.AccountCode = mstCOA.AccountParent.Trim() + "1";
                        }
                        else
                        {
                            if (int.Parse(GeneralFunction.Right(LastAccount, 1)) + 1 > 9)
                            {
                                respModel.isValid = false;
                                respModel.message = "Chart Account can't be Added anymore !";
                                return respModel;
                            }
                            else
                            {
                                mstCOA.AccountCode = mstCOA.AccountParent.Trim() + (int.Parse(LastAccount.Substring(LastAccount.Length - 1, 1)) + 1).ToString();
                            }
                        }
                    }
                }
                else if (model.AccountLevel > 4)
                {
                    if (LastAccount == null)
                    {
                        mstCOA.AccountCode = mstCOA.AccountParent.Trim() + "001";
                    }
                    else
                    {
                        if (decimal.Parse(GeneralFunction.Right(LastAccount, 3)) + 1 > 999)
                        {
                            respModel.isValid = false;
                            respModel.message = "Chart Account can't be Added anymore !";
                            return respModel;
                        }
                        else
                        {
                            mstCOA.AccountCode = mstCOA.AccountParent.Trim() + GeneralFunction.Replicate((int.Parse(LastAccount.Substring(LastAccount.Length - 3, 3)) + 1).ToString(), 3);
                        }
                    }
                }
                else
                {
                    if (LastAccount == null)
                    {
                        mstCOA.AccountCode = mstCOA.AccountParent + "01";
                        LOG.Debug("COAELSEa" + mstCOA.AccountCode);
                    }
                    else
                    {
                        if (decimal.Parse(GeneralFunction.Right(LastAccount, 2)) + 1 > 99)
                        {
                            respModel.isValid = false;
                            respModel.message = "Chart Account can't be Added anymore !";
                            return respModel;
                        }
                        else
                        {
                            mstCOA.AccountCode = mstCOA.AccountParent.Trim() + GeneralFunction.Replicate((int.Parse(LastAccount.Substring(LastAccount.Length - 2, 2)) + 1).ToString(), 2);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(mstCOA.AccountCode))
                    mstCOA.AccountCode = mstCOA.AccountCode.Trim();
            */

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
