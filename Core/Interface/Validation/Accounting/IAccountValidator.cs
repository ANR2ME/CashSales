using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IAccountValidator
    {
        //Account VHasCashBank(Account account, ICashBankService _cashBankService);
        Account VHasCode(Account account);
        Account VHasName(Account account);
        Account VIsValidGroup(Account account);
        Account VIsValidLevel(Account account);
        Account VIsValidParent(Account account, IAccountService _accountService);

        Account VCreateObject(Account account, IAccountService _accountService);
        Account VUpdateObject(Account account, IAccountService _accountService);
        Account VDeleteObject(Account account);
        bool ValidCreateObject(Account account, IAccountService _accountService);
        bool ValidUpdateObject(Account account, IAccountService _accountService);
        bool ValidDeleteObject(Account account);
        bool isValid(Account account);
        string PrintError(Account account);
    }
}
