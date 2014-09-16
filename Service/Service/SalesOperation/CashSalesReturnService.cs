using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class CashSalesReturnService : ICashSalesReturnService
    {
        private ICashSalesReturnRepository _repository;
        private ICashSalesReturnValidator _validator;
        public CashSalesReturnService(ICashSalesReturnRepository _cashSalesReturnRepository, ICashSalesReturnValidator _cashSalesReturnValidator)
        {
            _repository = _cashSalesReturnRepository;
            _validator = _cashSalesReturnValidator;
        }

        public ICashSalesReturnValidator GetValidator()
        {
            return _validator;
        }

        public ICashSalesReturnRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<CashSalesReturn> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashSalesReturn> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CashSalesReturn> GetObjectsByCashSalesInvoiceId(int CashSalesInvoiceId)
        {
            return _repository.GetObjectsByCashSalesInvoiceId(CashSalesInvoiceId);
        }

        public CashSalesReturn GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CashSalesReturn CreateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService)
        {
            cashSalesReturn.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(cashSalesReturn, _cashSalesInvoiceService, _cashBankService) ? _repository.CreateObject(cashSalesReturn) : cashSalesReturn);
        }

        public CashSalesReturn UpdateObject(CashSalesReturn cashSalesReturn, ICashSalesInvoiceService _cashSalesInvoiceService, ICashBankService _cashBankService)
        {
            return (cashSalesReturn = _validator.ValidUpdateObject(cashSalesReturn, _cashSalesInvoiceService, _cashBankService) ? _repository.UpdateObject(cashSalesReturn) : cashSalesReturn);
        }

        public CashSalesReturn ConfirmObject(CashSalesReturn cashSalesReturn, DateTime ConfirmationDate, decimal Allowance, 
                                                ICashSalesReturnDetailService _cashSalesReturnDetailService, IContactService _contactService,
                                                ICashSalesInvoiceService _cashSalesInvoiceService, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                IPriceMutationService _priceMutationService, IPayableService _payableService, 
                                                ICashSalesReturnService _cashSalesReturnService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            cashSalesReturn.ConfirmationDate = ConfirmationDate;
            cashSalesReturn.Allowance = Allowance;
            if (_validator.ValidConfirmObject(cashSalesReturn, _cashSalesReturnDetailService, _cashSalesReturnService, _cashSalesInvoiceService, _cashSalesInvoiceDetailService))
            {
                IList<CashSalesReturnDetail> cashSalesReturnDetails = _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(cashSalesReturn.Id);
                foreach (var cashSalesReturnDetail in cashSalesReturnDetails)
                {
                    cashSalesReturnDetail.Errors = new Dictionary<string, string>();
                    _cashSalesReturnDetailService.ConfirmObject(cashSalesReturnDetail, _cashSalesReturnService, _cashSalesInvoiceService, _cashSalesInvoiceDetailService,
                                                                _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService, _cashSalesReturnDetailService);
                }
                // DueDate untuk Payable dari mana ?
                Contact contact = _contactService.GetObjectByName(Core.Constants.Constant.BaseContact);
                Payable payable = _payableService.CreateObject(contact.Id, Core.Constants.Constant.PayableSource.CashSalesReturn, cashSalesReturn.Id, cashSalesReturn.Code, cashSalesReturn.Total, ConfirmationDate.Add(Core.Constants.Constant.PaymentDueDateTimeSpan));
                cashSalesReturn = _repository.ConfirmObject(cashSalesReturn);
            }
            else
            {
                cashSalesReturn.ConfirmationDate = null;
                cashSalesReturn.Allowance = 0;
            }
            return cashSalesReturn;
        }

        public CashSalesReturn UnconfirmObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService,
                                                  ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                  IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                  IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService, 
                                                  IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(cashSalesReturn, _cashSalesReturnDetailService, _payableService, _paymentVoucherDetailService))
            {
                cashSalesReturn = _repository.UnconfirmObject(cashSalesReturn);
                IList<CashSalesReturnDetail> cashSalesReturnDetails = _cashSalesReturnDetailService.GetObjectsByCashSalesReturnId(cashSalesReturn.Id);
                foreach (var cashSalesReturnDetail in cashSalesReturnDetails)
                {
                    cashSalesReturnDetail.Errors = new Dictionary<string, string>();
                    _cashSalesReturnDetailService.UnconfirmObject(cashSalesReturnDetail, _cashSalesInvoiceDetailService, _warehouseItemService,
                                                                  _warehouseService, _itemService, _barringService, _stockMutationService);
                }
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CashSalesReturn, cashSalesReturn.Id);
                _payableService.SoftDeleteObject(payable);
                cashSalesReturn.Total = 0;
                cashSalesReturn.Allowance = 0;
            }
            return cashSalesReturn;
        }

        public CashSalesReturn PaidObject(CashSalesReturn cashSalesReturn, /*decimal Allowance,*/ ICashBankService _cashBankService, IPayableService _payableService, 
                                          IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, 
                                          IContactService _contactService, ICashMutationService _cashMutationService,
                                          IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService)
        {
            if (_validator.ValidPaidObject(cashSalesReturn, _cashBankService))
            {
                CashBank cashBank = _cashBankService.GetObjectById((int)cashSalesReturn.CashBankId.GetValueOrDefault());
                //cashSalesReturn.Allowance = Allowance;
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CashSalesReturn, cashSalesReturn.Id);
                payable.AllowanceAmount = cashSalesReturn.Allowance;
                payable.RemainingAmount = payable.Amount - payable.AllowanceAmount;
                _payableService.UpdateObject(payable);
                PaymentVoucher paymentVoucher = _paymentVoucherService.CreateObject((int)cashSalesReturn.CashBankId.GetValueOrDefault(), payable.ContactId,
                                                                                    DateTime.Now, payable.RemainingAmount, false, payable.DueDate, cashBank.IsBank,
                                                                                    _paymentVoucherDetailService, _payableService, _contactService, _cashBankService);
                PaymentVoucherDetail paymentVoucherDetail = _paymentVoucherDetailService.CreateObject(paymentVoucher.Id, payable.Id, payable.RemainingAmount,
                                                                                "Automatic Payment", _paymentVoucherService, _cashBankService, _payableService);
                _paymentVoucherService.ConfirmObject(paymentVoucher, (DateTime)cashSalesReturn.ConfirmationDate.GetValueOrDefault(), _paymentVoucherDetailService,
                                                     _cashBankService, _payableService, _cashMutationService, _generalLedgerJournalService, _accountService);
                cashSalesReturn = _repository.PaidObject(cashSalesReturn);
            }
            return cashSalesReturn;
        }

        public CashSalesReturn UnpaidObject(CashSalesReturn cashSalesReturn, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                            IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService)
        {
            if (_validator.ValidUnpaidObject(cashSalesReturn))
            {
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CashSalesReturn, cashSalesReturn.Id);
                IList<PaymentVoucher> paymentVouchers = _paymentVoucherService.GetObjectsByCashBankId((int)cashSalesReturn.CashBankId.GetValueOrDefault());
                foreach (var paymentVoucher in paymentVouchers)
                {
                    if (paymentVoucher.ContactId == payable.ContactId)
                    {
                        paymentVoucher.Errors = new Dictionary<string, string>();
                        _paymentVoucherService.UnconfirmObject(paymentVoucher, _paymentVoucherDetailService, _cashBankService, _payableService,
                                                               _cashMutationService, _generalLedgerJournalService, _accountService);

                        IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                        foreach (var paymentVoucherDetail in paymentVoucherDetails)
                        {
                            paymentVoucherDetail.Errors = new Dictionary<string, string>();
                            _paymentVoucherDetailService.SoftDeleteObject(paymentVoucherDetail);
                        }
                        _paymentVoucherService.SoftDeleteObject(paymentVoucher, _paymentVoucherDetailService);
                    }
                }
                payable.AllowanceAmount = 0;
                _payableService.UpdateObject(payable);
                cashSalesReturn.Allowance = 0;
                cashSalesReturn = _repository.UnpaidObject(cashSalesReturn);
            }
            return cashSalesReturn;
        }

        public CashSalesReturn SoftDeleteObject(CashSalesReturn cashSalesReturn, ICashSalesReturnDetailService _cashSalesReturnDetailService)
        {
            return (cashSalesReturn = _validator.ValidDeleteObject(cashSalesReturn, _cashSalesReturnDetailService) ?
                    _repository.SoftDeleteObject(cashSalesReturn) : cashSalesReturn);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
