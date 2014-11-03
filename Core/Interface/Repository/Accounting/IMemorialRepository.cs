using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IMemorialRepository : IRepository<Memorial>
    {
        IQueryable<Memorial> GetQueryable();
        IList<Memorial> GetAll();
        IList<Memorial> GetAllByMonthCreated();
        Memorial GetObjectById(int Id);
        Memorial CreateObject(Memorial memorial);
        Memorial UpdateObject(Memorial memorial);
        Memorial SoftDeleteObject(Memorial memorial);
        bool DeleteObject(int Id);
        Memorial ConfirmObject(Memorial memorial);
        Memorial UnconfirmObject(Memorial memorial);
        string SetObjectCode();
    }
}