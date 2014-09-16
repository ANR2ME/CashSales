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
    public class ValidCombValidator : IValidCombValidator
    {

        public ValidComb VHasAccount(ValidComb validComb, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(validComb.AccountId);
            if (account == null)
            {
                validComb.Errors.Add("Account", "Tidak valid");
            }
            return validComb;
        }

        public ValidComb VHasClosing(ValidComb validComb, IClosingService _closingService)
        {
            Closing closing = _closingService.GetObjectById(validComb.ClosingId);
            if (closing == null)
            {
                validComb.Errors.Add("Closing", "Tidak valid");
            }
            return validComb;
        }

        public ValidComb VCreateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService)
        {
            VHasAccount(validComb, _accountService);
            if (!isValid(validComb)) { return validComb; }
            VHasClosing(validComb, _closingService);
            return validComb;
        }

        public ValidComb VUpdateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService)
        {
            VHasAccount(validComb, _accountService);
            if (!isValid(validComb)) { return validComb; }
            VHasClosing(validComb, _closingService);
            return validComb;
        }

        public ValidComb VDeleteObject(ValidComb validComb)
        {
            return validComb;
        }

        public bool ValidCreateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService)
        {
            VCreateObject(validComb, _accountService, _closingService);
            return isValid(validComb);
        }


        public bool ValidUpdateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService)
        {
            validComb.Errors.Clear();
            VUpdateObject(validComb, _accountService, _closingService);
            return isValid(validComb);
        }

        public bool ValidDeleteObject(ValidComb validComb)
        {
            validComb.Errors.Clear();
            VDeleteObject(validComb);
            return isValid(validComb);
        }

        public bool isValid(ValidComb obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ValidComb obj)
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
