﻿using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class RecoveryOrderRepository : EfRepository<RecoveryOrder>, IRecoveryOrderRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public RecoveryOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<RecoveryOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<RecoveryOrder> GetAllObjectsInHouse()
        {
            using (var db = GetContext())
            {
                IList<CoreIdentification> cores = (from obj in db.CoreIdentifications
                                                   where obj.IsInHouse
                                                   select obj).ToList();
                IList<RecoveryOrder> orders = new List<RecoveryOrder>();
                foreach (var core in cores)
                {
                    IList<RecoveryOrder> order = (from obj in db.RecoveryOrders
                                                  where obj.CoreIdentificationId == core.Id
                                                  select obj).ToList();
                    orders.Concat(order);
                }
                return orders;
            }
        }

        public IList<RecoveryOrder> GetAllObjectsByCustomerId(int CustomerId)
        {
            using (var db = GetContext())
            {
                IList<CoreIdentification> cores = (from obj in db.CoreIdentifications
                                                   where obj.CustomerId == CustomerId
                                                   select obj).ToList();
                IList<RecoveryOrder> orders = new List<RecoveryOrder>();
                foreach (var core in cores)
                {
                    IList<RecoveryOrder> order = (from obj in db.RecoveryOrders
                                                  where obj.CoreIdentificationId == core.Id
                                                  select obj).ToList();
                    orders.Concat(order);
                }
                return orders;
            }
        }

        public IList<RecoveryOrder> GetObjectsByCoreIdentificationId(int coreIdentificationId)
        {
            return FindAll(x => x.CoreIdentificationId == coreIdentificationId && !x.IsDeleted).ToList();
        }

        public RecoveryOrder GetObjectById(int Id)
        {
            RecoveryOrder recoveryOrder = Find(x => x.Id == Id && !x.IsDeleted);
            if (recoveryOrder != null) { recoveryOrder.Errors = new Dictionary<string, string>(); }
            return recoveryOrder;
        }

        public RecoveryOrder CreateObject(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.QuantityFinal = 0;
            recoveryOrder.QuantityRejected = 0;
            recoveryOrder.IsConfirmed = false;
            recoveryOrder.IsDeleted = false;
            recoveryOrder.CreatedAt = DateTime.Now;
            return Create(recoveryOrder);
        }

        public RecoveryOrder UpdateObject(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.UpdatedAt = DateTime.Now;
            Update(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder SoftDeleteObject(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.IsDeleted = true;
            recoveryOrder.DeletedAt = DateTime.Now;
            Update(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder ConfirmObject(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.IsConfirmed = true;
            recoveryOrder.ConfirmationDate = DateTime.Now;
            Update(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.IsConfirmed = false;
            recoveryOrder.ConfirmationDate = null;
            recoveryOrder.UpdatedAt = DateTime.Now;
            Update(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder CompleteObject(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.IsCompleted = true;
            recoveryOrder.UpdatedAt = DateTime.Now;
            Update(recoveryOrder);
            return recoveryOrder;
        }

        public bool DeleteObject(int Id)
        {
            RecoveryOrder recoveryOrder = Find(x => x.Id == Id);
            return (Delete(recoveryOrder) == 1) ? true : false;
        }
    }
}