using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICashBankMutationValidator
    {
        CashBankMutation VHasDifferentCashBank(CashBankMutation cashBankMutation);
        CashBankMutation VHasSourceCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation VHasTargetCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation VHasNotBeenConfirmed(CashBankMutation cashBankMutation);
        CashBankMutation VHasBeenConfirmed(CashBankMutation cashBankMutation);
        CashBankMutation VHasNotBeenDeleted(CashBankMutation cashBankMutation);
        CashBankMutation VNonNegativeNorZeroAmount(CashBankMutation cashBankMutation);
        CashBankMutation VNonNegativeNorZeroSourceCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation VNonNegativeNorZeroTargetCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation VGeneralLedgerPostingHasNotBeenClosed(CashBankMutation cashBankMutation, IClosingService _closingService, int CaseConfirmUnconfirm);
        CashBankMutation VCreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation VUpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation VDeleteObject(CashBankMutation cashBankMutation);
        CashBankMutation VHasConfirmationDate(CashBankMutation cashBankMutation);
        CashBankMutation VConfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService);
        CashBankMutation VUnconfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidCreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        bool ValidUpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        bool ValidDeleteObject(CashBankMutation cashBankMutation);
        bool ValidConfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService);
        bool ValidUnconfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService);
        bool isValid(CashBankMutation cashBankMutation);
        string PrintError(CashBankMutation cashBankMutation);
    }
}