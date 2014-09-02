using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    public class UserAccessController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("UserAccessController");
        private IUserAccountService _userAccountService;
        private IUserAccessService _userAccessService;

        public UserAccessController()
        {
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
        }

        public dynamic GetUserAccess(int userId,string groupname)
        {
           List<UserAccess> model = new List<UserAccess>();
            try
            {

                model = _userAccessService.GetAll().Where(x => x.UserAccountId == userId).ToList();

            }
            catch (Exception ex)
            {
                LOG.Error("GetUserAccessAccountingList", ex);
            }

            return Json(new
            {
                model
            }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Index()
        {
            return View();
        }

    }
}
