using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IValidCombService
    {
        IQueryable<ValidComb> GetQueryable();
        IValidCombValidator GetValidator();
        IList<ValidComb> GetAll();
        ValidComb GetObjectById(int Id);
        ValidComb FindOrCreateObjectByAccountAndClosing(int AccountId, int ClosingId);
        ValidComb CreateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService);
        ValidComb UpdateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService);
        bool DeleteObject(int Id);
    }
}