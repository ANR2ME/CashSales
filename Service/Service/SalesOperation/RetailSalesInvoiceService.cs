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
    public class RetailSalesInvoiceService : IRetailSalesInvoiceService
    {
        private IRetailSalesInvoiceRepository _repository;
        private IRetailSalesInvoiceValidator _validator;
        public RetailSalesInvoiceService(IRetailSalesInvoiceRepository _retailSalesInvoiceRepository, IRetailSalesInvoiceValidator _retailSalesInvoiceValidator)
        {
            _repository = _retailSalesInvoiceRepository;
            _validator = _retailSalesInvoiceValidator;
        }

        public IRetailSalesInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public IRetailSalesInvoiceRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<RetailSalesInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RetailSalesInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public RetailSalesInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RetailSalesInvoice CreateObject(RetailSalesInvoice retailSalesInvoice, IWarehouseService _warehouseService)
        {
            retailSalesInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(retailSalesInvoice, _warehouseService) ? _repository.CreateObject(retailSalesInvoice) : retailSalesInvoice);
        }

        public RetailSalesInvoice UpdateObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            return (retailSalesInvoice = _validator.ValidUpdateObject(retailSalesInvoice, _retailSalesInvoiceDetailService) ? _repository.UpdateObject(retailSalesInvoice) : retailSalesInvoice);
        }

        public RetailSalesInvoice ConfirmObject(RetailSalesInvoice retailSalesInvoice, DateTime ConfirmationDate, int ContactId, 
                                                IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService, IContactService _contactService,
                                                IPriceMutationService _priceMutationService, IReceivableService _receivableService, 
                                                IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService,
                                                IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            retailSalesInvoice.ContactId = ContactId;
            retailSalesInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(retailSalesInvoice, _retailSalesInvoiceDetailService, _retailSalesInvoiceService, _warehouseItemService, _contactService))
            {
                IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
                retailSalesInvoice.Total = 0;
                retailSalesInvoice.CoGS = 0;
                foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
                {
                    retailSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    _retailSalesInvoiceDetailService.ConfirmObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _warehouseItemService,
                                                                   _warehouseService, _itemService, _barringService, _stockMutationService);
                    retailSalesInvoice.Total += retailSalesInvoiceDetail.Amount;
                    retailSalesInvoice.CoGS += retailSalesInvoiceDetail.CoGS;
                }
                // Tax dihitung setelah Discount
                retailSalesInvoice.Total = (retailSalesInvoice.Total * ((100 - retailSalesInvoice.Discount) / 100) * ((100 + retailSalesInvoice.Tax) / 100));
                Receivable receivable = _receivableService.CreateObject(retailSalesInvoice.ContactId, Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id, retailSalesInvoice.Total, (DateTime)retailSalesInvoice.DueDate.GetValueOrDefault());
                retailSalesInvoice = _repository.ConfirmObject(retailSalesInvoice);
            }
            else
            {
                retailSalesInvoice.ConfirmationDate = null;
                //retailSalesInvoice.ContactId = 0; //null;
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice UnconfirmObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService,
                                                  IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                  IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IItemService _itemService, 
                                                  IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(retailSalesInvoice, _retailSalesInvoiceDetailService, _receivableService, _receiptVoucherDetailService))
            {
                retailSalesInvoice = _repository.UnconfirmObject(retailSalesInvoice);
                IList<RetailSalesInvoiceDetail> retailSalesInvoiceDetails = _retailSalesInvoiceDetailService.GetObjectsByRetailSalesInvoiceId(retailSalesInvoice.Id);
                foreach (var retailSalesInvoiceDetail in retailSalesInvoiceDetails)
                {
                    retailSalesInvoiceDetail.Errors = new Dictionary<string, string>();
                    _retailSalesInvoiceDetailService.UnconfirmObject(retailSalesInvoiceDetail, _warehouseItemService, _warehouseService, _itemService, _barringService, _stockMutationService);
                }
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id);
                _receivableService.SoftDeleteObject(receivable);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice PaidObject(RetailSalesInvoice retailSalesInvoice, decimal AmountPaid, ICashBankService _cashBankService, IReceivableService _receivableService, 
                                             IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, 
                                             IContactService _contactService, ICashMutationService _cashMutationService)
        {
            retailSalesInvoice.AmountPaid = AmountPaid;
            if (_validator.ValidPaidObject(retailSalesInvoice, _cashBankService, _receiptVoucherService))
            {
                CashBank cashBank = _cashBankService.GetObjectById((int)retailSalesInvoice.CashBankId.GetValueOrDefault());
                retailSalesInvoice.IsBank = cashBank.IsBank;
                
                if (!retailSalesInvoice.IsGBCH)
                {
                    retailSalesInvoice.GBCH_No = null;
                    retailSalesInvoice.Description = null;
                }
                if (retailSalesInvoice.AmountPaid == retailSalesInvoice.Total)
                {
                    retailSalesInvoice.IsFullPayment = true;
                }
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id);
                ReceiptVoucher receiptVoucher = _receiptVoucherService.CreateObject((int)retailSalesInvoice.CashBankId.GetValueOrDefault(), retailSalesInvoice.ContactId, DateTime.Now, retailSalesInvoice.Total,
                                                                            retailSalesInvoice.IsGBCH, (DateTime)retailSalesInvoice.DueDate.GetValueOrDefault(), retailSalesInvoice.IsBank, _receiptVoucherDetailService,
                                                                            _receivableService, _contactService, _cashBankService);
                ReceiptVoucherDetail receiptVoucherDetail = _receiptVoucherDetailService.CreateObject(receiptVoucher.Id, receivable.Id, (decimal)retailSalesInvoice.AmountPaid, 
                                                                            "Automatic Payment", _receiptVoucherService, _cashBankService, _receivableService);
                retailSalesInvoice = _repository.PaidObject(retailSalesInvoice);
                _receiptVoucherService.ConfirmObject(receiptVoucher, (DateTime)retailSalesInvoice.ConfirmationDate, _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice UnpaidObject(RetailSalesInvoice retailSalesInvoice, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                               ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService)
        {
            if (_validator.ValidUnpaidObject(retailSalesInvoice))
            {
                Receivable receivable = _receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, retailSalesInvoice.Id);
                IList<ReceiptVoucher> receiptVouchers = _receiptVoucherService.GetObjectsByCashBankId((int)retailSalesInvoice.CashBankId.GetValueOrDefault());
                foreach (var receiptVoucher in receiptVouchers)
                {
                    if (receiptVoucher.ContactId == retailSalesInvoice.ContactId)
                    {
                        receiptVoucher.Errors = new Dictionary<string, string>();
                        _receiptVoucherService.UnconfirmObject(receiptVoucher, _receiptVoucherDetailService, _cashBankService, _receivableService, _cashMutationService);

                        IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
                        foreach (var receiptVoucherDetail in receiptVoucherDetails)
                        {
                            _receiptVoucherDetailService.SoftDeleteObject(receiptVoucherDetail);
                        }
                        _receiptVoucherService.SoftDeleteObject(receiptVoucher, _receiptVoucherDetailService);
                    }
                }
                retailSalesInvoice.AmountPaid = 0;
                retailSalesInvoice.IsFullPayment = false;
                retailSalesInvoice = _repository.UnpaidObject(retailSalesInvoice);
            }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice SoftDeleteObject(RetailSalesInvoice retailSalesInvoice, IRetailSalesInvoiceDetailService _retailSalesInvoiceDetailService)
        {
            return (retailSalesInvoice = _validator.ValidDeleteObject(retailSalesInvoice, _retailSalesInvoiceDetailService) ?
                    _repository.SoftDeleteObject(retailSalesInvoice) : retailSalesInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
