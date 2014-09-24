using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IGeneralLedgerJournalValidator
    {
        GeneralLedgerJournal VIsValidSourceDocument(GeneralLedgerJournal generalLedgerJournal);
        GeneralLedgerJournal VIsLeafAccount(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService);

        GeneralLedgerJournal VCreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService);
        GeneralLedgerJournal VDeleteObject(GeneralLedgerJournal generalLedgerJournal);
        bool ValidCreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService);
        bool ValidDeleteObject(GeneralLedgerJournal generalLedgerJournal);
        bool isValid(GeneralLedgerJournal generalLedgerJournal);
        string PrintError(GeneralLedgerJournal generalLedgerJournal);
    }
}
