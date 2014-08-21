﻿using System;
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

namespace TestValidation
{
    public class CashSalesBuilder
    {
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
        public IQuantityPricingService _quantityPricingService;
        public IContactGroupService _contactGroupService;
        public IRetailSalesInvoiceService _retailSalesInvoiceService;
        public IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService;
        public ICashSalesInvoiceService _cashSalesInvoiceService;
        public ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService;
        public ICashSalesReturnService _cashSalesReturnService;
        public ICashSalesReturnDetailService _cashSalesReturnDetailService;

        public ContactGroup baseGroup, group2;
        public ItemType typeAccessory, typeBar, typeBarring, typeBearing, typeBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        public RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public UoM Pcs, Boxes, Tubs;

        public Warehouse localWarehouse;
        public Contact baseContact, contact, contact2, contact3;
        public Item blanket1, blanket2, blanket3;
        public StockAdjustment stockAdjustment;
        public StockAdjustmentDetail stockAD1, stockAD2;
        public CashBank cashBank, cashBank2;
        public CashBankAdjustment cashBankAdjustment, cashBankAdjustment2;

        public PaymentVoucher rv;
        public PaymentVoucherDetail rvd1, rvd2, rvd3;
        public CashSalesInvoice csi1, csi2, csi3;
        public CashSalesInvoiceDetail csid1, csid2, csid3, csid4;
        public QuantityPricing qp1, qp2, qp3;
        public CashSalesReturn csr1, csr2, csr3;
        public CashSalesReturnDetail csrd1, csrd2, csrd3, csrd4;

        public CashSalesBuilder()
        {
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
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(), new QuantityPricingValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _retailSalesInvoiceService = new RetailSalesInvoiceService(new RetailSalesInvoiceRepository(), new RetailSalesInvoiceValidator());
            _retailSalesInvoiceDetailService = new RetailSalesInvoiceDetailService(new RetailSalesInvoiceDetailRepository(), new RetailSalesInvoiceDetailValidator());
            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesInvoiceDetailService = new CashSalesInvoiceDetailService(new CashSalesInvoiceDetailRepository(), new CashSalesInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _cashSalesReturnDetailService = new CashSalesReturnDetailService(new CashSalesReturnDetailRepository(), new CashSalesReturnDetailValidator());

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
            group2 = _contactGroupService.CreateObject("Pedagang", "Group Pedagang", false);
            baseContact = _contactService.CreateObject(Core.Constants.Constant.BaseContact, "BaseAddr", "BaseNo", "BasePIC", "BasePICNo", "BaseEmail", _contactGroupService);
        }

        public void PopulateData()
        {
            PopulateMasterData();
            PopulateCashSalesData();
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
                SellingPrice = 10000,
                AvgPrice = 10000
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
                SellingPrice = 20000,
                AvgPrice = 20000
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
                SellingPrice = 30000,
                AvgPrice = 30000
            };

            blanket3 = _itemService.CreateObject(blanket3, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
            _itemService.AdjustQuantity(blanket3, 100000);
            _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(localWarehouse.Id, blanket3.Id), 100000);

            qp1 = new QuantityPricing()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Blanket").Id,
                Discount = 10,
                MinQuantity = 20,
                MaxQuantity = 40,
                IsInfiniteMaxQuantity = false
            };
            _quantityPricingService.CreateObject(qp1, _itemTypeService);

            qp2 = new QuantityPricing()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Blanket").Id,
                Discount = 25,
                MinQuantity = 30,
                MaxQuantity = 50,
                IsInfiniteMaxQuantity = false
            };
            _quantityPricingService.CreateObject(qp2, _itemTypeService);

            qp3 = new QuantityPricing()
            {
                ItemTypeId = _itemTypeService.GetObjectByName("Blanket").Id,
                Discount = 50,
                MinQuantity = 51,
                IsInfiniteMaxQuantity = true
            };
            _quantityPricingService.CreateObject(qp3, _itemTypeService);

            contact = new Contact()
            {
                Name = "President of Indonesia",
                Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                ContactNo = "021 3863777",
                PIC = "Mr. President",
                PICContactNo = "021 3863777",
                Email = "random@ri.gov.au"
            };
            _contactService.CreateObject(contact, _contactGroupService);

            contact2 = new Contact()
            {
                Name = "Wakil President of Indonesia",
                Address = "Istana Negara Jl. Veteran No. 16 Jakarta Pusat",
                ContactNo = "021 3863777",
                PIC = "Mr. Wakil President",
                PICContactNo = "021 3863777",
                Email = "random@ri.gov.au"
            };
            _contactService.CreateObject(contact2, _contactGroupService);

            contact3 = new Contact()
            {
                Name = "Roma Irama",
                Address = "Istana Negara Jl. Veteran No.20 Jakarta Pusat",
                ContactNo = "021 5551234",
                PIC = "Mr. King",
                PICContactNo = "021 5551234",
                Email = "raja@dangdut.com",
                ContactGroupId = group2.Id,
            };
            _contactService.CreateObject(contact3, _contactGroupService);

            cashBank = new CashBank()
            {
                Name = "Kontan",
                Description = "Bayar kontan",
                IsBank = false
            };
            _cashBankService.CreateObject(cashBank);

            cashBank2 = new CashBank()
            {
                Name = "Rekening BRI",
                Description = "Untuk cashflow",
                IsBank = true
            };
            _cashBankService.CreateObject(cashBank2);

