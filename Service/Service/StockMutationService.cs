using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interface.Validation;

namespace Service.Service
{
    public class StockMutationService : IStockMutationService
    {
        private IStockMutationRepository _repository;
        private IStockMutationValidator _validator;

        public StockMutationService(IStockMutationRepository _stockMutationRepository, IStockMutationValidator _stockMutationValidator)
        {
            _repository = _stockMutationRepository;
            _validator = _stockMutationValidator;
        }

        public IStockMutationValidator GetValidator()
        {
            return _validator;
        }

        public IList<StockMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<StockMutation> GetObjectsByWarehouseId(int warehouseId)
        {
            return _repository.GetObjectsByWarehouseId(warehouseId);
        }

        public IList<StockMutation> GetObjectsByWarehouseItemId(int warehouseItemId)
        {
            return _repository.GetObjectsByWarehouseItemId(warehouseItemId);
        }

        public StockMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<StockMutation> GetObjectsBySourceDocumentDetail(int itemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        {
            return _repository.GetObjectsBySourceDocumentDetail(itemId, SourceDocumentDetailType, SourceDocumentDetailId);
        }

        public StockMutation CreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            stockMutation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(stockMutation, _warehouseService, _warehouseItemService) ? _repository.CreateObject(stockMutation) : stockMutation);
        }

