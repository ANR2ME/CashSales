using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IStockAdjustmentService
    {
        IQueryable<StockAdjustment> GetQueryable();
        IStockAdjustmentValidator GetValidator();
        IList<StockAdjustment> GetAll();
        StockAdjustment GetObjectById(int Id);
        StockAdjustment CreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment CreateObject(int WarehouseId, DateTime AdjustmentDate, IWarehouseService _warehouseService);
        StockAdjustment UpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool DeleteObject(int Id);
        StockAdjustment ConfirmObject(StockAdjustment stockAdjustment, DateTime ConfirmationDate, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                      IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService,
                                      IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);
        StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                       IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService);

    }
}