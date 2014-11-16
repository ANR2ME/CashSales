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

        public Account VHasValidCode(Account account, IAccountService _accountService)
        {
            if (account.Code == null || account.Code.Trim() == "")
            {
                account.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_accountService.IsCodeDuplicated(account))
            {
                account.Errors.Add("Code", "Code ini sudah ada");
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

        public Account VIsLeaf(Account account)
        {
            if (!account.IsLeaf)
            {
                account.Errors.Add("Generic", "Account ini memiliki Anak/Detail/Leaf");
            }
            return account;
        }

        public Account VIsNotLegacy(Account account)
        {
            if (account.IsLegacy)
            {
                account.Errors.Add("Generic", "Legacy Account tidak boleh diubah/hapus");
            }
            return account;
        }

        public Account VIsNotCashBankAccount(Account account)
        {
            if (account.IsCashBankAccount)
            {
                account.Errors.Add("Generic", "CashBank Account tidak boleh diubah/hapus");
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
                if (account.ParentId == null || account.ParentId.GetValueOrDefault() <= 0)
                {
                    account.Errors.Add("Parent", "Tidak valid");
                }
                else
                {
                    Account parent = _accountService.GetObjectById((int)account.ParentId);
                    if (parent == null)
                    {
                        account.Errors.Add("Parent", "Tidak ada");
                    }
                    else if (parent.IsLeaf && parent.IsUsedBySystem)
                    {
                        account.Errors.Add("Parent", "Tidak boleh ada account anak pada account yang digunakan oleh System");
                    }
                }
            }
            return account;
        }

        public Account VCreateObject(Account account, IAccountService _accountService)
        {
            //VHasCashBank(account, _cashBankService);
            //if (!isValid(account)) { return account; }
            VHasValidCode(account, _accountService);
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
            VIsNotLegacy(account);
            if (!isValid(account)) { return account; }
            VIsNotCashBankAccount(account);
            if (!isValid(account)) { return account; }
            VCreateObject(account, _accountService);
            return account;
        }

        public Account VUpdateObjectForCashBank(Account account, IAccountService _accountService)
        {
            VIsNotLegacy(account);
            if (!isValid(account)) { return account; }
            VCreateObject(account, _accountService);
            return account;
        }

        public Account VUpdateObjectForLegacy(Account account, IAccountService _accountService)
        {
            VIsNotCashBankAccount(account);
            if (!isValid(account)) { return account; }
            VCreateObject(account, _accountService);
            return account;
        }

        public Account VDeleteObject(Account account)
        {
            VIsNotLegacy(account);
            if (!isValid(account)) { return account; }
            VIsNotCashBankAccount(account);
            if (!isValid(account)) { return account; }
            VIsLeaf(account);
            return account;
        }

        public Account VDeleteObjectForCashBank(Account account)
        {
            VIsNotLegacy(account);
            if (!isValid(account)) { return account; }
            VIsLeaf(account);
            return account;
        }

        public Account VDeleteObjectForLegacy(Account account)
        {
            VIsNotCashBankAccount(account);
            if (!isValid(account)) { return account; }
            VIsLeaf(account);
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

        public bool ValidUpdateObjectForCashBank(Account account, IAccountService _accountService)
        {
            VUpdateObjectForCashBank(account, _accountService);
            return isValid(account);
        }

        public bool ValidUpdateObjectForLegacy(Account account, IAccountService _accountService)
        {
            VUpdateObjectForLegacy(account, _accountService);
            return isValid(account);
        }

        public bool ValidDeleteObject(Account account)
        {
            account.Errors.Clear();
            VDeleteObject(account);
            return isValid(account);
        }

        public bool ValidDeleteObjectForCashBank(Account account)
        {
            account.Errors.Clear();
            VDeleteObjectForCashBank(account);
            return isValid(account);
        }

        public bool ValidDeleteObjectForLegacy(Account account)
        {
            account.Errors.Clear();
            VDeleteObjectForLegacy(account);
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
