using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IReceiptVoucherDetailRepository : IRepository<ReceiptVoucherDetail>
    {
        IQueryable<ReceiptVoucherDetail> GetQueryable();
        IList<ReceiptVoucherDetail> GetAll();
        IList<ReceiptVoucherDetail> GetAllByMonthCreated();
        IQueryable<ReceiptVoucherDetail> GetQueryableObjectsByReceiptVoucherId(int receiptVoucherId);
        IList<ReceiptVoucherDetail> GetObjectsByReceiptVoucherId(int receiptVoucherId);
        IQueryable<ReceiptVoucherDetail> GetQueryableObjectsByReceivableId(int receivableId);
        IList<ReceiptVoucherDetail> GetObjectsByReceivableId(int receivableId);
        ReceiptVoucherDetail GetObjectById(int Id);
        ReceiptVoucherDetail CreateObject(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail UpdateObject(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail SoftDeleteObject(ReceiptVoucherDetail receiptVoucherDetail);
        bool DeleteObject(int Id);
        ReceiptVoucherDetail ConfirmObject(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail UnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail);
        string SetObjectCode(string ParentCode);
    }
}