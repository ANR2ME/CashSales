using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using Core.Interface.Service;
using Service.Service;
using Data.Repository;
using Validation.Validation;
using Data.Context;
using System.Data.Entity;
using Core.DomainModel;
using System.Reflection;
using Core.Constants;

namespace Service
{
    /// <summary>
    /// A static class for reflection type functions
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();
            // Collect all the valid properties to map
            var results = from srcProp in typeSrc.GetProperties()
                          let targetProperty = typeDest.GetProperty(srcProp.Name)
                          where srcProp.CanRead
                          && targetProperty != null
                          && (targetProperty.GetSetMethod(true) != null && !targetProperty.GetSetMethod(true).IsPrivate)
                          && (targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) == 0
                          && targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)
                          select new { sourceProperty = srcProp, targetProperty = targetProperty };
            //map the properties
            foreach (var props in results)
            {
                props.targetProperty.SetValue(destination, props.sourceProperty.GetValue(source, null), null);
            }
        }
    }

    public class RepairFunctions
    {
        private static int logcount = 0;

        public IAccountService _accountService;
        public ICashBankService _cashBankService;
        public ICashBankAdjustmentService _cashBankAdjustmentService;
        public ICashBankMutationService _cashBankMutationService;
        public IItemService _itemService;
        public IStockAdjustmentService _stockAdjustmentService;
        public IStockAdjustmentDetailService _stockAdjustmentDetailService;
        public IWarehouseItemService _warehouseItemService;
        public IWarehouseMutationOrderService _warehouseMutationOrderService;
        public IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService;
        public ICustomPurchaseInvoiceService _customPurchaseInvoiceService;
        public ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService;
        public ICashSalesInvoiceService _cashSalesInvoiceService;
        public ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService;
        public ICashSalesReturnService _cashSalesReturnService;
        public ICashSalesReturnDetailService _cashSalesReturnDetailService;
        public IReceiptVoucherService _receiptVoucherService;
        public IReceiptVoucherDetailService _receiptVoucherDetailService;
        public IPaymentRequestService _paymentRequestService;
        public IPaymentRequestDetailService _paymentRequestDetailService;
        public IPaymentVoucherService _paymentVoucherService;
        public IPaymentVoucherDetailService _paymentVoucherDetailService;
        public IMemorialService _memorialService;
        public IMemorialDetailService _memorialDetailService;

        public IGeneralLedgerJournalService _generalLedgerJournalService;
        public IClosingService _closingService;
        public ICashMutationService _cashMutationService;
        public IUoMService _uomService;
        public IItemTypeService _itemTypeService;
        public IPriceMutationService _priceMutationService;
        public IContactGroupService _contactGroupService;
        public IWarehouseService _warehouseService;
        public IStockMutationService _stockMutationService;
        public IContactService _contactService;
        public IPayableService _payableService;
        public IReceivableService _receivableService;
        public IBarringService _barringService;
        public IQuantityPricingService _quantityPricingService;

        public IList<String> userDatas = new List<String>()
                                        {   
                                            "MemorialDetail", "Memorial", "Account",
                                            "PaymentVoucherDetail", "PaymentVoucher",
                                            "ReceiptVoucherDetail", "ReceiptVoucher",
                                            "PaymentRequestDetail", "PaymentRequest",
                                            "CashBankAdjustment", "CashBankMutation", "CashBank",
                                            "CustomPurchaseInvoiceDetail", "CustomPurchaseInvoice",
                                            "CashSalesReturnDetail", "CashSalesReturn", 
                                            "CashSalesInvoiceDetail", "CashSalesInvoice",
                                            "WarehouseMutationOrderDetail", "WarehouseMutationOrder",
                                            "StockAdjustmentDetail", "StockAdjustment", "WarehouseItem", "Item",
                                            "Receivable", "Payable",
                                        };

        public IList<String> masterDatas = new List<String>()
                                        {
                                            "UserAccess", "UserMenu", "UserAccount",
                                            "GroupItemPrice", "QuantityPricing", "Warehouse", "ItemType", "UoM", "Contact", "ContactGroup", "Company",
                                        };

        public RepairFunctions()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _warehouseMutationOrderService = new WarehouseMutationOrderService(new WarehouseMutationOrderRepository(), new WarehouseMutationOrderValidator());
            _warehouseMutationOrderDetailService = new WarehouseMutationOrderDetailService(new WarehouseMutationOrderDetailRepository(), new WarehouseMutationOrderDetailValidator());
            _customPurchaseInvoiceService = new CustomPurchaseInvoiceService(new CustomPurchaseInvoiceRepository(), new CustomPurchaseInvoiceValidator());
            _customPurchaseInvoiceDetailService = new CustomPurchaseInvoiceDetailService(new CustomPurchaseInvoiceDetailRepository(), new CustomPurchaseInvoiceDetailValidator());
            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesInvoiceDetailService = new CashSalesInvoiceDetailService(new CashSalesInvoiceDetailRepository(), new CashSalesInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _cashSalesReturnDetailService = new CashSalesReturnDetailService(new CashSalesReturnDetailRepository(), new CashSalesReturnDetailValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidator());
            _paymentRequestDetailService = new PaymentRequestDetailService(new PaymentRequestDetailRepository(), new PaymentRequestDetailValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _memorialDetailService = new MemorialDetailService(new MemorialDetailRepository(), new MemorialDetailValidator());
            _memorialService = new MemorialService(new MemorialRepository(), new MemorialValidator());

            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(), new QuantityPricingValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
        }

        public void Log(IDictionary<string,string> Errors, string Table, string Name, int OldId, int NewId) 
        {
            //if (logcount >= 100)
            //{
            //    Console.Write("Press ENTER to Continue...............................................");
            //    Console.ReadLine();
            //    logcount = 0;
            //}
            if (Errors != null)
            {
                if (Errors.Any())
                {
                    foreach (var e in Errors)
                    {
                        Console.WriteLine("{0} {1} [{2}->{3}] : {4} {5}", Table, Name, OldId, NewId, e.Key, e.Value);
                        logcount++;
                    }
                }
                else if (OldId != NewId)
                {
                    Console.WriteLine("{0} {1} [{2}->{3}]", Table, Name, OldId, NewId);
                    logcount++;
                }
            }
        }

        public void FixAccounts()
        {
            //if (!_accountService.GetLegacyObjects().Any())
            {
                Account Asset = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                Account AccountReceivable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (PPN masuk)", IsLeaf = true, Code = Constant.AccountCode.AccountReceivablePPNmasukan, LegacyCode = Constant.AccountLegacyCode.AccountReceivablePPNmasukan, Level = 3, Group = Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (Trading)", IsLeaf = true, Code = Constant.AccountCode.AccountReceivableTrading, LegacyCode = Constant.AccountLegacyCode.AccountReceivableTrading, Level = 3, Group = Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Inventory", IsLeaf = true, Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

                Account Expense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Discount", IsLeaf = true, Code = Constant.AccountCode.SalesDiscountExpense, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesAllowanceExpense, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight (In)", IsLeaf = true, Code = Constant.AccountCode.FreightIn, LegacyCode = Constant.AccountLegacyCode.FreightIn, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", IsLeaf = true, Code = Constant.AccountCode.SalesReturnExpense, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);

                Account Liability = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
                Account AccountPayable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Trading)", IsLeaf = true, Code = Constant.AccountCode.AccountPayableTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableTrading, Level = 3, Group = Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Non Trading)", IsLeaf = true, Code = Constant.AccountCode.AccountPayableNonTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading, Level = 3, Group = Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (PPN keluaran)", IsLeaf = true, Code = Constant.AccountCode.AccountPayablePPNkeluaran, LegacyCode = Constant.AccountLegacyCode.AccountPayablePPNkeluaran, Level = 3, Group = Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Payable", IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Discount", IsLeaf = true, Code = Constant.AccountCode.PurchaseDiscount, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", IsLeaf = true, Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesReturnAllowance, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);


                Account Equity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = Constant.AccountGroup.Equity, IsLegacy = true }, _accountService);
                Account OwnersEquity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity Adjustment", IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 3, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);

                Account Revenue = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight (Out)", IsLeaf = true, Code = Constant.AccountCode.FreightOut, LegacyCode = Constant.AccountLegacyCode.FreightOut, Level = 2, Group = Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true }, _accountService);
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales", IsLeaf = true, Code = Constant.AccountCode.SalesRevenue, LegacyCode = Constant.AccountLegacyCode.SalesRevenue, Level = 2, Group = Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true }, _accountService);
            }
        }

        // http://www.tech-recipes.com/rx/1487/copy-an-existing-mysql-table-to-a-new-table/
        public int BackUp(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                foreach (var tableName in masterDatas)
                {
                    // Create new table using source table structure, and then copy the data from source structure to new table
                    //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE 'Temp{0}' LIKE {0}; INSERT INTO 'Temp{0}' SELECT * FROM '{0}';", tableName));
                    //db.Database.ExecuteSqlCommand(string.Format("DELETE FROM 'Temp{0}'; CREATE TABLE 'Temp{0}' AS SELECT * FROM '{0}' WHERE IsDeleted=false;", tableName));
                    //List<string> a = new List<string>();
                    var a = db.Database.SqlQuery<string>(string.Format("select '[' + name + '], ' as [text()] from sys.columns where object_id = object_id('Temp{0}') for xml path('')", tableName)).ToList(); // where object_id = object_id('{0}')
                    string b = "";
                    foreach (var c in a) b += c;
                    b = b.Substring(0, b.Length - 2);
                    //db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} ON", tableName));
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} ON; INSERT INTO Temp{0}({1}) SELECT {1} FROM {0} WHERE 1=1; SET IDENTITY_INSERT Temp{0} OFF;", tableName, b));
                    //db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} OFF", tableName));
                }

                foreach (var tableName in userDatas)
                {
                    var a = db.Database.SqlQuery<string>(string.Format("select '[' + name + '], ' as [text()] from sys.columns where object_id = object_id('Temp{0}') for xml path('')", tableName)).ToList(); // where object_id = object_id('{0}')
                    string b = "";
                    foreach (var c in a) b += c;
                    b = b.Substring(0, b.Length - 2);
                    // Create new table using source table structure, and then copy the data from source structure to new table
                    //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE Temp{0} LIKE {0}; INSERT Temp{0} SELECT * FROM {0};", tableName));
                    //db.Database.ExecuteSqlCommand(string.Format("DELETE FROM Temp{0}; CREATE TABLE Temp{0} AS SELECT * FROM {0} WHERE IsDeleted=false;", tableName));
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT Temp{0} ON; INSERT INTO Temp{0}({1}) SELECT {1} FROM {0} WHERE 1=1; SET IDENTITY_INSERT Temp{0} OFF;", tableName, b));
                }
            }
            return 0;
        }

        public int Restore(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                foreach (var tableName in Enumerable.Reverse(masterDatas))
                {
                    var a = db.Database.SqlQuery<string>(string.Format("select '[' + name + '], ' as [text()] from sys.columns where object_id = object_id('Temp{0}') for xml path('')", tableName)).ToList(); // where object_id = object_id('{0}')
                    string b = "";
                    foreach (var c in a) b += c;
                    b = b.Substring(0, b.Length - 2);
                    // Create new table using source table structure, and then copy the data from source structure to new table
                    //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE Temp{0} LIKE {0}; INSERT Temp{0} SELECT * FROM {0};", tableName));
                    //db.Database.ExecuteSqlCommand(string.Format("CREATE TABLE {0} AS SELECT * FROM Temp{0};", tableName));
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT {0} ON; INSERT INTO {0}({1}) SELECT {1} FROM Temp{0} WHERE 1=1; SET IDENTITY_INSERT {0} OFF;", tableName, b));
                }

                // Allow defining the ID
                foreach (var tableName in userDatas)
                {
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] ON;", tableName));
                }

                // Restore non-deleted records one-by-one
                //var tmps = db.Database.SqlQuery(typeof(TempCashSalesInvoice), "SELECT * FROM TempCashSalesInvoice");

                // Account
                foreach (var rec in db.TempAccounts.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    Account obj = new Account();
                    Reflection.CopyProperties(rec, obj);
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _accountService.CreateObject(obj, _accountService);
                    Log(obj.Errors, obj.GetType().Name, obj.Name, rec.Id, obj.Id);
                }

                // CashBank
                foreach (var rec in db.TempCashBanks.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    CashBank obj = new CashBank();
                    Reflection.CopyProperties(rec, obj);
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _cashBankService.CreateObject(obj, _accountService);
                    Log(obj.Errors, obj.GetType().Name, obj.Name, rec.Id, obj.Id);
                }

                FixAccounts();

                // CashBankAdjustment
                foreach (var rec in db.TempCashBankAdjustments.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    CashBankAdjustment obj = new CashBankAdjustment();
                    Reflection.CopyProperties(rec, obj);
                    obj.IsConfirmed = false;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _cashBankAdjustmentService.CreateObject(obj, _cashBankService);
                    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    if (rec.IsConfirmed)
                    {
                        _cashBankAdjustmentService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _cashMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                }

                // CashBankMutation
                foreach (var rec in db.TempCashBankMutations.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    CashBankMutation obj = new CashBankMutation();
                    Reflection.CopyProperties(rec, obj);
                    obj.IsConfirmed = false;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _cashBankMutationService.CreateObject(obj, _cashBankService);
                    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    if (rec.IsConfirmed)
                    {
                        _cashBankMutationService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _cashMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                }

                // Item
                foreach (var rec in db.TempItems.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    Item obj = new Item();
                    Reflection.CopyProperties(rec, obj);
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _itemService.CreateObject(obj, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                    Log(obj.Errors, obj.GetType().Name, obj.Sku, rec.Id, obj.Id);
                }

                // StockAdjustment
                foreach (var rec in db.TempStockAdjustments.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    StockAdjustment obj = new StockAdjustment();
                    Reflection.CopyProperties(rec, obj);
                    obj.IsConfirmed = false;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _stockAdjustmentService.CreateObject(obj, _warehouseService);
                    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    foreach (var det in db.TempStockAdjustmentDetails.Where(x => !x.IsDeleted && x.StockAdjustmentId == rec.Id).OrderBy(x => x.Id).ToList())
                    {
                        StockAdjustmentDetail detobj = new StockAdjustmentDetail();
                        Reflection.CopyProperties(det, detobj);
                        detobj.IsConfirmed = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                        _stockAdjustmentDetailService.CreateObject(detobj, _stockAdjustmentService, _itemService, _warehouseItemService);
                        Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                    }
                    if (rec.IsConfirmed)
                    {
                        _stockAdjustmentService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService, _generalLedgerJournalService, _accountService, _closingService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                }

                // Warehouse Mutation
                foreach (var rec in db.TempWarehouseMutationOrders.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    WarehouseMutationOrder obj = new WarehouseMutationOrder();
                    Reflection.CopyProperties(rec, obj);
                    obj.IsConfirmed = false;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _warehouseMutationOrderService.CreateObject(obj, _warehouseService);
                    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    foreach (var det in db.TempWarehouseMutationOrderDetails.Where(x => !x.IsDeleted && x.WarehouseMutationOrderId == rec.Id).OrderBy(x => x.Id).ToList())
                    {
                        WarehouseMutationOrderDetail detobj = new WarehouseMutationOrderDetail();
                        Reflection.CopyProperties(det, detobj);
                        detobj.IsConfirmed = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                        _warehouseMutationOrderDetailService.CreateObject(detobj, _warehouseMutationOrderService, _itemService, _warehouseItemService);
                        Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                    }
                    if (rec.IsConfirmed)
                    {
                        _warehouseMutationOrderService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _warehouseMutationOrderDetailService, _itemService, _barringService, _warehouseItemService, _stockMutationService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                }

                // CustomPurchaseInvoice
                foreach (var rec in db.TempCustomPurchaseInvoices.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    try
                    {
                        CustomPurchaseInvoice obj = new CustomPurchaseInvoice();
                        Reflection.CopyProperties(rec, obj);
                        obj.IsConfirmed = false;
                        obj.IsPaid = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                        _customPurchaseInvoiceService.CreateObject(obj, _warehouseService, _contactService, _cashBankService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        foreach (var det in db.TempCustomPurchaseInvoiceDetails.Where(x => !x.IsDeleted && x.CustomPurchaseInvoiceId == rec.Id).OrderBy(x => x.Id).ToList())
                        {
                            CustomPurchaseInvoiceDetail detobj = new CustomPurchaseInvoiceDetail();
                            Reflection.CopyProperties(det, detobj);
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                            _customPurchaseInvoiceDetailService.CreateObject(detobj, _customPurchaseInvoiceService, _itemService, _warehouseItemService);
                            Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                        }
                        TempPayable payable = db.TempPayables.Where(x => !x.IsDeleted && x.PayableSourceId == rec.Id && x.PayableSource == Constant.PayableSource.CustomPurchaseInvoice).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (rec.IsConfirmed)
                        {
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Payable, RESEED, {0});", payable.Id - 1));
                            _customPurchaseInvoiceService.ConfirmObjectForRepair(obj, rec.ConfirmationDate.GetValueOrDefault(), payable.Code,
                                                                _customPurchaseInvoiceDetailService, _contactService, _priceMutationService,
                                                                _payableService, _customPurchaseInvoiceService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService,
                                                                _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                        if (rec.IsPaid)
                        {
                            TempPaymentVoucherDetail voucherDetail = db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (voucherDetail == null) 
                            {
                                voucherDetail = db.TempPaymentVoucherDetails.Where(x => x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            TempPaymentVoucher voucher = db.TempPaymentVouchers.Where(x => !x.IsDeleted && x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (voucher == null)
                            {
                                voucher = db.TempPaymentVouchers.Where(x => x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucher, RESEED, {0});", voucher.Id - 1));
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucherDetail, RESEED, {0});", voucherDetail.Id - 1));
                            _customPurchaseInvoiceService.PaidObjectForRepair(obj, rec.AmountPaid.GetValueOrDefault(), voucher.Code, voucherDetail.Code,
                                                                _cashBankService, _payableService, _paymentVoucherService, _paymentVoucherDetailService, _contactService, _cashMutationService,
                                                                _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                    }
                    catch 
                    {
                        Console.WriteLine("ERROR at {0}:{1}", rec.GetType().Name, rec.Code);
                    }
                }

                // CashSalesInvoice
                foreach (var rec in db.TempCashSalesInvoices.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    try
                    {
                        CashSalesInvoice obj = new CashSalesInvoice();
                        Reflection.CopyProperties(rec, obj);
                        obj.IsConfirmed = false;
                        obj.IsPaid = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                        _cashSalesInvoiceService.CreateObject(obj, _warehouseService, _cashBankService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        foreach (var det in db.TempCashSalesInvoiceDetails.Where(x => !x.IsDeleted && x.CashSalesInvoiceId == rec.Id).OrderBy(x => x.Id).ToList())
                        {
                            CashSalesInvoiceDetail detobj = new CashSalesInvoiceDetail();
                            Reflection.CopyProperties(det, detobj);
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                            _cashSalesInvoiceDetailService.CreateObject(detobj, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);
                            Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                        }
                        TempReceivable receivable = db.TempReceivables.Where(x => !x.IsDeleted && x.ReceivableSourceId == rec.Id && x.ReceivableSource == Constant.ReceivableSource.CashSalesInvoice).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (rec.IsConfirmed)
                        {
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Receivable, RESEED, {0});", receivable.Id - 1));
                            _cashSalesInvoiceService.ConfirmObjectForRepair(obj, rec.ConfirmationDate.GetValueOrDefault(), rec.Discount, rec.Tax, receivable.Code,
                                                                _cashSalesInvoiceDetailService, _contactService, _priceMutationService,
                                                                _receivableService, _cashSalesInvoiceService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService, _cashBankService,
                                                                _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                        if (rec.IsPaid)
                        {
                            TempReceiptVoucherDetail voucherDetail = db.TempReceiptVoucherDetails.Where(x => !x.IsDeleted && x.ReceivableId == receivable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (voucherDetail == null)
                            {
                                voucherDetail = db.TempReceiptVoucherDetails.Where(x => x.ReceivableId == receivable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            TempReceiptVoucher voucher = db.TempReceiptVouchers.Where(x => !x.IsDeleted && x.Id == voucherDetail.ReceiptVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (voucher == null)
                            {
                                voucher = db.TempReceiptVouchers.Where(x => x.Id == voucherDetail.ReceiptVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (ReceiptVoucher, RESEED, {0});", voucher.Id - 1));
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (ReceiptVoucherDetail, RESEED, {0});", voucherDetail.Id - 1));
                            _cashSalesInvoiceService.PaidObjectForRepair(obj, rec.AmountPaid.GetValueOrDefault(), rec.Allowance, voucher.Code, voucherDetail.Code,
                                                                _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService,
                                                                _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("ERROR at {0}:{1}", rec.GetType().Name, rec.Code);
                    }
                }

                // ReceiptVoucher
                //foreach (var rec in db.TempReceiptVouchers.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                //{
                //    ReceiptVoucher obj = new ReceiptVoucher();
                //    Reflection.CopyProperties(rec, obj);
                //    obj.IsConfirmed = false;
                //    obj.IsReconciled = false;
                //    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                //    _receiptVoucherService.CreateObject(obj, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
                //    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                //    foreach (var det in db.TempReceiptVoucherDetails.Where(x => !x.IsDeleted && x.ReceiptVoucherId == rec.Id).OrderBy(x => x.Id).ToList())
                //    {
                //        ReceiptVoucherDetail detobj = new ReceiptVoucherDetail();
                //        Reflection.CopyProperties(det, detobj);
                //        detobj.IsConfirmed = false;
                //        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                //        _receiptVoucherDetailService.CreateObject(detobj, _receiptVoucherService, _cashBankService, _receivableService);
                //        Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                //    }
                //    if (rec.IsConfirmed)
                //    {
                //        _receiptVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService, 
                //                                    _generalLedgerJournalService, _accountService, _closingService);
                //        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                //    }
                //    if (rec.IsReconciled)
                //    {
                //        _receiptVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashMutationService, _cashBankService, _receivableService, _generalLedgerJournalService, _accountService, _closingService);
                //        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                //    }
                //}

                // ReceiptVoucherDetail
                foreach (var det in db.TempReceiptVoucherDetails.Where(x => !x.IsDeleted && !x.Description.Contains("Automatic")).OrderBy(x => x.Id).ToList())
                {
                    TempReceiptVoucher rec = db.TempReceiptVouchers.Where(x => !x.IsDeleted && x.Id == det.ReceiptVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                    ReceiptVoucher obj = _receiptVoucherService.GetObjectById(rec.Id);
                    if (obj == null)
                    {
                        obj = new ReceiptVoucher();
                        Reflection.CopyProperties(rec, obj);
                        obj.IsConfirmed = false;
                        obj.IsReconciled = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                        _receiptVoucherService.CreateObject(obj, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                    TempReceivable tmp = db.TempReceivables.Where(x => !x.IsDeleted && x.Id == det.ReceivableId).FirstOrDefault();
                    ReceiptVoucherDetail detobj = new ReceiptVoucherDetail();
                    Reflection.CopyProperties(det, detobj);
                    detobj.IsConfirmed = false;
                    detobj.ReceivableId = _receivableService.GetObjectBySource(tmp.ReceivableSource, tmp.ReceivableSourceId).Id;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                    _receiptVoucherDetailService.CreateObject(detobj, _receiptVoucherService, _cashBankService, _receivableService);
                    Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                }
                // ReceiptVoucher
                foreach (var rec in db.TempReceiptVouchers.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    ReceiptVoucher obj = _receiptVoucherService.GetObjectById(rec.Id);
                    if (obj != null)
                    {
                        if (rec.IsConfirmed && !obj.IsConfirmed)
                        {
                            _receiptVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService,
                                                        _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                        if (rec.IsReconciled && !obj.IsReconciled)
                        {
                            _receiptVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _receiptVoucherDetailService, _cashMutationService, _cashBankService, _receivableService, _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                    }
                }

                // CashSalesReturn
                foreach (var rec in db.TempCashSalesReturns.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    try
                    {
                        CashSalesReturn obj = new CashSalesReturn();
                        Reflection.CopyProperties(rec, obj);
                        obj.IsConfirmed = false;
                        obj.IsPaid = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                        _cashSalesReturnService.CreateObject(obj, _cashSalesInvoiceService, _cashBankService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        foreach (var det in db.TempCashSalesReturnDetails.Where(x => !x.IsDeleted && x.CashSalesReturnId == rec.Id).OrderBy(x => x.Id).ToList())
                        {
                            CashSalesReturnDetail detobj = new CashSalesReturnDetail();
                            Reflection.CopyProperties(det, detobj);
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                            _cashSalesReturnDetailService.CreateObject(detobj, _cashSalesReturnService, _cashSalesInvoiceDetailService);
                            Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                        }
                        TempPayable payable = db.TempPayables.Where(x => !x.IsDeleted && x.PayableSourceId == rec.Id && x.PayableSource == Constant.PayableSource.CashSalesReturn).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (rec.IsConfirmed)
                        {
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Payable, RESEED, {0});", payable.Id - 1));
                            _cashSalesReturnService.ConfirmObjectForRepair(obj, rec.ConfirmationDate.GetValueOrDefault(), rec.Allowance, payable.Code,
                                                                _cashSalesReturnDetailService, _contactService, _cashSalesInvoiceService, _cashSalesInvoiceDetailService, _priceMutationService,
                                                                _payableService, _cashSalesReturnService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService,
                                                                _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                        if (rec.IsPaid)
                        {
                            TempPaymentVoucherDetail voucherDetail = db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (voucherDetail == null)
                            {
                                voucherDetail = db.TempPaymentVoucherDetails.Where(x => x.PayableId == payable.Id && x.Description.Contains("Automatic")).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            TempPaymentVoucher voucher = db.TempPaymentVouchers.Where(x => !x.IsDeleted && x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (voucher == null)
                            {
                                voucher = db.TempPaymentVouchers.Where(x => x.Id == voucherDetail.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                            }
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucher, RESEED, {0});", voucher.Id - 1));
                            db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentVoucherDetail, RESEED, {0});", voucherDetail.Id - 1));
                            _cashSalesReturnService.PaidObjectForRepair(obj, voucher.Code, voucherDetail.Code,
                                                                _cashBankService, _payableService, _paymentVoucherService, _paymentVoucherDetailService, _contactService, _cashMutationService,
                                                                _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("ERROR at {0}:{1}", rec.GetType().Name, rec.Code);
                    }
                }

                // PaymentRequest
                foreach (var rec in db.TempPaymentRequests.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    TempPaymentRequestDetail tmp = db.TempPaymentRequestDetails.Where(x => !x.IsDeleted && x.PaymentRequestId == rec.Id && x.IsLegacy).OrderByDescending(x => x.Id).FirstOrDefault();
                    PaymentRequest obj = new PaymentRequest();
                    Reflection.CopyProperties(rec, obj);
                    obj.IsConfirmed = false;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (PaymentRequestDetail, RESEED, {0});", tmp.Id - 1));
                    _paymentRequestService.CreateObject(obj, _contactService, _paymentRequestDetailService, _accountService, _generalLedgerJournalService, _closingService);
                    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    foreach (var det in db.TempPaymentRequestDetails.Where(x => !x.IsDeleted && x.PaymentRequestId == rec.Id && !x.IsLegacy).OrderBy(x => x.Id).ToList())
                    {
                        PaymentRequestDetail detobj = new PaymentRequestDetail();
                        Reflection.CopyProperties(det, detobj);
                        detobj.IsConfirmed = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                        _paymentRequestDetailService.CreateObject(detobj, _paymentRequestService, _accountService);
                        Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                    }
                    if (rec.IsConfirmed)
                    {
                        _paymentRequestService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _payableService, _paymentRequestDetailService, 
                                                    _accountService, _generalLedgerJournalService, _closingService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                }

                // PaymentVoucher
                //foreach (var rec in db.TempPaymentVouchers.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                //{
                //    PaymentVoucher obj = new PaymentVoucher();
                //    Reflection.CopyProperties(rec, obj);
                //    obj.IsConfirmed = false;
                //    obj.IsReconciled = false;
                //    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                //    _paymentVoucherService.CreateObject(obj, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
                //    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                //    foreach (var det in db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && x.PaymentVoucherId == rec.Id).OrderBy(x => x.Id).ToList())
                //    {
                //        PaymentVoucherDetail detobj = new PaymentVoucherDetail();
                //        Reflection.CopyProperties(det, detobj);
                //        detobj.IsConfirmed = false;
                //        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                //        _paymentVoucherDetailService.CreateObject(detobj, _paymentVoucherService, _cashBankService, _payableService);
                //        Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                //    }
                //    if (rec.IsConfirmed)
                //    {
                //        _paymentVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService,
                //                                    _generalLedgerJournalService, _accountService, _closingService);
                //        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                //    }
                //    if (rec.IsReconciled)
                //    {
                //        _paymentVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService, _generalLedgerJournalService, _accountService, _closingService);
                //        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                //    }
                //}

                // PaymentVoucherDetail
                foreach (var det in db.TempPaymentVoucherDetails.Where(x => !x.IsDeleted && !x.Description.Contains("Automatic")).OrderBy(x => x.Id).ToList())
                {
                    TempPaymentVoucher rec = db.TempPaymentVouchers.Where(x => !x.IsDeleted && x.Id == det.PaymentVoucherId).OrderByDescending(x => x.Id).FirstOrDefault();
                    PaymentVoucher obj = _paymentVoucherService.GetObjectById(rec.Id);
                    if (obj == null)
                    {
                        obj = new PaymentVoucher();
                        Reflection.CopyProperties(rec, obj);
                        obj.IsConfirmed = false;
                        obj.IsReconciled = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                        _paymentVoucherService.CreateObject(obj, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                    TempPayable tmp = db.TempPayables.Where(x => !x.IsDeleted && x.Id == det.PayableId).FirstOrDefault();
                    PaymentVoucherDetail detobj = new PaymentVoucherDetail();
                    Reflection.CopyProperties(det, detobj);
                    detobj.IsConfirmed = false;
                    detobj.PayableId = _payableService.GetObjectBySource(tmp.PayableSource, tmp.PayableSourceId).Id;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                    _paymentVoucherDetailService.CreateObject(detobj, _paymentVoucherService, _cashBankService, _payableService);
                    Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                }
                // PaymentVoucher
                foreach (var rec in db.TempPaymentVouchers.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    PaymentVoucher obj = _paymentVoucherService.GetObjectById(rec.Id);
                    if (obj != null)
                    {
                        if (rec.IsConfirmed && !obj.IsConfirmed)
                        {
                            _paymentVoucherService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService,
                                                        _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                        if (rec.IsReconciled && !obj.IsReconciled)
                        {
                            _paymentVoucherService.ReconcileObject(obj, rec.ReconciliationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService, _generalLedgerJournalService, _accountService, _closingService);
                            Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                        }
                    }
                }

                // Memorial
                foreach (var rec in db.TempMemorials.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToList())
                {
                    Memorial obj = new Memorial();
                    Reflection.CopyProperties(rec, obj);
                    obj.IsConfirmed = false;
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", obj.GetType().Name, rec.Id - 1));
                    _memorialService.CreateObject(obj);
                    Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    foreach (var det in db.TempMemorialDetails.Where(x => !x.IsDeleted && x.MemorialId == rec.Id).OrderBy(x => x.Id).ToList())
                    {
                        MemorialDetail detobj = new MemorialDetail();
                        Reflection.CopyProperties(det, detobj);
                        detobj.IsConfirmed = false;
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, {1});", detobj.GetType().Name, det.Id - 1));
                        _memorialDetailService.CreateObject(detobj, _memorialService, _accountService);
                        Log(detobj.Errors, detobj.GetType().Name, detobj.Code, det.Id, detobj.Id);
                    }
                    if (rec.IsConfirmed)
                    {
                        _memorialService.ConfirmObject(obj, rec.ConfirmationDate.GetValueOrDefault(), _memorialDetailService, _accountService, _generalLedgerJournalService, _closingService);
                        Log(obj.Errors, obj.GetType().Name, obj.Code, rec.Id, obj.Id);
                    }
                }

                // DisAllow defining the ID
                foreach (var tableName in userDatas)
                {
                    db.Database.ExecuteSqlCommand(string.Format("SET IDENTITY_INSERT [dbo].[{0}] OFF;", tableName));
                }

            }
            return 0;
        }

        public int DeleteTemp(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                foreach (var tableName in userDatas)
                {
                    //try
                    {
                        db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Temp{0}, RESEED, 0); ALTER TABLE Temp{0} NOCHECK CONSTRAINT ALL; DELETE FROM Temp{0};", tableName)); // ALTER TABLE Temp{0} DROP CONSTRAINT ALL; // ALTER TABLE Temp{0} CHECK CONSTRAINT ALL;
                    }
                    //catch (Exception ex)
                    {

                    }
                }

                foreach (var tableName in masterDatas)
                {
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT (Temp{0}, RESEED, 0); ALTER TABLE Temp{0} NOCHECK CONSTRAINT ALL; DELETE FROM Temp{0};", tableName));
                }
            }
            return 0;
        }

        public int DeleteOriginal(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {

                IList<String> tableNames = new List<String>();

                IList<String> userroleNames = new List<String>() { "UserMenu", "UserAccount", "UserAccess" };

                IList<String> financeNames = new List<String>()
                                        {   "PaymentVoucherDetail", "PaymentVoucher", "Payable",
                                            "ReceiptVoucherDetail", "ReceiptVoucher", "Receivable",
                                            "PaymentRequestDetail", "PaymentRequest",
                                            "CashMutation", "CashBankAdjustment", "CashBankMutation", "CashBank"};
                IList<String> manufacturingNames = new List<String>()
                                        { "RollerWarehouseMutationDetail", "RollerWarehouseMutation",
                                          "RecoveryAccessoryDetail", "RecoveryOrderDetail", "RecoveryOrder",
                                          "CoreIdentificationDetail", "CoreIdentification",
                                          "BarringOrderDetail", "BarringOrder" };
                IList<String> purchaseOperationNames = new List<String>()
                                        { "RetailPurchaseInvoice", "RetailPurchaseInvoiceDetail", "PurchaseInvoiceDetail", "PurchaseInvoice",
                                          "PurchaseReceivalDetail", "PurchaseReceival", "PurchaseOrderDetail", "PurchaseOrder",
                                          "CustomPurchaseInvoiceDetail", "CustomPurchaseInvoice" };
                IList<String> salesOperationNames = new List<String>()
                                        { "RetailSalesInvoiceDetail", "RetailSalesInvoice", "SalesInvoiceDetail", "SalesInvoice",
                                          "DeliveryOrderDetail", "DeliveryOrder", "SalesOrderDetail", "SalesOrder", "CashSalesReturnDetail", "CashSalesReturn", 
                                          "CashSalesInvoiceDetail", "CashSalesInvoice"};
                IList<String> stockAndMasterNames = new List<String>()
                                        { "GroupItemPrice", "QuantityPricing", "PriceMutation", "StockMutation", "WarehouseMutationOrderDetail", "WarehouseMutationOrder",
                                          "RollerBuilder", "StockAdjustmentDetail", "StockAdjustment", "WarehouseItem",
                                          "Warehouse", "CoreBuilder", "Item", "ItemType", "UoM", "Contact",
                                          "RollerType", "Machine", "ContactGroup", "Company"};

                IList<String> accountingNames = new List<String>() { "MemorialDetail", "Memorial", "GeneralLedgerJournal", "ValidComb", "Closing", "Account" };

                userroleNames.ToList().ForEach(x => tableNames.Add(x));
                financeNames.ToList().ForEach(x => tableNames.Add(x));
                manufacturingNames.ToList().ForEach(x => tableNames.Add(x));
                purchaseOperationNames.ToList().ForEach(x => tableNames.Add(x));
                salesOperationNames.ToList().ForEach(x => tableNames.Add(x));
                stockAndMasterNames.ToList().ForEach(x => tableNames.Add(x));
                accountingNames.ToList().ForEach(x => tableNames.Add(x));

                foreach (var tableName in tableNames)
                {
                    db.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ({0}, RESEED, 0); ALTER TABLE {0} NOCHECK CONSTRAINT ALL; DELETE FROM {0}; ALTER TABLE {0} CHECK CONSTRAINT ALL;", tableName));
                }

                //foreach (var tableName in tableNames)
                //{
                //    db.Database.ExecuteSqlCommand(string.Format("DROP TABLE {0}", tableName));
                //}

            }
            return 0;
        }
    }
}