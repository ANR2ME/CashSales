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
            public static string CashBank = "101";
            public static string AccountReceivable = "102";
            public static string AccountReceivablePPNmasukan = "102001";
            public static string AccountReceivableTrading = "102002";
            public static string GBCHReceivable = "103";
            public static string Inventory = "104";
            public static string TradingGoods = "104001"; // Raw

            public static string Expense = "2";
            public static string COGS = "201";
            public static string OperationalExpenses = "202";
            public static string FreightOutExpense = "202001";
            public static string CashBankAdjustmentExpense = "203";
            public static string SalesDiscountExpense = "204";
            public static string SalesAllowanceExpense = "205";
            public static string StockAdjustmentExpense = "206";
            public static string SalesReturnExpense = "207";
            public static string FreightIn = "208";
            //public static string NonOperationalExpenses = "203"; // Memorials
            public static string Tax = "209"; // Memorials
            public static string Divident = "210"; // Memorials
            public static string InterestEarning = "211"; // Memorials
            public static string Depreciation = "212"; // Memorials
            public static string Amortization = "213"; // Memorials
            //public static string COS = "204";

            public static string Liability = "3";
            public static string AccountPayable = "301";
            public static string AccountPayableTrading = "301001";
            public static string AccountPayableNonTrading = "301002";
            public static string AccountPayablePPNkeluaran = "301003";
            public static string GBCHPayable = "302";
            public static string GoodsPendingClearance = "303";
            public static string PurchaseDiscount = "304"; // contra account
            public static string PurchaseAllowance = "305"; // contra account
            public static string SalesReturnAllowance = "306"; // contra account
            //public static string DeliveryExpense = "307";

            public static string Equity = "4";
            public static string OwnersEquity = "401";
            public static string EquityAdjustment = "401001";
            public static string RetainedEarnings = "402";

            public static string Revenue = "5";
            public static string FreightOut = "501";
            public static string SalesRevenue = "502";
        }

        public class AccountLegacyCode
        {
            public static string Asset = "A1";
            public static string CashBank = "A101";
            public static string AccountReceivable = "A102";
            public static string AccountReceivablePPNmasukan = "A102001";
            public static string AccountReceivableTrading = "A102002";
            public static string GBCHReceivable = "A103";
            public static string Inventory = "A104";
            public static string TradingGoods = "A104001"; // Raw

            public static string Expense = "X2";
            public static string COGS = "X201";
            public static string OperationalExpenses = "X202";
            public static string FreightOutExpense = "X202001";
            public static string CashBankAdjustmentExpense = "X203";
            public static string SalesDiscountExpense = "X204";
            public static string SalesAllowanceExpense = "X205";
            public static string StockAdjustmentExpense = "X206";
            public static string SalesReturnExpense = "X207";
            public static string FreightIn = "X208";
            //public static string NonOperationalExpenses = "X203"; // Memorials
            public static string Tax = "X209"; // Memorials
            public static string Divident = "X210"; // Memorials
            public static string InterestEarning = "X211"; // Memorials
            public static string Depreciation = "X212"; // Memorials
            public static string Amortization = "X213"; // Memorials
            //public static string COS = "X204";

            public static string Liability = "L3";
            public static string AccountPayable = "L301";
            public static string AccountPayableTrading = "L301001";
            public static string AccountPayableNonTrading = "L301002";
            public static string AccountPayablePPNkeluaran = "L301003";
            public static string GBCHPayable = "L302";
            public static string GoodsPendingClearance = "L303";
            public static string PurchaseDiscount = "L304"; // contra account
            public static string PurchaseAllowance = "L305"; // contra account
            public static string SalesReturnAllowance = "L306"; // contra account
            //public static string DeliveryExpense = "L307";

            public static string Equity = "E4";
            public static string OwnersEquity = "E401";
            public static string EquityAdjustment = "E401001";
            public static string RetainedEarnings = "E402";

            public static string Revenue = "R5";
            public static string FreightOut = "R501";
            public static string SalesRevenue = "R502";

            public static string Unknown = "U1";
        }
    }
}
