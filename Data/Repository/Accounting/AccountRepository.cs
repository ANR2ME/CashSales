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
    public class AccountRepository : EfRepository<Account>, IAccountRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public AccountRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Account> GetQueryable()
        {
            return FindAll();
        }

        public IList<Account> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<Account> GetObjectsByParentId(int ParentId)
        {
            return FindAll(x => x.ParentId == ParentId).ToList();
        }

        public IList<Account> GetObjectsByLevel(int Level)
        {
            return FindAll(x => x.Level == Level).ToList();
        }

        public Account GetObjectById(int Id)
        {
            Account account = Find(x => x.Id == Id && !x.IsDeleted);
            if (account != null) { account.Errors = new Dictionary<string, string>(); }
            return account;
        }

        public Account GetObjectByIsLegacy(bool IsLegacy)
        {
            Account account = Find(x => x.IsLegacy && !x.IsDeleted);
            if (account != null) { account.Errors = new Dictionary<string, string>(); }
            return account;
        }

        public Account CreateObject(Account account)
        {
            account.IsDeleted = false;
            account.CreatedAt = DateTime.Now;
            return Create(account);
        }

        public Account UpdateObject(Account account)
        {
            account.UpdatedAt = DateTime.Now;
            Update(account);
            return account;
        }

        public Account SoftDeleteObject(Account account)
        {
            account.IsDeleted = true;
            account.DeletedAt = DateTime.Now;
            Update(account);
            return account;
        }

        public bool DeleteObject(int Id)
        {
            Account account = Find(x => x.Id == Id);
            return (Delete(account) == 1) ? true : false;
        }

        /*
        public int SetObjectCode(Account account)
        {
            // level 1 - 1 digit
            // level 2 - 1 digit
            // level 3 - 2 digit
            // level 4 - 2 digit
            // level 5 - 3 digit
            if (account.Level == 1)
            {
                IList<Account> accounts = GetObjectsByLevel(account.Level);
                return accounts.Count() + 1;
            }
            else
            {
                IList<Account> accounts = GetObjectsByParentId((int) account.ParentId);
                int LastDigitCode = accounts.Count() + 1;
                int ParentDigitCode = GetObjectById((int)account.ParentId).Code;
                int Code = 0;
                switch(account.Level)
                {
                    case(1):
                    {    // 1
                         Code = LastDigitCode;
                         break;
                    }
                    case (2):
                    {   // 11
                        Code = (ParentDigitCode * 10) + LastDigitCode;
                        break;
                    }
                    case (3): // 1101
                    case (4): // 110101
                    {   
                        Code = (ParentDigitCode * 100) + LastDigitCode;
                        break;
                    }
                    case (5):
                    {   // 110101001
                        Code = (ParentDigitCode * 1000) + LastDigitCode;
                        break;
                    }
                }
                return Code;
            }
        }
         */
    }
}