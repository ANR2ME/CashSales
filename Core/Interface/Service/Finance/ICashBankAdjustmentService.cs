using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashBankAdjustmentService
    {
        IQueryable<CashBankAdjustment> GetQueryable();
        ICashBankAdjustmentValidator GetValidator();
        IList<CashBankAdjustment> GetAll();
        IList<CashBankAdjustment> GetObjectsByCashBankId(int cashBankId);
        CashBankAdjustment GetObjectById(int Id);
        CashBankAdjustment CreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService);
        CashBankAdjustment CreateObject(int CashBankId, DateTime AdjustmentDate, ICashBankService _cashBankService);
        CashBankAdjustment UpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService);
        CashBankAdjustment SoftDeleteObject(CashBankAdjustment cashBankAdjustment);
        bool DeleteObject(int Id);
        CashBankAdjustment ConfirmObject(CashBankAdjustment cashBankAdjustment, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                         IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService);
        CashBankAdjustment UnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                           IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService);
    }
}