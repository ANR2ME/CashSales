using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class CashBankMutationRepository : EfRepository<CashBankMutation>, ICashBankMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CashBankMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashBankMutation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CashBankMutation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CashBankMutation> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public CashBankMutation GetObjectById(int Id)
        {
            CashBankMutation cashBankMutation = Find(x => x.Id == Id && !x.IsDeleted);
            if (cashBankMutation != null) { cashBankMutation.Errors = new Dictionary<string, string>(); }
            return cashBankMutation;
        }

        public CashBank GetSourceCashBank(CashBankMutation cashBankMutation)
        {
            using (var db = GetContext())
            {
                CashBank sourceCashBank =
                    (from obj in db.CashBanks
                     where obj.Id == cashBankMutation.SourceCashBankId && !obj.IsDeleted
                     select obj).First();
                return sourceCashBank;
            }
        }

        public CashBank GetTargetCashBank(CashBankMutation cashBankMutation)
        {
            using (var db = GetContext())
            {
                CashBank targetCashBank =
                    (from obj in db.CashBanks
                     where obj.Id == cashBankMutation.TargetCashBankId && !obj.IsDeleted
                     select obj).First();
                return targetCashBank;
            }
        }

        public CashBankMutation CreateObject(CashBankMutation cashBankMutation)
        {
            cashBankMutation.Code = SetObjectCode();
            cashBankMutation.IsConfirmed = false;
            cashBankMutation.IsDeleted = false;
            cashBankMutation.CreatedAt = DateTime.Now;
            return Create(cashBankMutation);
        }

        public CashBankMutation UpdateObject(CashBankMutation cashBankMutation)
        {
            cashBankMutation.UpdatedAt = DateTime.Now;
            Update(cashBankMutation);
            return cashBankMutation;
        }

        public CashBankMutation SoftDeleteObject(CashBankMutation cashBankMutation)
        {
            cashBankMutation.IsDeleted = true;
            cashBankMutation.DeletedAt = DateTime.Now;
            Update(cashBankMutation);
            return cashBankMutation;
        }

        public CashBankMutation ConfirmObject(CashBankMutation cashBankMutation)
        {
            cashBankMutation.IsConfirmed = true;
            Update(cashBankMutation);
            return cashBankMutation;
        }

        public CashBankMutation UnconfirmObject(CashBankMutation cashBankMutation)
        {
            cashBankMutation.IsConfirmed = false;
            cashBankMutation.ConfirmationDate = null;
            Update(cashBankMutation);
            return cashBankMutation;
        }

        public bool DeleteObject(int Id)
        {
            CashBankMutation cashBankMutation =  Find(x => x.Id == Id);
            return (Delete(cashBankMutation) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }

    }
}