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
    public class RetailPurchaseInvoiceService : IRetailPurchaseInvoiceService
    {
        private IRetailPurchaseInvoiceRepository _repository;
        private IRetailPurchaseInvoiceValidator _validator;
        public RetailPurchaseInvoiceService(IRetailPurchaseInvoiceRepository _retailPurchaseInvoiceRepository, IRetailPurchaseInvoiceValidator _retailPurchaseInvoiceValidator)
        {
            _repository = _retailPurchaseInvoiceRepository;
            _validator = _retailPurchaseInvoiceValidator;
        }

        public IRetailPurchaseInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public IRetailPurchaseInvoiceRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<RetailPurchaseInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RetailPurchaseInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public RetailPurchaseInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RetailPurchaseInvoice CreateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            retailPurchaseInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(retailPurchaseInvoice, _warehouseService) ? _repository.CreateObject(retailPurchaseInvoice) : retailPurchaseInvoice);
        }

        public RetailPurchaseInvoice UpdateObject(RetailPurchaseInvoice retailPurchaseInvoice, IWarehouseService _warehouseService)
        {
            return (retailPurchaseInvoice = _validator.ValidUpdateObject(retailPurchaseInvoice, _warehouseService) ? _repository.UpdateObject(retailPurchaseInvoice) : retailPurchaseInvoice);
        }

        public RetailPurchaseInvoice ConfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, DateTime ConfirmationDate, int ContactId, 
                                                IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService, IContactService _contactService,
                                                IPriceMutationService _priceMutationService, IPayableService _payableService, 
                                                IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            
            retailPurchaseInvoice.ContactId = ContactId;
            retailPurchaseInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService, _retailPurchaseInvoiceService, _warehouseItemService, _contactService))
            {
                IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
                retailPurchaseInvoice.Total = 0;
                retailPurchaseInvoice.CoGS = 0;
                foreach (var retailPurchaseInvoiceDetail in retailPurchaseInvoiceDetails)
                {
                    retailPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    _retailPurchaseInvoiceDetailService.ConfirmObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _warehouseItemService,
                                                                   _warehouseService, _itemService, _barringService, _stockMutationService);
                    retailPurchaseInvoice.Total += retailPurchaseInvoiceDetail.Amount;
                    retailPurchaseInvoice.CoGS += retailPurchaseInvoiceDetail.CoGS;
                }
                // Tax dihitung setelah discount
                retailPurchaseInvoice.Total = (retailPurchaseInvoice.Total * (100 - retailPurchaseInvoice.Discount) / 100) * (100 - retailPurchaseInvoice.Tax) / 100;
                Payable payable = _payableService.CreateObject(retailPurchaseInvoice.ContactId, Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id, retailPurchaseInvoice.Total, (DateTime)retailPurchaseInvoice.DueDate.GetValueOrDefault());
                retailPurchaseInvoice = _repository.ConfirmObject(retailPurchaseInvoice);
            }
            else
            {
                retailPurchaseInvoice.ConfirmationDate = null;
                //retailPurchaseInvoice.ContactId = 0; //null;
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice UnconfirmObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService,
                                                  IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                  IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService, 
                                                  IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService, _payableService, _paymentVoucherDetailService))
            {
                retailPurchaseInvoice = _repository.UnconfirmObject(retailPurchaseInvoice);
                IList<RetailPurchaseInvoiceDetail> retailPurchaseInvoiceDetails = _retailPurchaseInvoiceDetailService.GetObjectsByRetailPurchaseInvoiceId(retailPurchaseInvoice.Id);
                foreach (var retailPurchaseInvoiceDetail in retailPurchaseInvoiceDetails)
                {
                    retailPurchaseInvoiceDetail.Errors = new Dictionary<string, string>();
                    _retailPurchaseInvoiceDetailService.UnconfirmObject(retailPurchaseInvoiceDetail, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService);
                }
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id);
                _payableService.SoftDeleteObject(payable);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice PaidObject(RetailPurchaseInvoice retailPurchaseInvoice, decimal AmountPaid, ICashBankService _cashBankService, IPayableService _payableService, 
                                             IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, IContactService _contactService, ICashMutationService _cashMutationService)
        {
            retailPurchaseInvoice.AmountPaid = AmountPaid;
            if (_validator.ValidPaidObject(retailPurchaseInvoice, _cashBankService, _paymentVoucherService))
            {
                CashBank cashBank = _cashBankService.GetObjectById((int)retailPurchaseInvoice.CashBankId.GetValueOrDefault());
                retailPurchaseInvoice.IsBank = cashBank.IsBank;
                
                if (!retailPurchaseInvoice.IsGBCH)
                {
                    retailPurchaseInvoice.GBCH_No = null;
                    retailPurchaseInvoice.Description = null;
                }
                if (retailPurchaseInvoice.AmountPaid == retailPurchaseInvoice.Total)
                {
                    retailPurchaseInvoice.IsFullPayment = true;
                }
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id);
                PaymentVoucher paymentVoucher = _paymentVoucherService.CreateObject((int)retailPurchaseInvoice.CashBankId.GetValueOrDefault(), retailPurchaseInvoice.ContactId, DateTime.Now, retailPurchaseInvoice.Total,
                                                                            retailPurchaseInvoice.IsGBCH, (DateTime)retailPurchaseInvoice.DueDate.GetValueOrDefault(), retailPurchaseInvoice.IsBank, _paymentVoucherDetailService,
                                                                            _payableService, _contactService, _cashBankService);
                PaymentVoucherDetail paymentVoucherDetail = _paymentVoucherDetailService.CreateObject(paymentVoucher.Id, payable.Id, (decimal)retailPurchaseInvoice.AmountPaid, 
                                                                            "Automatic Payment", _paymentVoucherService, _cashBankService, _payableService);
                retailPurchaseInvoice = _repository.PaidObject(retailPurchaseInvoice);
                _paymentVoucherService.ConfirmObject(paymentVoucher, (DateTime)retailPurchaseInvoice.ConfirmationDate.GetValueOrDefault(), _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice UnpaidObject(RetailPurchaseInvoice retailPurchaseInvoice, IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService)
        {
            if (_validator.ValidUnpaidObject(retailPurchaseInvoice))
            {
                Payable payable = _payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, retailPurchaseInvoice.Id);
                IList<PaymentVoucher> paymentVouchers = _paymentVoucherService.GetObjectsByCashBankId((int)retailPurchaseInvoice.CashBankId.GetValueOrDefault());
                foreach (var paymentVoucher in paymentVouchers)
                {
                    if (paymentVoucher.ContactId == retailPurchaseInvoice.ContactId)
                    {
                        paymentVoucher.Errors = new Dictionary<string, string>();
                        _paymentVoucherService.UnconfirmObject(paymentVoucher, _paymentVoucherDetailService, _cashBankService, _payableService, _cashMutationService);

                        IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPaymentVoucherId(paymentVoucher.Id);
                        foreach (var paymentVoucherDetail in paymentVoucherDetails)
                        {
                            _paymentVoucherDetailService.SoftDeleteObject(paymentVoucherDetail);
                        }
                        _paymentVoucherService.SoftDeleteObject(paymentVoucher, _paymentVoucherDetailService);
                    }
                }
                retailPurchaseInvoice.AmountPaid = 0;
                retailPurchaseInvoice.IsFullPayment = false;
                retailPurchaseInvoice = _repository.UnpaidObject(retailPurchaseInvoice);
            }
            return retailPurchaseInvoice;
        }

        public RetailPurchaseInvoice SoftDeleteObject(RetailPurchaseInvoice retailPurchaseInvoice, IRetailPurchaseInvoiceDetailService _retailPurchaseInvoiceDetailService)
        {
            return (retailPurchaseInvoice = _validator.ValidDeleteObject(retailPurchaseInvoice, _retailPurchaseInvoiceDetailService) ?
                    _repository.SoftDeleteObject(retailPurchaseInvoice) : retailPurchaseInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
