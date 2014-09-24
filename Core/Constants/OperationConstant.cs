using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Constants
{
    public partial class Constant
    {
        public static string BaseContact = "BaseContact";
        public static TimeSpan PaymentDueDateTimeSpan = new TimeSpan(14, 0, 0);

        public class GroupType
        {
            public static string Base = "Base";
        }

        public class MenuGroupName
        {
            public static string Master = "Master";
            public static string Transaction = "Transaction";
            public static string Report = "Report";
            public static string Setting = "Setting";
        }

        public class MenuName
        {
            public static string Contact = "Contact";
            public static string ItemType = "ItemType";
            public static string UoM = "UoM";
            public static string QuantityPricing = "Quantity Pricing";
            public static string CashBank = "CashBank";
            public static string CashBankAdjustment = "CashBank Adjustment";
            public static string CashBankMutation = "CashBank Mutation";
            public static string CashMutation = "Cash Mutation";
            public static string PaymentRequest = "Payment Request";

            public static string Item = "Item";
            public static string StockAdjustment = "Stock Adjustment";
            public static string StockMutation = "Stock Mutation";
            public static string Warehouse = "Warehouse";
            public static string WarehouseItem = "WarehouseItem";
            public static string WarehouseMutation = "Warehouse Mutation";

            public static string PurchaseOrder = "Purchase Order";
            public static string PurchaseReceival = "Purchase Receival";
            public static string PurchaseInvoice = "Purchase Invoice";
            public static string CustomPurchaseInvoice = "Custom Purchase Invoice";
            public static string PaymentVoucher = "Payment Voucher";
            public static string Payable = "Payable";

            public static string SalesOrder = "Sales Order";
            public static string DeliveryOrder = "Delivery Order";
            public static string SalesInvoice = "Sales Invoice";
            public static string CashSalesInvoice = "Cash Sales Invoice";
            public static string CashSalesReturn = "Cash Sales Return";

            public static string RetailSalesInvoice = "Retail Sales Invoice";
            public static string ReceiptVoucher = "Receipt Voucher";
            public static string Receivable = "Receivable";

            public static string Account = "Account";
            public static string Closing = "Closing";
            public static string GeneralLedger = "GeneralLedger";
            public static string ValidComb = "ValidComb";
            public static string BalanceSheet = "BalanceSheet";
            public static string IncomeStatement = "IncomeStatement";
            
            public static string User = "User";
            public static string UserAccessRight = "User Access Right";
            public static string CompanyInfo = "Company Info";
            
            //public static string Stock = "Stock";
            public static string Sales = "Sales";
            public static string TopSales = "Top Sales";
            public static string ProfitLoss = "Profit/Loss";
        }

        public class ItemCase
        {
            public static int Ready = 1;
            public static int PendingReceival = 2;
            public static int PendingDelivery = 3;
        }

        public class MutationStatus
        {
            public static int Addition = 1;
            public static int Deduction = 2;
        }

        public class UserType
        {
            public static string Admin = "Admin";
        }

        public class SourceDocument
        {
            public static string PaymentVoucherDetail = "PaymentVoucherDetail";
            public static string ReceiptVoucherDetail = "ReceiptVoucherDetail";
            public static string CustomPurchaseInvoiceDetail = "CustomPurchaseInvoiceDetail";
            public static string CashSalesInvoiceDetailDetail = "CashSalesInvoiceDetailDetail";
            public static string RetailSalesInvoiceDetail = "RetailSalesInvoiceDetail";
        }

        public class SourceDocumentType
        {
            public static string BarringOrder = "BarringOrder";
            public static string CashBankMutation = "CashBankMutation";
            public static string CashBankAdjustment = "CashBankAdjustment";
            public static string CoreIdentification = "CoreIdentification";
            public static string DeliveryOrder = "DeliveryOrder";
            public static string PaymentVoucher = "PaymentVoucher";
            public static string PurchaseOrder = "PurchaseOrder";
            public static string PurchaseReceival = "PurchaseReceival";
            public static string ReceiptVoucher = "ReceiptVoucher";
            public static string RecoveryOrder = "RecoveryOrder";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RetailPurchaseInvoice = "RetailPurchaseInvoice";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string RollerWarehouseMutation = "RollerWarehouseMutation";
            public static string SalesOrder = "SalesOrder";
            public static string StockAdjustment = "StockAdjustment";
            public static string WarehouseMutationOrder = "WarehouseMutationOrder";
            public static string CashSalesInvoice = "CashSalesInvoice";
            public static string CashSalesReturn = "CashSalesReturn";
            public static string CustomPurchaseInvoice = "CustomPurchaseInvoice";
        }

        public class SourceDocumentDetailType
        {
            public static string BarringOrderDetail = "BarringOrderDetail";
            public static string CoreIdentificationDetail = "CoreIdentificationDetail";
            public static string DeliveryOrderDetail = "DeliveryOrderDetail";
            public static string PurchaseOrderDetail = "PurchaseOrderDetail";
            public static string PurchaseReceivalDetail = "PurchaseReceivalDetail";
            public static string RecoveryOrderDetail = "RecoveryOrderDetail";
            public static string RecoveryAccessoryDetail = "RecoveryAccessoryDetail";
            public static string RetailPurchaseInvoiceDetail = "RetailPurchaseInvoiceDetail";
            public static string RetailSalesInvoiceDetail = "RetailSalesInvoiceDetail";
            public static string RollerWarehouseMutationDetail = "RollerWarehouseMutationDetail";
            public static string SalesOrderDetail = "SalesOrderDetail";
            public static string StockAdjustmentDetail = "StockAdjustmentDetail";
            public static string WarehouseMutationOrderDetail = "WarehouseMutationOrderDetail";
            public static string CashSalesInvoiceDetail = "CashSalesInvoiceDetail";
            public static string CashSalesReturnDetail = "CashSalesReturnDetail";
            public static string CustomPurchaseInvoiceDetail = "CustomPurchaseInvoiceDetail";
        }

        public class PayableSource
        {
            public static string PurchaseInvoice = "PurchaseInvoice";
            public static string RetailPurchaseInvoice = "RetailPurchaseInvoice";
            public static string CashSalesReturn = "CashSalesReturn";
            public static string CustomPurchaseInvoice = "CustomPurchaseInvoice";
            public static string PaymentRequest = "PaymentRequest";
        }

        public class ReceivableSource
        {
            public static string SalesInvoice = "SalesInvoice";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string CashSalesInvoice = "CashSalesInvoice";
        }

        public class ErrorPage
        {
            public static string PageViewNotAllowed = "You are not allowed to View this Page. <tr><td><a href='/Authentication/Logout'>[Logout]</a></td></tr>";
            public static string PagePrintNotAllowed = "You are not allowed to Print this Page. <tr><td><a href='/Authentication/Logout'>[Logout]</a></td></tr>";
            public static string ClosingNotFound = "Closing Not Found.";
        }
    }
}
