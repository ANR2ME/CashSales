using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Repository;

namespace Core.Interface.Service
{
    public interface ICashBankService
    {
        IQueryable<CashBank> GetQueryable();
        ICashBankValidator GetValidator();
        ICashBankRepository GetRepository();
        IList<CashBank> GetAll();
        CashBank GetObjectById(int Id);
        CashBank GetObjectByName(string Name);
        CashBank CreateObject(CashBank cashBank, IAccountService _accountService);
        CashBank UpdateObject(CashBank cashBank, string OldName, IAccountService _accountService);
        CashBank SoftDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService, IAccountService _accountService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(CashBank cashBank);
        decimal GetTotalCashBank();
    }
}