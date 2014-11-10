using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class DeliveryOrderDetailRepository : EfRepository<DeliveryOrderDetail>, IDeliveryOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public DeliveryOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<DeliveryOrderDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<DeliveryOrderDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<DeliveryOrderDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IQueryable<DeliveryOrderDetail> GetQueryableObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return FindAll(x => x.DeliveryOrderId == deliveryOrderId && !x.IsDeleted);
        }

        public IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return FindAll(x => x.DeliveryOrderId == deliveryOrderId && !x.IsDeleted).ToList();
        }

        public DeliveryOrderDetail GetObjectById(int Id)
        {
            return Find(x => x.Id == Id && !x.IsDeleted);
        }

        public IQueryable<DeliveryOrderDetail> GetQueryableObjectsBySalesOrderDetailId(int salesOrderDetailId)
        {
            return FindAll(x => x.SalesOrderDetailId == salesOrderDetailId && !x.IsDeleted);
        }

        public IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId)
        {
            return FindAll(x => x.SalesOrderDetailId == salesOrderDetailId && !x.IsDeleted).ToList();
        }

        public DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            string ParentCode = ""; 
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.DeliveryOrders
                              where obj.Id == deliveryOrderDetail.DeliveryOrderId
                              select obj.Code).FirstOrDefault();
            }
            if (deliveryOrderDetail.Code == null || deliveryOrderDetail.Code.Trim() == "")
            deliveryOrderDetail.Code = SetObjectCode(ParentCode);
            deliveryOrderDetail.IsConfirmed = false;
            deliveryOrderDetail.IsDeleted = false;
            deliveryOrderDetail.IsAllInvoiced = false;
            deliveryOrderDetail.PendingInvoicedQuantity = deliveryOrderDetail.Quantity;
            deliveryOrderDetail.CreatedAt = DateTime.Now;
            return Create(deliveryOrderDetail);
        }

        public DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.IsDeleted = true;
            deliveryOrderDetail.DeletedAt = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            DeliveryOrderDetail dod = Find(x => x.Id == Id);
            return (Delete(dod) == 1) ? true : false;
        }

        public DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.IsConfirmed = true;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.IsConfirmed = false;
            deliveryOrderDetail.ConfirmationDate = null;
            UpdateObject(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + totalnumberinthemonth;
            return Code;
        } 
    }
}