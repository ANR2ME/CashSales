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
        private IPayableService _payableService;
        private IReceivableService _receivableService;
        private ICashSalesInvoiceService _cashSalesInvoiceService;
        private ICashSalesReturnService _cashSalesReturnService;


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

        public ReportController()
        {
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
            _companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
        }

        public ActionResult Item()
        {
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
            return View();
        }

        public ActionResult ReportSales(DateTime startDate, DateTime endDate)
        {
            var company = _companyService.GetQueryable().FirstOrDefault();
            var q = _cashSalesInvoiceService.GetQueryable().Include("CashBank").Include("Warehouse").Where(x => x.IsPaid && x.SalesDate >= startDate && x.SalesDate <= endDate);

            var query = (from model in q
                         select new
                         {
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

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/Sales.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }


    }
}
