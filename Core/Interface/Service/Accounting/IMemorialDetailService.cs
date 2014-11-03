using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IMemorialDetailService
    {
        IMemorialDetailValidator GetValidator();
        IQueryable<MemorialDetail> GetQueryable();
        IList<MemorialDetail> GetAll();
        IList<MemorialDetail> GetObjectsByMemorialId(int memorialId);
        MemorialDetail GetObjectById(int Id);
        MemorialDetail CreateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IAccountService _accountService);
        MemorialDetail UpdateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IAccountService _accountService);
        MemorialDetail SoftDeleteObject(MemorialDetail memorialDetail);
        bool DeleteObject(int Id);
        MemorialDetail ConfirmObject(MemorialDetail memorialDetail, DateTime ConfirmationDate, IMemorialService _memorialService);
        MemorialDetail UnconfirmObject(MemorialDetail memorialDetail, IMemorialService _memorialService);
    }
}