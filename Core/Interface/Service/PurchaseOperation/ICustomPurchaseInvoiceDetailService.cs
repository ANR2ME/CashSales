using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ICustomPurchaseInvoiceDetailService
    {
        IQueryable<CustomPurchaseInvoiceDetail> GetQueryable();
        ICustomPurchaseInvoiceDetailValidator GetValidator();
        IList<CustomPurchaseInvoiceDetail> GetAll();
        IQueryable<CustomPurchaseInvoiceDetail> GetQueryableObjectsByCustomPurchaseInvoiceId(int CustomPurchaseInvoiceId);
        IList<CustomPurchaseInvoiceDetail> GetObjectsByCustomPurchaseInvoiceId(int customPurchaseInvoiceId);
        CustomPurchaseInvoiceDetail GetObjectById(int Id);
        CustomPurchaseInvoiceDetail CreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService);
        CustomPurchaseInvoiceDetail UpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService);
        CustomPurchaseInvoiceDetail SoftDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService);
        CustomPurchaseInvoiceDetail ConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                               IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService, IPriceMutationService _priceMutationService);
        CustomPurchaseInvoiceDetail UnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, IWarehouseItemService _warehouseItemService, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                                 IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService, IPriceMutationService _priceMutationService);
        bool DeleteObject(int Id);
        decimal CalculateTotal(int CustomPurchaseInvoiceId);
        decimal CalculateCoGS(int CustomPurchaseInvoiceId);
        decimal GetHighestPrice(/*int CustomPurchaseInvoiceId,*/ int ItemId);
    }
}
