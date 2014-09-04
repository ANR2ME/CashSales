using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IUserAccessService
    {
        IUserAccessValidator GetValidator();
        IQueryable<UserAccess> GetQueryable();
        IQueryable<UserAccess> GetQueryableObjectsByUserAccountId(int UserAccountId);
        IList<UserAccess> GetAll();
        IList<UserAccess> GetObjectsByUserAccountId(int UserAccountId);
        UserAccess GetObjectByUserAccountIdAndUserMenuId(int UserAccountId, int UserMenuId);
        UserAccess GetObjectById(int Id);
        UserAccess CreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService);
        UserAccess UpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService);
        UserAccess SoftDeleteObject(UserAccess userAccess);
        bool DeleteObject(int Id);
        int CreateDefaultAccess(int UserAccountId, IUserMenuService _userMenuService, IUserAccountService _userAccountService);
    }
}