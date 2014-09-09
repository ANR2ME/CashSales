using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IReceivableService
    {
        IQueryable<Receivable> GetQueryable();
        IReceivableValidator GetValidator();
        IList<Receivable> GetAll();
        IList<Receivable> GetObjectsByContactId(int contactId);
        IList<Receivable> GetObjectsByDueDate(DateTime fromDueDate, DateTime toDueDate);
        Receivable GetObjectBySource(string ReceivableSource, int ReceivableSourceId); 
        Receivable GetObjectById(int Id);
        Receivable CreateObject(Receivable receivable);
        Receivable CreateObject(int contactId, string receivableSource, int receivableSourceId, decimal amount, DateTime dueDate);
        Receivable UpdateObject(Receivable receivable);
        Receivable SoftDeleteObject(Receivable receivable);
        bool DeleteObject(int Id);
        decimal GetTotalRemainingAmountByDueDate(DateTime fromDueDate, DateTime toDueDate);
    }
}