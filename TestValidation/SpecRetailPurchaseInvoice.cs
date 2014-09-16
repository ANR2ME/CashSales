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

    public class SpecRetailPurchaseInvoice : nspec
    {
        public RetailPurchaseBuilder rpb;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                rpb = new RetailPurchaseBuilder();
                rpb.PopulateData();
            }
        }

        void retailsalesinvoice_validation()
        {
            it["validates_all_variables"] = () =>
            {
                rpb.localWarehouse.Errors.Count().should_be(0);
                rpb.contact.Errors.Count().should_be(0);
                rpb.contact2.Errors.Count().should_be(0);
                rpb.contact3.Errors.Count().should_be(0);
                rpb.blanket1.Errors.Count().should_be(0);
                rpb.blanket2.Errors.Count().should_be(0);
                rpb.blanket3.Errors.Count().should_be(0);
                rpb.cashBank.Errors.Count().should_be(0);
                rpb.cashBank2.Errors.Count().should_be(0);
                rpb.rpi1.Errors.Count().should_be(0);
                rpb.rpi2.Errors.Count().should_be(0);
                rpb.rpi3.Errors.Count().should_be(0);
                rpb.rpid1.Errors.Count().should_be(0);
                rpb.rpid2.Errors.Count().should_be(0);
                rpb.rpid3.Errors.Count().should_be(0);
            };

            it["validates_confirmed_retailpurchaseinvoice"] = () =>
            {
                rpb.rpi1.IsConfirmed.should_be_true();
                rpb.rpi2.IsConfirmed.should_be_true();
                rpb.rpi3.IsConfirmed.should_be_true();

            };

            it["validates_paid_retailpurchaseinvoice"] = () =>
            {
                rpb.rpi1.IsPaid.should_be_true();
                rpb.rpi2.IsPaid.should_be_true();
                rpb.rpi3.IsPaid.should_be_true();

                rpb.rpi1.IsFullPayment.should_be_false();
                rpb.rpi2.IsFullPayment.should_be_true();
                rpb.rpi3.IsFullPayment.should_be_true();
                
            };

            it["validates_payables_and_paymentvouchers"] = () =>
            {
                Payable payables1 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi1.Id);
                Payable payables2 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi2.Id);
                Payable payables3 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi3.Id);

                IList<PaymentVoucherDetail> paymentVoucherDetails1 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables1.Id);
                IList<PaymentVoucherDetail> paymentVoucherDetails2 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables2.Id);
                IList<PaymentVoucherDetail> paymentVoucherDetails3 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables3.Id);

                foreach (var paymentVoucherDetail in paymentVoucherDetails1)
                {
                    paymentVoucherDetail.IsConfirmed.should_be_true();
                    payables1.RemainingAmount.should_be(rpb.rpi1.Total - paymentVoucherDetail.Amount);
                }

                foreach (var paymentVoucherDetail in paymentVoucherDetails2)
                {
                    paymentVoucherDetail.IsConfirmed.should_be_true();
                    payables2.RemainingAmount.should_be(rpb.rpi2.Total - paymentVoucherDetail.Amount);
                    
                }
                
                foreach (var paymentVoucherDetail in paymentVoucherDetails3)
                {
                    paymentVoucherDetail.IsConfirmed.should_be_true();
                    payables3.RemainingAmount.should_be(rpb.rpi3.Total - paymentVoucherDetail.Amount);
                    payables3.PendingClearanceAmount.should_be(paymentVoucherDetail.Amount);
                }
                paymentVoucherDetails1.Count().should_be(1);
                paymentVoucherDetails2.Count().should_be(1);
                paymentVoucherDetails3.Count().should_be(1);
            };

            context["when_unpaid_retailpurchaseinvoice"] = () =>
            {
                before = () =>
                {
                    rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi1, rpb._paymentVoucherService, rpb._paymentVoucherDetailService, rpb._cashBankService, rpb._payableService,
                                                                   rpb._cashMutationService, rpb._generalLedgerJournalService, rpb._accountService, rpb._closingService);
                    rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi2, rpb._paymentVoucherService, rpb._paymentVoucherDetailService, rpb._cashBankService, rpb._payableService,
                                                                   rpb._cashMutationService, rpb._generalLedgerJournalService, rpb._accountService, rpb._closingService);
                    rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi3, rpb._paymentVoucherService, rpb._paymentVoucherDetailService, rpb._cashBankService, rpb._payableService,
                                                                   rpb._cashMutationService, rpb._generalLedgerJournalService, rpb._accountService, rpb._closingService);
                    rpb.rpi1.Errors.Count().should_be(0);
                    rpb.rpi2.Errors.Count().should_be(0);
                    rpb.rpi3.Errors.Count().should_be(0);
                    rpb.rpid1.Errors.Count().should_be(0);
                    rpb.rpid2.Errors.Count().should_be(0);
                    rpb.rpid3.Errors.Count().should_be(0);
                };

                it["validates_unpaid_retailpurchaseinvoice"] = () =>
                {
                    rpb.rpi1.IsPaid.should_be_false();
                    rpb.rpi2.IsPaid.should_be_false();
                    rpb.rpi3.IsPaid.should_be_false();

                    rpb.rpi2.IsFullPayment.should_be_false();
                    rpb.rpi3.IsFullPayment.should_be_false();

                    rpb.rpi1.AmountPaid.should_be(0);
                    rpb.rpi2.AmountPaid.should_be(0);
                    rpb.rpi3.AmountPaid.should_be(0);
                };

                context["when_unconfirm_retailpurchaseinvoice"] = () =>
                {
                    before = () =>
                    {
                        rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi1, rpb._retailPurchaseInvoiceDetailService, rpb._payableService, rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService, rpb._itemService, rpb._barringService, rpb._stockMutationService, rpb._closingService);
                        rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi2, rpb._retailPurchaseInvoiceDetailService, rpb._payableService, rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService, rpb._itemService, rpb._barringService, rpb._stockMutationService, rpb._closingService);
                        rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi3, rpb._retailPurchaseInvoiceDetailService, rpb._payableService, rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService, rpb._itemService, rpb._barringService, rpb._stockMutationService, rpb._closingService);
                        rpb.rpi1.Errors.Count().should_be(0);
                        rpb.rpi2.Errors.Count().should_be(0);
                        rpb.rpi3.Errors.Count().should_be(0);
                        rpb.rpid1.Errors.Count().should_be(0);
                        rpb.rpid2.Errors.Count().should_be(0);
                        rpb.rpid3.Errors.Count().should_be(0);
                    };

                    it["validates_unconfirmed_retailpurchaseinvoice"] = () =>
                    {
                        rpb.rpi1.IsConfirmed.should_be_false();
                        rpb.rpi2.IsConfirmed.should_be_false();
                        rpb.rpi3.IsConfirmed.should_be_false();
                    };

                };

            };
        }
    }
}