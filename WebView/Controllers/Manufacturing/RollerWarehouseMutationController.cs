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
    public class RollerWarehouseMutationController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RollerWarehouseMutationController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IRollerWarehouseMutationService _rollerWarehouseMutationService;
        private IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService;
        private IContactService _contactService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private ICoreIdentificationService _coreIdentificationService;
        private IWarehouseService _warehouseService;
        private ICoreBuilderService _coreBuilderService;

        public RollerWarehouseMutationController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _rollerWarehouseMutationService = new RollerWarehouseMutationService(new RollerWarehouseMutationRepository(), new RollerWarehouseMutationValidator());
            _rollerWarehouseMutationDetailService = new RollerWarehouseMutationDetailService(new RollerWarehouseMutationDetailRepository(), new RollerWarehouseMutationDetailValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
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
            var query =  _rollerWarehouseMutationService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RollerWarehouseMutation>;

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
                            model.Code,
                            model.CoreIdentificationId,
                            _coreIdentificationService.GetObjectById(model.CoreIdentificationId).Code,
                            model.WarehouseFromId,
                            _warehouseService.GetObjectById(model.WarehouseFromId).Code,
                            _warehouseService.GetObjectById(model.WarehouseFromId).Name,
                            model.WarehouseToId,
                            _warehouseService.GetObjectById(model.WarehouseToId).Code,
                            _warehouseService.GetObjectById(model.WarehouseToId).Name,
                            model.Quantity,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

       


        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id,string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RollerWarehouseMutationDetail>;

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
                            model.Code,
                            model.RollerWarehouseMutationId,
                            model.CoreIdentificationDetailId,
                            model.ItemId,
                            _itemService.GetObjectById(model.ItemId).Name,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            RollerWarehouseMutation model = new RollerWarehouseMutation();
            try
            {
                model = _rollerWarehouseMutationService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.CoreIdentificationId,
                CoreIdentification = _coreIdentificationService.GetObjectById(model.CoreIdentificationId).Code,
                model.WarehouseFromId,
                WarehouseFromCode = _warehouseService.GetObjectById(model.WarehouseFromId).Code,
                WarehouseFrom = _warehouseService.GetObjectById(model.WarehouseFromId).Name,
                model.WarehouseToId,
                model.MutationDate,
                WarehouseToCode = _warehouseService.GetObjectById(model.WarehouseToId).Code,
                WarehouseTo = _warehouseService.GetObjectById(model.WarehouseToId).Name,
                model.Quantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            RollerWarehouseMutationDetail model = new RollerWarehouseMutationDetail();
            try
            {
                model = _rollerWarehouseMutationDetailService.GetObjectById(Id);
            
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.RollerWarehouseMutationId,
                model.CoreIdentificationDetailId,
                model.ItemId,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(RollerWarehouseMutation model)
        {
            try
            {
                model = _rollerWarehouseMutationService.CreateObject(model,_warehouseService,_coreIdentificationService);
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
        public dynamic InsertDetail(RollerWarehouseMutationDetail model)
        {
            try
            {
                var data = _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId);
                var item = data.MaterialCase == Core.Constants.Constant.MaterialCase.New ? _coreBuilderService.GetNewCore(data.CoreBuilderId) :
                    _coreBuilderService.GetUsedCore(data.CoreBuilderId);
                model.ItemId = item.Id;
                model = _rollerWarehouseMutationDetailService.CreateObject(model,_rollerWarehouseMutationService,
                    _coreIdentificationDetailService
                    ,_itemService,_warehouseItemService);
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
        public dynamic Update(RollerWarehouseMutation model)
        {
            try
            {
                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                data.CoreIdentificationId = model.CoreIdentificationId;
                data.WarehouseFromId = model.WarehouseFromId;
                data.WarehouseToId = model.WarehouseToId;
                data.Quantity = model.Quantity;
                data.MutationDate = model.MutationDate;
                model = _rollerWarehouseMutationService.UpdateObject(data,_warehouseService,_coreIdentificationService);
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
        public dynamic Delete(RollerWarehouseMutation model)
        {
            try
            {
                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationService.SoftDeleteObject(data);
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

        [HttpPost]
        public dynamic DeleteDetail(RollerWarehouseMutationDetail model)
        {
            try
            {
                var data = _rollerWarehouseMutationDetailService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationDetailService.SoftDeleteObject(data,_rollerWarehouseMutationService,_warehouseItemService);
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

        [HttpPost]
        public dynamic UpdateDetail(RollerWarehouseMutationDetail model)
        {
            try
            {
                var data1 = _coreIdentificationDetailService.GetObjectById(model.CoreIdentificationDetailId);
                var item = data1.MaterialCase == Core.Constants.Constant.MaterialCase.New ? _coreBuilderService.GetNewCore(data1.CoreBuilderId) :
                    _coreBuilderService.GetUsedCore(data1.CoreBuilderId);
                model.ItemId = item.Id;



                var data = _rollerWarehouseMutationDetailService.GetObjectById(model.Id);
                data.RollerWarehouseMutationId = model.RollerWarehouseMutationId;
                data.CoreIdentificationDetailId = model.CoreIdentificationDetailId;
                data.ItemId = model.ItemId;
                model = _rollerWarehouseMutationDetailService.UpdateObject(data,_rollerWarehouseMutationService
                    ,_coreIdentificationDetailService,_itemService,_warehouseItemService);
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
        public dynamic Confirm(RollerWarehouseMutation model)
        {
            try
            {
                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationService.ConfirmObject(data,model.ConfirmationDate.Value
                    ,_rollerWarehouseMutationDetailService,_itemService,_barringService
                    ,_warehouseItemService,_stockMutationService,_coreIdentificationDetailService,_coreIdentificationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnConfirm(RollerWarehouseMutation model)
        {
            try
            {

                var data = _rollerWarehouseMutationService.GetObjectById(model.Id);
                model = _rollerWarehouseMutationService.UnconfirmObject(data,_rollerWarehouseMutationDetailService
                    ,_itemService,_barringService,_warehouseItemService,_stockMutationService,_coreIdentificationDetailService
                    ,_coreIdentificationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }


    }
}
