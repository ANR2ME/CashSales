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
    public class ValidCombController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ValidCombController");

        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IAccountService _accountService;
        private IValidCombService _validCombService;
        private IClosingService _closingService;

        public ValidCombController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.ValidComb, Core.Constants.Constant.MenuGroupName.Report))
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
            var q = _validCombService.GetQueryable().Include("Account").Include("Closing").Where(x => x.Closing.IsClosed);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Account.Code,
                            model.Account.Name,
                            model.Closing.Period,
                            model.Closing.YearPeriod,
                            model.Closing.BeginningPeriod,
                            model.Closing.EndDatePeriod,
                            model.Amount
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
                            model.Name,
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                            model.Amount
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListByDate(string _search, long nd, int rows, int? page, string sidx, string sord, int? ClosingId, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            if (ClosingId.HasValue)
            {
                filter = "(" + filter + ") AND ClosingId == @0";
            }

            // Get Data
            var q = _validCombService.GetQueryable().Include("Account").Include("Closing").Where(x => x.Closing.IsClosed);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Account.Code,
                             model.Account.Name,
                             model.Closing.Period,
                             model.Closing.YearPeriod,
                             model.Closing.BeginningPeriod,
                             model.Closing.EndDatePeriod,
                             model.Amount,
                             ClosingId = model.Closing.Id
                         }).Where(filter, ClosingId.GetValueOrDefault()).OrderBy(sidx + " " + sord); //.ToList();

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
                            model.Name,
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                            model.Amount,
                            model.ClosingId
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}