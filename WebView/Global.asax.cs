using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service.Service;
using Validation.Validation;
using Core.Constants;

namespace WebView
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private IAccountService _accountService;
        private IContactGroupService _contactGroupService;
        private IContactService _contactService;
        private IUserMenuService _userMenuService;
        private IUserAccountService _userAccountService;
        private IUserAccessService _userAccessService;
        private ICompanyService _companyService;
        private ContactGroup baseContactGroup;
        private Contact baseContact;
        private Company baseCompany;
        private Account Asset, CashBank, AccountReceivable, AccountReceivablePPNmasuk, AccountReceivableTrading, GBCHReceivable, Inventory, TradingGoods;
        private Account Expense, CashBankAdjustmentExpense, COGS, SalesDiscount, SalesAllowance, StockAdjustmentExpense, FreightIn, SalesReturnExpense;
        private Account TaxExpense, Divident, InterestExpense, Depreciation, Amortization, OperationalExpenses, NonOperationalExpenses, FreightOutExpense;
        private Account Liability, AccountPayable, AccountPayableTrading, AccountPayableNonTrading, AccountPayablePPNkeluar, GBCHPayable, GoodsPendingClearance, PurchaseDiscount, PurchaseAllowance, SalesReturnAllowance;
        private Account Equity, OwnersEquity, EquityAdjustment, RetainedEarnings;
        private Account Revenue, FreightOut, Sales, OtherIncome;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            PopulateData();
        }

        public void PopulateData()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());

            baseContactGroup = _contactGroupService.FindOrCreateBaseContactGroup(); // .CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);
            baseContact = _contactService.FindOrCreateBaseContact(_contactGroupService); // _contactService.CreateObject(Core.Constants.Constant.BaseContact, "BaseAddr", "123456", "PIC", "123", "Base@email.com", _contactGroupService);
            baseCompany = _companyService.GetQueryable().FirstOrDefault();
            if (baseCompany == null)
            {
                baseCompany = _companyService.CreateObject("Jakarta Andalan Bike", "Jl. Hos Cokroaminoto No.12A Mencong Ciledug, Tangerang", "021-7316575", "", "jakartaandalanbike@gmail.com");
            }
            
            //if (!_accountService.GetLegacyObjects().Any())
            {
                Asset = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = (int)Constant.AccountGroup.Asset, IsLegacy = true });
                CashBank = _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                var CashBankCash = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Cash", Code = Constant.AccountCode.CashBankCash, LegacyCode = Constant.AccountLegacyCode.CashBankCash, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = CashBank.Id, IsLegacy = true });
                var CashBankBank = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Bank", Code = Constant.AccountCode.CashBankBank, LegacyCode = Constant.AccountLegacyCode.CashBankBank, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = CashBank.Id, IsLegacy = true });
                AccountReceivable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                var AccountReceivablePPNmasuk3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (PPN masukan)", Code = Constant.AccountCode.AccountReceivablePPNmasukan3, LegacyCode = Constant.AccountLegacyCode.AccountReceivablePPNmasukan3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
                AccountReceivablePPNmasuk = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (PPN masukan)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountReceivablePPNmasukan, LegacyCode = Constant.AccountLegacyCode.AccountReceivablePPNmasukan, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivablePPNmasuk3.Id, IsLegacy = true });
                var AccountReceivableTrading3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (Trading)", Code = Constant.AccountCode.AccountReceivableTrading3, LegacyCode = Constant.AccountLegacyCode.AccountReceivableTrading3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
                AccountReceivableTrading = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountReceivableTrading, LegacyCode = Constant.AccountLegacyCode.AccountReceivableTrading, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivableTrading3.Id, IsLegacy = true });
                var GBCHReceivable2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", Code = Constant.AccountCode.GBCHReceivable2, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable2, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                var GBCHReceivable3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", Code = Constant.AccountCode.GBCHReceivable3, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = GBCHReceivable2.Id, IsLegacy = true });
                GBCHReceivable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = GBCHReceivable3.Id, IsLegacy = true });
                Inventory = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Inventory", Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                var TradingGoods3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Trading Goods", Code = Constant.AccountCode.TradingGoods3, LegacyCode = Constant.AccountLegacyCode.TradingGoods3, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true });
                TradingGoods = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Trading Goods", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.TradingGoods, LegacyCode = Constant.AccountLegacyCode.TradingGoods, Level = 4, Group = (int)Constant.AccountGroup.Asset, ParentId = TradingGoods3.Id, IsLegacy = true });

                Expense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = (int)Constant.AccountGroup.Expense, IsLegacy = true });
                var COGS2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS2, LegacyCode = Constant.AccountLegacyCode.COGS2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                var COGS3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", Code = Constant.AccountCode.COGS3, LegacyCode = Constant.AccountLegacyCode.COGS3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = COGS2.Id, IsLegacy = true });
                COGS = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = COGS3.Id, IsLegacy = true });
                OperationalExpenses = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Operating Expenses", Code = Constant.AccountCode.OperatingExpenses, LegacyCode = Constant.AccountLegacyCode.OperatingExpenses, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                FreightOutExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight Out Expenses", IsLeaf = true, Code = Constant.AccountCode.FreightOutExpense, LegacyCode = Constant.AccountLegacyCode.FreightOutExpense, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                var CashBankAdjustmentExpense2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", Code = Constant.AccountCode.CashBankAdjustmentExpense2, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                var CashBankAdjustmentExpense3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", Code = Constant.AccountCode.CashBankAdjustmentExpense3, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = CashBankAdjustmentExpense2.Id, IsLegacy = true });
                CashBankAdjustmentExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = CashBankAdjustmentExpense3.Id, IsLegacy = true });
                var SalesDiscount2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Discount", Code = Constant.AccountCode.SalesDiscountExpense2, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                var SalesDiscount3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Discount", Code = Constant.AccountCode.SalesDiscountExpense3, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesDiscount2.Id, IsLegacy = true });
                SalesDiscount = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Discount", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesDiscountExpense, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesDiscount3.Id, IsLegacy = true });
                var SalesAllowance2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Allowance", Code = Constant.AccountCode.SalesAllowanceExpense2, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                var SalesAllowance3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Allowance", Code = Constant.AccountCode.SalesAllowanceExpense3, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesAllowance2.Id, IsLegacy = true });
                SalesAllowance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesAllowanceExpense, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesAllowance3.Id, IsLegacy = true });
                var StockAdjustmentExpense2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", Code = Constant.AccountCode.StockAdjustmentExpense2, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                var StockAdjustmentExpense3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", Code = Constant.AccountCode.StockAdjustmentExpense3, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = StockAdjustmentExpense2.Id, IsLegacy = true });
                StockAdjustmentExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = StockAdjustmentExpense3.Id, IsLegacy = true });
                var SalesReturnExpense2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", Code = Constant.AccountCode.SalesReturnExpense2, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                var SalesReturnExpense3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", Code = Constant.AccountCode.SalesReturnExpense3, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesReturnExpense2.Id, IsLegacy = true });
                SalesReturnExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesReturnExpense, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = SalesReturnExpense3.Id, IsLegacy = true });
                var FreightIn2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight In", Code = Constant.AccountCode.FreightIn2, LegacyCode = Constant.AccountLegacyCode.FreightIn2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                var FreightIn3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight In", Code = Constant.AccountCode.FreightIn3, LegacyCode = Constant.AccountLegacyCode.FreightIn3, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = FreightIn2.Id, IsLegacy = true });
                FreightIn = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight In", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.FreightIn, LegacyCode = Constant.AccountLegacyCode.FreightIn, Level = 4, Group = (int)Constant.AccountGroup.Expense, ParentId = FreightIn3.Id, IsLegacy = true });
                // Memorial Expenses
                var NonOperationalExpenses2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Non-Operating Expenses", Code = Constant.AccountCode.NonOperatingExpenses2, LegacyCode = Constant.AccountLegacyCode.NonOperatingExpenses2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                NonOperationalExpenses = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Non-Operating Expenses", IsLeaf = true, Code = Constant.AccountCode.NonOperatingExpenses, LegacyCode = Constant.AccountLegacyCode.NonOperatingExpenses, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = NonOperationalExpenses2.Id, IsLegacy = true });
                var TaxExpense2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Tax Expense", Code = Constant.AccountCode.Tax2, LegacyCode = Constant.AccountLegacyCode.Tax2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                TaxExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Tax Expense", IsLeaf = true, Code = Constant.AccountCode.Tax, LegacyCode = Constant.AccountLegacyCode.Tax, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = TaxExpense2.Id, IsLegacy = true });
                var Divident2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Divident", Code = Constant.AccountCode.Divident2, LegacyCode = Constant.AccountLegacyCode.Divident2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                Divident = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Divident", IsLeaf = true, Code = Constant.AccountCode.Divident, LegacyCode = Constant.AccountLegacyCode.Divident, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = Divident2.Id, IsLegacy = true });
                var InterestExpense2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Interest Expense", Code = Constant.AccountCode.InterestExpense2, LegacyCode = Constant.AccountLegacyCode.InterestExpense2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                InterestExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Interest Expense", IsLeaf = true, Code = Constant.AccountCode.InterestExpense, LegacyCode = Constant.AccountLegacyCode.InterestExpense, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = InterestExpense2.Id, IsLegacy = true });
                var Depreciation2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Depreciation", Code = Constant.AccountCode.Depreciation2, LegacyCode = Constant.AccountLegacyCode.Depreciation2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                Depreciation = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Depreciation", IsLeaf = true, Code = Constant.AccountCode.Depreciation, LegacyCode = Constant.AccountLegacyCode.Depreciation, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = Depreciation2.Id, IsLegacy = true });
                var Amortization2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Amortization", Code = Constant.AccountCode.Amortization2, LegacyCode = Constant.AccountLegacyCode.Amortization2, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                Amortization = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Amortization", IsLeaf = true, Code = Constant.AccountCode.Amortization, LegacyCode = Constant.AccountLegacyCode.Amortization, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = Amortization2.Id, IsLegacy = true });

                Liability = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = (int)Constant.AccountGroup.Liability, IsLegacy = true });
                AccountPayable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                var AccountPayableTrading3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Trading)", Code = Constant.AccountCode.AccountPayableTrading3, LegacyCode = Constant.AccountLegacyCode.AccountPayableTrading3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                AccountPayableTrading = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayableTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableTrading, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayableTrading3.Id, IsLegacy = true });
                var AccountPayableNonTrading3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Non Trading)", Code = Constant.AccountCode.AccountPayableNonTrading3, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                AccountPayableNonTrading = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Non Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayableNonTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayableNonTrading3.Id, IsLegacy = true });
                var AccountPayablePPNkeluar3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (PPN keluaran)", Code = Constant.AccountCode.AccountPayablePPNkeluaran3, LegacyCode = Constant.AccountLegacyCode.AccountPayablePPNkeluaran3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                AccountPayablePPNkeluar = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (PPN keluaran)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayablePPNkeluaran, LegacyCode = Constant.AccountLegacyCode.AccountPayablePPNkeluaran, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayablePPNkeluar3.Id, IsLegacy = true });
                var GBCHPayable2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable2, LegacyCode = Constant.AccountLegacyCode.GBCHPayable2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                var GBCHPayable3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Payable", Code = Constant.AccountCode.GBCHPayable3, LegacyCode = Constant.AccountLegacyCode.GBCHPayable3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = GBCHPayable2.Id, IsLegacy = true });
                GBCHPayable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Payable", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = GBCHPayable3.Id, IsLegacy = true });
                var GoodsPendingClearance2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance2, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                var GoodsPendingClearance3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", Code = Constant.AccountCode.GoodsPendingClearance3, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = GoodsPendingClearance2.Id, IsLegacy = true });
                GoodsPendingClearance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = GoodsPendingClearance3.Id, IsLegacy = true });
                var PurchaseDiscount2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Discount", Code = Constant.AccountCode.PurchaseDiscount2, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                var PurchaseDiscount3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Discount", Code = Constant.AccountCode.PurchaseDiscount3, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseDiscount2.Id, IsLegacy = true });
                PurchaseDiscount = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Discount", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.PurchaseDiscount, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseDiscount3.Id, IsLegacy = true });
                var PurchaseAllowance2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", Code = Constant.AccountCode.PurchaseAllowance2, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                var PurchaseAllowance3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", Code = Constant.AccountCode.PurchaseAllowance3, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseAllowance2.Id, IsLegacy = true });
                PurchaseAllowance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = PurchaseAllowance3.Id, IsLegacy = true });
                var SalesReturnAllowance2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", Code = Constant.AccountCode.SalesReturnAllowance2, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance2, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                var SalesReturnAllowance3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", Code = Constant.AccountCode.SalesReturnAllowance3, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance3, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = SalesReturnAllowance2.Id, IsLegacy = true });
                SalesReturnAllowance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesReturnAllowance, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance, Level = 4, Group = (int)Constant.AccountGroup.Liability, ParentId = SalesReturnAllowance3.Id, IsLegacy = true });

                Equity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = (int)Constant.AccountGroup.Equity, IsLegacy = true });
                OwnersEquity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = (int)Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });
                var EquityAdjustment3 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity Adjustment", Code = Constant.AccountCode.EquityAdjustment3, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment3, Level = 3, Group = (int)Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true });
                EquityAdjustment = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity Adjustment", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 4, Group = (int)Constant.AccountGroup.Equity, ParentId = EquityAdjustment3.Id, IsLegacy = true });
                var RetainedEarnings2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Retained Earnings", Code = Constant.AccountCode.RetainedEarnings2, LegacyCode = Constant.AccountLegacyCode.RetainedEarnings2, Level = 2, Group = (int)Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });
                RetainedEarnings = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Retained Earnings", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.RetainedEarnings, LegacyCode = Constant.AccountLegacyCode.RetainedEarnings, Level = 3, Group = (int)Constant.AccountGroup.Equity, ParentId = RetainedEarnings2.Id, IsLegacy = true });

                Revenue = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = (int)Constant.AccountGroup.Revenue, IsLegacy = true });
                var FreightOut2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight Out", Code = Constant.AccountCode.FreightOut2, LegacyCode = Constant.AccountLegacyCode.FreightOut2, Level = 2, Group = (int)Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
                FreightOut = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight Out", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.FreightOut, LegacyCode = Constant.AccountLegacyCode.FreightOut, Level = 3, Group = (int)Constant.AccountGroup.Revenue, ParentId = FreightOut2.Id, IsLegacy = true });
                var Sales2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales", Code = Constant.AccountCode.SalesRevenue2, LegacyCode = Constant.AccountLegacyCode.SalesRevenue2, Level = 2, Group = (int)Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
                Sales = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesRevenue, LegacyCode = Constant.AccountLegacyCode.SalesRevenue, Level = 3, Group = (int)Constant.AccountGroup.Revenue, ParentId = Sales2.Id, IsLegacy = true });
                var OtherIncome2 = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Other Income", Code = Constant.AccountCode.OtherIncome2, LegacyCode = Constant.AccountLegacyCode.OtherIncome2, Level = 2, Group = (int)Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
                OtherIncome = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Other Income", IsLeaf = true, Code = Constant.AccountCode.OtherIncome, LegacyCode = Constant.AccountLegacyCode.OtherIncome, Level = 3, Group = (int)Constant.AccountGroup.Revenue, ParentId = OtherIncome2.Id, IsLegacy = true });
            }
            
            CreateUserMenus();
            CreateSysAdmin();
        }

        public void CreateUserMenus()
        {
            _userMenuService.CreateObject(Constant.MenuName.CompanyInfo, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Contact, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.ItemType, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.UoM, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.QuantityPricing, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.CashBank, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Account, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.StockAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.StockMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.Warehouse, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.WarehouseItem, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.WarehouseMutation, Constant.MenuGroupName.Master);

            //_userMenuService.CreateObject(Constant.MenuName.PurchaseOrder, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.PurchaseReceival, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.PurchaseInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CustomPurchaseInvoice, Constant.MenuGroupName.Transaction);
            

            //_userMenuService.CreateObject(Constant.MenuName.SalesOrder, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.DeliveryOrder, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.SalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.RetailSalesInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CashSalesInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CashSalesReturn, Constant.MenuGroupName.Transaction);
            
            _userMenuService.CreateObject(Constant.MenuName.ReceiptVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Receivable, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PaymentVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Payable, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.PaymentRequest, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Memorial, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Closing, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.GeneralLedger, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.ValidComb, Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Purchase, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Sales, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.TopSales, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.ProfitLoss, Constant.MenuGroupName.Report);
            
            _userMenuService.CreateObject(Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report);
            
            _userMenuService.CreateObject(Constant.MenuName.User, Constant.MenuGroupName.Setting);
            _userMenuService.CreateObject(Constant.MenuName.UserAccessRight, Constant.MenuGroupName.Setting);
        }

        public void CreateSysAdmin()
        {
            UserAccount userAccount = _userAccountService.GetObjectByUsername(Constant.UserType.Admin);
            if (userAccount == null)
            {
                userAccount = _userAccountService.CreateObject(Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true);
            }
            _userAccessService.CreateDefaultAccess(userAccount.Id, _userMenuService, _userAccountService);

        }

    }
}