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

    public class SpecCustomPurchaseInvoice : nspec
    {
        public CustomPurchaseBuilder cpb;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                cpb = new CustomPurchaseBuilder();
                cpb.PopulateData();
            }
        }

        void retailsalesinvoice_validation()
        {
            it["validates_all_variables"] = () =>
            {
                cpb.localWarehouse.Errors.Count().should_be(0);
                cpb.contact.Errors.Count().should_be(0);
                cpb.contact2.Errors.Count().should_be(0);
                cpb.contact3.Errors.Count().should_be(0);
                cpb.blanket1.Errors.Count().should_be(0);
                cpb.blanket2.Errors.Count().should_be(0);
                cpb.blanket3.Errors.Count().should_be(0);
                cpb.cashBank.Errors.Count().should_be(0);
                cpb.cashBank2.Errors.Count().should_be(0);
                cpb.cpi1.Errors.Count().should_be(0);
                cpb.cpi2.Errors.Count().should_be(0);
                cpb.cpi3.Errors.Count().should_be(0);
                cpb.cpid1.Errors.Count().should_be(0);
                cpb.cpid2.Errors.Count().should_be(0);
                cpb.cpid3.Errors.Count().should_be(0);
            };

            it["validates_confirmed_custompurchaseinvoice"] = () =>
            {
                cpb.cpi1.IsConfirmed.should_be_true();
                cpb.cpi2.IsConfirmed.should_be_true();
                cpb.cpi3.IsConfirmed.should_be_true();

            };

            it["validates_paid_custompurchaseinvoice"] = () =>
            {
                cpb.cpi1.IsPaid.should_be_true();
                cpb.cpi2.IsPaid.should_be_true();
                cpb.cpi3.IsPaid.should_be_true();

                cpb.cpi1.IsFullPayment.should_be_false();
                cpb.cpi2.IsFullPayment.should_be_true();
                cpb.cpi3.IsFullPayment.should_be_true();
                
            };

            it["validates_payables_and_paymentvouchers"] = () =>
            {
                Payable payables1 = cpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, cpb.cpi1.Id);
                Payable payables2 = cpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, cpb.cpi2.Id);
                Payable payables3 = cpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, cpb.cpi3.Id);

                IList<PaymentVoucherDetail> paymentVoucherDetails1 = cpb._paymentVoucherDetailService.GetObjectsByPayableId(payables1.Id);
                IList<PaymentVoucherDetail> paymentVoucherDetails2 = cpb._paymentVoucherDetailService.GetObjectsByPayableId(payables2.Id);
                IList<PaymentVoucherDetail> paymentVoucherDetails3 = cpb._paymentVoucherDetailService.GetObjectsByPayableId(payables3.Id);

                foreach (var paymentVoucherDetail in paymentVoucherDetails1)
                {
                    paymentVoucherDetail.IsConfirmed.should_be_true();
                    payables1.RemainingAmount.should_be(cpb.cpi1.Total - paymentVoucherDetail.Amount);
                }

                foreach (var paymentVoucherDetail in paymentVoucherDetails2)
                {
                    paymentVoucherDetail.IsConfirmed.should_be_true();
                    payables2.RemainingAmount.should_be(cpb.cpi2.Total - paymentVoucherDetail.Amount);
                    
                }
                
                foreach (var paymentVoucherDetail in paymentVoucherDetails3)
                {
                    paymentVoucherDetail.IsConfirmed.should_be_true();
                    payables3.RemainingAmount.should_be(cpb.cpi3.Total - paymentVoucherDetail.Amount);
                    payables3.PendingClearanceAmount.should_be(paymentVoucherDetail.Amount);
                }
                paymentVoucherDetails1.Count().should_be(1);
                paymentVoucherDetails2.Count().should_be(1);
                paymentVoucherDetails3.Count().should_be(1);
            };

            context["when_unpaid_custompurchaseinvoice"] = () =>
            {
                before = () =>
                {
                    cpb._customPurchaseInvoiceService.UnpaidObject(cpb.cpi1, cpb._paymentVoucherService, cpb._paymentVoucherDetailService, cpb._cashBankService, cpb._payableService,
                                                                   cpb._cashMutationService, cpb._generalLedgerJournalService, cpb._accountService);
                    cpb._customPurchaseInvoiceService.UnpaidObject(cpb.cpi2, cpb._paymentVoucherService, cpb._paymentVoucherDetailService, cpb._cashBankService, cpb._payableService,
                                                                   cpb._cashMutationService, cpb._generalLedgerJournalService, cpb._accountService);
                    cpb._customPurchaseInvoiceService.UnpaidObject(cpb.cpi3, cpb._paymentVoucherService, cpb._paymentVoucherDetailService, cpb._cashBankService, cpb._payableService,
                                                                   cpb._cashMutationService, cpb._generalLedgerJournalService, cpb._accountService);
                    cpb.cpi1.Errors.Count().should_be(0);
                    cpb.cpi2.Errors.Count().should_be(0);
                    cpb.cpi3.Errors.Count().should_be(0);
                    cpb.cpid1.Errors.Count().should_be(0);
                    cpb.cpid2.Errors.Count().should_be(0);
                    cpb.cpid3.Errors.Count().should_be(0);
                };

                it["validates_unpaid_custompurchaseinvoice"] = () =>
                {
                    cpb.cpi1.IsPaid.should_be_false();
                    cpb.cpi2.IsPaid.should_be_false();
                    cpb.cpi3.IsPaid.should_be_false();

                    cpb.cpi2.IsFullPayment.should_be_false();
                    cpb.cpi3.IsFullPayment.should_be_false();

                    cpb.cpi1.AmountPaid.should_be(0);
                    cpb.cpi2.AmountPaid.should_be(0);
                    cpb.cpi3.AmountPaid.should_be(0);
                };

                context["when_unconfirm_custompurchaseinvoice"] = () =>
                {
                    before = () =>
                    {
                        cpb._customPurchaseInvoiceService.UnconfirmObject(cpb.cpi1, cpb._customPurchaseInvoiceDetailService, cpb._payableService, cpb._paymentVoucherDetailService,
                                                                          cpb._warehouseItemService, cpb._warehouseService, cpb._itemService, cpb._barringService, cpb._stockMutationService,
                                                                          cpb._priceMutationService, cpb._generalLedgerJournalService, cpb._accountService);
                        cpb._customPurchaseInvoiceService.UnconfirmObject(cpb.cpi2, cpb._customPurchaseInvoiceDetailService, cpb._payableService, cpb._paymentVoucherDetailService,
                                                                          cpb._warehouseItemService, cpb._warehouseService, cpb._itemService, cpb._barringService, cpb._stockMutationService,
                                                                          cpb._priceMutationService, cpb._generalLedgerJournalService, cpb._accountService);
                        cpb._customPurchaseInvoiceService.UnconfirmObject(cpb.cpi3, cpb._customPurchaseInvoiceDetailService, cpb._payableService, cpb._paymentVoucherDetailService,
                                                                          cpb._warehouseItemService, cpb._warehouseService, cpb._itemService, cpb._barringService, cpb._stockMutationService,
                                                                          cpb._priceMutationService, cpb._generalLedgerJournalService, cpb._accountService);
                        cpb.cpi1.Errors.Count().should_be(0);
                        cpb.cpi2.Errors.Count().should_be(0);
                        cpb.cpi3.Errors.Count().should_be(0);
                        cpb.cpid1.Errors.Count().should_be(0);
                        cpb.cpid2.Errors.Count().should_be(0);
                        cpb.cpid3.Errors.Count().should_be(0);
                    };

                    it["validates_unconfirmed_custompurchaseinvoice"] = () =>
                    {
                        cpb.cpi1.IsConfirmed.should_be_false();
                        cpb.cpi2.IsConfirmed.should_be_false();
                        cpb.cpi3.IsConfirmed.should_be_false();
                    };

                };

            };
        }
    }
}