            cashBankAdjustment = new CashBankAdjustment()
            {
                CashBankId = cashBank.Id,
                Amount = 1000000000,
                AdjustmentDate = DateTime.Today,
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment, _cashBankService);

            cashBankAdjustment2 = new CashBankAdjustment()
            {
                CashBankId = cashBank2.Id,
                Amount = 1000000000,
                AdjustmentDate = DateTime.Today,
            };
            _cashBankAdjustmentService.CreateObject(cashBankAdjustment2, _cashBankService);

            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment, DateTime.Now, _cashMutationService, _cashBankService);
            _cashBankAdjustmentService.ConfirmObject(cashBankAdjustment2, DateTime.Now, _cashMutationService, _cashBankService);
        
        }

        public void PopulateCashSalesData()
        {
            TimeSpan salesDate = new TimeSpan(10, 0, 0, 0);
            TimeSpan dueDate = new TimeSpan(3, 0, 0 ,0);
            
            // Cash without Discount & Tax
            csi1 = new CashSalesInvoice()
            {
                SalesDate = DateTime.Today.Subtract(salesDate),
                WarehouseId = localWarehouse.Id,
                CashBankId = cashBank.Id,
                DueDate = DateTime.Today.Subtract(dueDate),
            };
            _cashSalesInvoiceService.CreateObject(csi1, _warehouseService);

            // Cash with Discount & Tax
            csi2 = new CashSalesInvoice()
            {
                SalesDate = DateTime.Today.Subtract(salesDate),
                WarehouseId = localWarehouse.Id,
                CashBankId = cashBank.Id,
                DueDate = DateTime.Today.Subtract(dueDate),
                Discount = 25,
                Tax = 10,
            };
            _cashSalesInvoiceService.CreateObject(csi2, _warehouseService);

            // Bank without Discount & Tax
            csi3 = new CashSalesInvoice()
            {
                SalesDate = DateTime.Today.Subtract(salesDate),
                WarehouseId = localWarehouse.Id,
                CashBankId = cashBank2.Id,
                DueDate = DateTime.Today.Subtract(dueDate),
            };
            _cashSalesInvoiceService.CreateObject(csi3, _warehouseService);

            csid1 = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = csi1.Id,
                Quantity = 100,
                ItemId = blanket1.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(csid1, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);

            csid2 = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = csi2.Id,
                Quantity = 30,
                ItemId = blanket2.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(csid2, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);

            csid3 = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = csi3.Id,
                Quantity = 10,
                ItemId = blanket3.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(csid3, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);

            csid4 = new CashSalesInvoiceDetail()
            {
                CashSalesInvoiceId = csi2.Id,
                Quantity = 10,
                ItemId = blanket3.Id,
            };
            _cashSalesInvoiceDetailService.CreateObject(csid4, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);


            _cashSalesInvoiceService.ConfirmObject(csi1, csi1.SalesDate, 0, 0, _cashSalesInvoiceDetailService, _contactService, _priceMutationService,_receivableService,_cashSalesInvoiceService,_warehouseItemService,_warehouseService,_itemService,_barringService,_stockMutationService,_cashBankService);
            _cashSalesInvoiceService.ConfirmObject(csi2, csi2.SalesDate, 0, 0, _cashSalesInvoiceDetailService, _contactService, _priceMutationService, _receivableService, _cashSalesInvoiceService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService, _cashBankService);
            _cashSalesInvoiceService.ConfirmObject(csi3, csi3.SalesDate, 0, 0, _cashSalesInvoiceDetailService, _contactService, _priceMutationService, _receivableService, _cashSalesInvoiceService, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService, _cashBankService);

            _cashSalesInvoiceService.PaidObject(csi1, 200000, 50000, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService);
            _cashSalesInvoiceService.PaidObject(csi2, csi2.Total, 50000, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService);
            _cashSalesInvoiceService.PaidObject(csi3, csi3.Total, 50000, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, _contactService, _cashMutationService, _cashSalesReturnService);
        
            // --------- CashSalesReturn -------------

            csr1 = new CashSalesReturn()
            {
                ReturnDate = DateTime.Now,
                CashSalesInvoiceId = csi1.Id,
                CashBankId = csi1.CashBankId,
            };
            _cashSalesReturnService.CreateObject(csr1, _cashSalesInvoiceService, _cashBankService);

            csrd1 = new CashSalesReturnDetail()
            {
                CashSalesReturnId = csr1.Id,
                CashSalesInvoiceDetailId = csid1.Id,
                Quantity = 50,
            };
            _cashSalesReturnDetailService.CreateObject(csrd1, _cashSalesReturnService, _cashSalesInvoiceDetailService);

            _cashSalesReturnService.ConfirmObject(csr1, DateTime.Now, 50000, _cashSalesReturnDetailService, _contactService, _cashSalesInvoiceService,
                                                  _cashSalesInvoiceDetailService, _priceMutationService, _payableService,
                                                  _cashSalesReturnService, _warehouseItemService, _warehouseService,
                                                  _itemService, _barringService, _stockMutationService);

            _cashSalesReturnService.PaidObject(csr1, /*50000,*/ _cashBankService, _payableService, _paymentVoucherService,
                                               _paymentVoucherDetailService, _contactService, _cashMutationService);

            //_cashSalesReturnService.UnpaidObject(csr1, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService);

            
        }

        
    }
}