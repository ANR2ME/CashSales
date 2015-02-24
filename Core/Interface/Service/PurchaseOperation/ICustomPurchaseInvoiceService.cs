using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ICustomPurchaseInvoiceService
    {
        IQueryable<CustomPurchaseInvoice> GetQueryable();
        ICustomPurchaseInvoiceValidator GetValidator();
        ICustomPurchaseInvoiceRepository GetRepository();
        IList<CustomPurchaseInvoice> GetAll();
        CustomPurchaseInvoice GetObjectById(int Id);
        CustomPurchaseInvoice CreateObject(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService, IContactService _contactService, ICashBankService _cashBankService);
        CustomPurchaseInvoice UpdateObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                           IWarehouseService _warehouseService, IContactService _contactService, ICashBankService _cashBankService);
        CustomPurchaseInvoice SoftDeleteObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService);
        CustomPurchaseInvoice ConfirmObjectForRepair(CustomPurchaseInvoice customPurchaseInvoice, DateTime ConfirmationDate, string PayableCode,
                                         ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IContactService _contactService,
                                         IPriceMutationService _priceMutationService, IPayableService _payableService,
                                         ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                         IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService,
                                         IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CustomPurchaseInvoice ConfirmObject(CustomPurchaseInvoice customPurchaseInvoice, DateTime ConfirmationDate,
                                         ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IContactService _contactService,
                                         IPriceMutationService _priceMutationService, IPayableService _payableService,
                                         ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                         IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService,
                                         IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CustomPurchaseInvoice UnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                           IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                           IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService,
                                           IBarringService _barringService, IStockMutationService _stockMutationService, IPriceMutationService _priceMutationService,
                                           IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CustomPurchaseInvoice PaidObjectForRepair(CustomPurchaseInvoice customPurchaseInvoice, decimal AmountPaid, Nullable<DateTime> PaymentDate, string VoucherCode, string VoucherDetailCode, int VoucherId,
                                           ICashBankService _cashBankService, IPayableService _payableService,
                                           IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                           IContactService _contactService, ICashMutationService _cashMutationService,
                                           IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CustomPurchaseInvoice PaidObject(CustomPurchaseInvoice customPurchaseInvoice, decimal AmountPaid, Nullable<DateTime> PaymentDate, ICashBankService _cashBankService, IPayableService _payableService,
                                           IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                           IContactService _contactService, ICashMutationService _cashMutationService,
                                           IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CustomPurchaseInvoice UnpaidObject(CustomPurchaseInvoice customPurchaseInvoice, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                           ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                           IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        bool DeleteObject(int Id);
        decimal CalculateTotalAmountAfterDiscountAndTax(CustomPurchaseInvoice customPurchaseInvoice);
    }
}
