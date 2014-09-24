using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseOrderService
    {
        IQueryable<PurchaseOrder> GetQueryable();
        IPurchaseOrderValidator GetValidator();
        IList<PurchaseOrder> GetAll();
        PurchaseOrder GetObjectById(int Id);
        IList<PurchaseOrder> GetObjectsByContactId(int contactId);
        IQueryable<PurchaseOrder> GetQueryableConfirmedObjects();
        IList<PurchaseOrder> GetConfirmedObjects();
        PurchaseOrder CreateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder CreateObject(int contactId, DateTime purchaseDate, IContactService _contactService);
        PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool DeleteObject(int Id);
        PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder, DateTime ConfirmationDate, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                    IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService,
                                    IWarehouseItemService _warehouseItemService);
        PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalService _purchaseReceivalService,
                                    IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService,
                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        PurchaseOrder CheckAndSetReceivalComplete(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder UnsetReceivalComplete(PurchaseOrder purchaseOrder);
    }
}