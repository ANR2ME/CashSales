using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IStockAdjustmentDetailService
    {
        IQueryable<StockAdjustmentDetail> GetQueryable();
        IList<StockAdjustmentDetail> GetAll();
        IStockAdjustmentDetailValidator GetValidator();
        IQueryable<StockAdjustmentDetail> GetQueryableObjectsByStockAdjustmentId(int stockAdjustmentId);
        IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int stockAdjustmentId);
        IList<StockAdjustmentDetail> GetObjectsByItemId(int itemId);
        StockAdjustmentDetail GetObjectById(int Id);
        StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail CreateObject(int stockAdjustmentId, int itemId, int quantity, decimal price, IStockAdjustmentService _stockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail);
        bool DeleteObject(int Id);
        StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, DateTime ConfirmationDate, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
    }
}