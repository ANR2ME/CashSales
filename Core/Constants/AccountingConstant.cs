using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Constants
{
    public partial class Constant
    {
        public class GeneralLedgerStatus
        {
            public static int Debit = 1;
            public static int Credit = 2;
        }

        public class GeneralLedgerSource
        {
            public static string CashBankAdjustment = "CashBankAdjustment";
            public static string CashBankMutation = "CashBankMutation";
            public static string CashSalesInvoice = "CashSalesInvoice";
            public static string CashSalesReturn = "CashSalesReturn";
            public static string CustomPurchaseInvoice = "CustomPurchaseInvoice";
            public static string Memorial = "Memorial";
            public static string PaymentRequest = "PaymentRequest";
            public static string PaymentVoucher = "PaymentVoucher";
            public static string ReceiptVoucher = "ReceiptVoucher";
            public static string RetailSalesInvoice = "RetailSalesInvoice";
            public static string StockAdjustment = "StockAdjustment";
        }

        public class AccountGroup
        {
            public static int Asset = 1;
            public static int Expense = 2;
            public static int Liability = 3;
            public static int Equity = 4;
            public static int Revenue = 5;
        }

        public class AccountCode
        {
            public static string Asset = "1";
            public static string CashBank = "11";
            public static string AccountReceivable = "12";
            public static string AccountReceivablePPNmasukan = "121";
            public static string AccountReceivableTrading = "122";
            public static string GBCHReceivable = "13";
            public static string Inventory = "14";
            //public static string FinishedGoods = "141"; // Raw

            public static string Expense = "2";
            public static string COGS = "21";
            public static string CashBankAdjustmentExpense = "22";
            public static string SalesDiscountExpense = "23";
            public static string SalesAllowanceExpense = "24";
            public static string StockAdjustmentExpense = "25";
            public static string FreightIn = "26";
            public static string SalesReturnExpense = "27";
            //public static string TaxExpense = "28";

            public static string Liability = "3";
            public static string AccountPayable = "31";
            public static string AccountPayableTrading = "311";
            public static string AccountPayableNonTrading = "312";
            public static string AccountPayablePPNkeluaran = "313";
            public static string GBCHPayable = "32";
            public static string GoodsPendingClearance = "33";
            public static string PurchaseDiscount = "34"; // contra account
            public static string PurchaseAllowance = "35"; // contra account
            public static string SalesReturnAllowance = "36"; // contra account
            //public static string DeliveryExpense = "37";

            public static string Equity = "4";
            public static string OwnersEquity = "41";
            public static string EquityAdjustment = "411";

            public static string Revenue = "5";
            public static string FreightOut = "51";
            public static string SalesRevenue = "52";
        }

        public class AccountLegacyCode
        {
            public static string Asset = "A1";
            public static string CashBank = "A11";
            public static string AccountReceivable = "A12";
            public static string AccountReceivablePPNmasukan = "A121";
            public static string AccountReceivableTrading = "A122";
            public static string GBCHReceivable = "A13";
            public static string Inventory = "A14";

            public static string Expense = "X1";
            public static string COGS = "X11";
            public static string CashBankAdjustmentExpense = "X12";
            public static string SalesDiscountExpense = "X13";
            public static string SalesAllowanceExpense = "X14";
            public static string StockAdjustmentExpense = "X15";
            public static string FreightIn = "X16";
            public static string SalesReturnExpense = "X17";
            //public static string TaxExpense = "X18";

            public static string Liability = "L1";
            public static string AccountPayable = "L11";
            public static string AccountPayableTrading = "L111";
            public static string AccountPayableNonTrading = "L112";
            public static string AccountPayablePPNkeluaran = "L113";
            public static string GBCHPayable = "L12";
            public static string GoodsPendingClearance = "L13";
            public static string PurchaseDiscount = "L14"; // contra account
            public static string PurchaseAllowance = "L15"; // contra account
            public static string SalesReturnAllowance = "L16"; // contra account
            //public static string DeliveryExpense = "L17";

            public static string Equity = "E1";
            public static string OwnersEquity = "E11";
            public static string EquityAdjustment = "E111";

            public static string Revenue = "R1";
            public static string FreightOut = "R11";
            public static string SalesRevenue = "R12";

            public static string Unknown = "U1";
        }
    }
}
