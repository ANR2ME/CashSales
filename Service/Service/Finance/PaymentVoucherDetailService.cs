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
    public class PaymentVoucherDetailService : IPaymentVoucherDetailService
    {
        private IPaymentVoucherDetailRepository _repository;
        private IPaymentVoucherDetailValidator _validator;

        public PaymentVoucherDetailService(IPaymentVoucherDetailRepository _paymentVoucherDetailRepository, IPaymentVoucherDetailValidator _paymentVoucherDetailValidator)
        {
            _repository = _paymentVoucherDetailRepository;
            _validator = _paymentVoucherDetailValidator;
        }

        public IPaymentVoucherDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PaymentVoucherDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PaymentVoucherDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<PaymentVoucherDetail> GetQueryableObjectsByPaymentVoucherId(int paymentVoucherId)
        {
            return _repository.GetQueryableObjectsByPaymentVoucherId(paymentVoucherId);
        }

        public IList<PaymentVoucherDetail> GetObjectsByPaymentVoucherId(int paymentVoucherId)
        {
            return _repository.GetObjectsByPaymentVoucherId(paymentVoucherId);
        }

        public IQueryable<PaymentVoucherDetail> GetQueryableObjectsByPayableId(int payableId)
        {
            return _repository.GetQueryableObjectsByPayableId(payableId);
        }

        public IList<PaymentVoucherDetail> GetObjectsByPayableId(int payableId)
        {
            return _repository.GetObjectsByPayableId(payableId);
        }

        public PaymentVoucherDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PaymentVoucherDetail CreateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService,
                                                ICashBankService _cashBankService, IPayableService _payableService)
        {
            paymentVoucherDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(paymentVoucherDetail, _paymentVoucherService, this, _cashBankService, _payableService)) {
                _repository.CreateObject(paymentVoucherDetail);
                CalcAndUpdateTotalAmount(paymentVoucherDetail.PaymentVoucherId, _paymentVoucherService);
            };
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail CreateObject(int paymentVoucherId, int payableId, decimal amount, string description, string Code, 
                                         IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService,
                                         IPayableService _payableService)
        {
            PaymentVoucherDetail paymentVoucherDetail = new PaymentVoucherDetail
            {
                PaymentVoucherId = paymentVoucherId,
                PayableId = payableId,
                Amount = amount,
                Description = description,
                Code = Code,
            };
            return this.CreateObject(paymentVoucherDetail, _paymentVoucherService, _cashBankService, _payableService);
        }

        public PaymentVoucherDetail UpdateObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, ICashBankService _cashBankService, IPayableService _payableService)
        {
            if(_validator.ValidUpdateObject(paymentVoucherDetail, _paymentVoucherService, this, _cashBankService, _payableService))
            {
                _repository.UpdateObject(paymentVoucherDetail);
                CalcAndUpdateTotalAmount(paymentVoucherDetail.PaymentVoucherId, _paymentVoucherService);
            };
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail SoftDeleteObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService)
        {
            if(_validator.ValidDeleteObject(paymentVoucherDetail))
            {
                _repository.SoftDeleteObject(paymentVoucherDetail);
                CalcAndUpdateTotalAmount(paymentVoucherDetail.PaymentVoucherId, _paymentVoucherService);
            };
            return paymentVoucherDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PaymentVoucherDetail ConfirmObject(PaymentVoucherDetail paymentVoucherDetail, DateTime ConfirmationDate,
                                                  IPaymentVoucherService _paymentVoucherService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            paymentVoucherDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(paymentVoucherDetail, _payableService, _paymentVoucherDetailService))
            {
                paymentVoucherDetail = _repository.ConfirmObject(paymentVoucherDetail);

                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
                Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);

                if (paymentVoucher.IsGBCH) { payable.PendingClearanceAmount += paymentVoucherDetail.Amount; }
                payable.RemainingAmount -= paymentVoucherDetail.Amount;
                if (payable.RemainingAmount == 0 && payable.PendingClearanceAmount == 0)
                {
                    payable.IsCompleted = true;
                    payable.CompletionDate = ConfirmationDate; // DateTime.Now;
                }
                _payableService.UpdateObject(payable);
                
            }
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail UnconfirmObject(PaymentVoucherDetail paymentVoucherDetail, IPaymentVoucherService _paymentVoucherService, IPayableService _payableService)
        {
            if (_validator.ValidUnconfirmObject(paymentVoucherDetail))
            {
                paymentVoucherDetail = _repository.UnconfirmObject(paymentVoucherDetail);

                PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(paymentVoucherDetail.PaymentVoucherId);
                Payable payable = _payableService.GetObjectById(paymentVoucherDetail.PayableId);

                if (paymentVoucher.IsGBCH) { payable.PendingClearanceAmount -= paymentVoucherDetail.Amount; }
                payable.RemainingAmount += paymentVoucherDetail.Amount;
                if (payable.RemainingAmount != 0 || payable.PendingClearanceAmount != 0)
                {
                    payable.IsCompleted = false;
                    payable.CompletionDate = null;
                }
                _payableService.UpdateObject(payable);
                
            }
            return paymentVoucherDetail;
        }

        public decimal CalcTotalAmountByPaymentVoucher(int PaymentVoucherId)
        {
            decimal totalamount = 0;
            IList<PaymentVoucherDetail> details = GetObjectsByPaymentVoucherId(PaymentVoucherId);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            return totalamount;
        }

        public decimal CalcTotalAmountByPayable(int PayableId)
        {
            decimal totalamount = 0;
            IList<PaymentVoucherDetail> details = GetObjectsByPayableId(PayableId);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            return totalamount;
        }

        public decimal CalcAndUpdateTotalAmount(int PaymentVoucherId, IPaymentVoucherService _paymentVoucherService)
        {
            PaymentVoucher paymentVoucher = _paymentVoucherService.GetObjectById(PaymentVoucherId);
            paymentVoucher.TotalAmount = CalcTotalAmountByPaymentVoucher(PaymentVoucherId);
            _paymentVoucherService.GetRepository().UpdateObject(paymentVoucher);
            return paymentVoucher.TotalAmount;
        }

    }
}