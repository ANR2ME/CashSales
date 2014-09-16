using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;

namespace TestValidation
{
    public class SalesBuilder
    {
        public IAccountService _accountService;
        public IBarringService _barringService;
        public IBarringOrderService _barringOrderService;
        public IBarringOrderDetailService _barringOrderDetailService;
        public ICashBankService _cashBankService;
        public ICashBankAdjustmentService _cashBankAdjustmentService;
        public ICashBankMutationService _cashBankMutationService;
        public ICashMutationService _cashMutationService;
        public IClosingService _closingService;
        public ICoreBuilderService _coreBuilderService;
        public ICoreIdentificationService _coreIdentificationService;
        public ICoreIdentificationDetailService _coreIdentificationDetailService;
        public IContactService _contactService;
        public IGeneralLedgerJournalService _generalLedgerJournalService;
        public IItemService _itemService;
        public IItemTypeService _itemTypeService;
        public IMachineService _machineService;
        public IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        public IRecoveryOrderDetailService _recoveryOrderDetailService;
        public IRecoveryOrderService _recoveryOrderService;
        public IRollerBuilderService _rollerBuilderService;
        public IRollerTypeService _rollerTypeService;
        public IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService;
        public IRollerWarehouseMutationService _rollerWarehouseMutationService;
        public IStockAdjustmentDetailService _stockAdjustmentDetailService;
        public IStockAdjustmentService _stockAdjustmentService;
        public IStockMutationService _stockMutationService;
        public IUoMService _uomService;
        public IWarehouseItemService _warehouseItemService;
        public IWarehouseService _warehouseService;
        public IWarehouseMutationOrderService _warehouseMutationOrderService;
        public IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService;
        public IPayableService _payableService;
        public IPaymentVoucherDetailService _paymentVoucherDetailService;
        public IPaymentVoucherService _paymentVoucherService;
        public IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        public IPurchaseInvoiceService _purchaseInvoiceService;
        public IPurchaseOrderService _purchaseOrderService;
        public IPurchaseOrderDetailService _purchaseOrderDetailService;
        public IPurchaseReceivalService _purchaseReceivalService;
        public IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        public IReceivableService _receivableService;
        public ISalesInvoiceDetailService _salesInvoiceDetailService;
        public ISalesInvoiceService _salesInvoiceService;
        public IReceiptVoucherDetailService _receiptVoucherDetailService;
        public IReceiptVoucherService _receiptVoucherService;
        public ISalesOrderService _salesOrderService;
        public ISalesOrderDetailService _salesOrderDetailService;
        public IDeliveryOrderService _deliveryOrderService;
        public IDeliveryOrderDetailService _deliveryOrderDetailService;

        public IPriceMutationService _priceMutationService;
        public IContactGroupService _contactGroupService;

        public ContactGroup baseGroup;
        public ItemType typeAccessory, typeBar, typeBarring, typeBearing, typeBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public UoM Pcs, Boxes, Tubs;

        public Warehouse localWarehouse;
        public Contact contact;
        public Item blanket1, blanket2, blanket3;
        public StockAdjustment stockAdjustment;
        public StockAdjustmentDetail stockAD1, stockAD2;
        public CashBank cashBank, pettyCash;
        public CashBankAdjustment cashBankAdjustment;
        public SalesOrder so1, so2;
        public SalesOrderDetail so1a, so1b, so1c, so2a, so2b;
        public DeliveryOrder do1, do2, do3;
        public DeliveryOrderDetail do1a, do1b, do2a, do2b, do1a2, do1c;
        public SalesInvoice si1, si2, si3;
        public SalesInvoiceDetail si1a, si1b, si2a, si2b, si1a2, si1c;
        public ReceiptVoucher rv;
        public ReceiptVoucherDetail rvd1, rvd2, rvd3;

        public Account Asset, CashBank, AccountReceivable, GBCHReceivable, Inventory;
        public Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;
        public Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
        public Account Equity, OwnersEquity, EquityAdjustment;
        public Account Revenue;

