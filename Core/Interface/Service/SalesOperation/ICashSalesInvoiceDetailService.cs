using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ICashSalesInvoiceDetailService
    {
        IQueryable<CashSalesInvoiceDetail> GetQueryable();
        ICashSalesInvoiceDetailValidator GetValidator();
        IList<CashSalesInvoiceDetail> GetAll();
        IQueryable<CashSalesInvoiceDetail> GetQueryableObjectsByCashSalesInvoiceId(int CashSalesInvoiceId);
        IList<CashSalesInvoiceDetail> GetObjectsByCashSalesInvoiceId(int cashSalesInvoiceId);
        CashSalesInvoiceDetail GetObjectById(int Id);
        CashSalesInvoiceDetail CreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                              IQuantityPricingService _quantityPricingService);
        CashSalesInvoiceDetail UpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                              IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                              IQuantityPricingService _quantityPricingService);
        CashSalesInvoiceDetail SoftDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService);
        CashSalesInvoiceDetail ConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        CashSalesInvoiceDetail UnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                 IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
        decimal CalculateTotal(int CashSalesInvoiceId);
        decimal CalculateCoGS(int CashSalesInvoiceId);
    }
}
