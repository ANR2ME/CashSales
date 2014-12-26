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
    public class ClosingValidator : IClosingValidator
    {

        public Closing VIsValidPeriod(Closing closing)
        {
            if (closing.Period < 1 || closing.Period > 12)
            {
                closing.Errors.Add("Period", "Harus antara 1 sampai dengan 12");
            }
            return closing;
        }

        public Closing VIsValidYearPeriod(Closing closing)
        {
            if (closing.YearPeriod > DateTime.Now.Year || closing.YearPeriod < DateTime.MinValue.Year)
            {
                closing.Errors.Add("YearPeriod", "Tidak boleh lebih besar dari tahun ini atau kurang dari " + DateTime.MinValue.Year);
            }
            return closing;
        }

        public Closing VHasBeginningPeriod(Closing closing)
        {
            if (closing.BeginningPeriod == null || closing.BeginningPeriod.Equals(DateTime.FromBinary(0)))
            {
                closing.Errors.Add("BeginningPeriod", "Tidak ada");
            }
            return closing;
        }

        public Closing VHasEndDatePeriod(Closing closing)
        {
            if (closing.EndDatePeriod == null || closing.EndDatePeriod.Equals(DateTime.FromBinary(0)))
            {
                closing.Errors.Add("EndDatePeriod", "Tidak ada");
            }
            return closing;
        }

        public Closing VHasBeenClosed(Closing closing)
        {
            if (!closing.IsClosed)
            {
                closing.Errors.Add("Generic", "Belum tutup buku");
            }
            return closing;
        }

        public Closing VHasNotBeenClosed(Closing closing)
        {
            if (closing.IsClosed)
            {
                closing.Errors.Add("Generic", "Sudah tutup buku");
            }
            return closing;
        }

        public Closing VIsBackToBackToPreviousClosing(Closing closing, IClosingService _closingService)
        {
            IList<Closing> closings = _closingService.GetQueryable().Where(x => x.Id != closing.Id).OrderByDescending(x => x.Id).ToList();
            if (closings.Any())
            {
                if (closings.FirstOrDefault().EndDatePeriod.AddDays(1).Date != closing.BeginningPeriod)
                {
                    closing.Errors.Add("Generic", "Tanggal Beginning Date harus " + closings.FirstOrDefault().EndDatePeriod.AddDays(1).ToLongDateString());
                }
            }
            return closing;

            /*
            int PreviousMonth = closing.Period == 1 ? 12 : closing.Period - 1;
            int PreviousYear = closing.Period == 1 ? closing.YearPeriod - 1 : closing.YearPeriod;
            Closing previousClosing = _closingService.GetObjectByPeriodAndYear(PreviousMonth, PreviousYear);
            */
        }

        public Closing VCreateObject(Closing closing, IClosingService _closingService)
        {
            VHasBeginningPeriod(closing);
            if (!isValid(closing)) { return closing; }
            VHasEndDatePeriod(closing);
            if (!isValid(closing)) { return closing; }
            VIsValidPeriod(closing);
            if (!isValid(closing)) { return closing; }
            VIsValidYearPeriod(closing);
            if (!isValid(closing)) { return closing; }
            VIsBackToBackToPreviousClosing(closing, _closingService);
            if (!isValid(closing)) { return closing; }
            VHasNotBeenClosed(closing);
            return closing;
        }

        public Closing VCloseObject(Closing closing, IClosingService _closingService)
        {
            VCreateObject(closing, _closingService);
            return closing;
        }

        public Closing VOpenObject(Closing closing)
        {
            VHasBeenClosed(closing);
            return closing;
        }

        public Closing VDeleteObject(Closing closing)
        {
            VHasNotBeenClosed(closing);
            return closing;
        }

        public bool ValidCreateObject(Closing closing, IClosingService _closingService)
        {
            VCreateObject(closing, _closingService);
            return isValid(closing);
        }

        public bool ValidCloseObject(Closing closing, IClosingService _closingService)
        {
            closing.Errors.Clear();
            VCloseObject(closing, _closingService);
            return isValid(closing);
        }

        public bool ValidOpenObject(Closing closing)
        {
            closing.Errors.Clear();
            VOpenObject(closing);
            return isValid(closing);
        }

        public bool ValidDeleteObject(Closing closing)
        {
            closing.Errors.Clear();
            VDeleteObject(closing);
            return isValid(closing);
        }

        public bool isValid(Closing obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Closing obj)
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
