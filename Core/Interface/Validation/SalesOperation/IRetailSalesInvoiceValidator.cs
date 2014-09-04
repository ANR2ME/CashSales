using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IRetailSalesInvoiceValidator
    {
        RetailSalesInvoice VHasSalesDate(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VHasDueDate(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VHasConfirmationDate(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsValidDiscount(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsValidTax(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VHasWarehouse(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService);
        RetailSalesInvoice VHasContact(RetailSalesInvoice retailSalesInvoice, IContactService _contactService);
        RetailSalesInvoice VHasNoReceiptVoucherDetails(RetailSalesInvoice retailSalesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        RetailSalesInvoice VHasNoRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        RetailSalesInvoice VHasRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        RetailSalesInvoice VIsConfirmableRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService,
                                                                          IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService);
        RetailSalesInvoice VIsUnconfirmableRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        RetailSalesInvoice VIsNotDeleted(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsNotPaid(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsPaid(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsNotConfirmed(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsConfirmed(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsValidGBCH_No(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsValidGBCH_DueDate(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsValidAmountPaid(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VIsValidFullPayment(RetailSalesInvoice retailSalesInvoice);
        RetailSalesInvoice VHasCashBank(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService);
        RetailSalesInvoice VIsCashBankTypeBank(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService);

        RetailSalesInvoice VConfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService,
                                                 IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService);
        RetailSalesInvoice VUnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, 
                                            IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        RetailSalesInvoice VPaidObject(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        RetailSalesInvoice VUnpaidObject(RetailSalesInvoice retailSalesInvoice);

        RetailSalesInvoice VCreateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService);
        RetailSalesInvoice VUpdateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService);
        RetailSalesInvoice VDeleteObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);

        bool ValidConfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService,
                                       IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService);
        bool ValidUnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, 
                                  IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidPaidObject(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        bool ValidUnpaidObject(RetailSalesInvoice retailSalesInvoice);

        bool ValidCreateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService);
        bool ValidUpdateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService);
        bool ValidDeleteObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        bool isValid(RetailSalesInvoice retailSalesInvoice);
        string PrintError(RetailSalesInvoice retailSalesInvoice);
    }
}
