using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class PaymentRequestValidator : IPaymentRequestValidator
    {
        public PaymentRequest VHasContact(PaymentRequest paymentRequest, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(paymentRequest.ContactId);
            if (contact == null)
            {
                paymentRequest.Errors.Add("ContactcId", "Tidak boleh kosong");
            }
            return paymentRequest;
        }

        public PaymentRequest VIsValidAmount(PaymentRequest paymentRequest)
        {
            if (paymentRequest.Amount < 0)
            {
                paymentRequest.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return paymentRequest;
        }

        public PaymentRequest VDebitEqualCreditEqualAmount(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService)
        {
            IList<PaymentRequestDetail> paymentRequestDetails = _paymentRequestDetailService.GetObjectsByPaymentRequestId(paymentRequest.Id);
            decimal debit = 0;
            decimal credit = 0;
            foreach (var detail in paymentRequestDetails)
            {
                debit += detail.Status == Constant.GeneralLedgerStatus.Debit ? detail.Amount : 0;
                credit += detail.Status == Constant.GeneralLedgerStatus.Credit ? detail.Amount : 0;
            }
            if (debit != credit || debit != paymentRequest.Amount)
            {
                paymentRequest.Errors.Add("Generic", "Jumlah debit, credit, dan amount harus sama: " + paymentRequest.Amount.ToString("f0"));
            }
            return paymentRequest;
        }

        public PaymentRequest VHasBeenConfirmed(PaymentRequest paymentRequest)
        {
            if (!paymentRequest.IsConfirmed)
            {
                paymentRequest.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasNotBeenConfirmed(PaymentRequest paymentRequest)
        {
            if (paymentRequest.IsConfirmed)
            {
                paymentRequest.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasNotBeenDeleted(PaymentRequest paymentRequest)
        {
            if (paymentRequest.IsDeleted)
            {
                paymentRequest.Errors.Add("Generic", "Sudah dihapus");
            }
            return paymentRequest;
        }

        public PaymentRequest VHasConfirmationDate(PaymentRequest obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PaymentRequest VHasNoVoucher(PaymentRequest paymentRequest, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            var payable = _payableService.GetObjectBySource(Constant.PayableSource.PaymentRequest, paymentRequest.Id);
            if (payable != null)
            {
                var vouchers = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id);
                if (vouchers.Any())
                {
                    paymentRequest.Errors.Add("Generic", "Tidak boleh terasosiasi dengan PaymentVoucher Details (PaymentVoucher ID : " + vouchers.FirstOrDefault().PaymentVoucherId + ")");
                }
            }
            return paymentRequest;
        }

        public PaymentRequest VGeneralLedgerPostingHasNotBeenClosed(PaymentRequest paymentRequest, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch (CaseConfirmUnconfirm)
            {
                case (1): // Confirm
                    {
                        if (_closingService.IsDateClosed(paymentRequest.ConfirmationDate.GetValueOrDefault()))
                        {
                            paymentRequest.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (2): // Unconfirm
                    {
                        if (_closingService.IsDateClosed(paymentRequest.ConfirmationDate.GetValueOrDefault()))
                        {
                            paymentRequest.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
            }
            return paymentRequest;
        }

        public PaymentRequest VCreateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            VHasContact(paymentRequest, _contactService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VIsValidAmount(paymentRequest);
            return paymentRequest;
        }

        public PaymentRequest VUpdateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            VHasNotBeenConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VCreateObject(paymentRequest, _contactService);
            return paymentRequest;
        }

        public PaymentRequest VDeleteObject(PaymentRequest paymentRequest)
        {
            VHasNotBeenConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            return paymentRequest;
        }

        public PaymentRequest VConfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService)
        {
            VHasConfirmationDate(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VDebitEqualCreditEqualAmount(paymentRequest, _paymentRequestDetailService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VGeneralLedgerPostingHasNotBeenClosed(paymentRequest, _closingService, 1);
            return paymentRequest;
        }

        public PaymentRequest VUnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            VHasBeenConfirmed(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNotBeenDeleted(paymentRequest);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VHasNoVoucher(paymentRequest, _payableService, _paymentVoucherDetailService);
            if (!isValid(paymentRequest)) { return paymentRequest; }
            VGeneralLedgerPostingHasNotBeenClosed(paymentRequest, _closingService, 2);
            return paymentRequest;
        }

        public bool ValidCreateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            VCreateObject(paymentRequest, _contactService);
            return isValid(paymentRequest);
        }

        public bool ValidUpdateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            paymentRequest.Errors.Clear();
            VUpdateObject(paymentRequest, _contactService);
            return isValid(paymentRequest);
        }

        public bool ValidDeleteObject(PaymentRequest paymentRequest)
        {
            paymentRequest.Errors.Clear();
            VDeleteObject(paymentRequest);
            return isValid(paymentRequest);
        }

        public bool ValidConfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService)
        {
            paymentRequest.Errors.Clear();
            VConfirmObject(paymentRequest, _paymentRequestDetailService, _closingService);
            return isValid(paymentRequest);
        }

        public bool ValidUnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IClosingService _closingService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            paymentRequest.Errors.Clear();
            VUnconfirmObject(paymentRequest, _paymentRequestDetailService, _closingService, _payableService, _paymentVoucherDetailService);
            return isValid(paymentRequest);
        }

        public bool isValid(PaymentRequest obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PaymentRequest obj)
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