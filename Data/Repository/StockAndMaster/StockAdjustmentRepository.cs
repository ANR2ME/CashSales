using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class StockAdjustmentRepository : EfRepository<StockAdjustment>, IStockAdjustmentRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public StockAdjustmentRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<StockAdjustment> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<StockAdjustment> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<StockAdjustment> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public StockAdjustment GetObjectById(int Id)
        {
            StockAdjustment stockAdjustment = Find(x => x.Id == Id && !x.IsDeleted);
            if (stockAdjustment != null) { stockAdjustment.Errors = new Dictionary<string, string>(); }
            return stockAdjustment;
        }

        public StockAdjustment CreateObject(StockAdjustment stockAdjustment)
        {
            if (stockAdjustment.Code == null || stockAdjustment.Code.Trim() == "")
            stockAdjustment.Code = SetObjectCode();
            stockAdjustment.IsDeleted = false;
            stockAdjustment.IsConfirmed = false;
            stockAdjustment.CreatedAt = DateTime.Now;
            return Create(stockAdjustment);
        }

        public StockAdjustment UpdateObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.UpdatedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsDeleted = true;
            stockAdjustment.DeletedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment ConfirmObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsConfirmed = true;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsConfirmed = false;
            stockAdjustment.ConfirmationDate = null;
            stockAdjustment.UpdatedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public bool DeleteObject(int Id)
        {
            StockAdjustment stockAdjustment = Find(x => x.Id == Id);
            return (Delete(stockAdjustment) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}