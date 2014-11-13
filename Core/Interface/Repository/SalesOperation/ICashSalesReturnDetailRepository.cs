using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface ICashSalesReturnDetailRepository : IRepository<CashSalesReturnDetail>
    {
        IQueryable<CashSalesReturnDetail> GetQueryable();
        IList<CashSalesReturnDetail> GetAll();
        IQueryable<CashSalesReturnDetail> GetQueryableObjectsByCashSalesReturnId(int CashSalesReturnId);
        IQueryable<CashSalesReturnDetail> GetQueryableObjectsByCashSalesInvoiceDetailId(int CashSalesInvoiceDetailId);
        IList<CashSalesReturnDetail> GetObjectsByCashSalesReturnId(int CashSalesReturnId);
        CashSalesReturnDetail GetObjectById(int Id);
        IList<CashSalesReturnDetail> GetObjectsByCashSalesInvoiceDetailId(int CashSalesInvoiceDetailId);
        CashSalesReturnDetail ConfirmObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail UnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail CreateObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail UpdateObject(CashSalesReturnDetail cashSalesReturnDetail);
        CashSalesReturnDetail SoftDeleteObject(CashSalesReturnDetail cashSalesReturnDetail);
        bool DeleteObject(int Id);
        string SetObjectCode(string ParentCode);
    }
}
