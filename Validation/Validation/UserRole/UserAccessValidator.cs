using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class UserAccessValidator : IUserAccessValidator
    {

        public UserAccess VIsValidUserAccount(UserAccess userAccess, IUserAccountService _userAccountService)
        {
            UserAccount userAccount = _userAccountService.GetObjectById(userAccess.UserAccountId);
            if (userAccount == null)
            {
                userAccess.Errors.Add("Generic", "UserAccount Tidak valid");
            }
            return userAccess;
        }

        public UserAccess VIsValidUserMenu(UserAccess userAccess, IUserMenuService _userMenuService)
        {
            UserMenu userMenu = _userMenuService.GetObjectById(userAccess.UserMenuId);
            if (userMenu == null)
            {
                userAccess.Errors.Add("Generic", "UserMenu Tidak valid");
            }
            return userAccess;
        }

        public UserAccess VHasUniqueUserMenuAndUserAccountCombination(UserAccess userAccess, IUserAccessService _userAccessService)
        {
            UserAccess userAccess2 = _userAccessService.GetObjectByUserAccountIdAndUserMenuId(userAccess.UserAccountId, userAccess.UserMenuId);
            if (userAccess2 != null)
            {
                userAccess.Errors.Add("Generic", "Kombinasi UserAccount dan UserMenu sudah ada");
            }
            return userAccess;
        }

        public UserAccess VCreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService)
        {
            VIsValidUserAccount(userAccess, _userAccountService);
            if (!isValid(userAccess)) { return userAccess; }
            VIsValidUserMenu(userAccess, _userMenuService);
            if (!isValid(userAccess)) { return userAccess; }
            VHasUniqueUserMenuAndUserAccountCombination(userAccess, _userAccessService);
            return userAccess;
        }

        public UserAccess VUpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService)
        {
            return VCreateObject(userAccess, _userAccountService, _userMenuService, _userAccessService);
        }

        public UserAccess VDeleteObject(UserAccess userAccess)
        {
            return userAccess;
        }

        public bool ValidCreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService)
        {
            VCreateObject(userAccess, _userAccountService, _userMenuService, _userAccessService);
            return isValid(userAccess);
        }

        public bool ValidUpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService)
        {
            VUpdateObject(userAccess, _userAccountService, _userMenuService, _userAccessService);
            return isValid(userAccess);
        }

        public bool ValidDeleteObject(UserAccess userAccess)
        {
            userAccess.Errors.Clear();
            VDeleteObject(userAccess);
            return isValid(userAccess);
        }

        public bool isValid(UserAccess obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(UserAccess obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
