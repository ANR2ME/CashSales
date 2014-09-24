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
                return Content(Core.Constants.Constant.PageViewNotAllowed);
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
            var q = _receivableService.GetQueryable().Include("Contact");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             contact = model.Contact.Name,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
                             model.ReceivableSourceCode,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             model.AllowanceAmount,
                             model.IsCompleted,
                             model.CompletionDate,
                             model.DueDate,
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
                            model.Code,
                            model.ContactId,
                            model.contact,
                            model.ReceivableSource,
                            model.ReceivableSourceId,
                            model.ReceivableSourceCode,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.AllowanceAmount,
                            model.IsCompleted,
                            model.CompletionDate,
                            model.DueDate,
                            model.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, DateTime startdate, DateTime enddate, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            filter += " AND CreatedAt >= @0 AND CreatedAt < @1";

            // Get Data
            var q = _receivableService.GetQueryable().Include("Contact");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.ContactId,
                             contact = model.Contact.Name,
                             model.ReceivableSource,
                             model.ReceivableSourceId,
                             model.ReceivableSourceCode,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             model.AllowanceAmount,
                             model.IsCompleted,
                             model.CompletionDate,
                             model.DueDate,
                             model.CreatedAt,
                         }).Where(filter, startdate.Date, enddate.Date.AddDays(1)).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.Code,
                            model.ContactId,
                            model.contact,
                            model.ReceivableSource,
                            model.ReceivableSourceId,
                            model.ReceivableSourceCode,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.AllowanceAmount,
                            model.IsCompleted,
                            model.CompletionDate,
                            model.DueDate,
                            model.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}