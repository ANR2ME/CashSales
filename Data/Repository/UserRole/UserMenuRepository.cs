using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class UserMenuRepository : EfRepository<UserMenu>, IUserMenuRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public UserMenuRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<UserMenu> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<UserMenu> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public UserMenu GetObjectById(int Id)
        {
            UserMenu userMenu = Find(x => x.Id == Id && !x.IsDeleted);
            if (userMenu != null) { userMenu.Errors = new Dictionary<string, string>(); }
            return userMenu;
        }

        public UserMenu GetObjectByNameAndGroupName(string Name, string GroupName)
        {
            UserMenu userMenu = Find(x => x.Name == Name && x.GroupName == GroupName && !x.IsDeleted);
            if (userMenu != null) { userMenu.Errors = new Dictionary<string, string>(); }
            return userMenu;
        }

        public UserMenu CreateObject(UserMenu userMenu)
        {
            userMenu.IsDeleted = false;
            userMenu.CreatedAt = DateTime.Now;
            return Create(userMenu);
        }

        public UserMenu SoftDeleteObject(UserMenu userMenu)
        {
            userMenu.IsDeleted = true;
            userMenu.DeletedAt = DateTime.Now;
            Update(userMenu);
            return userMenu;
        }

        public bool DeleteObject(int Id)
        {
            UserMenu userMenu = Find(x => x.Id == Id);
            return (Delete(userMenu) == 1) ? true : false;
        }

    }
}