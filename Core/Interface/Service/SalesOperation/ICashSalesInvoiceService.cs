using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ICashSalesInvoiceService
    {
        IQueryable<CashSalesInvoice> GetQueryable();
        ICashSalesInvoiceValidator GetValidator();
        ICashSalesInvoiceRepository GetRepository();
        IList<CashSalesInvoice> GetAll();
        CashSalesInvoice GetObjectById(int Id);
        CashSalesInvoice CreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService, ICashBankService _cashBankService);
        CashSalesInvoice UpdateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService, ICashBankService _cashBankService);
        CashSalesInvoice SoftDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesInvoice ConfirmObject(CashSalesInvoice cashSalesInvoice, DateTime ConfirmationDate, decimal Discount, decimal Tax,
                                                ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IContactService _contactService,
                                                IPriceMutationService _priceMutationService, IReceivableService _receivableService,
                                                ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService,
                                                IStockMutationService _stockMutationService, ICashBankService _cashBankService);
        CashSalesInvoice UnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                           IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                           IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService,
                                           IBarringService _barringService, IStockMutationService _stockMutationService);
        CashSalesInvoice PaidObject(CashSalesInvoice cashSalesInvoice, decimal AmountPaid, decimal Allowance, ICashBankService _cashBankService, IReceivableService _receivableService,
                                      IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                      IContactService _contactService, ICashMutationService _cashMutationService, ICashSalesReturnService _cashSalesReturnService);
        CashSalesInvoice UnpaidObject(CashSalesInvoice cashSalesInvoice, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                        ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService, ICashSalesReturnService _cashSalesReturnService);
        bool DeleteObject(int Id);
    }
}
