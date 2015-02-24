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
        CashSalesInvoice ConfirmObjectForRepair(CashSalesInvoice cashSalesInvoice, DateTime ConfirmationDate, decimal Discount, decimal Tax, string ReceivableCode,
                                       ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IContactService _contactService,
                                       IPriceMutationService _priceMutationService, IReceivableService _receivableService,
                                       ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                       IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService,
                                       IStockMutationService _stockMutationService, ICashBankService _cashBankService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesInvoice ConfirmObject(CashSalesInvoice cashSalesInvoice, DateTime ConfirmationDate, decimal Discount, decimal Tax,
                                       ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IContactService _contactService,
                                       IPriceMutationService _priceMutationService, IReceivableService _receivableService,
                                       ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                       IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService,
                                       IStockMutationService _stockMutationService, ICashBankService _cashBankService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesInvoice UnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                         IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                         IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService,
                                         IBarringService _barringService, IStockMutationService _stockMutationService,
                                         IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesInvoice PaidObjectForRepair(CashSalesInvoice cashSalesInvoice, decimal AmountPaid, decimal Allowance, Nullable<DateTime> PaymentDate, string VoucherCode, string VoucherDetailCode, int VoucherId,
                                    ICashBankService _cashBankService, IReceivableService _receivableService, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                    IContactService _contactService, ICashMutationService _cashMutationService, ICashSalesReturnService _cashSalesReturnService,
                                    IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesInvoice PaidObject(CashSalesInvoice cashSalesInvoice, decimal AmountPaid, decimal Allowance, ICashBankService _cashBankService, IReceivableService _receivableService, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                    IContactService _contactService, ICashMutationService _cashMutationService, ICashSalesReturnService _cashSalesReturnService,
                                    IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesInvoice UnpaidObject(CashSalesInvoice cashSalesInvoice, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                      ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService, ICashSalesReturnService _cashSalesReturnService,
                                      IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        bool DeleteObject(int Id);
    }
}
