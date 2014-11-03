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
    public class MemorialValidator : IMemorialValidator
    {
        public Memorial VIsValidAmount(Memorial memorial)
        {
            if (memorial.Amount < 0)
            {
                memorial.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return memorial;
        }

        public Memorial VDebitEqualCreditEqualAmount(Memorial memorial, IMemorialDetailService _memorialDetailService)
        {
            IList<MemorialDetail> memorialDetails = _memorialDetailService.GetObjectsByMemorialId(memorial.Id);
            decimal debit = 0;
            decimal credit = 0;
            foreach (var detail in memorialDetails)
            {
                debit += detail.Status == Constant.GeneralLedgerStatus.Debit ? detail.Amount : 0;
                credit += detail.Status == Constant.GeneralLedgerStatus.Credit ? detail.Amount : 0;
            }
            if (debit != credit || debit != memorial.Amount)
            {
                memorial.Errors.Add("Generic", "Jumlah debit, credit, dan amount harus sama: " + memorial.Amount);
            }
            return memorial;
        }

        public Memorial VHasBeenConfirmed(Memorial memorial)
        {
            if (!memorial.IsConfirmed)
            {
                memorial.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return memorial;
        }

        public Memorial VHasNotBeenConfirmed(Memorial memorial)
        {
            if (memorial.IsConfirmed)
            {
                memorial.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return memorial;
        }

        public Memorial VHasNotBeenDeleted(Memorial memorial)
        {
            if (memorial.IsDeleted)
            {
                memorial.Errors.Add("Generic", "Sudah dihapus");
            }
            return memorial;
        }

        public Memorial VHasConfirmationDate(Memorial obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public Memorial VGeneralLedgerPostingHasNotBeenClosed(Memorial memorial, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch (CaseConfirmUnconfirm)
            {
                case (1): // Confirm
                    {
                        if (_closingService.IsDateClosed(memorial.ConfirmationDate.GetValueOrDefault()))
                        {
                            memorial.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (2): // Unconfirm
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            memorial.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
            }
            return memorial;
        }

        public Memorial VCreateObject(Memorial memorial)
        {
            VIsValidAmount(memorial);
            return memorial;
        }

        public Memorial VUpdateObject(Memorial memorial)
        {
            VHasNotBeenConfirmed(memorial);
            if (!isValid(memorial)) { return memorial; }
            VHasNotBeenDeleted(memorial);
            if (!isValid(memorial)) { return memorial; }
            VCreateObject(memorial);
            return memorial;
        }

        public Memorial VDeleteObject(Memorial memorial)
        {
            VHasNotBeenConfirmed(memorial);
            if (!isValid(memorial)) { return memorial; }
            VHasNotBeenDeleted(memorial);
            return memorial;
        }

        public Memorial VConfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService)
        {
            VHasConfirmationDate(memorial);
            if (!isValid(memorial)) { return memorial; }
            VHasNotBeenDeleted(memorial);
            if (!isValid(memorial)) { return memorial; }
            VHasNotBeenConfirmed(memorial);
            if (!isValid(memorial)) { return memorial; }
            VDebitEqualCreditEqualAmount(memorial, _memorialDetailService);
            if (!isValid(memorial)) { return memorial; }
            VGeneralLedgerPostingHasNotBeenClosed(memorial, _closingService, 1);
            return memorial;
        }

        public Memorial VUnconfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService)
        {
            VHasBeenConfirmed(memorial);
            if (!isValid(memorial)) { return memorial; }
            VHasNotBeenDeleted(memorial);
            if (!isValid(memorial)) { return memorial; }
            VGeneralLedgerPostingHasNotBeenClosed(memorial, _closingService, 2);
            return memorial;
        }

        public bool ValidCreateObject(Memorial memorial)
        {
            VCreateObject(memorial);
            return isValid(memorial);
        }

        public bool ValidUpdateObject(Memorial memorial)
        {
            memorial.Errors.Clear();
            VUpdateObject(memorial);
            return isValid(memorial);
        }

        public bool ValidDeleteObject(Memorial memorial)
        {
            memorial.Errors.Clear();
            VDeleteObject(memorial);
            return isValid(memorial);
        }

        public bool ValidConfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService)
        {
            memorial.Errors.Clear();
            VConfirmObject(memorial, _memorialDetailService, _closingService);
            return isValid(memorial);
        }

        public bool ValidUnconfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService)
        {
            memorial.Errors.Clear();
            VUnconfirmObject(memorial, _memorialDetailService, _closingService);
            return isValid(memorial);
        }

        public bool isValid(Memorial obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Memorial obj)
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