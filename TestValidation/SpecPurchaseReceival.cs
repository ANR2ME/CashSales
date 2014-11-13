using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;
using Core.Constants;

namespace TestValidation
{

    public class SpecPurchaseReceival : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
        Item item_batiktulis;
        Item item_busway;
        Item item_botolaqua;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
        PurchaseOrder purchaseOrder1;
        PurchaseOrder purchaseOrder2;
        PurchaseOrderDetail purchaseOrderDetail_batiktulis_so1;
        PurchaseOrderDetail purchaseOrderDetail_busway_so1;
        PurchaseOrderDetail purchaseOrderDetail_botolaqua_so1;
        PurchaseOrderDetail purchaseOrderDetail_batiktulis_so2;
        PurchaseOrderDetail purchaseOrderDetail_busway_so2;
        PurchaseOrderDetail purchaseOrderDetail_botolaqua_so2;
        PurchaseReceival purchaseReceival1;
        PurchaseReceival purchaseReceival2;
        PurchaseReceival purchaseReceival3;
        PurchaseReceivalDetail purchaseReceivalDetail_batiktulis_do1;
        PurchaseReceivalDetail purchaseReceivalDetail_busway_do1;
        PurchaseReceivalDetail purchaseReceivalDetail_botolaqua_do1;
        PurchaseReceivalDetail purchaseReceivalDetail_batiktulis_do2a;
        PurchaseReceivalDetail purchaseReceivalDetail_batiktulis_do2b;
        PurchaseReceivalDetail purchaseReceivalDetail_busway_do2;
        PurchaseReceivalDetail purchaseReceivalDetail_botolaqua_do2;
        IContactService _contactService;
        IItemService _itemService;
        IStockMutationService _stockMutationService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        IPurchaseInvoiceService _purchaseInvoiceService;
        IPurchaseOrderService _purchaseOrderService;
        IPurchaseOrderDetailService _purchaseOrderDetailService;
        IPurchaseReceivalService _purchaseReceivalService;
        IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        IUoMService _uomService;
        IBarringService _barringService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;
        IAccountService _accountService;
        IGeneralLedgerJournalService _generalLedgerJournalService;
        IClosingService _closingService;

        public Account Asset, CashBank, AccountReceivable, GBCHReceivable, Inventory;
        public Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;
        public Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
        public Account Equity, OwnersEquity, EquityAdjustment;
        public Account Revenue;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                _contactService = new ContactService(new ContactRepository(), new ContactValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
                _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
                _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
                _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
                _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _uomService = new UoMService(new UoMRepository(), new UoMValidator());
                _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
                _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
                _barringService = new BarringService(new BarringRepository(), new BarringValidator());

                _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
                _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
                _accountService = new AccountService(new AccountRepository(), new AccountValidator());
                _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
                _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());

