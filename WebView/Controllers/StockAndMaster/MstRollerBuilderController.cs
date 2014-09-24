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
    public class MstRollerBuilderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RollerBuilderController");
        private ICoreBuilderService _coreBuilderService;
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IRollerBuilderService _rollerBuilderService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IMachineService _machineService;
        private IRollerTypeService _rollerTypeService;
        private IBarringService _barringService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IContactGroupService _contactGroupService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;


        public MstRollerBuilderController()
        {
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _machineService = new MachineService(new MachineRepository(), new MachineValidator());
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(),new SalesOrderDetailValidator());
        
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
            var query = _rollerBuilderService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RollerBuilder>;

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
                            model.UoMId,
                            _uomService.GetObjectById(model.UoMId).Name,
                            model.BaseSku,
                            model.SkuRollerUsedCore,
                            model.SkuRollerNewCore,
                            model.MachineId,
                            _machineService.GetObjectById(model.MachineId).Name,
                            model.RollerTypeId,
                            _rollerTypeService.GetObjectById(model.RollerTypeId).Name,
                            model.CompoundId,
                            _itemTypeService.GetObjectById(model.CompoundId).Name,
                            model.CoreBuilderId,
                            _coreBuilderService.GetObjectById(model.CoreBuilderId).Name,
                            model.RD,
                            model.CD,
                            model.RL,
                            model.WL,
                            model.TL,
                            _itemService.GetObjectById(model.RollerUsedCoreItemId).Quantity,
                            _itemService.GetObjectById(model.RollerNewCoreItemId).Quantity,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListCompound(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            int itemtypeId = _itemTypeService.GetObjectByName("Compound").Id;
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
            RollerBuilder model = new RollerBuilder();
            try
            {
                model = _rollerBuilderService.GetObjectById(Id);
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
                model.UoMId,
                UoM = _uomService.GetObjectById(model.UoMId).Name,
                model.BaseSku,
                model.SkuRollerUsedCore,
                model.SkuRollerNewCore,
                model.MachineId,
                Machine = _machineService.GetObjectById(model.MachineId).Name,
                model.RollerTypeId,
                RollerType = _rollerTypeService.GetObjectById(model.RollerTypeId).Name,
                model.CompoundId,
                Compound = _itemTypeService.GetObjectById(model.CompoundId).Name,
                model.CoreBuilderId,
                CoreBuilder = _coreBuilderService.GetObjectById(model.CoreBuilderId).Name,
                model.RD,
                model.CD,
                model.RL,
                model.WL,
                model.TL,
                RollerUsedCoreQuantity = _itemService.GetObjectById(model.RollerUsedCoreItemId).Quantity,
                RollerNewCoreQuantity =_itemService.GetObjectById(model.RollerNewCoreItemId).Quantity,

                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(RollerBuilder model)
        {
            try
            {
                model = _rollerBuilderService.CreateObject(model,_machineService,
                    _uomService,_itemService,_itemTypeService,_coreBuilderService,
                    _rollerTypeService,_warehouseItemService,_warehouseService,_priceMutationService,_contactGroupService);
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
        public dynamic Update(RollerBuilder model)
        {
            try
            {
                var data = _rollerBuilderService.GetObjectById(model.Id);
                data.Name = model.Name;
                model = _rollerBuilderService.UpdateObject(data,_machineService,_uomService,_itemService,
                    _itemTypeService,_coreBuilderService,_rollerTypeService,_warehouseItemService,_warehouseService,
                    _barringService,_contactService,_priceMutationService,_contactGroupService);
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
        public dynamic Delete(RollerBuilder model)
        {
            try
            {
                var data = _rollerBuilderService.GetObjectById(model.Id);
                model = _rollerBuilderService.SoftDeleteObject(data,_itemService,_barringService,
                    _priceMutationService,_recoveryOrderDetailService,_coreBuilderService,
                    _warehouseItemService,_stockMutationService,_itemTypeService,_purchaseOrderDetailService,
                    _stockAdjustmentDetailService,_salesOrderDetailService);
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
