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
    public class RetailSalesInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("RetailSalesInvoiceController");
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
        private IRetailSalesInvoiceService _retailSalesInvoiceService;
        private IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService;
        private ICashSalesReturnService _cashSalesReturnService;
        private IQuantityPricingService _quantityPricingService;
        private IReceivableService _receivableService;
        private IReceiptVoucherService _receiptVoucherService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private IAccountService _accountService;
        private IGeneralLedgerJournalService _generalLedgerJournalService;
        
        public RetailSalesInvoiceController()
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
            _retailSalesInvoiceService = new RetailSalesInvoiceService(new RetailSalesInvoiceRepository(), new RetailSalesInvoiceValidator());
            _retailSalesInvoiceDetailService = new RetailSalesInvoiceDetailService(new RetailSalesInvoiceDetailRepository(), new RetailSalesInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(), new QuantityPricingValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _receiptVoucherService = new ReceiptVoucherService(new ReceiptVoucherRepository(), new ReceiptVoucherValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
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
            var q = _retailSalesInvoiceService.GetQueryable().Include("Contact").Include("CashBank");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.Description,
                             model.SalesDate,
                             model.DueDate,
                             model.Discount,
                             model.Tax,
                             model.IsGroupPricing,
                             model.ContactId,
                             contact = model.Contact.Name,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.AmountPaid,
                             model.IsGBCH,
                             model.GBCH_No,
                             model.GBCH_DueDate,
                             model.CashBankId,
                             cashbank = model.CashBank.Name,
                             model.IsBank,
                             model.IsPaid,
                             model.IsFullPayment,
                             model.Total,
                             model.CoGS,
                             profitloss = (model.IsConfirmed ? (Nullable<decimal>)((model.Total * (100 - model.Tax)/100) - model.CoGS) : null),
                             model.WarehouseId,
                             warehouse = model.Warehouse.Name,
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
                            model.SalesDate,
                            model.DueDate,
                            model.Discount,
                            model.Tax,
                            model.IsGroupPricing,
                            model.ContactId,
                            model.contact,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.AmountPaid,
                            model.IsGBCH,
                            model.GBCH_No,
                            model.GBCH_DueDate,
                            model.CashBankId,
                            model.cashbank,
                            model.IsBank,
                            model.IsPaid,
                            model.IsFullPayment,
                            model.Total,
                            model.CoGS,
                            model.profitloss,
                            model.WarehouseId,
                            model.warehouse,
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
            var q = _retailSalesInvoiceDetailService.GetQueryableObjectsByRetailSalesInvoiceId(id).Include("RetailSalesInvoice").Include("Item");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.RetailSalesInvoiceId,
                             retailsalesinvoice = model.RetailSalesInvoice.Code,
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
                            model.RetailSalesInvoiceId,
                            model.retailsalesinvoice,
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
            RetailSalesInvoice model = new RetailSalesInvoice();
            try
            {
                model = _retailSalesInvoiceService.GetObjectById(Id);
          
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
                model.Total,
                model.ContactId,
                model.IsGBCH,
                model.GBCH_No,
                model.GBCH_DueDate,
                Contact = _contactService.GetObjectById(model.ContactId).Name,
                model.CashBankId,
                CashBank = _cashBankService.GetObjectById((int)model.CashBankId.GetValueOrDefault()).Name,
                model.WarehouseId,
                Warehouse = _warehouseService.GetObjectById(model.WarehouseId).Name,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            RetailSalesInvoiceDetail model = new RetailSalesInvoiceDetail();
            try
            {
                model = _retailSalesInvoiceDetailService.GetObjectById(Id);
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
                model.RetailSalesInvoiceId,
                RetailSalesInvoice = _retailSalesInvoiceService.GetObjectById(model.RetailSalesInvoiceId).Code,
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
        public dynamic Insert(RetailSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _retailSalesInvoiceService.CreateObject(model, _warehouseService);
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
        public dynamic InsertDetail(RetailSalesInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _retailSalesInvoiceDetailService.CreateObject(model, _retailSalesInvoiceService, _itemService, _warehouseItemService, _priceMutationService);
                total = _retailSalesInvoiceService.GetObjectById(model.RetailSalesInvoiceId).Total;
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
        public dynamic Update(RetailSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceService.GetObjectById(model.Id);
                data.Description = model.Description;
                data.SalesDate = model.SalesDate;
                data.DueDate = model.DueDate;
                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.WarehouseId = model.WarehouseId;
                //data.Discount = model.Discount;
                model = _retailSalesInvoiceService.UpdateObject(data, _warehouseService);
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
        public dynamic UpdateDetail(RetailSalesInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.Discount = model.Discount;
                data.IsManualPriceAssignment = model.IsManualPriceAssignment;
                data.AssignedPrice = model.AssignedPrice;
                data.RetailSalesInvoiceId = model.RetailSalesInvoiceId;
                model = _retailSalesInvoiceDetailService.UpdateObject(data, _retailSalesInvoiceService, _itemService, _warehouseItemService, _priceMutationService);
                total = _retailSalesInvoiceService.GetObjectById(model.RetailSalesInvoiceId).Total;
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
        public dynamic Confirm(RetailSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceService.GetObjectById(model.Id);
                data.Discount = model.Discount;
                data.Tax = model.Tax;
                model = _retailSalesInvoiceService.ConfirmObject(data, model.ConfirmationDate.GetValueOrDefault(), data.ContactId, _retailSalesInvoiceDetailService, 
                                                    _contactService, _priceMutationService, _receivableService, _retailSalesInvoiceService, _warehouseItemService, 
                                                    _warehouseService, _itemService, _barringService, _stockMutationService);
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
        public dynamic UnConfirm(RetailSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnConfirm", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnConfirm record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceService.GetObjectById(model.Id);
                model = _retailSalesInvoiceService.UnconfirmObject(data, _retailSalesInvoiceDetailService, _receivableService, _receiptVoucherDetailService,
                                                   _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService);

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
        public dynamic Paid(RetailSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Paid", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Paid record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceService.GetObjectById(model.Id);
                data.IsGBCH = model.IsGBCH;
                data.GBCH_No = model.GBCH_No;
                data.GBCH_DueDate = model.GBCH_DueDate;
                model = _retailSalesInvoiceService.PaidObject(data, model.AmountPaid.Value, _cashBankService, _receivableService, _receiptVoucherService, _receiptVoucherDetailService, 
                                                    _contactService, _cashMutationService, _generalLedgerJournalService, _accountService);
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
        public dynamic UnPaid(RetailSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnPaid", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnPaid record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceService.GetObjectById(model.Id);
                model = _retailSalesInvoiceService.UnpaidObject(data, _receiptVoucherService, _receiptVoucherDetailService, _cashBankService,
                                                   _receivableService, _cashMutationService,_generalLedgerJournalService,_accountService);

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
        public dynamic Delete(RetailSalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceService.GetObjectById(model.Id);
                model = _retailSalesInvoiceService.SoftDeleteObject(data, _retailSalesInvoiceDetailService);
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
        public dynamic DeleteDetail(RetailSalesInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _retailSalesInvoiceDetailService.GetObjectById(model.Id);
                model = _retailSalesInvoiceDetailService.SoftDeleteObject(data, _retailSalesInvoiceService);
                total = _retailSalesInvoiceService.GetObjectById(model.RetailSalesInvoiceId).Total;
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