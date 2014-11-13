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

namespace NSpec
{

    public class SpecPurchaseOrder : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
        PurchaseOrderDetail poDetail1;
        PurchaseOrderDetail poDetail2;
        PurchaseOrder newPO;
        Item item1;
        Item item2;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
        IItemService itemService;
        IContactService contactService;
        IPurchaseOrderService poService;
        IPurchaseOrderDetailService poDetailService;
        IStockMutationService _stockMutationService;
        IUoMService _uomService;
        IBarringService _barringService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        IStockAdjustmentService _stockAdjustmentService;
        IItemService _itemService;
        IAccountService _accountService;
        IClosingService _closingService;
        IGeneralLedgerJournalService _generalLedgerJournalService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;

        public Account Asset, CashBank, AccountReceivable, GBCHReceivable, Inventory;
        public Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;
        public Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
        public Account Equity, OwnersEquity, EquityAdjustment;
        public Account Revenue;

        int Quantity1;
        int Quantity2;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                itemService = new ItemService(new ItemRepository(), new ItemValidator());
                contactService = new ContactService(new ContactRepository(), new ContactValidator());
                poService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
                poDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _uomService = new UoMService(new UoMRepository(), new UoMValidator());
                _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
                _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
                _barringService = new BarringService(new BarringRepository(), new BarringValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _accountService = new AccountService(new AccountRepository(), new AccountValidator());
                _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
                _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
                _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
                _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());

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
                contact = contactService.CreateObject(contact, _contactGroupService);

                type = _itemTypeService.CreateObject("Item", "Item");

                warehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                warehouse = _warehouseService.CreateObject(warehouse, _warehouseItemService, itemService);

