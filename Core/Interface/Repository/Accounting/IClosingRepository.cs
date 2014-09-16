using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IClosingRepository : IRepository<Closing>
    {
        IQueryable<Closing> GetQueryable();
        IList<Closing> GetAll();
        Closing GetObjectById(int Id);
        Closing CreateObject(Closing closing);
        Closing CloseObject(Closing closing);
        Closing OpenObject(Closing closing);
        //Closing SoftDeleteObject(Closing closing);
        bool DeleteObject(int Id);
    }
}