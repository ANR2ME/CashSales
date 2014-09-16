using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICashBankAdjustmentValidator
    {
        CashBankAdjustment VHasCashBank(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService);
        CashBankAdjustment VAdjustmentDate(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment VHasBeenConfirmed(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment VHasNotBeenConfirmed(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment VNonZeroAmount(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment VNonNegativeNorZeroCashBankAmount(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, bool CaseConfirm);
        CashBankAdjustment VGeneralLedgerPostingHasNotBeenClosed(CashBankAdjustment cashBankAdjustment, IClosingService _closingService, int CaseConfirmUnconfirm);
        CashBankAdjustment VCreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService);
        CashBankAdjustment VUpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService);
        CashBankAdjustment VDeleteObject(CashBankAdjustment cashBankAdjustment);
        CashBankAdjustment VHasConfirmationDate(CashBankAdjustment cashBankAdjusment);
        CashBankAdjustment VConfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        CashBankAdjustment VUnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidCreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService);
        bool ValidUpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService);
        bool ValidDeleteObject(CashBankAdjustment cashBankAdjustment);
        bool ValidConfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, IClosingService _closingService);
        bool isValid(CashBankAdjustment cashBankAdjustment);
        string PrintError(CashBankAdjustment cashBankAdjustment);
    }
}
