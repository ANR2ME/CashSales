using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PaymentVoucherRepository : EfRepository<PaymentVoucher>, IPaymentVoucherRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PaymentVoucherRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PaymentVoucher> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PaymentVoucher> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PaymentVoucher> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public PaymentVoucher GetObjectById(int Id)
        {
            PaymentVoucher paymentVoucher = Find(pv => pv.Id == Id && !pv.IsDeleted);
            if (paymentVoucher != null) { paymentVoucher.Errors = new Dictionary<string, string>(); }
            return paymentVoucher;
        }

        public IList<PaymentVoucher> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(pv => pv.CashBankId == cashBankId && !pv.IsDeleted).ToList();
        }

        public IList<PaymentVoucher> GetObjectsByContactId(int contactId)
        {
            return FindAll(pv => pv.ContactId == contactId && !pv.IsDeleted).ToList();
        }

        public PaymentVoucher CreateObject(PaymentVoucher paymentVoucher)
        {
            paymentVoucher.Code = SetObjectCode();
            paymentVoucher.IsDeleted = false;
            paymentVoucher.IsConfirmed = false;
            paymentVoucher.IsReconciled = false;
            paymentVoucher.CreatedAt = DateTime.Now;
            return Create(paymentVoucher);
        }

        public PaymentVoucher UpdateObject(PaymentVoucher paymentVoucher)
        {
            paymentVoucher.UpdatedAt = DateTime.Now;
            Update(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher SoftDeleteObject(PaymentVoucher paymentVoucher)
        {
            paymentVoucher.IsDeleted = true;
            paymentVoucher.DeletedAt = DateTime.Now;
            Update(paymentVoucher);
            return paymentVoucher;
        }

        public bool DeleteObject(int Id)
        {
            PaymentVoucher pv = Find(x => x.Id == Id);
            return (Delete(pv) == 1) ? true : false;
        }

        public PaymentVoucher ConfirmObject(PaymentVoucher paymentVoucher)
        {
            paymentVoucher.IsConfirmed = true;
            Update(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher UnconfirmObject(PaymentVoucher paymentVoucher)
        {
            paymentVoucher.IsConfirmed = false;
            paymentVoucher.ConfirmationDate = null;
            UpdateObject(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher ReconcileObject(PaymentVoucher paymentVoucher)
        {
            paymentVoucher.IsReconciled = true;
            Update(paymentVoucher);
            return paymentVoucher;
        }

        public PaymentVoucher UnreconcileObject(PaymentVoucher paymentVoucher)
        {
            paymentVoucher.IsReconciled = false;
            paymentVoucher.ReconciliationDate = null;
            UpdateObject(paymentVoucher);
            return paymentVoucher;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}