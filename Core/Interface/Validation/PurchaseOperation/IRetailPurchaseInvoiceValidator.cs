using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IRetailPurchaseInvoiceValidator
    {
        RetailPurchaseInvoice VHasPurchaseDate(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VHasDueDate(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VHasConfirmationDate(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsValidDiscount(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsValidTax(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VHasWarehouse(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService);
        RetailPurchaseInvoice VHasContact(RetailPurchaseInvoice retailPurchaseInvoice, IContactService _contactService);
        RetailPurchaseInvoice VHasNoPaymentVoucherDetails(RetailPurchaseInvoice retailPurchaseInvoice, IPayableService _receivableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        RetailPurchaseInvoice VHasNoRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService);
        RetailPurchaseInvoice VHasRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService);
        RetailPurchaseInvoice VIsConfirmableRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService,
                                                                          IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        RetailPurchaseInvoice VIsUnconfirmableRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService);
        RetailPurchaseInvoice VIsNotDeleted(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsNotPaid(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsPaid(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsNotConfirmed(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsConfirmed(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsValidGBCH_No(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsValidGBCH_DueDate(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsValidAmountPaid(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VIsValidFullPayment(RetailPurchaseInvoice retailPurchaseInvoice);
        RetailPurchaseInvoice VHasCashBank(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService);
        RetailPurchaseInvoice VIsCashBankTypeBank(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService);

        RetailPurchaseInvoice VConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService,
                                                 IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService);
        RetailPurchaseInvoice VUnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, 
                                            IPayableService _receivableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        RetailPurchaseInvoice VPaidObject(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        RetailPurchaseInvoice VUnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice);

        RetailPurchaseInvoice VCreateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService);
        RetailPurchaseInvoice VUpdateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService);
        RetailPurchaseInvoice VDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService);

        bool ValidConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService,
                                       IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService);
        bool ValidUnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, 
                                  IPayableService _receivableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool ValidPaidObject(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        bool ValidUnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice);

        bool ValidCreateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService);
        bool ValidUpdateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService);
        bool ValidDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService);
        bool isValid(RetailPurchaseInvoice retailPurchaseInvoice);
        string PrintError(RetailPurchaseInvoice retailPurchaseInvoice);
    }
}
