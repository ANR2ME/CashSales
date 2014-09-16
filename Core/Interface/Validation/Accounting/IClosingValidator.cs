using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IClosingValidator
    {
        Closing VIsValidPeriod(Closing closing);
        Closing VIsValidYearPeriod(Closing closing);
        Closing VHasBeginningPeriod(Closing closing);
        Closing VHasEndDatePeriod(Closing closing);
        Closing VHasNotBeenClosed(Closing closing);
        Closing VHasBeenClosed(Closing closing);
        Closing VIsBackToBackToPreviousClosing(Closing closing, IClosingService _closingService);

        Closing VCreateObject(Closing closing, IClosingService _closingService);
        Closing VCloseObject(Closing closing, IClosingService _closingService);
        Closing VOpenObject(Closing closing);
        Closing VDeleteObject(Closing closing);
        bool ValidCreateObject(Closing closing, IClosingService _closingService);
        bool ValidCloseObject(Closing closing, IClosingService _closingService);
        bool ValidOpenObject(Closing closing);
        bool ValidDeleteObject(Closing closing);
        bool isValid(Closing closing);
        string PrintError(Closing closing);
    }
}
