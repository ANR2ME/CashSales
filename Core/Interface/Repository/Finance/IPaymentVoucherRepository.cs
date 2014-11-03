using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPaymentVoucherRepository : IRepository<PaymentVoucher>
    {
        IQueryable<PaymentVoucher> GetQueryable();
        IList<PaymentVoucher> GetAll();
        IList<PaymentVoucher> GetAllByMonthCreated();
        PaymentVoucher GetObjectById(int Id);
        IList<PaymentVoucher> GetObjectsByCashBankId(int cashBankId);
        IList<PaymentVoucher> GetObjectsByContactId(int contactId);
        PaymentVoucher CreateObject(PaymentVoucher paymentVoucher);
        PaymentVoucher UpdateObject(PaymentVoucher paymentVoucher);
        PaymentVoucher SoftDeleteObject(PaymentVoucher paymentVoucher);
        bool DeleteObject(int Id);
        PaymentVoucher ConfirmObject(PaymentVoucher paymentVoucher);
        PaymentVoucher UnconfirmObject(PaymentVoucher paymentVoucher);
        PaymentVoucher ReconcileObject(PaymentVoucher paymentVoucher);
        PaymentVoucher UnreconcileObject(PaymentVoucher paymentVoucher);
        string SetObjectCode();
    }
}