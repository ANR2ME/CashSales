using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;


namespace Service.Service
{
    public class UserAccountService : IUserAccountService
    {
        private IUserAccountRepository _repository;
        private IUserAccountValidator _validator;

        public UserAccountService(IUserAccountRepository _userAccountRepository, IUserAccountValidator _userAccountValidator)
        {
            _repository = _userAccountRepository;
            _validator = _userAccountValidator;
        }

        public IUserAccountValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<UserAccount> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<UserAccount> GetAll()
        {
            return _repository.GetAll();
        }

        public UserAccount GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public UserAccount GetObjectByIsAdmin(bool IsAdmin)
        {
            return _repository.GetObjectByIsAdmin(IsAdmin);
        }

        public UserAccount GetObjectByUsername(string username)
        {
            return _repository.GetObjectByUsername(username);
        }

        public UserAccount IsLoginValid(string username, string password)
        {
            StringEncryption se = new StringEncryption();
            return _repository.IsLoginValid(username, se.Encrypt(password));
        }

        public UserAccount CreateObject(UserAccount userAccount)
        {
            userAccount.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(userAccount, this))
            {
                userAccount.Username = userAccount.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = userAccount.Password;
                userAccount.Password = se.Encrypt(userAccount.Password);
                _repository.CreateObject(userAccount);
                //userAccount.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return userAccount;
        }

        public UserAccount CreateObject(string username, string password, string name, string description, bool IsAdmin)
        {
            UserAccount userAccount = new UserAccount()
            {
                Username = username,
                Password = password,
                Name = name,
                Description = description,
                IsAdmin = IsAdmin
            };
            return CreateObject(userAccount);
        }

        public UserAccount UpdateObjectPassword(UserAccount userAccount, string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (_validator.ValidUpdateObjectPassword(userAccount, OldPassword, NewPassword, ConfirmPassword, this))
            {
                userAccount.Username = userAccount.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = userAccount.Password;
                userAccount.Password = se.Encrypt(NewPassword);
                _repository.UpdateObject(userAccount);
                //userAccount.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return userAccount;
        }

        public UserAccount UpdateObject(UserAccount userAccount)
        {
            if(_validator.ValidUpdateObject(userAccount, this))
            {
                userAccount.Username = userAccount.Username.Trim();
                StringEncryption se = new StringEncryption();
                //string realpassword = userAccount.Password;
                userAccount.Password = se.Encrypt(userAccount.Password);
                _repository.UpdateObject(userAccount);
                //userAccount.Password = realpassword; // set back to unencrypted password to prevent encrypting an already encrypted password on the next update
            }
            return userAccount;
        }

        public UserAccount SoftDeleteObject(UserAccount userAccount, int LoggedId)
        {
            return (_validator.ValidDeleteObject(userAccount, LoggedId) ? _repository.SoftDeleteObject(userAccount) : userAccount);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public UserAccount FindOrCreateSysAdmin()
        {
            UserAccount userAccount = GetObjectByIsAdmin(true);
            if (userAccount == null)
            {
                userAccount = CreateObject(Core.Constants.Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true);
            }
            return userAccount;
        }

        

        /*public static bool IsAuthenticated()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return true;

            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return true;
                }
            }
            return false;
        }//end function IsAuthenticated

        public static int GetUserId()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return ConfigurationModels.GET_MODE_TESTING_USERID();

            int userId = 0;
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 
                        string strUserId = ticket.Name;
                        userId = int.Parse(strUserId);
                    }
                }
            }
            return userId;
        }//end function GetUserCode

        public static int GetUserTypeId()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return ConfigurationModels.GET_MODE_TESTING_USERTYPEID();

            int userTypeId = 0;
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 
                        string[] userDetail = ticket.UserData.Split('#');
                        if (!String.IsNullOrEmpty(userDetail[1]))
                        {
                            string strUserTypeId = userDetail[1];
                            userTypeId = int.Parse(strUserTypeId);
                        }
                    }
                }
            }
            return userTypeId;
        }//end function GetUserTypeId

        public static string GetUserName()
        {
            // IF IN MODE TESTING
            if (ConfigurationModels.MODE_TESTING())
                return ConfigurationModels.GET_MODE_TESTING_USERNAME();

            string userName = "";
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get Forms Identity From Current User
                        FormsIdentity id = (FormsIdentity)
                        HttpContext.Current.User.Identity;

                        // Get Forms Ticket From Identity object
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Retrieve stored user-data (role information is assigned 
                        // when the ticket is created, separate multiple roles 
                        // with commas) 
                        string[] userDetail = ticket.UserData.Split('#');
                        if (!String.IsNullOrEmpty(userDetail[2]))
                            userName = userDetail[2];
                    }
                }
            }
            return userName;
        }//end function GetUserName
        */

    }
}
