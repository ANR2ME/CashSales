using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IRetailPurchaseInvoiceService
    {
        IQueryable<RetailPurchaseInvoice> GetQueryable();
        IRetailPurchaseInvoiceValidator GetValidator();
        IRetailPurchaseInvoiceRepository GetRepository();
        IList<RetailPurchaseInvoice> GetAll();
        RetailPurchaseInvoice GetObjectById(int Id);
        RetailPurchaseInvoice CreateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService);
        RetailPurchaseInvoice UpdateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService);
        RetailPurchaseInvoice SoftDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService);
        RetailPurchaseInvoice ConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, DateTime ConfirmationDate, int ContactId,
                                         IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IContactService _contactService,
                                         IPriceMutationService _priceMutationService, IPayableService _payableService,
                                         IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                         IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        RetailPurchaseInvoice UnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService,
                                           IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                           IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService,
                                           IBarringService _barringService, IStockMutationService _stockMutationService);
        RetailPurchaseInvoice PaidObject(RetailPurchaseInvoice retailPurchaseInvoice, decimal AmountPaid, ICashBankService _cashBankService, IPayableService _payableService,
                                           IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                           IContactService _contactService, ICashMutationService _cashMutationService);
        RetailPurchaseInvoice UnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                           ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService);
        bool DeleteObject(int Id);
    }
}
