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
    public class ReceiptVoucherValidator : IReceiptVoucherValidator
    {
        public ReceiptVoucher VHasContact(ReceiptVoucher receiptVoucher, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(receiptVoucher.ContactId);
            if (contact == null)
            {
                receiptVoucher.Errors.Add("ContactId", "Tidak boleh tidak ada");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasCashBank(ReceiptVoucher receiptVoucher, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(receiptVoucher.CashBankId);
            if (cashBank == null)
            {
                receiptVoucher.Errors.Add("CashBankId", "Tidak boleh tidak ada");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasReceiptDate(ReceiptVoucher receiptVoucher)
        {
            if (receiptVoucher.ReceiptDate == null)
            {
                receiptVoucher.Errors.Add("ReceiptDate", "Tidak boleh kosong");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VIfGBCHThenIsBank(ReceiptVoucher receiptVoucher, ICashBankService _cashBankService)
        {
            if (receiptVoucher.IsGBCH)
            {
                CashBank cashBank = _cashBankService.GetObjectById(receiptVoucher.CashBankId);
                if (!cashBank.IsBank)
                {
                    receiptVoucher.Errors.Add("Generic", "Jika GBCH CashBank harus IsBank");
                }
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VIfGBCHThenHasDueDate(ReceiptVoucher receiptVoucher)
        {
            if (receiptVoucher.IsGBCH)
            {
                if (receiptVoucher.DueDate == null)
                {
                    receiptVoucher.Errors.Add("DueDate", "Jika GBCH maka DueDate harus diisi");
                }
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasReceiptVoucherDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            IList<ReceiptVoucherDetail> details = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
            if (!details.Any())
            {
                receiptVoucher.Errors.Add("Generic", "Harus memiliki receipt voucher details");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasNoReceiptVoucherDetail(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            IList<ReceiptVoucherDetail> details = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
            if (details.Any())
            {
                receiptVoucher.Errors.Add("Generic", "Tidak boleh ada receipt voucher details");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VTotalAmountIsNotZero(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            decimal totalamount = 0;
            IList<ReceiptVoucherDetail> details = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
            foreach (var detail in details)
            {
                totalamount += detail.Amount;
            }
            if (totalamount == 0)
            {
                receiptVoucher.Errors.Add("Generic", "Total Amount tidak boleh 0");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasNotBeenDeleted(ReceiptVoucher receiptVoucher)
        {
            if (receiptVoucher.IsDeleted)
            {
                receiptVoucher.Errors.Add("Generic", "Tidak boleh sudah di deleted");
            }
            return receiptVoucher;
        }
        
        public ReceiptVoucher VHasNotBeenConfirmed(ReceiptVoucher receiptVoucher)
        {
            if (receiptVoucher.IsConfirmed)
            {
                receiptVoucher.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasBeenConfirmed(ReceiptVoucher receiptVoucher)
        {
            if (!receiptVoucher.IsConfirmed)
            {
                receiptVoucher.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VAllReceiptVoucherDetailsAreConfirmable(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                                              IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                                              IReceivableService _receivableService)
        {
            IList<ReceiptVoucherDetail> details = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
            foreach (var detail in details)
            {
                detail.ConfirmationDate = receiptVoucher.ConfirmationDate;
                detail.Errors = new Dictionary<string, string>();
                if (!_receiptVoucherDetailService.GetValidator().ValidConfirmObject(detail, _receivableService, _receiptVoucherDetailService))
                {
                    foreach (var error in detail.Errors)
                    {
                        receiptVoucher.Errors.Add(error.Key, error.Value);
                    }
                    if (receiptVoucher.Errors.Any()) { return receiptVoucher; }
                }
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VCashBankIsGreaterOrEqualReceiptVoucherDetails(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, bool IsGBCH)
        {
            if (!IsGBCH)
            {
                decimal totalamount = 0;
                IList<ReceiptVoucherDetail> details = _receiptVoucherDetailService.GetObjectsByReceiptVoucherId(receiptVoucher.Id);
                foreach (var detail in details)
                {
                    totalamount += detail.Amount;
                }

                CashBank cashBank = _cashBankService.GetObjectById(receiptVoucher.CashBankId);
                if (cashBank.Amount < totalamount)
                {
                    receiptVoucher.Errors.Add("Generic", "Cash bank tidak boleh kurang dari total amount");
                }
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VDetailAmountLessOrEqualTotalAmount(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            decimal totalamount = _receiptVoucherDetailService.CalcTotalAmount(receiptVoucher.Id);
            if (totalamount > receiptVoucher.TotalAmount)
            {
                receiptVoucher.Errors.Add("Generic", "Total Amount seluruh details Tidak boleh lebih dari TotalAmount");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasBeenReconciled(ReceiptVoucher receiptVoucher)
        {
            if (!receiptVoucher.IsReconciled)
            {
                receiptVoucher.Errors.Add("Generic", "Belum di reconcile");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasNotBeenReconciled(ReceiptVoucher receiptVoucher)
        {
            if (receiptVoucher.IsReconciled)
            {
                receiptVoucher.Errors.Add("Generic", "Sudah di reconcile");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VHasReconciliationDate(ReceiptVoucher receiptVoucher)
        {
            if (receiptVoucher.ReconciliationDate == null)
            {
                receiptVoucher.Errors.Add("ReconciliationDate", "Harus memiliki reconciliation date");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VIsGBCH(ReceiptVoucher receiptVoucher)
        {
            if (!receiptVoucher.IsGBCH)
            {
                receiptVoucher.Errors.Add("IsGBCH", "Harus GBCH");
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VGeneralLedgerPostingHasNotBeenClosed(ReceiptVoucher receiptVoucher, IClosingService _closingService, int CaseConfirmUnconfirmReconcileUnreconcile)
        {
            switch (CaseConfirmUnconfirmReconcileUnreconcile)
            {
                case (1): // Confirm
                    {
                        if (_closingService.IsDateClosed(receiptVoucher.ConfirmationDate.GetValueOrDefault()))
                        {
                            receiptVoucher.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (2): // Unconfirm
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            receiptVoucher.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (3): // Reconcile
                    {
                        if (_closingService.IsDateClosed(receiptVoucher.ReconciliationDate.GetValueOrDefault()))
                        {
                            receiptVoucher.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (4): // Unreconcile
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            receiptVoucher.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
            }
            return receiptVoucher;
        }

        public ReceiptVoucher VCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasContact(receiptVoucher, _contactService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasCashBank(receiptVoucher, _cashBankService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasReceiptDate(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VIfGBCHThenIsBank(receiptVoucher, _cashBankService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VIfGBCHThenHasDueDate(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher VUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasNoReceiptVoucherDetail(receiptVoucher, _receiptVoucherDetailService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VCreateObject(receiptVoucher, _receiptVoucherService, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
            return receiptVoucher;
        }

        public ReceiptVoucher VDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            VHasNotBeenDeleted(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasNoReceiptVoucherDetail(receiptVoucher, _receiptVoucherDetailService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasNotBeenConfirmed(receiptVoucher);
            return receiptVoucher;
        }

        public ReceiptVoucher VHasConfirmationDate(ReceiptVoucher obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;

        }
        public ReceiptVoucher VConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                             IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                             IReceivableService _receivableService, IClosingService _closingService)
        {
            VHasConfirmationDate(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasReceiptVoucherDetails(receiptVoucher, _receiptVoucherDetailService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VTotalAmountIsNotZero(receiptVoucher, _receiptVoucherDetailService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasNotBeenConfirmed(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasNotBeenDeleted(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VAllReceiptVoucherDetailsAreConfirmable(receiptVoucher, _receiptVoucherService, _receiptVoucherDetailService, _cashBankService, _receivableService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VDetailAmountLessOrEqualTotalAmount(receiptVoucher, _receiptVoucherDetailService);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(receiptVoucher, _closingService, 1);
            return receiptVoucher;
        }

        public ReceiptVoucher VUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasNotBeenReconciled(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VCashBankIsGreaterOrEqualReceiptVoucherDetails(receiptVoucher, _receiptVoucherDetailService, _cashBankService, receiptVoucher.IsGBCH);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(receiptVoucher, _closingService, 2);
            return receiptVoucher;
        }

        public ReceiptVoucher VReconcileObject(ReceiptVoucher receiptVoucher, IClosingService _closingService)
        {
            VHasBeenConfirmed(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasNotBeenReconciled(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VIsGBCH(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasReconciliationDate(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(receiptVoucher, _closingService, 3);
            return receiptVoucher;
        }

        public ReceiptVoucher VUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VHasBeenReconciled(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VIsGBCH(receiptVoucher);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VCashBankIsGreaterOrEqualReceiptVoucherDetails(receiptVoucher, _receiptVoucherDetailService, _cashBankService, false);
            if (!isValid(receiptVoucher)) { return receiptVoucher; }
            VGeneralLedgerPostingHasNotBeenClosed(receiptVoucher, _closingService, 4);
            return receiptVoucher;
        }

        public bool ValidCreateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                      IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            VCreateObject(receiptVoucher, _receiptVoucherService, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
            return isValid(receiptVoucher);
        }

        public bool ValidUpdateObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                      IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            receiptVoucher.Errors.Clear();
            VUpdateObject(receiptVoucher, _receiptVoucherService, _receiptVoucherDetailService, _receivableService, _contactService, _cashBankService);
            return isValid(receiptVoucher);
        }

        public bool ValidDeleteObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService)
        {
            receiptVoucher.Errors.Clear();
            VDeleteObject(receiptVoucher, _receiptVoucherDetailService);
            return isValid(receiptVoucher);
        }

        public bool ValidConfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherService _receiptVoucherService,
                                       IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                       IReceivableService _receivableService, IClosingService _closingService)
        {
            receiptVoucher.Errors.Clear();
            VConfirmObject(receiptVoucher, _receiptVoucherService, _receiptVoucherDetailService,
                           _cashBankService, _receivableService, _closingService);
            return isValid(receiptVoucher);
        }

        public bool ValidUnconfirmObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            receiptVoucher.Errors.Clear();
            VUnconfirmObject(receiptVoucher, _receiptVoucherDetailService, _cashBankService, _closingService);
            return isValid(receiptVoucher);
        }

        public bool ValidReconcileObject(ReceiptVoucher receiptVoucher, IClosingService _closingService)
        {
            receiptVoucher.Errors.Clear();
            VReconcileObject(receiptVoucher, _closingService);
            return isValid(receiptVoucher);
        }

        public bool ValidUnreconcileObject(ReceiptVoucher receiptVoucher, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService, IClosingService _closingService)
        {
            receiptVoucher.Errors.Clear();
            VUnreconcileObject(receiptVoucher, _receiptVoucherDetailService, _cashBankService, _closingService);
            return isValid(receiptVoucher);
        }

        public bool isValid(ReceiptVoucher obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ReceiptVoucher obj)
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