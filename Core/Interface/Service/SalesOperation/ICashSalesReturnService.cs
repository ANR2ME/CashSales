using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ICashSalesReturnService
    {
        IQueryable<CashSalesReturn> GetQueryable();
        ICashSalesReturnValidator GetValidator();
        ICashSalesReturnRepository GetRepository();
        IList<CashSalesReturn> GetAll();
        IList<CashSalesReturn> GetObjectsByCashSalesInvoiceId(int CashSalesInvoiceId);
        CashSalesReturn GetObjectById(int Id);
        CashSalesReturn CreateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService);
        CashSalesReturn UpdateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService);
        CashSalesReturn SoftDeleteObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        CashSalesReturn ConfirmObjectForRepair(CashSalesReturn cashSalesReturn, DateTime ConfirmationDate, decimal Allowance, string PayableCode,
                                      ICashSalesReturnDetailService _cashSalesReturnDetailService, IContactService _contactService,
                                      ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                      IPriceMutationService _priceMutationService, IPayableService _payableService,
                                      ICashSalesReturnService _cashSalesReturnService, IWarehouseItemService _warehouseItemService,
                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService,
                                      IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesReturn ConfirmObject(CashSalesReturn cashSalesReturn, DateTime ConfirmationDate, decimal Allowance,
                                      ICashSalesReturnDetailService _cashSalesReturnDetailService, IContactService _contactService,
                                      ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                      IPriceMutationService _priceMutationService, IPayableService _payableService,
                                      ICashSalesReturnService _cashSalesReturnService, IWarehouseItemService _warehouseItemService,
                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService,
                                      IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesReturn UnconfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService,
                                        ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                        IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                        IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService,
                                        IBarringService _barringService, IStockMutationService _stockMutationService,
                                        IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesReturn PaidObjectForRepair(CashSalesReturn cashSalesReturn, Nullable<DateTime> PaymentDate, /*decimal Allowance,*/ string VoucherCode, string VoucherDetailCode, int VoucherId,
                                   ICashBankService _cashBankService, IPayableService _payableService,
                                   IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                   IContactService _contactService, ICashMutationService _cashMutationService,
                                   IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesReturn PaidObject(CashSalesReturn cashSalesReturn, /*decimal Allowance,*/ ICashBankService _cashBankService, IPayableService _payableService,
                                   IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                   IContactService _contactService, ICashMutationService _cashMutationService,
                                   IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashSalesReturn UnpaidObject(CashSalesReturn cashSalesReturn, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                     ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                     IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        bool DeleteObject(int Id);
    }
}
