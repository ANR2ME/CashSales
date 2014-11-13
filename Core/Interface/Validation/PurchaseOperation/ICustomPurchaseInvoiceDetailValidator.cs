using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICustomPurchaseInvoiceDetailValidator
    {
        CustomPurchaseInvoiceDetail VIsValidDiscount(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        CustomPurchaseInvoiceDetail VIsValidListedUnitPrice(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        CustomPurchaseInvoiceDetail VIsParentNotConfirmed(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService);
        CustomPurchaseInvoiceDetail VHasCustomPurchaseInvoice(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService);
        CustomPurchaseInvoiceDetail VHasItem(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, IItemService _itemService);
        CustomPurchaseInvoiceDetail VUniqueItem(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService);
        //CustomPurchaseInvoiceDetail VIsValidQuantityOrdered(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        CustomPurchaseInvoiceDetail VIsValidQuantity(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);

        CustomPurchaseInvoiceDetail VConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        CustomPurchaseInvoiceDetail VUnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);

        CustomPurchaseInvoiceDetail VCreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                               ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        CustomPurchaseInvoiceDetail VUpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                               ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        CustomPurchaseInvoiceDetail VDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService);

        bool ValidConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService);

        bool ValidCreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                               ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                               ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService);
        bool isValid(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
        string PrintError(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail);
    }
}
