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
    public class MstCoreBuilderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ContactController");
        private ICoreBuilderService _coreBuilderService;
        private IItemService _itemService;
        private IUoMService _uomService;
        private IWarehouseService _warehouseService;
        private IItemTypeService _itemTypeService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IContactService _contactService;
        private IPriceMutationService _priceMutationService;
        private IContactGroupService _contactGroupService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IMachineService _machineService;
        private ContactGroup baseGroup;
        private IRollerTypeService _rollerTypeService;
        private ItemType typeAccessory, typeBar, typeBarring, typeBearing, typeBlanket, typeCore, typeCompound, typeChemical,
                        typeConsumable, typeGlue, typeUnderpacking, typeRoller;
        private RollerType typeDamp, typeFoundDT, typeInkFormX, typeInkDistD, typeInkDistM, typeInkDistE,
                        typeInkDuctB, typeInkDistH, typeInkFormW, typeInkDistHQ, typeDampFormDQ, typeInkFormY;
        public MstCoreBuilderController()
        {
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _uomService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
              _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(),new SalesOrderDetailValidator());
            _machineService = new MachineService(new MachineRepository(),new MachineValidator());
               _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
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
            var query = _coreBuilderService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<CoreBuilder>;

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
                    from item in list
                    select new
                    {
                        id = item.Id,
                        cell = new object[] {
                            item.Id,
                            item.Name,
                            item.Category,
                            item.UoMId,
                            _uomService.GetObjectById(item.UoMId).Name,
                            item.BaseSku,
                            item.SkuUsedCore, 
                            item.SkuNewCore,
                            _itemService.GetObjectById(item.UsedCoreItemId).Quantity,
                            _itemService.GetObjectById(item.NewCoreItemId).Quantity,
                            item.CreatedAt,
                            item.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            CoreBuilder model = new CoreBuilder();
            try
            {
                model = _coreBuilderService.GetObjectById(Id);
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
                model.SkuUsedCore,
                model.SkuNewCore,
                UsedCoreQuantity = _itemService.GetObjectById(model.UsedCoreItemId).Quantity,
                NewCoreQuantity = _itemService.GetObjectById(model.NewCoreItemId).Quantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CoreBuilder model)
        {
            try
            {
                model = _coreBuilderService.CreateObject(model,_uomService,_itemService,_itemTypeService,
                    _warehouseItemService,_warehouseService,_priceMutationService,_contactGroupService);
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
        public dynamic Update(CoreBuilder model)
        {
            try
            {
                var data = _coreBuilderService.GetObjectById(model.Id);
                data.Name = model.Name;
                model = _coreBuilderService.UpdateObject(data,_uomService,_itemService,_itemTypeService,_warehouseItemService
                    ,_warehouseService,_barringService,_contactService,_machineService,_priceMutationService,_contactGroupService);
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
        public dynamic Delete(CoreBuilder model)
        {
            try
            {
                var data = _coreBuilderService.GetObjectById(model.Id);
                model = _coreBuilderService.SoftDeleteObject(data, _itemService, _rollerBuilderService, 
                    _coreIdentificationDetailService, _recoveryOrderDetailService, _recoveryAccessoryDetailService, 
                    _warehouseItemService, _stockMutationService, _itemTypeService,_barringService,_purchaseOrderDetailService
                    ,_stockAdjustmentDetailService,_salesOrderDetailService,_priceMutationService);
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
