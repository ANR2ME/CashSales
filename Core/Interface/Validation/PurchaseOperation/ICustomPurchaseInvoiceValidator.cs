using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICustomPurchaseInvoiceValidator
    {
        CustomPurchaseInvoice VHasPurchaseDate(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VHasDueDate(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VHasConfirmationDate(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsValidDiscount(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsValidTax(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VHasWarehouse(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService);
        CustomPurchaseInvoice VHasContact(CustomPurchaseInvoice customPurchaseInvoice, IContactService _contactService);
        CustomPurchaseInvoice VHasNoPaymentVoucherDetails(CustomPurchaseInvoice customPurchaseInvoice, IPayableService _receivableService, IPaymentVoucherDetailService _paymentVoucherDetailService);
        CustomPurchaseInvoice VHasNoCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService);
        CustomPurchaseInvoice VHasCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService);
        CustomPurchaseInvoice VIsConfirmableCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                                                          ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        CustomPurchaseInvoice VIsUnconfirmableCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService);
        CustomPurchaseInvoice VIsNotDeleted(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsNotPaid(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsPaid(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsNotConfirmed(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsConfirmed(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsValidGBCH_No(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsValidGBCH_DueDate(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsValidAmountPaid(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VIsValidFullPayment(CustomPurchaseInvoice customPurchaseInvoice);
        CustomPurchaseInvoice VHasCashBank(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService);
        CustomPurchaseInvoice VIsCashBankTypeBank(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService);
        CustomPurchaseInvoice VGeneralLedgerPostingHasNotBeenClosed(CustomPurchaseInvoice customPurchaseInvoice, IClosingService _closingService, int CaseConfirmUnconfirm);

        CustomPurchaseInvoice VConfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                             ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService,
                                             IClosingService _closingService);
        CustomPurchaseInvoice VUnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, 
                                            IPayableService _receivableService, IPaymentVoucherDetailService _paymentVoucherDetailService, IClosingService _closingService);
        CustomPurchaseInvoice VPaidObject(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                          IClosingService _closingService);
        CustomPurchaseInvoice VUnpaidObject(CustomPurchaseInvoice customPurchaseInvoice, IClosingService _closingService);

        CustomPurchaseInvoice VCreateObject(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService, IContactService _contactService);
        CustomPurchaseInvoice VUpdateObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                            IWarehouseService _warehouseService, IContactService _contactService);
        CustomPurchaseInvoice VDeleteObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService);

        bool ValidConfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService,
                                IClosingService _closingService);
        bool ValidUnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, 
                                  IPayableService _receivableService, IPaymentVoucherDetailService _paymentVoucherDetailService, IClosingService _closingService);
        bool ValidPaidObject(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                             IClosingService _closingService);
        bool ValidUnpaidObject(CustomPurchaseInvoice customPurchaseInvoice, IClosingService _closingService);

        bool ValidCreateObject(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService, IContactService _contactService);
        bool ValidUpdateObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                               IWarehouseService _warehouseService, IContactService _contactService);
        bool ValidDeleteObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService);
        bool isValid(CustomPurchaseInvoice customPurchaseInvoice);
        string PrintError(CustomPurchaseInvoice customPurchaseInvoice);
    }
}
