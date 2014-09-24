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

namespace WebView.Controllers
{
    public class MstBarringController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("BarringController");
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IMachineService _machineService;
        private IWarehouseItemService _warehouseItemService;
        private IBarringService _barringService;
        private IContactService _contactService;
        private IContactGroupService _contactGroupService;
        private IPriceMutationService _priceMutationService;
       
        public MstBarringController()
        {
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
             _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _contactService = new ContactService(new ContactRepository(),new ContactValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
        }
        public ActionResult Index()
        {
            return View();
        }
        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _barringService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Barring>;

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
                            model.Name,
                            model.Category,
                            model.ItemTypeId,
                            _itemTypeService.GetObjectById(model.ItemTypeId).Name,
                            model.UoMId,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.Sku,
                            model.RollNo,
                            model.ContactId,
                            _contactService.GetObjectById(model.ContactId).Name,
                            model.MachineId,
                            _machineService.GetObjectById(model.MachineId).Name,
                            model.BlanketItemId,
                            _itemService.GetObjectById(model.BlanketItemId).Name,
                            model.LeftBarItemId.HasValue ? model.LeftBarItemId.Value : model.LeftBarItemId = null,
                            model.LeftBarItemId.HasValue ? _itemService.GetObjectById(model.LeftBarItemId.Value).Name : "",
                            model.RightBarItemId.HasValue ? model.RightBarItemId.Value : model.RightBarItemId = null,
                            model.RightBarItemId.HasValue ? _itemService.GetObjectById(model.RightBarItemId.Value).Name : "",
                            model.AC,
                            model.AR,
                            model.thickness,
                            model.KS,
                            model.Quantity,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListBlanket(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            int itemtypeId = _itemTypeService.GetObjectByName("Blanket").Id;
            var query = _itemService.GetAll().Where(d => d.IsDeleted == false && d.ItemTypeId == itemtypeId);

            var list = query as IEnumerable<Item>;

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
                            model.Name,
                            model.Sku,
                            model.Category,
                            model.UoMId,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListBar(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            int itemtypeId = _itemTypeService.GetObjectByName("Bar").Id;
            var query = _itemService.GetAll().Where(d => d.IsDeleted == false && d.ItemTypeId == itemtypeId);

            var list = query as IEnumerable<Item>;

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
                            model.Name,
                            model.Sku,
                            model.Category,
                            model.UoMId,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.Quantity,
                            model.PendingReceival,
                            model.PendingDelivery,
                            model.CreatedAt,
                            model.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Barring model = new Barring();
            try
            {
                model = _barringService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Name,
                model.Category,
                model.ItemTypeId,
                ItemType = _itemTypeService.GetObjectById(model.ItemTypeId).Name,
                model.UoMId,
                UoM = _uomService.GetObjectById(model.UoMId).Name,
                model.Sku,
                model.RollNo,
                model.ContactId,
                Contact =_contactService.GetObjectById(model.ContactId).Name,
                model.MachineId,
                Machine = _machineService.GetObjectById(model.MachineId).Name,
                model.BlanketItemId,
                BlanketItem = _itemService.GetObjectById(model.BlanketItemId).Name,
                LeftBarItemId = model.LeftBarItemId.HasValue ? model.LeftBarItemId.Value : model.LeftBarItemId = null,
                LeftBarItem = model.LeftBarItemId.HasValue ? _itemService.GetObjectById(model.LeftBarItemId.Value).Name : "",
                RightBarItemId = model.RightBarItemId.HasValue ? model.RightBarItemId.Value : model.RightBarItemId = null,
                RightBarItem = model.RightBarItemId.HasValue ? _itemService.GetObjectById(model.RightBarItemId.Value).Name : "",
                model.AC,
                model.AR, 
                model.thickness,
                model.KS,
                model.Quantity,
                model.CreatedAt,
                model.UpdatedAt,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Barring model)
        {
            try
            {
                model = _barringService.CreateObject(model,_barringService,_uomService,
                    _itemService,_itemTypeService,_contactService,_machineService,_warehouseItemService,
                    _warehouseService,_priceMutationService,_contactGroupService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(Barring model)
        {
            try
            {
                var data = _barringService.GetObjectById(model.Id);
                data.Name = model.Name;
                model = _barringService.UpdateObject(data,_barringService,_uomService,_itemService,_itemTypeService,_contactService,_machineService,_warehouseItemService,_warehouseService,_contactGroupService,_priceMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(Barring model)
        {
            try
            {
                var data = _barringService.GetObjectById(model.Id);
                model = _barringService.SoftDeleteObject(data,_itemTypeService,_warehouseItemService
                    ,_priceMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
