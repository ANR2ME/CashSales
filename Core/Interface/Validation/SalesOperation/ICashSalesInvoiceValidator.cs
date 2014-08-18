﻿using System;
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
        CashSalesInvoice VIsConfirmableCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                                          ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService);
        CashSalesInvoice VIsUnconfirmableCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesInvoice VIsNotDeleted(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsNotPaid(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsPaid(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsNotConfirmed(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsConfirmed(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsValidAmountPaid(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VIsValidFullPayment(CashSalesInvoice cashSalesInvoice);
        CashSalesInvoice VHasCashBank(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService);
        CashSalesInvoice VIsCashBankTypeNotBank(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService);

        CashSalesInvoice VConfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                 ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService);
        CashSalesInvoice VUnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                            IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        CashSalesInvoice VPaidObject(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        CashSalesInvoice VUnpaidObject(CashSalesInvoice cashSalesInvoice);

        CashSalesInvoice VCreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService);
        CashSalesInvoice VUpdateObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesInvoice VDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);

        bool ValidConfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                       ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService);
        bool ValidUnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                  IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidPaidObject(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService);
        bool ValidUnpaidObject(CashSalesInvoice cashSalesInvoice);

        bool ValidCreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService);
        bool ValidUpdateObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        bool ValidDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        bool isValid(CashSalesInvoice cashSalesInvoice);
        string PrintError(CashSalesInvoice cashSalesInvoice);
    }
}
