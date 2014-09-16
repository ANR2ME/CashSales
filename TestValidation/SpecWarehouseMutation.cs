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

    public class SpecWarehouseMutation: nspec
    {
        DataBuilder d;
        
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();
                d.PopulateWarehouse();
                d.PopulateItem();
                d.PopulateSingles();
                d.PopulateBuilders();
                d.PopulateBarring();
                d.PopulateWarehouseMutationForRollerIdentificationAndRecovery();
            }
        }

        void data_validation()
        {
        
            it["validates_data"] = () =>
            {
                d.warehouseMutationOrder.Errors.Count().should_be(0);
                d.wmoDetail1.Errors.Count().should_be(0);
                d.wmoDetail2.Errors.Count().should_be(0);
                d.wmoDetail3.Errors.Count().should_be(0);
                d.wmoDetail4.Errors.Count().should_be(0);
                d.wmoDetail5.Errors.Count().should_be(0);
                d.wmoDetail6.Errors.Count().should_be(0);
            };

            it["deletes_warehousemutationorder"] = () =>
            {
                d.warehouseMutationOrder = d._warehouseMutationOrderService.SoftDeleteObject(d.warehouseMutationOrder, d._warehouseMutationOrderDetailService);
                d.warehouseMutationOrder.Errors.Count().should_not_be(0);
            };

            it["deletes_warehousemutationorderdetail"] = () =>
            {
                d.wmoDetail1 = d._warehouseMutationOrderDetailService.SoftDeleteObject(d.wmoDetail1, d._warehouseMutationOrderService, d._warehouseItemService);
                d.wmoDetail1.Errors.Count().should_be(0);
            };

            it["confirms_warehousemutationorder"] = () =>
            {
                d.warehouseMutationOrder = d._warehouseMutationOrderService.ConfirmObject(d.warehouseMutationOrder, DateTime.Now, d._warehouseMutationOrderDetailService, d._itemService, d._barringService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutationOrder.IsConfirmed.should_be(true);
                d.warehouseMutationOrder.Errors.Count().should_be(0);
            };

            it["unconfirms_warehousemutationorder"] = () =>
            {
                d.warehouseMutationOrder = d._warehouseMutationOrderService.ConfirmObject(d.warehouseMutationOrder, DateTime.Now, d._warehouseMutationOrderDetailService, d._itemService, d._barringService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutationOrder.IsConfirmed.should_be(true);
                d.warehouseMutationOrder = d._warehouseMutationOrderService.UnconfirmObject(d.warehouseMutationOrder, d._warehouseMutationOrderDetailService, d._itemService, d._barringService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutationOrder.IsConfirmed.should_be(false);
                d.warehouseMutationOrder.Errors.Count().should_be(0);
            };

            it["finishes_detail"] = () =>
            {
                int quantitywarehousefrom = d._warehouseItemService.FindOrCreateObject(d.warehouseMutationOrder.WarehouseFromId, d.wmoDetail1.ItemId).Quantity;
                int quantitywarehouseto = d._warehouseItemService.FindOrCreateObject(d.warehouseMutationOrder.WarehouseToId, d.wmoDetail1.ItemId).Quantity;
                d.warehouseMutationOrder = d._warehouseMutationOrderService.ConfirmObject(d.warehouseMutationOrder, DateTime.Today, d._warehouseMutationOrderDetailService, d._itemService,
                                                                                          d._barringService, d._warehouseItemService, d._stockMutationService);
                d.wmoDetail1.IsConfirmed.should_be(true);
                int quantitywarehousefromfinal = d._warehouseItemService.FindOrCreateObject(d.warehouseMutationOrder.WarehouseFromId, d.wmoDetail1.ItemId).Quantity;
                int quantitywarehousetofinal = d._warehouseItemService.FindOrCreateObject(d.warehouseMutationOrder.WarehouseToId, d.wmoDetail1.ItemId).Quantity;
                d.wmoDetail1.Errors.Count().should_be(0);
                quantitywarehousefromfinal.should_be(quantitywarehousefrom - d.wmoDetail1.Quantity);
                quantitywarehousetofinal.should_be(quantitywarehouseto + d.wmoDetail1.Quantity);
            };

            it["unconfirm_whendetailisfinished"] = () =>
            {
                d.warehouseMutationOrder = d._warehouseMutationOrderService.ConfirmObject(d.warehouseMutationOrder, DateTime.Today, d._warehouseMutationOrderDetailService, d._itemService,
                                                                                          d._barringService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutationOrder.IsConfirmed.should_be(true);
                d.warehouseMutationOrder = d._warehouseMutationOrderService.UnconfirmObject(d.warehouseMutationOrder, d._warehouseMutationOrderDetailService, d._itemService, d._barringService,
                                                                                            d._warehouseItemService, d._stockMutationService);
                d.warehouseMutationOrder.IsConfirmed.should_be(false);
                d.warehouseMutationOrder.Errors.Count().should_be(0);
            };

            it["deletes_whendetailisfinished"] = () =>
            {
                d.warehouseMutationOrder = d._warehouseMutationOrderService.ConfirmObject(d.warehouseMutationOrder, DateTime.Today, d._warehouseMutationOrderDetailService, d._itemService, d._barringService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutationOrder = d._warehouseMutationOrderService.SoftDeleteObject(d.warehouseMutationOrder, d._warehouseMutationOrderDetailService);
                d.warehouseMutationOrder.IsDeleted.should_be(false);
                d.warehouseMutationOrder.Errors.Count().should_not_be(0);
            };

            context["confirm_orderanddetail"] = () =>
            {
                before = () =>
                {
                    d.warehouseMutationOrder = d._warehouseMutationOrderService.ConfirmObject(d.warehouseMutationOrder, DateTime.Today, d._warehouseMutationOrderDetailService, d._itemService,
                                                                                              d._barringService, d._warehouseItemService, d._stockMutationService);
                };

                it["validates_confirmeddetails"] = () =>
                {
                    d.warehouseMutationOrder.IsConfirmed.should_be(true);
                    d.warehouseMutationOrder.Errors.Count().should_be(0);
                };
            };
        }
    }
}