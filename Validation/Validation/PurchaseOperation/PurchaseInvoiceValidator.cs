using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class PurchaseInvoiceValidator : IPurchaseInvoiceValidator
    {
        public PurchaseInvoice VHasPurchaseReceival(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService)
        {
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseInvoice.PurchaseReceivalId);
            if (purchaseReceival == null)
            {
                purchaseInvoice.Errors.Add("PurchaseReceivalId", "Tidak terasosiasi dengan Purchase Receival");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasNoPurchaseInvoiceDetails(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetailService.GetObjectsByPurchaseInvoiceId(purchaseInvoice.Id);
            if (details.Any())
            {
                purchaseInvoice.Errors.Add("Generic", "Tidak boleh memiliki Purchase Invoice Details");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasPurchaseInvoiceDetails(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetailService.GetObjectsByPurchaseInvoiceId(purchaseInvoice.Id);
            if (!details.Any())
            {
                purchaseInvoice.Errors.Add("Generic", "Tidak memiliki Purchase Invoice Details");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VPurchaseReceivalHasNotBeenInvoiceCompleted(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService)
        {
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseInvoice.PurchaseReceivalId);
            if (purchaseReceival.IsInvoiceCompleted)
            {
                purchaseInvoice.Errors.Add("Generic", "Tidak boleh memilih Purchase Receival dengan invoice yang sudah terbayar");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasInvoiceDate(PurchaseInvoice purchaseInvoice)
        {
            if (purchaseInvoice.InvoiceDate == null)
            {
                purchaseInvoice.Errors.Add("InvoiceDate", "Tidak boleh kosong");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasDueDate(PurchaseInvoice purchaseInvoice)
        {
            if (purchaseInvoice.DueDate == null)
            {
                purchaseInvoice.Errors.Add("DueDate", "Tidak boleh kosong");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasDiscountBetweenZeroAndHundred(PurchaseInvoice purchaseInvoice)
        {
            if (purchaseInvoice.Discount < 0 || purchaseInvoice.Discount > 100)
            {
                purchaseInvoice.Errors.Add("Discount", "Harus antara 0 dan 100");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasPaymentVoucherDetails(PurchaseInvoice purchaseInvoice, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            Payable payable = _payableService.GetObjectBySource(Constant.PayableSource.PurchaseInvoice, purchaseInvoice.Id);
            IList<PaymentVoucherDetail> pvdetails = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id);
            if (pvdetails.Any())
            {
                purchaseInvoice.Errors.Add("Generic", "Tidak boleh sudah ada proses pembayaran");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasBeenConfirmed(PurchaseInvoice purchaseInvoice)
        {
            if (!purchaseInvoice.IsConfirmed)
            {
                purchaseInvoice.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasNotBeenConfirmed(PurchaseInvoice purchaseInvoice)
        {
            if (purchaseInvoice.IsConfirmed)
            {
                purchaseInvoice.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasNotBeenDeleted(PurchaseInvoice purchaseInvoice)
        {
            if (purchaseInvoice.IsDeleted)
            {
                purchaseInvoice.Errors.Add("Generic", "Sudah dihapus");
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VAllPurchaseInvoiceDetailsAreConfirmable(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetailService.GetObjectsByPurchaseInvoiceId(purchaseInvoice.Id);
            foreach (var detail in details)
            {
                detail.ConfirmationDate = purchaseInvoice.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                _purchaseInvoiceDetailService.GetValidator().VConfirmObject(detail, _purchaseInvoiceDetailService, _purchaseReceivalDetailService);
                foreach (var error in detail.Errors)
                {
                    purchaseInvoice.Errors.Add("Generic", error.Value);
                }
                if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VAllPurchaseInvoiceDetailsAreUnconfirmable(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetailService.GetObjectsByPurchaseInvoiceId(purchaseInvoice.Id);
            foreach (var detail in details)
            {
                if (!_purchaseInvoiceDetailService.GetValidator().ValidUnconfirmObject(detail))
                {
                    foreach (var error in detail.Errors)
                    {
                        purchaseInvoice.Errors.Add("Generic", error.Value);
                    }
                    if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
                }
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice VPayableHasNoOtherAssociation(PurchaseInvoice purchaseInvoice, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService)
        {
            Payable payable = _payableService.GetObjectBySource(Constant.PayableSource.PurchaseInvoice, purchaseInvoice.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails = _paymentVoucherDetailService.GetObjectsByPayableId(payable.Id);
            if (paymentVoucherDetails.Any())
            {
                purchaseInvoice.Errors.Add("Generic", "Payable memiliki asosiasi dengan payment voucher detail");
                return purchaseInvoice;
            }

            /*
            IList<PurchaseAllowanceAllocationDetail> purchaseAllowanceAllocationDetails = _purchaseAllowanceAllocationDetailService.GetObjectsByPayableId(payable.Id);
            if (purchaseAllowanceAllocationDetails.Any())
            {
                purchaseInvoice.Errors.Add("Generic", "Payable memiliki asosiasi dengan purchase allowance allocation detail");
            }
             */
            return purchaseInvoice;
        }

        public PurchaseInvoice VCreateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService)
        {
            VHasPurchaseReceival(purchaseInvoice, _purchaseReceivalService);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VPurchaseReceivalHasNotBeenInvoiceCompleted(purchaseInvoice, _purchaseReceivalService);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasInvoiceDate(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasDueDate(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasDiscountBetweenZeroAndHundred(purchaseInvoice);
            return purchaseInvoice;
        }

        public PurchaseInvoice VUpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            VCreateObject(purchaseInvoice, _purchaseReceivalService);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNoPurchaseInvoiceDetails(purchaseInvoice, _purchaseInvoiceDetailService);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNotBeenConfirmed(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNotBeenDeleted(purchaseInvoice);
            return purchaseInvoice;
        }

        public PurchaseInvoice VDeleteObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            VHasNotBeenConfirmed(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNotBeenDeleted(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNoPurchaseInvoiceDetails(purchaseInvoice, _purchaseInvoiceDetailService);
            return purchaseInvoice;
        }

        public PurchaseInvoice VHasConfirmationDate(PurchaseInvoice obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public PurchaseInvoice VConfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                              IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            VHasConfirmationDate(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VPurchaseReceivalHasNotBeenInvoiceCompleted(purchaseInvoice, _purchaseReceivalService);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNotBeenDeleted(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNotBeenConfirmed(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasPurchaseInvoiceDetails(purchaseInvoice, _purchaseInvoiceDetailService);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VAllPurchaseInvoiceDetailsAreConfirmable(purchaseInvoice, _purchaseInvoiceDetailService, _purchaseReceivalDetailService);
            return purchaseInvoice;
        }

        public PurchaseInvoice VUnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                                IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            VHasBeenConfirmed(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VHasNotBeenDeleted(purchaseInvoice);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VAllPurchaseInvoiceDetailsAreUnconfirmable(purchaseInvoice, _purchaseInvoiceDetailService, _paymentVoucherDetailService, _payableService);
            if (!isValid(purchaseInvoice)) { return purchaseInvoice; }
            VPayableHasNoOtherAssociation(purchaseInvoice, _payableService, _paymentVoucherDetailService); // _purchaseAllowanceAllocationDetailService
            return purchaseInvoice;
        }

        public bool ValidCreateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService)
        {
            VCreateObject(purchaseInvoice, _purchaseReceivalService);
            return isValid(purchaseInvoice);
        }

        public bool ValidUpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            purchaseInvoice.Errors.Clear();
            VUpdateObject(purchaseInvoice, _purchaseReceivalService, _purchaseInvoiceDetailService);
            return isValid(purchaseInvoice);
        }

        public bool ValidDeleteObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            purchaseInvoice.Errors.Clear();
            VDeleteObject(purchaseInvoice, _purchaseInvoiceDetailService);
            return isValid(purchaseInvoice);
        }

        public bool ValidConfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                       IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            purchaseInvoice.Errors.Clear();
            VConfirmObject(purchaseInvoice, _purchaseInvoiceDetailService, _purchaseReceivalService, _purchaseReceivalDetailService);
            return isValid(purchaseInvoice);
        }

        public bool ValidUnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                         IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService)
        {
            purchaseInvoice.Errors.Clear();
            VUnconfirmObject(purchaseInvoice, _purchaseInvoiceDetailService, _paymentVoucherDetailService, _payableService);
            return isValid(purchaseInvoice);
        }

        public bool isValid(PurchaseInvoice obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PurchaseInvoice obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }
    }
}