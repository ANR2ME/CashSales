using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IUserMenuService
    {
        IQueryable<UserMenu> GetQueryable();
        IUserMenuValidator GetValidator();
        IList<UserMenu> GetAll();
        UserMenu GetObjectById(int Id);
        UserMenu GetObjectByNameAndGroupName(string Name, string GroupName);
        UserMenu CreateObject(UserMenu userMenu);
        UserMenu CreateObject(string Name, string GroupName);
        UserMenu SoftDeleteObject(UserMenu userMenu);
        bool DeleteObject(int Id);
    }
}