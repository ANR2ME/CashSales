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
    public class ClosingRepository : EfRepository<Closing>, IClosingRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ClosingRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Closing> GetQueryable()
        {
            return FindAll(/*x => !x.IsDeleted*/);
        }

        public IList<Closing> GetAll()
        {
            return FindAll(/*x => !x.IsDeleted*/).ToList();
        }

        public Closing GetObjectById(int Id)
        {
            Closing closing = Find(x => x.Id == Id /*&& !x.IsDeleted*/);
            if (closing != null) { closing.Errors = new Dictionary<string, string>(); }
            return closing;
        }

        public Closing GetObjectByPeriodAndYear(int Period, int YearPeriod)
        {
            Closing closing = Find(x => x.Period == Period && x.YearPeriod == YearPeriod);
            if (closing != null) { closing.Errors = new Dictionary<string, string>(); }
            return closing;
        }

        public Closing CreateObject(Closing closing)
        {
            //closing.IsDeleted = false;
            closing.CreatedAt = DateTime.Now;
            return Create(closing);
        }

        public Closing CloseObject(Closing closing)
        {
            closing.IsClosed = true;    
            Update(closing);
            return closing;
        }

        public Closing OpenObject(Closing closing)
        {
            closing.IsClosed = false;
            closing.ClosedAt = null;
            Update(closing);
            return closing;
        }

        /*public Closing SoftDeleteObject(Closing closing)
        {
            closing.IsDeleted = true;
            closing.DeletedAt = DateTime.Now;
            Update(closing);
            return closing;
        }*/

        public bool DeleteObject(int Id)
        {
            Closing closing = Find(x => x.Id == Id);
            return (Delete(closing) == 1) ? true : false;
        }

    }
}