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
    public class UserMenuValidator : IUserMenuValidator
    {

        public UserMenu VIsValidName(UserMenu userMenu)
        {
            if (userMenu.Name == null || userMenu.Name.Trim() == "")
            {
                userMenu.Errors.Add("Name", "Tidak boleh kosong");
            }
            return userMenu;
        }

        public UserMenu VIsValidGroupName(UserMenu userMenu)
        {
            if (userMenu.GroupName == null || userMenu.GroupName.Trim() == "")
            {
                userMenu.Errors.Add("GroupName", "Tidak boleh kosong");
            }
            return userMenu;
        }

        public UserMenu VHasUniqueNameAndGroupName(UserMenu userMenu, IUserMenuService _userMenuService)
        {
            UserMenu userMenu2 = _userMenuService.GetObjectByNameAndGroupName(userMenu.Name.Trim(), userMenu.GroupName.Trim());
            if (userMenu2 != null && userMenu.Id != userMenu2.Id)
            {
                userMenu.Errors.Add("Generic", "Kombinasi Name dan GroupName sudah ada");
            }
            return userMenu;
        }

        public UserMenu VCreateObject(UserMenu userMenu, IUserMenuService _userMenuService)
        {
            VIsValidName(userMenu);
            if (!isValid(userMenu)) { return userMenu; }
            VIsValidGroupName(userMenu);
            if (!isValid(userMenu)) { return userMenu; }
            VHasUniqueNameAndGroupName(userMenu, _userMenuService);
            return userMenu;
        }

        public UserMenu VDeleteObject(UserMenu userMenu)
        {
            return userMenu;
        }

        public bool ValidCreateObject(UserMenu userMenu, IUserMenuService _userMenuService)
        {
            VCreateObject(userMenu, _userMenuService);
            return isValid(userMenu);
        }

        public bool ValidDeleteObject(UserMenu userMenu)
        {
            userMenu.Errors.Clear();
            VDeleteObject(userMenu);
            return isValid(userMenu);
        }

        public bool isValid(UserMenu obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(UserMenu obj)
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
