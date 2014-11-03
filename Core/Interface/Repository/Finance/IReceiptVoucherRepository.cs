using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IReceiptVoucherRepository : IRepository<ReceiptVoucher>
    {
        IQueryable<ReceiptVoucher> GetQueryable();
        IList<ReceiptVoucher> GetAll();
        IList<ReceiptVoucher> GetAllByMonthCreated();
        ReceiptVoucher GetObjectById(int Id);
        IList<ReceiptVoucher> GetObjectsByCashBankId(int cashBankId);
        IList<ReceiptVoucher> GetObjectsByContactId(int contactId);
        ReceiptVoucher CreateObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher UpdateObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher SoftDeleteObject(ReceiptVoucher receiptVoucher);
        bool DeleteObject(int Id);
        ReceiptVoucher ConfirmObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher UnconfirmObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher ReconcileObject(ReceiptVoucher receiptVoucher);
        ReceiptVoucher UnreconcileObject(ReceiptVoucher receiptVoucher);
        string SetObjectCode();
    }
}