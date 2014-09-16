using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IClosingService
    {
        IQueryable<Closing> GetQueryable();
        IClosingValidator GetValidator();
        IList<Closing> GetAll();
        Closing GetObjectById(int Id);
        Closing GetObjectByPeriodAndYear(int Period, int YearPeriod);
        Closing CreateObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService);
        Closing CloseObject(Closing closing, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IValidCombService _validCombService);
        Closing OpenObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService);
        bool DeleteObject(int Id, IAccountService _accountService, IValidCombService _validCombService);
        bool IsDateClosed(DateTime DateToCheck);
    }
}