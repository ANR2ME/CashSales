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
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReportController");
        private IItemService _itemService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;
        private ICompanyService _companyService;
        private ICashBankService _cashBankService;
        private IPayableService _payableService;
        private IReceivableService _receivableService;
        private ICashSalesInvoiceService _cashSalesInvoiceService;
        private ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService;
        private ICashSalesReturnService _cashSalesReturnService;
        private ICashSalesReturnDetailService _cashSalesReturnDetailService;
        private ICustomPurchaseInvoiceService _customPurchaseInvoiceService;
        private ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService;

        public class ModelProfitLoss
        {
           public  DateTime StartDate { get; set; }
           public DateTime EndDate { get; set; }
           public decimal TotalSales { get; set; }
           public decimal TotalCoGS { get; set; }
           public decimal TotalSalesReturn { get; set; }
           public decimal TotalExpense { get; set; }
           public string CompanyName { get; set; }
           public string CompanyAddress { get; set; }
           public string CompanyContactNo { get; set; }
        }

        public class ModelFund
        {
            public DateTime FromDueDate { get; set; }
            public DateTime ToDueDate { get; set; }
            public DateTime CurDate { get; set; }
            public decimal cashBank { get; set; }
            public decimal receivable { get; set; }
            public decimal payable { get; set; }
            public decimal dailySalesProjection { get; set; }
            public string CompanyName { get; set; }
            public string CompanyAddress { get; set; }
            public string CompanyContactNo { get; set; }
        }

        public ReportController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());

            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesInvoiceDetailService = new CashSalesInvoiceDetailService(new CashSalesInvoiceDetailRepository(), new CashSalesInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _cashSalesReturnDetailService = new CashSalesReturnDetailService(new CashSalesReturnDetailRepository(), new CashSalesReturnDetailValidator());
            _customPurchaseInvoiceService = new CustomPurchaseInvoiceService(new CustomPurchaseInvoiceRepository(), new CustomPurchaseInvoiceValidator());
            _customPurchaseInvoiceDetailService = new CustomPurchaseInvoiceDetailService(new CustomPurchaseInvoiceDetailRepository(), new CustomPurchaseInvoiceDetailValidator());
        }

        public ActionResult Item()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Item, Core.Constants.Constant.MenuGroupName.Report))
            {
                return Content(Core.Constants.Constant.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportItem(int itemTypeId = 0)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _itemService.GetQueryable().Include("ItemType").Include("UoM");
            
            string filter = "true";
            ItemType itemType = _itemTypeService.GetObjectById(itemTypeId);
            if (itemType != null)
            {
                filter = "ItemTypeId == " + itemTypeId.ToString();
            }
            
            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Name,
                             model.ItemTypeId,
                             itemtype = model.ItemType.Name,
                             model.Sku,
                             uom = model.UoM.Name,
                             model.Quantity,
                             model.SellingPrice,
                             model.AvgPrice,
                             model.Margin,
                             model.PendingReceival,
                             model.PendingDelivery,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             cogs = model.AvgPrice * model.Quantity,
                         }).Where(filter).ToList();
          
            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Item.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult ProfitLoss()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.ProfitLoss, Core.Constants.Constant.MenuGroupName.Report))
            {
                return Content(Core.Constants.Constant.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportProfitLoss(DateTime startDate, DateTime endDate)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var cashSalesReturnPayables = _payableService.GetQueryable().Where(x => x.IsCompleted &&
                            x.CompletionDate.Value >= startDate && x.CompletionDate.Value <= endDate && x.PayableSource == Core.Constants.Constant.PayableSource.CashSalesReturn);
            decimal totalSalesReturnPayable = 0;
            foreach (var payable in cashSalesReturnPayables)
            {
                totalSalesReturnPayable += (payable.Amount - payable.AllowanceAmount);
            }
            
            var paymentRequestPayables = _payableService.GetQueryable().Where(x => x.IsCompleted &&
                            x.CompletionDate.Value >= startDate && x.CompletionDate.Value <= endDate && x.PayableSource == Core.Constants.Constant.PayableSource.PaymentRequest);
            decimal totalPaymentRequestPayable = 0;
            foreach (var payable in cashSalesReturnPayables)
            {
                totalPaymentRequestPayable += (payable.Amount - payable.AllowanceAmount);
            }

            var cashSalesInvoices = _cashSalesInvoiceService.GetQueryable().Where(x => x.IsPaid &&
                            x.ConfirmationDate.Value >= startDate && x.ConfirmationDate.Value <= endDate).ToList();
            decimal totalCashSales = 0;
            decimal totalCoGS = 0;
            foreach (var cashSales in cashSalesInvoices)
            {
                totalCashSales += (cashSales.Total - cashSales.Allowance);
                totalCoGS += cashSales.CoGS;
            }

            List<ModelProfitLoss> query = new List<ModelProfitLoss>();
            var profitloss = new ModelProfitLoss
                         {
                             StartDate = startDate.Date,
                             EndDate = endDate.Date,
                             TotalSales = totalCashSales,
                             TotalCoGS = totalCoGS,
                             TotalSalesReturn = totalSalesReturnPayable,
                             TotalExpense = totalPaymentRequestPayable,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                         };

            query.Add(profitloss);

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/ProfitLoss.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult Sales()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.Sales, Core.Constants.Constant.MenuGroupName.Report))
            {
                return Content(Core.Constants.Constant.PageViewNotAllowed);
            }

            return View();
        }

        public ActionResult ReportSales(DateTime startDate, DateTime endDate)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _cashSalesInvoiceService.GetQueryable().Include("CashBank").Include("Warehouse").Where(x => x.IsPaid && x.SalesDate >= startDate && x.SalesDate <= endDate);

            var query = (from model in q
                         select new
                         {
                             StartDate = startDate,
                             EndDate = endDate,
                             model.Code,
                             model.Description,
                             model.SalesDate,
                             ConfirmationDate = model.ConfirmationDate.Value,
                             DueDate = model.DueDate.Value,
                             model.Discount,
                             model.Tax,
                             model.Allowance,
                             model.CoGS,
                             model.Total,
                             CashBank = model.CashBank.Name,
                             Warehouse = model.Warehouse.Name,
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo
                         }).ToList();

            var rd = new ReportDocument();

            if (!query.Any()) return Content("");

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Sales.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult Funds()
        {
            return View();
        }

        public ActionResult ReportFunds(int Id, DateTime DueDate, decimal Discount, decimal Tax, decimal Allowance, decimal DailySalesProjection, bool IncludeSaturdaySales, bool IncludeSundaySales)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            DateTime startDate = DateTime.Today;
            DateTime endDate = DueDate.Date;

            var data = _customPurchaseInvoiceService.GetObjectById(Id);
            data.DueDate = DueDate;
            data.Discount = Discount;
            data.Tax = Tax;
            data.Allowance = Allowance;
            decimal total = _customPurchaseInvoiceService.CalculateTotalAmountAfterDiscountAndTax(data) - Allowance;
            decimal totalcashbank = _cashBankService.GetTotalCashBank();

            DateTime curDate = startDate;
            decimal funds = totalcashbank;
            var query = new List<ModelFund>();
            while (curDate <= endDate)
            {
                decimal receivable = _receivableService.GetTotalRemainingAmountByDueDate(curDate, curDate);
                decimal payable = _payableService.GetTotalRemainingAmountByDueDate(curDate, curDate);
                decimal sales = 0;
                if ((curDate.DayOfWeek != DayOfWeek.Saturday && curDate.DayOfWeek != DayOfWeek.Sunday) ||
                    (curDate.DayOfWeek == DayOfWeek.Saturday && IncludeSaturdaySales) ||
                    (curDate.DayOfWeek == DayOfWeek.Sunday && IncludeSundaySales))
                {
                    sales = DailySalesProjection;
                }

                var curFund = new ModelFund()
                {
                    FromDueDate = startDate,
                    ToDueDate = endDate,
                    CurDate = curDate,
                    cashBank = funds,
                    payable = payable,
                    receivable = receivable,
                    dailySalesProjection = sales,
                    CompanyName = company.Name,
                    CompanyAddress = company.Address,
                    CompanyContactNo = company.ContactNo
                };
                query.Add(curFund);

                funds += sales;
                funds += receivable;
                funds -= payable;

                curDate = curDate.AddDays(1);
            }

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Funds.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Clear();
            //response.Content = new StreamContent(pdf);
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            //return response;
            
            return File(stream, "application/pdf");
        }

        public ActionResult PurchaseInvoice()
        {
            return View();
        }

        public ActionResult ReportPurchaseInvoice(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _customPurchaseInvoiceDetailService.GetQueryableObjectsByCustomPurchaseInvoiceId(Id).Include("CustomPurchaseInvoice").Include("Item").Include("UoM");
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = model.ListedUnitPrice,
                             model.Discount,
                             GlobalDiscount = model.CustomPurchaseInvoice.Discount,
                             Tax = model.CustomPurchaseInvoice.Tax,
                             Allowance = model.CustomPurchaseInvoice.Allowance,
                             Code = model.CustomPurchaseInvoice.Code,
                             Date = model.CustomPurchaseInvoice.ConfirmationDate.Value,
                             contact = "",
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = model.CustomPurchaseInvoice.Description,
                         }).ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/PurchaseInvoice.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult SalesInvoice()
        {
            return View();
        }

        public ActionResult ReportSalesInvoice(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var cashSalesInvoice = _cashSalesInvoiceService.GetObjectById(Id);
            var q = _cashSalesInvoiceDetailService.GetQueryableObjectsByCashSalesInvoiceId(Id).Include("CashSalesInvoice").Include("Item").Include("UoM");
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.Item.Sku,
                             Name = model.Item.Name,
                             UoM = model.Item.UoM.Name,
                             model.Quantity,
                             Price = model.IsManualPriceAssignment? model.AssignedPrice : model.Item.SellingPrice,
                             model.Discount,
                             GlobalDiscount = model.CashSalesInvoice.Discount,
                             Tax = model.CashSalesInvoice.Tax,
                             Allowance = model.CashSalesInvoice.Allowance,
                             Code = model.CashSalesInvoice.Code,
                             Date = model.CashSalesInvoice.ConfirmationDate.Value,
                             contact = "",
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = model.CashSalesInvoice.Description,
                         }).ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/SalesInvoice.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        public ActionResult SalesReturnInvoice()
        {
            return View();
        }

        public ActionResult ReportSalesReturnInvoice(int Id)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            //var cashSalesReturnInvoice = _cashSalesInvoiceService.GetObjectById(Id);
            var q = _cashSalesReturnDetailService.GetQueryableObjectsByCashSalesReturnId(Id).Include("CashSalesReturn").Include("CashSalesInvoiceDetail").Include("CashSalesInvoice").Include("Item").Include("UoM");
            string user = AuthenticationModel.GetUserName();

            var query = (from model in q
                         select new
                         {
                             SKU = model.CashSalesInvoiceDetail.Item.Sku,
                             Name = model.CashSalesInvoiceDetail.Item.Name,
                             UoM = model.CashSalesInvoiceDetail.Item.UoM.Name,
                             Quantity = model.Quantity,
                             Price = model.CashSalesInvoiceDetail.IsManualPriceAssignment ? model.CashSalesInvoiceDetail.AssignedPrice : model.CashSalesInvoiceDetail.Item.SellingPrice,
                             Amount = model,
                             Discount = model.CashSalesInvoiceDetail.Discount,
                             GlobalDiscount = model.CashSalesInvoiceDetail.CashSalesInvoice.Discount,
                             Tax = model.CashSalesInvoiceDetail.CashSalesInvoice.Tax,
                             Allowance = model.CashSalesReturn.Allowance,
                             Code = model.CashSalesReturn.Code,
                             SalesCode = model.CashSalesInvoiceDetail.CashSalesInvoice.Code,
                             Date = model.CashSalesReturn.ConfirmationDate.Value,
                             contact = "",
                             CompanyName = company.Name,
                             CompanyAddress = company.Address,
                             CompanyContactNo = company.ContactNo,
                             User = user,
                             Description = model.CashSalesReturn.Description,
                         }).ToList();

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/SalesReturnInvoice.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

    }
}
