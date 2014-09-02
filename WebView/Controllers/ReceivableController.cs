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
    public class ReceivableController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReceivableController");
        private IReceivableService _receivableService;
        private IContactService _contactService;

        public ReceivableController()
        {
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Receivable, Core.Constants.Constant.MenuGroupName.Transaction))
            {
                return Content("You are not allowed to View this Page.");
            }

            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _receivableService.GetAll().Where(d => d.IsDeleted == false);

            var list = query as IEnumerable<Receivable>;

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
                    from receivable in list
                    select new
                    {
                        id = receivable.Id,
                        cell = new object[] {
                            receivable.Id,
                            receivable.ContactId,
                            _contactService.GetObjectById(receivable.ContactId).Name,
                            receivable.Code,
                            receivable.ReceivableSource,
                            receivable.ReceivableSourceId,
                            receivable.Amount,
                            receivable.RemainingAmount,
                            receivable.PendingClearanceAmount,
                            receivable.AllowanceAmount,
                            receivable.DueDate,
                            receivable.IsCompleted,
                            receivable.CompletionDate,
                            receivable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, DateTime startdate, DateTime enddate, string filters = "")
        {
            // Construct where statement

            string strWhere = GeneralFunction.ConstructWhere(filters);

            // Get Data
            var query = _receivableService.GetAll().Where(d => d.CreatedAt >= startdate && d.CreatedAt < enddate.AddDays(1) && d.IsDeleted == false);

            var list = query as IEnumerable<Receivable>;

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
                    from receivable in list
                    select new
                    {
                        id = receivable.Id,
                        cell = new object[] {
                            receivable.Id,
                            receivable.ContactId,
                            _contactService.GetObjectById(receivable.ContactId).Name,
                            receivable.Code,
                            receivable.ReceivableSource,
                            receivable.ReceivableSourceId,
                            receivable.Amount,
                            receivable.RemainingAmount,
                            receivable.PendingClearanceAmount,
                            receivable.AllowanceAmount,
                            receivable.DueDate,
                            receivable.IsCompleted,
                            receivable.CompletionDate,
                            receivable.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}