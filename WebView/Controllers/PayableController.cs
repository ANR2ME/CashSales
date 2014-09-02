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
    public class PayableController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PayableController");
        private IPayableService _payableService;
        private IContactService _contactService;

        public PayableController()
        {
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Payable, Core.Constants.Constant.MenuGroupName.Transaction))
            {
                return Content("You are not allowed to View this Page.");
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
            var q = _payableService.GetQueryable().Include("Contact");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.ContactId,
                             contact = model.Contact.Name,
                             model.Code,
                             model.PayableSource,
                             model.PayableSourceId,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             model.AllowanceAmount,
                             model.DueDate,
                             model.IsCompleted,
                             model.CompletionDate,
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
                            model.ContactId,
                            model.contact,
                            model.Code,
                            model.PayableSource,
                            model.PayableSourceId,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.AllowanceAmount,
                            model.DueDate,
                            model.IsCompleted,
                            model.CompletionDate,
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
            var q = _payableService.GetQueryable().Include("Contact");

            var query = (from model in q
                         //where model.CreatedAt >= EntityFunctions.AddMicroseconds(startdate, 0) && model.CreatedAt < EntityFunctions.AddDays(enddate, 1)
                         select new
                         {
                             model.Id,
                             model.ContactId,
                             contact = model.Contact.Name,
                             model.Code,
                             model.PayableSource,
                             model.PayableSourceId,
                             model.Amount,
                             model.RemainingAmount,
                             model.PendingClearanceAmount,
                             model.AllowanceAmount,
                             model.DueDate,
                             model.IsCompleted,
                             model.CompletionDate,
                             model.CreatedAt,
                         }
                         ).Where(filter, startdate.GetValueOrDefault().Date, enddate.GetValueOrDefault().AddDays(1).Date).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.ContactId,
                            model.contact,
                            model.Code,
                            model.PayableSource,
                            model.PayableSourceId,
                            model.Amount,
                            model.RemainingAmount,
                            model.PendingClearanceAmount,
                            model.AllowanceAmount,
                            model.DueDate,
                            model.IsCompleted,
                            model.CompletionDate,
                            model.CreatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}