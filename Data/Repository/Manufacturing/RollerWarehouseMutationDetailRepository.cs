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
    public class RollerWarehouseMutationDetailRepository : EfRepository<RollerWarehouseMutationDetail>, IRollerWarehouseMutationDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RollerWarehouseMutationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<RollerWarehouseMutationDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<RollerWarehouseMutationDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<RollerWarehouseMutationDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public IList<RollerWarehouseMutationDetail> GetObjectsByRollerWarehouseMutationId(int rollerWarehouseMutationId)
        {
            return FindAll(x => x.RollerWarehouseMutationId == rollerWarehouseMutationId && !x.IsDeleted).ToList();
        }

        public RollerWarehouseMutationDetail GetObjectByCoreIdentificationDetailId(int coreIdentificationDetailId)
        {
            return Find(x => x.CoreIdentificationDetailId == coreIdentificationDetailId && !x.IsDeleted);
        }

        public RollerWarehouseMutationDetail GetObjectById(int Id)
        {
            RollerWarehouseMutationDetail rollerWarehouseMutationDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (rollerWarehouseMutationDetail != null) { rollerWarehouseMutationDetail.Errors = new Dictionary<string, string>(); }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail CreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.RollerWarehouseMutations
                              where obj.Id == rollerWarehouseMutationDetail.RollerWarehouseMutationId
                              select obj.Code).FirstOrDefault();
            }
            if (rollerWarehouseMutationDetail.Code == null || rollerWarehouseMutationDetail.Code.Trim() == "")
            rollerWarehouseMutationDetail.Code = SetObjectCode(ParentCode);
            rollerWarehouseMutationDetail.IsConfirmed = false;
            rollerWarehouseMutationDetail.IsDeleted = false;
            rollerWarehouseMutationDetail.CreatedAt = DateTime.Now;
            return Create(rollerWarehouseMutationDetail);
        }

        public RollerWarehouseMutationDetail UpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            rollerWarehouseMutationDetail.UpdatedAt = DateTime.Now;
            Update(rollerWarehouseMutationDetail);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail SoftDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            rollerWarehouseMutationDetail.IsDeleted = true;
            rollerWarehouseMutationDetail.DeletedAt = DateTime.Now;
            Update(rollerWarehouseMutationDetail);
            return rollerWarehouseMutationDetail;
        }

        public bool DeleteObject(int Id)
        {
            RollerWarehouseMutationDetail rollerWarehouseMutationDetail =  Find(x => x.Id == Id);
            return (Delete(rollerWarehouseMutationDetail) == 1) ? true : false;
        }

        public RollerWarehouseMutationDetail ConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            rollerWarehouseMutationDetail.IsConfirmed = true;
            Update(rollerWarehouseMutationDetail);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail UnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            rollerWarehouseMutationDetail.IsConfirmed = false;
            rollerWarehouseMutationDetail.ConfirmationDate = null;
            UpdateObject(rollerWarehouseMutationDetail);
            return rollerWarehouseMutationDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + totalnumberinthemonth;
            return Code;
        } 
    }
}