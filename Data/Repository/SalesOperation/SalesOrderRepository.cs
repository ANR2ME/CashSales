using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesOrderRepository : EfRepository<SalesOrder>, ISalesOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesOrder> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalesOrder> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesOrder> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month).ToList();
        }

        public SalesOrder GetObjectById(int Id)
        {
            SalesOrder salesOrder = Find(so => so.Id == Id && !so.IsDeleted);
            if (salesOrder != null) { salesOrder.Errors = new Dictionary<string, string>(); }
            return salesOrder;
        }

        public IList<SalesOrder> GetObjectsByContactId(int contactId)
        {
            return FindAll(so => so.ContactId == contactId && !so.IsDeleted).ToList();
        }

        public IQueryable<SalesOrder> GetQueryableConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted);
        }

        public IList<SalesOrder> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public SalesOrder CreateObject(SalesOrder salesOrder)
        {
            if (salesOrder.Code == null || salesOrder.Code.Trim() == "")
            salesOrder.Code = SetObjectCode();
            salesOrder.IsDeleted = false;
            salesOrder.IsConfirmed = false;
            salesOrder.CreatedAt = DateTime.Now;
            return Create(salesOrder);
        }

        public SalesOrder UpdateObject(SalesOrder salesOrder)
        {
            salesOrder.UpdatedAt = DateTime.Now;
            Update(salesOrder);
            return salesOrder;
        }

        public SalesOrder SoftDeleteObject(SalesOrder salesOrder)
        {
            salesOrder.IsDeleted = true;
            salesOrder.DeletedAt = DateTime.Now;
            Update(salesOrder);
            return salesOrder;
        }

        public bool DeleteObject(int Id)
        {
            SalesOrder so = Find(x => x.Id == Id);
            return (Delete(so) == 1) ? true : false;
        }

        public SalesOrder ConfirmObject(SalesOrder salesOrder)
        {
            salesOrder.IsConfirmed = true;
            Update(salesOrder);
            return salesOrder;
        }

        public SalesOrder UnconfirmObject(SalesOrder salesOrder)
        {
            salesOrder.IsConfirmed = false;
            salesOrder.ConfirmationDate = null;
            UpdateObject(salesOrder);
            return salesOrder;
        }

        public SalesOrder SetDeliveryComplete(SalesOrder salesOrder)
        {
            salesOrder.IsDeliveryCompleted = true;
            UpdateObject(salesOrder);
            return salesOrder;
        }

        public SalesOrder UnsetDeliveryComplete(SalesOrder salesOrder)
        {
            salesOrder.IsDeliveryCompleted = false;
            UpdateObject(salesOrder);
            return salesOrder;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}