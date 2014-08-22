﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface ICashSalesReturnDetailService
    {
        ICashSalesReturnDetailValidator GetValidator();
        IList<CashSalesReturnDetail> GetAll();
        IList<CashSalesReturnDetail> GetObjectsByCashSalesReturnId(int cashSalesReturnId);
        CashSalesReturnDetail GetObjectById(int Id);
        IList<CashSalesReturnDetail> GetObjectsByCashSalesInvoiceDetailId(int CashSalesInvoiceDetailId);
        CashSalesReturnDetail CreateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                           ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesReturnDetail UpdateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                           ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesReturnDetail SoftDeleteObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService);
        CashSalesReturnDetail ConfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                            ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                            IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                            IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        CashSalesReturnDetail UnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                              IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                              IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
        decimal CalculateTotal(int CashSalesReturnId);
        int GetTotalQuantityByCashSalesInvoiceDetailId(int Id);
    }
}
