using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Data.Entity;

namespace Data.Repository
{
    public class ItemRepository : EfRepository<Item>, IItemRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ItemRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Item> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Item> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<Item> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return FindAll(x => x.ItemTypeId == ItemTypeId && !x.IsDeleted).ToList();
        }

        public IQueryable<Item> GetQueryableObjectsByItemTypeId(int ItemTypeId)
        {
            return FindAll(x => x.ItemTypeId == ItemTypeId && !x.IsDeleted);
        }

        public IList<Item> GetObjectsByUoMId(int UoMId)
        {
            return FindAll(x => x.UoMId == UoMId && !x.IsDeleted).ToList();
        }

        public Item GetObjectById(int Id)
        {
            Item item = Find(x => x.Id == Id && !x.IsDeleted);
            if (item != null) { item.Errors = new Dictionary<string, string>(); }
            return item;
        }

        public Item GetObjectBySku(string Sku)
        {
            return FindAll(i => i.Sku == Sku && !i.IsDeleted).FirstOrDefault();
        }

        public Item CreateObject(Item item)
        {
            item.Quantity = 0;
            item.IsDeleted = false;
            item.CreatedAt = DateTime.Now;
            return Create(item);
        }

        public Item UpdateObject(Item item)
        {
            item.UpdatedAt = DateTime.Now;
            Update(item);
            return item;
        }

        public Item SoftDeleteObject(Item item)
        {
            item.IsDeleted = true;
            item.DeletedAt = DateTime.Now;
            Update(item);
            return item;
        }

        public bool DeleteObject(int Id)
        {
            Item item = Find(x => x.Id == Id);
            return (Delete(item) == 1) ? true : false;
        }

        public bool IsSkuDuplicated(Item item)
        {
            IQueryable<Item> items = FindAll(x => x.Sku == item.Sku && !x.IsDeleted && x.Id != item.Id);
            return (items.Count() > 0 ? true : false);
        }
    }
}