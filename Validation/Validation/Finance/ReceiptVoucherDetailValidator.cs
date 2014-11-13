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
    public class ReceiptVoucherDetailValidator : IReceiptVoucherDetailValidator
    {
        public ReceiptVoucherDetail VHasReceiptVoucher(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService)
        {
            ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
            if (receiptVoucher == null)
            {
                receiptVoucherDetail.Errors.Add("ReceiptVoucherId", "Tidak boleh tidak ada");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VHasReceivable(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);
            if (receivable == null)
            {
                receiptVoucherDetail.Errors.Add("ReceivableId", "Tidak boleh tidak ada");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VHasNotBeenConfirmed(ReceiptVoucherDetail receiptVoucherDetail)
        {
            if (receiptVoucherDetail.IsConfirmed)
            {
                receiptVoucherDetail.Errors.Add("Generic", "Sudah diKonfirmasi");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VHasBeenConfirmed(ReceiptVoucherDetail receiptVoucherDetail)
        {
            if (!receiptVoucherDetail.IsConfirmed)
            {
                receiptVoucherDetail.Errors.Add("Generic", "Belum diKonfirmasi");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VHasNotBeenDeleted(ReceiptVoucherDetail receiptVoucherDetail)
        {
            if (receiptVoucherDetail.IsDeleted)
            {
                receiptVoucherDetail.Errors.Add("Generic", "Sudah diDelete");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VParentHasNotBeenConfirmed(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService)
        {
            ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
            if (receiptVoucher.IsConfirmed)
            {
                receiptVoucherDetail.Errors.Add("Generic", "ReceiptVoucher tidak boleh terkonfirmasi");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VReceivableHasNotBeenCompleted(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService)
        {
            Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);
            if (receivable.IsCompleted)
            {
                receiptVoucherDetail.Errors.Add("Generic", "Receivable sudah Complete");
            }
            return receiptVoucherDetail;
        }
        
        public ReceiptVoucherDetail VNonNegativeAmount(ReceiptVoucherDetail receiptVoucherDetail)
        {
            if (receiptVoucherDetail.Amount < 0)
            {
                receiptVoucherDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VAmountLessOrEqualReceivable(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);
            //decimal totalamount = _receiptVoucherDetailService.CalcTotalAmountByReceivable(receiptVoucherDetail.ReceivableId);
            decimal totalamount = _receiptVoucherDetailService.GetQueryableObjectsByReceivableId(receiptVoucherDetail.ReceivableId).Where(x => !x.IsDeleted && x.Id != receiptVoucherDetail.Id).Sum(x => (decimal?)x.Amount) ?? 0;
            if (totalamount + receiptVoucherDetail.Amount > receivable.Amount - receivable.AllowanceAmount)
            {
                receiptVoucherDetail.Errors.Add("Generic", "Total Amount seluruh ReceiptVoucherDetails Tidak boleh melebihi Receivable Amount (minus Allowance)");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VTotalAmountLessOrEqualReceivable(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);
            decimal totalamount = _receiptVoucherDetailService.CalcTotalAmountByReceivable(receiptVoucherDetail.ReceivableId);
            if (totalamount > receivable.Amount - receivable.AllowanceAmount)
            {
                receiptVoucherDetail.Errors.Add("Generic", "Total Amount seluruh ReceiptVoucherDetails Tidak boleh melebihi Receivable Amount (minus Allowance)");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VAmountLessOrEqualReceiptVoucher(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            ReceiptVoucher receiptVoucher = _receiptVoucherService.GetObjectById(receiptVoucherDetail.ReceiptVoucherId);
            //decimal totalamount = _receiptVoucherDetailService.CalcTotalAmountByReceiptVoucher(receiptVoucherDetail.ReceiptVoucherId);
            decimal totalamount = _receiptVoucherDetailService.GetQueryableObjectsByReceiptVoucherId(receiptVoucherDetail.ReceiptVoucherId).Where(x => !x.IsDeleted && x.Id != receiptVoucherDetail.Id).Sum(x => (decimal?)x.Amount) ?? 0;
            if (totalamount + receiptVoucherDetail.Amount > receiptVoucher.TotalAmount)
            {
                receiptVoucherDetail.Errors.Add("Generic", "Total ReceiptVoucherDetails Amount Tidak boleh lebih dari TotalAmount ReceiptVoucher");
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VUniqueReceivableId(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                    IReceivableService _receivableService)
        {
            IList<ReceiptVoucherDetail> receiptVoucherDetails = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucherDetail.ReceiptVoucherId);
            Receivable receivable = _receivableService.GetObjectById(receiptVoucherDetail.ReceivableId);
            foreach (var detail in receiptVoucherDetails)
            {
                if (detail.ReceivableId == receiptVoucherDetail.ReceivableId && detail.Id != receiptVoucherDetail.Id)
                {
                    receiptVoucherDetail.Errors.Add("Generic", "ReceivableId harus unique dibandingkan receipt voucher details di dalam satu receipt voucher");
                    return receiptVoucherDetail;
                }
            }
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VCreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService,
                                                  IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VHasReceiptVoucher(receiptVoucherDetail, _receiptVoucherService);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VHasNotBeenConfirmed(receiptVoucherDetail);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VHasNotBeenDeleted(receiptVoucherDetail);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VHasReceivable(receiptVoucherDetail, _receivableService);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VReceivableHasNotBeenCompleted(receiptVoucherDetail, _receivableService);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VParentHasNotBeenConfirmed(receiptVoucherDetail, _receiptVoucherService);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VNonNegativeAmount(receiptVoucherDetail);
            //if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            //VAmountLessOrEqualReceiptVoucher(receiptVoucherDetail, _receiptVoucherService, _receiptVoucherDetailService);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VAmountLessOrEqualReceivable(receiptVoucherDetail, _receivableService, _receiptVoucherDetailService);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VUniqueReceivableId(receiptVoucherDetail, _receiptVoucherDetailService, _receivableService);
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VUpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService,
                                                  IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VHasNotBeenConfirmed(receiptVoucherDetail);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VCreateObject(receiptVoucherDetail, _receiptVoucherService, _receiptVoucherDetailService, _cashBankService, _receivableService);
            return receiptVoucherDetail;    
        }

        public ReceiptVoucherDetail VDeleteObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            VHasNotBeenConfirmed(receiptVoucherDetail);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VHasNotBeenDeleted(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VHasConfirmationDate(ReceiptVoucherDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public ReceiptVoucherDetail VConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            VHasNotBeenConfirmed(receiptVoucherDetail);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VHasConfirmationDate(receiptVoucherDetail);
            if (!isValid(receiptVoucherDetail)) { return receiptVoucherDetail; }
            VTotalAmountLessOrEqualReceivable(receiptVoucherDetail, _receivableService, _receiptVoucherDetailService);
            return receiptVoucherDetail;
        }

        public ReceiptVoucherDetail VUnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            VHasBeenConfirmed(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public bool ValidCreateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VCreateObject(receiptVoucherDetail, _receiptVoucherService, _receiptVoucherDetailService, _cashBankService, _receivableService);
            return isValid(receiptVoucherDetail);
        }

        public bool ValidUpdateObject(ReceiptVoucherDetail receiptVoucherDetail, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IReceivableService _receivableService)
        {
            VUpdateObject(receiptVoucherDetail, _receiptVoucherService, _receiptVoucherDetailService, _cashBankService, _receivableService);
            return isValid(receiptVoucherDetail);
        }

        public bool ValidDeleteObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            VDeleteObject(receiptVoucherDetail);
            return isValid(receiptVoucherDetail);
        }

        public bool ValidConfirmObject(ReceiptVoucherDetail receiptVoucherDetail, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            VConfirmObject(receiptVoucherDetail, _receivableService, _receiptVoucherDetailService);
            return isValid(receiptVoucherDetail);
        }

        public bool ValidUnconfirmObject(ReceiptVoucherDetail receiptVoucherDetail)
        {
            VUnconfirmObject(receiptVoucherDetail);
            return isValid(receiptVoucherDetail);
        }

        public bool isValid(ReceiptVoucherDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ReceiptVoucherDetail obj)
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