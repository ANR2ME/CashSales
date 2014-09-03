using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IUserAccessValidator
    {
        UserAccess VIsValidUserAccount(UserAccess userAccess, IUserAccountService _userAccountService);
        UserAccess VIsValidUserMenu(UserAccess userAccess, IUserMenuService _userMenuService);
        UserAccess VHasUniqueUserMenuAndUserAccountCombination(UserAccess userAccess, IUserAccessService _userAccessService);
        UserAccess VCreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService);
        UserAccess VUpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService);
        UserAccess VDeleteObject(UserAccess userAccess);
        bool ValidCreateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService);
        bool ValidUpdateObject(UserAccess userAccess, IUserAccountService _userAccountService, IUserMenuService _userMenuService, IUserAccessService _userAccessService);
        bool ValidDeleteObject(UserAccess userAccess);
        bool isValid(UserAccess userAccess);
        string PrintError(UserAccess userAccess);
    }
}
