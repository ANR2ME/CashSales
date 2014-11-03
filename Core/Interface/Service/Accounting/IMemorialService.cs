using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IMemorialService
    {
        IQueryable<Memorial> GetQueryable();
        IMemorialValidator GetValidator();
        IList<Memorial> GetAll();
        Memorial GetObjectById(int Id);
        Memorial CreateObject(Memorial memorial);
        Memorial UpdateObject(Memorial memorial);
        Memorial SoftDeleteObject(Memorial memorial);
        bool DeleteObject(int Id);
        Memorial ConfirmObject(Memorial memorial, DateTime ConfirmationDate, IMemorialDetailService _memorialDetailService,
                               IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        Memorial UnconfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService,
                                 IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}