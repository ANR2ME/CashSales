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
    public class CashBankAdjustmentRepository : EfRepository<CashBankAdjustment>, ICashBankAdjustmentRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CashBankAdjustmentRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashBankAdjustment> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CashBankAdjustment> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CashBankAdjustment> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<CashBankAdjustment> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public CashBankAdjustment GetObjectById(int Id)
        {
            CashBankAdjustment cashBankAdjustment = Find(x => x.Id == Id && !x.IsDeleted);
            if (cashBankAdjustment != null) { cashBankAdjustment.Errors = new Dictionary<string, string>(); }
            return cashBankAdjustment;
        }

        public CashBankAdjustment CreateObject(CashBankAdjustment cashBankAdjustment)
        {
            if (cashBankAdjustment.Code == null || cashBankAdjustment.Code.Trim() == "")
            cashBankAdjustment.Code = SetObjectCode();
            cashBankAdjustment.IsDeleted = false;
            cashBankAdjustment.IsConfirmed = false;
            cashBankAdjustment.CreatedAt = DateTime.Now;
            return Create(cashBankAdjustment);
        }

        public CashBankAdjustment UpdateObject(CashBankAdjustment cashBankAdjustment)
        {
            cashBankAdjustment.UpdatedAt = DateTime.Now;
            Update(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public CashBankAdjustment SoftDeleteObject(CashBankAdjustment cashBankAdjustment)
        {
            cashBankAdjustment.IsDeleted = true;
            cashBankAdjustment.DeletedAt = DateTime.Now;
            Update(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public CashBankAdjustment ConfirmObject(CashBankAdjustment cashBankAdjustment)
        {
            cashBankAdjustment.IsConfirmed = true;
            Update(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public CashBankAdjustment UnconfirmObject(CashBankAdjustment cashBankAdjustment)
        {
            cashBankAdjustment.IsConfirmed = false;
            cashBankAdjustment.ConfirmationDate = null;
            cashBankAdjustment.UpdatedAt = DateTime.Now;
            Update(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public bool DeleteObject(int Id)
        {
            CashBankAdjustment cashBankAdjustment = Find(x => x.Id == Id);
            return (Delete(cashBankAdjustment) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}