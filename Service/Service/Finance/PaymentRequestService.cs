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

        public PaymentRequest CreateObject(PaymentRequest paymentRequest, IContactService _contactService, IPaymentRequestDetailService _paymentRequestDetailService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            paymentRequest.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(paymentRequest, _contactService))
            {
                paymentRequest = _repository.CreateObject(paymentRequest);
                PaymentRequestDetail paymentRequestDetail = new PaymentRequestDetail()
                {
                    PaymentRequestId = paymentRequest.Id,
                    AccountId = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayableNonTrading).Id,
                    Amount = paymentRequest.Amount,
                    IsLegacy = true,
                    Status = Constant.GeneralLedgerStatus.Credit
                };
                _paymentRequestDetailService.CreateLegacyObject(paymentRequestDetail, this, _accountService);
            }
            return paymentRequest;
        }

        public PaymentRequest UpdateObject(PaymentRequest paymentRequest, IContactService _contactService, IPaymentRequestDetailService _paymentRequestDetailService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUpdateObject(paymentRequest, _contactService))
            {
                _repository.UpdateObject(paymentRequest);
                PaymentRequestDetail APNonTrading = _paymentRequestDetailService.GetLegacyObjectByPaymentRequestId(paymentRequest.Id);
                APNonTrading.Amount = paymentRequest.Amount;
                _paymentRequestDetailService.UpdateLegacyObject(APNonTrading, this, _accountService);
            }
            return paymentRequest;
        }

        public PaymentRequest SoftDeleteObject(PaymentRequest paymentRequest)
        {
            return (_validator.ValidDeleteObject(paymentRequest) ? _repository.SoftDeleteObject(paymentRequest) : paymentRequest);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentRequest ConfirmObject(PaymentRequest paymentRequest, DateTime ConfirmationDate, IPayableService _payableService,
                                            IPaymentRequestDetailService _paymentRequestDetailService, IAccountService _accountService,
                                            IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            paymentRequest.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(paymentRequest, _paymentRequestDetailService, _closingService))
            {
                // confirm object
                // create payable
                paymentRequest = _repository.ConfirmObject(paymentRequest);
                Payable payable = new Payable()
                {
                    ContactId = paymentRequest.ContactId,
                    PayableSource = Constant.PayableSource.PaymentRequest,
                    PayableSourceId = paymentRequest.Id,
                    Code = paymentRequest.Code,
                    Amount = paymentRequest.Amount,
                    RemainingAmount = paymentRequest.Amount,
                    DueDate = paymentRequest.DueDate
                };
                _payableService.CreateObject(payable);
                _generalLedgerJournalService.CreateConfirmationJournalForPaymentRequest(paymentRequest, _paymentRequestDetailService, _accountService);
            }
            return paymentRequest;
        }

        public PaymentRequest UnconfirmObject(PaymentRequest paymentRequest, IPaymentRequestDetailService _paymentRequestDetailService, IPayableService _payableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(paymentRequest, _paymentRequestDetailService, _closingService))
            {
                _repository.UnconfirmObject(paymentRequest);
                Payable payable = _payableService.GetObjectBySource(Constant.PayableSource.PaymentRequest, paymentRequest.Id);
                _payableService.DeleteObject(payable.Id);
                _generalLedgerJournalService.CreateUnconfirmationJournalForPaymentRequest(paymentRequest, _paymentRequestDetailService, _accountService);
            }
            return paymentRequest;
        }
    }
}