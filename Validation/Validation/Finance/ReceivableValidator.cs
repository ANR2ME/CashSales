using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class ReceivableValidator : IReceivableValidator
    {
        public Receivable VReceivableSource(Receivable receivable)
        {
            if (!receivable.ReceivableSource.Equals(Core.Constants.Constant.ReceivableSource.CashSalesInvoice) &&
                !receivable.ReceivableSource.Equals(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice) &&
                !receivable.ReceivableSource.Equals(Core.Constants.Constant.ReceivableSource.SalesInvoice))
            {
                receivable.Errors.Add("Generic", "Harus merupakan bagian dari Constant.ReceivableSource");
            }
            return receivable;
        }

        public Receivable VDontHaveReceiptVoucherDetails(Receivable receivable, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            IList<ReceiptVoucherDetail> objs = _receiptVoucherDetailService.GetQueryableObjectsByReceivableId(receivable.Id).ToList();
            if (objs.Any())
            {
                receivable.Errors.Add("Generic", "Receivable Tidak boleh terasosiasi dengan ReceiptVoucherDetail");
            }
            return receivable;
        }

        public Receivable VHasValidAmount(Receivable receivable)
        {
            if (receivable.Amount < 0)
            {
                receivable.Errors.Add("Generic", "Receivable Amount Harus lebih besar atau sama dengan 0");
            }
            return receivable;
        }

        public Receivable VCreateObject(Receivable receivable, IReceivableService _receivableService)
        {
            //VHasValidAmount(receivable);
            return receivable;
        }

        public Receivable VUpdateObject(Receivable receivable, IReceivableService _receivableService)
        {
            VCreateObject(receivable, _receivableService);
            return receivable;
        }

        public Receivable VDeleteObject(Receivable receivable, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            VDontHaveReceiptVoucherDetails(receivable, _receiptVoucherDetailService);
            return receivable;
        }

        public bool ValidCreateObject(Receivable receivable, IReceivableService _receivableService)
        {
            VCreateObject(receivable, _receivableService);
            return isValid(receivable);
        }

        public bool ValidUpdateObject(Receivable receivable, IReceivableService _receivableService)
        {
            receivable.Errors.Clear();
            VUpdateObject(receivable, _receivableService);
            return isValid(receivable);
        }

        public bool ValidDeleteObject(Receivable receivable, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            receivable.Errors.Clear();
            VDeleteObject(receivable, _receiptVoucherDetailService);
            return isValid(receivable);
        }

        public bool isValid(Receivable obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Receivable obj)
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
