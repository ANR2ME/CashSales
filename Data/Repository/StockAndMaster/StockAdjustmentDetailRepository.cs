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
    public class StockAdjustmentDetailRepository : EfRepository<StockAdjustmentDetail>, IStockAdjustmentDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public StockAdjustmentDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<StockAdjustmentDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<StockAdjustmentDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<StockAdjustmentDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IQueryable<StockAdjustmentDetail> GetQueryableObjectsByStockAdjustmentId(int stockAdjustmentId)
        {
            return FindAll(x => x.StockAdjustmentId == stockAdjustmentId && !x.IsDeleted);
        }

        public IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int stockAdjustmentId)
        {
            return FindAll(x => x.StockAdjustmentId == stockAdjustmentId && !x.IsDeleted).ToList();
        }

        public IList<StockAdjustmentDetail> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId && !x.IsDeleted).ToList();
        }

        public StockAdjustmentDetail GetObjectById(int Id)
        {
            StockAdjustmentDetail detail = Find(x => x.Id == Id && !x.IsDeleted);
            detail.Errors = new Dictionary<string, string>();
            return detail;
        }

        public StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.StockAdjustments
                              where obj.Id == stockAdjustmentDetail.StockAdjustmentId
                              select obj.Code).FirstOrDefault();
            }
            if (stockAdjustmentDetail.Code == null || stockAdjustmentDetail.Code.Trim() == "")
            stockAdjustmentDetail.Code = SetObjectCode(ParentCode);
            stockAdjustmentDetail.IsConfirmed = false;
            stockAdjustmentDetail.IsDeleted = false;
            stockAdjustmentDetail.CreatedAt = DateTime.Now;
            return Create(stockAdjustmentDetail);
        }

        public StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.UpdatedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.IsDeleted = true;
            stockAdjustmentDetail.DeletedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public bool DeleteObject(int Id)
        {
            StockAdjustmentDetail stockAdjustmentDetail = Find(x => x.Id == Id);
            return (Delete(stockAdjustmentDetail) == 1) ? true : false;
        }

        public StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.IsConfirmed = true;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.IsConfirmed = false;
            stockAdjustmentDetail.ConfirmationDate = null;
            stockAdjustmentDetail.UpdatedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}