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

        public Receivable VCreateObject(Receivable receivable, IReceivableService _receivableService)
        {
            return receivable;
        }

        public Receivable VUpdateObject(Receivable receivable, IReceivableService _receivableService)
        {
            return receivable;
        }

        public Receivable VDeleteObject(Receivable receivable)
        {
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

        public bool ValidDeleteObject(Receivable receivable)
        {
            receivable.Errors.Clear();
            VDeleteObject(receivable);
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
