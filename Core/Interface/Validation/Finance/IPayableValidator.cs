using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Repository;

namespace Core.Interface.Validation
{
    public interface IPayableValidator
    {
        Payable VPayableSource(Payable payable);
        Payable VCreateObject(Payable payable, IPayableService _payableService);
        Payable VUpdateObject(Payable payable, IPayableService _payableService);
        Payable VDeleteObject(Payable payable, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool ValidCreateObject(Payable payable, IPayableService _payableService);
        bool ValidUpdateObject(Payable payable, IPayableService _payableService);
        bool ValidDeleteObject(Payable payable, IPaymentVoucherDetailService _paymentVoucherDetailService);
        bool isValid(Payable payable);
        string PrintError(Payable payable);
    }
}
