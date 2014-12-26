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
    public class ReceiptVoucherDetailService : IReceiptVoucherDetailService
    {
        private IReceiptVoucherDetailRepository _repository;
        private IReceiptVoucherDetailValidator _validator;

        public ReceiptVoucherDetailService(IReceiptVoucherDetailRepository _receiptVoucherDetailRepository, IReceiptVoucherDetailValidator _receiptVoucherDetailValidator)
        {
            _repository = _receiptVoucherDetailRepository;
            _validator = _receiptVoucherDetailValidator;
        }

        public IReceiptVoucherDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ReceiptVoucherDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ReceiptVoucherDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<ReceiptVoucherDetail> GetQueryableObjectsByReceiptVoucherId(int receiptVoucherId)
        {
            return _repository.GetQueryableObjectsByReceiptVoucherId(receiptVoucherId);
        }

        public IList<ReceiptVoucherDetail> GetObjectsByReceiptVoucherId(int receiptVoucherId)
        {
            return _repository.GetObjectsByReceiptVoucherId(receiptVoucherId);
        }

        public IQueryable<ReceiptVoucherDetail> GetQueryableObjectsByReceivableId(int receivableId)
        {
            return _repository.GetQueryableObjectsByReceivableId(receivableId);
        }

        public IList<ReceiptVoucherDetail> GetObjectsByReceivableId(int receivableId)
        {
            return _repository.GetObjectsByReceivableId(receivableId);
        }

        public ReceiptVoucherDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ReceiptVoucherDetail CreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService,
                                                ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            receiptVoucherDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(receiptVoucherDetail, _receiptVoucherService, this, _cashBankService, _receivableService))
            {
                _repository.CreateObject(receiptVoucherDetail);
                CalcAndUpdateTotalAmount(receiptVoucherDetail.ReceiptVoucherId, _receiptVoucherService);
            };
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail CreateObject(int receiptVoucherId, int receivableId, decimal amount, string description, string Code, 
                                         IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService,
                                         IReceivableService _receivableService)
        {
            ReceiptVoucherDetail receiptVoucherDetail = new ReceiptVoucherDetail
            {
                ReceiptVoucherId = receiptVoucherId,
                ReceivableId = receivableId,
                Amount = amount,
                Description = description,
                Code = Code,
            };
            return this.CreateObject(receiptVoucherDetail, _receiptVoucherService, _cashBankService, _receivableService);
        }

        public ReceiptVoucherDetail UpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            if(_validator.ValidUpdateObject(receiptVoucherDetail, _receiptVoucherService, this, _cashBankService, _receivableService))
            {
                _repository.UpdateObject(receiptVoucherDetail);
                CalcAndUpdateTotalAmount(receiptVoucherDetail.ReceiptVoucherId, _receiptVoucherService);
            };
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail SoftDeleteObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService)
        {
            if(_validator.ValidDeleteObject(receiptVoucherDetail))
            {
                _repository.SoftDeleteObject(receiptVoucherDetail);
                CalcAndUpdateTotalAmount(receiptVoucherDetail.ReceiptVoucherId, _receiptVoucherService);
            };
            return receiptVoucherDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public ReceiptVoucherDetail ConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, DateTime ConfirmationDate, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            receiptVoucherDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(receiptVoucherDetail, _receivableService, _receiptVoucherDetailService))
            {
                receiptVoucherDetail = _repository.ConfirmObject(receiptVoucherDetail);

                ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
                Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);

                if (receiptVoucher.IsGBCH) { receivable.PendingClearanceAmount += receiptVoucherDetail.Amount; }
                receivable.RemainingAmount -= receiptVoucherDetail.Amount;
                if (receivable.RemainingAmount == 0 && receivable.PendingClearanceAmount == 0)
                {
                    receivable.IsCompleted = true;
                    receivable.CompletionDate = ConfirmationDate; // DateTime.Now;
                }
                _receivableService.UpdateObject(receivable);
                
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail UnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceivableService _receivableService)
        {
            if (_validator.ValidUnconfirmObject(receiptVoucherDetail))
            {
                receiptVoucherDetail = _repository.UnconfirmObject(receiptVoucherDetail);

                ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
                Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);

                if (receiptVoucher.IsGBCH) { receivable.PendingClearanceAmount -= receiptVoucherDetail.Amount; }
                receivable.RemainingAmount += receiptVoucherDetail.Amount;
                if (receivable.RemainingAmount != 0 || receivable.PendingClearanceAmount != 0)
                {
                    receivable.IsCompleted = false;
                    receivable.CompletionDate = null;
                }
                _receivableService.UpdateObject(receivable);
                
            }
            return receiptVoucherDetail;
        }

        public decimal CalcTotalAmountByReceiptVoucher(int ReceiptVoucherId)
        {
            decimal totalamount = 0;
            IList<ReceiptVoucherDetail> details = GetObjectsByReceiptVoucherId(ReceiptVoucherId);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            return totalamount;
        }

        public decimal CalcTotalAmountByReceivable(int ReceivableId)
        {
            decimal totalamount = 0;
            IList<ReceiptVoucherDetail> details = GetObjectsByReceivableId(ReceivableId);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            return totalamount;
        }

        public decimal CalcAndUpdateTotalAmount(int ReceiptVoucherId, IReceiptVoucherService _receiptVoucherService)
        {
            ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(ReceiptVoucherId);
            receiptVoucher.TotalAmount = CalcTotalAmountByReceiptVoucher(ReceiptVoucherId);
            _receiptVoucherService.GetRepository().UpdateObject(receiptVoucher);
            return receiptVoucher.TotalAmount;
        }

    }
}