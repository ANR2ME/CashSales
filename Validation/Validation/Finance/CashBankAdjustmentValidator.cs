using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class CashBankAdjustmentValidator : ICashBankAdjustmentValidator
    {
        public CashBankAdjustment VHasCashBank(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(cashBankAdjustment.CashBankId);
            if (cashBank == null)
            {
                cashBankAdjustment.Errors.Add("CashBank", "Tidak terasosiasi dengan cashBank");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VAdjustmentDate(CashBankAdjustment cashBankAdjustment)
        {
            if (cashBankAdjustment.AdjustmentDate == null)
            {
                cashBankAdjustment.Errors.Add("AdjustmentDate", "Tidak boleh kosong");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VHasNotBeenConfirmed(CashBankAdjustment cashBankAdjustment)
        {
            if (cashBankAdjustment.IsConfirmed)
            {
                cashBankAdjustment.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VHasBeenConfirmed(CashBankAdjustment cashBankAdjustment)
        {
            if (!cashBankAdjustment.IsConfirmed)
            {
                cashBankAdjustment.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VNonZeroAmount(CashBankAdjustment cashBankAdjustment)
        {
            if (cashBankAdjustment.Amount == 0)
            {
                cashBankAdjustment.Errors.Add("Amount", "Tidak boleh 0");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VNonNegativeNorZeroCashBankAmount(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, bool CaseConfirm)
        {
            CashBank cashBank = _cashBankService.GetObjectById(cashBankAdjustment.CashBankId);
            if (CaseConfirm && cashBankAdjustment.Amount < 0)
            {
                if (cashBank.Amount + cashBankAdjustment.Amount < 0)
                {
                    cashBankAdjustment.Errors.Add("Generic", "Final CashBank Amount tidak boleh kurang dari 0");
                }
            }
            else if (!CaseConfirm && cashBankAdjustment.Amount > 0)
            {
                if (cashBank.Amount - cashBankAdjustment.Amount < 0)
                {
                    cashBankAdjustment.Errors.Add("Generic", "Adjustment Amount tidak boleh lebih besar dari CashBank Amount");
                }
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VGeneralLedgerPostingHasNotBeenClosed(CashBankAdjustment cashBankAdjustment, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch (CaseConfirmUnconfirm)
            {
                case (1): // Confirm
                {
                    if (_closingService.IsDateClosed(cashBankAdjustment.ConfirmationDate.GetValueOrDefault()))
                    {
                        cashBankAdjustment.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
                case (2): // Unconfirm
                {
                    if (_closingService.IsDateClosed(cashBankAdjustment.ConfirmationDate.GetValueOrDefault()))
                    {
                        cashBankAdjustment.Errors.Add("Generic", "Ledger sudah tutup buku");
                    }
                    break;
                }
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VCreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VAdjustmentDate(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VHasCashBank(cashBankAdjustment, _cashBankService);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonZeroAmount(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonNegativeNorZeroCashBankAmount(cashBankAdjustment, _cashBankService, true);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VUpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VCreateObject(cashBankAdjustment, _cashBankService);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VHasNotBeenConfirmed(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VDeleteObject(CashBankAdjustment cashBankAdjustment)
        {
            VHasNotBeenConfirmed(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VHasConfirmationDate(CashBankAdjustment obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public CashBankAdjustment VConfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasConfirmationDate(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VHasNotBeenConfirmed(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonNegativeNorZeroCashBankAmount(cashBankAdjustment, _cashBankService, true);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VGeneralLedgerPostingHasNotBeenClosed(cashBankAdjustment, _closingService, 1);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VUnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonNegativeNorZeroCashBankAmount(cashBankAdjustment, _cashBankService, false);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VGeneralLedgerPostingHasNotBeenClosed(cashBankAdjustment, _closingService, 2);
            return cashBankAdjustment;
        }

        public bool ValidCreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VCreateObject(cashBankAdjustment, _cashBankService);
            return isValid(cashBankAdjustment);
        }

        public bool ValidUpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            cashBankAdjustment.Errors.Clear();
            VUpdateObject(cashBankAdjustment, _cashBankService);
            return isValid(cashBankAdjustment);
        }

        public bool ValidDeleteObject(CashBankAdjustment cashBankAdjustment)
        {
            cashBankAdjustment.Errors.Clear();
            VDeleteObject(cashBankAdjustment);
            return isValid(cashBankAdjustment);
        }

        public bool ValidConfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            cashBankAdjustment.Errors.Clear();
            VConfirmObject(cashBankAdjustment, _cashBankService, _closingService);
            return isValid(cashBankAdjustment);
        }

        public bool ValidUnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            cashBankAdjustment.Errors.Clear();
            VUnconfirmObject(cashBankAdjustment, _cashBankService, _closingService);
            return isValid(cashBankAdjustment);
        }

        public bool isValid(CashBankAdjustment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashBankAdjustment obj)
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