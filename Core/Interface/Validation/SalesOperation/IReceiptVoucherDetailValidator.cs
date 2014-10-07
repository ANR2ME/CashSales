using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IReceiptVoucherDetailValidator
    {
        ReceiptVoucherDetail VHasReceiptVoucher(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService);
        ReceiptVoucherDetail VHasReceivable(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService);
        ReceiptVoucherDetail VHasBeenConfirmed(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VHasNotBeenConfirmed(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VHasNotBeenDeleted(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VReceivableHasNotBeenCompleted(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService);
        ReceiptVoucherDetail VNonNegativeAmount(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VAmountLessOrEqualReceivable(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucherDetail VUniqueReceivableId(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);

        ReceiptVoucherDetail VCreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        ReceiptVoucherDetail VUpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        ReceiptVoucherDetail VDeleteObject(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VHasConfirmationDate(ReceiptVoucherDetail receiptVoucherDetail);
        ReceiptVoucherDetail VConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucherDetail VUnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail);
        bool ValidCreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidUpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService);
        bool ValidDeleteObject(ReceiptVoucherDetail receiptVoucherDetail);
        bool ValidConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidUnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail);
        bool isValid(ReceiptVoucherDetail receiptVoucherDetail);
        string PrintError(ReceiptVoucherDetail receiptVoucherDetail);
    }
}
