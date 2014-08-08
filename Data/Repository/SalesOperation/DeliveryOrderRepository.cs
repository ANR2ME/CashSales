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

        public IList<DeliveryOrder> GetAll()
        {
            return FindAll(d => !d.IsDeleted).ToList();
        }

        public DeliveryOrder GetObjectById(int Id)
        {
            DeliveryOrder deliveryOrder = Find(d => d.Id == Id && !d.IsDeleted);
            if (deliveryOrder != null) { deliveryOrder.Errors = new Dictionary<string, string>(); }
            return deliveryOrder;
        }

        public IList<DeliveryOrder> GetObjectsBySalesOrderId(int salesOrderId)
        {
            return FindAll(d => d.SalesOrderId == salesOrderId && !d.IsDeleted).ToList();
        }

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder)
        {
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
            // Code: #{year}/#{total_number
            int totalobject = FindAll().Count() + 1;
            string Code = "#" + DateTime.Now.Year.ToString() + "/#" + totalobject;
            return Code;
        }
    }
}