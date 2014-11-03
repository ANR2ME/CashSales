using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;

namespace Data.Repository
{
    public class PayableRepository : EfRepository<Payable>, IPayableRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PayableRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Payable> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Payable> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<Payable> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<Payable> GetObjectsByContactId(int contactId)
        {
            return FindAll(p => p.ContactId == contactId && !p.IsDeleted).ToList();
        }

        public IList<Payable> GetObjectsByDueDate(DateTime fromDueDate, DateTime toDueDate)
        {
            return FindAll(x => !x.IsDeleted && x.DueDate >= fromDueDate && x.DueDate <= toDueDate).ToList();
        }

        public Payable GetObjectBySource(string PayableSource, int PayableSourceId)
        {
            Payable payable = FindAll(p => p.PayableSource == PayableSource && p.PayableSourceId == PayableSourceId && !p.IsDeleted).FirstOrDefault();
            if (payable != null) { payable.Errors = new Dictionary<string, string>(); }
            return payable;
        }

        public Payable GetObjectById(int Id)
        {
            Payable payable = Find(p => p.Id == Id && !p.IsDeleted);
            if (payable != null) { payable.Errors = new Dictionary<string, string>(); }
            return payable;
        }

        public Payable CreateObject(Payable payable)
        {
            payable.Code = SetObjectCode();
            payable.PendingClearanceAmount = 0;
            payable.IsCompleted = false;
            payable.IsDeleted = false;
            payable.CreatedAt = DateTime.Now;
            return Create(payable);
        }

        public Payable UpdateObject(Payable payable)
        {
            payable.UpdatedAt = DateTime.Now;
            Update(payable);
            return payable;
        }

        public Payable SoftDeleteObject(Payable payable)
        {
            payable.IsDeleted = true;
            payable.DeletedAt = DateTime.Now;
            Update(payable);
            return payable;
        }

        public bool DeleteObject(int Id)
        {
            Payable payable = Find(x => x.Id == Id);
            return (Delete(payable) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}