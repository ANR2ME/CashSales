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
                _itemService.AdjustQuantity(item_batiktulis, 1000);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item_batiktulis.Id), 1000);

                item_busway = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Busway",
                    Category = "Untuk disumbangkan bagi kebutuhan DKI Jakarta",
                    Sku = "DKI002",
                    UoMId = Pcs.Id
                };
                item_busway = _itemService.CreateObject(item_busway, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                _itemService.AdjustQuantity(item_busway, 200);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item_busway.Id), 200);

                item_botolaqua = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Botol Aqua",
                    Category = "Minuman",
                    Sku = "DKI003",
                    UoMId = Pcs.Id
                };
                item_botolaqua = _itemService.CreateObject(item_botolaqua, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                _itemService.AdjustQuantity(item_botolaqua, 20000);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item_botolaqua.Id), 20000);

                purchaseOrder1 = _purchaseOrderService.CreateObject(contact.Id, new DateTime(2014, 07, 09), _contactService);
                purchaseOrder2 = _purchaseOrderService.CreateObject(contact.Id, new DateTime(2014, 04, 09), _contactService);
                purchaseOrderDetail_batiktulis_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_batiktulis.Id, 500, 2000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_busway_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_busway.Id, 91, 800000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_botolaqua_so1 = _purchaseOrderDetailService.CreateObject(purchaseOrder1.Id, item_botolaqua.Id, 2000, 5000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_batiktulis_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_batiktulis.Id, 40, 2000500, _purchaseOrderService, _itemService);
                purchaseOrderDetail_busway_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_busway.Id, 3, 810000000, _purchaseOrderService, _itemService);
                purchaseOrderDetail_botolaqua_so2 = _purchaseOrderDetailService.CreateObject(purchaseOrder2.Id, item_botolaqua.Id, 340, 5500, _purchaseOrderService, _itemService);
                purchaseOrder1 = _purchaseOrderService.ConfirmObject(purchaseOrder1, DateTime.Today, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService, _purchaseOrderService);
                purchaseOrder2 = _purchaseOrderService.ConfirmObject(purchaseOrder2, DateTime.Today, _purchaseOrderDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService, _purchaseOrderService);
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