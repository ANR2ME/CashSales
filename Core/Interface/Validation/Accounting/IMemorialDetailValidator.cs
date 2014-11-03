using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IMemorialDetailValidator
    {
        MemorialDetail VHasMemorial(MemorialDetail memorialDetail, IMemorialService _memorialService);
        MemorialDetail VHasAccount(MemorialDetail memorialDetail, IAccountService _accountService);
        MemorialDetail VHasBeenConfirmed(MemorialDetail memorialDetail);
        MemorialDetail VHasNotBeenConfirmed(MemorialDetail memorialDetail);
        MemorialDetail VHasNotBeenDeleted(MemorialDetail memorialDetail);
        MemorialDetail VNonNegativeAmount(MemorialDetail memorialDetail);
        MemorialDetail VHasStatus(MemorialDetail memorialDetail);
        MemorialDetail VCreateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        MemorialDetail VUpdateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        MemorialDetail VDeleteObject(MemorialDetail memorialDetail);
        MemorialDetail VHasConfirmationDate(MemorialDetail memorialDetail);
        MemorialDetail VConfirmObject(MemorialDetail memorialDetail);
        MemorialDetail VUnconfirmObject(MemorialDetail memorialDetail);
        bool ValidCreateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        bool ValidUpdateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IMemorialDetailService _memorialDetailService, IAccountService _accountService);
        bool ValidDeleteObject(MemorialDetail memorialDetail);
        bool ValidConfirmObject(MemorialDetail memorialDetail);
        bool ValidUnconfirmObject(MemorialDetail memorialDetail);
        bool isValid(MemorialDetail memorialDetail);
        string PrintError(MemorialDetail memorialDetail);
    }
}
