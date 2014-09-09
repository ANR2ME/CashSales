using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    public class WarehouseItemController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("WarehouseItemController");
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;

        public WarehouseItemController()
        {
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.WarehouseItem, Core.Constants.Constant.MenuGroupName.Master))
            {
                return Content(Core.Constants.Constant.PageViewNotAllowed);
            }

            return View();
        }

        public dynamic GetListItem(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _warehouseItemService.GetQueryableObjectsByWarehouseId(id).Include("Item").Include("ItemType").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.ItemId,
                             item = model.Item.Name,
                             itemtypeid = model.Item.ItemTypeId,
                             itemtype = model.Item.ItemType.Name,
                             sku = model.Item.Sku,
                             category = model.Item.Category,
                             uomid = model.Item.UoMId,
                             uom = model.Item.UoM.Name,
                             model.Quantity,
                             model.PendingDelivery,
                             model.PendingReceival,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                            model.ItemId,
                            model.item,
                            model.itemtypeid,
                            model.itemtype,
                            model.sku,
                            model.category,
                            model.uomid,
                            model.uom,
                            model.Quantity,
                            model.PendingDelivery,
                            model.PendingReceival,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListWarehouse(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _warehouseItemService.GetQueryableObjectsByItemId(id).Include("Warehouse");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.WarehouseId,
                             warehousecode = model.Warehouse.Code,
                             warehouse = model.Warehouse.Name,
                             warehousedesc = model.Warehouse.Description,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = query.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
            // default last page
            if (totalPages > 0)
            {
                if (!page.HasValue)
                {
                    pageIndex = totalPages - 1;
                    page = totalPages;
                }
            }

            list = list.Skip(pageIndex * pageSize).Take(pageSize);

            return Json(new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                    from model in list
                    select new
                    {
                        id = model.Id,
                        cell = new object[] {
                           model.WarehouseId,
                           model.warehousecode,
                           model.warehouse,
                           model.warehousedesc,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
