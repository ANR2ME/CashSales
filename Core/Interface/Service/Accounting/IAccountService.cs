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
        IList<Account> GetLeafObjectsById(int? Id);
        IList<Account> GetLegacyObjects();
        Account GetObjectById(int Id);
        Account GetObjectByLegacyCode(string LegacyCode);
        Account GetObjectByNameAndLegacyCode(string LegacyCode, string Name);
        Account GetObjectByNameAndParentLegacyCode(string ParentLegacyCode, string Name);
        Account GetObjectByIsLegacy(bool IsLegacy);
        Account CreateObject(Account account);
        Account CreateLegacyObject(Account account);
        Account FindOrCreateLegacyObject(Account account);
        Account CreateCashBankAccount(Account account);
        Account UpdateObject(Account account, int? OldParentId);
        Account UpdateObjectForCashBank(Account account, int? OldParentId);
        Account UpdateObjectForLegacy(Account account, int? OldParentId);
        Account SoftDeleteObject(Account account);
        Account SoftDeleteObjectForCashBank(Account account);
        Account SoftDeleteObjectForLegacy(Account account);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Account account);
        string GetGroupName(int Group);
    }
}