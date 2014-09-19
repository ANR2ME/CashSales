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

    public class SpecItemType: nspec
    {

        DataBuilder d;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                d = new DataBuilder();

                d.baseGroup = d._contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

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

                d.localWarehouse = new Warehouse()
                {
                    Name = "Sentral Solusi Data",
                    Description = "Kali Besar Jakarta",
                    Code = "LCL"
                };
                d.localWarehouse = d._warehouseService.CreateObject(d.localWarehouse, d._warehouseItemService, d._itemService);

                d.item = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Accessory").Id,
                    Sku = "ABC1001",
                    Name = "ABC",
                    Category = "ABC123",
                    UoMId = d.Pcs.Id,
                    Quantity = 0
                };
                d.item = d._itemService.CreateObject(d.item, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);
            }
        }

        void itemtype_validation()
        {
        
            it["validates_itemtypes"] = () =>
            {
                d.typeAccessory.Errors.Count().should_be(0);
                d.typeBearing.Errors.Count().should_be(0);
                d.typeBlanket.Errors.Count().should_be(0);
                d.typeCore.Errors.Count().should_be(0);
                d.typeConsumable.Errors.Count().should_be(0);
                d.typeChemical.Errors.Count().should_be(0);
                d.typeCompound.Errors.Count().should_be(0);
                d.typeGlue.Errors.Count.should_be(0);
                d.typeUnderpacking.Errors.Count().should_be(0);
                d.typeRoller.Errors.Count().should_be(0);
            };

            it["itemtype_with_no_name"] = () =>
            {
                ItemType nonameitemtype = new ItemType()
                {
                    Name = "     ",
                    Description = "Empty"
                };
                nonameitemtype = d._itemTypeService.CreateObject(nonameitemtype);
                nonameitemtype.Errors.Count().should_not_be(0);
            };

            it["delete_itemtype"] = () =>
            {
                d.typeGlue = d._itemTypeService.SoftDeleteObject(d.typeGlue, d._itemService);
                d.typeGlue.Errors.Count().should_be(0);
            };

            it["delete_itemtype_with_item"] = () =>
            {
                Item glue101 = new Item()
                {
                    ItemTypeId = d.typeGlue.Id,
                    Name = "Glue101",
                    Category = "Glue",
                    Sku = "G101",
                    UoMId = d.Pcs.Id
                };
                glue101 = d._itemService.CreateObject(glue101, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.stockAdjustment = new StockAdjustment()
                {
                    WarehouseId = d.localWarehouse.Id,
                    AdjustmentDate = DateTime.Today,
                    Description = "Glue Adjustment"
                };
                d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);

                d.stockAD1 = new StockAdjustmentDetail()
                {
                    StockAdjustmentId = d.stockAdjustment.Id,
                    Quantity = 100,
                    ItemId = glue101.Id
                };
                d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._barringService, d._warehouseItemService, d._generalLedgerJournalService, d._accountService, d._closingService);

                d.typeGlue = d._itemTypeService.SoftDeleteObject(d.typeGlue, d._itemService);
                d.typeGlue.Errors.Count().should_not_be(0);
            };
        }
    }
}