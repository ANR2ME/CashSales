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

    public class SpecUserRole: nspec
    {

        DataBuilder d;
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                d.PopulateData();

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

                d.contact = d._contactService.CreateObject("Abbey", "1 Abbey St", "001234567", "Daddy", "001234888", "abbey@abbeyst.com", d._contactGroupService);
            }
        }

        void useraccount_validation()
        {
        
            it["validates_useraccount"] = () =>
            {
                d.admin.Errors.Count().should_be(0);
                d.menudata.Errors.Count().should_be(0);
                d.menufinance.Errors.Count().should_be(0);
                d.admindata.Errors.Count().should_be(0);
                d.adminfinance.Errors.Count().should_be(0);
            };

            it["useraccount_with_same_username"] = () =>
            {
                d.user.Errors.Count().should_not_be(0);
            };

            it["update_with_correct_oldpassword"] = () =>
            {
                string oldpassword = "sysadmin";
                //d.admin.Password = "sysadmin123";
                d.admin = d._userAccountService.UpdateObjectPassword(d.admin, oldpassword, "sysadmin123", "sysadmin123");
                d.admin.Errors.Count().should_be(0);
            };

            it["update_with_wrong_oldpassword"] = () =>
            {
                string oldpassword = "123";
                //d.admin.Password = "asal2an";
                d.admin = d._userAccountService.UpdateObjectPassword(d.admin, oldpassword, "asal2an", "asal2an");
                d.admin.Errors.Count().should_not_be(0);
            };

            it["delete_loggedin_useraccount"] = () =>
            {
                d.admin = d._userAccountService.SoftDeleteObject(d.admin, d.admin.Id);
                d.admin.Errors.Count().should_not_be(0);
            };

            it["delete_other_useraccount"] = () =>
            {
                d.user.Username = "Adam";
                d._userAccountService.CreateObject(d.user);
                d.admin = d._userAccountService.SoftDeleteObject(d.admin, d.user.Id);
                d.admin.Errors.Count().should_be(0);
            };
        }
    }
}