                item1 = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Batik Tulis",
                    Category = "Item",
                    Sku = "bt123",
                    UoMId = Pcs.Id
                };
                itemService.CreateObject(item1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item2 = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Buku Gambar",
                    Category = "Item",
                    Sku = "bg123",
                    UoMId = Pcs.Id
                };
                itemService.CreateObject(item2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                StockAdjustment sa = new StockAdjustment() { AdjustmentDate = DateTime.Today, WarehouseId = warehouse.Id, Description = "item adjustment" };
                _stockAdjustmentService.CreateObject(sa, _warehouseService);
                StockAdjustmentDetail sadItem1 = new StockAdjustmentDetail()
                {
                    ItemId = item1.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadItem1, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadItem2 = new StockAdjustmentDetail()
                {
                    ItemId = item2.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadItem2, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _barringService, _warehouseItemService, _generalLedgerJournalService,
                                                      _accountService, _closingService);

            }
        }

        void po_validation()
        {
            it["prepares_valid_data"] = () =>
                {
                    item1.Errors.Count().should_be(0);
                    item2.Errors.Count().should_be(0);
                    contact.Errors.Count().should_be(0);
                };

            it["creates_po"] = () =>
                {
                    newPO = new PurchaseOrder
                    {
                        ContactId = contact.Id,
                        PurchaseDate = DateTime.Now
                    };
                    newPO = poService.CreateObject(newPO, contactService);
                    newPO.Errors.Count().should_be(0);
                    poService.GetObjectById(newPO.Id).should_not_be_null();
                };

            context["when creating PO Detail"] = () =>
                {
                    before = () =>
                    {
                        newPO = new PurchaseOrder
                        {
                            ContactId = contact.Id,
                            PurchaseDate = DateTime.Now
                        };
                        newPO = poService.CreateObject(newPO, contactService);
                    };

                    it["creates po detail"] = () =>
                    {
                        PurchaseOrderDetail poDetail = new PurchaseOrderDetail
                        {
                            PurchaseOrderId = newPO.Id,
                            ItemId = item1.Id,
                            Quantity = 5,
                            Price = 30000
                        };
                        poDetail = poDetailService.CreateObject(poDetail, poService, itemService);
                        poDetail.Errors.Count().should_be(0);
                    };

                    it["must not allow PO confirmation if there is no PO Detail"] = () =>
                    {
                        newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, _stockMutationService, itemService, _barringService, _warehouseItemService);
                        newPO.Errors.Count().should_not_be(0);
                    };

                    it["should not create po detail if there is no item id"] = () =>
                    {
                        PurchaseOrderDetail poDetail = new PurchaseOrderDetail
                        {
                            PurchaseOrderId = newPO.Id,
                            ItemId = 0,
                            Quantity = 5,
                            Price = 25000
                        };
                        poDetail = poDetailService.CreateObject(poDetail, poService, itemService);
                        poDetail.Errors.Count().should_not_be(0);
                    };

                    context["when creating PO Detail"] = () =>
                    {
                        before = () =>
                        {
                            poDetail1 = new PurchaseOrderDetail
                            {
                                PurchaseOrderId = newPO.Id,
                                ItemId = item1.Id,
                                Quantity = 5,
                                Price = 30000
                            };
                            poDetail1 = poDetailService.CreateObject(poDetail1, poService, itemService);
                        };

                        it["should be valid when creating PO Detail 1"] = () =>
                        {
                            poDetail1.Errors.Count.should_be(0);
                        };

                        it["should not be valid when creating PO Detail 2 with the same item"] = () =>
                        {
                            poDetail2 = new PurchaseOrderDetail
                            {
                                PurchaseOrderId = newPO.Id,
                                ItemId = item1.Id,
                                Quantity = 5,
                                Price = 2500000
                            };
                            poDetail2 = poDetailService.CreateObject(poDetail2, poService, itemService);
                            poDetail2.Errors.Count().should_not_be(0);
                        };

                        context["should not be valid when creating PO Detail 2 with the same item"] = () =>
                        {
                            before = () =>
                            {
                                poDetail2 = new PurchaseOrderDetail
                                {
                                    PurchaseOrderId = newPO.Id,
                                    ItemId = item2.Id,
                                    Quantity = 5,
                                    Price = 2500000
                                };
                                poDetail2 = poDetailService.CreateObject(poDetail2, poService, itemService);
                            };

                            it["should be valid when creating a new PO Detail 2"] = () =>
                            {
                                poDetail2.Errors.Count().should_be(0);
                            };

                            it["should not be valid when updating PO Detail 2 item to the same item as POD1"] = () =>
                            {
                                poDetail2.ItemId = item1.Id;
                                poDetailService.UpdateObject(poDetail2, poService, itemService);
                                poDetail2.Errors.Count().should_not_be(0);
                            };
                        };
                    };
                };

            context["when confirming PO"] = () =>
            {
                before = () =>
                {
                    newPO = new PurchaseOrder
                    {
                        ContactId = contact.Id,
                        PurchaseDate = DateTime.Now
                    };
                    newPO = poService.CreateObject(newPO, contactService);

                    poDetail1 = new PurchaseOrderDetail
                    {
                        PurchaseOrderId = newPO.Id,
                        ItemId = item1.Id,
                        Quantity = 5,
                        Price = 30000
                    };
                    poDetail1 = poDetailService.CreateObject(poDetail1, poService, itemService);

                    poDetail2 = new PurchaseOrderDetail
                    {
                        PurchaseOrderId = newPO.Id,
                        ItemId = item2.Id,
                        Quantity = 5,
                        Price = 25000
                    };
                    poDetail2 = poDetailService.CreateObject(poDetail2, poService, itemService);
                };

                it["allows confirmation"] = () =>
                {
                    newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, _stockMutationService, itemService, _barringService, _warehouseItemService);
                    newPO.IsConfirmed.should_be(true);
                };

                context["post PO confirm"] = () =>
                {
                    before = () =>
                    {
                        item1 = itemService.GetObjectById(item1.Id);
                        item2 = itemService.GetObjectById(item2.Id);
                        Quantity1 = item1.PendingReceival;
                        Quantity2 = item2.PendingReceival;

                        newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, _stockMutationService, itemService, _barringService, _warehouseItemService);
                    };

                    it["should increase pending receival in item"] = () =>
                    {
                        int diff_1 = item1.PendingReceival - Quantity1;
                        diff_1.should_be(poDetail1.Quantity);

                        int diff_2 = item2.PendingReceival - Quantity2;
                        diff_2.should_be(poDetail2.Quantity);
                    };

                    it["should create StockMutation"] = () =>
                    {
                        _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item1.Id, Core.Constants.Constant.SourceDocumentDetailType.PurchaseOrderDetail, poDetail1.Id).Count().should_be(1);
                        _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item2.Id, Core.Constants.Constant.SourceDocumentDetailType.PurchaseOrderDetail, poDetail2.Id).Count().should_be(1);

                    };
                };
            };
        }
    }
}