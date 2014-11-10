using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPaymentVoucherDetailService
    {
        IQueryable<PaymentVoucherDetail> GetQueryable();
        IList<PaymentVoucherDetail> GetAll();
        IPaymentVoucherDetailValidator GetValidator();
        IQueryable<PaymentVoucherDetail> GetQueryableObjectsByPaymentVoucherId(int paymentVoucherId);
        IList<PaymentVoucherDetail> GetObjectsByPaymentVoucherId(int paymentVoucherId);
        IQueryable<PaymentVoucherDetail> GetQueryableObjectsByPayableId(int payableId);
        IList<PaymentVoucherDetail> GetObjectsByPayableId(int payableId);
        PaymentVoucherDetail GetObjectById(int Id);        
        PaymentVoucherDetail CreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService,
                                          ICashBankService _cashBankService, IPayableService _payableService);
        PaymentVoucherDetail CreateObject(int paymentVoucherId, int payableId, decimal amount, string description, string Code,
                                          IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService, IPayableService _payableService);
        PaymentVoucherDetail UpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService,
                                          ICashBankService _cashBankService, IPayableService _payableService);
        PaymentVoucherDetail SoftDeleteObject(PaymentVoucherDetail paymentVoucherDetail);
        bool DeleteObject(int Id);
        PaymentVoucherDetail ConfirmObject(PaymentVoucherDetail paymentVoucherDetail, DateTime ConfirmationDate, IPaymentVoucherService _paymentVoucherService, IPayableService _payableService);
        PaymentVoucherDetail UnconfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPayableService _payableService);
        decimal CalcTotalAmount(int PaymentVoucherId);
    }
}