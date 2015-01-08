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
    public class ReceivableRepository : EfRepository<Receivable>, IReceivableRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ReceivableRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Receivable> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Receivable> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<Receivable> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IList<Receivable> GetObjectsByContactId(int contactId)
        {
            return FindAll(p => p.ContactId == contactId && !p.IsDeleted).ToList();
        }

        public IList<Receivable> GetObjectsByDueDate(DateTime fromDueDate, DateTime toDueDate)
        {
            return FindAll(x => !x.IsDeleted && x.DueDate >= fromDueDate && x.DueDate <= toDueDate).ToList();
        }

        public Receivable GetObjectBySource(string ReceivableSource, int ReceivableSourceId)
        {
            Receivable receivable = FindAll(p => p.ReceivableSource == ReceivableSource && p.ReceivableSourceId == ReceivableSourceId && !p.IsDeleted).FirstOrDefault();
            if (receivable != null) { receivable.Errors = new Dictionary<string, string>(); }
            return receivable;
        }

        public Receivable GetObjectById(int Id)
        {
            Receivable receivable = Find(p => p.Id == Id && !p.IsDeleted);
            if (receivable != null) { receivable.Errors = new Dictionary<string, string>(); }
            return receivable;
        }

        public Receivable CreateObject(Receivable receivable)
        {
            if (receivable.Code == null || receivable.Code.Trim() == "")
            receivable.Code = SetObjectCode();
            receivable.PendingClearanceAmount = 0;
            receivable.IsCompleted = false;
            receivable.IsDeleted = false;
            receivable.CreatedAt = DateTime.Now;
            return Create(receivable);
        }

        public Receivable UpdateObject(Receivable receivable)
        {
            receivable.UpdatedAt = DateTime.Now;
            Update(receivable);
            return receivable;
        }

        public Receivable SoftDeleteObject(Receivable receivable)
        {
            receivable.IsDeleted = true;
            receivable.DeletedAt = DateTime.Now;
            Update(receivable);
            return receivable;
        }

        public bool DeleteObject(int Id)
        {
            Receivable receivable = Find(x => x.Id == Id);
            return (Delete(receivable) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}