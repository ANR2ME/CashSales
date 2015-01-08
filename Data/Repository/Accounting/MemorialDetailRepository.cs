using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class MemorialDetailRepository : EfRepository<MemorialDetail>, IMemorialDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public MemorialDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<MemorialDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<MemorialDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<MemorialDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IList<MemorialDetail> GetObjectsByMemorialId(int memorialId)
        {
            return FindAll(x => x.MemorialId == memorialId && !x.IsDeleted).ToList();
        }

        public MemorialDetail GetObjectById(int Id)
        {
            MemorialDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public MemorialDetail CreateObject(MemorialDetail memorialDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.Memorials
                              where obj.Id == memorialDetail.MemorialId
                              select obj.Code).FirstOrDefault();
            }
            if (memorialDetail.Code == null || memorialDetail.Code.Trim() == "")
            memorialDetail.Code = SetObjectCode(ParentCode);
            memorialDetail.IsConfirmed = false;
            memorialDetail.IsDeleted = false;
            memorialDetail.CreatedAt = DateTime.Now;
            return Create(memorialDetail);
        }

        public MemorialDetail UpdateObject(MemorialDetail memorialDetail)
        {
            memorialDetail.UpdatedAt = DateTime.Now;
            Update(memorialDetail);
            return memorialDetail;
        }

        public MemorialDetail SoftDeleteObject(MemorialDetail memorialDetail)
        {
            memorialDetail.IsDeleted = true;
            memorialDetail.DeletedAt = DateTime.Now;
            Update(memorialDetail);
            return memorialDetail;
        }

        public bool DeleteObject(int Id)
        {
            MemorialDetail memorialDetail = Find(x => x.Id == Id);
            return (Delete(memorialDetail) == 1) ? true : false;
        }

        public MemorialDetail ConfirmObject(MemorialDetail memorialDetail)
        {
            memorialDetail.IsConfirmed = true;
            Update(memorialDetail);
            return memorialDetail;
        }

        public MemorialDetail UnconfirmObject(MemorialDetail memorialDetail)
        {
            memorialDetail.IsConfirmed = false;
            memorialDetail.ConfirmationDate = null;
            UpdateObject(memorialDetail);
            return memorialDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}