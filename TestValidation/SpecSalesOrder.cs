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

    public class SpecSalesOrder : nspec
    {
        ContactGroup baseGroup;
        Contact contact;
        Item item_sepatubola;
        Item item_batiktulis;
        SalesOrder salesOrder;
        SalesOrderDetail salesOrderDetail1;
        SalesOrderDetail salesOrderDetail2;
        UoM Pcs;
        ItemType type;
        Warehouse warehouse;
        IContactService _contactService;
        IItemService _itemService;
        ISalesOrderService _salesOrderService;
        ISalesOrderDetailService _salesOrderDetailService;
        IDeliveryOrderService _deliveryOrderService;
        IDeliveryOrderDetailService _deliveryOrderDetailService;
        IStockMutationService _stockMutationService;
        IUoMService _uomService;
        IBarringService _barringService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;
        IStockAdjustmentService _stockAdjustmentService;
        IStockAdjustmentDetailService _stockAdjustmentDetailService;
        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;
        IGeneralLedgerJournalService _generalLedgerJournalService;
        IClosingService _closingService;
        IAccountService _accountService;

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
                _contactService = new ContactService(new ContactRepository(), new ContactValidator());
                _itemService = new ItemService(new ItemRepository(), new ItemValidator());
                _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
                _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
                _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
                _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
                _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
                _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
                _uomService = new UoMService(new UoMRepository(), new UoMValidator());
                _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
                _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
                _barringService = new BarringService(new BarringRepository(), new BarringValidator());
                _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
                _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());

                _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
                _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
                _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
                _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
                _accountService = new AccountService(new AccountRepository(), new AccountValidator());

                if (!_accountService.GetLegacyObjects().Any())
                {
                    Asset = _accountService.CreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
                    CashBank = _accountService.CreateLegacyObject(new Account() { Name = "CashBank", IsLeaf = true, Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                    AccountReceivable = _accountService.CreateLegacyObject(new Account() { Name = "Account Receivable", IsLeaf = true, Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                    GBCHReceivable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Receivable", IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
                    Inventory = _accountService.CreateLegacyObject(new Account() { Name = "Inventory", IsLeaf = true, Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

                    Expense = _accountService.CreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
                    CashBankAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    COGS = _accountService.CreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    Discount = _accountService.CreateLegacyObject(new Account() { Name = "Discount", IsLeaf = true, Code = Constant.AccountCode.Discount, LegacyCode = Constant.AccountLegacyCode.Discount, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    SalesAllowance = _accountService.CreateLegacyObject(new Account() { Name = "Sales Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesAllowance, LegacyCode = Constant.AccountLegacyCode.SalesAllowance, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
                    StockAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);

                    Liability = _accountService.CreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
                    AccountPayable = _accountService.CreateLegacyObject(new Account() { Name = "Account Payable", IsLeaf = true, Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                    GBCHPayable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Payable", IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
                    GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);

                    Equity = _accountService.CreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = Constant.AccountGroup.Equity, IsLegacy = true }, _accountService);
                    OwnersEquity = _accountService.CreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true }, _accountService);
                    EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Name = "Equity Adjustment", IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 3, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);

                    Revenue = _accountService.CreateLegacyObject(new Account() { Name = "Revenue", IsLeaf = true, Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
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
                _itemService.CreateObject(item_batiktulis, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                item_sepatubola = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Sepatu Bola",
                    Category = "Item",
                    Sku = "sb123",
                    UoMId = Pcs.Id
                };
                _itemService.CreateObject(item_sepatubola, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

                StockAdjustment sa = new StockAdjustment() { AdjustmentDate = DateTime.Today, WarehouseId = warehouse.Id, Description = "item adjustment" };
                _stockAdjustmentService.CreateObject(sa, _warehouseService);
                StockAdjustmentDetail sadSepatuBola = new StockAdjustmentDetail()
                {
                    ItemId = item_sepatubola.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadSepatuBola, _stockAdjustmentService, _itemService, _warehouseItemService);

                StockAdjustmentDetail sadBatikTulis = new StockAdjustmentDetail()
                {
                    ItemId = item_batiktulis.Id,
                    Quantity = 1000,
                    StockAdjustmentId = sa.Id
                };
                _stockAdjustmentDetailService.CreateObject(sadBatikTulis, _stockAdjustmentService, _itemService, _warehouseItemService);

                _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                      _itemService, _barringService, _warehouseItemService, _generalLedgerJournalService,
                                                      _accountService, _closingService);

            }
        }

        void salesorder_validation()
        {
            it["validate_contact_and_items"] = () =>
            {
                contact.Errors.Count().should_be(0);
                item_batiktulis.Errors.Count().should_be(0);
                item_sepatubola.Errors.Count().should_be(0);
            };

            it["create_salesorder"] = () =>
            {
                salesOrder = _salesOrderService.CreateObject(contact.Id, new DateTime(2000, 1, 1), _contactService);
                salesOrder.Errors.Count().should_be(0);
            };

            it["create_salesorder_with_no_contactid"] = () =>
            {
                salesOrder = new SalesOrder()
                {
                    SalesDate = DateTime.Now
                };
                _salesOrderService.CreateObject(salesOrder, _contactService);
                salesOrder.Errors.Count().should_not_be(0);
            };

            it["create_salesorder_with_no_elements"] = () =>
            {
                salesOrder = new SalesOrder();
                _salesOrderService.CreateObject(salesOrder, _contactService);
                salesOrder.Errors.Count().should_not_be(0);
            };

            context["validating_salesorder"] = () =>
            {
                before = () =>
                {
                    salesOrder = new SalesOrder
                    {
                        ContactId = contact.Id,
                        SalesDate = DateTime.Now
                    };
                    salesOrder = _salesOrderService.CreateObject(salesOrder, _contactService);
                };

                it["delete_salesorder"] = () =>
                {
                    salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                    salesOrder.Errors.Count().should_be(0);
                };

                it["delete_salesorder_and_details"] = () =>
                {
                    salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_sepatubola.Id, 12, 850000, _salesOrderService, _itemService);
                    salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                    salesOrder.Errors.Count().should_not_be(0);
                };

                it["delete_salesorderdetail"] = () =>
                {
                    salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_sepatubola.Id, 12, 850000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.SoftDeleteObject(salesOrderDetail2);
                    salesOrderDetail2.Errors.Count().should_be(0);
                };

                it["create_salesorderdetails_with_same_item"] = () =>
                {
                    salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                    salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 12, 850000, _salesOrderService, _itemService);
                    salesOrderDetail2.Errors.Count().should_not_be(0);
                };

                context["when confirming SO"] = () =>
                {
                    before = () =>
                    {
                        salesOrderDetail1 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_batiktulis.Id, 5, 100000, _salesOrderService, _itemService);
                        salesOrderDetail2 = _salesOrderDetailService.CreateObject(salesOrder.Id, item_sepatubola.Id, 12, 850000, _salesOrderService, _itemService);
                        Quantity1 = item_batiktulis.PendingDelivery;
                        Quantity2 = item_sepatubola.PendingDelivery;
                        salesOrder = _salesOrderService.ConfirmObject(salesOrder, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    };

                    it["confirmed_salesorder"] = () =>
                    {
                        salesOrder.Errors.Count().should_be(0);
                    };

                    it["check_detailsconfirmation"] = () =>
                    {
                        IList<StockMutation> stockMutationsDetail1 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_batiktulis.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail1.Id);
                        stockMutationsDetail1.Count().should_be(1);
                        IList<StockMutation> stockMutationsDetail2 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_sepatubola.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail2.Id);
                        stockMutationsDetail2.Count().should_be(1);
                        salesOrderDetail1.IsConfirmed.should_be(true);
                        salesOrderDetail2.IsConfirmed.should_be(true);
                    };

                    it["delete_confirmed_salesorder"] = () =>
                    {
                        salesOrder = _salesOrderService.SoftDeleteObject(salesOrder, _salesOrderDetailService);
                        salesOrder.Errors.Count().should_not_be(0);
                    };

                    it["unfinish_salesorderdetail"] = () =>
                    {
                        _salesOrderService.UnconfirmObject(salesOrder, _salesOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _stockMutationService,
                                                           _itemService, _barringService, _warehouseItemService);
                        salesOrderDetail1.IsConfirmed.should_be(false);
                        salesOrderDetail2.IsConfirmed.should_be(false);
                        salesOrderDetail1.Errors.Count().should_be(0);
                        salesOrderDetail2.Errors.Count().should_be(0);
                        IList<StockMutation> stockMutationsDetail1 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_batiktulis.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail1.Id);
                        stockMutationsDetail1.Count().should_be(0);
                        IList<StockMutation> stockMutationsDetail2 = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(item_sepatubola.Id, Core.Constants.Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail2.Id);
                        stockMutationsDetail2.Count().should_be(0);
                    };

                    it["delete_unfinish_salesorderdetail"] = () =>
                    {
                        _salesOrderService.UnconfirmObject(salesOrder, _salesOrderDetailService, _deliveryOrderService, _deliveryOrderDetailService, _stockMutationService,
                                                           _itemService, _barringService, _warehouseItemService);
                        salesOrderDetail2 = _salesOrderDetailService.SoftDeleteObject(salesOrderDetail2);
                        salesOrderDetail2.Errors.Count().should_be(0);
                        salesOrderDetail2.IsDeleted.should_be(true);
                    };

                    it["should increase pending delivery in item"] = () =>
                    {
                        Item NewItem1 = _itemService.GetObjectById(item_batiktulis.Id);
                        Item NewItem2 = _itemService.GetObjectById(item_sepatubola.Id);

                        int diff_1 = NewItem1.PendingDelivery - Quantity1;
                        diff_1.should_be(salesOrderDetail1.Quantity);

                        int diff_2 = NewItem2.PendingDelivery - Quantity2;
                        diff_2.should_be(salesOrderDetail2.Quantity);
                    };
                };
            };
        }
    }
}