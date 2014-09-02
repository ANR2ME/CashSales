using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IUserAccessRepository : IRepository<UserAccess>
    {
        IQueryable<UserAccess> GetQueryable();
        IQueryable<UserAccess> GetQueryableObjectsByUserAccountId(int UserAccountId);
        IList<UserAccess> GetAll();
        IList<UserAccess> GetObjectsByUserAccountId(int UserAccountId);
        UserAccess GetObjectByUserAccountIdAndUserMenuId(int UserAccountId, int UserMenuId);
        UserAccess GetObjectById(int Id);
        UserAccess CreateObject(UserAccess userAccess);
        UserAccess UpdateObject(UserAccess userAccess);
        UserAccess SoftDeleteObject(UserAccess userAccess);
        bool DeleteObject(int Id);
    }
}