using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class RetailSalesInvoiceValidator : IRetailSalesInvoiceValidator
    {
        public RetailSalesInvoice VHasSalesDate(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.SalesDate == null || retailSalesInvoice.SalesDate.Equals(DateTime.FromBinary(0)))
            {
                retailSalesInvoice.Errors.Add("SalesDate", "Tidak ada");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasDueDate(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.DueDate == null || retailSalesInvoice.DueDate.Equals(DateTime.FromBinary(0)))
            {
                retailSalesInvoice.Errors.Add("DueDate", "Tidak ada");
            }
            else if (retailSalesInvoice.DueDate.GetValueOrDefault().Date < retailSalesInvoice.SalesDate.Date)
            {
                retailSalesInvoice.Errors.Add("DueDate", "Harus lebih besar atau sama dengan Sales Date");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasConfirmationDate(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.ConfirmationDate == null || retailSalesInvoice.ConfirmationDate.Equals(DateTime.FromBinary(0)))
            {
                retailSalesInvoice.Errors.Add("ConfirmationDate", "Tidak ada");
            }
            else if (retailSalesInvoice.ConfirmationDate.GetValueOrDefault().Date < retailSalesInvoice.SalesDate.Date)
            {
                retailSalesInvoice.Errors.Add("ConfirmationDate", "Harus lebih besar atau sama dengan Sales Date");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsValidDiscount(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.Discount < 0 || retailSalesInvoice.Discount > 100)
            {
                retailSalesInvoice.Errors.Add("Discount", "Harus diantara 0 sampai 100");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsValidTax(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.Tax < 0 || retailSalesInvoice.Tax > 100)
            {
                retailSalesInvoice.Errors.Add("Tax", "Harus diantara 0 sampai 100");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasWarehouse(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(retailSalesInvoice.WarehouseId);
            if (warehouse == null)
            {
                retailSalesInvoice.Errors.Add("WarehouseId", "Tidak valid");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasContact(RetailSalesInvoice retailSalesInvoice, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(retailSalesInvoice.ContactId);
            if (contact == null)
            {
                retailSalesInvoice.Errors.Add("ContactId", "Tidak valid");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasNoRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
            if (retailSalesInvoiceDetails.Any())
            {
                retailSalesInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan RetailSalesInvoiceDetails");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasNoReceiptVoucherDetails(RetailSalesInvoice retailSalesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id);
            if (receivable != null)
            {
                IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceivableId(receivable.Id);
                if (receiptVoucherDetails.Any())
                {
                    retailSalesInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan ReceiptVoucherDetails");
                }
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
            if (!retailSalesInvoiceDetails.Any())
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoiceDetils Tidak ada");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsConfirmableRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, 
                                                                          IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
            if (!retailSalesInvoiceDetails.Any())
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoiceDetails Tidak ada");
            }
            else
            {
                IRetailSalesInvoiceDetailValidator validator = _retailSalesInvoiceDetailService.GetValidator();
                foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
                {
                    retailSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidConfirmObject(retailSalesInvoiceDetail, _retailSalesInvoiceService,_warehouseItemService))
                    {
                        retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoiceDetails harus confirmable semua");
                        return retailSalesInvoice;
                    }
                }
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsUnconfirmableRetailSalesInvoiceDetails(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
            if (!retailSalesInvoiceDetails.Any())
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoiceDetails Tidak ada");
            }
            else
            {
                IRetailSalesInvoiceDetailValidator validator = _retailSalesInvoiceDetailService.GetValidator();
                foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
                {
                    retailSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidUnconfirmObject(retailSalesInvoiceDetail))
                    {
                        retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoiceDetails harus unconfirmable semua");
                        return retailSalesInvoice;
                    }
                }
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsNotDeleted(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.IsDeleted)
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoice tidak boleh terhapus");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsNotPaid(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.IsPaid)
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoice sudah terbayar");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsPaid(RetailSalesInvoice retailSalesInvoice)
        {
            if (!retailSalesInvoice.IsPaid)
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoice belum dibayar");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsNotConfirmed(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.IsConfirmed)
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoice tidak boleh terkonfirmasi");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsConfirmed(RetailSalesInvoice retailSalesInvoice)
        {
            if (!retailSalesInvoice.IsConfirmed)
            {
                retailSalesInvoice.Errors.Add("Generic", "RetailSalesInvoice tidak terkonfirmasi");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsValidGBCH_No(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.IsGBCH && retailSalesInvoice.GBCH_No.Trim() == "")
            {
                retailSalesInvoice.Errors.Add("GBCH_No", "Tidak ada");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsValidGBCH_DueDate(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.IsGBCH && (retailSalesInvoice.GBCH_DueDate == null || retailSalesInvoice.GBCH_DueDate.Equals(DateTime.FromBinary(0))))
            {
                retailSalesInvoice.Errors.Add("GBCH_DueDate", "Tidak ada");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsValidAmountPaid(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.AmountPaid > retailSalesInvoice.Total)
            {
                retailSalesInvoice.Errors.Add("AmountPaid", "Harus lebih kecil atau sama dengan Total Payable");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsValidFullPayment(RetailSalesInvoice retailSalesInvoice)
        {
            if (retailSalesInvoice.AmountPaid != retailSalesInvoice.Total)
            {
                retailSalesInvoice.Errors.Add("AmountPaid", "Harus sama dengan Total Payable");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VHasCashBank(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)retailSalesInvoice.CashBankId.GetValueOrDefault());
            if (cashBank == null)
            {
                retailSalesInvoice.Errors.Add("CashBankId", "Tidak valid");
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VIsCashBankTypeBank(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)retailSalesInvoice.CashBankId.GetValueOrDefault());
            if (!cashBank.IsBank)
            {
                retailSalesInvoice.Errors.Add("Generic", "CashBank bukan tipe Bank");
                return retailSalesInvoice;
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VConfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, 
                                                 IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService)
        {
            VHasRetailSalesInvoiceDetails(retailSalesInvoice, _retailSalesInvoiceDetailService);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsConfirmableRetailSalesInvoiceDetails(retailSalesInvoice, _retailSalesInvoiceDetailService, _retailSalesInvoiceService, _warehouseItemService);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsNotConfirmed(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VHasDueDate(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsValidDiscount(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsValidTax(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VHasConfirmationDate(retailSalesInvoice);
            if (retailSalesInvoice.IsGroupPricing)
            {
                if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
                VHasContact(retailSalesInvoice, _contactService);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VUnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, 
                                                   IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            VIsNotDeleted(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsConfirmed(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsNotPaid(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsUnconfirmableRetailSalesInvoiceDetails(retailSalesInvoice, _retailSalesInvoiceDetailService);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VHasNoReceiptVoucherDetails(retailSalesInvoice, _receivableService, _receiptVoucherDetailService);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VPaidObject(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            VIsNotPaid(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsConfirmed(retailSalesInvoice);
            if (retailSalesInvoice.IsGBCH)
            {
                if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
                VIsValidGBCH_No(retailSalesInvoice);
                if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
                VIsValidGBCH_DueDate(retailSalesInvoice);
                if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
                VHasCashBank(retailSalesInvoice, _cashBankService);
                if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
                VIsCashBankTypeBank(retailSalesInvoice, _cashBankService);
            }
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsValidAmountPaid(retailSalesInvoice);
            if (retailSalesInvoice.IsFullPayment)
            {
                if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
                VIsValidFullPayment(retailSalesInvoice);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VUnpaidObject(RetailSalesInvoice retailSalesInvoice)
        {
            VIsPaid(retailSalesInvoice);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VCreateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService)
        {
            VHasSalesDate(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VHasWarehouse(retailSalesInvoice, _warehouseService);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VUpdateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService)
        {
            VIsNotDeleted(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsNotConfirmed(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VCreateObject(retailSalesInvoice, _warehouseService);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice VDeleteObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            VIsNotDeleted(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VIsNotConfirmed(retailSalesInvoice);
            if (!isValid(retailSalesInvoice)) { return retailSalesInvoice; }
            VHasNoRetailSalesInvoiceDetails(retailSalesInvoice, _retailSalesInvoiceDetailService);
            return retailSalesInvoice;
        }

        public bool ValidCreateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService)
        {
            VCreateObject(retailSalesInvoice, _warehouseService);
            return isValid(retailSalesInvoice);
        }

        public bool ValidConfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, 
                                       IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService)
        {
            retailSalesInvoice.Errors.Clear();
            VConfirmObject(retailSalesInvoice, _retailSalesInvoiceDetailService, _retailSalesInvoiceService, _warehouseItemService, _contactService);
            return isValid(retailSalesInvoice);
        }

        public bool ValidUnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, 
                                         IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            retailSalesInvoice.Errors.Clear();
            VUnconfirmObject(retailSalesInvoice, _retailSalesInvoiceDetailService, _receivableService, _receiptVoucherDetailService);
            return isValid(retailSalesInvoice);
        }

        public bool ValidPaidObject(RetailSalesInvoice retailSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService)
        {
            retailSalesInvoice.Errors.Clear();
            VPaidObject(retailSalesInvoice, _cashBankService, _receiptVoucherService);
            return isValid(retailSalesInvoice);
        }

        public bool ValidUnpaidObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.Errors.Clear();
            VUnpaidObject(retailSalesInvoice);
            return isValid(retailSalesInvoice);
        }

        public bool ValidUpdateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService)
        {
            retailSalesInvoice.Errors.Clear();
            VUpdateObject(retailSalesInvoice, _warehouseService);
            return isValid(retailSalesInvoice);
        }

        public bool ValidDeleteObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            retailSalesInvoice.Errors.Clear();
            VDeleteObject(retailSalesInvoice, _retailSalesInvoiceDetailService);
            return isValid(retailSalesInvoice);
        }

        public bool isValid(RetailSalesInvoice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RetailSalesInvoice obj)
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
