using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseOrderDetailService
    {
        IQueryable<PurchaseOrderDetail> GetQueryable();
        IList<PurchaseOrderDetail> GetAll();
        IPurchaseOrderDetailValidator GetValidator();
        IQueryable<PurchaseOrderDetail> GetQueryableObjectsByPurchaseOrderId(int purchaseOrderId);
        IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        IList<PurchaseOrderDetail> GetObjectsByItemId(int itemId);
        PurchaseOrderDetail GetObjectById(int Id);
        PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail CreateObject(int purchaseOrderId, int itemId, int quantity, decimal Price, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        bool DeleteObject(int Id);
        PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail, DateTime ConfirmationDate, IStockMutationService _stockMutationService,
                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IPurchaseOrderService _purchaseOrderService);
        PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                           IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService,
                                           IWarehouseItemService _warehouseItemService);
        PurchaseOrderDetail SetReceivalComplete(PurchaseOrderDetail purchaseOrderDetail, int Quantity);
        PurchaseOrderDetail UnsetReceivalComplete(PurchaseOrderDetail purchaseOrderDetail, int Quantity, IPurchaseOrderService _purchaseOrderService);
   }
}