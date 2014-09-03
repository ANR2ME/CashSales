using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IRetailPurchaseInvoiceDetailValidator
    {
        RetailPurchaseInvoiceDetail VIsParentNotConfirmed(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService);
        RetailPurchaseInvoiceDetail VHasRetailPurchaseInvoice(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService);
        RetailPurchaseInvoiceDetail VHasItem(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IItemService _itemService);
        RetailPurchaseInvoiceDetail VUniqueItem(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService);
        RetailPurchaseInvoiceDetail VIsValidQuantityOrdered(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        RetailPurchaseInvoiceDetail VIsValidQuantity(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);

        RetailPurchaseInvoiceDetail VConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        RetailPurchaseInvoiceDetail VUnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);

        RetailPurchaseInvoiceDetail VCreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                               IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RetailPurchaseInvoiceDetail VUpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                               IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RetailPurchaseInvoiceDetail VDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService);

        bool ValidConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);

        bool ValidCreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                               IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                               IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService);
        bool isValid(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);
        string PrintError(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);
    }
}
