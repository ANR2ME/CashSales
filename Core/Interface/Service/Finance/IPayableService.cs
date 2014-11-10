using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IPayableService
    {
        IQueryable<Payable> GetQueryable();
        IPayableValidator GetValidator();
        IList<Payable> GetAll();
        IList<Payable> GetObjectsByContactId(int contactId);
        IList<Payable> GetObjectsByDueDate(DateTime fromDueDate, DateTime toDueDate);
        Payable GetObjectBySource(string PayableSource, int PayableSourceId); 
        Payable GetObjectById(int Id);
        Payable GetObjectByCode(string Code);
        Payable CreateObject(Payable payable);
        Payable CreateObject(int contactId, string payableSource, int payableSourceId, string payableSourceCode, decimal amount, DateTime dueDate, string Code);
        Payable UpdateObject(Payable payable);
        Payable SoftDeleteObject(Payable payable, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool DeleteObject(int Id);
        decimal GetTotalRemainingAmountByDueDate(DateTime fromDueDate, DateTime toDueDate);
    }
}