using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashBankMutationService
    {
        IQueryable<CashBankMutation> GetQueryable();
        ICashBankMutationValidator GetValidator();
        IList<CashBankMutation> GetAll();
        CashBankMutation GetObjectById(int Id);
        CashBank GetSourceCashBank(CashBankMutation cashBankMutation);
        CashBank GetTargetCashBank(CashBankMutation cashBankMutation);
        CashBankMutation CreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation UpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation SoftDeleteObject(CashBankMutation cashBankMutation);
        bool DeleteObject(int Id);
        CashBankMutation ConfirmObject(CashBankMutation cashBankMutation, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        CashBankMutation UnconfirmObject(CashBankMutation cashBankMutation, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                         IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);

    }
}