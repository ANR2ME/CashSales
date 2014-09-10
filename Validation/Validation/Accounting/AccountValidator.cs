using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class AccountValidator : IAccountValidator
    {

        /*public Account VHasCashBank(Account account, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectByAccountId(account.Id);
            if (cashBank == null)
            {
                account.Errors.Add("Generic", "Tidak terasosiasi dengan CashBank");
            }
            return account;
        }*/

        public Account VHasCode(Account account)
        {
            if (account.Code == null)
            {
                account.Errors.Add("Code", "Tidak boleh kosong");
            }
            return account;
        }

        public Account VHasName(Account account)
        {
            if (account.Name == null || account.Name.Trim() == "")
            {
                account.Errors.Add("Name", "Tidak boleh kosong");
            }
            return account;
        }

        public Account VIsValidGroup(Account account)
        {
            if (!account.Group.Equals(Constant.AccountGroup.Asset) &&
                !account.Group.Equals(Constant.AccountGroup.Expense) &&
                !account.Group.Equals(Constant.AccountGroup.Liability) &&
                !account.Group.Equals(Constant.AccountGroup.Equity) &&
                !account.Group.Equals(Constant.AccountGroup.Revenue))
            {
                account.Errors.Add("Group", "Harus merupakan bagian dari Constant.AccountGroup");
            }
            return account;
        }

        public Account VIsValidLevel(Account account)
        {
            if (account.Level < 1 || account.Level > 5)
            {
                account.Errors.Add("Level", "Tidak valid");
            }
            return account;
        }

        public Account VIsValidParent(Account account, IAccountService _accountService)
        {
            if (account.Level > 1)
            {
                if (account.ParentId == null)
                {
                    account.Errors.Add("Parent", "Tidak boleh null");
                }
                else
                {
                    Account parent = _accountService.GetObjectById((int)account.ParentId);
                    if (parent == null)
                    {
                        account.Errors.Add("Parent", "Tidak ada");
                    }
                }
            }
            return account;
        }

        public Account VCreateObject(Account account, IAccountService _accountService)
        {
            //VHasCashBank(account, _cashBankService);
            //if (!isValid(account)) { return account; }
            VHasCode(account);
            if (!isValid(account)) { return account; }
            VHasName(account);
            if (!isValid(account)) { return account; }
            VIsValidGroup(account);
            if (!isValid(account)) { return account; }
            VIsValidLevel(account);
            if (!isValid(account)) { return account; }
            VIsValidParent(account, _accountService);
            return account;
        }

        public Account VUpdateObject(Account account, IAccountService _accountService)
        {
            VCreateObject(account, _accountService);
            return account;
        }

        public Account VDeleteObject(Account account)
        {
            return account;
        }

        public bool ValidCreateObject(Account account, IAccountService _accountService)
        {
            VCreateObject(account, _accountService);
            return isValid(account);
        }

        public bool ValidUpdateObject(Account account, IAccountService _accountService)
        {
            VUpdateObject(account, _accountService);
            return isValid(account);
        }

        public bool ValidDeleteObject(Account account)
        {
            account.Errors.Clear();
            VDeleteObject(account);
            return isValid(account);
        }

        public bool isValid(Account obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Account obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
