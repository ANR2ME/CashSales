using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IReceiptVoucherValidator
    {
        ReceiptVoucher VHasContact(ReceiptVoucher receiptVoucher, IContactService _contactService);
        ReceiptVoucher VHasCashBank(ReceiptVoucher receiptVoucher, ICashBankService _cashBankService);
        ReceiptVoucher VHasReceiptDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VIfGBCHThenIsBank(ReceiptVoucher receiptVoucher, ICashBankService _cashBankService);
        ReceiptVoucher VIfGBCHThenHasDueDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNoReceiptVoucherDetail(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VHasReceiptVoucherDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VTotalAmountIsNotZero(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VHasNotBeenDeleted(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasBeenConfirmed(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNotBeenConfirmed(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VAllReceiptVoucherDetailsAreConfirmable(ReceiptVoucher receiptVoucher, IReceiptVoucherService _paymetnVoucherService,
                                                               IReceiptVoucherDetailService receiptVoucherDetailService, ICashBankService _cashBankService,
                                                               IReceivableService _receivableService);
        //ReceiptVoucher VCashBankHasMoreAmountReceiptVoucherDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService,
        //                                                           ICashBankService _cashBankService);
        ReceiptVoucher VHasBeenReconciled(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasNotBeenReconciled(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VHasReconciliationDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VGeneralLedgerPostingHasNotBeenClosed(ReceiptVoucher receiptVoucher, IClosingService _closingService, int CaseConfirmUnconfirm);
        ReceiptVoucher VCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher VUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                     IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        ReceiptVoucher VDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        ReceiptVoucher VHasConfirmationDate(ReceiptVoucher receiptVoucher);
        ReceiptVoucher VConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                       IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService, IClosingService _closingService);
        ReceiptVoucher VUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, 
                                       IClosingService _closingService);
        ReceiptVoucher VReconcileObject(ReceiptVoucher receiptVoucher, IClosingService _closingService);
        ReceiptVoucher VUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, 
                                       IClosingService _closingService);
        bool ValidCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                               IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService);
        bool ValidDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService);
        bool ValidConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                IReceivableService _receivableService, IClosingService _closingService);
        bool ValidUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, 
                                IClosingService _closingService);
        bool ValidReconcileObject(ReceiptVoucher receiptVoucher, IClosingService _closingService);
        bool ValidUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, 
                                IClosingService _closingService);
        bool isValid(ReceiptVoucher receiptVoucher);
        string PrintError(ReceiptVoucher receiptVoucher);
    }
}
