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
    public class PurchaseInvoiceController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("PurchaseInvoiceController");
        private IPurchaseOrderService _purchaseOrderService;
        private IPurchaseOrderDetailService _purchaseOrderDetailService;
        private IPurchaseInvoiceService _purchaseInvoiceService;
        private IPurchaseInvoiceDetailService _purchaseInvoiceDetailService;
        private IPurchaseReceivalService _purchaseReceivalService;
        private IPurchaseReceivalDetailService _purchaseReceivalDetailService;
        private IPaymentVoucherDetailService _paymentVoucherDetailService;
        private IPayableService _payableService;
        private IItemService _itemService;

        public PurchaseInvoiceController()
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
        }


        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
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
            var q = _purchaseInvoiceService.GetQueryable().Include("PurchaseReceival");

            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.PurchaseReceivalId,
                             purchasereceival = model.PurchaseReceival.Code,
                             model.Description,
                             model.Discount,
                             model.IsTaxable,
                             model.InvoiceDate,
                             model.DueDate,
                             model.AmountPayable,
                             model.IsConfirmed,
                             model.ConfirmationDate,
                             model.CreatedAt,
                             model.UpdatedAt,
                         }).Where(filter).OrderBy(sidx + " " + sord); //.ToList();

            var list = query.AsEnumerable();

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = list.Count();
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
                            model.PurchaseReceivalId,
                            model.purchasereceival,
                            model.Description,
                            model.Discount,
                            model.IsTaxable,
                            model.InvoiceDate,
                            model.DueDate,
                            model.AmountPayable,
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
            var q = _purchaseInvoiceDetailService.GetQueryableObjectsByPurchaseInvoiceId(id).Include("PurchaseReceivalDetail").Include("Item");
            
            var query = (from model in q
                         select new
                         {
                             model.Id,
                             model.Code,
                             model.PurchaseReceivalDetailId,
                             purchasereceivaldetail = model.PurchaseReceivalDetail.Code,
                             itemid = model.PurchaseReceivalDetail.ItemId,
                             item = model.PurchaseReceivalDetail.Item.Name, 
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
                            model.PurchaseReceivalDetailId,
                            model.purchasereceivaldetail,
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
            PurchaseInvoice model = new PurchaseInvoice();
            try
            {
                model = _purchaseInvoiceService.GetObjectById(Id);
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
                model.PurchaseReceivalId,
                PurchaseReceival = _purchaseReceivalService.GetObjectById(model.PurchaseReceivalId).Code,
                model.Discount,
                model.Description,
                model.IsTaxable,
                model.InvoiceDate,
                model.DueDate,
                model.AmountPayable,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        public dynamic GetInfoDetail(int Id)
        {
            PurchaseInvoiceDetail model = new PurchaseInvoiceDetail();
            try
            {
                model = _purchaseInvoiceDetailService.GetObjectById(Id);
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
                model.PurchaseReceivalDetailId,
                PurchaseReceival = _purchaseReceivalDetailService.GetObjectById(model.PurchaseReceivalDetailId).Code,
                ItemId = _purchaseReceivalDetailService.GetObjectById(model.PurchaseReceivalDetailId).ItemId,
                Item = _itemService.GetObjectById(_purchaseReceivalDetailService.GetObjectById(model.PurchaseReceivalDetailId).ItemId).Name,
                model.Quantity,
                model.Amount,
                model.Errors
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public dynamic Insert(PurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Create", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Add record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _purchaseInvoiceService.CreateObject(model,_purchaseReceivalService);
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
        public dynamic InsertDetail(PurchaseInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                model = _purchaseInvoiceDetailService.CreateObject(model,_purchaseInvoiceService,_purchaseOrderDetailService,_purchaseReceivalDetailService);
                amount = _purchaseInvoiceService.GetObjectById(model.PurchaseInvoiceId).AmountPayable;
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
        public dynamic Update(PurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                data.PurchaseReceivalId = model.PurchaseReceivalId;
                data.Description = model.Description;
                data.Discount = model.Discount;
                data.IsTaxable = model.IsTaxable;
                data.InvoiceDate = model.InvoiceDate;
                data.DueDate = model.DueDate;
                model = _purchaseInvoiceService.UpdateObject(data,_purchaseReceivalService,_purchaseInvoiceDetailService);
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
                model.AmountPayable
            });
        }

        [HttpPost]
        public dynamic Delete(PurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Delete", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Delete Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                model = _purchaseInvoiceService.SoftDeleteObject(data,_purchaseInvoiceDetailService);
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
        public dynamic DeleteDetail(PurchaseInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _purchaseInvoiceDetailService.GetObjectById(model.Id);
                model = _purchaseInvoiceDetailService.SoftDeleteObject(data,_purchaseInvoiceService);
                amount = _purchaseInvoiceService.GetObjectById(model.PurchaseInvoiceId).AmountPayable;
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
        public dynamic UpdateDetail(PurchaseInvoiceDetail model)
        {
            decimal amount = 0;
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _purchaseInvoiceDetailService.GetObjectById(model.Id);
                data.PurchaseReceivalDetailId = model.PurchaseReceivalDetailId;
                data.Quantity = model.Quantity;
                model = _purchaseInvoiceDetailService.UpdateObject(data,_purchaseInvoiceService,_purchaseOrderDetailService,_purchaseReceivalDetailService);
                amount = _purchaseInvoiceService.GetObjectById(model.PurchaseInvoiceId).AmountPayable;
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
        public dynamic Confirm(PurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Confirm", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Confirm Record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                model = _purchaseInvoiceService.ConfirmObject(data, model.ConfirmationDate.Value, _purchaseInvoiceDetailService, _purchaseOrderService, _purchaseReceivalService, _purchaseReceivalDetailService, _payableService);
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
        public dynamic UnConfirm(PurchaseInvoice model)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("UnConfirm", Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to UnConfirm record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _purchaseInvoiceService.GetObjectById(model.Id);
                model = _purchaseInvoiceService.UnconfirmObject(data,_purchaseInvoiceDetailService,_purchaseReceivalService,_purchaseReceivalDetailService,_paymentVoucherDetailService,_payableService);
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

