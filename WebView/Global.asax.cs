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
            _userMenuService.CreateObject("Contact", "Master");
            _userMenuService.CreateObject("ItemType", "Master");
            _userMenuService.CreateObject("UoM", "Master");
            _userMenuService.CreateObject("Quantity Pricing", "Master");

            _userMenuService.CreateObject("CashBank", "Master");
            _userMenuService.CreateObject("CashBank Adjustment", "Master");
            _userMenuService.CreateObject("CashBank Mutation", "Master");
            _userMenuService.CreateObject("Cash Mutation", "Master");
            _userMenuService.CreateObject("Payment Request", "Master");

            _userMenuService.CreateObject("Item", "Master");
            _userMenuService.CreateObject("Stock Adjustment", "Master");
            _userMenuService.CreateObject("Stock Mutation", "Master");
            _userMenuService.CreateObject("Warehouse", "Master");
            _userMenuService.CreateObject("WarehouseItem", "Master");
            _userMenuService.CreateObject("Warehouse Mutation", "Master");

            _userMenuService.CreateObject("Purchase Order", "Transaction");
            _userMenuService.CreateObject("Purchase Receival", "Transaction");
            _userMenuService.CreateObject("Purchase Invoice", "Transaction");
            _userMenuService.CreateObject("Custom Purchase Invoice", "Transaction");
            _userMenuService.CreateObject("Payment Voucher", "Transaction");
            _userMenuService.CreateObject("Payable", "Transaction");

            _userMenuService.CreateObject("Sales Order", "Transaction");
            _userMenuService.CreateObject("Delivery Order", "Transaction");
            _userMenuService.CreateObject("Sales Invoice", "Transaction");
            _userMenuService.CreateObject("Cash Sales Invoice", "Transaction");
            _userMenuService.CreateObject("Cash Sales Return", "Transaction");
            _userMenuService.CreateObject("Retail Sales Invoice", "Transaction");
            _userMenuService.CreateObject("Receipt Voucher", "Transaction");
            _userMenuService.CreateObject("Receivable", "Transaction");

            _userMenuService.CreateObject("User", "Setting");
            _userMenuService.CreateObject("User Access Right", "Setting");
            _userMenuService.CreateObject("Change Password", "Setting");
        }

        public void CreateSysAdmin()
        {
            UserAccount userAccount = _userAccountService.CreateObject("Admin", "sysadmin", "Administrator", "Administrator", true);

            var userMenus = _userMenuService.GetAll();
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
            }
        }

    }
}