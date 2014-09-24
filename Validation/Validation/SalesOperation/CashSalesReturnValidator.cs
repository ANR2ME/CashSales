using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class CashSalesReturnValidator : ICashSalesReturnValidator
    {
        public CashSalesReturn VHasReturnDate(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.ReturnDate == null || cashSalesReturn.ReturnDate.Equals(DateTime.FromBinary(0)))
            {
                cashSalesReturn.Errors.Add("ReturnDate", "Tidak ada");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VHasConfirmationDate(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.ConfirmationDate == null || cashSalesReturn.ConfirmationDate.Equals(DateTime.FromBinary(0)))
            {
                cashSalesReturn.Errors.Add("ConfirmationDate", "Tidak ada");
            }
            //else if (cashSalesReturn.ConfirmationDate.GetValueOrDefault().Date.AddDays(1) < cashSalesReturn.ReturnDate.GetValueOrDefault().Date)
            //{
            //    cashSalesReturn.Errors.Add("ConfirmationDate", "Harus lebih besar atau sama dengan Return Date");
            //}
            return cashSalesReturn;
        }

        public CashSalesReturn VIsValidTotal(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.Allowance < 0)
            {
                cashSalesReturn.Errors.Add("Total", "Harus lebih besar atau sama dengan 0");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsValidAllowance(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.Allowance < 0 || cashSalesReturn.Allowance > cashSalesReturn.Total)
            {
                cashSalesReturn.Errors.Add("Allowance", "Harus diantara 0 sampai Total");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsValidCashSalesInvoice(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService)
        {
            CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesReturn.CashSalesInvoiceId);
            if (cashSalesInvoice == null)
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesInvoice Tidak valid");
            }
            else if (!(cashSalesInvoice.IsConfirmed && cashSalesInvoice.IsPaid))
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesInvoice Harus sudah terkonfirmasi dan terbayar");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VHasNoCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            IList<CashSalesReturnDetail> cashSalesReturnDetails = _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(cashSalesReturn.Id);
            if (cashSalesReturnDetails.Any())
            {
                cashSalesReturn.Errors.Add("Generic", "Tidak boleh terasosiasi dengan CashSalesReturnDetails");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VHasCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            IList<CashSalesReturnDetail> cashSalesReturnDetails = _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(cashSalesReturn.Id);
            if (!cashSalesReturnDetails.Any())
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturnDetils Tidak ada");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsConfirmableCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService, 
                                                                    ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            IList<CashSalesReturnDetail> cashSalesReturnDetails = _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(cashSalesReturn.Id);
            if (!cashSalesReturnDetails.Any())
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturnDetails Tidak ada");
            }
            else
            {
                ICashSalesReturnDetailValidator validator = _cashSalesReturnDetailService.GetValidator();
                foreach (var cashSalesReturnDetail in cashSalesReturnDetails)
                {
                    cashSalesReturnDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidConfirmObject(cashSalesReturnDetail, _cashSalesInvoiceDetailService, _cashSalesReturnDetailService))
                    {
                        cashSalesReturn.Errors.Add("Generic", "CashSalesReturnDetails harus confirmable semua");
                        return cashSalesReturn;
                    }
                }
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsUnconfirmableCashSalesReturnDetails(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            IList<CashSalesReturnDetail> cashSalesReturnDetails = _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(cashSalesReturn.Id);
            if (!cashSalesReturnDetails.Any())
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturnDetails Tidak ada");
            }
            else
            {
                ICashSalesReturnDetailValidator validator = _cashSalesReturnDetailService.GetValidator();
                foreach (var cashSalesReturnDetail in cashSalesReturnDetails)
                {
                    cashSalesReturnDetail.Errors = new Dictionary<string, string>();
                    if (!validator.ValidUnconfirmObject(cashSalesReturnDetail))
                    {
                        cashSalesReturn.Errors.Add("Generic", "CashSalesReturnDetails harus unconfirmable semua");
                        return cashSalesReturn;
                    }
                }
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsNotDeleted(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.IsDeleted)
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturn tidak boleh terhapus");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsNotPaid(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.IsPaid)
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturn sudah terbayar");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsPaid(CashSalesReturn cashSalesReturn)
        {
            if (!cashSalesReturn.IsPaid)
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturn belum dibayar");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsNotConfirmed(CashSalesReturn cashSalesReturn)
        {
            if (cashSalesReturn.IsConfirmed)
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturn tidak boleh terkonfirmasi");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VIsConfirmed(CashSalesReturn cashSalesReturn)
        {
            if (!cashSalesReturn.IsConfirmed)
            {
                cashSalesReturn.Errors.Add("Generic", "CashSalesReturn tidak terkonfirmasi");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VHasCashBank(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)cashSalesReturn.CashBankId.GetValueOrDefault());
            if (cashBank == null)
            {
                cashSalesReturn.Errors.Add("CashBankId", "Tidak valid");
            }
            return cashSalesReturn;
        }

        /*public CashSalesReturn VIsCashBankTypeNotBank(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById((int)cashSalesReturn.CashBankId.GetValueOrDefault());
            if (cashBank.IsBank)
            {
                cashSalesReturn.Errors.Add("Generic", "CashBank harus bukan tipe Bank");
                return cashSalesReturn;
            }
            return cashSalesReturn;
        }*/

        public CashSalesReturn VHasNoPaymentVoucherDetails(CashSalesReturn cashSalesReturn, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CashSalesReturn, cashSalesReturn.Id);
            if (payable != null)
            {
                IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id);
                if (paymentVoucherDetails.Any())
                {
                    cashSalesReturn.Errors.Add("Generic", "Tidak boleh terasosiasi dengan PaymentVoucherDetails");
                }
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VHasCashSalesInvoice(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService)
        {
            CashSalesInvoice cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(cashSalesReturn.CashSalesInvoiceId);
            if (cashSalesInvoice == null)
            {
                cashSalesReturn.Errors.Add("CashSalesInvoiceId", "Tidak valid");
            }
            return cashSalesReturn;
        }

        public CashSalesReturn VConfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesReturnService _cashSalesReturnService,
                                              ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            VIsNotConfirmed(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsValidCashSalesInvoice(cashSalesReturn, _cashSalesInvoiceService);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VHasCashSalesReturnDetails(cashSalesReturn, _cashSalesReturnDetailService);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsConfirmableCashSalesReturnDetails(cashSalesReturn, _cashSalesReturnDetailService, _cashSalesInvoiceDetailService);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            
            
            return cashSalesReturn;
        }

        public CashSalesReturn VUnconfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService, 
                                                   IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            VIsNotDeleted(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsConfirmed(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsNotPaid(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VHasNoPaymentVoucherDetails(cashSalesReturn, _payableService, _paymentVoucherDetailService);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsUnconfirmableCashSalesReturnDetails(cashSalesReturn, _cashSalesReturnDetailService);
            return cashSalesReturn;
        }

        public CashSalesReturn VPaidObject(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService)
        {
            VIsNotPaid(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsConfirmed(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VHasCashBank(cashSalesReturn, _cashBankService);
            //if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            //VIsCashBankTypeNotBank(cashSalesReturn, _cashBankService);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsValidTotal(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsValidAllowance(cashSalesReturn);
            return cashSalesReturn;
        }

        public CashSalesReturn VUnpaidObject(CashSalesReturn cashSalesReturn)
        {
            VIsPaid(cashSalesReturn);
            return cashSalesReturn;
        }

        public CashSalesReturn VCreateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService)
        {
            VHasReturnDate(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VHasCashSalesInvoice(cashSalesReturn, _cashSalesInvoiceService);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VHasCashBank(cashSalesReturn, _cashBankService);
            return cashSalesReturn;
        }

        public CashSalesReturn VUpdateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService)
        {
            VIsNotDeleted(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsNotConfirmed(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VCreateObject(cashSalesReturn, _cashSalesInvoiceService, _cashBankService);
            return cashSalesReturn;
        }

        public CashSalesReturn VDeleteObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            VIsNotDeleted(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VIsNotConfirmed(cashSalesReturn);
            if (!isValid(cashSalesReturn)) { return cashSalesReturn; }
            VHasNoCashSalesReturnDetails(cashSalesReturn, _cashSalesReturnDetailService);
            return cashSalesReturn;
        }

        public bool ValidCreateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService)
        {
            VCreateObject(cashSalesReturn, _cashSalesInvoiceService, _cashBankService);
            return isValid(cashSalesReturn);
        }

        public bool ValidConfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesReturnService _cashSalesReturnService,
                                       ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            cashSalesReturn.Errors.Clear();
            VConfirmObject(cashSalesReturn, _cashSalesReturnDetailService, _cashSalesReturnService, _cashSalesInvoiceService, _cashSalesInvoiceDetailService);
            return isValid(cashSalesReturn);
        }

        public bool ValidUnconfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService,
                                         IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            cashSalesReturn.Errors.Clear();
            VUnconfirmObject(cashSalesReturn, _cashSalesReturnDetailService, _payableService, _paymentVoucherDetailService);
            return isValid(cashSalesReturn);
        }

        public bool ValidPaidObject(CashSalesReturn cashSalesReturn, ICashBankService _cashBankService)
        {
            cashSalesReturn.Errors.Clear();
            VPaidObject(cashSalesReturn, _cashBankService);
            return isValid(cashSalesReturn);
        }

        public bool ValidUnpaidObject(CashSalesReturn cashSalesReturn)
        {
            cashSalesReturn.Errors.Clear();
            VUnpaidObject(cashSalesReturn);
            return isValid(cashSalesReturn);
        }

        public bool ValidUpdateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService)
        {
            cashSalesReturn.Errors.Clear();
            VUpdateObject(cashSalesReturn, _cashSalesInvoiceService, _cashBankService);
            return isValid(cashSalesReturn);
        }

        public bool ValidDeleteObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            cashSalesReturn.Errors.Clear();
            VDeleteObject(cashSalesReturn, _cashSalesReturnDetailService);
            return isValid(cashSalesReturn);
        }

        public bool isValid(CashSalesReturn obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashSalesReturn obj)
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
