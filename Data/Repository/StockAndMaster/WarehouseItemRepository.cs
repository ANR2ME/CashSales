using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class WarehouseItemRepository : EfRepository<WarehouseItem>, IWarehouseItemRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public WarehouseItemRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<WarehouseItem> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<WarehouseItem> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IQueryable<WarehouseItem> GetQueryableObjectsByWarehouseId(int warehouseId)
        {
            return FindAll(x => x.WarehouseId == warehouseId && !x.IsDeleted);
        }

        public IList<WarehouseItem> GetObjectsByWarehouseId(int warehouseId)
        {
            return FindAll(x => x.WarehouseId == warehouseId && !x.IsDeleted).ToList();
        }

        public IQueryable<WarehouseItem> GetQueryableObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId);
        }

        public IList<WarehouseItem> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId).ToList();
        }

        public WarehouseItem GetObjectById(int Id)
        {
            WarehouseItem warehouseItem = Find(x => x.Id == Id && !x.IsDeleted);
            if (warehouseItem != null) { warehouseItem.Errors = new Dictionary<string, string>(); }
            return warehouseItem;
        }

        public WarehouseItem FindOrCreateObject(int WarehouseId, int ItemId)
        {
            WarehouseItem warehouseItem = Find(x => x.WarehouseId == WarehouseId && x.ItemId == ItemId && !x.IsDeleted);
            if (warehouseItem == null)
            {
                warehouseItem = new WarehouseItem()
                {
                    WarehouseId = WarehouseId,
                    ItemId = ItemId,
                };
                warehouseItem = CreateObject(warehouseItem);
            }
            warehouseItem.Errors = new Dictionary<string, string>();
            return warehouseItem;
        }

        public WarehouseItem CreateObject(WarehouseItem warehouseItem)
        {
            warehouseItem.Quantity = 0;
            warehouseItem.PendingDelivery = 0;
            warehouseItem.PendingReceival = 0;
            warehouseItem.IsDeleted = false;
            warehouseItem.CreatedAt = DateTime.Now;
            return Create(warehouseItem);
        }

        public WarehouseItem UpdateObject(WarehouseItem warehouseItem)
        {
            warehouseItem.UpdatedAt = DateTime.Now;
            Update(warehouseItem);
            return warehouseItem;
        }

        public WarehouseItem SoftDeleteObject(WarehouseItem warehouseItem)
        {
            warehouseItem.IsDeleted = true;
            warehouseItem.DeletedAt = DateTime.Now;
            Update(warehouseItem);
            return warehouseItem;
        }

        public bool DeleteObject(int Id)
        {
            WarehouseItem warehouseItem =  Find(x => x.Id == Id);
            return (Delete(warehouseItem) == 1) ? true : false;
        }
    }
}