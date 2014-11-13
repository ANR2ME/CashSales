using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICashSalesReturnDetailValidator
    {
        CashSalesReturnDetail VIsParentNotConfirmed(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService);
        CashSalesReturnDetail VHasCashSalesReturn(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService);
        CashSalesReturnDetail VIsValidCashSalesInvoiceDetail(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnService _cashSalesReturnService);
        CashSalesReturnDetail VIsValidQuantity(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        //CashSalesReturnDetail VIsValidTotalPrice(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);

        CashSalesReturnDetail VConfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        CashSalesReturnDetail VUnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail, IWarehouseItemService _warehouseItemService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);

        CashSalesReturnDetail VCreateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                               ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesReturnDetail VUpdateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                                               ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        CashSalesReturnDetail VDeleteObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService);

        bool ValidConfirmObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, ICashSalesReturnDetailService _cashSalesReturnDetailService);
        bool ValidUnconfirmObject(CashSalesReturnDetail cashSalesReturnDetail, IWarehouseItemService _warehouseItemService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);

        bool ValidCreateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                               ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        bool ValidUpdateObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService,
                               ICashSalesReturnDetailService _cashSalesReturnDetailService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService);
        bool ValidDeleteObject(CashSalesReturnDetail cashSalesReturnDetail, ICashSalesReturnService _cashSalesReturnService);
        bool isValid(CashSalesReturnDetail cashSalesReturnDetail);
        string PrintError(CashSalesReturnDetail cashSalesReturnDetail);
    }
}