        public SalesBuilder()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _barringOrderService = new BarringOrderService(new BarringOrderRepository(), new BarringOrderValidator());
            _barringOrderDetailService = new BarringOrderDetailService(new BarringOrderDetailRepository(), new BarringOrderDetailValidator());
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _rollerWarehouseMutationDetailService = new RollerWarehouseMutationDetailService(new RollerWarehouseMutationDetailRepository(), new RollerWarehouseMutationDetailValidator());
            _rollerWarehouseMutationService = new RollerWarehouseMutationService(new RollerWarehouseMutationRepository(), new RollerWarehouseMutationValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _stockAdjustmentService = new StockAdjustmentService(new StockAdjustmentRepository(), new StockAdjustmentValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _warehouseMutationOrderService = new WarehouseMutationOrderService(new WarehouseMutationOrderRepository(), new WarehouseMutationOrderValidator());
            _warehouseMutationOrderDetailService = new WarehouseMutationOrderDetailService(new WarehouseMutationOrderDetailRepository(), new WarehouseMutationOrderDetailValidator());

            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());

            typeAccessory = _itemTypeService.CreateObject("Accessory", "Accessory");
            typeBar = _itemTypeService.CreateObject("Bar", "Bar");
            typeBarring = _itemTypeService.CreateObject("Barring", "Barring", true);
            typeBearing = _itemTypeService.CreateObject("Bearing", "Bearing");
            typeBlanket = _itemTypeService.CreateObject("Blanket", "Blanket");
            typeChemical = _itemTypeService.CreateObject("Chemical", "Chemical");
            typeCompound = _itemTypeService.CreateObject("Compound", "Compound");
            typeConsumable = _itemTypeService.CreateObject("Consumable", "Consumable");
            typeCore = _itemTypeService.CreateObject("Core", "Core", true);
            typeGlue = _itemTypeService.CreateObject("Glue", "Glue");
            typeUnderpacking = _itemTypeService.CreateObject("Underpacking", "Underpacking");
            typeRoller = _itemTypeService.CreateObject("Roller", "Roller", true);

            typeDamp = _rollerTypeService.CreateObject("Damp", "Damp");
            typeFoundDT = _rollerTypeService.CreateObject("Found DT", "Found DT");
            typeInkFormX = _rollerTypeService.CreateObject("Ink Form X", "Ink Form X");
            typeInkDistD = _rollerTypeService.CreateObject("Ink Dist D", "Ink Dist D");
            typeInkDistM = _rollerTypeService.CreateObject("Ink Dist M", "Ink Dist M");
            typeInkDistE = _rollerTypeService.CreateObject("Ink Dist E", "Ink Dist E");
            typeInkDuctB = _rollerTypeService.CreateObject("Ink Duct B", "Ink Duct B");
            typeInkDistH = _rollerTypeService.CreateObject("Ink Dist H", "Ink Dist H");
            typeInkFormW = _rollerTypeService.CreateObject("Ink Form W", "Ink Form W");
            typeInkDistHQ = _rollerTypeService.CreateObject("Ink Dist HQ", "Ink Dist HQ");
            typeDampFormDQ = _rollerTypeService.CreateObject("Damp Form DQ", "Damp Form DQ");
            typeInkFormY = _rollerTypeService.CreateObject("Ink Form Y", "Ink Form Y");

            baseGroup = _contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

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
        }

        public void PopulateData()
        {
            PopulateMasterData();
            PopulateOrderAndReceivalData();
            PopulateInvoiceData();
            PopulateVoucher();
        }

        public void PopulateMasterData()
        {
            localWarehouse = new Warehouse()
            {
                Name = "Sentral Solusi Data",
                Description = "Kali Besar Jakarta",
                Code = "LCL"
            };
            localWarehouse = _warehouseService.CreateObject(localWarehouse, _warehouseItemService, _itemService);

            Pcs = new UoM()
            {
                Name = "Pcs"
            };
            _uomService.CreateObject(Pcs);

            Boxes = new UoM()
            {
                Name = "Boxes"
            };
            _uomService.CreateObject(Boxes);

            Tubs = new UoM()
            {
                Name = "Tubs"
            };
            _uomService.CreateObject(Tubs);

            blanket1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Blanket").Id,
                Name = "Blanket1",
                Category = "Blanket",
                Sku = "BLK1",
                UoMId = Pcs.Id
            };

            blanket1 = _itemService.CreateObject(blanket1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
            _itemService.AdjustQuantity(blanket1, 100000);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, blanket1.Id), 100000);

