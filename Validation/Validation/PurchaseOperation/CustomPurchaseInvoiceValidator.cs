using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class CustomPurchaseInvoiceValidator : ICustomPurchaseInvoiceValidator
    {
        public CustomPurchaseInvoice VHasPurchaseDate(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.PurchaseDate == null || customPurchaseInvoice.PurchaseDate.Equals(DateTime.FromBinary(0)))
            {
                customPurchaseInvoice.Errors.Add("PurchaseDate", "Tidak ada");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasDueDate(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.DueDate == null || customPurchaseInvoice.DueDate.Equals(DateTime.FromBinary(0)))
            {
                customPurchaseInvoice.Errors.Add("DueDate", "Tidak ada");
            }
            else if (customPurchaseInvoice.DueDate.GetValueOrDefault().Date < customPurchaseInvoice.PurchaseDate.Date)
            {
                customPurchaseInvoice.Errors.Add("DueDate", "Harus lebih besar atau sama dengan Purchase Date");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasConfirmationDate(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.ConfirmationDate == null || customPurchaseInvoice.ConfirmationDate.Equals(DateTime.FromBinary(0)))
            {
                customPurchaseInvoice.Errors.Add("ConfirmationDate", "Tidak ada");
            }
            else if (customPurchaseInvoice.ConfirmationDate.GetValueOrDefault().Date < customPurchaseInvoice.PurchaseDate.Date)
            {
                customPurchaseInvoice.Errors.Add("ConfirmationDate", "Harus lebih besar atau sama dengan Purchase Date");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsValidDiscount(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.Discount < 0 || customPurchaseInvoice.Discount > 100)
            {
                customPurchaseInvoice.Errors.Add("Discount", "Harus diantara 0 sampai 100");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsValidTax(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.Tax < 0 || customPurchaseInvoice.Tax > 100)
            {
                customPurchaseInvoice.Errors.Add("Tax", "Harus diantara 0 sampai 100");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasWarehouse(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(customPurchaseInvoice.WarehouseId);
            if (warehouse == null)
            {
                customPurchaseInvoice.Errors.Add("WarehouseId", "Tidak valid");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasContact(CustomPurchaseInvoice customPurchaseInvoice, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(customPurchaseInvoice.ContactId);
            if (contact == null)
            {
                customPurchaseInvoice.Errors.Add("Contact", "Tidak valid");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasNoCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService)
        {
            IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = _customPurchaseInvoiceDetailService.GetObjectsByCustomPurchaseInvoiceId(customPurchaseInvoice.Id);
            if (customPurchaseInvoiceDetails.Any())
            {
                customPurchaseInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan CustomPurchaseInvoiceDetails");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasNoPaymentVoucherDetails(CustomPurchaseInvoice customPurchaseInvoice, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, customPurchaseInvoice.Id);
            if (payable != null)
            {
                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id);
                if (paymentVoucherDetails.Any())
                {
                    customPurchaseInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan PaymentVoucherDetails");
                }
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService)
        {
            IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = _customPurchaseInvoiceDetailService.GetObjectsByCustomPurchaseInvoiceId(customPurchaseInvoice.Id);
            if (!customPurchaseInvoiceDetails.Any())
            {
                customPurchaseInvoice.Errors.Add("Generic", "Tidak ada retail purchase invoice details");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsConfirmableCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, 
                                                                          ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = _customPurchaseInvoiceDetailService.GetObjectsByCustomPurchaseInvoiceId(customPurchaseInvoice.Id);
            if (!customPurchaseInvoiceDetails.Any())
            {
                customPurchaseInvoice.Errors.Add("Generic", "CustomPurchaseInvoiceDetails Tidak ada");
            }
            else
            {
                ICustomPurchaseInvoiceDetailValidator validator = _customPurchaseInvoiceDetailService.GetValidator();
                foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
                {
                    customPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidConfirmObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService,_warehouseItemService))
                    {
                        customPurchaseInvoice.Errors.Add("Generic", "Harus confirmable semua");
                        return customPurchaseInvoice;
                    }
                }
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsUnconfirmableCustomPurchaseInvoiceDetails(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService)
        {
            IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = _customPurchaseInvoiceDetailService.GetObjectsByCustomPurchaseInvoiceId(customPurchaseInvoice.Id);
            if (!customPurchaseInvoiceDetails.Any())
            {
                customPurchaseInvoice.Errors.Add("Generic", "CustomPurchaseInvoiceDetails Tidak ada");
            }
            else
            {
                ICustomPurchaseInvoiceDetailValidator validator = _customPurchaseInvoiceDetailService.GetValidator();
                foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
                {
                    customPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidUnconfirmObject(customPurchaseInvoiceDetail))
                    {
                        customPurchaseInvoice.Errors.Add("Generic", "Harus unconfirmable semua");
                        return customPurchaseInvoice;
                    }
                }
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsNotDeleted(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.IsDeleted)
            {
                customPurchaseInvoice.Errors.Add("Generic", "CustomPurchaseInvoice tidak boleh terhapus");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsNotPaid(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.IsPaid)
            {
                customPurchaseInvoice.Errors.Add("Generic", "CustomPurchaseInvoice sudah terbayar");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsPaid(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (!customPurchaseInvoice.IsPaid)
            {
                customPurchaseInvoice.Errors.Add("Generic", "CustomPurchaseInvoice belum dibayar");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsNotConfirmed(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.IsConfirmed)
            {
                customPurchaseInvoice.Errors.Add("Generic", "CustomPurchaseInvoice tidak boleh terkonfirmasi");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsConfirmed(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (!customPurchaseInvoice.IsConfirmed)
            {
                customPurchaseInvoice.Errors.Add("Generic", "CustomPurchaseInvoice tidak terkonfirmasi");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsValidGBCH_No(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.IsGBCH && customPurchaseInvoice.GBCH_No.Trim() == "")
            {
                customPurchaseInvoice.Errors.Add("GBCH_No", "Tidak ada");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsValidGBCH_DueDate(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.IsGBCH && (customPurchaseInvoice.GBCH_DueDate == null || customPurchaseInvoice.GBCH_DueDate.Equals(DateTime.FromBinary(0))))
            {
                customPurchaseInvoice.Errors.Add("GBCH_DueDate", "Tidak ada");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsValidAmountPaid(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.AmountPaid > customPurchaseInvoice.Total)
            {
                customPurchaseInvoice.Errors.Add("AmountPaid", "Harus lebih kecil atau sama dengan Total Payable");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsValidFullPayment(CustomPurchaseInvoice customPurchaseInvoice)
        {
            if (customPurchaseInvoice.AmountPaid != customPurchaseInvoice.Total)
            {
                customPurchaseInvoice.Errors.Add("AmountPaid", "Harus sama dengan Total Payable");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VHasCashBank(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)customPurchaseInvoice.CashBankId.GetValueOrDefault());
            if (cashBank == null)
            {
                customPurchaseInvoice.Errors.Add("CashBankId", "Tidak valid");
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VIsCashBankTypeBank(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)customPurchaseInvoice.CashBankId.GetValueOrDefault());
            if (!cashBank.IsBank)
            {
                customPurchaseInvoice.Errors.Add("Generic", "CashBank bukan tipe Bank");
                return customPurchaseInvoice;
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VConfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, 
                                                 ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService)
        {
            VHasCustomPurchaseInvoiceDetails(customPurchaseInvoice, _customPurchaseInvoiceDetailService);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsConfirmableCustomPurchaseInvoiceDetails(customPurchaseInvoice, _customPurchaseInvoiceDetailService, _customPurchaseInvoiceService, _warehouseItemService);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsNotConfirmed(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VHasDueDate(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsValidDiscount(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsValidTax(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VHasConfirmationDate(customPurchaseInvoice);
            if (customPurchaseInvoice.IsGroupPricing)
            {
                if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
                VHasContact(customPurchaseInvoice, _contactService);
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VUnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, 
                                                   IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            VIsNotDeleted(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsConfirmed(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsNotPaid(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsUnconfirmableCustomPurchaseInvoiceDetails(customPurchaseInvoice, _customPurchaseInvoiceDetailService);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VHasNoPaymentVoucherDetails(customPurchaseInvoice, _payableService, _paymentVoucherDetailService);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VPaidObject(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            VIsNotPaid(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsConfirmed(customPurchaseInvoice);
            if (customPurchaseInvoice.IsGBCH)
            {
                if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
                VIsValidGBCH_No(customPurchaseInvoice);
                if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
                VIsValidGBCH_DueDate(customPurchaseInvoice);
                if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
                VHasCashBank(customPurchaseInvoice, _cashBankService);
                if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
                VIsCashBankTypeBank(customPurchaseInvoice, _cashBankService);
            }
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsValidAmountPaid(customPurchaseInvoice);
            if (customPurchaseInvoice.IsFullPayment)
            {
                if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
                VIsValidFullPayment(customPurchaseInvoice);
            }
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VUnpaidObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            VIsPaid(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VCreateObject(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService, IContactService _contactService)
        {
            VHasPurchaseDate(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VHasContact(customPurchaseInvoice, _contactService);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VHasWarehouse(customPurchaseInvoice, _warehouseService);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VUpdateObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                                   IWarehouseService _warehouseService, IContactService _contactService)
        {
            VDeleteObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VCreateObject(customPurchaseInvoice, _warehouseService, _contactService);
            return customPurchaseInvoice;
        }

        public CustomPurchaseInvoice VDeleteObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService)
        {
            VIsNotDeleted(customPurchaseInvoice);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VHasNoCustomPurchaseInvoiceDetails(customPurchaseInvoice, _customPurchaseInvoiceDetailService);
            if (!isValid(customPurchaseInvoice)) { return customPurchaseInvoice; }
            VIsNotConfirmed(customPurchaseInvoice);
            return customPurchaseInvoice;
        }

        public bool ValidCreateObject(CustomPurchaseInvoice customPurchaseInvoice, IWarehouseService _warehouseService, IContactService _contactService)
        {
            VCreateObject(customPurchaseInvoice, _warehouseService, _contactService);
            return isValid(customPurchaseInvoice);
        }

        public bool ValidConfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, 
                                       ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService)
        {
            customPurchaseInvoice.Errors.Clear();
            VConfirmObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService, _customPurchaseInvoiceService, _warehouseItemService, _contactService);
            return isValid(customPurchaseInvoice);
        }

        public bool ValidUnconfirmObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, 
                                         IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            customPurchaseInvoice.Errors.Clear();
            VUnconfirmObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService, _payableService, _paymentVoucherDetailService);
            return isValid(customPurchaseInvoice);
        }

        public bool ValidPaidObject(CustomPurchaseInvoice customPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            customPurchaseInvoice.Errors.Clear();
            VPaidObject(customPurchaseInvoice, _cashBankService, _paymentVoucherService);
            return isValid(customPurchaseInvoice);
        }

        public bool ValidUnpaidObject(CustomPurchaseInvoice customPurchaseInvoice)
        {
            customPurchaseInvoice.Errors.Clear();
            VUnpaidObject(customPurchaseInvoice);
            return isValid(customPurchaseInvoice);
        }

        public bool ValidUpdateObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService,
                                      IWarehouseService _warehouseService, IContactService _contactService)
        {
            customPurchaseInvoice.Errors.Clear();
            VUpdateObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService, _warehouseService, _contactService);
            return isValid(customPurchaseInvoice);
        }

        public bool ValidDeleteObject(CustomPurchaseInvoice customPurchaseInvoice, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService)
        {
            customPurchaseInvoice.Errors.Clear();
            VDeleteObject(customPurchaseInvoice, _customPurchaseInvoiceDetailService);
            return isValid(customPurchaseInvoice);
        }

        public bool isValid(CustomPurchaseInvoice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CustomPurchaseInvoice obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
