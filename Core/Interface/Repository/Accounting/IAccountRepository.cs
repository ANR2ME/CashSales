using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IAccountRepository : IRepository<Account>
    {
        IQueryable<Account> GetQueryable();
        IList<Account> GetAll();
        Account GetObjectById(int Id);
        Account GetObjectByIsLegacy(bool IsLegacy);
        Account CreateObject(Account account);
        Account UpdateObject(Account account);
        Account SoftDeleteObject(Account account);
        bool DeleteObject(int Id);
    }
}