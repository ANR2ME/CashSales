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
    public class SalesInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalesInvoiceController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IPayableService _payableService;
        private IItemService _itemService;
        private ISalesInvoiceService _salesInvoiceService;
        private ISalesInvoiceDetailService _salesInvoiceDetailService;
        private IDeliveryOrderDetailService _deliveryOrderDetailService;
        private IReceiptVoucherDetailService _receiptVoucherDetailService;
        private IReceivableService _receivableService;
        private ISalesOrderService _salesOrderService;
        private ISalesOrderDetailService _salesOrderDetailService;
        private IDeliveryOrderService _deliveryOrderService;
        
        public SalesInvoiceController()
        {
            _purchaseOrderService = new PurchaseOrderService(new PurchaseOrderRepository(), new PurchaseOrderValidator());
            _purchaseOrderDetailService = new PurchaseOrderDetailService(new PurchaseOrderDetailRepository(), new PurchaseOrderDetailValidator());
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(), new PurchaseInvoiceValidator());
            _purchaseInvoiceDetailService = new PurchaseInvoiceDetailService(new PurchaseInvoiceDetailRepository(), new PurchaseInvoiceDetailValidator());
            _purchaseReceivalService = new PurchaseReceivalService(new PurchaseReceivalRepository(), new PurchaseReceivalValidator());
            _purchaseReceivalDetailService = new PurchaseReceivalDetailService(new PurchaseReceivalDetailRepository(), new PurchaseReceivalDetailValidator());
            _paymentVoucherDetailService = new PaymentVoucherDetailService(new PaymentVoucherDetailRepository(), new PaymentVoucherDetailValidator());
            _payableService = new PayableService(new PayableRepository(), new PayableValidator());
            _itemService = new ItemService(new ItemRepository(), new ItemValidator());
            _salesInvoiceService = new SalesInvoiceService(new SalesInvoiceRepository(), new SalesInvoiceValidator());
            _salesInvoiceDetailService = new SalesInvoiceDetailService(new SalesInvoiceDetailRepository(), new SalesInvoiceDetailValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _receiptVoucherDetailService = new ReceiptVoucherDetailService(new ReceiptVoucherDetailRepository(), new ReceiptVoucherDetailValidator());
            _receivableService = new ReceivableService(new ReceivableRepository(), new ReceivableValidator());
            _salesOrderService = new SalesOrderService(new SalesOrderRepository(), new SalesOrderValidator());
            _deliveryOrderDetailService = new DeliveryOrderDetailService(new DeliveryOrderDetailRepository(), new DeliveryOrderDetailValidator());
            _salesOrderDetailService = new SalesOrderDetailService(new SalesOrderDetailRepository(), new SalesOrderDetailValidator());
            _deliveryOrderService = new DeliveryOrderService(new DeliveryOrderRepository(), new DeliveryOrderValidator());
        
        
        }



        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
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
            var q = _salesInvoiceService.GetQueryable().Include("DeliveryOrder");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.DeliveryOrderId,
                             deliveryorder = model.DeliveryOrder.Code,
                             model.Description,
                             model.Discount,
                             model.IsTaxable,
                             model.InvoiceDate,
                             model.DueDate,
                             model.AmountReceivable,
                             model.IsConfirmed,
                             model.ConfirmationDate,
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
                            model.DeliveryOrderId,
                            model.deliveryorder,
                            model.Description,
                            model.Discount,
                            model.IsTaxable,
                            model.InvoiceDate,
                            model.DueDate,
                            model.AmountReceivable,
                            model.IsConfirmed,
                            model.ConfirmationDate,
                            model.CreatedAt,
                            model.UpdatedAt,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetListDetail(string _search, long nd, int rows, int? page, string sidx, string sord, int id,string filters = "")
        {
            // Construct where statement
            string strWhere = GeneralFunction.ConstructWhere(filters);
            string filter = null;
            GeneralFunction.ConstructWhereInLinq(strWhere, out filter);
            if (filter == "") filter = "true";

            // Get Data
            var q = _salesInvoiceDetailService.GetQueryableObjectsBySalesInvoiceId(id).Include("DeliveryOrderDetail").Include("Item");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.DeliveryOrderDetailId,
                             deliveryorderdetail = model.DeliveryOrderDetail.Code,
                             itemid = model.DeliveryOrderDetail.ItemId,
                             item = model.DeliveryOrderDetail.Item.Name,
                             model.Quantity,
                             model.Amount,
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
                            model.DeliveryOrderDetailId,
                            model.deliveryorderdetail,
                            model.itemid,
                            model.item,
                            model.Quantity,
                            model.Amount,
                      }
                    }).ToArray()
            }, JsonRequestBehavior.AllowGet);
        }

      
        public dynamic GetInfo(int Id)
        {
            SalesInvoice model = new SalesInvoice();
            try
            {
                model = _salesInvoiceService.GetObjectById(Id);
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
                model.DeliveryOrderId,
                DeliveryOrder = _deliveryOrderService.GetObjectById(model.DeliveryOrderId).Code,
                model.AmountReceivable,
                model.Discount,
                model.Description,
                model.IsTaxable,
                model.InvoiceDate,
                model.DueDate,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            SalesInvoiceDetail model = new SalesInvoiceDetail();
            try
            {
                model = _salesInvoiceDetailService.GetObjectById(Id);
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
                model.DeliveryOrderDetailId,
                DeliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(model.DeliveryOrderDetailId).Code,
                model.Quantity,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(SalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _salesInvoiceService.CreateObject(model,_deliveryOrderService);
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
        public dynamic InsertDetail(SalesInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _salesInvoiceDetailService.CreateObject(model,_salesInvoiceService,_salesOrderDetailService,_deliveryOrderDetailService);
                amount = _salesInvoiceService.GetObjectById(model.SalesInvoiceId).AmountReceivable;
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
                AmountPayable = amount
            });
        }

        [HttpPost]
        public dynamic Update(SalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salesInvoiceService.GetObjectById(model.Id);
                data.DeliveryOrderId = model.DeliveryOrderId;
                data.Description = model.Description;
                data.Discount = model.Discount;
                data.IsTaxable = model.IsTaxable;
                data.InvoiceDate = model.InvoiceDate;
                data.DueDate = model.DueDate;
                model = _salesInvoiceService.UpdateObject(data,_deliveryOrderService,_salesInvoiceDetailService);
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
                model.AmountReceivable
            });
        }

        [HttpPost]
        public dynamic Delete(SalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salesInvoiceService.GetObjectById(model.Id);
                model = _salesInvoiceService.SoftDeleteObject(data,_salesInvoiceDetailService);
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
        public dynamic DeleteDetail(SalesInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salesInvoiceDetailService.GetObjectById(model.Id);
                model = _salesInvoiceDetailService.SoftDeleteObject(data,_salesInvoiceService);
                amount = _salesInvoiceService.GetObjectById(model.SalesInvoiceId).AmountReceivable;
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
                AmountPayable = amount
            });
        }

        [HttpPost]
        public dynamic UpdateDetail(SalesInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salesInvoiceDetailService.GetObjectById(model.Id);
                data.Quantity = model.Quantity;
                data.DeliveryOrderDetailId = model.DeliveryOrderDetailId;
                model = _salesInvoiceDetailService.UpdateObject(data,_salesInvoiceService,_salesOrderDetailService,
                    _deliveryOrderDetailService);
                amount = _salesInvoiceService.GetObjectById(model.SalesInvoiceId).AmountReceivable;
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
                AmountPayable = amount
            });
        }


        [HttpPost]
        public dynamic Confirm(SalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salesInvoiceService.GetObjectById(model.Id);
                model = _salesInvoiceService.ConfirmObject(data,model.ConfirmationDate.Value,_salesInvoiceDetailService,
                    _salesOrderService,_deliveryOrderService,_deliveryOrderDetailService,_receivableService);
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
        public dynamic UnConfirm(SalesInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnConfirm", Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnConfirm record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _salesInvoiceService.GetObjectById(model.Id);
                model = _salesInvoiceService.UnconfirmObject(data, _salesInvoiceDetailService,_deliveryOrderService,
                    _deliveryOrderDetailService, _receiptVoucherDetailService, _receivableService);

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


    }
}
