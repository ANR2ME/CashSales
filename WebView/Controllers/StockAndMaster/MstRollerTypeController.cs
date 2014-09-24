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
    public class MstRollerTypeController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RollerTypeController");
        private IRollerTypeService _rollerTypeService;
        private IRollerBuilderService _rollerBuilderService;
        private ICoreIdentificationDetailService _coreIdentificationDetailService;
        public MstRollerTypeController()
        {  
            _rollerTypeService = new RollerTypeService(new RollerTypeRepository(), new RollerTypeValidator());
            _rollerBuilderService = new RollerBuilderService(new RollerBuilderRepository(), new RollerBuilderValidator());
            _coreIdentificationDetailService = new CoreIdentificationDetailService(new CoreIdentificationDetailRepository(), new CoreIdentificationDetailValidator());
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
            var query = _rollerTypeService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<RollerType>;

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
                            item.Description,
                            item.CreatedAt,
                            item.UpdatedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            RollerType model = new RollerType();
            try
            {
                model = _rollerTypeService.GetObjectById(Id);
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
        public dynamic Insert(RollerType model)
        {
            try
            {
                model = _rollerTypeService.CreateObject(model);
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
        public dynamic Update(RollerType model)
        {
            try
            {
                var data = _rollerTypeService.GetObjectById(model.Id);
                data.Name = model.Name;
                data.Description = model.Description;
                model = _rollerTypeService.UpdateObject(data);
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
        public dynamic Delete(RollerType model)
        {
            try
            {
                var data = _rollerTypeService.GetObjectById(model.Id);
                model = _rollerTypeService.SoftDeleteObject(data,_rollerBuilderService,_coreIdentificationDetailService);
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

