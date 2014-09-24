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
    public class RecoveryWorkOrderController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RecoveryWorkOrderController");
        private IItemService _itemService;
        private IWarehouseItemService _warehouseItemService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IWarehouseService _warehouseService;
        private IBarringOrderService _barringOrderService;
        private IBarringOrderDetailService _barringOrderDetailService;
        private IContactService _contactService;
        private IRecoveryOrderService _recoveryOrderService;
        private IRecoveryOrderDetailService _recoveryOrderDetailService;
        private IRecoveryAccessoryDetailService _recoveryAccessoryDetailService;
        private ICoreIdentificationService _coreIdentificationService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreBuilderService _coreBuilderService;
        private IItemTypeService _itemTypeService;

        public RecoveryWorkOrderController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(), new WarehouseItemValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(), new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _barringOrderService = new BarringOrderService(new BarringOrderRepository(), new BarringOrderValidator());
            _barringOrderDetailService = new BarringOrderDetailService(new BarringOrderDetailRepository(), new BarringOrderDetailValidator());
            _recoveryOrderService = new RecoveryOrderService(new RecoveryOrderRepository(), new RecoveryOrderValidator());
            _recoveryOrderDetailService = new RecoveryOrderDetailService(new RecoveryOrderDetailRepository(), new RecoveryOrderDetailValidator());
            _recoveryAccessoryDetailService = new RecoveryAccessoryDetailService(new RecoveryAccessoryDetailRepository(), new RecoveryAccessoryDetailValidator());
            _coreIdentificationService = new CoreIdentificationService(new CoreIdentificationRepository(), new CoreIdentificationValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _coreBuilderService = new CoreBuilderService(new CoreBuilderRepository(), new CoreBuilderValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
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
            var query =  _recoveryOrderService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RecoveryOrder>;

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
                            model.WarehouseId,
                            _warehouseService.GetObjectById(model.WarehouseId).Code,
                            _warehouseService.GetObjectById(model.WarehouseId).Name,
                            model.QuantityReceived,
                            model.QuantityFinal,
                            model.QuantityRejected,
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
            var query = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RecoveryOrderDetail>;

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
                            model.CoreIdentificationDetailId,
                            model.RollerBuilderId,
                            _rollerBuilderService.GetObjectById(model.RollerBuilderId).Name,
                            model.CoreTypeCase,
                            model.Acc,
                            model.RepairRequestCase == 1 ? "BearingSeat":"CentreDrill",
                            model.HasAccessory,
                            model.IsDisassembled,
                            model.IsStrippedAndGlued,
                            model.IsWrapped,
                            model.CompoundUsage,
                            model.IsVulcanized,
                            model.IsFacedOff,
                            model.IsConventionalGrinded,
                            model.IsCWCGrinded,
                            model.IsPolishedAndQC,
                            model.IsPackaged,
                            model.IsRejected,
                            model.RejectedDate,
                            model.IsFinished,
                            model.FinishedDate
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListAccessory(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(id).Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RecoveryAccessoryDetail>;

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
                            _itemService.GetObjectById(model.ItemId).Name,
                            model.Quantity
                        }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }


        public dynamic GetInfo(int Id)
        {
            RecoveryOrder model = new RecoveryOrder();
            try
            {
                model = _recoveryOrderService.GetObjectById(Id);
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
                model.WarehouseId,
                model.CoreIdentificationId,
                CoreIdentification =_coreIdentificationService.GetObjectById(model.CoreIdentificationId).Code,
                WarehouseCode = _warehouseService.GetObjectById(model.WarehouseId).Code,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.QuantityReceived,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            RecoveryOrderDetail model = new RecoveryOrderDetail();
            try
            {
                model = _recoveryOrderDetailService.GetObjectById(Id);
            
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.CoreIdentificationDetailId,
                model.RollerBuilderId,
                RollerBuilder = _rollerBuilderService.GetObjectById(model.RollerBuilderId).Name,
                model.CoreTypeCase,
                model.Acc,
                RepairRequestCase = model.RepairRequestCase == 1 ? "BearingSeat":"CentreDrill",
                model.HasAccessory,
                model.IsDisassembled,
                model.IsStrippedAndGlued,
                model.IsWrapped,
                model.IsVulcanized,
                model.IsFacedOff,
                model.IsConventionalGrinded,
                model.IsCWCGrinded,
                model.IsPolishedAndQC,
                model.IsPackaged,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoAccessory(int Id)
        {
            RecoveryAccessoryDetail model = new RecoveryAccessoryDetail();
            try
            {
                model = _recoveryAccessoryDetailService.GetObjectById(Id);

            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Id,
                model.ItemId,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(RecoveryOrder model)
        {
            try
            {
                model = _recoveryOrderService.CreateObject(model,_coreIdentificationService);
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
        public dynamic InsertDetail(RecoveryOrderDetail model)
        {
            try
            {
                model = _recoveryOrderDetailService.CreateObject(model,_recoveryOrderService,_coreIdentificationDetailService,_rollerBuilderService);
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
        public dynamic InsertAccessory(RecoveryAccessoryDetail model)
        {
            try
            {
                model = _recoveryAccessoryDetailService.CreateObject(model,_recoveryOrderService,_recoveryOrderDetailService
                    ,_itemService,_itemTypeService,_warehouseItemService);
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
        public dynamic Update(RecoveryOrder model)
        {
            try
            {
                var data = _recoveryOrderService.GetObjectById(model.Id);
                data.CoreIdentificationId = model.CoreIdentificationId;
                data.WarehouseId = model.WarehouseId;
                data.Code = model.Code;
                data.QuantityReceived = model.QuantityReceived;
                model = _recoveryOrderService.UpdateObject(data,_recoveryOrderDetailService,_coreIdentificationService);
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
        public dynamic Delete(RecoveryOrder model)
        {
            try
            {
                var data = _recoveryOrderService.GetObjectById(model.Id);
                model = _recoveryOrderService.SoftDeleteObject(model,_recoveryOrderDetailService,_recoveryAccessoryDetailService);
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
        public dynamic DeleteDetail(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.SoftDeleteObject(data,_recoveryOrderService,_recoveryAccessoryDetailService);
                 
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
        public dynamic DeleteAccessory(RecoveryAccessoryDetail model)
        {
            try
            {
                var data = _recoveryAccessoryDetailService.GetObjectById(model.Id);
                model = _recoveryAccessoryDetailService.SoftDeleteObject(data,_recoveryOrderDetailService);

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
        public dynamic UpdateDetail(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                data.CoreIdentificationDetailId = model.CoreIdentificationDetailId;
                data.RollerBuilderId = model.RollerBuilderId;
                data.CoreTypeCase = model.CoreTypeCase;
                data.Acc = model.Acc;
                data.RepairRequestCase = model.RepairRequestCase;
                data.HasAccessory = model.HasAccessory;
                model = _recoveryOrderDetailService.UpdateObject(data,_recoveryOrderService,_coreIdentificationDetailService,_rollerBuilderService);
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
        public dynamic UpdateAccessory(RecoveryAccessoryDetail model)
        {
            try
            {
                var data = _recoveryAccessoryDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                model = _recoveryAccessoryDetailService.UpdateObject(data,_recoveryOrderService,_recoveryOrderDetailService
                    ,_itemService,_itemTypeService,_warehouseItemService
                    );
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
        public dynamic ProgressDetail(int Id,string Progress,int CompoundUsage)
        {
            var model = new RecoveryOrderDetail();
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(Id);
                if (Progress == "IsDisassembled") { model = _recoveryOrderDetailService.DisassembleObject(data); }
                else if (Progress == "IsStrippedAndGlued") { model = _recoveryOrderDetailService.StripAndGlueObject(data); }
                else if (Progress == "IsWrapped") { model = _recoveryOrderDetailService.WrapObject(data, CompoundUsage); }
                else if (Progress == "IsVulcanized") { model = _recoveryOrderDetailService.VulcanizeObject(data); }
                else if (Progress == "IsFacedOff") { model = _recoveryOrderDetailService.FaceOffObject(data); }
                else if (Progress == "IsConventionalGrinded") { model = _recoveryOrderDetailService.ConventionalGrindObject(data); }
                else if (Progress == "IsCWCGrinded") { model = _recoveryOrderDetailService.CWCGrindObject(data); }
                else if (Progress == "IsPolishedAndQC") { model = _recoveryOrderDetailService.PolishAndQCObject(data); }
                else if (Progress == "IsPackaged") { model = _recoveryOrderDetailService.PackageObject(data); }
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
        public dynamic Confirm(RecoveryOrder model)
        {
            try
            {
                var data = _recoveryOrderService.GetObjectById(model.Id);
                model = _recoveryOrderService.ConfirmObject(data,model.ConfirmationDate.Value
                   ,_coreIdentificationDetailService,_recoveryOrderDetailService,_recoveryAccessoryDetailService,
                   _coreBuilderService,_stockMutationService,_itemService,_barringService,_warehouseItemService,_warehouseService);
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
        public dynamic UnConfirm(RecoveryOrder model)
        {
            try
            {

                var data = _recoveryOrderService.GetObjectById(model.Id);
                model = _recoveryOrderService.UnconfirmObject(data,_coreIdentificationDetailService,_recoveryOrderDetailService,_recoveryAccessoryDetailService,
                    _coreBuilderService,_stockMutationService,_itemService,_barringService,_warehouseItemService,_warehouseService);
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

        [HttpPost]
        public dynamic Finish(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.FinishObject(data, model.FinishedDate.Value,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService,
                    _coreBuilderService,_rollerBuilderService,_itemService,_warehouseItemService,
                    _barringService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Finish Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnFinish(RecoveryOrderDetail model)
        {
            try
            {

                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.UnfinishObject(data,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService
                    ,_coreBuilderService,_rollerBuilderService,_itemService,_warehouseItemService,_barringService
                    ,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unfinish Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Reject(RecoveryOrderDetail model)
        {
            try
            {
                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.RejectObject(data,model.RejectedDate.Value,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService,_coreBuilderService
                    ,_rollerBuilderService,_itemService,_warehouseItemService,_barringService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Reject Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic UnReject(RecoveryOrderDetail model)
        {
            try
            {

                var data = _recoveryOrderDetailService.GetObjectById(model.Id);
                model = _recoveryOrderDetailService.UndoRejectObject(data,_coreIdentificationService,
                    _coreIdentificationDetailService,_recoveryOrderService,_recoveryAccessoryDetailService,
                    _coreBuilderService,_rollerBuilderService,_itemService,_warehouseItemService,_barringService,_stockMutationService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unreject Failed", ex);
                model.Errors.Add("Generic", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

    }
}
