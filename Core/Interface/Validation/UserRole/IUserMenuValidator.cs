using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IUserMenuValidator
    {
        UserMenu VIsValidName(UserMenu userMenu);
        UserMenu VIsValidGroupName(UserMenu userMenu);
        UserMenu VHasUniqueNameAndGroupName(UserMenu userMenu, IUserMenuService _userMenuService);
        UserMenu VCreateObject(UserMenu userMenu, IUserMenuService _userMenuService);
        UserMenu VDeleteObject(UserMenu userMenu);
        bool ValidCreateObject(UserMenu userMenu, IUserMenuService _userMenuService);
        bool ValidDeleteObject(UserMenu userMenu);
        bool isValid(UserMenu userMenu);
        string PrintError(UserMenu userMenu);
    }
}
