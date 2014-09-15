using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICashBankMutationRepository : IRepository<CashBankMutation>
    {
        IQueryable<CashBankMutation> GetQueryable();
        IList<CashBankMutation> GetAll();
        IList<CashBankMutation> GetAllByMonthCreated();
        CashBankMutation GetObjectById(int Id);
        CashBank GetSourceCashBank(CashBankMutation cashBankMutation);
        CashBank GetTargetCashBank(CashBankMutation cashBankMutation);
        CashBankMutation CreateObject(CashBankMutation cashBankMutation);
        CashBankMutation UpdateObject(CashBankMutation cashBankMutation);
        CashBankMutation SoftDeleteObject(CashBankMutation cashBankMutation);
        CashBankMutation ConfirmObject(CashBankMutation cashBankMutation);
        CashBankMutation UnconfirmObject(CashBankMutation cashBankMutation);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}