using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IMemorialValidator
    {
        Memorial VIsValidAmount(Memorial memorial);
        Memorial VDebitEqualCreditEqualAmount(Memorial memorial, IMemorialDetailService _memorialDetailService);
        Memorial VHasNotBeenConfirmed(Memorial memorial);
        Memorial VHasBeenConfirmed(Memorial memorial);
        Memorial VHasNotBeenDeleted(Memorial memorial);
        Memorial VHasConfirmationDate(Memorial memorial);
        Memorial VGeneralLedgerPostingHasNotBeenClosed(Memorial memorial, IClosingService _closingService, int CaseConfirmUnconfirm);

        Memorial VCreateObject(Memorial memorial);
        Memorial VUpdateObject(Memorial memorial);
        Memorial VDeleteObject(Memorial memorial);
        Memorial VConfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService);
        Memorial VUnconfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService);
        bool ValidCreateObject(Memorial memorial);
        bool ValidUpdateObject(Memorial memorial);
        bool ValidDeleteObject(Memorial memorial);
        bool ValidConfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService);
        bool ValidUnconfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, IClosingService _closingService);
        bool isValid(Memorial memorial);
        string PrintError(Memorial memorial);
    }
}
