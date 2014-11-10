using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class DeliveryOrderRepository : EfRepository<DeliveryOrder>, IDeliveryOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public DeliveryOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<DeliveryOrder> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<DeliveryOrder> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<DeliveryOrder> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public DeliveryOrder GetObjectById(int Id)
        {
            DeliveryOrder deliveryOrder = Find(x => x.Id == Id && !x.IsDeleted);
            if (deliveryOrder != null) { deliveryOrder.Errors = new Dictionary<string, string>(); }
            return deliveryOrder;
        }

        public IQueryable<DeliveryOrder> GetQueryableObjectsBySalesOrderId(int salesOrderId)
        {
            return FindAll(x => x.SalesOrderId == salesOrderId && !x.IsDeleted);
        }

        public IList<DeliveryOrder> GetObjectsBySalesOrderId(int salesOrderId)
        {
            return FindAll(x => x.SalesOrderId == salesOrderId && !x.IsDeleted).ToList();
        }

        public IQueryable<DeliveryOrder> GetQueryableConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted);
        }

        public IList<DeliveryOrder> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder)
        {
            if (deliveryOrder.Code == null || deliveryOrder.Code.Trim() == "")
            deliveryOrder.Code = SetObjectCode();
            deliveryOrder.IsDeleted = false;
            deliveryOrder.IsConfirmed = false;
            deliveryOrder.CreatedAt = DateTime.Now;
            return Create(deliveryOrder);
        }

        public DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.UpdatedAt = DateTime.Now;
            Update(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.IsDeleted = true;
            deliveryOrder.DeletedAt = DateTime.Now;
            Update(deliveryOrder);
            return deliveryOrder;
        }

        public bool DeleteObject(int Id)
        {
            DeliveryOrder d = Find(x => x.Id == Id);
            return (Delete(d) == 1) ? true : false;
        }

        public DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.IsConfirmed = true;
            Update(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.IsConfirmed = false;
            deliveryOrder.ConfirmationDate = null;
            UpdateObject(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder SetInvoiceComplete(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.IsInvoiceCompleted = true;
            UpdateObject(deliveryOrder);
            return deliveryOrder;
        }

        public DeliveryOrder UnsetInvoiceComplete(DeliveryOrder deliveryOrder)
        {
            deliveryOrder.IsInvoiceCompleted = false;
            UpdateObject(deliveryOrder);
            return deliveryOrder;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}