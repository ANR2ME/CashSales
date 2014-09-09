using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class CashBankMutationService : ICashBankMutationService
    {
        private ICashBankMutationRepository _repository;
        private ICashBankMutationValidator _validator;
        public CashBankMutationService(ICashBankMutationRepository _cashBankMutationRepository, ICashBankMutationValidator _cashBankMutationValidator)
        {
            _repository = _cashBankMutationRepository;
            _validator = _cashBankMutationValidator;
        }

        public ICashBankMutationValidator GetValidator()
        {
            return _validator;
        }

        public ICashBankMutationRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<CashBankMutation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashBankMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public CashBank GetSourceCashBank(CashBankMutation cashBankMutation)
        {
            return _repository.GetSourceCashBank(cashBankMutation);
        }

        public CashBank GetTargetCashBank(CashBankMutation cashBankMutation)
        {
            return _repository.GetSourceCashBank(cashBankMutation);
        }

        public CashBankMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CashBankMutation CreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            cashBankMutation.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(cashBankMutation, _cashBankService)) {
                cashBankMutation.SourceCashBankName = _cashBankService.GetObjectById(cashBankMutation.SourceCashBankId).Name;
                cashBankMutation.TargetCashBankName = _cashBankService.GetObjectById(cashBankMutation.TargetCashBankId).Name;
                _repository.CreateObject(cashBankMutation);
            }
            return cashBankMutation;
        }

        public CashBankMutation UpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            if(_validator.ValidUpdateObject(cashBankMutation, _cashBankService)) {
                cashBankMutation.SourceCashBankName = _cashBankService.GetObjectById(cashBankMutation.SourceCashBankId).Name;
                cashBankMutation.TargetCashBankName = _cashBankService.GetObjectById(cashBankMutation.TargetCashBankId).Name;
                _repository.UpdateObject(cashBankMutation);
            }
            return cashBankMutation;
        }

        public CashBankMutation SoftDeleteObject(CashBankMutation cashBankMutation)
        {
            return (cashBankMutation = _validator.ValidDeleteObject(cashBankMutation) ? _repository.SoftDeleteObject(cashBankMutation) : cashBankMutation);
        }

        public CashBankMutation ConfirmObject(CashBankMutation cashBankMutation, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService)
        {
            cashBankMutation.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(cashBankMutation, _cashBankService))
            {
                CashBank sourceCashBank = _cashBankService.GetObjectById(cashBankMutation.SourceCashBankId);
                CashBank targetCashBank = _cashBankService.GetObjectById(cashBankMutation.TargetCashBankId);
                IList<CashMutation> cashMutations = _cashMutationService.CreateCashMutationForCashBankMutation(cashBankMutation, sourceCashBank, targetCashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService);
                }
                _repository.ConfirmObject(cashBankMutation);
            }
            return cashBankMutation;
        }

        public CashBankMutation UnconfirmObject(CashBankMutation cashBankMutation, ICashMutationService _cashMutationService, ICashBankService _cashBankService)
        {
            if (_validator.ValidUnconfirmObject(cashBankMutation, _cashBankService))
            {
                CashBank sourceCashBank = _cashBankService.GetObjectById(cashBankMutation.SourceCashBankId);
                CashBank targetCashBank = _cashBankService.GetObjectById(cashBankMutation.TargetCashBankId);
                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForCashBankMutation(cashBankMutation, sourceCashBank, targetCashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                }
                _repository.UnconfirmObject(cashBankMutation);
            }
            return cashBankMutation;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}