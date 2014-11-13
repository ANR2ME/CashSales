using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPaymentVoucherDetailValidator
    {
        PaymentVoucherDetail VHasPaymentVoucher(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService);
        PaymentVoucherDetail VHasPayable(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService);
        PaymentVoucherDetail VHasBeenConfirmed(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VHasNotBeenConfirmed(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VHasNotBeenDeleted(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VPayableHasNotBeenCompleted(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService);
        PaymentVoucherDetail VNonNegativeAmount(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VAmountLessOrEqualPayable(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucherDetail VAmountLessOrEqualPaymentVoucher(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucherDetail VUniquePayableId(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);

        PaymentVoucherDetail VCreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        PaymentVoucherDetail VUpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        PaymentVoucherDetail VDeleteObject(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VHasConfirmationDate(PaymentVoucherDetail paymentVoucherDetail);
        PaymentVoucherDetail VConfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        PaymentVoucherDetail VUnconfirmObject(PaymentVoucherDetail paymentVoucherDetail);
        bool ValidCreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidUpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService);
        bool ValidDeleteObject(PaymentVoucherDetail paymentVoucherDetail);
        bool ValidConfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool ValidUnconfirmObject(PaymentVoucherDetail paymentVoucherDetail);
        bool isValid(PaymentVoucherDetail paymentVoucherDetail);
        string PrintError(PaymentVoucherDetail paymentVoucherDetail);
    }
}
