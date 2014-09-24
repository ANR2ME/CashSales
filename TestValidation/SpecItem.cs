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

    public class SpecItem: nspec
    {
        DataBuilder d;
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                //d.baseGroup = d._contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

                d.Pcs = new UoM()
                {
                    Name = "Pcs"
                };
                d._uomService.CreateObject(d.Pcs);

                d.Boxes = new UoM()
                {
                    Name = "Boxes"
                };
                d._uomService.CreateObject(d.Boxes);

                d.Tubs = new UoM()
                {
                    Name = "Tubs"
                };
                d._uomService.CreateObject(d.Tubs);

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoMId = d.Pcs.Id
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

            }
        }

        void item_validation()
        {
        
            it["validates_item"] = () =>
            {
                d.item.Errors.Count().should_be(0);
            };

            it["item_with_no_name"] = () =>
            {
                Item nonameitem = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1002",
                    Name = "     ",
                    Category = "ABC222",
                    UoMId = d.Pcs.Id
                };
                nonameitem = d._itemService.CreateObject(nonameitem, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);
                nonameitem.Errors.Count().should_not_be(0);
            };

            it["item_with_same_sku"] = () =>
            {
                Item sameskuitem = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "BBC",
                    Category = "ABC222",
                    UoMId = d.Pcs.Id
                };
                sameskuitem = d._itemService.CreateObject(sameskuitem, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);
                sameskuitem.Errors.Count().should_not_be(0);
            };

            context["when creating item"] = () =>
            {
                before = () =>
                {
                    
                };

                it["should create PriceMutation"] = () =>
                {
                    d._priceMutationService.GetObjectById(d.item.PriceMutationId).should_not_be_null();
                };

                it["should have 1 active PriceMutation"] = () =>
                {
                    d._priceMutationService.GetObjectsByIsActive(true, d.item.Id/*, d.baseGroup.Id*/, 0).Count().should_be(1);
                };

            };

            it["adjust_quantity_valid"] = () =>
            {
                d.stockAdjustment = new StockAdjustment()
                {
                    WarehouseId = d.localWarehouse.Id,
                    AdjustmentDate = DateTime.Today,
                    Description = "Stock Adjust Positive"
                };
                d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);

                d.stockAD1 = new StockAdjustmentDetail()
                {
                    ItemId = d.item.Id,
                    Quantity = 10,
                    StockAdjustmentId = d.stockAdjustment.Id
                };
                d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._barringService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService,
                                                        d._closingService);
                d.item.Errors.Count().should_be(0);
            };

            it["adjust_quantity_invalid"] = () =>
            {
                d.stockAdjustment = new StockAdjustment()
                {
                    WarehouseId = d.localWarehouse.Id,
                    AdjustmentDate = DateTime.Today,
                    Description = "Stock Adjust Positive"
                };
                d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);

                d.stockAD1 = new StockAdjustmentDetail()
                {
                    ItemId = d.item.Id,
                    Quantity = -10,
                    StockAdjustmentId = d.stockAdjustment.Id
                };
                d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._barringService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService, d._closingService);
                d.stockAdjustment.Errors.Count().should_not_be(0);
                d.item.Quantity.should_be(0);
            };

            it["delete_item"] = () =>
            {
                d.item = d._itemService.SoftDeleteObject(d.item, d._stockMutationService, d._itemTypeService, d._warehouseItemService, d._barringService, 
                                                         d._purchaseOrderDetailService, d._stockAdjustmentDetailService, d._salesOrderDetailService, d._priceMutationService);
                d.item.Errors.Count().should_be(0);
            };

            context["when deleting item"] = () =>
            {
                before = () =>
                {
                    d.item = d._itemService.SoftDeleteObject(d.item, d._stockMutationService, d._itemTypeService, d._warehouseItemService, d._barringService,
                                                         d._purchaseOrderDetailService, d._stockAdjustmentDetailService, d._salesOrderDetailService, d._priceMutationService);
                };

                it["should not have active PriceMutation"] = () =>
                {
                    d._priceMutationService.GetObjectsByIsActive(true, d.item.Id, /*d.baseGroup.Id,*/ 0).Count().should_be(0);
                };

            };

            it["delete_item_with_compound_inrollerbuilder"] = () =>
            {
                d.machine = new Machine()
                {
                    Code = "M00001",
                    Name = "Machine 00001",
                    Description = "Machine"
                };
                d.machine = d._machineService.CreateObject(d.machine);
                d.coreBuilder = new CoreBuilder()
                {
                    BaseSku = "CB00001",
                    SkuNewCore = "CB00001N",
                    SkuUsedCore = "CB00001U",
                    Name = "CoreBuilder00001",
                    Category = "X",
                    UoMId = d.Pcs.Id
                };
                d.coreBuilder = d._coreBuilderService.CreateObject(d.coreBuilder, d._uomService, d._itemService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);
                d.coreIdentification = new CoreIdentification()
                {
                    Code = "CI0001",
                    Quantity = 1,
                    IdentifiedDate = DateTime.Now,
                    WarehouseId = d.localWarehouse.Id
                };
                d.coreIdentification = d._coreIdentificationService.CreateObject(d.coreIdentification, d._contactService);
                d.coreIdentificationDetail = new CoreIdentificationDetail()
                {
                    CoreIdentificationId = d.coreIdentification.Id,
                    DetailId = 1,
                    MaterialCase = 2,
                    CoreBuilderId = d.coreBuilder.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = d.machine.Id,
                    RD = 12,
                    CD = 12,
                    RL = 12,
                    WL = 12,
                    TL = 12
                };
                d.coreIdentificationDetail = d._coreIdentificationDetailService.CreateObject(d.coreIdentificationDetail, d._coreIdentificationService, d._coreBuilderService, d._rollerTypeService, d._machineService);
                Item compound = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Compound").Id,
                    Name = "Compound",
                    Category = "Compound",
                    Sku = "CMP0001",
                    UoMId = d.Pcs.Id
                };
                compound = d._itemService.CreateObject(compound, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.stockAdjustment = new StockAdjustment()
                {
                    WarehouseId = d.localWarehouse.Id,
                    AdjustmentDate = DateTime.Today,
                    Description = "Compound"
                };
                d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);

                d.stockAD1 = new StockAdjustmentDetail()
                {
                    StockAdjustmentId = d.stockAdjustment.Id,
                    Quantity = 2,
                    ItemId = compound.Id
                };
                d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._barringService, d._warehouseItemService, d._generalLedgerJournalService,
                                                        d._accountService, d._closingService);

                d.rollerBuilder = new RollerBuilder()
                {
                    CoreBuilderId = d.coreBuilder.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Found DT").Id,
                    MachineId = d.machine.Id,
                    RD = 13,
                    CD = 13,
                    RL = 13,
                    WL = 13,
                    TL = 13,
                    BaseSku = "RB0001",
                    SkuRollerUsedCore = "RB0001U",
                    SkuRollerNewCore = "RB0001N",
                    Name = "Roller Builder",
                    Category = "RB",
                    CompoundId = compound.Id,
                    UoMId = d.Pcs.Id
                };
                d.rollerBuilder = d._rollerBuilderService.CreateObject(d.rollerBuilder, d._machineService, d._uomService, d._itemService, d._itemTypeService, d._coreBuilderService, d._rollerTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                compound = d._itemService.SoftDeleteObject(compound, d._stockMutationService, d._itemTypeService, d._warehouseItemService, d._barringService,
                                                           d._purchaseOrderDetailService, d._stockAdjustmentDetailService, d._salesOrderDetailService, d._priceMutationService);
                compound.Errors.Count().should_not_be(0);
            };
        }
    }
}