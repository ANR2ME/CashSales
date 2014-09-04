using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service.Service;
using Validation.Validation;

namespace WebView
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private IContactGroupService _contactGroupService;
        private IContactService _contactService;
        private IUserMenuService _userMenuService;
        private IUserAccountService _userAccountService;
        private IUserAccessService _userAccessService;
        private ContactGroup baseContactGroup;
        private Contact baseContact;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            PopulateData();
        }

        public void PopulateData()
        {
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());

            baseContactGroup = _contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);
            baseContact = _contactService.CreateObject(Core.Constants.Constant.BaseContact, "BaseAddr", "123456", "PIC", "123", "Base@email.com", _contactGroupService);

            CreateUserMenus();
            CreateSysAdmin();
        }

        public void CreateUserMenus()
        {
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.Contact, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.ItemType, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.UoM, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.QuantityPricing, Core.Constants.Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.CashBank, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.CashBankAdjustment, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.CashBankMutation, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.CashMutation, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.PaymentRequest, Core.Constants.Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.Item, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.StockAdjustment, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.StockMutation, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.Warehouse, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.WarehouseItem, Core.Constants.Constant.MenuGroupName.Master);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.WarehouseMutation, Core.Constants.Constant.MenuGroupName.Master);

            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.PurchaseOrder, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.PurchaseReceival, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.PurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.CustomPurchaseInvoice, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.PaymentVoucher, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.Payable, Core.Constants.Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.SalesOrder, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.DeliveryOrder, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.SalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.CashSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.CashSalesReturn, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.RetailSalesInvoice, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.ReceiptVoucher, Core.Constants.Constant.MenuGroupName.Transaction);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.Receivable, Core.Constants.Constant.MenuGroupName.Transaction);

            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.User, Core.Constants.Constant.MenuGroupName.Setting);
            _userMenuService.CreateObject(Core.Constants.Constant.MenuName.UserAccessRight, Core.Constants.Constant.MenuGroupName.Setting);
        }

        public void CreateSysAdmin()
        {
            UserAccount userAccount = _userAccountService.GetObjectByUsername(Core.Constants.Constant.UserType.Admin);
            if (userAccount == null)
            {
                userAccount = _userAccountService.CreateObject(Core.Constants.Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true);
            }
            _userAccessService.CreateDefaultAccess(userAccount.Id, _userMenuService, _userAccountService);

            /*var userMenus = _userMenuService.GetAll();
            foreach (var userMenu in userMenus)
            {
                UserAccess userAccess = new UserAccess() 
                {
                    UserAccountId = userAccount.Id,
                    UserMenuId = userMenu.Id,
                    AllowConfirm = true,
                    AllowCreate = true,
                    AllowDelete = true,
                    AllowEdit = true,
                    AllowPaid = true,
                    AllowPrint = true,
                    AllowReconcile = true,
                    AllowUnconfirm = true,
                    AllowUndelete = true,
                    AllowUnpaid = true,
                    AllowUnreconcile = true,
                    AllowView = true,
                };
                _userAccessService.CreateObject(userAccess, _userAccountService, _userMenuService);
            }*/
        }

    }
}