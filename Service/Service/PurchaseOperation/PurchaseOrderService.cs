using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private IPurchaseOrderRepository _repository;
        private IPurchaseOrderValidator _validator;

        public PurchaseOrderService(IPurchaseOrderRepository _purchaseOrderRepository, IPurchaseOrderValidator _purchaseOrderValidator)
        {
            _repository = _purchaseOrderRepository;
            _validator = _purchaseOrderValidator;
        }

        public IPurchaseOrderValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseOrder> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseOrder> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public IQueryable<PurchaseOrder> GetQueryableConfirmedObjects()
        {
            return _repository.GetQueryableConfirmedObjects();
        }

        public IList<PurchaseOrder> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public PurchaseOrder CreateObject(PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            purchaseOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseOrder, _contactService) ? _repository.CreateObject(purchaseOrder) : purchaseOrder);
        }

        public PurchaseOrder CreateObject(int contactId, DateTime purchaseDate, IContactService _contactService)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder
            {
                ContactId = contactId,
                PurchaseDate = purchaseDate
            };
            return this.CreateObject(purchaseOrder, _contactService);
        }

        public PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(purchaseOrder, _contactService) ? _repository.UpdateObject(purchaseOrder) : purchaseOrder);
        }

        public PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseOrder, _purchaseOrderDetailService) ? _repository.SoftDeleteObject(purchaseOrder) : purchaseOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder, DateTime ConfirmationDate, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                    IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService,
                                    IWarehouseItemService _warehouseItemService, IPurchaseOrderService _purchaseOrderService)
        {
            purchaseOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseOrder, _purchaseOrderDetailService))
            {
                IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
                foreach (var detail in purchaseOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseOrderDetailService.ConfirmObject(detail, ConfirmationDate, _stockMutationService, _itemService, _barringService, _warehouseItemService, _purchaseOrderService);
                }
                _repository.ConfirmObject(purchaseOrder);
            }
            return purchaseOrder;
        }

        public PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalService _purchaseReceivalService,
                                             IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService,
                                             IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(purchaseOrder, _purchaseReceivalService))
            {
                IList<PurchaseOrderDetail> purchaseOrderDetails = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);
                foreach (var detail in purchaseOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseOrderDetailService.UnconfirmObject(detail, _purchaseReceivalDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                }
                _repository.UnconfirmObject(purchaseOrder);
            }
            return purchaseOrder;
        }

        public PurchaseOrder CheckAndSetReceivalComplete(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService)
        {
            IList<PurchaseOrderDetail> details = _purchaseOrderDetailService.GetObjectsByPurchaseOrderId(purchaseOrder.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllReceived)
                {
                    return purchaseOrder;
                }
            }
            return _repository.SetReceivalComplete(purchaseOrder);
        }

        public PurchaseOrder UnsetReceivalComplete(PurchaseOrder purchaseOrder)
        {
            return _repository.UnsetReceivalComplete(purchaseOrder);
        }
    }
}