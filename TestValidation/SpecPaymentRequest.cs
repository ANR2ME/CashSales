using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;

namespace TestValidation
{

    public class SpecPaymentRequest: nspec
    {

        DataBuilder d;
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                d.PopulateData();
            }
        }

        void paymentrequest_validation()
        {
        
            it["validates_paymentrequest"] = () =>
            {
                d.paymentRequest1.Errors.Count().should_be(0);
            };

            it["paymentrequest_without_contact"] = () =>
            {
                PaymentRequest nocontact = new PaymentRequest()
                {
                    RequestedDate = DateTime.Now,
                    Description = "Tanpa Kontak",
                    DueDate = DateTime.Today.AddDays(14),
                    Amount = 200000,
                };
                nocontact = d._paymentRequestService.CreateObject(nocontact, d._contactService, d._paymentRequestDetailService, d._accountService, d._generalLedgerJournalService, d._closingService);
                nocontact.Errors.Count().should_not_be(0);
            };

            it["update_without_contact"] = () =>
            {
                d.paymentRequest1.ContactId = 0;
                d.paymentRequest1 = d._paymentRequestService.UpdateObject(d.paymentRequest1, d._contactService, d._paymentRequestDetailService, d._accountService, d._generalLedgerJournalService, d._closingService);
                d.paymentRequest1.Errors.Count().should_not_be(0);
            };

            context["when_paymentrequest_confirmed"] = () =>
            {
                before = () =>
                {
                    d._paymentRequestService.ConfirmObject(d.paymentRequest1, DateTime.Now, d._payableService, d._paymentRequestDetailService, d._accountService, d._generalLedgerJournalService, d._closingService);
                    d.paymentRequest1.Errors.Count().should_be(0);
                };

                it["validates_confirmed_paymentrequest"] = () =>
                {
                    d.paymentRequest1.IsConfirmed.should_be_true();
                };

                it["update_after_confirmed"] = () =>
                {
                    d.paymentRequest1.Amount = 300000;
                    d.paymentRequest1 = d._paymentRequestService.UpdateObject(d.paymentRequest1, d._contactService, d._paymentRequestDetailService, d._accountService, d._generalLedgerJournalService, d._closingService);
                    d.paymentRequest1.Errors.Count().should_not_be(0);
                };

                it["delete_after_confirmed"] = () =>
                {
                    d.paymentRequest1 = d._paymentRequestService.SoftDeleteObject(d.paymentRequest1);
                    d.paymentRequest1.Errors.Count().should_not_be(0);
                };

                it["unconfirm_paymentrequest"] = () =>
                {
                    d.paymentRequest1 = d._paymentRequestService.UnconfirmObject(d.paymentRequest1, d._paymentRequestDetailService, d._payableService, d._accountService, d._generalLedgerJournalService, d._closingService);
                    d.paymentRequest1.Errors.Count().should_be(0);
                };

                it["unconfirm_after_payable_have_paymentvoucherdetail"] = () =>
                {
                    PaymentVoucher paymentVoucher = d._paymentVoucherService.CreateObject(d.cashBank1.Id, d.contact.Id, DateTime.Now, d.paymentRequest1.Amount, false, d.paymentRequest1.DueDate, d.cashBank1.IsBank, "",
                                                                                d._paymentVoucherDetailService, d._payableService, d._contactService, d._cashBankService);
                    Payable payable = d._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.PaymentRequest, d.paymentRequest1.Id);
                    PaymentVoucherDetail paymentVoucherDetail = d._paymentVoucherDetailService.CreateObject(paymentVoucher.Id, payable.Id, payable.Amount, "Pembayaran Listrik", "",
                                                                                                d._paymentVoucherService, d._cashBankService, d._payableService);
                    d.paymentRequest1 = d._paymentRequestService.UnconfirmObject(d.paymentRequest1, d._paymentRequestDetailService, d._payableService, d._accountService, d._generalLedgerJournalService, d._closingService);
                    d.paymentRequest1.Errors.Count().should_not_be(0);
                };
            };

            it["delete_paymentrequest"] = () =>
            {
                d.paymentRequest1 = d._paymentRequestService.SoftDeleteObject(d.paymentRequest1);
                d.paymentRequest1.Errors.Count().should_be(0);
            };
        }
    }
}