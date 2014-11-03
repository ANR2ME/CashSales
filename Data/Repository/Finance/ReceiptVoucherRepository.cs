using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class ReceiptVoucherRepository : EfRepository<ReceiptVoucher>, IReceiptVoucherRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ReceiptVoucherRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ReceiptVoucher> GetQueryable()
        {
            return FindAll(pv => !pv.IsDeleted);
        }

        public IList<ReceiptVoucher> GetAll()
        {
            return FindAll(pv => !pv.IsDeleted).ToList();
        }

        public IList<ReceiptVoucher> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public ReceiptVoucher GetObjectById(int Id)
        {
            ReceiptVoucher receiptVoucher = Find(pv => pv.Id == Id && !pv.IsDeleted);
            if (receiptVoucher != null) { receiptVoucher.Errors = new Dictionary<string, string>(); }
            return receiptVoucher;
        }

        public IList<ReceiptVoucher> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(pv => pv.CashBankId == cashBankId && !pv.IsDeleted).ToList();
        }

        public IList<ReceiptVoucher> GetObjectsByContactId(int contactId)
        {
            return FindAll(pv => pv.ContactId == contactId && !pv.IsDeleted).ToList();
        }

        public ReceiptVoucher CreateObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.Code = SetObjectCode();
            receiptVoucher.IsDeleted = false;
            receiptVoucher.IsConfirmed = false;
            receiptVoucher.IsReconciled = false;
            receiptVoucher.CreatedAt = DateTime.Now;
            return Create(receiptVoucher);
        }

        public ReceiptVoucher UpdateObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.UpdatedAt = DateTime.Now;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher SoftDeleteObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsDeleted = true;
            receiptVoucher.DeletedAt = DateTime.Now;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public bool DeleteObject(int Id)
        {
            ReceiptVoucher pv = Find(x => x.Id == Id);
            return (Delete(pv) == 1) ? true : false;
        }

        public ReceiptVoucher ConfirmObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsConfirmed = true;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher UnconfirmObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsConfirmed = false;
            receiptVoucher.ConfirmationDate = null;
            UpdateObject(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher ReconcileObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsReconciled = true;
            Update(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher UnreconcileObject(ReceiptVoucher receiptVoucher)
        {
            receiptVoucher.IsReconciled = false;
            receiptVoucher.ReconciliationDate = null;
            UpdateObject(receiptVoucher);
            return receiptVoucher;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}