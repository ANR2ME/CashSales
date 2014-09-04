using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class CashSalesInvoiceValidator : ICashSalesInvoiceValidator
    {
        public CashSalesInvoice VHasSalesDate(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.SalesDate == null || cashSalesInvoice.SalesDate.Equals(DateTime.FromBinary(0)))
            {
                cashSalesInvoice.Errors.Add("SalesDate", "Tidak ada");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasDueDate(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.DueDate == null || cashSalesInvoice.DueDate.Equals(DateTime.FromBinary(0)))
            {
                cashSalesInvoice.Errors.Add("DueDate", "Tidak ada");
            }
            else if (cashSalesInvoice.DueDate.GetValueOrDefault().Date < cashSalesInvoice.SalesDate.Date)
            {
                cashSalesInvoice.Errors.Add("DueDate", "Harus lebih besar atau sama dengan Sales Date");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasConfirmationDate(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.ConfirmationDate == null || cashSalesInvoice.ConfirmationDate.Equals(DateTime.FromBinary(0)))
            {
                cashSalesInvoice.Errors.Add("ConfirmationDate", "Tidak ada");
            }
            else if (cashSalesInvoice.ConfirmationDate.GetValueOrDefault().Date < cashSalesInvoice.SalesDate.Date)
            {
                cashSalesInvoice.Errors.Add("ConfirmationDate", "Harus lebih besar atau sama dengan Sales Date");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsValidDiscount(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.Discount < 0 || cashSalesInvoice.Discount > 100)
            {
                cashSalesInvoice.Errors.Add("Discount", "Harus diantara 0 sampai 100");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsValidTax(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.Tax < 0 || cashSalesInvoice.Tax > 100)
            {
                cashSalesInvoice.Errors.Add("Tax", "Harus diantara 0 sampai 100");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsValidAllowance(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.Allowance < 0 || cashSalesInvoice.Allowance > cashSalesInvoice.Total)
            {
                cashSalesInvoice.Errors.Add("Allowance", "Harus diantara 0 sampai Total");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasWarehouse(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(cashSalesInvoice.WarehouseId);
            if (warehouse == null)
            {
                cashSalesInvoice.Errors.Add("WarehouseId", "Tidak valid");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasNoCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id);
            if (cashSalesInvoiceDetails.Any())
            {
                cashSalesInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan CashSalesInvoiceDetails");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasNoCashSalesReturns(CashSalesInvoice cashSalesInvoice, ICashSalesReturnService _cashSalesReturnService)
        {
            IList<CashSalesReturn> cashSalesReturns = _cashSalesReturnService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id);
            if (cashSalesReturns.Any())
            {
                cashSalesInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan CashSalesReturns");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasNoReceiptVoucherDetails(CashSalesInvoice cashSalesInvoice, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, cashSalesInvoice.Id);
            if (receivable != null)
            {
                IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceivableId(receivable.Id);
                if (receiptVoucherDetails.Any())
                {
                    cashSalesInvoice.Errors.Add("Generic", "Tidak boleh terasosiasi dengan ReceiptVoucherDetails");
                }
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id);
            if (!cashSalesInvoiceDetails.Any())
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoiceDetils Tidak ada");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsConfirmableCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                                                          ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService)
        {
            IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id);
            if (!cashSalesInvoiceDetails.Any())
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoiceDetails Tidak ada");
            }
            else
            {
                ICashSalesInvoiceDetailValidator validator = _cashSalesInvoiceDetailService.GetValidator();
                foreach (var cashSalesInvoiceDetail in cashSalesInvoiceDetails)
                {
                    cashSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidConfirmObject(cashSalesInvoiceDetail, _cashSalesInvoiceService,_warehouseItemService))
                    {
                        cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoiceDetails harus confirmable semua");
                        return cashSalesInvoice;
                    }
                }
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsUnconfirmableCashSalesInvoiceDetails(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id);
            if (!cashSalesInvoiceDetails.Any())
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoiceDetails Tidak ada");
            }
            else
            {
                ICashSalesInvoiceDetailValidator validator = _cashSalesInvoiceDetailService.GetValidator();
                foreach (var cashSalesInvoiceDetail in cashSalesInvoiceDetails)
                {
                    cashSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidUnconfirmObject(cashSalesInvoiceDetail))
                    {
                        cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoiceDetails harus unconfirmable semua");
                        return cashSalesInvoice;
                    }
                }
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsNotDeleted(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.IsDeleted)
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoice tidak boleh terhapus");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsNotPaid(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.IsPaid)
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoice sudah terbayar");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsPaid(CashSalesInvoice cashSalesInvoice)
        {
            if (!cashSalesInvoice.IsPaid)
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoice belum dibayar");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsNotConfirmed(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.IsConfirmed)
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoice tidak boleh terkonfirmasi");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsConfirmed(CashSalesInvoice cashSalesInvoice)
        {
            if (!cashSalesInvoice.IsConfirmed)
            {
                cashSalesInvoice.Errors.Add("Generic", "CashSalesInvoice tidak terkonfirmasi");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsValidAmountPaid(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.AmountPaid > cashSalesInvoice.Total)
            {
                cashSalesInvoice.Errors.Add("AmountPaid", "Harus lebih kecil atau sama dengan Total Payable");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsValidFullPayment(CashSalesInvoice cashSalesInvoice)
        {
            if (cashSalesInvoice.AmountPaid != cashSalesInvoice.Total)
            {
                cashSalesInvoice.Errors.Add("AmountPaid", "Harus sama dengan Total Payable");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VHasCashBank(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)cashSalesInvoice.CashBankId.GetValueOrDefault());
            if (cashBank == null)
            {
                cashSalesInvoice.Errors.Add("CashBankId", "Tidak valid");
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VIsCashBankTypeNotBank(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)cashSalesInvoice.CashBankId.GetValueOrDefault());
            if (cashBank.IsBank)
            {
                cashSalesInvoice.Errors.Add("Generic", "CashBank harus bukan tipe Bank");
                return cashSalesInvoice;
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VConfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                                 ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasCashSalesInvoiceDetails(cashSalesInvoice, _cashSalesInvoiceDetailService);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsConfirmableCashSalesInvoiceDetails(cashSalesInvoice, _cashSalesInvoiceDetailService, _cashSalesInvoiceService, _warehouseItemService);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsNotConfirmed(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VHasDueDate(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsValidDiscount(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsValidTax(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VHasConfirmationDate(cashSalesInvoice);
            //if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            //VHasCashBank(cashSalesInvoice, _cashBankService);
            //if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            //VIsCashBankTypeNotBank(cashSalesInvoice, _cashBankService);
            return cashSalesInvoice;
        }

        public CashSalesInvoice VUnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                                   IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            VIsNotDeleted(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsConfirmed(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsNotPaid(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsUnconfirmableCashSalesInvoiceDetails(cashSalesInvoice, _cashSalesInvoiceDetailService);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VHasNoReceiptVoucherDetails(cashSalesInvoice, _receivableService, _receiptVoucherDetailService);
            return cashSalesInvoice;
        }

        public CashSalesInvoice VPaidObject(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService, ICashSalesReturnService _cashSalesReturnService)
        {
            VIsNotPaid(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsConfirmed(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VHasCashBank(cashSalesInvoice, _cashBankService);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsCashBankTypeNotBank(cashSalesInvoice, _cashBankService);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsValidAllowance(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsValidAmountPaid(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VHasNoCashSalesReturns(cashSalesInvoice, _cashSalesReturnService);
            if (cashSalesInvoice.IsFullPayment)
            {
                if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
                VIsValidFullPayment(cashSalesInvoice);
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice VUnpaidObject(CashSalesInvoice cashSalesInvoice, ICashSalesReturnService _cashSalesReturnService)
        {
            VIsPaid(cashSalesInvoice);
            VHasNoCashSalesReturns(cashSalesInvoice, _cashSalesReturnService);
            return cashSalesInvoice;
        }

        public CashSalesInvoice VCreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService)
        {
            VHasSalesDate(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VHasWarehouse(cashSalesInvoice, _warehouseService);
            return cashSalesInvoice;
        }

        public CashSalesInvoice VUpdateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService)
        {
            VIsNotDeleted(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsNotConfirmed(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VCreateObject(cashSalesInvoice, _warehouseService);
            return cashSalesInvoice;
        }

        public CashSalesInvoice VDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            VIsNotDeleted(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VIsNotConfirmed(cashSalesInvoice);
            if (!isValid(cashSalesInvoice)) { return cashSalesInvoice; }
            VHasNoCashSalesInvoiceDetails(cashSalesInvoice, _cashSalesInvoiceDetailService);
            return cashSalesInvoice;
        }

        public bool ValidCreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService)
        {
            VCreateObject(cashSalesInvoice, _warehouseService);
            return isValid(cashSalesInvoice);
        }

        public bool ValidConfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesInvoiceService _cashSalesInvoiceService, 
                                       IWarehouseItemService _warehouseItemService, IContactService _contactService, ICashBankService _cashBankService)
        {
            cashSalesInvoice.Errors.Clear();
            VConfirmObject(cashSalesInvoice, _cashSalesInvoiceDetailService, _cashSalesInvoiceService, _warehouseItemService, _contactService, _cashBankService);
            return isValid(cashSalesInvoice);
        }

        public bool ValidUnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, 
                                         IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            cashSalesInvoice.Errors.Clear();
            VUnconfirmObject(cashSalesInvoice, _cashSalesInvoiceDetailService, _receivableService, _receiptVoucherDetailService);
            return isValid(cashSalesInvoice);
        }

        public bool ValidPaidObject(CashSalesInvoice cashSalesInvoice, ICashBankService _cashBankService, IReceiptVoucherService _receiptVoucherService, ICashSalesReturnService _cashSalesReturnService)
        {
            cashSalesInvoice.Errors.Clear();
            VPaidObject(cashSalesInvoice, _cashBankService, _receiptVoucherService, _cashSalesReturnService);
            return isValid(cashSalesInvoice);
        }

        public bool ValidUnpaidObject(CashSalesInvoice cashSalesInvoice, ICashSalesReturnService _cashSalesReturnService)
        {
            cashSalesInvoice.Errors.Clear();
            VUnpaidObject(cashSalesInvoice, _cashSalesReturnService);
            return isValid(cashSalesInvoice);
        }

        public bool ValidUpdateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService)
        {
            cashSalesInvoice.Errors.Clear();
            VUpdateObject(cashSalesInvoice, _warehouseService);
            return isValid(cashSalesInvoice);
        }

        public bool ValidDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            cashSalesInvoice.Errors.Clear();
            VDeleteObject(cashSalesInvoice, _cashSalesInvoiceDetailService);
            return isValid(cashSalesInvoice);
        }

        public bool isValid(CashSalesInvoice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashSalesInvoice obj)
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