        public StockMutation UpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            return (_validator.ValidUpdateObject(stockMutation, _warehouseService, _warehouseItemService) ? _repository.UpdateObject(stockMutation) : stockMutation);
        }

        public StockMutation SoftDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            return (_validator.ValidDeleteObject(stockMutation, _warehouseService, _warehouseItemService) ? _repository.SoftDeleteObject(stockMutation) : stockMutation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        /*
        public StockMutation CreateStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = purchaseOrderDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.PurchaseOrder;
            stockMutation.SourceDocumentId = purchaseOrderDetail.PurchaseOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseOrderDetail;
            stockMutation.SourceDocumentDetailId = purchaseOrderDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.PendingReceival;
            stockMutation.Status = Constant.StockMutationStatus.Addition;
            return _repository.CreateObject(stockMutation);
        }
        
        public IList<StockMutation> SoftDeleteStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(item.Id, Constant.SourceDocumentDetailType.PurchaseOrderDetail, purchaseOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();
            
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = purchaseReceivalDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.PurchaseReceival;
            stockMutation.SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseReceivalDetail;
            stockMutation.SourceDocumentDetailId = purchaseReceivalDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.PendingReceival;
            stockMutation.Status = Constant.StockMutationStatus.Deduction;
            result.Add(_repository.CreateObject(stockMutation));

            stockMutation.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutation.Status = (purchaseReceivalDetail.Quantity > 0) ? Constant.StockMutationStatus.Addition : Constant.StockMutationStatus.Deduction;
            result.Add(_repository.CreateObject(stockMutation));
            return result;
        }

        public IList<StockMutation> SoftDeleteStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(item.Id, Constant.SourceDocumentDetailType.PurchaseReceivalDetail, purchaseReceivalDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = salesOrderDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.SalesOrder;
            stockMutation.SourceDocumentId = salesOrderDetail.SalesOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.SalesOrderDetail;
            stockMutation.SourceDocumentDetailId = salesOrderDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.PendingDelivery;
            stockMutation.Status = Constant.StockMutationStatus.Addition;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(item.Id, Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();

            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.ItemId = deliveryOrderDetail.ItemId;
            stockMutation.Quantity = deliveryOrderDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.DeliveryOrder;
            stockMutation.SourceDocumentId = deliveryOrderDetail.DeliveryOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.DeliveryOrderDetail;
            stockMutation.SourceDocumentDetailId = deliveryOrderDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.PendingDelivery;
            stockMutation.Status = Constant.StockMutationStatus.Deduction;
            result.Add(_repository.CreateObject(stockMutation));

            stockMutation.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutation.Status = Constant.StockMutationStatus.Deduction;
            result.Add(_repository.CreateObject(stockMutation));
            return result;
        }

        public IList<StockMutation> SoftDeleteStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(item.Id, Constant.SourceDocumentDetailType.DeliveryOrderDetail, deliveryOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }
        */

        public StockMutation CreateStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = (stockAdjustmentDetail.Quantity >= 0) ? stockAdjustmentDetail.Quantity : (-1) * stockAdjustmentDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.StockAdjustment;
            stockMutation.SourceDocumentId = stockAdjustmentDetail.StockAdjustmentId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.StockAdjustmentDetail;
            stockMutation.SourceDocumentDetailId = stockAdjustmentDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutation.Status = (stockAdjustmentDetail.Quantity >= 0) ? Constant.StockMutationStatus.Addition : Constant.StockMutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(warehouseItem.Id, Constant.SourceDocumentDetailType.StockAdjustmentDetail, stockAdjustmentDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = 1;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.CoreIdentification;
            stockMutation.SourceDocumentId = coreIdentificationDetail.CoreIdentificationId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.CoreIdentificationDetail;
            stockMutation.SourceDocumentDetailId = coreIdentificationDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutation.Status = Constant.StockMutationStatus.Addition;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(warehouseItem.Id, Constant.SourceDocumentDetailType.CoreIdentificationDetail, coreIdentificationDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = 1;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrder;
            stockMutation.SourceDocumentId = recoveryOrderDetail.RecoveryOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryOrderDetail;
            stockMutation.SourceDocumentDetailId = recoveryOrderDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutation.Status = CaseAddition ? Constant.StockMutationStatus.Addition : Constant.StockMutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(warehouseItem.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = recoveryAccessoryDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrderDetail;
            stockMutation.SourceDocumentId = recoveryAccessoryDetail.RecoveryOrderDetailId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryAccessoryDetail;
            stockMutation.SourceDocumentDetailId = recoveryAccessoryDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutation.Status = Constant.StockMutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(warehouseItem.Id, Constant.SourceDocumentDetailType.RecoveryAccessoryDetail, recoveryAccessoryDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForBarringOrder(BarringOrderDetail barringOrderDetail, WarehouseItem warehouseItem, bool CaseAddition)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = 1;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.BarringOrder;
            stockMutation.SourceDocumentId = barringOrderDetail.BarringOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BarringOrderDetail;
            stockMutation.SourceDocumentDetailId = barringOrderDetail.Id;
            stockMutation.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutation.Status = CaseAddition ? Constant.StockMutationStatus.Addition : Constant.StockMutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForBarringOrder(BarringOrderDetail barringOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetail(warehouseItem.Id, Constant.SourceDocumentDetailType.BarringOrderDetail, barringOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            StockMutation stockMutationFrom = new StockMutation();
            stockMutationFrom.WarehouseId = warehouseItemFrom.WarehouseId;
            stockMutationFrom.WarehouseItemId = warehouseItemFrom.Id;
            stockMutationFrom.Quantity = 1;
            stockMutationFrom.SourceDocumentType = Constant.SourceDocumentType.RollerWarehouseMutation;
            stockMutationFrom.SourceDocumentId = rollerWarehouseMutationDetail.RollerWarehouseMutationId;
            stockMutationFrom.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail;
            stockMutationFrom.SourceDocumentDetailId = rollerWarehouseMutationDetail.Id;
            stockMutationFrom.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutationFrom.Status = Constant.StockMutationStatus.Deduction;
            stockMutationFrom = _repository.CreateObject(stockMutationFrom);

            StockMutation stockMutationTo = new StockMutation();
            stockMutationTo.WarehouseId = warehouseItemTo.WarehouseId;
            stockMutationTo.WarehouseItemId = warehouseItemTo.Id;
            stockMutationTo.Quantity = 1;
            stockMutationTo.SourceDocumentType = Constant.SourceDocumentType.RollerWarehouseMutation;
            stockMutationTo.SourceDocumentId = rollerWarehouseMutationDetail.RollerWarehouseMutationId;
            stockMutationTo.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail;
            stockMutationTo.SourceDocumentDetailId = rollerWarehouseMutationDetail.Id;
            stockMutationTo.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutationTo.Status = Constant.StockMutationStatus.Addition;
            stockMutationTo = _repository.CreateObject(stockMutationTo);

            stockMutations.Add(stockMutationFrom);
            stockMutations.Add(stockMutationTo);
            return stockMutations;
        }

        public IList<StockMutation> SoftDeleteStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            IList<StockMutation> stockMutationFrom = _repository.GetObjectsBySourceDocumentDetail(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationFrom)
            {
                _repository.Delete(stockMutation);
            }

            IList<StockMutation> stockMutationTo = _repository.GetObjectsBySourceDocumentDetail(warehouseItemTo.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationTo)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            StockMutation stockMutationFrom = new StockMutation();
            stockMutationFrom.WarehouseId = warehouseItemFrom.WarehouseId;
            stockMutationFrom.WarehouseItemId = warehouseItemFrom.Id;
            stockMutationFrom.Quantity = warehouseMutationOrderDetail.Quantity;
            stockMutationFrom.SourceDocumentType = Constant.SourceDocumentType.WarehouseMutationOrder;
            stockMutationFrom.SourceDocumentId = warehouseMutationOrderDetail.WarehouseMutationOrderId;
            stockMutationFrom.SourceDocumentDetailType = Constant.SourceDocumentDetailType.WarehouseMutationOrderDetail;
            stockMutationFrom.SourceDocumentDetailId = warehouseMutationOrderDetail.Id;
            stockMutationFrom.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutationFrom.Status = Constant.StockMutationStatus.Deduction;
            stockMutationFrom = _repository.CreateObject(stockMutationFrom);

            StockMutation stockMutationTo = new StockMutation();
            stockMutationTo.WarehouseId = warehouseItemTo.WarehouseId;
            stockMutationTo.WarehouseItemId = warehouseItemTo.Id;
            stockMutationTo.Quantity = warehouseMutationOrderDetail.Quantity;
            stockMutationTo.SourceDocumentType = Constant.SourceDocumentType.WarehouseMutationOrder;
            stockMutationTo.SourceDocumentId = warehouseMutationOrderDetail.WarehouseMutationOrderId;
            stockMutationTo.SourceDocumentDetailType = Constant.SourceDocumentDetailType.WarehouseMutationOrderDetail;
            stockMutationTo.SourceDocumentDetailId = warehouseMutationOrderDetail.Id;
            stockMutationTo.ItemCase = Constant.StockMutationItemCase.Ready;
            stockMutationTo.Status = Constant.StockMutationStatus.Addition;
            stockMutationTo = _repository.CreateObject(stockMutationTo);

            stockMutations.Add(stockMutationFrom);
            stockMutations.Add(stockMutationTo);
            return stockMutations;
        }

        public IList<StockMutation> SoftDeleteStockMutationForWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            IList<StockMutation> stockMutationFrom = _repository.GetObjectsBySourceDocumentDetail(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.WarehouseMutationOrderDetail, warehouseMutationOrderDetail.Id);
            stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationFrom)
            {
                _repository.Delete(stockMutation);
            }

            IList<StockMutation> stockMutationTo = _repository.GetObjectsBySourceDocumentDetail(warehouseItemTo.Id, Constant.SourceDocumentDetailType.WarehouseMutationOrderDetail, warehouseMutationOrderDetail.Id);
            stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationTo)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

    }
}