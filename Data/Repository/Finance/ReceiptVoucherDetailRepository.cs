using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class ReceiptVoucherDetailRepository : EfRepository<ReceiptVoucherDetail>, IReceiptVoucherDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ReceiptVoucherDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ReceiptVoucherDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<ReceiptVoucherDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<ReceiptVoucherDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IQueryable<ReceiptVoucherDetail> GetQueryableObjectsByReceiptVoucherId(int receiptVoucherId)
        {
            return FindAll(pvd => pvd.ReceiptVoucherId == receiptVoucherId && !pvd.IsDeleted);
        }

        public IList<ReceiptVoucherDetail> GetObjectsByReceiptVoucherId(int receiptVoucherId)
        {
            return FindAll(pvd => pvd.ReceiptVoucherId == receiptVoucherId && !pvd.IsDeleted).ToList();
        }

        public IQueryable<ReceiptVoucherDetail> GetQueryableObjectsByReceivableId(int receivableId)
        {
            return FindAll(pvd => pvd.ReceivableId == receivableId && !pvd.IsDeleted);
        }

        public IList<ReceiptVoucherDetail> GetObjectsByReceivableId(int receivableId)
        {
            return FindAll(pvd => pvd.ReceivableId == receivableId && !pvd.IsDeleted).ToList();
        }

        public ReceiptVoucherDetail GetObjectById(int Id)
        {
            ReceiptVoucherDetail detail = Find(pvd => pvd.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public ReceiptVoucherDetail CreateObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.ReceiptVouchers
                              where obj.Id == receiptVoucherDetail.ReceiptVoucherId
                              select obj.Code).FirstOrDefault();
            }
            receiptVoucherDetail.Code = SetObjectCode(ParentCode);
            receiptVoucherDetail.IsConfirmed = false;
            receiptVoucherDetail.IsDeleted = false;
            receiptVoucherDetail.CreatedAt = DateTime.Now;
            return Create(receiptVoucherDetail);
        }

        public ReceiptVoucherDetail UpdateObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.UpdatedAt = DateTime.Now;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail SoftDeleteObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsDeleted = true;
            receiptVoucherDetail.DeletedAt = DateTime.Now;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public bool DeleteObject(int Id)
        {
            ReceiptVoucherDetail pvd = Find(x => x.Id == Id);
            return (Delete(pvd) == 1) ? true : false;
        }

        public ReceiptVoucherDetail ConfirmObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsConfirmed = true;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail UnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsConfirmed = false;
            receiptVoucherDetail.ConfirmationDate = null;
            UpdateObject(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}