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
using System.Data.Objects;

namespace WebView.Controllers
{
    public class StockMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("StockMutationController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IUoMService _uomService;

        public StockMutationController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(),new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.StockMutation, Core.Constants.Constant.MenuGroupName.Master))
            {
                return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            }

            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _stockMutationService.GetQueryable().Include("Item").Include("Warehouse").Include("UoM");

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.ItemId,
                            sku = model.Item.Sku,
                            item = model.Item.Name,
                            model.WarehouseId,
                            warehouse = model.Warehouse.Name,
                            model.WarehouseItemId,
                            ready = model.ItemCase == Core.Constants.Constant.ItemCase.Ready ?
                                (model.Status == Core.Constants.Constant.MutationStatus.Addition ? model.Quantity : model.Quantity * (-1)) : 0,
                            pendingreceival = model.ItemCase == Core.Constants.Constant.ItemCase.PendingReceival ?
                                (model.Status == Core.Constants.Constant.MutationStatus.Addition ? model.Quantity : model.Quantity * (-1)) : 0,
                            pendingdelivery = model.ItemCase == Core.Constants.Constant.ItemCase.PendingDelivery ?
                                (model.Status == Core.Constants.Constant.MutationStatus.Addition ? model.Quantity : model.Quantity * (-1)) : 0,
                            uom = model.Item.UoM.Name,
                            model.SourceDocumentType,
                            model.SourceDocumentId,
                            model.SourceDocumentCode,
                            model.SourceDocumentDetailType,
                            model.SourceDocumentDetailId,
                            model.SourceDocumentDetailCode,
                            model.CreatedAt,
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
                            model.Id,
                            model.ItemId,
                            model.sku,
                            model.item,
                            model.WarehouseId,
                            model.warehouse,
                            model.WarehouseItemId,
                            model.ready,
                            model.pendingreceival,
                            model.pendingdelivery,
                            model.uom,
                            model.SourceDocumentType,
                            model.SourceDocumentId,
                            model.SourceDocumentCode,
                            model.SourceDocumentDetailType,
                            model.SourceDocumentDetailId,
                            model.SourceDocumentDetailCode,
                            model.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, DateTime? startdate, DateTime? enddate, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            if (startdate.HasValue && enddate.HasValue)
            {
                filter = "(" + filter + ") AND CreatedAt >= @0 AND CreatedAt < @1";
            }

            // Get Data
            var q = _stockMutationService.GetQueryable().Include("Item").Include("Warehouse").Include("UoM");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.ItemId,
                             sku = model.Item.Sku,
                             item = model.Item.Name,
                             model.WarehouseId,
                             warehouse = model.Warehouse.Name,
                             model.WarehouseItemId,
                             ready = model.ItemCase == Core.Constants.Constant.ItemCase.Ready ?
                                 (model.Status == Core.Constants.Constant.MutationStatus.Addition ? model.Quantity : model.Quantity * (-1)) : 0,
                             pendingreceival = model.ItemCase == Core.Constants.Constant.ItemCase.PendingReceival ?
                                 (model.Status == Core.Constants.Constant.MutationStatus.Addition ? model.Quantity : model.Quantity * (-1)) : 0,
                             pendingdelivery = model.ItemCase == Core.Constants.Constant.ItemCase.PendingDelivery ?
                                 (model.Status == Core.Constants.Constant.MutationStatus.Addition ? model.Quantity : model.Quantity * (-1)) : 0,
                             uom = model.Item.UoM.Name,
                             model.SourceDocumentType,
                             model.SourceDocumentId,
                             model.SourceDocumentCode,
                             model.SourceDocumentDetailType,
                             model.SourceDocumentDetailId,
                             model.SourceDocumentDetailCode,
                             model.CreatedAt,
                         }).Where(filter, startdate.GetValueOrDefault().Date, enddate.GetValueOrDefault().AddDays(1).Date).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.Id,
                            model.ItemId,
                            model.sku,
                            model.item,
                            model.WarehouseId,
                            model.warehouse,
                            model.WarehouseItemId,
                            model.ready,
                            model.pendingreceival,
                            model.pendingdelivery,
                            model.uom,
                            model.SourceDocumentType,
                            model.SourceDocumentId,
                            model.SourceDocumentCode,
                            model.SourceDocumentDetailType,
                            model.SourceDocumentDetailId,
                            model.SourceDocumentDetailCode,
                            model.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}