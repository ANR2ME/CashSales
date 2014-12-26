using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICashSalesInvoiceDetailValidator
    {
        CashSalesInvoiceDetail VIsParentNotConfirmed(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService);
        CashSalesInvoiceDetail VHasCashSalesInvoice(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService);
        CashSalesInvoiceDetail VHasItem(CashSalesInvoiceDetail cashSalesInvoiceDetail, IItemService _itemService);
        CashSalesInvoiceDetail VUniqueItem(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService);
        CashSalesInvoiceDetail VIsValidQuantityOrdered(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        CashSalesInvoiceDetail VIsValidQuantity(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService);
        CashSalesInvoiceDetail VIsValidDiscount(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        CashSalesInvoiceDetail VIsValidAssignedPrice(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        //CashSalesInvoiceDetail VHasQuantityPricing(CashSalesInvoiceDetail cashSalesInvoiceDetail, IItemService _itemService, IQuantityPricingService _quantityPricingService);

        CashSalesInvoiceDetail VConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        CashSalesInvoiceDetail VUnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail);

        CashSalesInvoiceDetail VCreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                               ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                               IQuantityPricingService _quantityPricingService);
        CashSalesInvoiceDetail VUpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                                               ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                                               IQuantityPricingService _quantityPricingService);
        CashSalesInvoiceDetail VDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService);

        bool ValidConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        bool ValidUnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail);

        bool ValidCreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                               ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                               IQuantityPricingService _quantityPricingService);
        bool ValidUpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService,
                               ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService,
                               IQuantityPricingService _quantityPricingService);
        bool ValidDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail, ICashSalesInvoiceService _cashSalesInvoiceService);
        bool isValid(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        string PrintError(CashSalesInvoiceDetail cashSalesInvoiceDetail);
    }
}
