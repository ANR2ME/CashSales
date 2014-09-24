using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IStockAdjustmentValidator
    {
        StockAdjustment VHasWarehouse(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment VAdjustmentDate(StockAdjustment stockAdjustment);
        StockAdjustment VHasBeenConfirmed(StockAdjustment stockAdjustment);
        StockAdjustment VHasNotBeenConfirmed(StockAdjustment stockAdjustment);
        StockAdjustment VHasStockAdjustmentDetails(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustment VDetailsAreVerifiedConfirmable(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        StockAdjustment VCreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment VUpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment VDeleteObject(StockAdjustment stockAdjustment);
        StockAdjustment VHasConfirmationDate(StockAdjustment stockAdjustment);
        StockAdjustment VConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        StockAdjustment VUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool ValidCreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        bool ValidUpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        bool ValidDeleteObject(StockAdjustment stockAdjustment);
        bool ValidConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool ValidUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool isValid(StockAdjustment stockAdjustment);
        string PrintError(StockAdjustment stockAdjustment);
    }
}
