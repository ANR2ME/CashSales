using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IUserMenuRepository : IRepository<UserMenu>
    {
        IQueryable<UserMenu> GetQueryable();
        IList<UserMenu> GetAll();
        UserMenu GetObjectById(int Id);
        UserMenu GetObjectByNameAndGroupName(string Name, string GroupName);
        UserMenu CreateObject(UserMenu userMenu);
        UserMenu SoftDeleteObject(UserMenu userMenu);
        bool DeleteObject(int Id);
    }
}