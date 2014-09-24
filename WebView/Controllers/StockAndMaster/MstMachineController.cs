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
    public class MstMachineController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("MachineController");
        private IMachineService _MachineService;
        private IItemService _itemService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        private IBarringService _barringService;
        public MstMachineController()
        {
            _MachineService = new MachineService(new MachineRepository(),new MachineValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
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
            var query = _MachineService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Machine>;

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
                            item.Code,
                            item.Name,
                            item.Description,
                            item.CreatedAt,
                            item.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            Machine model = new Machine();
            try
            {
              model = _MachineService.GetObjectById(Id);
           
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                model.Errors.Add("Generic", "Error" + ex);
            }

            return Json(new
            {
                model
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public dynamic Insert(Machine model)
        {
            try
            {
                model = _MachineService.CreateObject(model);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                model.Errors.Add("Insert Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Update(Machine model)
        {
            try
            {
                var data = _MachineService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Description = model.Description;
                model = _MachineService.UpdateObject(data);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                model.Errors.Add("Update Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(Machine model)
        {
            try
            {
                var data = _MachineService.GetObjectById(model.Id);
                model = _MachineService.SoftDeleteObject(data,_rollerBuilderService,_coreIdentificationDetailService,_barringService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                model.Errors.Add("Delete Failed", "Error : " + ex);
            }

            return Json(new
            {
                model.Errors
            });
        }
    }
}
