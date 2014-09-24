using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IRetailSalesInvoiceService
    {
        IQueryable<RetailSalesInvoice> GetQueryable();
        IRetailSalesInvoiceValidator GetValidator();
        IRetailSalesInvoiceRepository GetRepository();
        IList<RetailSalesInvoice> GetAll();
        RetailSalesInvoice GetObjectById(int Id);
        RetailSalesInvoice CreateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService);
        RetailSalesInvoice UpdateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService);
        RetailSalesInvoice SoftDeleteObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService);
        RetailSalesInvoice ConfirmObject(RetailSalesInvoice retailSalesInvoice, DateTime ConfirmationDate, int ContactId,
                                         IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IContactService _contactService,
                                         IPriceMutationService _priceMutationService, IReceivableService _receivableService,
                                         IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                         IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService,
                                         IStockMutationService _stockMutationService, IClosingService _closingService);
        RetailSalesInvoice UnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService,
                                           IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                           IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService,
                                           IBarringService _barringService, IStockMutationService _stockMutationService, IClosingService _closingService);
        RetailSalesInvoice PaidObject(RetailSalesInvoice retailSalesInvoice, decimal AmountPaid, ICashBankService _cashBankService, IReceivableService _receivableService,
                                      IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                      IContactService _contactService, ICashMutationService _cashMutationService,
                                      IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        RetailSalesInvoice UnpaidObject(RetailSalesInvoice retailSalesInvoice, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                        ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                        IGeneralLedgerJournalService _generalLedgerService, IAccountService _accountService, IClosingService _closingService);
        bool DeleteObject(int Id);
    }
}
