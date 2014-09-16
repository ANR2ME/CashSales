using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IValidCombRepository : IRepository<ValidComb>
    {
        IQueryable<ValidComb> GetQueryable();
        IList<ValidComb> GetAll();
        ValidComb GetObjectById(int Id);
        ValidComb FindOrCreateObjectByAccountAndClosing(int accountId, int closingId);
        ValidComb CreateObject(ValidComb validComb);
        ValidComb UpdateObject(ValidComb validComb);
        //ValidComb SoftDeleteObject(ValidComb validComb);
        bool DeleteObject(int Id);
    }
}