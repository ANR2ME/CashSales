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
    public class UserMenuService : IUserMenuService
    {
        private IUserMenuRepository _repository;
        private IUserMenuValidator _validator;

        public UserMenuService(IUserMenuRepository _userMenuRepository, IUserMenuValidator _userMenuValidator)
        {
            _repository = _userMenuRepository;
            _validator = _userMenuValidator;
        }

        public IUserMenuValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<UserMenu> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<UserMenu> GetAll()
        {
            return _repository.GetAll();
        }

        public UserMenu GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public UserMenu GetObjectByNameAndGroupName(string Name, string GroupName)
        {
            return _repository.GetObjectByNameAndGroupName(Name, GroupName);
        }

        public UserMenu CreateObject(UserMenu userMenu)
        {
            userMenu.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(userMenu, this) ? _repository.CreateObject(userMenu) : userMenu);
        }

        public UserMenu CreateObject(string Name, string GroupName)
        {
            UserMenu userMenu = new UserMenu()
            {
                Name = Name,
                GroupName = GroupName
            };
            return CreateObject(userMenu);
        }

        public UserMenu SoftDeleteObject(UserMenu userMenu)
        {
            return (_validator.ValidDeleteObject(userMenu) ? _repository.SoftDeleteObject(userMenu) : userMenu);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