                if (!_accountService.GetLegacyObjects().Any())
                {
                    Asset = _accountService.CreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = Constant.AccountGroup.Asset, IsLegacy = true });
                    CashBank = _accountService.CreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                    AccountReceivable = _accountService.CreateLegacyObject(new Account() { Name = "Account Receivable", IsLeaf = true, Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                    GBCHReceivable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Receivable", IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });
                    Inventory = _accountService.CreateLegacyObject(new Account() { Name = "Inventory", IsLeaf = true, Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true });

                    Expense = _accountService.CreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = Constant.AccountGroup.Expense, IsLegacy = true });
                    CashBankAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                    COGS = _accountService.CreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                    Discount = _accountService.CreateLegacyObject(new Account() { Name = "Discount", IsLeaf = true, Code = Constant.AccountCode.SalesDiscountExpense, LegacyCode = Constant.AccountLegacyCode.SalesDiscountExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                    SalesAllowance = _accountService.CreateLegacyObject(new Account() { Name = "Sales Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesAllowanceExpense, LegacyCode = Constant.AccountLegacyCode.SalesAllowanceExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });
                    StockAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true });

                    Liability = _accountService.CreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = Constant.AccountGroup.Liability, IsLegacy = true });
                    AccountPayable = _accountService.CreateLegacyObject(new Account() { Name = "Account Payable", IsLeaf = true, Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                    GBCHPayable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Payable", IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });
                    GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true });

                    Equity = _accountService.CreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = Constant.AccountGroup.Equity, IsLegacy = true });
                    OwnersEquity = _accountService.CreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true });
                    EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Name = "Equity Adjustment", IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 3, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true });

                    Revenue = _accountService.CreateLegacyObject(new Account() { Name = "Revenue", IsLeaf = true, Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = Constant.AccountGroup.Revenue, IsLegacy = true });
                }

                baseGroup = _contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

                Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                _uomService.CreateObject(Pcs);

                contact = new Contact()
                {
                    Name = "President of Indonesia",
                    Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                    ContactNo = "021 3863777",
                    PIC = "Mr. President",
                    PICContactNo = "021 3863777",
                    Email = "random@ri.gov.au"
                };
                contact = _contactService.CreateObject(contact, _contactGroupService);

                type = _itemTypeService.CreateObject("Item", "Item");

                warehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                warehouse = _warehouseService.CreateObject(warehouse, _warehouseItemService, _itemService);

                item_batiktulis = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Batik Tulis",
                    Category = "Item",
                    Sku = "bt123",
                    UoMId = Pcs.Id
                };

                item_batiktulis = _itemService.CreateObject(item_batiktulis, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_busway = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Busway",
                    Category = "Untuk disumbangkan bagi kebutuhan DKI Jakarta",
                    Sku = "DKI002",
                    UoMId = Pcs.Id
                };
                item_busway = _itemService.CreateObject(item_busway, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_botolaqua = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Botol Aqua",
                    Category = "Minuman",
                    Sku = "DKI003",
                    UoMId = Pcs.Id
                };
                item_botolaqua = _itemService.CreateObject(item_botolaqua, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                StockAdjustment sa = new StockAdjustment() { AdjustmentDate = DateTime.Today, WarehouseId = warehouse.Id, Description = "item adjustment" };
                _stockAdjustmentService.CreateObject(sa, _warehouseService);
                StockAdjustmentDetail sadBatikTulis = new StockAdjustmentDetail()
                {
                    ItemId = item_batiktulis.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBatikTulis, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBusWay = new StockAdjustmentDetail()
                {
                    ItemId = item_busway.Id,
                    Quantity = 200,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBusWay, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBotolAqua = new StockAdjustmentDetail()
                {
                    ItemId = item_botolaqua.Id,
                    Quantity = 20000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBotolAqua, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _barringService, _warehouseItemService, _generalLedgerJournalService, _accountService, _closingService);


                purchaseOrder1 = _purchaseOrderService.CreateObject(contact.Id, new DateTime(2014, 07, 09), _contactService);
                purchaseOrder2 = _purchaseOrderService.CreateObject(contact.Id, new DateTime(2014, 04, 09), _contactService);
                purchaseOrderDetail_batiktulis_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_batiktulis.Id, 500, 2000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_busway_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_busway.Id, 91, 800000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_botolaqua_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_botolaqua.Id, 2000, 5000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_batiktulis_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_batiktulis.Id, 40, 2000500, _purchaseOrderService, _itemService);
                purchaseOrderDetail_busway_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_busway.Id, 3, 810000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_botolaqua_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_botolaqua.Id, 340, 5500, _purchaseOrderService, _itemService);
                purchaseOrder1 = _purchaseOrderService.ConfirmObject(purchaseOrder1, DateTime.Today, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                purchaseOrder2 = _purchaseOrderService.ConfirmObject(purchaseOrder2, DateTime.Today, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            }
        }

        void purchasereceival_validation()
        {
            it["validates_all_variables"] = () =>
            {
                contact.Errors.Count().should_be(0);
                item_batiktulis.Errors.Count().should_be(0);
                item_busway.Errors.Count().should_be(0);
                item_botolaqua.Errors.Count().should_be(0);
                purchaseOrder1.Errors.Count().should_be(0);
                purchaseOrder2.Errors.Count().should_be(0);
            };

            it["validates the item pending receival"] = () =>
            {
                item_batiktulis.PendingReceival.should_be(purchaseOrderDetail_batiktulis_so1.Quantity + purchaseOrderDetail_batiktulis_so2.Quantity);
                item_busway.PendingReceival.should_be(purchaseOrderDetail_busway_so1.Quantity + purchaseOrderDetail_busway_so2.Quantity);
                item_botolaqua.PendingReceival.should_be(purchaseOrderDetail_botolaqua_so1.Quantity + purchaseOrderDetail_botolaqua_so2.Quantity);
            };

            context["when confirming purchase receival"] = () =>
            {
                before = () =>
                {
                    purchaseReceival1 = _purchaseReceivalService.CreateObject(warehouse.Id, purchaseOrder1.Id, new DateTime(2000, 1, 1), _purchaseOrderService, _warehouseService);
                    purchaseReceival2 = _purchaseReceivalService.CreateObject(warehouse.Id, purchaseOrder2.Id, new DateTime(2014, 5, 5), _purchaseOrderService, _warehouseService);
                    purchaseReceival3 = _purchaseReceivalService.CreateObject(warehouse.Id, purchaseOrder1.Id, new DateTime(2014, 5, 5), _purchaseOrderService, _warehouseService);
                    purchaseReceivalDetail_batiktulis_do1 = _purchaseReceivalDetailService.CreateObject(purchaseReceival1.Id, item_batiktulis.Id, 400, purchaseOrderDetail_batiktulis_so1.Id, _purchaseReceivalService,
                                                                                                  _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_busway_do1 = _purchaseReceivalDetailService.CreateObject(purchaseReceival1.Id, item_busway.Id, 91, purchaseOrderDetail_busway_so1.Id, _purchaseReceivalService,
                                                                                                _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_botolaqua_do1 = _purchaseReceivalDetailService.CreateObject(purchaseReceival1.Id, item_botolaqua.Id, 2000, purchaseOrderDetail_botolaqua_so1.Id,  _purchaseReceivalService,
                                                                                                  _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_batiktulis_do2b = _purchaseReceivalDetailService.CreateObject(purchaseReceival2.Id, item_batiktulis.Id, 40, purchaseOrderDetail_batiktulis_so2.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_busway_do2 = _purchaseReceivalDetailService.CreateObject(purchaseReceival2.Id, item_busway.Id, 3, purchaseOrderDetail_busway_so2.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_botolaqua_do2 = _purchaseReceivalDetailService.CreateObject(purchaseReceival2.Id, item_botolaqua.Id, 340, purchaseOrderDetail_botolaqua_so2.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceivalDetail_batiktulis_do2a = _purchaseReceivalDetailService.CreateObject(purchaseReceival3.Id, item_batiktulis.Id, 100, purchaseOrderDetail_batiktulis_so1.Id, _purchaseReceivalService,
                                                                                                                          _purchaseOrderDetailService, _purchaseOrderService, _itemService);
                    purchaseReceival1 = _purchaseReceivalService.ConfirmObject(purchaseReceival1, DateTime.Today, _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                                               _itemService, _barringService, _warehouseItemService);
                    purchaseReceival2 = _purchaseReceivalService.ConfirmObject(purchaseReceival2, DateTime.Today, _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                                               _itemService, _barringService, _warehouseItemService);
                    purchaseReceival3 = _purchaseReceivalService.ConfirmObject(purchaseReceival3, DateTime.Today, _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                                               _itemService, _barringService, _warehouseItemService);
                };

                it["validates_purchasereceivals"] = () =>
                {
                    purchaseReceival1.Errors.Count().should_be(0);
                    purchaseReceival2.Errors.Count().should_be(0);
                };

                it["deletes confirmed purchase receival"] = () =>
                {
                    purchaseReceival1 = _purchaseReceivalService.SoftDeleteObject(purchaseReceival1, _purchaseReceivalDetailService);
                    purchaseReceival1.Errors.Count().should_not_be(0);
                };

                it["unconfirm purchase receival"] = () =>
                {
                    purchaseReceival1 = _purchaseReceivalService.UnconfirmObject(purchaseReceival1, _purchaseReceivalDetailService, _purchaseInvoiceService, _purchaseInvoiceDetailService,
                                                                                 _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    purchaseReceival1.Errors.Count().should_be(0);
                };

                it["validates item pending receival"] = () =>
                {
                    item_batiktulis.PendingReceival.should_be(0);
                    item_busway.PendingReceival.should_be(0);
                    item_botolaqua.PendingReceival.should_be(0);
                };
            };
        }
    }
}