using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interface.Service;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.Repository;
using Service.Service;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.DomainModel;

namespace WebView.Controllers
{
    public class FinanceReportController : Controller
    {
        //
        // GET: /FinanceReport/
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("FinanceReportController");
        private IAccountService _accountService;
        private IClosingService _closingService;
        private IValidCombService _validCombService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private ICompanyService _companyService;

        public class ModelIncomeStatement
        {
            public string CompanyName { get; set; }
            public DateTime Date { get; set; }
            public string Title { get; set; }
            public DateTime RangeDate { get; set; }
            public string JenisRange { get; set; }
            public decimal Current { get; set; }
            public decimal Previous { get; set; }
            public string Group { get; set; }
            public int Level { get; set; }
            public string GroupLevel { get; set; }
            public string AccountCode { get; set; }
            public string Unaudited { get; set; }
        }

        public class ModelBalanceSheet
        {
            public string CompanyName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string DCNote { get; set; }
            public string AccountName { get; set; }
            public string AccountGroup { get; set; }
            public string AccountTitle { get; set; }
            public decimal CurrentAmount { get; set; }
            public decimal PrevAmount { get; set; }
            public string ASSET { get; set; }
            public string AccountCode { get; set; }
        }

        public FinanceReportController()
        {
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
        }

        public ActionResult IncomeStatement()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.IncomeStatement, Core.Constants.Constant.MenuGroupName.Report))
            {
                return Content(Core.Constants.Constant.PageViewNotAllowed);
            }

            return View();
        }

        // Revenue - Expense - TaxExpense - Divident = NetEarnings
        public ActionResult ReportIncomeStatement(int period, int yearPeriod)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectByPeriodAndYear(period, yearPeriod);

            IList<ModelIncomeStatement> incomeStatements = new List<ModelIncomeStatement>();

            var IncomeValidCombs = _validCombService.GetQueryable().Include("Account").Include("Closing")
                                                    .Where(x => x.ClosingId == closing.Id && (x.Account.Group == Core.Constants.Constant.AccountGroup.Revenue || 
                                                                                       x.Account.Group == Core.Constants.Constant.AccountGroup.Expense));
            List<ModelIncomeStatement> query = new List<ModelIncomeStatement>();
            query = (from obj in IncomeValidCombs
                         select new ModelIncomeStatement()
                         {
                             CompanyName = company.Name,
                             Date = closing.BeginningPeriod,
                             Title = obj.Account.Name,
                             RangeDate = closing.EndDatePeriod,
                             JenisRange = "Monthly",
                             Current = obj.Amount,
                             Previous = obj.Amount,
                             Group = ((obj.Account.Group == Core.Constants.Constant.AccountGroup.Expense) ? "Expense" :
                                     (obj.Account.Group == Core.Constants.Constant.AccountGroup.Revenue) ? "Revenue" : "dll"),
                             Level = obj.Account.Level,
                             GroupLevel = ((obj.Account.Group == Core.Constants.Constant.AccountGroup.Expense) ? "Expense" :
                                     (obj.Account.Group == Core.Constants.Constant.AccountGroup.Revenue) ? "Revenue" : "dll"),
                             AccountCode = obj.Account.Code,
                             Unaudited = "Unaudited"
                         }).ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Finance/IncomeStatement.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult BalanceSheet()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.BalanceSheet, Core.Constants.Constant.MenuGroupName.Report))
            {
                return Content(Core.Constants.Constant.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportBalanceSheet(int closingId)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectById(closingId);

            IList<ModelIncomeStatement> incomeStatements = new List<ModelIncomeStatement>();

            var IncomeValidCombs = _validCombService.GetQueryable().Include("Account").Include("Closing")
                                                    .Where(x => x.ClosingId == closing.Id & x.Account.Level == 2);
            List<ModelBalanceSheet> query = new List<ModelBalanceSheet>();
            query = (from obj in IncomeValidCombs
                     select new ModelBalanceSheet()
                     {
                         CompanyName = company.Name,
                         StartDate = closing.BeginningPeriod,
                         EndDate = closing.EndDatePeriod,
                         DCNote = (obj.Account.Group == Core.Constants.Constant.AccountGroup.Asset ||
                                  obj.Account.Group == Core.Constants.Constant.AccountGroup.Expense) ? "D" : "C",
                         AccountName = obj.Account.Code.Substring(0, 1),
                         AccountGroup = (obj.Account.Group == Core.Constants.Constant.AccountGroup.Asset) ? "Asset" :
                                        (obj.Account.Group == Core.Constants.Constant.AccountGroup.Expense) ? "Expense" :
                                        (obj.Account.Group == Core.Constants.Constant.AccountGroup.Liability) ? "Liability" :
                                        (obj.Account.Group == Core.Constants.Constant.AccountGroup.Equity) ? "Equity" :
                                        (obj.Account.Group == Core.Constants.Constant.AccountGroup.Revenue) ? "Revenue" : "",
                        AccountTitle = obj.Account.Name,
                        CurrentAmount = obj.Amount,
                        PrevAmount = obj.Amount,
                        ASSET = "nonASSET", // untuk Fix Asset ? "ASSET" : "nonASSET",
                        AccountCode = obj.Account.Code
                     }).OrderBy(x => x.AccountCode).ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Finance/BalanceSheet.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }
    }
}
