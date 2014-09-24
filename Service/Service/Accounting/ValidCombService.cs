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
    public class ValidCombService : IValidCombService
    {
        private IValidCombRepository _repository;
        private IValidCombValidator _validator;

        public ValidCombService(IValidCombRepository _validCombRepository, IValidCombValidator _validCombValidator)
        {
            _repository = _validCombRepository;
            _validator = _validCombValidator;
        }

        public IValidCombValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ValidComb> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ValidComb> GetAll()
        {
            return _repository.GetAll();
        }

        public ValidComb GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ValidComb FindOrCreateObjectByAccountAndClosing(int accountId, int closingId)
        {
            return _repository.FindOrCreateObjectByAccountAndClosing(accountId, closingId);
        }

        public ValidComb CreateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService)
        {
            validComb.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(validComb, _accountService, _closingService) ? _repository.CreateObject(validComb) : validComb);
        }

        public ValidComb UpdateObject(ValidComb validComb, IAccountService _accountService, IClosingService _closingService)
        {
            return (_validator.ValidUpdateObject(validComb, _accountService, _closingService) ? _repository.UpdateObject(validComb) : validComb);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public ValidComb CalculateTotalAmount(ValidComb validComb, IAccountService _accountService, IClosingService _closingService)
        {
            decimal totalAmount = 0;

            IList<Account> subnodeaccounts = _accountService.GetQueryable().Where(x => x.ParentId == validComb.AccountId).ToList();
            if (subnodeaccounts.Any())
            {
                foreach (var subnode in subnodeaccounts)
                {
                    totalAmount += FindOrCreateObjectByAccountAndClosing(subnode.Id, validComb.ClosingId).Amount;
                }
                validComb.Amount = totalAmount;
                UpdateObject(validComb, _accountService, _closingService);
            }
            return validComb;
        }
    }
}
