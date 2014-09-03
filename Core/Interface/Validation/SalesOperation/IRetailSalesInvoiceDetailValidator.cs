using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IRetailSalesInvoiceDetailValidator
    {
        RetailSalesInvoiceDetail VIsParentNotConfirmed(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoiceDetail VHasRetailSalesInvoice(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService);
        RetailSalesInvoiceDetail VHasItem(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IItemService _itemService);
        RetailSalesInvoiceDetail VUniqueItem(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService);
        RetailSalesInvoiceDetail VIsValidQuantityOrdered(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService);
        RetailSalesInvoiceDetail VIsValidQuantity(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService);
        RetailSalesInvoiceDetail VIsValidDiscount(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        RetailSalesInvoiceDetail VIsValidAssignedPrice(RetailSalesInvoiceDetail retailSalesInvoiceDetail);

        RetailSalesInvoiceDetail VConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService);
        RetailSalesInvoiceDetail VUnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);

        RetailSalesInvoiceDetail VCreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                               IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RetailSalesInvoiceDetail VUpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                               IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RetailSalesInvoiceDetail VDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService);

        bool ValidConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);

        bool ValidCreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                               IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                               IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService);
        bool isValid(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        string PrintError(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
    }
}
