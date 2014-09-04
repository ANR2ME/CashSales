using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class RetailPurchaseInvoiceValidator : IRetailPurchaseInvoiceValidator
    {
        public RetailPurchaseInvoice VHasPurchaseDate(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.PurchaseDate == null || retailPurchaseInvoice.PurchaseDate.Equals(DateTime.FromBinary(0)))
            {
                retailPurchaseInvoice.Errors.Add("PurchaseDate", "Tidak ada");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasDueDate(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.DueDate == null || retailPurchaseInvoice.DueDate.Equals(DateTime.FromBinary(0)))
            {
                retailPurchaseInvoice.Errors.Add("DueDate", "Tidak ada");
            }
            else if (retailPurchaseInvoice.DueDate.GetValueOrDefault().Date < retailPurchaseInvoice.PurchaseDate.Date)
            {
                retailPurchaseInvoice.Errors.Add("DueDate", "Harus lebih besar atau sama dengan Purchase Date");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasConfirmationDate(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.ConfirmationDate == null || retailPurchaseInvoice.ConfirmationDate.Equals(DateTime.FromBinary(0)))
            {
                retailPurchaseInvoice.Errors.Add("ConfirmationDate", "Tidak ada");
            }
            else if (retailPurchaseInvoice.ConfirmationDate.GetValueOrDefault().Date < retailPurchaseInvoice.PurchaseDate.Date)
            {
                retailPurchaseInvoice.Errors.Add("ConfirmationDate", "Harus lebih besar atau sama dengan Purchase Date");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsValidDiscount(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.Discount < 0 || retailPurchaseInvoice.Discount > 100)
            {
                retailPurchaseInvoice.Errors.Add("Discount", "Harus diantara 0 sampai 100");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsValidTax(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.Tax < 0 || retailPurchaseInvoice.Tax > 100)
            {
                retailPurchaseInvoice.Errors.Add("Tax", "Harus diantara 0 sampai 100");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasWarehouse(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(retailPurchaseInvoice.WarehouseId);
            if (warehouse == null)
            {
                retailPurchaseInvoice.Errors.Add("WarehouseId", "Tidak valid");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasContact(RetailPurchaseInvoice retailPurchaseInvoice, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(retailPurchaseInvoice.ContactId);
            if (contact == null)
            {
                retailPurchaseInvoice.Errors.Add("ContactId", "Tidak valid");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasNoRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
            if (retailPurchaseInvoiceDetails.Any())
            {
                retailPurchaseInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan RetailPurchaseInvoiceDetails");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasNoPaymentVoucherDetails(RetailPurchaseInvoice retailPurchaseInvoice, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id);
            if (payable != null)
            {
                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id);
                if (paymentVoucherDetails.Any())
                {
                    retailPurchaseInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan PaymentVoucherDetails");
                }
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
            if (!retailPurchaseInvoiceDetails.Any())
            {
                retailPurchaseInvoice.Errors.Add("Generic", "Tidak ada retail purchase invoice details");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsConfirmableRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, 
                                                                          IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
            if (!retailPurchaseInvoiceDetails.Any())
            {
                retailPurchaseInvoice.Errors.Add("Generic", "RetailPurchaseInvoiceDetails Tidak ada");
            }
            else
            {
                IRetailPurchaseInvoiceDetailValidator validator = _retailPurchaseInvoiceDetailService.GetValidator();
                foreach (var retailPurchaseInvoiceDetail in retailPurchaseInvoiceDetails)
                {
                    retailPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidConfirmObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService,_warehouseItemService))
                    {
                        retailPurchaseInvoice.Errors.Add("Generic", "Harus confirmable semua");
                        return retailPurchaseInvoice;
                    }
                }
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsUnconfirmableRetailPurchaseInvoiceDetails(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
            if (!retailPurchaseInvoiceDetails.Any())
            {
                retailPurchaseInvoice.Errors.Add("Generic", "RetailPurchaseInvoiceDetails Tidak ada");
            }
            else
            {
                IRetailPurchaseInvoiceDetailValidator validator = _retailPurchaseInvoiceDetailService.GetValidator();
                foreach (var retailPurchaseInvoiceDetail in retailPurchaseInvoiceDetails)
                {
                    retailPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidUnconfirmObject(retailPurchaseInvoiceDetail))
                    {
                        retailPurchaseInvoice.Errors.Add("Generic", "Harus unconfirmable semua");
                        return retailPurchaseInvoice;
                    }
                }
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsNotDeleted(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.IsDeleted)
            {
                retailPurchaseInvoice.Errors.Add("Generic", "RetailPurchaseInvoice tidak boleh terhapus");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsNotPaid(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.IsPaid)
            {
                retailPurchaseInvoice.Errors.Add("Generic", "RetailPurchaseInvoice sudah terbayar");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsPaid(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (!retailPurchaseInvoice.IsPaid)
            {
                retailPurchaseInvoice.Errors.Add("Generic", "RetailPurchaseInvoice belum dibayar");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsNotConfirmed(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.IsConfirmed)
            {
                retailPurchaseInvoice.Errors.Add("Generic", "RetailPurchaseInvoice tidak boleh terkonfirmasi");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsConfirmed(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (!retailPurchaseInvoice.IsConfirmed)
            {
                retailPurchaseInvoice.Errors.Add("Generic", "RetailPurchaseInvoice tidak terkonfirmasi");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsValidGBCH_No(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.IsGBCH && retailPurchaseInvoice.GBCH_No.Trim() == "")
            {
                retailPurchaseInvoice.Errors.Add("GBCH_No", "Tidak ada");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsValidGBCH_DueDate(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.IsGBCH && (retailPurchaseInvoice.GBCH_DueDate == null || retailPurchaseInvoice.GBCH_DueDate.Equals(DateTime.FromBinary(0))))
            {
                retailPurchaseInvoice.Errors.Add("GBCH_DueDate", "Tidak ada");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsValidAmountPaid(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.AmountPaid > retailPurchaseInvoice.Total)
            {
                retailPurchaseInvoice.Errors.Add("AmountPaid", "Harus lebih kecil atau sama dengan Total Payable");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsValidFullPayment(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            if (retailPurchaseInvoice.AmountPaid != retailPurchaseInvoice.Total)
            {
                retailPurchaseInvoice.Errors.Add("AmountPaid", "Harus sama dengan Total Payable");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VHasCashBank(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)retailPurchaseInvoice.CashBankId.GetValueOrDefault());
            if (cashBank == null)
            {
                retailPurchaseInvoice.Errors.Add("CashBankId", "Tidak valid");
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VIsCashBankTypeBank(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)retailPurchaseInvoice.CashBankId.GetValueOrDefault());
            if (!cashBank.IsBank)
            {
                retailPurchaseInvoice.Errors.Add("Generic", "CashBank bukan tipe Bank");
                return retailPurchaseInvoice;
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, 
                                                 IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService)
        {
            VHasRetailPurchaseInvoiceDetails(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsConfirmableRetailPurchaseInvoiceDetails(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService, _retailPurchaseInvoiceService, _warehouseItemService);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsNotConfirmed(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VHasDueDate(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsValidDiscount(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsValidTax(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VHasConfirmationDate(retailPurchaseInvoice);
            if (retailPurchaseInvoice.IsGroupPricing)
            {
                if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
                VHasContact(retailPurchaseInvoice, _contactService);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VUnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, 
                                                   IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            VIsNotDeleted(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsConfirmed(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsNotPaid(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsUnconfirmableRetailPurchaseInvoiceDetails(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VHasNoPaymentVoucherDetails(retailPurchaseInvoice, _payableService, _paymentVoucherDetailService);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VPaidObject(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            VIsNotPaid(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsConfirmed(retailPurchaseInvoice);
            if (retailPurchaseInvoice.IsGBCH)
            {
                if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
                VIsValidGBCH_No(retailPurchaseInvoice);
                if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
                VIsValidGBCH_DueDate(retailPurchaseInvoice);
                if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
                VHasCashBank(retailPurchaseInvoice, _cashBankService);
                if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
                VIsCashBankTypeBank(retailPurchaseInvoice, _cashBankService);
            }
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsValidAmountPaid(retailPurchaseInvoice);
            if (retailPurchaseInvoice.IsFullPayment)
            {
                if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
                VIsValidFullPayment(retailPurchaseInvoice);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VUnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            VIsPaid(retailPurchaseInvoice);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VCreateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            VHasPurchaseDate(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VHasWarehouse(retailPurchaseInvoice, _warehouseService);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VUpdateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            VIsNotDeleted(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsNotConfirmed(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VCreateObject(retailPurchaseInvoice, _warehouseService);
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice VDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            VIsNotDeleted(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VIsNotConfirmed(retailPurchaseInvoice);
            if (!isValid(retailPurchaseInvoice)) { return retailPurchaseInvoice; }
            VHasNoRetailPurchaseInvoiceDetails(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService);
            return retailPurchaseInvoice;
        }

        public bool ValidCreateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            VCreateObject(retailPurchaseInvoice, _warehouseService);
            return isValid(retailPurchaseInvoice);
        }

        public bool ValidConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, 
                                       IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService)
        {
            retailPurchaseInvoice.Errors.Clear();
            VConfirmObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService, _retailPurchaseInvoiceService, _warehouseItemService, _contactService);
            return isValid(retailPurchaseInvoice);
        }

        public bool ValidUnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, 
                                         IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            retailPurchaseInvoice.Errors.Clear();
            VUnconfirmObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService, _payableService, _paymentVoucherDetailService);
            return isValid(retailPurchaseInvoice);
        }

        public bool ValidPaidObject(RetailPurchaseInvoice retailPurchaseInvoice, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService)
        {
            retailPurchaseInvoice.Errors.Clear();
            VPaidObject(retailPurchaseInvoice, _cashBankService, _paymentVoucherService);
            return isValid(retailPurchaseInvoice);
        }

        public bool ValidUnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice)
        {
            retailPurchaseInvoice.Errors.Clear();
            VUnpaidObject(retailPurchaseInvoice);
            return isValid(retailPurchaseInvoice);
        }

        public bool ValidUpdateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            retailPurchaseInvoice.Errors.Clear();
            VUpdateObject(retailPurchaseInvoice, _warehouseService);
            return isValid(retailPurchaseInvoice);
        }

        public bool ValidDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            retailPurchaseInvoice.Errors.Clear();
            VDeleteObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService);
            return isValid(retailPurchaseInvoice);
        }

        public bool isValid(RetailPurchaseInvoice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RetailPurchaseInvoice obj)
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
