using Core.Constants;
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
    public class MemorialService : IMemorialService
    {
        private IMemorialRepository _repository;
        private IMemorialValidator _validator;

        public MemorialService(IMemorialRepository _memorialRepository, IMemorialValidator _memorialValidator)
        {
            _repository = _memorialRepository;
            _validator = _memorialValidator;
        }

        public IMemorialValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Memorial> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Memorial> GetAll()
        {
            return _repository.GetAll();
        }

        public Memorial GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Memorial CreateObject(Memorial memorial)
        {
            memorial.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(memorial))
            {
                memorial = _repository.CreateObject(memorial);
            }
            return memorial;
        }

        public Memorial UpdateObject(Memorial memorial)
        {
            if (_validator.ValidUpdateObject(memorial))
            {
                _repository.UpdateObject(memorial);
            }
            return memorial;
        }

        public Memorial SoftDeleteObject(Memorial memorial)
        {
            return (_validator.ValidDeleteObject(memorial) ? _repository.SoftDeleteObject(memorial) : memorial);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public Memorial ConfirmObject(Memorial memorial, DateTime ConfirmationDate, IMemorialDetailService _memorialDetailService,
                                      IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            memorial.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(memorial, _memorialDetailService, _closingService))
            {
                IList<MemorialDetail> memorialDetails = _memorialDetailService.GetObjectsByMemorialId(memorial.Id);
                foreach (var detail in memorialDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _memorialDetailService.ConfirmObject(detail, ConfirmationDate, this);
                }
                memorial = _repository.ConfirmObject(memorial);
                _generalLedgerJournalService.CreateConfirmationJournalForMemorial(memorial, _memorialDetailService, _accountService);
            }
            return memorial;
        }

        public Memorial UnconfirmObject(Memorial memorial, IMemorialDetailService _memorialDetailService, 
                                        IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(memorial, _memorialDetailService, _closingService))
            {
                DateTime confirmdate = memorial.ConfirmationDate.GetValueOrDefault();
                IList<MemorialDetail> memorialDetails = _memorialDetailService.GetObjectsByMemorialId(memorial.Id);
                foreach (var detail in memorialDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _memorialDetailService.UnconfirmObject(detail, this);
                }
                _repository.UnconfirmObject(memorial);
                _generalLedgerJournalService.CreateUnconfirmationJournalForMemorial(memorial, confirmdate, _memorialDetailService, _accountService);
            }
            return memorial;
        }
    }
}