using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IRetailSalesInvoiceDetailService
    {
        IQueryable<RetailSalesInvoiceDetail> GetQueryable();
        IRetailSalesInvoiceDetailValidator GetValidator();
        IList<RetailSalesInvoiceDetail> GetAll();
        IQueryable<RetailSalesInvoiceDetail> GetQueryableObjectsByRetailSalesInvoiceId(int RetailSalesInvoiceId);
        IList<RetailSalesInvoiceDetail> GetObjectsByRetailSalesInvoiceId(int retailSalesInvoiceId);
        RetailSalesInvoiceDetail GetObjectById(int Id);
        RetailSalesInvoiceDetail CreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService);
        RetailSalesInvoiceDetail UpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService);
        RetailSalesInvoiceDetail SoftDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoiceDetail ConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                               IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        RetailSalesInvoiceDetail UnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                 IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
        decimal CalculateTotal(int RetailSalesInvoiceId);
        decimal CalculateCoGS(int RetailSalesInvoiceId);
    }
}
