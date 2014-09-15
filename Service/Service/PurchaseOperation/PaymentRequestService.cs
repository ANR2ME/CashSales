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
    public class PaymentRequestService : IPaymentRequestService
    {
        private IPaymentRequestRepository _repository;
        private IPaymentRequestValidator _validator;

        public PaymentRequestService(IPaymentRequestRepository _paymentRequestRepository, IPaymentRequestValidator _paymentRequestValidator)
        {
            _repository = _paymentRequestRepository;
            _validator = _paymentRequestValidator;
        }

        public IPaymentRequestValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PaymentRequest> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PaymentRequest> GetAll()
        {
            return _repository.GetAll();
        }

        public PaymentRequest GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PaymentRequest> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PaymentRequest CreateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            paymentRequest.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(paymentRequest, _contactService) ? _repository.CreateObject(paymentRequest) : paymentRequest);
        }

        public PaymentRequest CreateObject(int contactId, string description, decimal amount, DateTime requestedDate, DateTime dueDate, IContactService _contactService)
        {
            PaymentRequest paymentRequest = new PaymentRequest
            {
                ContactId = contactId,
                Description = description,
                Amount = amount,
                RequestedDate = requestedDate,
                DueDate = dueDate
            };
            return this.CreateObject(paymentRequest, _contactService);
        }

        public PaymentRequest UpdateObject(PaymentRequest paymentRequest, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(paymentRequest, _contactService) ? _repository.UpdateObject(paymentRequest) : paymentRequest);
        }

        public PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest)
        {
            return (_validator.ValidDeleteObject(paymentRequest) ? _repository.SoftDeleteObject(paymentRequest) : paymentRequest);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentRequest ConfirmObject(PaymentRequest paymentRequest, DateTime ConfirmationDate, IPayableService _payableService)
        {
            paymentRequest.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(paymentRequest))
            {
                // confirm object
                // create payable
                paymentRequest = _repository.ConfirmObject(paymentRequest);
                Payable payable = _payableService.CreateObject(paymentRequest.ContactId, Constant.PayableSource.PaymentRequest, paymentRequest.Id, paymentRequest.Code, paymentRequest.Amount, paymentRequest.DueDate);
            }
            else
            {
                paymentRequest.ConfirmationDate = null;
            }
            return paymentRequest;
        }

        public PaymentRequest UnconfirmObject(PaymentRequest paymentRequest, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            if (_validator.ValidUnconfirmObject(paymentRequest, _paymentVoucherDetailService, _payableService))
            {
                _repository.UnconfirmObject(paymentRequest);
                Payable payable = _payableService.GetObjectBySource(Constant.PayableSource.PaymentRequest, paymentRequest.Id);
                _payableService.SoftDeleteObject(payable);
            }
            return paymentRequest;
        }

    }
}