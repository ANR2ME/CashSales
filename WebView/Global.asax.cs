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
        private Account Asset, CashBank, AccountReceivable, AccountReceivablePPNmasuk, AccountReceivableTrading, GBCHReceivable, Inventory;
        private Account Expense, CashBankAdjustmentExpense, COGS, SalesDiscount, SalesAllowance, StockAdjustmentExpense, FreightIn, SalesReturnExpense;
        private Account TaxExpense, Divident, InterestEarning, Deprecation, Amortization, OperationalExpenses, NonOperationalExpenses;
        private Account Liability, AccountPayable, AccountPayableTrading, AccountPayableNonTrading, AccountPayablePPNkeluar, GBCHPayable, GoodsPendingClearance, PurchaseDiscount, PurchaseAllowance, SalesReturnAllowance;
        private Account Equity, OwnersEquity, EquityAdjustment, RetainedEarnings;
        private Account Revenue, FreightOut, Sales;

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
                Asset = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = Constant.AccountGroup.Asset, IsLegacy = true });
                CashBank = _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                AccountReceivable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                AccountReceivablePPNmasuk = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (PPN masuk)", IsLeaf = true, Code = Constant.AccountCode.AccountReceivablePPNmasukan, LegacyCode = Constant.AccountLegacyCode.AccountReceivablePPNmasukan, Level = 3, Group = Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
                AccountReceivableTrading = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (Trading)", IsLeaf = true, Code = Constant.AccountCode.AccountReceivableTrading, LegacyCode = Constant.AccountLegacyCode.AccountReceivableTrading, Level = 3, Group = Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
                GBCHReceivable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                Inventory = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Inventory", IsLeaf = true, Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });

                Expense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = Constant.AccountGroup.Expense, IsLegacy = true });
                COGS = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                OperationalExpenses = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Operational Expenses", Code = Constant.AccountCode.OperationalExpenses, LegacyCode = Constant.AccountLegacyCode.OperationalExpenses, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                CashBankAdjustmentExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                SalesDiscount = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Discount", IsLeaf = true, Code = Constant.AccountCode.SalesDiscountExpense, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                SalesAllowance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesAllowanceExpense, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                StockAdjustmentExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                SalesReturnExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", IsLeaf = true, Code = Constant.AccountCode.SalesReturnExpense, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                FreightIn = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight In", IsLeaf = true, Code = Constant.AccountCode.FreightIn, LegacyCode = Constant.AccountLegacyCode.FreightIn, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                // Memorial Expenses
                NonOperationalExpenses = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Non-Operational Expenses", Code = Constant.AccountCode.NonOperationalExpenses, LegacyCode = Constant.AccountLegacyCode.NonOperationalExpenses, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                TaxExpense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Tax Expense", IsLeaf = true, Code = Constant.AccountCode.Tax, LegacyCode = Constant.AccountLegacyCode.Tax, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpenses.Id, IsLegacy = true });
                Divident = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Divident", IsLeaf = true, Code = Constant.AccountCode.Divident, LegacyCode = Constant.AccountLegacyCode.Divident, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpenses.Id, IsLegacy = true });
                InterestEarning = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Interest Earning", IsLeaf = true, Code = Constant.AccountCode.InterestEarning, LegacyCode = Constant.AccountLegacyCode.InterestEarning, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpenses.Id, IsLegacy = true });
                Deprecation = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Deprecation", IsLeaf = true, Code = Constant.AccountCode.Depreciation, LegacyCode = Constant.AccountLegacyCode.Depreciation, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpenses.Id, IsLegacy = true });
                Amortization = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Amortization", IsLeaf = true, Code = Constant.AccountCode.Amortization, LegacyCode = Constant.AccountLegacyCode.Amortization, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = NonOperationalExpenses.Id, IsLegacy = true });
                

                Liability = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = Constant.AccountGroup.Liability, IsLegacy = true });
                AccountPayable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                AccountPayableTrading = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Trading)", IsLeaf = true, Code = Constant.AccountCode.AccountPayableTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableTrading, Level = 3, Group = Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                AccountPayableNonTrading = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Non Trading)", IsLeaf = true, Code = Constant.AccountCode.AccountPayableNonTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading, Level = 3, Group = Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                AccountPayablePPNkeluar = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (PPN keluaran)", IsLeaf = true, Code = Constant.AccountCode.AccountPayablePPNkeluaran, LegacyCode = Constant.AccountLegacyCode.AccountPayablePPNkeluaran, Level = 3, Group = Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                GBCHPayable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Payable", IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                GoodsPendingClearance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                PurchaseDiscount = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Discount", IsLeaf = true, Code = Constant.AccountCode.PurchaseDiscount, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                PurchaseAllowance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", IsLeaf = true, Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                SalesReturnAllowance = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesReturnAllowance, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });

                Equity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = Constant.AccountGroup.Equity, IsLegacy = true });
                OwnersEquity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });
                EquityAdjustment = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity Adjustment", IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 3, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true });
                RetainedEarnings = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Retained Earnings", IsLeaf = true, Code = Constant.AccountCode.RetainedEarnings, LegacyCode = Constant.AccountLegacyCode.RetainedEarnings, Level = 2, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });

                Revenue = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = Constant.AccountGroup.Revenue, IsLegacy = true });
                FreightOut = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight Out", IsLeaf = true, Code = Constant.AccountCode.FreightOut, LegacyCode = Constant.AccountLegacyCode.FreightOut, Level = 2, Group = Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
                Sales = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales", IsLeaf = true, Code = Constant.AccountCode.SalesRevenue, LegacyCode = Constant.AccountLegacyCode.SalesRevenue, Level = 2, Group = Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
            }
            
            CreateUserMenus();
            CreateSysAdmin();
        }

        public void CreateUserMenus()
        {
            _userMenuService.CreateObject(Constant.MenuName.Contact, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.ItemType, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.UoM, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.QuantityPricing, Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Constant.MenuName.CashBank, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankAdjustment, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashBankMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.CashMutation, Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Constant.MenuName.PaymentRequest, Constant.MenuGroupName.Master);

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
            _userMenuService.CreateObject(Constant.MenuName.PaymentVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Payable, Constant.MenuGroupName.Transaction);

            //_userMenuService.CreateObject(Constant.MenuName.SalesOrder, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.DeliveryOrder, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.SalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.RetailSalesInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CashSalesInvoice, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.CashSalesReturn, Constant.MenuGroupName.Transaction);
            
            _userMenuService.CreateObject(Constant.MenuName.ReceiptVoucher, Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Constant.MenuName.Receivable, Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Sales, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.TopSales, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.ProfitLoss, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Account, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.Closing, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.GeneralLedger, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.ValidComb, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report);
            _userMenuService.CreateObject(Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report);
            
            _userMenuService.CreateObject(Constant.MenuName.User, Constant.MenuGroupName.Setting);
            _userMenuService.CreateObject(Constant.MenuName.UserAccessRight, Constant.MenuGroupName.Setting);
            _userMenuService.CreateObject(Constant.MenuName.CompanyInfo, Constant.MenuGroupName.Setting);
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