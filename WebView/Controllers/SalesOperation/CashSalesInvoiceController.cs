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
    public class CashSalesInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CashSalesInvoiceController");
        private IItemService _itemService;
        private IContactService _contactService;
        private IItemTypeService _itemTypeService;
        private IUoMService _uoMService;
        private IWarehouseItemService _warehouseItemService;
        private IWarehouseService _warehouseService;
        private IStockMutationService _stockMutationService;
        private IBarringService _barringService;
        private IPriceMutationService _priceMutationService;
        private IContactGroupService _contactGroupService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private ICashBankService _cashBankService;
        private ICashMutationService _cashMutationService;
        private ICashSalesInvoiceService _cashSalesInvoiceService;
        private ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService;
        private ICashSalesReturnService _cashSalesReturnService;
        private IQuantityPricingService _quantityPricingService;
        private IReceivableService _receivableService;
        private IReceiptVoucherService _receiptVoucherService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        private IClosingService _closingService;
        private IValidCombService _validCombService;

        public CashSalesInvoiceController()
        {
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _itemTypeService = new ItemTypeService(new ItemTypeRepository(),new ItemTypeValidator());
            _uoMService = new UoMService(new UoMRepository(), new UoMValidator());
            _warehouseItemService = new WarehouseItemService(new WarehouseItemRepository(),new WarehouseItemValidator());
            _warehouseService = new WarehouseService(new WarehouseRepository(), new WarehouseValidator());
            _stockMutationService = new StockMutationService(new StockMutationRepository(),new StockMutationValidator());
            _barringService = new BarringService(new BarringRepository(), new BarringValidator());
            _priceMutationService = new PriceMutationService(new PriceMutationRepository(), new PriceMutationValidator());
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _stockAdjustmentDetailService = new StockAdjustmentDetailService(new StockAdjustmentDetailRepository(), new StockAdjustmentDetailValidator());
            _cashBankService = new CashBankService(new CashBankRepository(), new CashBankValidator());
            _cashMutationService = new CashMutationService(new CashMutationRepository(), new CashMutationValidator());
            _cashSalesInvoiceService = new CashSalesInvoiceService(new CashSalesInvoiceRepository(), new CashSalesInvoiceValidator());
            _cashSalesInvoiceDetailService = new CashSalesInvoiceDetailService(new CashSalesInvoiceDetailRepository(), new CashSalesInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(), new QuantityPricingValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _accountService = new AccountService(new AccountRepository(), new AccountValidator());
            _generalLedgerJournalService = new GeneralLedgerJournalService(new GeneralLedgerJournalRepository(), new GeneralLedgerJournalValidator());
            _closingService = new ClosingService(new ClosingRepository(), new ClosingValidator());
            _validCombService = new ValidCombService(new ValidCombRepository(), new ValidCombValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
            {
                return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            }

            return View();
        }

        public dynamic GetList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "", string findSKU = null)
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _cashSalesInvoiceService.GetQueryable().Include("CashBank").Include("Warehouse").Include("CashSalesDetails").Where(x => x.CashSalesInvoiceDetails.Where(y => findSKU == null || y.Item.Sku.Contains(findSKU)).FirstOrDefault() != null);

            var query = (from model in q
                         select new
                         {
                            model.Id,
                            model.Code,
                            model.Description,
                            model.Discount,
                            model.Tax,
                            model.ShippingFee,
                            model.Allowance,
                            model.AmountPaid,
                            model.Total,
                            model.CoGS,
                            profitloss = (model.IsConfirmed ? (Nullable<decimal>)((model.Total * (100 - model.Tax) / 100) - model.CoGS) : null),
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CashBankId,
                            cashbank = model.CashBank.Name,
                            model.IsBank,
                            model.IsPaid,
                            model.IsFullPayment,
                            model.ContactName,
                            model.ContactPhone,
                            model.WarehouseId,
                            warehouse = model.Warehouse.Name,
                            model.SalesDate,
                            model.DueDate,
                            model.CreatedAt,
                            model.UpdatedAt,
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
                            model.Description,
                            model.Discount,
                            model.Tax,
                            model.ShippingFee,
                            model.Allowance,
                            model.AmountPaid,
                            model.Total,
                            model.CoGS,
                            model.profitloss,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CashBankId,
                            model.cashbank,
                            model.IsBank,
                            model.IsPaid,
                            model.IsFullPayment,
                            model.ContactName,
                            model.ContactPhone,
                            model.WarehouseId,
                            model.warehouse,
                            model.SalesDate,
                            model.DueDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetPaidList(string _search, long nd, int rows, int? page, string sidx, string sord, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _cashSalesInvoiceService.GetQueryable().Include("CashBank").Include("Warehouse").Where(x => x.IsPaid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.Description,
                             model.Discount,
                             model.Tax,
                             model.ShippingFee,
                             model.Allowance,
                             model.AmountPaid,
                             model.Total,
                             model.CoGS,
                             profitloss = (model.IsConfirmed ? (Nullable<decimal>)((model.Total * (100 - model.Tax) / 100) - model.CoGS) : null),
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.CashBankId,
                             cashbank = model.CashBank.Name,
                             model.IsBank,
                             model.IsPaid,
                             model.IsFullPayment,
                             model.ContactName,
                             model.ContactPhone,
                             model.WarehouseId,
                             warehouse = model.Warehouse.Name,
                             model.SalesDate,
                             model.DueDate,
                             model.CreatedAt,
                             model.UpdatedAt,
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
                            model.Description,
                            model.Discount,
                            model.Tax,
                            model.ShippingFee,
                            model.Allowance,
                            model.AmountPaid,
                            model.Total,
                            model.CoGS,
                            model.profitloss,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CashBankId,
                            model.cashbank,
                            model.IsBank,
                            model.IsPaid,
                            model.IsFullPayment,
                            model.ContactName,
                            model.ContactPhone,
                            model.WarehouseId,
                            model.warehouse,
                            model.SalesDate,
                            model.DueDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _cashSalesInvoiceDetailService.GetQueryableObjectsByCashSalesInvoiceId(id).Include("CashSalesInvoice").Include("Item");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.CashSalesInvoiceId,
                             cashsalesinvoice = model.CashSalesInvoice.Code,
                             model.ItemId,
                             itemSku = model.Item.Sku,
                             item = model.Item.Name,
                             model.Quantity,
                             model.Amount,
                             model.CoGS,
                             model.PriceMutationId,
                             model.Discount,
                             model.IsManualPriceAssignment,
                             model.AssignedPrice,
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
                             model.Code,
                             model.CashSalesInvoiceId,
                             model.cashsalesinvoice,
                             model.ItemId,
                             model.itemSku,
                             model.item,
                             model.Quantity,
                             model.Amount,
                             model.CoGS,
                             model.PriceMutationId,
                             model.Discount,
                             model.IsManualPriceAssignment,
                             model.AssignedPrice,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetPaidListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id, string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _cashSalesInvoiceDetailService.GetQueryableObjectsByCashSalesInvoiceId(id).Include("CashSalesInvoice").Include("Item").Where(x => x.CashSalesInvoice.IsPaid);

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.CashSalesInvoiceId,
                             cashsalesinvoice = model.CashSalesInvoice.Code,
                             model.ItemId,
                             item = model.Item.Name,
                             model.Quantity,
                             model.Amount,
                             model.CoGS,
                             model.PriceMutationId,
                             model.Discount,
                             model.IsManualPriceAssignment,
                             model.AssignedPrice,
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
                             model.Code,
                             model.CashSalesInvoiceId,
                             model.cashsalesinvoice,
                             model.ItemId,
                             model.item,
                             model.Quantity,
                             model.Amount,
                             model.CoGS,
                             model.PriceMutationId,
                             model.Discount,
                             model.IsManualPriceAssignment,
                             model.AssignedPrice,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CashSalesInvoice model = new CashSalesInvoice();
            try
            {
                model = _cashSalesInvoiceService.GetObjectById(Id);
          
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.Description,
                model.SalesDate,
                model.DueDate,
                model.Discount,
                model.Tax,
                model.Allowance,
                model.Total,
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById((int)model.CashBankId.GetValueOrDefault()).Name,
                model.WarehouseId,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            CashSalesInvoiceDetail model = new CashSalesInvoiceDetail();
            try
            {
                model = _cashSalesInvoiceDetailService.GetObjectById(Id);
            }
            catch (Exception ex)
            {
                LOG.Error("GetInfo", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Id,
                model.Code,
                model.CashSalesInvoiceId,
                CashSalesInvoice = _cashSalesInvoiceService.GetObjectById(model.CashSalesInvoiceId).Code,
                model.ItemId,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                model.Discount,
                model.IsManualPriceAssignment,
                model.AssignedPrice,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CashSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _cashSalesInvoiceService.CreateObject(model, _warehouseService, _cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
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
        public dynamic InsertDetail(CashSalesInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
               }
                if((model.IsManualPriceAssignment || model.Discount > 0) && (!AuthenticationModel.IsAllowed("ManualPricing", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction)))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Use Manual Pricing/Discount");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
               }
                model = _cashSalesInvoiceDetailService.CreateObject(model, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);
                total = _cashSalesInvoiceService.GetObjectById(model.CashSalesInvoiceId).Total;
            }
            catch (Exception ex)
            {
                LOG.Error("Insert Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);

            }

            return Json(new
            {
                model.Errors,
                Total = total
            });
        }

        [HttpPost]
        public dynamic Update(CashSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                data.Description = model.Description;
                data.SalesDate = model.SalesDate;
                data.DueDate = model.DueDate;
                data.Discount = model.Discount;
                data.Tax = model.Tax;
                data.Allowance = model.Allowance;
                data.CashBankId = model.CashBankId;
                data.WarehouseId = model.WarehouseId;
                model = _cashSalesInvoiceService.UpdateObject(data, _warehouseService, _cashBankService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
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
        public dynamic UpdateDetail(CashSalesInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }
                if ((model.IsManualPriceAssignment || model.Discount > 0) && (!AuthenticationModel.IsAllowed("ManualPricing", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction)))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Use Manual Pricing/Discount");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.Discount = model.Discount;
                data.IsManualPriceAssignment = model.IsManualPriceAssignment;
                data.AssignedPrice = model.AssignedPrice;
                data.CashSalesInvoiceId = model.CashSalesInvoiceId;
                model = _cashSalesInvoiceDetailService.UpdateObject(data, _cashSalesInvoiceService, _itemService, _warehouseItemService, _quantityPricingService);
                total = _cashSalesInvoiceService.GetObjectById(model.CashSalesInvoiceId).Total;
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors,
                Total = total
            });
        }

        [HttpPost]
        public dynamic Confirm(CashSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }
                if ((model.Discount > 0) && (!AuthenticationModel.IsAllowed("ManualPricing", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction)))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Use Manual Pricing/Discount/Allowance");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                data.ShippingFee = model.ShippingFee;
                model = _cashSalesInvoiceService.ConfirmObject(data, model.ConfirmationDate.Value, model.Discount, model.Tax, _cashSalesInvoiceDetailService, 
                                                    _contactService, _priceMutationService, _receivableService, _cashSalesInvoiceService, _warehouseItemService, 
                                                    _warehouseService, _itemService, _barringService, _stockMutationService, _cashBankService,
                                                    _generalLedgerJournalService, _accountService,_closingService);
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
        public dynamic UnConfirm(CashSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnConfirm", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnConfirm record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                model = _cashSalesInvoiceService.UnconfirmObject(data, _cashSalesInvoiceDetailService, _receivableService, _receiptVoucherDetailService,
                                                   _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService,
                                                   _generalLedgerJournalService,_accountService,_closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unconfirm Failed", ex);
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
        public dynamic Paid(CashSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Paid", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Paid record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }
                if ((model.Allowance > 0) && (!AuthenticationModel.IsAllowed("ManualPricing", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction)))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Use Manual Pricing/Discount/Allowance");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                data.Allowance = model.Allowance;
                data.ContactName = model.ContactName;
                data.ContactPhone = model.ContactPhone;
                model = _cashSalesInvoiceService.PaidObject(data, model.AmountPaid.Value, model.Allowance, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, 
                                                           _contactService, _cashMutationService, _cashSalesReturnService,
                                                           _generalLedgerJournalService,_accountService,_closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Paid Failed", ex);
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
        public dynamic UnPaid(CashSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnPaid", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnPaid record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                model = _cashSalesInvoiceService.UnpaidObject(data, _receiptVoucherService, _receiptVoucherDetailService, _cashBankService,
                                                   _receivableService, _cashMutationService, _cashSalesReturnService,_generalLedgerJournalService,
                                                   _accountService,_closingService);
            }
            catch (Exception ex)
            {
                LOG.Error("Unpaid Failed", ex);
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
        public dynamic Delete(CashSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceService.GetObjectById(model.Id);
                model = _cashSalesInvoiceService.SoftDeleteObject(data, _cashSalesInvoiceDetailService);
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
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
        public dynamic DeleteDetail(CashSalesInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _cashSalesInvoiceDetailService.GetObjectById(model.Id);
                model = _cashSalesInvoiceDetailService.SoftDeleteObject(data, _cashSalesInvoiceService);
                total = _cashSalesInvoiceService.GetObjectById(model.CashSalesInvoiceId).Total;
            }
            catch (Exception ex)
            {
                LOG.Error("Delete Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors,
                Total = total
            });
        }
    }
}