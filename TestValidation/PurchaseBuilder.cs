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
    public class PurchaseBuilder
    {
        public IAccountService _accountService;
        public IBarringService _barringService;
        public IBarringOrderService _barringOrderService;
        public IBarringOrderDetailService _barringOrderDetailService;
        public ICashBankService _cashBankService;
        public ICashBankAdjustmentService _cashBankAdjustmentService;
        public ICashBankMutationService _cashBankMutationService;
        public ICashMutationService _cashMutationService;
        public ICoreBuilderService _coreBuilderService;
        public ICoreIdentificationService _coreIdentificationService;
        public ICoreIdentificationDetailService _coreIdentificationDetailService;
        public IContactService _contactService;
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
        public PurchaseOrder po1, po2;
        public PurchaseOrderDetail po1a, po1b, po1c, po2a, po2b;
        public PurchaseReceival pr1, pr2, pr3;
        public PurchaseReceivalDetail pr1a, pr1b, pr2a, pr2b, pr1a2, pr1c;
        public PurchaseInvoice pi1, pi2, pi3;
        public PurchaseInvoiceDetail pi1a, pi1b, pi2a, pi2b, pi1a2, pi1c;
        public PaymentVoucher pv;
        public PaymentVoucherDetail pvd1, pvd2, pvd3;

        public Account Asset, CashBank, AccountReceivable, GBCHReceivable, Inventory;
        public Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;
        public Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
        public Account Equity, OwnersEquity, EquityAdjustment;
        public Account Revenue;

        public PurchaseBuilder()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _barringOrderService = new BarringOrderService(new BarringOrderRepository(), new BarringOrderValidator());
            _barringOrderDetailService = new BarringOrderDetailService(new BarringOrderDetailRepository(), new BarringOrderDetailValidator());
            _cashBankAdjustmentService = new CashBankAdjustmentService(new CashBankAdjustmentRepository(), new CashBankAdjustmentValidator());
            _cashBankMutationService = new CashBankMutationService(new CashBankMutationRepository(), new CashBankMutationValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
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
                EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Name = "Equity Adjustment", IsLeaf = true, Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 3, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);

                Revenue = _accountService.CreateLegacyObject(new Account() { Name = "Revenue", IsLeaf = true, Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
            }
        }

        public void PopulateData()
        {
            PopulateMasterData();
            PopulateOrderAndReceivalData();
            PopulateInvoiceData();
            PopulatePaymentVouher();
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
                UoMId = Pcs.Id,
                SellingPrice = 50000,
                AvgPrice = 50000
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
                UoMId = Pcs.Id,
                SellingPrice = 72000,
                AvgPrice = 72000
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
                UoMId = Pcs.Id,
                SellingPrice = 100000,
                AvgPrice = 100000
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
            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment, DateTime.Now, _cashMutationService, _cashBankService);
        }

        public void PopulateOrderAndReceivalData()
        {
            TimeSpan purchaseDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan receivedDate = new TimeSpan(3, 0, 0 ,0);
            TimeSpan lateReceivedDate = new TimeSpan(2, 0, 0, 0);
            po1 = new PurchaseOrder()
            {
                PurchaseDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id
            };
            _purchaseOrderService.CreateObject(po1, _contactService);

            po2 = new PurchaseOrder()
            {
                PurchaseDate = DateTime.Today.Subtract(purchaseDate),
                ContactId = contact.Id
            };
            _purchaseOrderService.CreateObject(po2, _contactService);

            po1a = new PurchaseOrderDetail()
            {
                ItemId = blanket1.Id,
                PurchaseOrderId = po1.Id,
                Quantity = 300,
                Price = 50000
            };
            _purchaseOrderDetailService.CreateObject(po1a, _purchaseOrderService, _itemService);

            po1b = new PurchaseOrderDetail()
            {
                ItemId = blanket2.Id,
                PurchaseOrderId = po1.Id,
                Quantity = 250,
                Price = 72000
            };
            _purchaseOrderDetailService.CreateObject(po1b, _purchaseOrderService, _itemService);

            po1c = new PurchaseOrderDetail()
            {
                ItemId = blanket3.Id,
                PurchaseOrderId = po1.Id,
                Quantity = 100,
                Price = 100000
            };
            _purchaseOrderDetailService.CreateObject(po1c, _purchaseOrderService, _itemService);

            po2a = new PurchaseOrderDetail()
            {
                ItemId = blanket1.Id,
                PurchaseOrderId = po2.Id,
                Quantity = 300,
                Price = 50000
            };
            _purchaseOrderDetailService.CreateObject(po2a, _purchaseOrderService, _itemService);

            po2b = new PurchaseOrderDetail()
            {
                ItemId = blanket2.Id,
                PurchaseOrderId = po2.Id,
                Quantity = 250,
                Price = 72000
            };
            _purchaseOrderDetailService.CreateObject(po2b, _purchaseOrderService, _itemService);

            _purchaseOrderService.ConfirmObject(po1, po1.PurchaseDate, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            _purchaseOrderService.ConfirmObject(po2, po2.PurchaseDate, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);

            pr1 = new PurchaseReceival()
            {
                PurchaseOrderId = po1.Id,
                ReceivalDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _purchaseReceivalService.CreateObject(pr1, _purchaseOrderService, _warehouseService);

            pr2 = new PurchaseReceival()
            {
                PurchaseOrderId = po2.Id,
                ReceivalDate = DateTime.Now.Subtract(receivedDate),
                WarehouseId = localWarehouse.Id
            };
            _purchaseReceivalService.CreateObject(pr2, _purchaseOrderService, _warehouseService);

            pr1a = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po1a.Id,
                PurchaseReceivalId = pr1.Id,
                ItemId = po1a.ItemId,
                Quantity = po1a.Quantity - 100
            };
            _purchaseReceivalDetailService.CreateObject(pr1a, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr1b = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po1b.Id,
                PurchaseReceivalId = pr1.Id,
                ItemId = po1b.ItemId,
                Quantity = po1b.Quantity
            };
            _purchaseReceivalDetailService.CreateObject(pr1b, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr2a = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po2a.Id,
                PurchaseReceivalId = pr2.Id,
                ItemId = po2a.ItemId,
                Quantity = po2a.Quantity
            };
            _purchaseReceivalDetailService.CreateObject(pr2a, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr2b = new PurchaseReceivalDetail()
            {
                PurchaseOrderDetailId = po2b.Id,
                PurchaseReceivalId = pr2.Id,
                ItemId = po2b.ItemId,
                Quantity = po2b.Quantity
            };
            _purchaseReceivalDetailService.CreateObject(pr2b, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr3 = new PurchaseReceival()
            {
                PurchaseOrderId = po1.Id,
                ReceivalDate = DateTime.Now.Subtract(lateReceivedDate),
                WarehouseId = localWarehouse.Id
            };
            _purchaseReceivalService.CreateObject(pr3, _purchaseOrderService, _warehouseService);

            pr1c = new PurchaseReceivalDetail()
            {
                PurchaseReceivalId = pr3.Id,
                PurchaseOrderDetailId = po1c.Id,
                Quantity = po1c.Quantity,
                ItemId = po1c.ItemId
            };
            _purchaseReceivalDetailService.CreateObject(pr1c, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

            pr1a2 = new PurchaseReceivalDetail()
            {
                PurchaseReceivalId = pr3.Id,
                PurchaseOrderDetailId = po1a.Id,
                Quantity = 100,
                ItemId = po1a.ItemId
            };
            _purchaseReceivalDetailService.CreateObject(pr1a2, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);

        }

        public void PopulateInvoiceData()
        {
            TimeSpan receivedDate = new TimeSpan(3, 0, 0, 0);
            TimeSpan lateReceivedDate = new TimeSpan(2, 0, 0, 0);
            _purchaseReceivalService.ConfirmObject(pr1, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                       _itemService, _barringService, _warehouseItemService);
            _purchaseReceivalService.ConfirmObject(pr2, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService, _stockMutationService,
                                                   _itemService, _barringService, _warehouseItemService);
            _purchaseReceivalService.ConfirmObject(pr3, DateTime.Now.Subtract(receivedDate), _purchaseReceivalDetailService, _purchaseOrderService, _purchaseOrderDetailService,
                                                   _stockMutationService, _itemService, _barringService, _warehouseItemService);

            pi1 = new PurchaseInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Pembayaran PR1",
                PurchaseReceivalId = pr1.Id,
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
            };
            pi1 = _purchaseInvoiceService.CreateObject(pi1, _purchaseReceivalService);

            pi1a = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi1.Id,
                PurchaseReceivalDetailId = pr1a.Id,
                Quantity = pr1a.Quantity
            };
            pi1a = _purchaseInvoiceDetailService.CreateObject(pi1a, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi1b = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi1.Id,
                PurchaseReceivalDetailId = pr1b.Id,
                Quantity = pr1b.Quantity
            };
            pi1b = _purchaseInvoiceDetailService.CreateObject(pi1b, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi2 = new PurchaseInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Pembayaran PR2",
                PurchaseReceivalId = pr2.Id,
                IsTaxable = true,
                Discount = 5,
                DueDate = DateTime.Today.AddDays(14)
            };
            pi2 = _purchaseInvoiceService.CreateObject(pi2, _purchaseReceivalService);

            pi2a = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi2.Id,
                PurchaseReceivalDetailId = pr2a.Id,
                Quantity = pr2a.Quantity
            };
            pi2a = _purchaseInvoiceDetailService.CreateObject(pi2a, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi2b = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi2.Id,
                PurchaseReceivalDetailId = pr2b.Id,
                Quantity = pr2b.Quantity
            };
            pi2b = _purchaseInvoiceDetailService.CreateObject(pi2b, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi3 = new PurchaseInvoice()
            {
                InvoiceDate = DateTime.Today,
                Description = "Pembayaran PR3",
                PurchaseReceivalId = pr3.Id,
                IsTaxable = true,
                Discount = 0,
                DueDate = DateTime.Today.AddDays(14)
            };
            pi3 = _purchaseInvoiceService.CreateObject(pi3, _purchaseReceivalService);

            pi1a2 = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi3.Id,
                PurchaseReceivalDetailId = pr1a2.Id,
                Quantity = pr1a2.Quantity
            };
            pi1a2 = _purchaseInvoiceDetailService.CreateObject(pi1a2, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

            pi1c = new PurchaseInvoiceDetail()
            {
                PurchaseInvoiceId = pi3.Id,
                PurchaseReceivalDetailId = pr1c.Id,
                Quantity = pr1c.Quantity
            };
            pi1c = _purchaseInvoiceDetailService.CreateObject(pi1c, _purchaseInvoiceService, _purchaseOrderDetailService, _purchaseReceivalDetailService);

        }

        void PopulatePaymentVouher()
        {
            _purchaseInvoiceService.ConfirmObject(pi1, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService);
            _purchaseInvoiceService.ConfirmObject(pi2, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService);
            _purchaseInvoiceService.ConfirmObject(pi3, DateTime.Today, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService,
                                                  _purchaseReceivalDetailService, _payableService);

            pv = new PaymentVoucher()
            {
                ContactId = contact.Id,
                CashBankId = cashBank.Id,
                PaymentDate = DateTime.Today.AddDays(14),
                IsGBCH = true,
                //IsBank = true,
                DueDate = DateTime.Today.AddDays(14),
                TotalAmount = pi1.AmountPayable + pi2.AmountPayable + pi3.AmountPayable
            };
            _paymentVoucherService.CreateObject(pv, _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);

            pvd1 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi1.Id).Id,
                Amount = pi1.AmountPayable,
                Description = "Payment buat Purchase Invoice 1"
            };
            _paymentVoucherDetailService.CreateObject(pvd1, _paymentVoucherService, _cashBankService, _payableService);

            pvd2 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi2.Id).Id,
                Amount = pi2.AmountPayable,
                Description = "Payment buat Purchase Invoice 2"
            };
            _paymentVoucherDetailService.CreateObject(pvd2, _paymentVoucherService, _cashBankService, _payableService);

            pvd3 = new PaymentVoucherDetail()
            {
                PaymentVoucherId = pv.Id,
                PayableId = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PurchaseInvoice, pi3.Id).Id,
                Amount = pi3.AmountPayable,
                Description = "Payment buat Purchase Invoice 3"
            };
            _paymentVoucherDetailService.CreateObject(pvd3, _paymentVoucherService, _cashBankService, _payableService);

            _paymentVoucherService.ConfirmObject(pv, DateTime.Today, _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService);

            _paymentVoucherService.ReconcileObject(pv, DateTime.Today.AddDays(10), _paymentVoucherDetailService, _cashMutationService, _cashBankService, _payableService);
        }
    }
}
