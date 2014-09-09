using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IReceivableRepository : IRepository<Receivable>
    {
        IQueryable<Receivable> GetQueryable();
        IList<Receivable> GetAll();
        IList<Receivable> GetAllByMonthCreated();
        IList<Receivable> GetObjectsByContactId(int contactId);
        IList<Receivable> GetObjectsByDueDate(DateTime fromDueDate, DateTime toDueDate);
        Receivable GetObjectBySource(string ReceivableSource, int ReceivableSourceId); 
        Receivable GetObjectById(int Id);
        Receivable CreateObject(Receivable receivable);
        Receivable UpdateObject(Receivable receivable);
        Receivable SoftDeleteObject(Receivable receivable);
        bool DeleteObject(int Id);
        string SetObjectCode();
    }
}