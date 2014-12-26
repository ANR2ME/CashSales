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
using Core.Constants;
using Data.Context;
using System.Data.Objects;
using System.Data.Objects.SqlClient;

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
            public DateTime BeginningDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal Revenue { get; set; }
            public decimal COGS { get; set; }
            public decimal OperationalExpenses { get; set; }
            public decimal InterestEarning { get; set; }
            public decimal Depreciation { get; set; }
            public decimal Amortization { get; set; }
            public decimal Tax { get; set; }
            public decimal Divident { get; set; }
        }

        public class ModelIncomeStatementDetail
        {
            public string CompanyName { get; set; }
            public DateTime Date { get; set; }
            public DateTime RangeDate { get; set; }
            public string Title { get; set; }
            public string JenisRange { get; set; }
            public decimal Current { get; set; }
            public decimal Previous { get; set; }
            public string Group { get; set; }
            public string Level { get; set; }
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
            public string AccountParent { get; set; }
            public string AccountTitle { get; set; }
            public decimal CurrentAmount { get; set; }
            public decimal PrevAmount { get; set; }
            public string ASSET { get; set; }
            public string AccountCode { get; set; }
            public string AccountParentCode { get; set; }
            public int AccountLevel { get; set; }
            public bool IsLeaf { get; set; }
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
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ErrorPage.PageViewNotAllowed);
            }

            return View();
        }

        // Revenue - Expense - TaxExpense - Divident = NetEarnings
        public ActionResult ReportIncomeStatement(int period, int yearPeriod)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectByPeriodAndYear(period, yearPeriod);

            if (closing == null) return Content(Constant.ErrorPage.ClosingNotFound);

            ValidComb Revenue = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Revenue).Id, closing.Id);
            //ValidComb Sales = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.SalesRevenue).Id, closing.Id);
            ValidComb COGS = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.COGS).Id, closing.Id);
            // Memorial Expenses
            ValidComb OperationalExpenses = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.OperationalExpenses).Id, closing.Id);
            ValidComb InterestEarning = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.InterestEarning).Id, closing.Id);
            ValidComb Depreciation = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Depreciation).Id, closing.Id);
            ValidComb Amortization = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Amortization).Id, closing.Id);
            ValidComb Tax = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Tax).Id, closing.Id);
            ValidComb Divident = _validCombService.FindOrCreateObjectByAccountAndClosing(_accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.Divident).Id, closing.Id);
            // TODO
            //decimal OperationalExpensesAmount = 0, InterestEarningAmount = 0, DepreciationAmount = 0, AmortizationAmount = 0, TaxAmount = 0, DividentAmount = 0;
 
            ModelIncomeStatement model = new ModelIncomeStatement()
            {
                CompanyName = company.Name,
                BeginningDate = closing.BeginningPeriod.Date,
                EndDate = closing.EndDatePeriod.Date,
                Revenue = Revenue.Amount,
                COGS = COGS.Amount,
                OperationalExpenses = OperationalExpenses.Amount,
                InterestEarning = InterestEarning.Amount,
                Depreciation = Depreciation.Amount,
                Amortization = Amortization.Amount,
                Tax = Tax.Amount,
                Divident = Divident.Amount
            };

            List<ModelIncomeStatement> list = new List<ModelIncomeStatement>();
            list.Add(model);

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Finance/IncomeStatement.rpt");

            // Setting report data source
            rd.SetDataSource(list);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult IncomeStatementDetail()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ErrorPage.PageViewNotAllowed);
            }

            return View();
        }

        // Revenue - Expense - TaxExpense - Divident = NetEarnings
        public ActionResult ReportIncomeStatementDetail(int period, int yearPeriod)
        {
            DateTime startDate = new DateTime(yearPeriod, period, 1);
            DateTime prevStartDate = startDate.AddMonths(-1);
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectByPeriodAndYear(period, yearPeriod);

            if (closing == null) return Content(Constant.ErrorPage.ClosingNotFound);
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                var q = db.ValidCombs.Include("Closing").Include("Account").Where(x => x.Closing.BeginningPeriod == startDate && (x.Account.Group == Constant.AccountGroup.Expense || x.Account.Group == Constant.AccountGroup.Revenue)).OrderBy(x => x.Account.Code);
                var prevq = db.ValidCombs.Include("Closing").Include("Account").Where(x => x.Closing.BeginningPeriod == prevStartDate && (x.Account.Group == Constant.AccountGroup.Expense || x.Account.Group == Constant.AccountGroup.Revenue)); //.OrderByDescending(x => x.Closing.EndDatePeriod);

                var query = (from model in q
                             select new
                             {
                                 CompanyName = company.Name,
                                 Date = model.Closing.BeginningPeriod,
                                 RangeDate = model.Closing.EndDatePeriod,
                                 Title = model.Account.Name,
                                 JenisRange = "Monthly",
                                 Current = model.Amount,
                                 Previous = (decimal?)prevq.Where(x => x.AccountId == model.AccountId).FirstOrDefault().Amount ?? 0,
                                 Group = (model.Account.Code.Substring(0, Constant.AccountCode.Revenue.Length) == Constant.AccountCode.Revenue) ? Constant.AccountCode.Revenue : // "Sales ( Gross )"
                                         (model.Account.Code.Substring(0, Constant.AccountCode.COGS.Length) == Constant.AccountCode.COGS) ? Constant.AccountCode.COGS : // "Cost of Goods Sold" 
                                         (model.Account.Code.Substring(0, Constant.AccountCode.OperationalExpenses.Length) == Constant.AccountCode.OperationalExpenses) ? Constant.AccountCode.OperationalExpenses : // "Operational Expenses"
                                         (model.Account.Code.Substring(0, 3) == "214") ? "214" : // "Non-Operational Expenses"
                                         (model.Account.Code.Substring(0, Constant.AccountCode.InterestEarning.Length) == Constant.AccountCode.InterestEarning) ? Constant.AccountCode.InterestEarning :
                                         (model.Account.Code.Substring(0, Constant.AccountCode.Depreciation.Length) == Constant.AccountCode.Depreciation) ? Constant.AccountCode.Depreciation :
                                         (model.Account.Code.Substring(0, Constant.AccountCode.Amortization.Length) == Constant.AccountCode.Amortization) ? Constant.AccountCode.Amortization :
                                         (model.Account.Code.Substring(0, Constant.AccountCode.Tax.Length) == Constant.AccountCode.Tax) ? Constant.AccountCode.Tax :
                                         (model.Account.Code.Substring(0, Constant.AccountCode.Divident.Length) == Constant.AccountCode.Divident) ? Constant.AccountCode.Divident : "Other", //SqlFunctions.StringConvert(model.Account.Group), // "Sales (Netto)", "Cost of Goods Sold"
                                 Level = SqlFunctions.StringConvert((decimal?)model.Account.Level), // not used
                                 GroupLevel = "GOODS", // "EXP", "IMP", "DOM", "SOT" // "GOODS", "SERVS"
                                 AccountCode = model.Account.Code,
                                 //GroupIndex = 0,
                                 Unaudited = "",
                             }).ToList();

                var rd = new ReportDocument();

                //Loading Report
                rd.Load(Server.MapPath("~/") + "Reports/Finance/IncStt.rpt");

                // Setting report data source
                rd.SetDataSource(query);

                var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf");
            }
        }

        public ActionResult BalanceSheet()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ErrorPage.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportBalanceSheet(Nullable<int> closingId)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectById(closingId.GetValueOrDefault());

            if (closing == null) return Content(Constant.ErrorPage.ClosingNotFound);

            var balanceValidComb = _validCombService.GetQueryable().Include("Account").Include("Closing")
                                                    .Where(x => x.ClosingId == closing.Id & x.Account.Level == 2 
                                                    && x.Account.Group != Constant.AccountGroup.Expense && x.Account.Group != Constant.AccountGroup.Revenue
                                                    );

            List<ModelBalanceSheet> query = new List<ModelBalanceSheet>();
            query = (from obj in balanceValidComb
                     select new ModelBalanceSheet()
                     {
                         CompanyName = company.Name,
                         StartDate = closing.BeginningPeriod.Date,
                         EndDate = closing.EndDatePeriod.Date,
                         DCNote = (obj.Account.Group == Constant.AccountGroup.Asset ||
                                  obj.Account.Group == Constant.AccountGroup.Expense) ? "D" : "C",
                         AccountName = obj.Account.Code.Substring(0, 1),
                         AccountGroup = (obj.Account.Group == Constant.AccountGroup.Asset) ? "Asset" :
                                        (obj.Account.Group == Constant.AccountGroup.Expense) ? "Expense" :
                                        (obj.Account.Group == Constant.AccountGroup.Liability) ? "Liability" :
                                        (obj.Account.Group == Constant.AccountGroup.Equity) ? "Equity" :
                                        (obj.Account.Group == Constant.AccountGroup.Revenue) ? "Revenue" : "",
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

        public ActionResult BalanceSheetDetail()
        {
            if (!AuthenticationModel.IsAllowed("View", Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report))
            {
                return Content(Constant.ErrorPage.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportBalanceSheetDetail(Nullable<int> closingId)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            Closing closing = _closingService.GetObjectById(closingId.GetValueOrDefault());

            if (closing == null) return Content(Constant.ErrorPage.ClosingNotFound);

            var balanceValidComb = _validCombService.GetQueryable().Include("Account").Include("Closing")
                                                    .Where(x => x.ClosingId == closing.Id & x.Account.Level >= 2
                                                    && x.Account.Group != Constant.AccountGroup.Expense && x.Account.Group != Constant.AccountGroup.Revenue
                                                    );

            List<ModelBalanceSheet> query = new List<ModelBalanceSheet>();
            query = (from obj in balanceValidComb
                     select new ModelBalanceSheet()
                     {
                         CompanyName = company.Name,
                         StartDate = closing.BeginningPeriod.Date,
                         EndDate = closing.EndDatePeriod.Date,
                         DCNote = (obj.Account.Group == Constant.AccountGroup.Asset ||
                                  obj.Account.Group == Constant.AccountGroup.Expense) ? "D" : "C",
                         AccountName = obj.Account.Code.Substring(0, 1),
                         AccountGroup = (obj.Account.Group == Constant.AccountGroup.Asset) ? "Asset" :
                                        (obj.Account.Group == Constant.AccountGroup.Expense) ? "Expense" :
                                        (obj.Account.Group == Constant.AccountGroup.Liability) ? "Liability" :
                                        (obj.Account.Group == Constant.AccountGroup.Equity) ? "Equity" :
                                        (obj.Account.Group == Constant.AccountGroup.Revenue) ? "Revenue" : "",
                         AccountTitle = obj.Account.Name,
                         AccountParent = obj.Account.Parent.Name,
                         CurrentAmount = obj.Amount,
                         PrevAmount = obj.Amount,
                         ASSET = "nonASSET", // untuk Fix Asset ? "ASSET" : "nonASSET",
                         AccountCode = obj.Account.Code,
                         AccountParentCode = obj.Account.Parent.Code,
                         AccountLevel = obj.Account.Level,
                         IsLeaf = obj.Account.IsLeaf,
                     }).OrderBy(x => x.AccountCode).ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Finance/BalanceSheetDetail.rpt");
           
            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

    }
}
