using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IUserAccountService
    {
        IQueryable<UserAccount> GetQueryable();
        IUserAccountValidator GetValidator();
        IList<UserAccount> GetAll();
        UserAccount GetObjectById(int Id);
        UserAccount GetObjectByIsAdmin(bool IsAdmin);
        UserAccount GetObjectByUsername(string username);
        UserAccount IsLoginValid(string username, string password);
        UserAccount FindOrCreateSysAdmin();
        UserAccount CreateObject(UserAccount userAccount);
        UserAccount CreateObject(string username, string password, string name, string description, bool IsAdmin);
        UserAccount UpdateObject(UserAccount userAccount);
        UserAccount UpdateObjectPassword(UserAccount userAccount, string OldPassword, string NewPassword, string ConfirmPassword);
        UserAccount SoftDeleteObject(UserAccount userAccount, int LoggedId);
        bool DeleteObject(int Id);
        
    }
}