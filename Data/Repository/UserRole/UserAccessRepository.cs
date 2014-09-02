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
    public class UserAccessRepository : EfRepository<UserAccess>, IUserAccessRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public UserAccessRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<UserAccess> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IQueryable<UserAccess> GetQueryableObjectsByUserAccountId(int UserAccountId)
        {
            return FindAll(x => !x.IsDeleted && x.UserAccountId == UserAccountId);
        }

        public IList<UserAccess> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<UserAccess> GetObjectsByUserAccountId(int UserAccountId)
        {
            return FindAll(x => !x.IsDeleted && x.UserAccountId == UserAccountId).ToList();
        }

        public UserAccess GetObjectById(int Id)
        {
            UserAccess userAccess = Find(x => x.Id == Id && !x.IsDeleted);
            if (userAccess != null) { userAccess.Errors = new Dictionary<string, string>(); }
            return userAccess;
        }

        public UserAccess GetObjectByUserAccountIdAndUserMenuId(int UserAccountId, int UserMenuId)
        {
            UserAccess userAccess = Find(x => x.UserAccountId == UserAccountId && x.UserMenuId == UserMenuId && !x.IsDeleted);
            if (userAccess != null) { userAccess.Errors = new Dictionary<string, string>(); }
            return userAccess;
        }

        public UserAccess CreateObject(UserAccess userAccess)
        {
            userAccess.IsDeleted = false;
            userAccess.CreatedAt = DateTime.Now;
            return Create(userAccess);
        }

        public UserAccess UpdateObject(UserAccess userAccess)
        {
            userAccess.UpdatedAt = DateTime.Now;
            Update(userAccess);
            return userAccess;
        }

        public UserAccess SoftDeleteObject(UserAccess userAccess)
        {
            userAccess.IsDeleted = true;
            userAccess.DeletedAt = DateTime.Now;
            Update(userAccess);
            return userAccess;
        }

        public bool DeleteObject(int Id)
        {
            UserAccess userAccess = Find(x => x.Id == Id);
            return (Delete(userAccess) == 1) ? true : false;
        }

    }
}