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
    public class CustomPurchaseInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("CustomPurchaseInvoiceController");
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
        private ICustomPurchaseInvoiceService _customPurchaseInvoiceService;
        private ICustomPurchaseInvoiceDetailService _customPurchaseInvoiceDetailService;
        private ICashSalesReturnService _cashSalesReturnService;
        private IQuantityPricingService _quantityPricingService;
        private IPayableService _payableService;
        private IPaymentVoucherService _paymentVoucherService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IStockAdjustmentDetailService _stockAdjustmentDetailService;
        private IReceivableService _receivableService;
        public CustomPurchaseInvoiceController()
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
            _customPurchaseInvoiceService = new CustomPurchaseInvoiceService(new CustomPurchaseInvoiceRepository(), new CustomPurchaseInvoiceValidator());
            _customPurchaseInvoiceDetailService = new CustomPurchaseInvoiceDetailService(new CustomPurchaseInvoiceDetailRepository(), new CustomPurchaseInvoiceDetailValidator());
            _cashSalesReturnService = new CashSalesReturnService(new CashSalesReturnRepository(), new CashSalesReturnValidator());
            _quantityPricingService = new QuantityPricingService(new QuantityPricingRepository(), new QuantityPricingValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _paymentVoucherService = new PaymentVoucherService(new PaymentVoucherRepository(), new PaymentVoucherValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
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
            var q = _customPurchaseInvoiceService.GetQueryable().Include("Contact").Include("CashBank").Include("Warehouse");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.Description,
                             model.Discount,
                             model.Tax,
                             model.Allowance,
                             model.Total,
                             model.CoGS,
                             model.AmountPaid,
                             model.IsGroupPricing,
                             model.ContactId,
                             contact = model.Contact.Name,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.IsGBCH,
                             model.GBCH_No,
                             model.GBCH_DueDate,
                             model.CashBankId,
                             cashbank = model.CashBank.Name,
                             model.IsBank,
                             model.IsPaid,
                             model.IsFullPayment,
                             model.WarehouseId,
                             warehouse = model.Warehouse.Name,
                             model.PurchaseDate,
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
                            model.Allowance,
                            model.Total,
                            model.CoGS,
                            model.AmountPaid,
                            model.IsGroupPricing,
                            model.ContactId,
                            model.contact,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.IsGBCH,
                            model.GBCH_No,
                            model.GBCH_DueDate,
                            model.CashBankId,
                            model.cashbank,
                            model.IsBank,
                            model.IsPaid,
                            model.IsFullPayment,
                            model.WarehouseId,
                            model.warehouse,
                            model.PurchaseDate,
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
            var q = _customPurchaseInvoiceDetailService.GetQueryableObjectsByCustomPurchaseInvoiceId(id).Include("CustomPurchaseInvoice").Include("Item");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.CustomPurchaseInvoiceId,
                             custompurchaseinvoice = model.CustomPurchaseInvoice.Code,
                             model.ItemId,
                             item = model.Item.Name,
                             model.Quantity,
                             model.Discount,
                             model.ListedUnitPrice,
                             model.Amount,
                             model.CoGS,
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
                            model.CustomPurchaseInvoiceId,
                            model.custompurchaseinvoice,
                            model.ItemId,
                            model.item,
                            model.Quantity,
                            model.Discount,
                            model.ListedUnitPrice,
                            model.Amount,
                            model.CoGS,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfo(int Id)
        {
            CustomPurchaseInvoice model = new CustomPurchaseInvoice();
            try
            {
                model = _customPurchaseInvoiceService.GetObjectById(Id);
          
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
                model.PurchaseDate,
                model.DueDate,
                model.Discount,
                model.Tax,
                model.Allowance,
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
            CustomPurchaseInvoiceDetail model = new CustomPurchaseInvoiceDetail();
            try
            {
                model = _customPurchaseInvoiceDetailService.GetObjectById(Id);
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
                model.CustomPurchaseInvoiceId,
                CustomPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(model.CustomPurchaseInvoiceId).Code,
                model.ItemId,
                Item = _itemService.GetObjectById(model.ItemId).Name,
                model.Quantity,
                model.Discount,
                model.ListedUnitPrice,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(CustomPurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _customPurchaseInvoiceService.CreateObject(model, _warehouseService, _contactService);
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
        public dynamic InsertDetail(CustomPurchaseInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _customPurchaseInvoiceDetailService.CreateObject(model, _customPurchaseInvoiceService, _itemService, _warehouseItemService);
                total = _customPurchaseInvoiceService.GetObjectById(model.CustomPurchaseInvoiceId).Total;
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
        public dynamic Update(CustomPurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceService.GetObjectById(model.Id);
                data.Description = model.Description;
                data.PurchaseDate = model.PurchaseDate;
                data.DueDate = model.DueDate;
                data.ContactId = model.ContactId;
                data.CashBankId = model.CashBankId;
                data.WarehouseId = model.WarehouseId;
                model = _customPurchaseInvoiceService.UpdateObject(data, _customPurchaseInvoiceDetailService, _warehouseService, _contactService);
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
        public dynamic UpdateDetail(CustomPurchaseInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceDetailService.GetObjectById(model.Id);
                data.ItemId = model.ItemId;
                data.Quantity = model.Quantity;
                data.Discount = model.Discount;
                data.ListedUnitPrice = model.ListedUnitPrice;
                data.CustomPurchaseInvoiceId = model.CustomPurchaseInvoiceId;
                model = _customPurchaseInvoiceDetailService.UpdateObject(data, _customPurchaseInvoiceService, _itemService, _warehouseItemService);
                total = _customPurchaseInvoiceService.GetObjectById(model.CustomPurchaseInvoiceId).Total;
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
        public dynamic Check(CustomPurchaseInvoice model, decimal DailySalesProjection, bool IncludeSaturdaySales, bool IncludeSundaySales)
        {
            Dictionary<string, string> Errors = new Dictionary<string, string>();
            try
            {
                DateTime startDate = DateTime.Today;
                DateTime endDate = model.DueDate.GetValueOrDefault();

                var data = _customPurchaseInvoiceService.GetObjectById(model.Id);
                decimal total = _customPurchaseInvoiceService.CalculateTotalAmountAfterDiscountAndTax(data) - model.Allowance;
                decimal totalcashbank = _cashBankService.GetTotalCashBank();

                DateTime curDate = startDate;
                decimal funds = totalcashbank;
                while (curDate <= endDate)
                {
                    if ((curDate.DayOfWeek != DayOfWeek.Saturday && curDate.DayOfWeek != DayOfWeek.Sunday) ||
                        (curDate.DayOfWeek == DayOfWeek.Saturday && IncludeSaturdaySales) ||
                        (curDate.DayOfWeek == DayOfWeek.Sunday && IncludeSundaySales))
                    {
                        funds += DailySalesProjection;
                    }
                    funds += _receivableService.GetTotalRemainingAmountByDueDate(curDate, curDate);
                    funds -= _payableService.GetTotalRemainingAmountByDueDate(curDate, curDate);

                    curDate = curDate.AddDays(1);
                }

                if (funds < total)
                {
                    Errors.Add("Generic", "Dana Tidak tersedia");
                }
            }
            catch (Exception ex)
            {
                LOG.Error("Check Funds Failed", ex);
                Errors.Add("Generic", "Error " + ex);
            }

            return Json(new
            {
                Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Confirm(CustomPurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceService.GetObjectById(model.Id);
                data.Discount = model.Discount;
                data.Tax = model.Tax;
                model = _customPurchaseInvoiceService.ConfirmObject(data, model.ConfirmationDate.Value, _customPurchaseInvoiceDetailService, 
                                                    _contactService, _priceMutationService, _payableService, _customPurchaseInvoiceService, _warehouseItemService, 
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
        public dynamic UnConfirm(CustomPurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnConfirm", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnConfirm record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceService.GetObjectById(model.Id);
                model = _customPurchaseInvoiceService.UnconfirmObject(data, _customPurchaseInvoiceDetailService, _payableService, _paymentVoucherDetailService,
                                                   _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService, _priceMutationService);

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
        public dynamic Paid(CustomPurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Paid", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Paid record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceService.GetObjectById(model.Id);
                data.Allowance = model.Allowance;
                data.IsGBCH = model.IsGBCH;
                data.GBCH_No = model.GBCH_No;
                data.GBCH_DueDate = model.GBCH_DueDate;
                model = _customPurchaseInvoiceService.PaidObject(data, model.AmountPaid.Value, _cashBankService, _payableService, _paymentVoucherService, _paymentVoucherDetailService, 
                                                    _contactService, _cashMutationService);
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
        public dynamic UnPaid(CustomPurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnPaid", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnPaid record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceService.GetObjectById(model.Id);
                model = _customPurchaseInvoiceService.UnpaidObject(data, _paymentVoucherService, _paymentVoucherDetailService, _cashBankService,
                                                   _payableService, _cashMutationService);

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
        public dynamic Delete(CustomPurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceService.GetObjectById(model.Id);
                model = _customPurchaseInvoiceService.SoftDeleteObject(data, _customPurchaseInvoiceDetailService);
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
        public dynamic DeleteDetail(CustomPurchaseInvoiceDetail model)
        {
            decimal total = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _customPurchaseInvoiceDetailService.GetObjectById(model.Id);
                model = _customPurchaseInvoiceDetailService.SoftDeleteObject(data, _customPurchaseInvoiceService);
                total = _customPurchaseInvoiceService.GetObjectById(model.CustomPurchaseInvoiceId).Total;
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