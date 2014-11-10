using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class MemorialRepository : EfRepository<Memorial>, IMemorialRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public MemorialRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Memorial> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Memorial> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<Memorial> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public Memorial GetObjectById(int Id)
        {
            Memorial memorial = Find(x => x.Id == Id && !x.IsDeleted);
            if (memorial != null) { memorial.Errors = new Dictionary<string, string>(); }
            return memorial;
        }

        public Memorial CreateObject(Memorial memorial)
        {
            if (memorial.Code == null || memorial.Code.Trim() == "")
            memorial.Code = SetObjectCode();
            memorial.IsDeleted = false;
            memorial.IsConfirmed = false;
            memorial.CreatedAt = DateTime.Now;
            return Create(memorial);
        }

        public Memorial UpdateObject(Memorial memorial)
        {
            memorial.UpdatedAt = DateTime.Now;
            Update(memorial);
            return memorial;
        }

        public Memorial SoftDeleteObject(Memorial memorial)
        {
            memorial.IsDeleted = true;
            memorial.DeletedAt = DateTime.Now;
            Update(memorial);
            return memorial;
        }

        public bool DeleteObject(int Id)
        {
            Memorial memorial = Find(x => x.Id == Id);
            return (Delete(memorial) == 1) ? true : false;
        }

        public Memorial ConfirmObject(Memorial memorial)
        {
            memorial.IsConfirmed = true;
            Update(memorial);
            return memorial;
        }

        public Memorial UnconfirmObject(Memorial memorial)
        {
            memorial.IsConfirmed = false;
            memorial.ConfirmationDate = null;
            UpdateObject(memorial);
            return memorial;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}