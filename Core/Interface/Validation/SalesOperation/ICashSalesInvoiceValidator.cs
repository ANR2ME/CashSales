using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICashSalesInvoiceValidator
    {
        CashSalesInvoice VHasSalesDate(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VHasDueDate(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VHasConfirmationDate(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsValidDiscount(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsValidTax(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VHasWarehouse(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService);
        CashSalesInvoice VHasNoReceiptVoucherDetails(CashSalesInvoice cashSalesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        CashSalesInvoice VHasNoCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesInvoice VHasCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesInvoice VHasNoCashSalesReturns(CashSalesInvoice cashSalesInvoice, ICashSalesReturnService _cashSalesReturnService);
        CashSalesInvoice VIsConfirmableCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                                          ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        CashSalesInvoice VIsUnconfirmableCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesInvoice VIsNotDeleted(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsNotPaid(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsPaid(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsNotConfirmed(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsConfirmed(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsValidAmountPaid(CashSalesInvoice cashSalesInvoice);
        //CashSalesInvoice VIsValidFullPayment(CashSalesInvoice cashSalesInvoice);
        //CashSalesInvoice VTotalPaymentIsEqualOrLessThanTotal(CashSalesInvoice cashSalesInvoice);

        CashSalesInvoice VHasCashBank(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService);

        //CashSalesInvoice VIsCashBankTypeNotBank(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService);
        CashSalesInvoice VGeneralLedgerPostingHasNotBeenClosed(CashSalesInvoice cashSalesInvoice, IClosingService _closingService, int CaseConfirmUnconfirm);

        CashSalesInvoice VConfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesInvoiceService _cashSalesInvoiceService,
                                        IWarehouseItemService _warehouseItemService, IContactService _contactService, ICashBankService _cashBankService, IItemService _itemService, IClosingService _closingService);
        CashSalesInvoice VUnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                          IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService, IClosingService _closingService);
        CashSalesInvoice VPaidObject(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService, ICashSalesReturnService _cashSalesReturnService,
                                     IClosingService _closingService);
        CashSalesInvoice VUnpaidObject(CashSalesInvoice cashSalesInvoice, ICashSalesReturnService _cashSalesReturnService, IClosingService _closingService);

        CashSalesInvoice VCreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService, ICashBankService _cashBankService);
        CashSalesInvoice VUpdateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService, ICashBankService _cashBankService);
        CashSalesInvoice VDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);

        bool ValidConfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesInvoiceService _cashSalesInvoiceService,
                                IWarehouseItemService _warehouseItemService, IContactService _contactService, ICashBankService _cashBankService, IItemService _itemService, IClosingService _closingService);
        bool ValidUnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                  IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService, IClosingService _closingService);
        bool ValidPaidObject(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService, ICashSalesReturnService _cashSalesReturnService,
                             IClosingService _closingService);
        bool ValidUnpaidObject(CashSalesInvoice cashSalesInvoice, ICashSalesReturnService _cashSalesReturnService, IClosingService _closingService);

        bool ValidCreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService, ICashBankService _cashBankService);
        bool ValidUpdateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService, ICashBankService _cashBankService);
        bool ValidDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        bool isValid(CashSalesInvoice cashSalesInvoice);
        string PrintError(CashSalesInvoice cashSalesInvoice);
    }
}
