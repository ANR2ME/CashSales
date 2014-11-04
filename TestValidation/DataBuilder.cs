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
    public class DataBuilder
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
        public IDeliveryOrderService _deliveryOrderService;
        public IDeliveryOrderDetailService _deliveryOrderDetailService;
        public IGeneralLedgerJournalService _generalLedgerJournalService;
        public IItemService _itemService;
        public IItemTypeService _itemTypeService;
        public IMachineService _machineService;
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
        public IReceiptVoucherDetailService _receiptVoucherDetailService;
        public IReceiptVoucherService _receiptVoucherService;
        public ISalesInvoiceDetailService _salesInvoiceDetailService;
        public ISalesInvoiceService _salesInvoiceService;
        public ISalesOrderService _salesOrderService;
        public ISalesOrderDetailService _salesOrderDetailService;
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
        public IValidCombService _validCombService;
        public IWarehouseItemService _warehouseItemService;
        public IWarehouseService _warehouseService;
        public IWarehouseMutationOrderService _warehouseMutationOrderService;
        public IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService;

        public ICashSalesInvoiceService _cashSalesInvoiceService;
        public ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService;
        public IRetailPurchaseInvoiceService _retailPurchaseInvoiceService;
        public IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService;
        public IQuantityPricingService _quantityPricingService;
        public ICashSalesReturnService _cashSalesReturnService;
        public ICashSalesReturnDetailService _cashSalesReturnDetailService;

        public IPriceMutationService _priceMutationService;
        public IContactGroupService _contactGroupService;
        public IGroupItemPriceService _groupItemPriceService;
        public IPaymentRequestService _paymentRequestService;
        public IPaymentRequestDetailService _paymentRequestDetailService;

        public IUserAccountService _userAccountService;
        public IUserMenuService _userMenuService;
        public IUserAccessService _userAccessService;

        public UserAccount admin, user;
        public UserMenu menudata, menufinance;
        public UserAccess admindata, userdata, adminfinance, userfinance;
        public ContactGroup baseGroup, contactGroup1;
        public GroupItemPrice groupItemPrice1;
        public PaymentRequest paymentRequest1;
        public CashBank cashBank1, cashBank2;
        public ItemType typeAccessory, typeBar, typeBarring, typeBearing, typeBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public UoM Pcs, Boxes, Tubs;
        public Item item, itemCompound, itemCompound1, itemCompound2, itemAccessory1, itemAccessory2;
        public Warehouse localWarehouse, movingWarehouse;
        public Contact contact, baseContact;
        public Machine machine;
        public CashBankAdjustment cashBankAdjustment, cashBankAdjustment2;
        public CashBankMutation cashBankMutation;
        public CoreBuilder coreBuilder, coreBuilder1, coreBuilder2, coreBuilder3, coreBuilder4;
        public CoreIdentification coreIdentification, coreIdentificationInHouse, coreIdentificationContact;
        public CoreIdentificationDetail coreIdentificationDetail, coreIDInHouse1, coreIDInHouse2, coreIDInHouse3,
                                        coreIDContact1, coreIDContact2, coreIDContact3;
        public RollerBuilder rollerBuilder, rollerBuilder1, rollerBuilder2, rollerBuilder3, rollerBuilder4;
        public RecoveryOrder recoveryOrder, recoveryOrderContact, recoveryOrderInHouse;
        public RecoveryOrderDetail recoveryODContact1, recoveryODContact2, recoveryODContact3,
                                   recoveryODInHouse1, recoveryODInHouse2, recoveryODInHouse3;
        public RecoveryOrder recoveryOrderContact2, recoveryOrderInHouse2;
        public RecoveryOrderDetail recoveryODContact2b, recoveryODInHouse3b;
        public RecoveryAccessoryDetail accessory1, accessory2, accessory3, accessory4;
        public Item bargeneric, barleft1, barleft2, barright1, barright2;
        public Item blanket1, blanket2, blanket3;
        public Barring barring1, barring2, barring3;
        public BarringOrder barringOrderContact;
        public BarringOrderDetail barringODContact1, barringODContact2, barringODContact3, barringODContact4; 
        public WarehouseMutationOrder warehouseMutationOrder;
        public WarehouseMutationOrderDetail wmoDetail1, wmoDetail2, wmoDetail3, wmoDetail4, wmoDetail5, wmoDetail6,
                                            wmoDetail7, wmoDetail8, wmoDetail9;
        public RollerWarehouseMutation rollerWarehouseMutationContact, rollerWarehouseMutationInHouse;
        public RollerWarehouseMutationDetail rwmDetailContact1, rwmDetailContact2, rwmDetailContact3,
                                             rwmDetailInHouse1, rwmDetailInHouse2, rwmDetailInHouse3;
        public StockAdjustment stockAdjustment, sa;
        public StockAdjustmentDetail stockAD1, stockAD2, sad1, sad2, sad3, sad4, sad5;

        public SalesOrder salesOrder1, salesOrder2, salesOrder3;
        public SalesOrderDetail salesOD1a, salesOD1b, salesOD2a, salesOD2b, salesOD3a, salesOD3b;
        public DeliveryOrder deliveryOrder1, deliveryOrder2, deliveryOrder3;
        public DeliveryOrderDetail deliveryOD1a, deliveryOD1b, deliveryOD2a, deliveryOD2b, deliveryOD3a, deliveryOD3b;
        public SalesInvoice salesInvoice1, salesInvoice2, salesInvoice3;
        public SalesInvoiceDetail salesID1a, salesID1b, salesID2a, salesID2b, salesID3a, salesID3b;
        public ReceiptVoucher receiptVoucher1, receiptVoucher2, receiptVoucher3;
        public ReceiptVoucherDetail receiptVD1a, receiptVD1b, receiptVD2a, receiptVD2b, receiptVD3a, receiptVD3b;
        public CashSalesInvoice cashSalesInvoice1, cashSalesInvoice2, cashSalesInvoice3;
        public CashSalesInvoiceDetail cashSalesID1a, cashSalesID2a, cashSalesID2b, cashSalesID3a;

        public PurchaseOrder purchaseOrder1;
        public PurchaseOrderDetail purchaseOD1a, purchaseOD1b;
        public PurchaseReceival purchaseReceival1;
        public PurchaseReceivalDetail purchaseRD1a, purchaseRD1b;
        public PurchaseInvoice purchaseInvoice1;
        public PurchaseInvoiceDetail purchaseID1a, purchaseID1b;
        public PaymentVoucher paymentVoucher1;
        public PaymentVoucherDetail paymentVD1a, paymentVD1b;

        // extended variable
        public int usedCoreBuilderQuantity, usedCoreBuilder1Quantity, usedCoreBuilder2Quantity, usedCoreBuilder3Quantity, usedCoreBuilder4Quantity;
        public int usedRollerBuilderQuantity, usedRollerBuilder1Quantity, usedRollerBuilder2Quantity, usedRollerBuilder3Quantity, usedRollerBuilder4Quantity;
        public int usedCoreBuilderFinal, usedCoreBuilder1Final, usedCoreBuilder2Final, usedCoreBuilder3Final, usedCoreBuilder4Final;
        public int usedRollerBuilderFinal, usedRollerBuilder1Final, usedRollerBuilder2Final, usedRollerBuilder3Final, usedRollerBuilder4Final;
        public int accessory1quantity;

        public Account Asset, CashBank, AccountReceivable, GBCHReceivable, Inventory;
        public Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;
        public Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
        public Account Equity, OwnersEquity, EquityAdjustment;
        public Account Revenue;

        public Closing thisMonthClosing;

        public DataBuilder()
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
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _warehouseMutationOrderService = new WarehouseMutationOrderService(new WarehouseMutationOrderRepository(), new WarehouseMutationOrderValidator());
            _warehouseMutationOrderDetailService = new WarehouseMutationOrderDetailService(new WarehouseMutationOrderDetailRepository(), new WarehouseMutationOrderDetailValidator());

            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesInvoiceDetailService = new CashSalesInvoiceDetailService(new CashSalesInvoiceDetailRepository(), new CashSalesInvoiceDetailValidator());
            _retailPurchaseInvoiceService = new RetailPurchaseInvoiceService(new RetailPurchaseInvoiceRepository(), new RetailPurchaseInvoiceValidator());
            _retailPurchaseInvoiceDetailService = new RetailPurchaseInvoiceDetailService(new RetailPurchaseInvoiceDetailRepository(), new RetailPurchaseInvoiceDetailValidator());
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(), new QuantityPricingValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _cashSalesReturnDetailService = new CashSalesReturnDetailService(new CashSalesReturnDetailRepository(), new CashSalesReturnDetailValidator());

            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _groupItemPriceService = new GroupItemPriceService(new GroupItemPriceRepository(), new GroupItemPriceValidator());
            _paymentRequestService = new PaymentRequestService(new PaymentRequestRepository(), new PaymentRequestValidator());
            _paymentRequestDetailService = new PaymentRequestDetailService(new PaymentRequestDetailRepository(), new PaymentRequestDetailValidator());

            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());

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
            baseContact = _contactService.FindOrCreateBaseContact(_contactGroupService);
            admin = _userAccountService.FindOrCreateSysAdmin();

            if (!_accountService.GetLegacyObjects().Any())
            {
                Asset = _accountService.CreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
                CashBank = _accountService.CreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
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
            PopulateUserRole();
            PopulateWarehouse();
            PopulateItem();
            PopulateSingles();
            PopulateBuilders();
            PopulateBarring();
            PopulateWarehouseMutationForRollerIdentificationAndRecovery();
            PopulateCoreIdentifications();
            PopulateRecoveryOrders();
            PopulateRecoveryOrders2();
            PopulateStockAdjustment();
            PopulateRecoveryOrders3();
            PopulateCoreIdentifications2();
            PopulateRollerWarehouseMutation();
            PopulateBarringOrders();
            PopulateCashBank();
            PopulateSales();
            PopulateCashSales();
            PopulateValidComb();
        }

        public void PopulateUserRole()
        {
            user = new UserAccount()
            {
                Username = "admin",
                Password = "123",
                Name = "admin",
                Description = "admin palsu",
            };
            user = _userAccountService.CreateObject(user);

            menudata = _userMenuService.CreateObject(Core.Constants.Constant.MenuName.Contact, Core.Constants.Constant.MenuGroupName.Master);
            menufinance = _userMenuService.CreateObject(Core.Constants.Constant.MenuName.ItemType, Core.Constants.Constant.MenuGroupName.Master);

            admindata = new UserAccess()
            {
                UserAccountId = admin.Id,
                UserMenuId = menudata.Id,
                AllowView = true,
                AllowCreate = true,
                AllowEdit = true,
                AllowConfirm = true,
                AllowUnconfirm = true,
                AllowPaid = true,
                AllowUnpaid = true,
                AllowReconcile = true,
                AllowUnreconcile = true,
                AllowPrint = true,
                AllowDelete = true,
                AllowUndelete = true
            };
            admindata = _userAccessService.CreateObject(admindata, _userAccountService, _userMenuService);

            adminfinance = new UserAccess()
            {
                UserAccountId = admin.Id,
                UserMenuId = menufinance.Id,
                AllowView = true,
                AllowCreate = true,
                AllowEdit = true,
                AllowConfirm = true,
                AllowUnconfirm = true,
                AllowPaid = true,
                AllowUnpaid = true,
                AllowReconcile = true,
                AllowUnreconcile = true,
                AllowPrint = true,
                AllowDelete = true,
                AllowUndelete = true
            };
            adminfinance = _userAccessService.CreateObject(adminfinance, _userAccountService, _userMenuService);
        }

        public void PopulateItem()
        {
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

            itemCompound = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB else",
                Category = "Compound",
                Sku = "CMP123",
                UoMId = Tubs.Id,
                SellingPrice = 20000,
                AvgPrice = 10000
            };

            itemCompound = _itemService.CreateObject(itemCompound, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemCompound1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB1",
                Category = "Compound",
                Sku = "CMP101",
                UoMId = _uomService.GetObjectByName("Tubs").Id,
                SellingPrice = 30000,
                AvgPrice = 10000
            };
            itemCompound1 = _itemService.CreateObject(itemCompound1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemCompound2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Compound").Id,
                Name = "Compound RB2",
                Category = "Compound",
                Sku = "CMP102",
                UoMId = Tubs.Id,
                SellingPrice = 40000,
                AvgPrice = 10000
            };
            itemCompound2 = _itemService.CreateObject(itemCompound2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemAccessory1 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 1",
                Category = "Accessory",
                Sku = "ACC001",
                UoMId = Pcs.Id,
                SellingPrice = 50000,
                AvgPrice = 10000
            };
            itemAccessory1 = _itemService.CreateObject(itemAccessory1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            itemAccessory2 = new Item()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Accessory").Id,
                Name = "Accessory Sample 2",
                Category = "Accessory",
                Sku = "ACC002",
                UoMId = Pcs.Id,
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            itemAccessory2 = _itemService.CreateObject(itemAccessory2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            sa = new StockAdjustment()
            {
                AdjustmentDate = DateTime.Now,
                Code = "SA001",
                WarehouseId = localWarehouse.Id
            };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);

            sad1 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemCompound.Id,
                Quantity = 200000,
                Code = "ITCM000",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad1, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad2 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemCompound1.Id,
                Quantity = 200000,
                Code = "ITCM001",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad2, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad3 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemCompound2.Id,
                Quantity = 200000,
                Code = "ITCM002",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad3, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad4 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 15,
                Code = "ITAC001",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad4, _stockAdjustmentService, _itemService, _warehouseItemService);

            sad5 = new StockAdjustmentDetail()
            {
                StockAdjustmentId = sa.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 10,
                Code = "ITAC002",
                Price = 50000
            };
            _stockAdjustmentDetailService.CreateObject(sad5, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService,
                                                  _warehouseItemService, _generalLedgerJournalService, _accountService, _closingService);

        }

        public void PopulateWarehouse()
        {
            localWarehouse = new Warehouse()
            {
                Name = "Sentral Solusi Data",
                Description = "Kali Besar Jakarta",
                Code = "LCL"
            };
            localWarehouse = _warehouseService.CreateObject(localWarehouse, _warehouseItemService, _itemService);

            movingWarehouse = new Warehouse()
            {
                Name = "Kepala Manufacturing Roland",
                Description = "Mengerjakan Roller dan Barring",
                Code = "MVG"
            };
            movingWarehouse = _warehouseService.CreateObject(movingWarehouse, _warehouseItemService, _itemService);
        }

        public void PopulateSingles()
        {
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

            machine = new Machine()
            {
                Code = "MX0001",
                Name = "Printing Machine",
                Description = "Generic"
            };
            machine = _machineService.CreateObject(machine);

            cashBank1 = new CashBank()
            {
                Name = "Kontan",
                IsBank = false,
                Description = "Kontan"
            };
            _cashBankService.CreateObject(cashBank1, _accountService);

            cashBank2 = new CashBank()
            {
                Name = "Bank BCA",
                IsBank = true,
                Description = "Bank BCA"
            };
            _cashBankService.CreateObject(cashBank2, _accountService);

            paymentRequest1 = _paymentRequestService.CreateObject(contact.Id, "Pembayaran Listrik", 500000, DateTime.Now, DateTime.Today.AddDays(14), _contactService, _paymentRequestDetailService, _accountService, _generalLedgerJournalService, _closingService) ;
        }

        public void PopulateCashBank()
        {
            cashBankAdjustment = new CashBankAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Amount = 200000000,
                CashBankId = cashBank1.Id,
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment, _cashBankService);

            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment, DateTime.Today, _cashMutationService, _cashBankService,
                                                     _generalLedgerJournalService, _accountService, _closingService);

            cashBankAdjustment2 = new CashBankAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Amount = -50000,
                CashBankId = cashBank1.Id,
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment2, _cashBankService);

            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment2, DateTime.Today, _cashMutationService, _cashBankService,
                                                     _generalLedgerJournalService, _accountService, _closingService);

            cashBankMutation = new CashBankMutation()
            {
                Amount = 50000000,
                SourceCashBankId = cashBank1.Id,
                TargetCashBankId = cashBank2.Id,
                Code = "CBM0001",
            };
            _cashBankMutationService.CreateObject(cashBankMutation, _cashBankService);

            _cashBankMutationService.ConfirmObject(cashBankMutation, DateTime.Today, _cashMutationService, _cashBankService,
                                                     _generalLedgerJournalService, _accountService, _closingService);

        }

        public void PopulateBuilders()
        {
            coreBuilder = new CoreBuilder()
            {
                BaseSku = "CBX",
                SkuNewCore = "CBXN",
                SkuUsedCore = "CBXU",
                Name = "Core X",
                Category = "X",
                UoMId = Pcs.Id
            };
            coreBuilder = _coreBuilderService.CreateObject(coreBuilder, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            coreBuilder1 = new CoreBuilder()
            {
                BaseSku = "CBA001",
                SkuNewCore = "CB001N",
                SkuUsedCore = "CB001U",
                Name = "Core A 001",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder1 = _coreBuilderService.CreateObject(coreBuilder1, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            coreBuilder2 = new CoreBuilder()
            {
                BaseSku = "CBA002",
                SkuNewCore = "CB002N",
                SkuUsedCore = "CB002U",
                Name = "Core A 002",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder2 = _coreBuilderService.CreateObject(coreBuilder2, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            coreBuilder3 = new CoreBuilder()
            {
                BaseSku = "CBA003",
                SkuNewCore = "CB003N",
                SkuUsedCore = "CB003U",
                Name = "Core A 003",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder3 = _coreBuilderService.CreateObject(coreBuilder3, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            coreBuilder4 = new CoreBuilder()
            {
                BaseSku = "CBA004",
                SkuNewCore = "CB004N",
                SkuUsedCore = "CB004U",
                Name = "Core A 004",
                Category = "A",
                UoMId = Pcs.Id
            };
            coreBuilder4 = _coreBuilderService.CreateObject(coreBuilder4, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            rollerBuilder = new RollerBuilder()
            {
                BaseSku = "RBX",
                SkuRollerNewCore = "RBXN",
                SkuRollerUsedCore = "RBXU",
                RD = 12,
                CD = 12,
                TL = 12,
                WL = 12,
                RL = 12,
                Name = "Roller X",
                Category = "X",
                CoreBuilderId = coreBuilder.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeDamp.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder = _rollerBuilderService.CreateObject(rollerBuilder, _machineService, _uomService, _itemService, _itemTypeService,
                                                               _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                               _priceMutationService, _contactGroupService);

            rollerBuilder1 = new RollerBuilder()
            {
                BaseSku = "RBA001",
                SkuRollerNewCore = "RBA001N",
                SkuRollerUsedCore = "RBA001U",
                RD = 11,
                CD = 11,
                TL = 11,
                WL = 11,
                RL = 11,
                Name = "Roller A001",
                Category = "A",
                CoreBuilderId = coreBuilder1.Id,
                CompoundId = itemCompound1.Id,
                MachineId = machine.Id,
                RollerTypeId = typeFoundDT.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder1 = _rollerBuilderService.CreateObject(rollerBuilder1, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);

            rollerBuilder2 = new RollerBuilder()
            {
                BaseSku = "RBA002",
                SkuRollerNewCore = "RBA002N",
                SkuRollerUsedCore = "RBA002U",
                RD = 13,
                CD = 13,
                TL = 13,
                WL = 13,
                RL = 13,
                Name = "Roller A002",
                Category = "A",
                CoreBuilderId = coreBuilder2.Id,
                CompoundId = itemCompound2.Id,
                MachineId = machine.Id,
                RollerTypeId = typeDampFormDQ.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder2 = _rollerBuilderService.CreateObject(rollerBuilder2, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);

            rollerBuilder3 = new RollerBuilder()
            {
                BaseSku = "RBA003",
                SkuRollerNewCore = "RBA003N",
                SkuRollerUsedCore = "RBA003U",
                RD = 13,
                CD = 13,
                TL = 13,
                WL = 13,
                RL = 13,
                Name = "Roller A003",
                Category = "A",
                CoreBuilderId = coreBuilder3.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeInkDistD.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder3 = _rollerBuilderService.CreateObject(rollerBuilder3, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);

            rollerBuilder4 = new RollerBuilder()
            {
                BaseSku = "RBA004",
                SkuRollerNewCore = "RBA004N",
                SkuRollerUsedCore = "RBA004U",
                RD = 14,
                CD = 14,
                TL = 14,
                WL = 14,
                RL = 14,
                Name = "Roller X",
                Category = "X",
                CoreBuilderId = coreBuilder4.Id,
                CompoundId = itemCompound.Id,
                MachineId = machine.Id,
                RollerTypeId = typeInkDistH.Id,
                UoMId = Pcs.Id
            };
            rollerBuilder4 = _rollerBuilderService.CreateObject(rollerBuilder4, _machineService, _uomService, _itemService, _itemTypeService,
                                                                _coreBuilderService, _rollerTypeService, _warehouseItemService, _warehouseService,
                                                                _priceMutationService, _contactGroupService);
            Item NewCore = _coreBuilderService.GetNewCore(coreBuilder.Id);
            Item NewCore1 = _coreBuilderService.GetNewCore(coreBuilder1.Id);
            Item NewCore2 = _coreBuilderService.GetNewCore(coreBuilder2.Id);
            Item NewCore3 = _coreBuilderService.GetNewCore(coreBuilder3.Id);
            Item NewCore4 = _coreBuilderService.GetNewCore(coreBuilder4.Id);
            Item UsedCore = _coreBuilderService.GetUsedCore(coreBuilder.Id);
            Item UsedCore1 = _coreBuilderService.GetUsedCore(coreBuilder1.Id);
            Item UsedCore2 = _coreBuilderService.GetUsedCore(coreBuilder2.Id);
            Item UsedCore3 = _coreBuilderService.GetUsedCore(coreBuilder3.Id);
            Item UsedCore4 = _coreBuilderService.GetUsedCore(coreBuilder4.Id);
            Item RollerNewCore = _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id);
            Item RollerNewCore1 = _rollerBuilderService.GetRollerNewCore(rollerBuilder1.Id);
            Item RollerNewCore2 = _rollerBuilderService.GetRollerNewCore(rollerBuilder2.Id);
            Item RollerNewCore3 = _rollerBuilderService.GetRollerNewCore(rollerBuilder3.Id);
            Item RollerNewCore4 = _rollerBuilderService.GetRollerNewCore(rollerBuilder4.Id);
            Item RollerUsedCore = _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
            Item RollerUsedCore1 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder1.Id);
            Item RollerUsedCore2 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder2.Id);
            Item RollerUsedCore3 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder3.Id);
            Item RollerUsedCore4 = _rollerBuilderService.GetRollerUsedCore(rollerBuilder4.Id);

            StockAdjustment saCore = new StockAdjustment()
            {
                AdjustmentDate = DateTime.Today,
                Description = "Core Adjustment",
                WarehouseId = localWarehouse.Id
            };
            _stockAdjustmentService.CreateObject(saCore, _warehouseService);
            StockAdjustmentDetail sadNewCore = new StockAdjustmentDetail() { ItemId = NewCore.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore1 = new StockAdjustmentDetail() { ItemId = NewCore1.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore2 = new StockAdjustmentDetail() { ItemId = NewCore2.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore3 = new StockAdjustmentDetail() { ItemId = NewCore3.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadNewCore4 = new StockAdjustmentDetail() { ItemId = NewCore4.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadNewCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadUsedCore = new StockAdjustmentDetail() { ItemId = UsedCore.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore1 = new StockAdjustmentDetail() { ItemId = UsedCore1.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore2 = new StockAdjustmentDetail() { ItemId = UsedCore2.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore3 = new StockAdjustmentDetail() { ItemId = UsedCore3.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadUsedCore4 = new StockAdjustmentDetail() { ItemId = UsedCore4.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadUsedCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadRollerNewCore = new StockAdjustmentDetail() { ItemId = RollerNewCore.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore1 = new StockAdjustmentDetail() { ItemId = RollerNewCore1.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore2 = new StockAdjustmentDetail() { ItemId = RollerNewCore2.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore3 = new StockAdjustmentDetail() { ItemId = RollerNewCore3.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerNewCore4 = new StockAdjustmentDetail() { ItemId = RollerNewCore4.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerNewCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            StockAdjustmentDetail sadRollerUsedCore = new StockAdjustmentDetail() { ItemId = RollerUsedCore.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore1 = new StockAdjustmentDetail() { ItemId = RollerUsedCore1.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore2 = new StockAdjustmentDetail() { ItemId = RollerUsedCore2.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore3 = new StockAdjustmentDetail() { ItemId = RollerUsedCore3.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollerUsedCore4 = new StockAdjustmentDetail() { ItemId = RollerUsedCore4.Id, Quantity = 7, StockAdjustmentId = saCore.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollerUsedCore4, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(saCore, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService,
                                                  _generalLedgerJournalService, _accountService, _closingService);

        }

        public void PopulateWarehouseMutationForRollerIdentificationAndRecovery()
        {
            warehouseMutationOrder = new WarehouseMutationOrder()
            {
                WarehouseFromId = localWarehouse.Id,
                WarehouseToId = movingWarehouse.Id,
                MutationDate = DateTime.Now
            };
            warehouseMutationOrder = _warehouseMutationOrderService.CreateObject(warehouseMutationOrder, _warehouseService);

            wmoDetail1 = new WarehouseMutationOrderDetail()
            {
                WarehouseMutationOrderId = warehouseMutationOrder.Id,
                ItemId = coreBuilder1.UsedCoreItemId,
                Quantity = 2
            };
            wmoDetail1 = _warehouseMutationOrderDetailService.CreateObject(wmoDetail1, _warehouseMutationOrderService, _itemService, _warehouseItemService);

            wmoDetail2 = new WarehouseMutationOrderDetail()
            {
                WarehouseMutationOrderId = warehouseMutationOrder.Id,
                ItemId = coreBuilder2.UsedCoreItemId,
                Quantity = 2
            };
            wmoDetail2 = _warehouseMutationOrderDetailService.CreateObject(wmoDetail2, _warehouseMutationOrderService, _itemService, _warehouseItemService);

            wmoDetail3 = new WarehouseMutationOrderDetail()
            {
                WarehouseMutationOrderId = warehouseMutationOrder.Id,
                ItemId = itemCompound.Id,
                Quantity = 500
            };
            wmoDetail3 = _warehouseMutationOrderDetailService.CreateObject(wmoDetail3, _warehouseMutationOrderService, _itemService, _warehouseItemService);

            wmoDetail4 = new WarehouseMutationOrderDetail()
            {
                WarehouseMutationOrderId = warehouseMutationOrder.Id,
                ItemId = itemCompound1.Id,
                Quantity = 500
            };
            wmoDetail4 = _warehouseMutationOrderDetailService.CreateObject(wmoDetail4, _warehouseMutationOrderService, _itemService, _warehouseItemService);

            wmoDetail5 = new WarehouseMutationOrderDetail()
            {
                WarehouseMutationOrderId = warehouseMutationOrder.Id,
                ItemId = itemCompound2.Id,
                Quantity = 500
            };
            wmoDetail5 = _warehouseMutationOrderDetailService.CreateObject(wmoDetail5, _warehouseMutationOrderService, _itemService, _warehouseItemService);

            wmoDetail6 = new WarehouseMutationOrderDetail()
            {
                WarehouseMutationOrderId = warehouseMutationOrder.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 1
            };
            wmoDetail6 = _warehouseMutationOrderDetailService.CreateObject(wmoDetail6, _warehouseMutationOrderService, _itemService, _warehouseItemService);

            wmoDetail7 = new WarehouseMutationOrderDetail()
            {
                WarehouseMutationOrderId = warehouseMutationOrder.Id,
                ItemId = barring3.Id,
                Quantity = 1
            };
            wmoDetail7 = _warehouseMutationOrderDetailService.CreateObject(wmoDetail7, _warehouseMutationOrderService, _itemService, _warehouseItemService);
        }

        public void PopulateCoreIdentifications()
        {
            warehouseMutationOrder = _warehouseMutationOrderService.ConfirmObject(warehouseMutationOrder, DateTime.Today, _warehouseMutationOrderDetailService, _itemService,
                                                                                  _barringService, _warehouseItemService, _stockMutationService);
            
            coreIdentification = new CoreIdentification()
            {
                Code = "CI001",
                ContactId = contact.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 1,
                WarehouseId = movingWarehouse.Id
            };
            coreIdentification = _coreIdentificationService.CreateObject(coreIdentification, _contactService);
            coreIdentificationInHouse = new CoreIdentification()
            {
                Code = "CIH001",
                IsInHouse = true,
                IdentifiedDate = DateTime.Now,
                Quantity = 3,
                WarehouseId = movingWarehouse.Id
            };
            coreIdentificationInHouse = _coreIdentificationService.CreateObject(coreIdentificationInHouse, _contactService);
            coreIdentificationContact = new CoreIdentification()
            {
                Code = "CIC001",
                ContactId = contact.Id,
                IsInHouse = false,
                IdentifiedDate = DateTime.Now,
                Quantity = 3,
                WarehouseId = movingWarehouse.Id
            };
            coreIdentificationContact = _coreIdentificationService.CreateObject(coreIdentificationContact, _contactService);

            coreIdentificationDetail = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder.Id,
                CoreIdentificationId = coreIdentification.Id,
                MachineId = machine.Id,
                DetailId = 1,
                RollerTypeId = typeDamp.Id,
                RD = (decimal) 10.1,
                CD = (decimal) 10.2,
                TL = (decimal) 11.9,
                WL = (decimal) 11.3,
                RL = (decimal) 9.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIdentificationDetail = _coreIdentificationDetailService.CreateObject(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDInHouse1 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder1.Id,
                CoreIdentificationId = coreIdentificationInHouse.Id,
                MachineId = machine.Id,
                DetailId = 1,
                RollerTypeId = typeFoundDT.Id,
                RD = (decimal)10.1,
                CD = (decimal)10.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)9.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDInHouse1 = _coreIdentificationDetailService.CreateObject(coreIDInHouse1, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDInHouse2 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder1.Id,
                CoreIdentificationId = coreIdentificationInHouse.Id,
                MachineId = machine.Id,
                DetailId = 2,
                RollerTypeId = typeFoundDT.Id,
                RD = (decimal)10.1,
                CD = (decimal)10.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)9.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDInHouse2 = _coreIdentificationDetailService.CreateObject(coreIDInHouse2, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDInHouse3 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder2.Id,
                CoreIdentificationId = coreIdentificationInHouse.Id,
                MachineId = machine.Id,
                DetailId = 3,
                RollerTypeId = typeDampFormDQ.Id,
                RD = (decimal)12.1,
                CD = (decimal)10.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)12.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDInHouse3 = _coreIdentificationDetailService.CreateObject(coreIDInHouse3, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDContact1 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder3.Id,
                CoreIdentificationId = coreIdentificationContact.Id,
                MachineId = machine.Id,
                DetailId = 1,
                RollerTypeId = typeInkDistD.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDContact1 = _coreIdentificationDetailService.CreateObject(coreIDContact1, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDContact2 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder3.Id,
                CoreIdentificationId = coreIdentificationContact.Id,
                MachineId = machine.Id,
                DetailId = 2,
                RollerTypeId = typeInkDistD.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDContact2 = _coreIdentificationDetailService.CreateObject(coreIDContact2, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);

            coreIDContact3 = new CoreIdentificationDetail()
            {
                CoreBuilderId = coreBuilder4.Id,
                CoreIdentificationId = coreIdentificationContact.Id,
                MachineId = machine.Id,
                DetailId = 3,
                RollerTypeId = typeInkDistH.Id,
                RD = (decimal)9.1,
                CD = (decimal)9.2,
                TL = (decimal)9.9,
                WL = (decimal)9.3,
                RL = (decimal)8.2,
                MaterialCase = Core.Constants.Constant.MaterialCase.Used
            };
            coreIDContact3 = _coreIdentificationDetailService.CreateObject(coreIDContact3, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);
        }
        
        public void PopulateRecoveryOrders()
        {
            coreIdentification = _coreIdentificationService.ConfirmObject(coreIdentification, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _barringService);
            coreIdentificationContact = _coreIdentificationService.ConfirmObject(coreIdentificationContact, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _barringService);
            coreIdentificationInHouse = _coreIdentificationService.ConfirmObject(coreIdentificationInHouse, DateTime.Today, _coreIdentificationDetailService, _stockMutationService, _recoveryOrderService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService, _barringService);

            coreIdentificationDetail = _coreIdentificationDetailService.FinishObject(coreIdentificationDetail, DateTime.Today, _coreIdentificationService, _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            coreIDContact1 = _coreIdentificationDetailService.FinishObject(coreIDContact1, DateTime.Today, _coreIdentificationService, _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            coreIDContact2 = _coreIdentificationDetailService.FinishObject(coreIDContact2, DateTime.Today, _coreIdentificationService, _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            coreIDContact3 = _coreIdentificationDetailService.FinishObject(coreIDContact3, DateTime.Today, _coreIdentificationService, _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            coreIDInHouse1 = _coreIdentificationDetailService.FinishObject(coreIDInHouse1, DateTime.Today, _coreIdentificationService, _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            coreIDInHouse2 = _coreIdentificationDetailService.FinishObject(coreIDInHouse2, DateTime.Today, _coreIdentificationService, _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            coreIDInHouse3 = _coreIdentificationDetailService.FinishObject(coreIDInHouse3, DateTime.Today, _coreIdentificationService, _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService);

            recoveryOrder = new RecoveryOrder()
            {
                Code = "ROX",
                CoreIdentificationId = coreIdentification.Id,
                QuantityReceived = coreIdentification.Quantity,
                WarehouseId = movingWarehouse.Id
            };
            recoveryOrder = _recoveryOrderService.CreateObject(recoveryOrder, _coreIdentificationService);

            recoveryOrderInHouse = new RecoveryOrder()
            {
                Code = "RO001",
                CoreIdentificationId = coreIdentificationInHouse.Id,
                QuantityReceived = coreIdentificationInHouse.Quantity,
                WarehouseId = movingWarehouse.Id
            };
            recoveryOrderInHouse = _recoveryOrderService.CreateObject(recoveryOrderInHouse, _coreIdentificationService);
            
            recoveryOrderContact = new RecoveryOrder()
            {
                Code = "RO002",
                CoreIdentificationId = coreIdentificationContact.Id,
                QuantityReceived = coreIdentificationContact.Quantity,
                WarehouseId = movingWarehouse.Id
            };
            recoveryOrderContact = _recoveryOrderService.CreateObject(recoveryOrderContact, _coreIdentificationService);

            recoveryODInHouse1 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse1.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder1.Id
            };
            recoveryODInHouse1 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse1, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse2 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse2.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder1.Id
            };
            recoveryODInHouse2 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse2, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse3 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse3.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder2.Id
            };
            recoveryODInHouse3 = _recoveryOrderDetailService.CreateObject(recoveryODInHouse3, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODContact1 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact.Id,
                CoreIdentificationDetailId = coreIDContact1.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder3.Id
            };
            recoveryODContact1 = _recoveryOrderDetailService.CreateObject(recoveryODContact1, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODContact2 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact.Id,
                CoreIdentificationDetailId = coreIDContact2.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder3.Id
            };
            recoveryODContact2 = _recoveryOrderDetailService.CreateObject(recoveryODContact2, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODContact3 = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact.Id,
                CoreIdentificationDetailId = coreIDContact3.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder4.Id
            };
            recoveryODContact3 = _recoveryOrderDetailService.CreateObject(recoveryODContact3, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
        }

        public void PopulateRecoveryOrders2()
        {
            // status after this function
            // recoveryODContact2, recoveryODInHouse3 are rejected.
            // The rest are delivered back to localWarehouse to complete the batch.
            // New recovery orders are created to complete coreIdentificationInHouse and coreIdentificationContact
            _recoveryOrderService.ConfirmObject(recoveryOrderContact, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService,
                                      _recoveryAccessoryDetailService, _coreBuilderService, _stockMutationService, _itemService,
                                      _barringService, _warehouseItemService, _warehouseService);
            _recoveryOrderDetailService.DisassembleObject(recoveryODContact1);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact1);
            _recoveryOrderDetailService.WrapObject(recoveryODContact1, 20);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact1);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact1);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact1);
            _recoveryOrderDetailService.CWCGrindObject(recoveryODContact1);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact1);
            _recoveryOrderDetailService.PackageObject(recoveryODContact1);

            _recoveryOrderDetailService.DisassembleObject(recoveryODContact2);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact2);
            _recoveryOrderDetailService.WrapObject(recoveryODContact2, 20);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact2);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact2);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact2);
            _recoveryOrderDetailService.CWCGrindObject(recoveryODContact2);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact2);
            _recoveryOrderDetailService.PackageObject(recoveryODContact2);

            _recoveryOrderDetailService.DisassembleObject(recoveryODContact3);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact3);
            _recoveryOrderDetailService.WrapObject(recoveryODContact3, 20);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact3);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact3);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact3);
            _recoveryOrderDetailService.CWCGrindObject(recoveryODContact3);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact3);
            _recoveryOrderDetailService.PackageObject(recoveryODContact3);

            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse1);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODInHouse1);
            _recoveryOrderDetailService.WrapObject(recoveryODInHouse1, 20);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODInHouse1);
            _recoveryOrderDetailService.FaceOffObject(recoveryODInHouse1);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODInHouse1);
            _recoveryOrderDetailService.CWCGrindObject(recoveryODInHouse1);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODInHouse1);
            _recoveryOrderDetailService.PackageObject(recoveryODInHouse1);

            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse2);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODInHouse2);
            _recoveryOrderDetailService.WrapObject(recoveryODInHouse2, 20);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODInHouse2);
            _recoveryOrderDetailService.FaceOffObject(recoveryODInHouse2);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODInHouse2);
            _recoveryOrderDetailService.CWCGrindObject(recoveryODInHouse2);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODInHouse2);
            _recoveryOrderDetailService.PackageObject(recoveryODInHouse2);

            accessory1 = new RecoveryAccessoryDetail()
            {
                ItemId = itemAccessory1.Id,
                Quantity = 1,
                RecoveryOrderDetailId = recoveryODInHouse2.Id
            };
            _recoveryAccessoryDetailService.CreateObject(accessory1, _recoveryOrderService, _recoveryOrderDetailService,
                                                         _itemService, _itemTypeService, _warehouseItemService);
            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse3);

            _recoveryOrderDetailService.FinishObject(recoveryODContact1, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _barringService, _stockMutationService);
            _recoveryOrderDetailService.RejectObject(recoveryODContact2, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                       _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService,
                                                       _warehouseItemService, _barringService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODContact3, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _barringService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODInHouse1, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _barringService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODInHouse2, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService,
                                                       _recoveryOrderService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService,
                                                       _itemService, _warehouseItemService, _barringService, _stockMutationService);
            _recoveryOrderDetailService.RejectObject(recoveryODInHouse3, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                       _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService,
                                                       _warehouseItemService, _barringService, _stockMutationService);
        }

        public void PopulateStockAdjustment()
        {
            stockAdjustment = new StockAdjustment()
            {
                WarehouseId = movingWarehouse.Id,
                AdjustmentDate = DateTime.Now
            };
            stockAdjustment = _stockAdjustmentService.CreateObject(stockAdjustment, _warehouseService);

            stockAD1 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder2.UsedCoreItemId,
                Quantity = 1,
                StockAdjustmentId = stockAdjustment.Id,
                Price = 0
            };
            stockAD1 = _stockAdjustmentDetailService.CreateObject(stockAD1, _stockAdjustmentService, _itemService, _warehouseItemService);

            stockAD2 = new StockAdjustmentDetail()
            {
                ItemId = coreBuilder3.UsedCoreItemId,
                Quantity = 1,
                StockAdjustmentId = stockAdjustment.Id,
                Price = 0
            };
            stockAD2 = _stockAdjustmentDetailService.CreateObject(stockAD2, _stockAdjustmentService, _itemService, _warehouseItemService);
        }

        public void PopulateRecoveryOrders3()
        {
            _stockAdjustmentService.ConfirmObject(stockAdjustment, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService,
                                                  _itemService, _barringService, _warehouseItemService, _generalLedgerJournalService, _accountService, _closingService);
            
            recoveryOrderContact2 = new RecoveryOrder()
            {
                Code = "R002C2",
                CoreIdentificationId = coreIdentificationContact.Id,
                QuantityReceived = 1,
                WarehouseId = movingWarehouse.Id
            };
            _recoveryOrderService.CreateObject(recoveryOrderContact2, _coreIdentificationService);

            recoveryOrderInHouse2 = new RecoveryOrder()
            {
                Code = "R002H2",
                CoreIdentificationId = coreIdentificationInHouse.Id,
                QuantityReceived = 1,
                WarehouseId = movingWarehouse.Id
            };
            _recoveryOrderService.CreateObject(recoveryOrderInHouse2, _coreIdentificationService);

            recoveryODContact2b = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderContact2.Id,
                CoreIdentificationDetailId = coreIDContact2.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder3.Id
            };
            _recoveryOrderDetailService.CreateObject(recoveryODContact2b, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);

            recoveryODInHouse3b = new RecoveryOrderDetail()
            {
                RecoveryOrderId = recoveryOrderInHouse2.Id,
                CoreIdentificationDetailId = coreIDInHouse3.Id,
                Acc = "Y",
                CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                RollerBuilderId = rollerBuilder2.Id
            };
            _recoveryOrderDetailService.CreateObject(recoveryODInHouse3b, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
        }

        public void PopulateCoreIdentifications2()
        {
            _recoveryOrderService.ConfirmObject(recoveryOrderContact2, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService,
                                                _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService, _warehouseService);
            _recoveryOrderService.ConfirmObject(recoveryOrderInHouse2, DateTime.Today, _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService,
                                                _coreBuilderService, _stockMutationService, _itemService, _barringService, _warehouseItemService, _warehouseService);

            _recoveryOrderDetailService.DisassembleObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.WrapObject(recoveryODInHouse3b, 20);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.FaceOffObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.CWCGrindObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODInHouse3b);
            _recoveryOrderDetailService.PackageObject(recoveryODInHouse3b);

            _recoveryOrderDetailService.DisassembleObject(recoveryODContact2b);
            _recoveryOrderDetailService.StripAndGlueObject(recoveryODContact2b);
            _recoveryOrderDetailService.WrapObject(recoveryODContact2b, 20);
            _recoveryOrderDetailService.VulcanizeObject(recoveryODContact2b);
            _recoveryOrderDetailService.FaceOffObject(recoveryODContact2b);
            _recoveryOrderDetailService.ConventionalGrindObject(recoveryODContact2b);
            _recoveryOrderDetailService.CWCGrindObject(recoveryODContact2b);
            _recoveryOrderDetailService.PolishAndQCObject(recoveryODContact2b);
            _recoveryOrderDetailService.PackageObject(recoveryODContact2b);

            _recoveryOrderDetailService.FinishObject(recoveryODInHouse3b, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                     _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService, _warehouseItemService,
                                                     _barringService, _stockMutationService);
            _recoveryOrderDetailService.FinishObject(recoveryODContact2b, DateTime.Today, _coreIdentificationService, _coreIdentificationDetailService, _recoveryOrderService,
                                                     _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService, _itemService, _warehouseItemService,
                                                     _barringService, _stockMutationService);
        }

        public void PopulateRollerWarehouseMutation()
        {
            rollerWarehouseMutationContact = new RollerWarehouseMutation()
            {
                CoreIdentificationId = coreIdentificationContact.Id,
                Quantity = 3,
                WarehouseFromId = movingWarehouse.Id,
                WarehouseToId = localWarehouse.Id,
                MutationDate = DateTime.Today
            };
            _rollerWarehouseMutationService.CreateObject(rollerWarehouseMutationContact, _warehouseService, _coreIdentificationService);

            rwmDetailContact1 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationContact.Id,
                CoreIdentificationDetailId = coreIDContact1.Id,
                ItemId = (coreIDContact1.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODContact1.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODContact1.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailContact1, _rollerWarehouseMutationService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);
            
            rwmDetailContact2 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationContact.Id,
                CoreIdentificationDetailId = coreIDContact2.Id,
                ItemId = (coreIDContact2.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODContact2b.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODContact2b.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailContact2, _rollerWarehouseMutationService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            rwmDetailContact3 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationContact.Id,
                CoreIdentificationDetailId = coreIDContact3.Id,
                ItemId = (coreIDContact3.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODContact3.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODContact3.RollerBuilderId).Id
            };

            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailContact3, _rollerWarehouseMutationService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            _rollerWarehouseMutationService.ConfirmObject(rollerWarehouseMutationContact, DateTime.Today, _rollerWarehouseMutationDetailService, _itemService,
                                                          _barringService, _warehouseItemService, _stockMutationService, _coreIdentificationDetailService, _coreIdentificationService);
            
            rollerWarehouseMutationInHouse = new RollerWarehouseMutation()
            {
                CoreIdentificationId = coreIdentificationInHouse.Id,
                Quantity = 3,
                WarehouseFromId = movingWarehouse.Id,
                WarehouseToId = localWarehouse.Id,
                MutationDate = DateTime.Today
            };
            _rollerWarehouseMutationService.CreateObject(rollerWarehouseMutationInHouse, _warehouseService, _coreIdentificationService);

            rwmDetailInHouse1 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse1.Id,
                ItemId = (coreIDInHouse1.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODInHouse1.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODInHouse1.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailInHouse1, _rollerWarehouseMutationService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            rwmDetailInHouse2 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse2.Id,
                ItemId = (coreIDInHouse2.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODInHouse2.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODInHouse2.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailInHouse2, _rollerWarehouseMutationService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            rwmDetailInHouse3 = new RollerWarehouseMutationDetail()
            {
                RollerWarehouseMutationId = rollerWarehouseMutationInHouse.Id,
                CoreIdentificationDetailId = coreIDInHouse3.Id,
                ItemId = (coreIDInHouse3.MaterialCase == Core.Constants.Constant.MaterialCase.Used) ?
                         _rollerBuilderService.GetRollerUsedCore(recoveryODInHouse3b.RollerBuilderId).Id :
                          _rollerBuilderService.GetRollerNewCore(recoveryODInHouse3b.RollerBuilderId).Id
            };
            _rollerWarehouseMutationDetailService.CreateObject(rwmDetailInHouse3, _rollerWarehouseMutationService,
                                                               _coreIdentificationDetailService, _itemService, _warehouseItemService);

            _rollerWarehouseMutationService.ConfirmObject(rollerWarehouseMutationInHouse, DateTime.Today, _rollerWarehouseMutationDetailService, _itemService,
                                                          _barringService, _warehouseItemService, _stockMutationService, _coreIdentificationDetailService, _coreIdentificationService);
        }

        public void PopulateBarring()
        {
            bargeneric = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Generic",
                UoMId = Pcs.Id,
                Sku = "BGEN",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(bargeneric, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barleft1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Left 1",
                UoMId = Pcs.Id,
                Sku = "BL1",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(barleft1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barleft2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Left 2",
                UoMId = Pcs.Id,
                Sku = "BL2",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(barleft2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barright1 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Right 1",
                UoMId = Pcs.Id,
                Sku = "BR1",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(barright1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barright2 = new Item()
            {
                ItemTypeId = typeBar.Id,
                Category = "bar",
                Name = "Bar Right 2",
                UoMId = Pcs.Id,
                Sku = "BR2",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(barright2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            blanket1 = new Item()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
                Name = "Blanket1",
                UoMId = Pcs.Id,
                Sku = "BLK1",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(blanket1, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            blanket2 = new Item()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
                Name = "Blanket2",
                UoMId = Pcs.Id,
                Sku = "BLK2",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(blanket2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            blanket3 = new Item()
            {
                ItemTypeId = typeBlanket.Id,
                Category = "Blanket",
                Name = "Blanket3",
                UoMId = Pcs.Id,
                Sku = "BLK3",
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _itemService.CreateObject(blanket3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barring1 = new Barring()
            {
                ItemTypeId = typeBarring.Id,
                Category = "Barring",
                Name = "Barring1",
                UoMId = Pcs.Id,
                Sku = "BRG1",
                RollNo = "BRG1_123",
                AC = 10,
                AR = 15,
                BlanketItemId = blanket1.Id,
                LeftBarItemId = bargeneric.Id,
                RightBarItemId = bargeneric.Id,
                ContactId = contact.Id,
                KS = 1,
                thickness = 1,
                MachineId = machine.Id,
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _barringService.CreateObject(barring1, _barringService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barring2 = new Barring()
            {
                ItemTypeId = typeBarring.Id,
                Category = "Barring",
                Name = "Barring2",
                UoMId = Pcs.Id,
                Sku = "BRG2",
                RollNo = "BRG2_123",
                AC = 3,
                AR = 5,
                BlanketItemId = blanket2.Id,
                LeftBarItemId = barleft1.Id,
                RightBarItemId = barright1.Id,
                ContactId = contact.Id,
                KS = 1,
                thickness = 1,
                MachineId = machine.Id,
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _barringService.CreateObject(barring2, _barringService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            barring3 = new Barring()
            {
                ItemTypeId = typeBarring.Id,
                Category = "Barring",
                Name = "Barring3",
                UoMId = Pcs.Id,
                Sku = "BRG3",
                RollNo = "BRG3_123",
                AC = 7,
                AR = 10,
                BlanketItemId = blanket3.Id,
                LeftBarItemId = barleft2.Id,
                RightBarItemId = barright2.Id,
                ContactId = contact.Id,
                KS = 1,
                thickness = 1,
                MachineId = machine.Id,
                SellingPrice = 20000,
                AvgPrice = 10000
            };
            _barringService.CreateObject(barring3, _barringService, _uomService, _itemService, _itemTypeService, _contactService,
                                         _machineService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);

            StockAdjustment sa = new StockAdjustment() { WarehouseId = localWarehouse.Id, AdjustmentDate = DateTime.Today, Description = "Bar Related Adjustment" };
            _stockAdjustmentService.CreateObject(sa, _warehouseService);
            StockAdjustmentDetail sadBarGeneric = new StockAdjustmentDetail() { ItemId = bargeneric.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarGeneric, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarleft1 = new StockAdjustmentDetail() { ItemId = barleft1.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarleft1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarleft2 = new StockAdjustmentDetail() { ItemId = barleft1.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarleft2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarright1 = new StockAdjustmentDetail() { ItemId = barright1.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarright1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBarright2 = new StockAdjustmentDetail() { ItemId = barright2.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBarright2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket1 = new StockAdjustmentDetail() { ItemId = blanket1.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket2 = new StockAdjustmentDetail() { ItemId = blanket2.Id, Quantity = 20, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadRollBlanket3 = new StockAdjustmentDetail() { ItemId = blanket3.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadRollBlanket3, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket1 = new StockAdjustmentDetail() { ItemId = blanket1.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBlanket1, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket2 = new StockAdjustmentDetail() { ItemId = blanket2.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBlanket2, _stockAdjustmentService, _itemService, _warehouseItemService);
            StockAdjustmentDetail sadBlanket3 = new StockAdjustmentDetail() { ItemId = blanket3.Id, Quantity = 10, StockAdjustmentId = sa.Id };
            _stockAdjustmentDetailService.CreateObject(sadBlanket3, _stockAdjustmentService, _itemService, _warehouseItemService);

            _stockAdjustmentService.ConfirmObject(sa, DateTime.Today, _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService,
                                                  _generalLedgerJournalService, _accountService, _closingService);

        }

        public void PopulateBarringOrders()
        {
            barringOrderContact = new BarringOrder()
            {
                ContactId = contact.Id,
                QuantityReceived = 4,
                Code = "BO0001",
                WarehouseId = localWarehouse.Id
            };
            _barringOrderService.CreateObject(barringOrderContact);

            barringODContact1 = new BarringOrderDetail()
            {
                BarringId = barring1.Id,
                BarringOrderId = barringOrderContact.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODContact1, _barringOrderService, _barringService);

            barringODContact2 = new BarringOrderDetail()
            {
                BarringId = barring1.Id,
                BarringOrderId = barringOrderContact.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODContact2, _barringOrderService, _barringService);

            barringODContact3 = new BarringOrderDetail()
            {
                BarringId = barring2.Id,
                BarringOrderId = barringOrderContact.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODContact3, _barringOrderService, _barringService);

            barringODContact4 = new BarringOrderDetail()
            {
                BarringId = barring2.Id,
                BarringOrderId = barringOrderContact.Id,
                IsBarRequired = true
            };
            _barringOrderDetailService.CreateObject(barringODContact4, _barringOrderService, _barringService);
        }

        public void PopulateSales()
        {
            salesOrder1 = new SalesOrder()
            {
                SalesDate = DateTime.Today,
                ContactId = contact.Id
            };
            _salesOrderService.CreateObject(salesOrder1, _contactService);

            salesOrder2 = new SalesOrder()
            {
                SalesDate = DateTime.Today,
                ContactId = contact.Id
            };
            _salesOrderService.CreateObject(salesOrder2, _contactService);

            salesOrder3 = new SalesOrder()
            {
                SalesDate = DateTime.Today,
                ContactId = contact.Id
            };
            _salesOrderService.CreateObject(salesOrder3, _contactService);

            salesOD1a = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder1.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
                Price = 52000
            };
            _salesOrderDetailService.CreateObject(salesOD1a, _salesOrderService, _itemService);

            salesOD1b = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder1.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
                Price = 22000
            };
            _salesOrderDetailService.CreateObject(salesOD1b, _salesOrderService, _itemService);

            salesOD2a = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder2.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
                Price = 51000
            };
            _salesOrderDetailService.CreateObject(salesOD2a, _salesOrderService, _itemService);

            salesOD2b = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder2.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
                Price = 21000
            };
            _salesOrderDetailService.CreateObject(salesOD2b, _salesOrderService, _itemService);

            salesOD3a = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder3.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
                Price = 53000
            };
            _salesOrderDetailService.CreateObject(salesOD3a, _salesOrderService, _itemService);

            salesOD3b = new SalesOrderDetail()
            {
                SalesOrderId = salesOrder3.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
                Price = 23000
            };
            _salesOrderDetailService.CreateObject(salesOD3b, _salesOrderService, _itemService);

            _salesOrderService.ConfirmObject(salesOrder1, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            _salesOrderService.ConfirmObject(salesOrder2, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);
            _salesOrderService.ConfirmObject(salesOrder3, DateTime.Today, _salesOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService);

            deliveryOrder1 = new DeliveryOrder()
            {
                DeliveryDate = DateTime.Today,
                SalesOrderId = salesOrder1.Id,
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(deliveryOrder1, _salesOrderService, _warehouseService);

            deliveryOrder2 = new DeliveryOrder()
            {
                DeliveryDate = DateTime.Today,
                SalesOrderId = salesOrder2.Id,
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(deliveryOrder2, _salesOrderService, _warehouseService);

            deliveryOrder3 = new DeliveryOrder()
            {
                DeliveryDate = DateTime.Today,
                SalesOrderId = salesOrder3.Id,
                WarehouseId = localWarehouse.Id
            };
            _deliveryOrderService.CreateObject(deliveryOrder3, _salesOrderService, _warehouseService);

            deliveryOD1a = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder1.Id,
                SalesOrderDetailId = salesOD1a.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD1a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD1b = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder1.Id,
                SalesOrderDetailId = salesOD1b.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2,
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD1b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD2a = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder2.Id,
                SalesOrderDetailId = salesOD2a.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2,
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD2a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD2b = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder2.Id,
                SalesOrderDetailId = salesOD2b.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD2b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD3a = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder3.Id,
                SalesOrderDetailId = salesOD3a.Id,
                ItemId = itemAccessory1.Id,
                Quantity = 2
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD3a, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            deliveryOD3b = new DeliveryOrderDetail()
            {
                DeliveryOrderId = deliveryOrder3.Id,
                SalesOrderDetailId = salesOD3b.Id,
                ItemId = itemAccessory2.Id,
                Quantity = 2
            };
            _deliveryOrderDetailService.CreateObject(deliveryOD3b, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService);

            _deliveryOrderService.ConfirmObject(deliveryOrder1, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _barringService, _warehouseItemService);
            _deliveryOrderService.ConfirmObject(deliveryOrder2, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _barringService, _warehouseItemService);
            _deliveryOrderService.ConfirmObject(deliveryOrder3, DateTime.Today, _deliveryOrderDetailService, _salesOrderService, _salesOrderDetailService, _stockMutationService,
                                                _itemService, _barringService, _warehouseItemService);

            salesInvoice1 = new SalesInvoice()
            {
                DeliveryOrderId = deliveryOrder1.Id,
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                IsTaxable = false,
                Discount = 0,
            };
            _salesInvoiceService.CreateObject(salesInvoice1, _deliveryOrderService);

            salesInvoice2 = new SalesInvoice()
            {
                DeliveryOrderId = deliveryOrder2.Id,
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                IsTaxable = false,
                Discount = 0,
            };
            _salesInvoiceService.CreateObject(salesInvoice2, _deliveryOrderService);

            salesInvoice3 = new SalesInvoice()
            {
                DeliveryOrderId = deliveryOrder3.Id,
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7),
                IsTaxable = false,
                Discount = 0,
            };
            _salesInvoiceService.CreateObject(salesInvoice3, _deliveryOrderService);

            salesID1a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice1.Id,
                DeliveryOrderDetailId = deliveryOD1a.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID1a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID1b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice1.Id,
                DeliveryOrderDetailId = deliveryOD1b.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID1b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID2a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice2.Id,
                DeliveryOrderDetailId = deliveryOD2a.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID2a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID2b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice2.Id,
                DeliveryOrderDetailId = deliveryOD2b.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID2b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID3a = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice3.Id,
                DeliveryOrderDetailId = deliveryOD3a.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID3a, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            salesID3b = new SalesInvoiceDetail()
            {
                SalesInvoiceId = salesInvoice3.Id,
                DeliveryOrderDetailId = deliveryOD3b.Id,
                Quantity = 2,
            };
            _salesInvoiceDetailService.CreateObject(salesID3b, _salesInvoiceService, _salesOrderDetailService, _deliveryOrderDetailService);

            _salesInvoiceService.ConfirmObject(salesInvoice1, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService);
            _salesInvoiceService.ConfirmObject(salesInvoice2, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService);
            _salesInvoiceService.ConfirmObject(salesInvoice3, DateTime.Today, _salesInvoiceDetailService, _salesOrderService, _deliveryOrderService,
                                               _deliveryOrderDetailService, _receivableService);

            receiptVoucher1 = new ReceiptVoucher()
            {
                CashBankId = cashBank1.Id,
                ContactId = contact.Id,
                DueDate = DateTime.Today.AddDays(6),
                IsGBCH = false,
                ReceiptDate = DateTime.Today,
                TotalAmount = salesInvoice1.AmountReceivable
            };
            _receiptVoucherService.CreateObject(receiptVoucher1, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            receiptVoucher2 = new ReceiptVoucher()
            {
                CashBankId = cashBank1.Id,
                ContactId = contact.Id,
                DueDate = DateTime.Today.AddDays(6),
                IsGBCH = false,
                ReceiptDate = DateTime.Today,
                TotalAmount = salesInvoice2.AmountReceivable
            };
            _receiptVoucherService.CreateObject(receiptVoucher2, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            receiptVoucher3 = new ReceiptVoucher()
            {
                CashBankId = cashBank1.Id,
                ContactId = contact.Id,
                DueDate = DateTime.Today.AddDays(6),
                IsGBCH = false,
                ReceiptDate = DateTime.Today,
                TotalAmount = salesInvoice3.AmountReceivable
            };
            _receiptVoucherService.CreateObject(receiptVoucher3, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);

            receiptVD1a = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher1.Id,
                Amount = salesID1a.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice1.Id).Id,
            };
            _receiptVoucherDetailService.CreateObject(receiptVD1a, _receiptVoucherService, _cashBankService, _receivableService);

            receiptVD1b = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher1.Id,
                Amount = salesID1b.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice1.Id).Id,
            };
            _receiptVoucherDetailService.CreateObject(receiptVD1b, _receiptVoucherService, _cashBankService, _receivableService);

            receiptVD2a = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher2.Id,
                Amount = salesID2a.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice2.Id).Id,
            };
            _receiptVoucherDetailService.CreateObject(receiptVD2a, _receiptVoucherService, _cashBankService, _receivableService);

            receiptVD2b = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher2.Id,
                Amount = salesID2b.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice2.Id).Id,
            };
            _receiptVoucherDetailService.CreateObject(receiptVD2b, _receiptVoucherService, _cashBankService, _receivableService);

            receiptVD3a = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher3.Id,
                Amount = salesID3a.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice3.Id).Id,
            };
            _receiptVoucherDetailService.CreateObject(receiptVD3a, _receiptVoucherService, _cashBankService, _receivableService);

            receiptVD3b = new ReceiptVoucherDetail()
            {
                ReceiptVoucherId = receiptVoucher3.Id,
                Amount = salesID3b.Amount,
                ReceivableId = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice3.Id).Id,
            };
            _receiptVoucherDetailService.CreateObject(receiptVD3b, _receiptVoucherService, _cashBankService, _receivableService);

            _receiptVoucherService.ConfirmObject(receiptVoucher1, DateTime.Now, _receiptVoucherDetailService, _cashBankService, _receivableService,
                                                 _cashMutationService, _generalLedgerJournalService, _accountService, _closingService);
            _receiptVoucherService.ConfirmObject(receiptVoucher2, DateTime.Now, _receiptVoucherDetailService, _cashBankService, _receivableService,
                                                 _cashMutationService, _generalLedgerJournalService, _accountService, _closingService);
            _receiptVoucherService.ConfirmObject(receiptVoucher3, DateTime.Now, _receiptVoucherDetailService, _cashBankService, _receivableService,
                                                 _cashMutationService, _generalLedgerJournalService, _accountService, _closingService);
        }

        public void PopulateCashSales()
        {
            TimeSpan salesDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan dueDate = new TimeSpan(3, 0, 0, 0);

            // Cash without Discount & Tax
            cashSalesInvoice1 = new CashSalesInvoice()
            {
                SalesDate = DateTime.Today.Subtract(salesDate),
                WarehouseId = localWarehouse.Id,
                CashBankId = cashBank1.Id,
                DueDate = DateTime.Today.Subtract(dueDate),
            };
            _cashSalesInvoiceService.CreateObject(cashSalesInvoice1, _warehouseService, _cashBankService);

            // Cash with Discount & Tax
            cashSalesInvoice2 = new CashSalesInvoice()
            {
                SalesDate = DateTime.Today.Subtract(salesDate),
                WarehouseId = localWarehouse.Id,
                CashBankId = cashBank1.Id,
                DueDate = DateTime.Today.Subtract(dueDate),
                Discount = 25,
                Tax = 10,
            };
            _cashSalesInvoiceService.CreateObject(cashSalesInvoice2, _warehouseService, _cashBankService);

            // Bank without Discount & Tax
            cashSalesInvoice3 = new CashSalesInvoice()
            {
                SalesDate = DateTime.Today.Subtract(salesDate),
                WarehouseId = localWarehouse.Id,
                CashBankId = cashBank2.Id,
                DueDate = DateTime.Today.Subtract(dueDate),
            };
            _cashSalesInvoiceService.CreateObject(cashSalesInvoice3, _warehouseService, _cashBankService);

            cashSalesID1a = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = cashSalesInvoice1.Id,
                Quantity = 2,
                ItemId = itemAccessory1.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(cashSalesID1a, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);

            cashSalesID2a = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = cashSalesInvoice2.Id,
                Quantity = 2,
                ItemId = itemAccessory1.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(cashSalesID2a, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);

            cashSalesID3a = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = cashSalesInvoice3.Id,
                Quantity = 2,
                ItemId = itemAccessory1.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(cashSalesID3a, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);

            cashSalesID2b = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = cashSalesInvoice2.Id,
                Quantity = 1,
                ItemId = itemAccessory2.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(cashSalesID2b, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);

            _cashSalesInvoiceService.ConfirmObject(cashSalesInvoice1, cashSalesInvoice1.SalesDate, 10, 0, _cashSalesInvoiceDetailService, _contactService, _priceMutationService, _receivableService, _cashSalesInvoiceService, _warehouseItemService, _warehouseService,
                                                   _itemService, _barringService, _stockMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);
            _cashSalesInvoiceService.ConfirmObject(cashSalesInvoice2, cashSalesInvoice2.SalesDate, 0, 0, _cashSalesInvoiceDetailService, _contactService, _priceMutationService, _receivableService, _cashSalesInvoiceService, _warehouseItemService, _warehouseService,
                                                   _itemService, _barringService, _stockMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);
            _cashSalesInvoiceService.ConfirmObject(cashSalesInvoice3, cashSalesInvoice3.SalesDate, 0, 0, _cashSalesInvoiceDetailService, _contactService, _priceMutationService, _receivableService, _cashSalesInvoiceService, _warehouseItemService, _warehouseService,
                                                   _itemService, _barringService, _stockMutationService, _cashBankService, _generalLedgerJournalService, _accountService, _closingService);

            _cashSalesInvoiceService.PaidObject(cashSalesInvoice1, 40000, 50000, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService,
                                                _generalLedgerJournalService, _accountService, _closingService);
            _cashSalesInvoiceService.PaidObject(cashSalesInvoice2, cashSalesInvoice2.Total, 0, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService,
                                                _generalLedgerJournalService, _accountService, _closingService);
            _cashSalesInvoiceService.PaidObject(cashSalesInvoice3, cashSalesInvoice3.Total - 50000, 50000, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService,
                                                _generalLedgerJournalService, _accountService, _closingService);
        }

        public void PopulateValidComb()
        {
            thisMonthClosing = new Closing()
            {
                BeginningPeriod = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                EndDatePeriod = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)),
                Period = DateTime.Today.Month,
                YearPeriod = DateTime.Today.Year,
            };
            _closingService.CreateObject(thisMonthClosing, _accountService, _validCombService);

            _closingService.CloseObject(thisMonthClosing, _accountService, _generalLedgerJournalService, _validCombService);
        }
    }
}
