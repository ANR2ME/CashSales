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

    public class FixFunctions
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
        public IRetailSalesInvoiceService _retailSalesInvoiceService;
        public IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService;

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

        struct sourceid {
                public string source;
                public int id;
            };

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

        public FixFunctions()
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
            _retailSalesInvoiceService = new RetailSalesInvoiceService(new RetailSalesInvoiceRepository(), new RetailSalesInvoiceValidator());
            _retailSalesInvoiceDetailService = new RetailSalesInvoiceDetailService(new RetailSalesInvoiceDetailRepository(), new RetailSalesInvoiceDetailValidator());

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

        public void FixLegacyAccounts()
        {
            //if (!_accountService.GetLegacyObjects().Any())
            {
                Account Asset = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = (int)Constant.AccountGroup.Asset, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                Account AccountReceivable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable", Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (PPN masuk)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountReceivablePPNmasukan, LegacyCode = Constant.AccountLegacyCode.AccountReceivablePPNmasukan, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Receivable (Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountReceivableTrading, LegacyCode = Constant.AccountLegacyCode.AccountReceivableTrading, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = AccountReceivable.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Receivable", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                Account Inventory = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Inventory", Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = (int)Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Trading Goods", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.TradingGoods, LegacyCode = Constant.AccountLegacyCode.TradingGoods, Level = 3, Group = (int)Constant.AccountGroup.Asset, ParentId = Inventory.Id, IsLegacy = true });

                Account Expense = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = (int)Constant.AccountGroup.Expense, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                Account OperationalExpenses = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Operational Expenses", Code = Constant.AccountCode.OperatingExpenses, LegacyCode = Constant.AccountLegacyCode.OperatingExpenses, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight Out Expenses", Code = Constant.AccountCode.FreightOutExpense, LegacyCode = Constant.AccountLegacyCode.FreightOutExpense, Level = 3, Group = (int)Constant.AccountGroup.Expense, ParentId = OperationalExpenses.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Discount", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesDiscountExpense, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesAllowanceExpense, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Expense", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesReturnExpense, LegacyCode = Constant.AccountLegacyCode.SalesReturnExpense, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight In", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.FreightIn, LegacyCode = Constant.AccountLegacyCode.FreightIn, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                // Memorial Expenses
                //Account NonOperationalExpenses = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Non-Operational Expenses", Code = Constant.AccountCode.NonOperationalExpenses, LegacyCode = Constant.AccountLegacyCode.NonOperationalExpenses, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Tax Expense", IsLeaf = true, Code = Constant.AccountCode.Tax, LegacyCode = Constant.AccountLegacyCode.Tax, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Divident", IsLeaf = true, Code = Constant.AccountCode.Divident, LegacyCode = Constant.AccountLegacyCode.Divident, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Interest Earning", IsLeaf = true, Code = Constant.AccountCode.InterestExpense, LegacyCode = Constant.AccountLegacyCode.InterestExpense, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Depreciation", IsLeaf = true, Code = Constant.AccountCode.Depreciation, LegacyCode = Constant.AccountLegacyCode.Depreciation, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Amortization", IsLeaf = true, Code = Constant.AccountCode.Amortization, LegacyCode = Constant.AccountLegacyCode.Amortization, Level = 2, Group = (int)Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                

                Account Liability = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = (int)Constant.AccountGroup.Liability, IsLegacy = true });
                Account AccountPayable = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable", Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayableTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableTrading, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (Non Trading)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayableNonTrading, LegacyCode = Constant.AccountLegacyCode.AccountPayableNonTrading, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Account Payable (PPN keluaran)", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.AccountPayablePPNkeluaran, LegacyCode = Constant.AccountLegacyCode.AccountPayablePPNkeluaran, Level = 3, Group = (int)Constant.AccountGroup.Liability, ParentId = AccountPayable.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "GBCH Payable", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Discount", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.PurchaseDiscount, LegacyCode = Constant.AccountLegacyCode.PurchaseDiscount, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Purchase Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.PurchaseAllowance, LegacyCode = Constant.AccountLegacyCode.PurchaseAllowance, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales Return Allowance", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesReturnAllowance, LegacyCode = Constant.AccountLegacyCode.SalesReturnAllowance, Level = 2, Group = (int)Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });

                Account Equity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = (int)Constant.AccountGroup.Equity, IsLegacy = true });
                Account OwnersEquity = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = (int)Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Equity Adjustment", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 3, Group = (int)Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Retained Earnings", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.RetainedEarnings, LegacyCode = Constant.AccountLegacyCode.RetainedEarnings, Level = 2, Group = (int)Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });

                Account Revenue = _accountService.FindOrCreateLegacyObject(new Account() { Name = "Revenue", Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = (int)Constant.AccountGroup.Revenue, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Freight Out", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.FreightOut, LegacyCode = Constant.AccountLegacyCode.FreightOut, Level = 2, Group = (int)Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
                _accountService.FindOrCreateLegacyObject(new Account() { Name = "Sales", IsUsedBySystem = true, IsLeaf = true, Code = Constant.AccountCode.SalesRevenue, LegacyCode = Constant.AccountLegacyCode.SalesRevenue, Level = 2, Group = (int)Constant.AccountGroup.Revenue, ParentId = Revenue.Id, IsLegacy = true });
            }
        }

        public int FixJournal(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                Dictionary<string, int> src = new Dictionary<string, int>();
                var list = db.GeneralLedgerJournals.Where(x => !x.IsDeleted).OrderBy(x => x.SourceDocument).ThenBy(x => x.Id);
                foreach (var gl in list)
                {
                    switch (gl.SourceDocument)
                    {
                        case Constant.GeneralLedgerSource.CashBankAdjustment:
                            //var obj = _cashBankAdjustmentService.GetObjectById(x.SourceDocumentId);
                            var obj = db.CashBankAdjustments.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault();
                            //if (obj == null) obj = db.CashBankAdjustments.Where(x => x.Id == gl.SourceDocumentId).FirstOrDefault();
                            if (obj.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj.AdjustmentDate)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj.AdjustmentDate.ToShortDateString());
                                gl.TransactionDate = obj.AdjustmentDate;
                            }
                            break;
                        case  Constant.GeneralLedgerSource.CashBankMutation:
                            //var obj1 = _cashBankMutationService.GetObjectById(gl.SourceDocumentId);
                            var obj1 = db.CashBankMutations.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault(); 
                            if (obj1.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj1.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj1.CreatedAt)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj1.CreatedAt.ToShortDateString());
                                //gl.TransactionDate = obj1.CreatedAt;
                            }
                            break;
                        case Constant.GeneralLedgerSource.CashSalesInvoice:
                            //var obj2 = _cashSalesInvoiceService.GetObjectById(gl.SourceDocumentId);
                            var obj2 = db.CashSalesInvoices.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault(); 
                            if (obj2.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj2.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj2.SalesDate)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj2.SalesDate.ToShortDateString());
                                gl.TransactionDate = obj2.SalesDate;
                            }
                            break;
                        case Constant.GeneralLedgerSource.CashSalesReturn:
                            //var obj3 = _cashSalesReturnService.GetObjectById(gl.SourceDocumentId);
                            var obj3 = db.CashSalesReturns.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault(); 
                            if (obj3.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj3.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj3.ReturnDate.GetValueOrDefault())
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj3.ReturnDate.GetValueOrDefault().ToShortDateString());
                                gl.TransactionDate = obj3.ReturnDate.GetValueOrDefault();
                            }
                            break;
                        case Constant.GeneralLedgerSource.CustomPurchaseInvoice:
                            //var obj4 = _customPurchaseInvoiceService.GetObjectById(gl.SourceDocumentId);
                            var obj4 = db.CustomPurchaseInvoices.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault(); 
                            if (obj4.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj4.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj4.PurchaseDate)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj4.PurchaseDate.ToShortDateString());
                                gl.TransactionDate = obj4.PurchaseDate;
                            }
                            break;
                        case Constant.GeneralLedgerSource.Memorial:
                            //var obj5 = _memorialService.GetObjectById(gl.SourceDocumentId);
                            var obj5 = db.Memorials.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault(); 
                            if (obj5.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj5.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj5.CreatedAt)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj5.CreatedAt.ToShortDateString());
                                //x.TransactionDate = obj5.CreatedAt;
                            }
                            break;
                        case Constant.GeneralLedgerSource.PaymentRequest:
                            //var obj6 = _paymentRequestService.GetObjectById(gl.SourceDocumentId);
                            var obj6 = db.PaymentRequests.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault(); 
                            if (obj6.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj6.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj6.RequestedDate)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj6.RequestedDate.ToShortDateString());
                                gl.TransactionDate = obj6.RequestedDate;
                            }
                            break;
                        case Constant.GeneralLedgerSource.PaymentVoucher:
                            //var obj7 = _paymentVoucherService.GetObjectById(gl.SourceDocumentId);
                            var obj7 = db.PaymentVouchers.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault(); 
                            if (obj7.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj7.ConfirmationDate.GetValueOrDefault();
                            }
                            else //if (gl.TransactionDate > obj7.PaymentDate)
                            {
                                var pvdet = db.PaymentVoucherDetails.Where(x => x.PaymentVoucherId == obj7.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                                var payable = db.Payables.Where(x => x.Id == pvdet.PayableId).OrderByDescending(x => x.Id).FirstOrDefault();
                                if (pvdet.Description != null && pvdet.Description.Contains("Automatic")) 
                                {
                                    switch (payable.PayableSource)
                                    {
                                        case Constant.PayableSource.CustomPurchaseInvoice:
                                            var cpi = db.CustomPurchaseInvoices.Where(x => x.Id == payable.PayableSourceId).OrderByDescending(x => x.Id).FirstOrDefault();
                                            DateTime cpidate = cpi.PurchaseDate;
                                            //if (cpi.PaymentDate != null) cpidate = cpi.PaymentDate.GetValueOrDefault();
                                            //else 
                                            if (cpi.ConfirmationDate != null) cpidate = cpi.ConfirmationDate.GetValueOrDefault();
                                            gl.TransactionDate = cpidate;
                                            break;
                                        case Constant.PayableSource.CashSalesReturn:
                                            var csr = db.CashSalesReturns.Where(x => x.Id == payable.PayableSourceId).OrderByDescending(x => x.Id).FirstOrDefault();
                                            DateTime csrdate = csr.ReturnDate.GetValueOrDefault();
                                            if (csr.ConfirmationDate != null) csrdate = csr.ConfirmationDate.GetValueOrDefault();
                                            gl.TransactionDate = csrdate;
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj7.PaymentDate.ToShortDateString());
                                    gl.TransactionDate = obj7.PaymentDate; // TODO : automatic payment should use the source invoice confirmation date
                                }
                            }
                            break;
                        case Constant.GeneralLedgerSource.ReceiptVoucher:
                            //var obj8 = _receiptVoucherService.GetObjectById(gl.SourceDocumentId);
                            var obj8 = db.ReceiptVouchers.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj8.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj8.ConfirmationDate.GetValueOrDefault();
                            }
                            else //if (gl.TransactionDate > obj8.ReceiptDate)
                            {
                                var rvdet = db.ReceiptVoucherDetails.Where(x => x.ReceiptVoucherId == obj8.Id).OrderByDescending(x => x.Id).FirstOrDefault();
                                var recv = db.Receivables.Where(x => x.Id == rvdet.ReceivableId).OrderByDescending(x => x.Id).FirstOrDefault();
                                if (rvdet.Description != null && rvdet.Description.Contains("Automatic"))
                                {
                                    switch (recv.ReceivableSource)
                                    {
                                        case Constant.ReceivableSource.CashSalesInvoice:
                                            var csi = db.CashSalesInvoices.Where(x => x.Id == recv.ReceivableSourceId).OrderByDescending(x => x.Id).FirstOrDefault();
                                            DateTime csidate = csi.SalesDate;
                                            if (csi.ConfirmationDate != null) csidate = csi.ConfirmationDate.GetValueOrDefault();
                                            gl.TransactionDate = csidate;
                                            break;
                                        case Constant.ReceivableSource.RetailSalesInvoice:
                                            var rsi = db.RetailSalesInvoices.Where(x => x.Id == recv.ReceivableSourceId).OrderByDescending(x => x.Id).FirstOrDefault();
                                            DateTime rsidate = rsi.SalesDate;
                                            if (rsi.ConfirmationDate != null) rsidate = rsi.ConfirmationDate.GetValueOrDefault();
                                            gl.TransactionDate = rsidate;
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj8.ReceiptDate.ToShortDateString());
                                    gl.TransactionDate = obj8.ReceiptDate; // TODO : automatic payment should use the source invoice confirmation date
                                }
                            }
                            break;
                        case Constant.GeneralLedgerSource.StockAdjustment:
                            //var obj9 = _stockAdjustmentService.GetObjectById(gl.SourceDocumentId);
                            var obj9 = db.StockAdjustments.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj9.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj9.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj9.AdjustmentDate)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj9.AdjustmentDate.ToShortDateString());
                                gl.TransactionDate = obj9.AdjustmentDate;
                            }
                            break;
                        case Constant.GeneralLedgerSource.RetailSalesInvoice:
                            //var obj10 = _retailSalesInvoiceService.GetObjectById(gl.SourceDocumentId);
                            var obj10 = db.RetailSalesInvoices.Where(x => x.Id == gl.SourceDocumentId).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (obj10.ConfirmationDate != null)
                            {
                                gl.TransactionDate = obj10.ConfirmationDate.GetValueOrDefault();
                            }
                            else if (gl.TransactionDate > obj10.SalesDate)
                            {
                                Console.WriteLine("[" + gl.Id + "]" + gl.SourceDocument + "[" + gl.SourceDocumentId + "] " + gl.TransactionDate.ToShortDateString() + " -> " + obj10.SalesDate.ToShortDateString());
                                gl.TransactionDate = obj10.SalesDate;
                            }
                            break;
                    }
                }
                db.SaveChanges();
            }
            return 0;
        }

        
    }
}