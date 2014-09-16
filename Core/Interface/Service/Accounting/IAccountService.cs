using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IAccountService
    {
        IQueryable<Account> GetQueryable();
        IAccountValidator GetValidator();
        IList<Account> GetAll();
        IList<Account> GetLeafObjects();
        IList<Account> GetLegacyObjects();
        Account GetObjectById(int Id);
        Account GetObjectByLegacyCode(string LegacyCode);
        Account GetObjectByIsLegacy(bool IsLegacy);
        Account CreateObject(Account account, IAccountService _accountService);
        Account CreateLegacyObject(Account account, IAccountService _accountService);
        Account CreateCashBankAccount(Account account, IAccountService _accountService);
        Account UpdateObject(Account account, IAccountService _accountService);
        Account SoftDeleteObject(Account account);
        bool DeleteObject(int Id);
    }
}