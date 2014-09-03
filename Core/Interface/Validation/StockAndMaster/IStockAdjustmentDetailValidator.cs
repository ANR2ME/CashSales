using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IStockAdjustmentDetailValidator
    {
        StockAdjustmentDetail VHasStockAdjustment(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentService _StockAdjustmentService);
        StockAdjustmentDetail VHasItem(StockAdjustmentDetail StockAdjustmentDetail, IItemService _itemService);
        StockAdjustmentDetail VHasWarehouseItem(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail VNonZeroQuantity(StockAdjustmentDetail StockAdjustmentDetail);
        StockAdjustmentDetail VNonNegativePrice(StockAdjustmentDetail stockAdjustmentDetail); // VNonZeroNorNegativePrice
        StockAdjustmentDetail VNonNegativeItemAvgPrice(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService);
        StockAdjustmentDetail VUniqueItem(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentDetailService _StockAdjustmentDetails, IItemService _itemService);
        StockAdjustmentDetail VHasBeenConfirmed(StockAdjustmentDetail StockAdjustmentDetail);
        StockAdjustmentDetail VHasNotBeenConfirmed(StockAdjustmentDetail StockAdjustmentDetail);
        StockAdjustmentDetail VNonNegativeStockQuantity(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                                        IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool ToConfirm);
        StockAdjustmentDetail VHasConfirmationDate(StockAdjustmentDetail stockAdjustmentDetail);
        StockAdjustmentDetail VCreateObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentDetailService _StockAdjustmentDetails, IStockAdjustmentService _StockAdjustmentService,
                                            IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail VUpdateObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentDetailService _StockAdjustmentDetails, IStockAdjustmentService _StockAdjustmentService,
                                            IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail VDeleteObject(StockAdjustmentDetail StockAdjustmentDetail);
        StockAdjustmentDetail VConfirmObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail VUnconfirmObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentDetailService _StockAdjustmentDetails, IStockAdjustmentService _StockAdjustmentService,
                               IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentDetailService _StockAdjustmentDetails, IStockAdjustmentService _StockAdjustmentService,
                               IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(StockAdjustmentDetail StockAdjustmentDetail);
        bool ValidConfirmObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(StockAdjustmentDetail StockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool isValid(StockAdjustmentDetail StockAdjustmentDetail);
        string PrintError(StockAdjustmentDetail StockAdjustmentDetail);
    }
}
