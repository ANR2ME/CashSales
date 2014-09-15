using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICashSalesReturnValidator
    {
        CashSalesReturn VHasReturnDate(CashSalesReturn cashSalesReturn);
        CashSalesReturn VHasConfirmationDate(CashSalesReturn cashSalesReturn);
        CashSalesReturn VIsValidAllowance(CashSalesReturn cashSalesReturn);
        CashSalesReturn VIsValidTotal(CashSalesReturn cashSalesReturn);
        CashSalesReturn VIsValidCashSalesInvoice(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService);
        CashSalesReturn VHasNoCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        CashSalesReturn VHasCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        CashSalesReturn VIsConfirmableCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService,
                                                             ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesReturn VIsUnconfirmableCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        CashSalesReturn VIsNotDeleted(CashSalesReturn cashSalesReturn);
        CashSalesReturn VIsNotPaid(CashSalesReturn cashSalesReturn);
        CashSalesReturn VIsPaid(CashSalesReturn cashSalesReturn);
        CashSalesReturn VIsNotConfirmed(CashSalesReturn cashSalesReturn);
        CashSalesReturn VIsConfirmed(CashSalesReturn cashSalesReturn);
        CashSalesReturn VHasCashBank(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService);
        //CashSalesReturn VIsCashBankTypeNotBank(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService);
        CashSalesReturn VHasCashSalesInvoice(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService);
        CashSalesReturn VHasNoPaymentVoucherDetails(CashSalesReturn cashSalesReturn, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);

        CashSalesReturn VConfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesReturnService _cashSalesReturnService,
                                              ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesReturn VUnconfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService,
                                                   IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        CashSalesReturn VPaidObject(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService);
        CashSalesReturn VUnpaidObject(CashSalesReturn cashSalesReturn);

        CashSalesReturn VCreateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService);
        CashSalesReturn VUpdateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService);
        CashSalesReturn VDeleteObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService);

        bool ValidConfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesReturnService _cashSalesReturnService,
                                ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        bool ValidUnconfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService,
                                  IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool ValidPaidObject(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService);
        bool ValidUnpaidObject(CashSalesReturn cashSalesReturn);

        bool ValidCreateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService);
        bool ValidUpdateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService);
        bool ValidDeleteObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        bool isValid(CashSalesReturn cashSalesReturn);
        string PrintError(CashSalesReturn cashSalesReturn);
    }
}
