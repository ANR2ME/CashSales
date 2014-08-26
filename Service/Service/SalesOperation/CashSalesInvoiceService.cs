﻿using System;
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
    public class CashSalesInvoiceService : ICashSalesInvoiceService
    {
        private ICashSalesInvoiceRepository _repository;
        private ICashSalesInvoiceValidator _validator;
        public CashSalesInvoiceService(ICashSalesInvoiceRepository _cashSalesInvoiceRepository, ICashSalesInvoiceValidator _cashSalesInvoiceValidator)
        {
            _repository = _cashSalesInvoiceRepository;
            _validator = _cashSalesInvoiceValidator;
        }

        public ICashSalesInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public ICashSalesInvoiceRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<CashSalesInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashSalesInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public CashSalesInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CashSalesInvoice CreateObject(CashSalesInvoice cashSalesInvoice, IWarehouseService _warehouseService)
        {
            cashSalesInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(cashSalesInvoice, _warehouseService) ? _repository.CreateObject(cashSalesInvoice) : cashSalesInvoice);
        }

        public CashSalesInvoice UpdateObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            return (cashSalesInvoice = _validator.ValidUpdateObject(cashSalesInvoice, _cashSalesInvoiceDetailService) ? _repository.UpdateObject(cashSalesInvoice) : cashSalesInvoice);
        }

        public CashSalesInvoice ConfirmObject(CashSalesInvoice cashSalesInvoice, DateTime ConfirmationDate, decimal Discount, decimal Tax, 
                                                ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService, IContactService _contactService,
                                                IPriceMutationService _priceMutationService, IReceivableService _receivableService, 
                                                ICashSalesInvoiceService _cashSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, 
                                                IStockMutationService _stockMutationService, ICashBankService _cashBankService)
        {
            cashSalesInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(cashSalesInvoice, _cashSalesInvoiceDetailService, _cashSalesInvoiceService, _warehouseItemService, _contactService, _cashBankService))
            {
                cashSalesInvoice.Discount = Discount;
                cashSalesInvoice.Tax = Tax;
                IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id);
                cashSalesInvoice.Total = 0;
                cashSalesInvoice.CoGS = 0;
                foreach (var cashSalesInvoiceDetail in cashSalesInvoiceDetails)
                {
                    cashSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    _cashSalesInvoiceDetailService.ConfirmObject(cashSalesInvoiceDetail, _cashSalesInvoiceService, _warehouseItemService,
                                                                   _warehouseService, _itemService, _barringService, _stockMutationService);
                    cashSalesInvoice.Total += cashSalesInvoiceDetail.Amount;
                    cashSalesInvoice.CoGS += cashSalesInvoiceDetail.CoGS;
                }
                // Tax dihitung setelah Discount
                cashSalesInvoice.Total = (cashSalesInvoice.Total * ((100 - cashSalesInvoice.Discount) / 100) * ((100 + cashSalesInvoice.Tax) / 100));
                Contact contact = _contactService.GetObjectByName(Core.Constants.Constant.BaseContact);
                Receivable receivable = _receivableService.CreateObject(contact.Id, Core.Constants.Constant.ReceivableSource.CashSalesInvoice, cashSalesInvoice.Id, cashSalesInvoice.Total, (DateTime)cashSalesInvoice.DueDate);
                cashSalesInvoice = _repository.ConfirmObject(cashSalesInvoice);
            }
            else
            {
                cashSalesInvoice.ConfirmationDate = null;
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice UnconfirmObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService,
                                                  IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                  IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService, 
                                                  IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(cashSalesInvoice, _cashSalesInvoiceDetailService, _receivableService, _receiptVoucherDetailService))
            {
                cashSalesInvoice = _repository.UnconfirmObject(cashSalesInvoice);
                IList<CashSalesInvoiceDetail> cashSalesInvoiceDetails = _cashSalesInvoiceDetailService.GetObjectsByCashSalesInvoiceId(cashSalesInvoice.Id);
                foreach (var cashSalesInvoiceDetail in cashSalesInvoiceDetails)
                {
                    cashSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    _cashSalesInvoiceDetailService.UnconfirmObject(cashSalesInvoiceDetail, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService);
                }
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, cashSalesInvoice.Id);
                _receivableService.SoftDeleteObject(receivable);
                cashSalesInvoice.CoGS = 0;
                cashSalesInvoice.Total = 0;
                cashSalesInvoice.Discount = 0;
                cashSalesInvoice.Tax = 0;
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice PaidObject(CashSalesInvoice cashSalesInvoice, decimal AmountPaid, decimal Allowance, ICashBankService _cashBankService, IReceivableService _receivableService, 
                                             IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                             IContactService _contactService, ICashMutationService _cashMutationService, ICashSalesReturnService _cashSalesReturnService)
        {
            if (_validator.ValidPaidObject(cashSalesInvoice, _cashBankService, _receiptVoucherService, _cashSalesReturnService))
            {
                CashBank cashBank = _cashBankService.GetObjectById((int)cashSalesInvoice.CashBankId);
                cashSalesInvoice.IsBank = cashBank.IsBank;
                cashSalesInvoice.AmountPaid = AmountPaid;
                cashSalesInvoice.Allowance = Allowance;
                if (cashSalesInvoice.AmountPaid == cashSalesInvoice.Total)
                {
                    cashSalesInvoice.IsFullPayment = true;
                }
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, cashSalesInvoice.Id);
                receivable.AllowanceAmount = Allowance;
                receivable.RemainingAmount = receivable.Amount - receivable.AllowanceAmount;
                _receivableService.UpdateObject(receivable);
                ReceiptVoucher receiptVoucher = _receiptVoucherService.CreateObject((int)cashSalesInvoice.CashBankId, receivable.ContactId, DateTime.Now, receivable.RemainingAmount,
                                                                            false, (DateTime)cashSalesInvoice.DueDate, cashSalesInvoice.IsBank, _receiptVoucherDetailService,
                                                                            _receivableService, _contactService, _cashBankService);
                ReceiptVoucherDetail receiptVoucherDetail = _receiptVoucherDetailService.CreateObject(receiptVoucher.Id, receivable.Id, (decimal)receivable.RemainingAmount, 
                                                                            "Automatic Payment", _receiptVoucherService, _cashBankService, _receivableService);
                cashSalesInvoice = _repository.PaidObject(cashSalesInvoice);
                _receiptVoucherService.ConfirmObject(receiptVoucher, (DateTime)cashSalesInvoice.ConfirmationDate, _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService);
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice UnpaidObject(CashSalesInvoice cashSalesInvoice, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                               ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService, ICashSalesReturnService _cashSalesReturnService)
        {
            if (_validator.ValidUnpaidObject(cashSalesInvoice, _cashSalesReturnService))
            {
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, cashSalesInvoice.Id);
                IList<ReceiptVoucher> receiptVouchers = _receiptVoucherService.GetObjectsByCashBankId((int)cashSalesInvoice.CashBankId);
                foreach (var receiptVoucher in receiptVouchers)
                {
                    if (receiptVoucher.ContactId == receivable.ContactId)
                    {
                        receiptVoucher.Errors = new Dictionary<string, string>();
                        _receiptVoucherService.UnconfirmObject(receiptVoucher, _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService);

                        IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
                        foreach (var receiptVoucherDetail in receiptVoucherDetails)
                        {
                            receiptVoucherDetail.Errors = new Dictionary<string, string>();
                            _receiptVoucherDetailService.SoftDeleteObject(receiptVoucherDetail);
                        }
                        _receiptVoucherService.SoftDeleteObject(receiptVoucher, _receiptVoucherDetailService);
                    }
                }
                receivable.AllowanceAmount = 0;
                _receivableService.UpdateObject(receivable);
                cashSalesInvoice.AmountPaid = 0;
                cashSalesInvoice.IsFullPayment = false;
                cashSalesInvoice.Allowance = 0;
                cashSalesInvoice = _repository.UnpaidObject(cashSalesInvoice);
            }
            return cashSalesInvoice;
        }

        public CashSalesInvoice SoftDeleteObject(CashSalesInvoice cashSalesInvoice, ICashSalesInvoiceDetailService _cashSalesInvoiceDetailService)
        {
            return (cashSalesInvoice = _validator.ValidDeleteObject(cashSalesInvoice, _cashSalesInvoiceDetailService) ?
                    _repository.SoftDeleteObject(cashSalesInvoice) : cashSalesInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
