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

    public class SpecRollerBuilder: nspec
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
                d.itemCompound = new Item()
                {
                    ItemTypeId = d._itemTypeService.GetObjectByName("Compound").Id,
                    Sku = "Cmp10001",
                    Name = "Cmp 10001",
                    Category = "cmp",
                    UoMId = d.Pcs.Id
                };
                d.itemCompound = d._itemService.CreateObject(d.itemCompound, d._uomService, d._itemTypeService, d._warehouseItemService, d._warehouseService, d._priceMutationService, d._contactGroupService);

                d.stockAdjustment = new StockAdjustment()
                {
                    AdjustmentDate = DateTime.Today,
                    WarehouseId = d.localWarehouse.Id
                };
                d._stockAdjustmentService.CreateObject(d.stockAdjustment, d._warehouseService);
                d.stockAD1 = new StockAdjustmentDetail()
                {
                    StockAdjustmentId = d.stockAdjustment.Id,
                    Quantity = 2,
                    ItemId = d.itemCompound.Id
                };
                d._stockAdjustmentDetailService.CreateObject(d.stockAD1, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._barringService, d._warehouseItemService, d._generalLedgerJournalService,
                                                        d._accountService, d._closingService);

                d.contact = d._contactService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com", d._contactGroupService);

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
                    ContactId = d.contact.Id,
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

                d.rollerBuilder = new RollerBuilder()
                {
                    BaseSku = "RB0001",
                    SkuRollerNewCore = "RB0001N",
                    SkuRollerUsedCore = "RB0001U",
                    Name = "Roller 0001",
                    Category = "0001",
                    RD = 12,
                    CD = 13,
                    RL = 14,
                    TL = 15,
                    WL = 16,
                    CompoundId = d.itemCompound.Id,
                    CoreBuilderId = d.coreBuilder.Id,
                    MachineId = d.machine.Id,
                    RollerTypeId = d._rollerTypeService.GetObjectByName("Damp").Id,
                    UoMId = d.Pcs.Id
                };
                d.rollerBuilder = d._rollerBuilderService.CreateObject(d.rollerBuilder, d._machineService, d._uomService, d._itemService, d._itemTypeService,
                                                                       d._coreBuilderService, d._rollerTypeService, d._warehouseItemService, d._warehouseService,
                                                                       d._priceMutationService, d._contactGroupService);
            }
        }

        void rollerbuilder_validation()
        {
        
            it["validates_rollerbuilder"] = () =>
            {
                d.item.Errors.Count().should_be(0);
                d.itemCompound.Errors.Count().should_be(0);
                d.machine.Errors.Count().should_be(0);
                d.coreBuilder.Errors.Count().should_be(0);
                d.coreIdentification.Errors.Count().should_be(0);
                d.coreIdentificationDetail.Errors.Count().should_be(0);
                d.rollerBuilder.Errors.Count().should_be(0);
            };

            it["delete_rollerbuilder"] = () =>
            {
                d.rollerBuilder = d._rollerBuilderService.SoftDeleteObject(d.rollerBuilder, d._itemService, d._barringService, d._priceMutationService, d._recoveryOrderDetailService,
                                                                           d._coreBuilderService, d._warehouseItemService, d._stockMutationService, d._itemTypeService,
                                                                           d._purchaseOrderDetailService, d._stockAdjustmentDetailService, d._salesOrderDetailService);
                d.rollerBuilder.Errors.Count().should_be(0);
            };

            it["delete_rollerbuilder_with_recoveryorderdetail"] = () =>
            {
                StockAdjustment sa = new StockAdjustment()
                {
                    AdjustmentDate = DateTime.Today,
                    WarehouseId = d.localWarehouse.Id
                };
                d._stockAdjustmentService.CreateObject(sa, d._warehouseService);
                StockAdjustmentDetail stockADx = new StockAdjustmentDetail()
                {
                    StockAdjustmentId = d.stockAdjustment.Id,
                    Quantity = 1,
                    ItemId = d._coreBuilderService.GetNewCore(d.coreBuilder.Id).Id
                };
                d._stockAdjustmentDetailService.CreateObject(stockADx, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                StockAdjustmentDetail stockADy = new StockAdjustmentDetail()
                {
                    StockAdjustmentId = d.stockAdjustment.Id,
                    Quantity = 1,
                    ItemId = d._coreBuilderService.GetUsedCore(d.coreBuilder.Id).Id
                };
                d._stockAdjustmentDetailService.CreateObject(stockADy, d._stockAdjustmentService, d._itemService, d._warehouseItemService);

                d._stockAdjustmentService.ConfirmObject(d.stockAdjustment, DateTime.Today, d._stockAdjustmentDetailService, d._stockMutationService,
                                                        d._itemService, d._barringService, d._warehouseItemService, d._generalLedgerJournalService,
                                                        d._accountService, d._closingService);

                d.coreIdentification = d._coreIdentificationService.ConfirmObject(d.coreIdentification, DateTime.Today, 
                                       d._coreIdentificationDetailService, d._stockMutationService, d._recoveryOrderService, d._recoveryOrderDetailService,
                                       d._coreBuilderService, d._itemService, d._warehouseItemService, d._barringService);
                d.coreIdentification.Errors.Count().should_be(0);

                RecoveryOrder recoveryOrder = new RecoveryOrder()
                {
                    Code = "RO0001",
                    CoreIdentificationId = d.coreIdentification.Id,
                    QuantityReceived = 1,
                    WarehouseId = d.localWarehouse.Id
                };
                recoveryOrder = d._recoveryOrderService.CreateObject(recoveryOrder, d._coreIdentificationService);
                recoveryOrder.Errors.Count().should_be(0);

                RecoveryOrderDetail recoveryOrderDetail = new RecoveryOrderDetail()
                {
                    RecoveryOrderId = recoveryOrder.Id,
                    CoreIdentificationDetailId = d.coreIdentificationDetail.Id,
                    RollerBuilderId = d.rollerBuilder.Id,
                    CoreTypeCase = Core.Constants.Constant.CoreTypeCase.R,
                    RepairRequestCase = Core.Constants.Constant.RepairRequestCase.BearingSeat,
                    Acc = "Y",
                };
                recoveryOrderDetail = d._recoveryOrderDetailService.CreateObject(recoveryOrderDetail, d._recoveryOrderService, d._coreIdentificationDetailService, d._rollerBuilderService);
                recoveryOrderDetail.Errors.Count().should_be(0);

                d.rollerBuilder = d._rollerBuilderService.SoftDeleteObject(d.rollerBuilder, d._itemService, d._barringService, d._priceMutationService, d._recoveryOrderDetailService, d._coreBuilderService, d._warehouseItemService,
                                                                           d._stockMutationService, d._itemTypeService, d._purchaseOrderDetailService,
                                                                           d._stockAdjustmentDetailService, d._salesOrderDetailService);
                d.rollerBuilder.Errors.Count().should_not_be(0);
            };
        }
    }
}