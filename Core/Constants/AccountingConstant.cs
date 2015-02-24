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
            public const string CashBankAdjustment = "CashBankAdjustment";
            public const string CashBankMutation = "CashBankMutation";
            public const string CashSalesInvoice = "CashSalesInvoice";
            public const string CashSalesReturn = "CashSalesReturn";
            public const string CustomPurchaseInvoice = "CustomPurchaseInvoice";
            public const string Memorial = "Memorial";
            public const string PaymentRequest = "PaymentRequest";
            public const string PaymentVoucher = "PaymentVoucher";
            public const string ReceiptVoucher = "ReceiptVoucher";
            public const string RetailSalesInvoice = "RetailSalesInvoice";
            public const string StockAdjustment = "StockAdjustment";
        }

        //public class AccountGroup
        //{
        //    public static int Asset = 1;
        //    public static int Expense = 2;
        //    public static int Liability = 3;
        //    public static int Equity = 4;
        //    public static int Revenue = 5;
        //}

        public enum AccountGroup
        {
            Asset = 1,
            Expense = 2,
            Liability = 3,
            Equity = 4,
            Revenue = 5,
        }

        public class AccountCode
        {
            public static string Asset = "1";
            public static string CashBank = "101";
            public static string CashBankCash = "101001";
            public static string CashBankBank = "101002";
            public static string AccountReceivable = "102";
            public static string AccountReceivablePPNmasukan3 = "102001";
            public static string AccountReceivablePPNmasukan = "102001001";
            public static string AccountReceivableTrading3 = "102002";
            public static string AccountReceivableTrading = "102002001";
            public static string GBCHReceivable2 = "103";
            public static string GBCHReceivable3 = "103001";
            public static string GBCHReceivable = "103001001";
            public static string Inventory = "104";
            public static string TradingGoods3 = "104001";
            public static string TradingGoods = "104001001"; // Raw

            public static string Expense = "2";
            public static string COGS2 = "201";
            public static string COGS3 = "201001";
            public static string COGS = "201001001";
            public static string OperatingExpenses = "202";
            public static string FreightOutExpense = "202001";
            public static string CashBankAdjustmentExpense2 = "203";
            public static string CashBankAdjustmentExpense3 = "203001";
            public static string CashBankAdjustmentExpense = "203001001";
            public static string SalesDiscountExpense2 = "204";
            public static string SalesDiscountExpense3 = "204001";
            public static string SalesDiscountExpense = "204001001";
            public static string SalesAllowanceExpense2 = "205";
            public static string SalesAllowanceExpense3 = "205001";
            public static string SalesAllowanceExpense = "205001001";
            public static string StockAdjustmentExpense2 = "206";
            public static string StockAdjustmentExpense3 = "206001";
            public static string StockAdjustmentExpense = "206001001";
            public static string SalesReturnExpense2 = "207";
            public static string SalesReturnExpense3 = "207001";
            public static string SalesReturnExpense = "207001001";
            public static string FreightIn2 = "208";
            public static string FreightIn3 = "208001";
            public static string FreightIn = "208001001";
            public static string NonOperatingExpenses2 = "209"; // Memorials
            public static string NonOperatingExpenses = "209001";
            public static string Tax2 = "210"; // Memorials
            public static string Tax = "210001";
            public static string Divident2 = "211"; // Memorials
            public static string Divident = "211001";
            public static string InterestExpense2 = "212"; // Memorials
            public static string InterestExpense = "212001";
            public static string Depreciation2 = "213"; // Memorials
            public static string Depreciation = "213001";
            public static string Amortization2 = "214"; // Memorials
            public static string Amortization = "214001";
            //public static string COS = "204";

            public static string Liability = "3";
            public static string AccountPayable = "301";
            public static string AccountPayableTrading3 = "301001";
            public static string AccountPayableTrading = "301001001";
            public static string AccountPayableNonTrading3 = "301002";
            public static string AccountPayableNonTrading = "301002001";
            public static string AccountPayablePPNkeluaran3 = "301003";
            public static string AccountPayablePPNkeluaran = "301003001";
            public static string GBCHPayable2 = "302";
            public static string GBCHPayable3 = "302001";
            public static string GBCHPayable = "302001001";
            public static string GoodsPendingClearance2 = "303";
            public static string GoodsPendingClearance3 = "303001";
            public static string GoodsPendingClearance = "303001001";
            public static string PurchaseDiscount2 = "304"; // contra account
            public static string PurchaseDiscount3 = "304001";
            public static string PurchaseDiscount = "304001001";
            public static string PurchaseAllowance2 = "305"; // contra account
            public static string PurchaseAllowance3 = "305001";
            public static string PurchaseAllowance = "305001001";
            public static string SalesReturnAllowance2 = "306"; // contra account
            public static string SalesReturnAllowance3 = "306001";
            public static string SalesReturnAllowance = "306001001";
            //public static string DeliveryExpense = "307";

            public static string Equity = "4";
            public static string OwnersEquity = "401";
            public static string EquityAdjustment3 = "401001";
            public static string EquityAdjustment = "401001001";
            public static string RetainedEarnings2 = "402";
            public static string RetainedEarnings = "402001";

            public static string Revenue = "5";
            public static string FreightOut2 = "501";
            public static string FreightOut = "501001";
            public static string SalesRevenue2 = "502";
            public static string SalesRevenue = "502001";
            public static string OtherIncome2 = "503"; // Non-Operating Income
            public static string OtherIncome = "503001";
        }

        public class AccountLegacyCode
        {
            public static string Asset = "A1";
            public static string CashBank = "A101";
            public static string CashBankCash = "A101001";
            public static string CashBankBank = "A101002";
            public static string AccountReceivable = "A102";
            public static string AccountReceivablePPNmasukan3 = "A102001";
            public static string AccountReceivablePPNmasukan = "A102001001";
            public static string AccountReceivableTrading3 = "A102002";
            public static string AccountReceivableTrading = "A102002001";
            public static string GBCHReceivable2 = "A103";
            public static string GBCHReceivable3 = "A103001";
            public static string GBCHReceivable = "A103001001";
            public static string Inventory = "A104";
            public static string TradingGoods3 = "A104001";
            public static string TradingGoods = "A104001001"; // Raw

            public static string Expense = "X2";
            public static string COGS2 = "X201";
            public static string COGS3 = "X201001";
            public static string COGS = "X201001001";
            public static string OperatingExpenses = "X202";
            public static string FreightOutExpense = "X202001";
            public static string CashBankAdjustmentExpense2 = "X203";
            public static string CashBankAdjustmentExpense3 = "X203001";
            public static string CashBankAdjustmentExpense = "X203001001";
            public static string SalesDiscountExpense2 = "X204";
            public static string SalesDiscountExpense3 = "X204001";
            public static string SalesDiscountExpense = "X204001001";
            public static string SalesAllowanceExpense2 = "X205";
            public static string SalesAllowanceExpense3 = "X205001";
            public static string SalesAllowanceExpense = "X205001001";
            public static string StockAdjustmentExpense2 = "X206";
            public static string StockAdjustmentExpense3 = "X206001";
            public static string StockAdjustmentExpense = "X206001001";
            public static string SalesReturnExpense2 = "X207";
            public static string SalesReturnExpense3 = "X207001";
            public static string SalesReturnExpense = "X207001001";
            public static string FreightIn2 = "X208";
            public static string FreightIn3 = "X208001";
            public static string FreightIn = "X208001001";
            public static string NonOperatingExpenses2 = "X209"; // Memorials
            public static string NonOperatingExpenses = "X209001";
            public static string Tax2 = "X210"; // Memorials
            public static string Tax = "X210001";
            public static string Divident2 = "X211"; // Memorials
            public static string Divident = "X211001";
            public static string InterestExpense2 = "X212"; // Memorials
            public static string InterestExpense = "X212001";
            public static string Depreciation2 = "X213"; // Memorials
            public static string Depreciation = "X213001";
            public static string Amortization2 = "X214"; // Memorials
            public static string Amortization = "X214001";
            //public static string COS = "X204";

            public static string Liability = "L3";
            public static string AccountPayable = "L301";
            public static string AccountPayableTrading3 = "L301001";
            public static string AccountPayableTrading = "L301001001";
            public static string AccountPayableNonTrading3 = "L301002";
            public static string AccountPayableNonTrading = "L301002001";
            public static string AccountPayablePPNkeluaran3 = "L301003";
            public static string AccountPayablePPNkeluaran = "L301003001";
            public static string GBCHPayable2 = "L302";
            public static string GBCHPayable3 = "L302001";
            public static string GBCHPayable = "L302001001";
            public static string GoodsPendingClearance2 = "L303";
            public static string GoodsPendingClearance3 = "L303001";
            public static string GoodsPendingClearance = "L303001001";
            public static string PurchaseDiscount2 = "L304"; // contra account
            public static string PurchaseDiscount3 = "L304001";
            public static string PurchaseDiscount = "L304001001";
            public static string PurchaseAllowance2 = "L305"; // contra account
            public static string PurchaseAllowance3 = "L305001";
            public static string PurchaseAllowance = "L305001001";
            public static string SalesReturnAllowance2 = "L306"; // contra account
            public static string SalesReturnAllowance3 = "L306001";
            public static string SalesReturnAllowance = "L306001001";
            //public static string DeliveryExpense = "L307";

            public static string Equity = "E4";
            public static string OwnersEquity = "E401";
            public static string EquityAdjustment3 = "E401001";
            public static string EquityAdjustment = "E401001001";
            public static string RetainedEarnings2 = "E402";
            public static string RetainedEarnings = "E402001";

            public static string Revenue = "R5";
            public static string FreightOut2 = "R501";
            public static string FreightOut = "R501001";
            public static string SalesRevenue2 = "R502";
            public static string SalesRevenue = "R502001";
            public static string OtherIncome2 = "R503"; // Non-Operating Income
            public static string OtherIncome = "R503001";

            public static string Unknown = "U1";
        }
    }
}
