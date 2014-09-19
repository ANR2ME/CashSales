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
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private ISalesInvoiceRepository _repository;
        private ISalesInvoiceValidator _validator;

        public SalesInvoiceService(ISalesInvoiceRepository _salesInvoiceRepository, ISalesInvoiceValidator _salesInvoiceValidator)
        {
            _repository = _salesInvoiceRepository;
            _validator = _salesInvoiceValidator;
        }

        public ISalesInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public SalesInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesInvoice> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return _repository.GetObjectsByDeliveryOrderId(deliveryOrderId);
        }

        public SalesInvoice CreateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService)
        {
            salesInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesInvoice, _deliveryOrderService) ? _repository.CreateObject(salesInvoice) : salesInvoice);
        }

        public SalesInvoice CreateObject(int deliveryOrderId, string description, int discount, bool isTaxable, DateTime InvoiceDate, DateTime DueDate, IDeliveryOrderService _deliveryOrderService)
        {
            SalesInvoice salesInvoice = new SalesInvoice
            {
                DeliveryOrderId = deliveryOrderId,
                Description = description,
                Discount = discount,
                IsTaxable = isTaxable,
                InvoiceDate = InvoiceDate,
                DueDate = DueDate
            };
            return this.CreateObject(salesInvoice, _deliveryOrderService);
        }

        public SalesInvoice UpdateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            return (_validator.ValidUpdateObject(salesInvoice, _deliveryOrderService, _salesInvoiceDetailService) ? _repository.UpdateObject(salesInvoice) : salesInvoice);
        }

        public SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            return (_validator.ValidDeleteObject(salesInvoice, _salesInvoiceDetailService) ? _repository.SoftDeleteObject(salesInvoice) : salesInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesInvoice ConfirmObject(SalesInvoice salesInvoice, DateTime ConfirmationDate, ISalesInvoiceDetailService _salesInvoiceDetailService, ISalesOrderService _salesOrderService,
                                             IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService, IReceivableService _receivableService)
        {
            salesInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesInvoice, _salesInvoiceDetailService, _deliveryOrderService, _deliveryOrderDetailService))
            {
                // confirm details
                // add all amount into amountreceivable
                IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesInvoiceDetailService.ConfirmObject(detail,ConfirmationDate, _deliveryOrderDetailService);
                }
                salesInvoice = CalculateAmountReceivable(salesInvoice, _salesInvoiceDetailService);

                // confirm object
                // create receivable
                salesInvoice = _repository.ConfirmObject(salesInvoice);
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(salesInvoice.DeliveryOrderId);
                _deliveryOrderService.CheckAndSetInvoiceComplete(deliveryOrder, _deliveryOrderDetailService);
                SalesOrder salesOrder = _salesOrderService.GetObjectById(deliveryOrder.SalesOrderId);
                Receivable receivable = _receivableService.CreateObject(salesOrder.ContactId, Constant.ReceivableSource.SalesInvoice, salesInvoice.Id, salesInvoice.Code, salesInvoice.AmountReceivable, salesInvoice.DueDate);
            }
            return salesInvoice;
        }

        public SalesInvoice UnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService,
                                               IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                               IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService)
        {
            if (_validator.ValidUnconfirmObject(salesInvoice, _salesInvoiceDetailService, _receiptVoucherDetailService, _receivableService))
            {
                IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesInvoiceDetailService.UnconfirmObject(detail, _deliveryOrderService, _deliveryOrderDetailService);
                }
                _repository.UnconfirmObject(salesInvoice);
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(salesInvoice.DeliveryOrderId);
                _deliveryOrderService.UnsetInvoiceComplete(deliveryOrder);
                Receivable receivable = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice.Id);
                _receivableService.SoftDeleteObject(receivable);
            }
            return salesInvoice;
        }

        public SalesInvoice CalculateAmountReceivable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
            decimal AmountReceivable = 0;
            foreach (var detail in details)
            {
                AmountReceivable += detail.Amount;
            }
            decimal Discount = salesInvoice.Discount / 100 * AmountReceivable;
            decimal TaxableAmount = AmountReceivable - Discount;
            salesInvoice.AmountReceivable = salesInvoice.IsTaxable ? TaxableAmount * (decimal)1.1 : TaxableAmount; // 10% Tax
            _repository.Update(salesInvoice);
            return salesInvoice;
        }
    }
}