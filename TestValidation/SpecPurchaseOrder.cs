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
        IStockMutationService stockMutationService;
        IUoMService _uomService;
        IBarringService _barringService;
        IItemTypeService _itemTypeService;
        IWarehouseItemService _warehouseItemService;
        IWarehouseService _warehouseService;

        IPriceMutationService _priceMutationService;
        IContactGroupService _contactGroupService;

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
                stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
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
                itemService.AdjustQuantity(item1, 1000);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item1.Id), 1000);

                item2 = new Item()
                {
                    ItemTypeId = _itemTypeService.GetObjectByName("Item").Id,
                    Name = "Buku Gambar",
                    Category = "Item",
                    Sku = "bg123",
                    UoMId = Pcs.Id
                };
                itemService.CreateObject(item2, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                itemService.AdjustQuantity(item2, 1000);
                _warehouseItemService.AdjustQuantity(_warehouseItemService.FindOrCreateObject(warehouse.Id, item2.Id), 1000);

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
                        newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, stockMutationService, itemService, _barringService, _warehouseItemService, poService);
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
                    newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, stockMutationService, itemService, _barringService, _warehouseItemService, poService);
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
                        newPO = poService.ConfirmObject(newPO, DateTime.Today, poDetailService, stockMutationService, itemService, _barringService, _warehouseItemService, poService);
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
                        stockMutationService.GetObjectsByItemId(item1.Id).Count().should_be(1);
                        stockMutationService.GetObjectsByItemId(item2.Id).Count().should_be(1);
                    };
                };
            };
        }
    }
}