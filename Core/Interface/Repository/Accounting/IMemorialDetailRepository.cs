using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IMemorialDetailRepository : IRepository<MemorialDetail>
    {
        IQueryable<MemorialDetail> GetQueryable();
        IList<MemorialDetail> GetAll();
        IList<MemorialDetail> GetAllByMonthCreated();
        IList<MemorialDetail> GetObjectsByMemorialId(int memorialId);
        MemorialDetail GetObjectById(int Id);
        MemorialDetail CreateObject(MemorialDetail memorialDetail);
        MemorialDetail UpdateObject(MemorialDetail memorialDetail);
        MemorialDetail SoftDeleteObject(MemorialDetail memorialDetail);
        bool DeleteObject(int Id);
        MemorialDetail ConfirmObject(MemorialDetail memorialDetail);
        MemorialDetail UnconfirmObject(MemorialDetail memorialDetail);
        string SetObjectCode(string ParentCode);
    }
}