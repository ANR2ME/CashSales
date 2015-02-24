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
    public class PaymentRequestDetailValidator : IPaymentRequestDetailValidator
    {
        public PaymentRequestDetail VHasPaymentRequest(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService)
        {
            PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
            if (paymentRequest == null)
            {
                paymentRequestDetail.Errors.Add("PaymentRequestId", "Tidak boleh tidak ada");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VHasAccount(PaymentRequestDetail paymentRequestDetail, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(paymentRequestDetail.AccountId);
            if (account == null)
            {
                paymentRequestDetail.Errors.Add("AccountId", "Tidak boleh tidak ada");
            }
            else if (account.IsCashBankAccount)
            {
                paymentRequestDetail.Errors.Add("AccountId", "Tidak boleh memilih CashBank Account secara langsung");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VHasNotBeenConfirmed(PaymentRequestDetail paymentRequestDetail)
        {
            if (paymentRequestDetail.IsConfirmed)
            {
                paymentRequestDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VHasBeenConfirmed(PaymentRequestDetail paymentRequestDetail)
        {
            if (!paymentRequestDetail.IsConfirmed)
            {
                paymentRequestDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VHasNotBeenDeleted(PaymentRequestDetail paymentRequestDetail)
        {
            if (paymentRequestDetail.IsDeleted)
            {
                paymentRequestDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VAmountIsTheSameWithPaymentRequestAmount(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService)
        {
            PaymentRequest paymentRequest = _paymentRequestService.GetObjectById(paymentRequestDetail.PaymentRequestId);
            if (paymentRequestDetail.Amount != paymentRequest.Amount)
            {
                paymentRequest.Errors.Add("Generic", "Account Payable Non Trading tidak memiliki amount yang sama dengan Payment Request");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VNonNegativeAmount(PaymentRequestDetail paymentRequestDetail)
        {
            if (paymentRequestDetail.Amount < 0)
            {
                paymentRequestDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VStatusIsCredit(PaymentRequestDetail paymentRequestDetail)
        {
            if (paymentRequestDetail.Status == Constant.GeneralLedgerStatus.Debit)
            {
                paymentRequestDetail.Errors.Add("Generic", "Sistem mengharapkan posting General Ledger Credit");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VStatusIsDebit(PaymentRequestDetail paymentRequestDetail)
        {
            if (paymentRequestDetail.Status == Constant.GeneralLedgerStatus.Credit)
            {
                paymentRequestDetail.Errors.Add("Generic", "Sistem mengharapkan posting General Ledger Debit");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VNotLegacyObject(PaymentRequestDetail paymentRequestDetail)
        {
            if (paymentRequestDetail.IsLegacy)
            {
                paymentRequestDetail.Errors.Add("Generic", "Hanya untuk non-legacy object");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VIsLegacyObject(PaymentRequestDetail paymentRequestDetail)
        {
            if (!paymentRequestDetail.IsLegacy)
            {
                paymentRequestDetail.Errors.Add("Generic", "Hanya untuk legacy object");
            }
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VCreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VHasPaymentRequest(paymentRequestDetail, _paymentRequestService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasAccount(paymentRequestDetail, _accountService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasNotBeenConfirmed(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasNotBeenDeleted(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VNotLegacyObject(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VNonNegativeAmount(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VStatusIsDebit(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VUpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VHasNotBeenConfirmed(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VCreateObject(paymentRequestDetail, _paymentRequestService, _paymentRequestDetailService, _accountService);
            return paymentRequestDetail;    
        }

        public PaymentRequestDetail VCreateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VHasPaymentRequest(paymentRequestDetail, _paymentRequestService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasAccount(paymentRequestDetail, _accountService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasNotBeenConfirmed(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasNotBeenDeleted(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VIsLegacyObject(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VNonNegativeAmount(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VAmountIsTheSameWithPaymentRequestAmount(paymentRequestDetail, _paymentRequestService);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VStatusIsCredit(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VUpdateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VHasNotBeenConfirmed(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VCreateLegacyObject(paymentRequestDetail, _paymentRequestService, _paymentRequestDetailService, _accountService);
            return paymentRequestDetail;
        }
        public PaymentRequestDetail VDeleteObject(PaymentRequestDetail paymentRequestDetail)
        {
            VHasNotBeenConfirmed(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasNotBeenDeleted(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VNotLegacyObject(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VHasConfirmationDate(PaymentRequestDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PaymentRequestDetail VConfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            VHasConfirmationDate(paymentRequestDetail);
            if (!isValid(paymentRequestDetail)) { return paymentRequestDetail; }
            VHasNotBeenConfirmed(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public PaymentRequestDetail VUnconfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            VHasBeenConfirmed(paymentRequestDetail);
            return paymentRequestDetail;
        }

        public bool ValidCreateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VCreateObject(paymentRequestDetail, _paymentRequestService, _paymentRequestDetailService, _accountService);
            return isValid(paymentRequestDetail);
        }

        public bool ValidUpdateObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VUpdateObject(paymentRequestDetail, _paymentRequestService, _paymentRequestDetailService, _accountService);
            return isValid(paymentRequestDetail);
        }

        public bool ValidCreateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VCreateLegacyObject(paymentRequestDetail, _paymentRequestService, _paymentRequestDetailService, _accountService);
            return isValid(paymentRequestDetail);
        }

        public bool ValidUpdateLegacyObject(PaymentRequestDetail paymentRequestDetail, IPaymentRequestService _paymentRequestService, IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService)
        {
            VUpdateLegacyObject(paymentRequestDetail, _paymentRequestService, _paymentRequestDetailService, _accountService);
            return isValid(paymentRequestDetail);
        }

        public bool ValidDeleteObject(PaymentRequestDetail paymentRequestDetail)
        {
            VDeleteObject(paymentRequestDetail);
            return isValid(paymentRequestDetail);
        }

        public bool ValidConfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            VConfirmObject(paymentRequestDetail);
            return isValid(paymentRequestDetail);
        }

        public bool ValidUnconfirmObject(PaymentRequestDetail paymentRequestDetail)
        {
            VUnconfirmObject(paymentRequestDetail);
            return isValid(paymentRequestDetail);
        }

        public bool isValid(PaymentRequestDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PaymentRequestDetail obj)
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