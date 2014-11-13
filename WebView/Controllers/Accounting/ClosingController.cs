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
using Core.Constants;

namespace WebView.Controllers
{
    public class ClosingController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ClosingController");

        private IAccountService _accountService;
        private IClosingService _closingService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IValidCombService _validCombService;

        public ClosingController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.Closing, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ErrorPage.PageViewNotAllowed);
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
            var q = _closingService.GetQueryable().Include("Account");

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                            model.IsClosed,
                            model.ClosedAt
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
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                            model.IsClosed,
                            model.ClosedAt
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetClosedList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _closingService.GetQueryable().Include("Account").Where(x => x.IsClosed);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Period,
                             model.YearPeriod,
                             model.BeginningPeriod,
                             model.EndDatePeriod,
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
                            model.Period,
                            model.YearPeriod,
                            model.BeginningPeriod,
                            model.EndDatePeriod,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(Closing model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Constant.MenuName.Closing, Constant.MenuGroupName.Report))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _closingService.CreateObject(model, _accountService, _validCombService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Close(Closing model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Constant.MenuName.Closing, Constant.MenuGroupName.Report))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Close Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _closingService.GetObjectById(model.Id);
                data.ClosedAt = model.ClosedAt;
                model = _closingService.CloseObject(data, _accountService, _generalLedgerJournalService, _validCombService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Open(Closing model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnConfirm", Constant.MenuName.Closing, Constant.MenuGroupName.Report))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Reopen Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _closingService.GetObjectById(model.Id);
                model = _closingService.OpenObject(data, _accountService, _validCombService);
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }

        [HttpPost]
        public dynamic Delete(Closing model)
        {
            Dictionary<string, string> Errors = new Dictionary<string, string>();
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Constant.MenuName.Closing, Constant.MenuGroupName.Report))
                {
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");
                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                bool result = _closingService.DeleteObject(model.Id, _accountService, _validCombService);
                if (!result) { Errors.Add("Generic", "Tidak boleh sudah ditutup"); }
            }
            catch (Exception ex)
            {
                LOG.Error("Confirm Failed", ex);
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Errors
            });
        }
    }
}