using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class MemorialDetailService : IMemorialDetailService
    {
        private IMemorialDetailRepository _repository;
        private IMemorialDetailValidator _validator;

        public MemorialDetailService(IMemorialDetailRepository _memorialDetailRepository, IMemorialDetailValidator _memorialDetailValidator)
        {
            _repository = _memorialDetailRepository;
            _validator = _memorialDetailValidator;
        }

        public IMemorialDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<MemorialDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<MemorialDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<MemorialDetail> GetObjectsByMemorialId(int memorialId)
        {
            return _repository.GetObjectsByMemorialId(memorialId);
        }
        
        public MemorialDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public MemorialDetail CreateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IAccountService _accountService)
        {
            memorialDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(memorialDetail, _memorialService, this, _accountService) ?
                    _repository.CreateObject(memorialDetail) : memorialDetail);
        }

        public MemorialDetail UpdateObject(MemorialDetail memorialDetail, IMemorialService _memorialService, IAccountService _accountService)
        {
            return (_validator.ValidUpdateObject(memorialDetail, _memorialService, this, _accountService) ?
                     _repository.UpdateObject(memorialDetail) : memorialDetail);
        }
        
        public MemorialDetail SoftDeleteObject(MemorialDetail memorialDetail)
        {
            return (_validator.ValidDeleteObject(memorialDetail) ? _repository.SoftDeleteObject(memorialDetail) : memorialDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public MemorialDetail ConfirmObject(MemorialDetail memorialDetail, DateTime ConfirmationDate, IMemorialService _memorialService)
        {
            memorialDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(memorialDetail))
            {
                memorialDetail = _repository.ConfirmObject(memorialDetail);
            }
            return memorialDetail;
        }

        public MemorialDetail UnconfirmObject(MemorialDetail memorialDetail, IMemorialService _memorialService)
        {
            if (_validator.ValidUnconfirmObject(memorialDetail))
            {
                memorialDetail = _repository.UnconfirmObject(memorialDetail);
            }
            return memorialDetail;
        }
    }
}