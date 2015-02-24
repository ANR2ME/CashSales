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
    public class MemorialDetailValidator : IMemorialDetailValidator
    {
        public MemorialDetail VHasMemorial(MemorialDetail memorialDetail, IMemorialService _memorialService)
        {
            Memorial memorial = _memorialService.GetObjectById(memorialDetail.MemorialId);
            if (memorial == null)
            {
                memorialDetail.Errors.Add("MemorialId", "Tidak boleh tidak ada");
            }
            return memorialDetail;
        }

        public MemorialDetail VHasAccount(MemorialDetail memorialDetail, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(memorialDetail.AccountId);
            if (account == null)
            {
                memorialDetail.Errors.Add("AccountId", "Tidak boleh tidak ada");
            }
            else if (account.IsCashBankAccount)
            {
                memorialDetail.Errors.Add("AccountId", "Tidak boleh memilih CashBank Account secara langsung");
            }
            return memorialDetail;
        }

        public MemorialDetail VHasNotBeenConfirmed(MemorialDetail memorialDetail)
        {
            if (memorialDetail.IsConfirmed)
            {
                memorialDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return memorialDetail;
        }

        public MemorialDetail VHasBeenConfirmed(MemorialDetail memorialDetail)
        {
            if (!memorialDetail.IsConfirmed)
            {
                memorialDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return memorialDetail;
        }

        public MemorialDetail VHasNotBeenDeleted(MemorialDetail memorialDetail)
        {
            if (memorialDetail.IsDeleted)
            {
                memorialDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return memorialDetail;
        }

        public MemorialDetail VAmountIsTheSameWithMemorialAmount(MemorialDetail memorialDetail, IMemorialService _memorialService)
        {
            Memorial memorial = _memorialService.GetObjectById(memorialDetail.MemorialId);
            if (memorialDetail.Amount != memorial.Amount)
            {
                memorial.Errors.Add("Generic", "Account Payable Non Trading tidak memiliki amount yang sama dengan Payment Request");
            }
            return memorialDetail;
        }

        public MemorialDetail VNonNegativeAmount(MemorialDetail memorialDetail)
        {
            if (memorialDetail.Amount < 0)
            {
                memorialDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return memorialDetail;
        }

        public MemorialDetail VHasStatus(MemorialDetail memorialDetail)
        {
            if (memorialDetail.Status != Constant.GeneralLedgerStatus.Debit &&
                memorialDetail.Status != Constant.GeneralLedgerStatus.Credit)
            {
                memorialDetail.Errors.Add("Generic", "Sistem mengharapkan posting General Ledger Credit/Debit");
            }
            return memorialDetail;
        }

        public MemorialDetail VCreateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            VHasMemorial(memorialDetail, _memorialService);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VHasAccount(memorialDetail, _accountService);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VHasNotBeenConfirmed(memorialDetail);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VHasNotBeenDeleted(memorialDetail);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VNonNegativeAmount(memorialDetail);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VHasStatus(memorialDetail);
            return memorialDetail;
        }

        public MemorialDetail VUpdateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            VHasNotBeenConfirmed(memorialDetail);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VCreateObject(memorialDetail, _memorialService, _memorialDetailService, _accountService);
            return memorialDetail;    
        }

        public MemorialDetail VDeleteObject(MemorialDetail memorialDetail)
        {
            VHasNotBeenConfirmed(memorialDetail);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VHasNotBeenDeleted(memorialDetail);
            return memorialDetail;
        }

        public MemorialDetail VHasConfirmationDate(MemorialDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public MemorialDetail VConfirmObject(MemorialDetail memorialDetail)
        {
            VHasConfirmationDate(memorialDetail);
            if (!isValid(memorialDetail)) { return memorialDetail; }
            VHasNotBeenConfirmed(memorialDetail);
            return memorialDetail;
        }

        public MemorialDetail VUnconfirmObject(MemorialDetail memorialDetail)
        {
            VHasBeenConfirmed(memorialDetail);
            return memorialDetail;
        }

        public bool ValidCreateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            VCreateObject(memorialDetail, _memorialService, _memorialDetailService, _accountService);
            return isValid(memorialDetail);
        }

        public bool ValidUpdateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService)
        {
            VUpdateObject(memorialDetail, _memorialService, _memorialDetailService, _accountService);
            return isValid(memorialDetail);
        }

        public bool ValidDeleteObject(MemorialDetail memorialDetail)
        {
            VDeleteObject(memorialDetail);
            return isValid(memorialDetail);
        }

        public bool ValidConfirmObject(MemorialDetail memorialDetail)
        {
            VConfirmObject(memorialDetail);
            return isValid(memorialDetail);
        }

        public bool ValidUnconfirmObject(MemorialDetail memorialDetail)
        {
            VUnconfirmObject(memorialDetail);
            return isValid(memorialDetail);
        }

        public bool isValid(MemorialDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(MemorialDetail obj)
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