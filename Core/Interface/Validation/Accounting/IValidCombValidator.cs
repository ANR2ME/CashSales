using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IValidCombValidator
    {
        ValidComb VHasAccount(ValidComb validComb, IAccountService _accountService);
        ValidComb VHasClosing(ValidComb validComb, IClosingService _closingService);

        ValidComb VCreateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService);
        ValidComb VUpdateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService);
        ValidComb VDeleteObject(ValidComb validComb);
        bool ValidCreateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService);
        bool ValidUpdateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService);
        bool ValidDeleteObject(ValidComb validComb);
        bool isValid(ValidComb validComb);
        string PrintError(ValidComb validComb);
    }
}