            blanket2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Blanket").Id,
                Name = "Blanket2",
                Category = "Blanket",
                Sku = "BLK2",
                UoMId = Pcs.Id
            };

            blanket2 = _itemService.CreateObject(blanket2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
            _itemService.AdjustQuantity(blanket2, 100000);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, blanket2.Id), 100000);

            blanket3 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Blanket").Id,
                Name = "Blanket3",
                Category = "Blanket",
                Sku = "BLK3",
                UoMId = Pcs.Id
            };

            blanket3 = _itemService.CreateObject(blanket3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
            _itemService.AdjustQuantity(blanket3, 100000);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, blanket3.Id), 100000);

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

            cashBank = new CashBank()
            {
                Name = "Rekening BRI",
                Description = "Untuk cashflow",
                IsBank = true
            };
            _cashBankService.CreateObject(cashBank, _accountService);

            cashBankAdjustment = new CashBankAdjustment()
            {
                CashBankId = cashBank.Id,
                Amount = 1000000000,
                AdjustmentDate = DateTime.Today
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment, _cashBankService);
            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment, DateTime.Now, _cashMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);
        }

        public void PopulateOrderAndReceivalData()
        {
            TimeSpan purchaseDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan receivedDate = new TimeSpan(3, 0, 0 ,0);
            TimeSpan lateDeliveryDate = new TimeSpan(2, 0, 0, 0);
            so1 = new SalesOrder()
            {
                SalesDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id
            };
            _salesOrderService.CreateObject(so1, _contactService);

            so2 = new SalesOrder()
            {
                SalesDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id
            };
            _salesOrderService.CreateObject(so2, _contactService);

            so1a = new SalesOrderDetail()
            {
                ItemId = blanket1.Id,
                SalesOrderId = so1.Id,
                Quantity = 300,
                Price = 50000
            };
            _salesOrderDetailService.CreateObject(so1a, _salesOrderService, _itemService);

            so1b = new SalesOrderDetail()
            {
                ItemId = blanket2.Id,
                SalesOrderId = so1.Id,
                Quantity = 250,
                Price = 72000
            };
            _salesOrderDetailService.CreateObject(so1b, _salesOrderService, _itemService);

            so1c = new SalesOrderDetail()
            {
                ItemId = blanket3.Id,
                SalesOrderId = so1.Id,
                Quantity = 100,
                Price = 100000
            };
            _salesOrderDetailService.CreateObject(so1c, _salesOrderService, _itemService);

            so2a = new SalesOrderDetail()
            {
                ItemId = blanket1.Id,
                SalesOrderId = so2.Id,
                Quantity = 300,
                Price = 50000
            };
            _salesOrderDetailService.CreateObject(so2a, _salesOrderService, _itemService);

            so2b = new SalesOrderDetail()
            {
                ItemId = blanket2.Id,
                SalesOrderId = so2.Id,
                Quantity = 250,
                Price = 72000
            };
            _salesOrderDetailService.CreateObject(so2b, _salesOrderService, _itemService);

            _salesOrderService.ConfirmObject(so1, so1.SalesDate, _salesOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            _salesOrderService.ConfirmObject(so2, so2.SalesDate, _salesOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);

            do1 = new DeliveryOrder()
            {
                SalesOrderId = so1.Id,
                DeliveryDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(do1, _salesOrderService, _warehouseService);

            do2 = new DeliveryOrder()
            {
                SalesOrderId = so2.Id,
                DeliveryDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(do2, _salesOrderService, _warehouseService);

            do1a = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so1a.Id,
                DeliveryOrderId = do1.Id,
                ItemId = so1a.ItemId,
                Quantity = so1a.Quantity - 100
            };
            _deliveryOrderDetailService.CreateObject(do1a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do1b = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so1b.Id,
                DeliveryOrderId = do1.Id,
                ItemId = so1b.ItemId,
                Quantity = so1b.Quantity
            };
            _deliveryOrderDetailService.CreateObject(do1b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do2a = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so2a.Id,
                DeliveryOrderId = do2.Id,
                ItemId = so2a.ItemId,
                Quantity = so2a.Quantity
            };
            _deliveryOrderDetailService.CreateObject(do2a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do2b = new DeliveryOrderDetail()
            {
                SalesOrderDetailId = so2b.Id,
                DeliveryOrderId = do2.Id,
                ItemId = so2b.ItemId,
                Quantity = so2b.Quantity
            };
            _deliveryOrderDetailService.CreateObject(do2b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do3 = new DeliveryOrder()
            {
                SalesOrderId = so1.Id,
                DeliveryDate = DateTime.Now.Subtract(lateDeliveryDate),
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(do3, _salesOrderService, _warehouseService);

            do1c = new DeliveryOrderDetail()
            {
                DeliveryOrderId = do3.Id,
                SalesOrderDetailId = so1c.Id,
                Quantity = so1c.Quantity,
                ItemId = so1c.ItemId
            };
            _deliveryOrderDetailService.CreateObject(do1c, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            do1a2 = new DeliveryOrderDetail()
            {
                DeliveryOrderId = do3.Id,
                SalesOrderDetailId = so1a.Id,
                Quantity = 100,
                ItemId = so1a.ItemId
            };
            _deliveryOrderDetailService.CreateObject(do1a2, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

        }

        public void PopulateInvoiceData()
        {
            TimeSpan receivedDate = new TimeSpan(3, 0, 0, 0);
            TimeSpan lateDeliveryDate = new TimeSpan(2, 0, 0, 0);
            _deliveryOrderService.ConfirmObject(do1, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                       _itemService, _barringService, _warehouseItemService);
            _deliveryOrderService.ConfirmObject(do2, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                   _itemService, _barringService, _warehouseItemService);
            _deliveryOrderService.ConfirmObject(do3, DateTime.Now.Subtract(receivedDate), _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService,
                                                   _stockMutationService, _itemService, _barringService, _warehouseItemService);

            si1 = new SalesInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Penjualan DO1",
                DeliveryOrderId = do1.Id,
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
            };
            si1 = _salesInvoiceService.CreateObject(si1, _deliveryOrderService);

            si1a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si1.Id,
                DeliveryOrderDetailId = do1a.Id,
                Quantity = do1a.Quantity
            };
            si1a = _salesInvoiceDetailService.CreateObject(si1a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si1b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si1.Id,
                DeliveryOrderDetailId = do1b.Id,
                Quantity = do1b.Quantity
            };
            si1b = _salesInvoiceDetailService.CreateObject(si1b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si2 = new SalesInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Penjualan DO2",
                DeliveryOrderId = do2.Id,
                IsTaxable = true,
                Discount = 5,
                DueDate = DateTime.Today.AddDays(14)
            };
            si2 = _salesInvoiceService.CreateObject(si2, _deliveryOrderService);

            si2a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si2.Id,
                DeliveryOrderDetailId = do2a.Id,
                Quantity = do2a.Quantity
            };
            si2a = _salesInvoiceDetailService.CreateObject(si2a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si2b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si2.Id,
                DeliveryOrderDetailId = do2b.Id,
                Quantity = do2b.Quantity
            };
            si2b = _salesInvoiceDetailService.CreateObject(si2b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si3 = new SalesInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Penjualan DO3",
                DeliveryOrderId = do3.Id,
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
            };
            si3 = _salesInvoiceService.CreateObject(si3, _deliveryOrderService);

            si1a2 = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si3.Id,
                DeliveryOrderDetailId = do1a2.Id,
                Quantity = do1a2.Quantity
            };
            si1a2 = _salesInvoiceDetailService.CreateObject(si1a2, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            si1c = new SalesInvoiceDetail()
            {
                SalesInvoiceId = si3.Id,
                DeliveryOrderDetailId = do1c.Id,
                Quantity = do1c.Quantity
            };
            si1c = _salesInvoiceDetailService.CreateObject(si1c, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

        }

        void PopulateVoucher()
        {
            _salesInvoiceService.ConfirmObject(si1, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                                  _deliveryOrderDetailService, _receivableService);
            _salesInvoiceService.ConfirmObject(si2, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                                  _deliveryOrderDetailService, _receivableService);
            _salesInvoiceService.ConfirmObject(si3, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                                  _deliveryOrderDetailService, _receivableService);

            rv = new ReceiptVoucher()
            {
                ContactId = contact.Id,
                CashBankId = cashBank.Id,
                ReceiptDate = DateTime.Today.AddDays(14),
                IsGBCH = true,
                //IsBank = true,
                DueDate = DateTime.Today.AddDays(14),
                TotalAmount = si1.AmountReceivable + si2.AmountReceivable + si3.AmountReceivable
            };
            _receiptVoucherService.CreateObject(rv, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            rvd1 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si1.Id).Id,
                Amount = si1.AmountReceivable,
                Description = "Receipt buat Sales Invoice 1"
            };
            _receiptVoucherDetailService.CreateObject(rvd1, _receiptVoucherService, _cashBankService, _receivableService);

            rvd2 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si2.Id).Id,
                Amount = si2.AmountReceivable,
                Description = "Receipt buat Sales Invoice 2"
            };
            _receiptVoucherDetailService.CreateObject(rvd2, _receiptVoucherService, _cashBankService, _receivableService);

            rvd3 = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = rv.Id,
                ReceivableId = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.SalesInvoice, si3.Id).Id,
                Amount = si3.AmountReceivable,
                Description = "Receipt buat Sales Invoice 3"
            };
            _receiptVoucherDetailService.CreateObject(rvd3, _receiptVoucherService, _cashBankService, _receivableService);

            _receiptVoucherService.ConfirmObject(rv, DateTime.Today, _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService, _generalLedgerJournalService, _accountService, _closingService);

            _receiptVoucherService.ReconcileObject(rv, DateTime.Today.AddDays(10), _receiptVoucherDetailService, _cashMutationService, _cashBankService, _receivableService,
                                                   _generalLedgerJournalService, _accountService, _closingService);
        }
    }
}
