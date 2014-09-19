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

    public class SpecRetailSalesInvoice : nspec
    {
        public RetailSalesBuilder rsb;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                rsb = new RetailSalesBuilder();
                rsb.PopulateData();
            }
        }

        void retailsalesinvoice_validation()
        {
            it["validates_all_variables"] = () =>
            {
                rsb.localWarehouse.Errors.Count().should_be(0);
                rsb.contact.Errors.Count().should_be(0);
                rsb.contact2.Errors.Count().should_be(0);
                rsb.contact3.Errors.Count().should_be(0);
                rsb.blanket1.Errors.Count().should_be(0);
                rsb.blanket2.Errors.Count().should_be(0);
                rsb.blanket3.Errors.Count().should_be(0);
                rsb.cashBank.Errors.Count().should_be(0);
                rsb.cashBank2.Errors.Count().should_be(0);
                rsb.rsi1.Errors.Count().should_be(0);
                rsb.rsi2.Errors.Count().should_be(0);
                rsb.rsi3.Errors.Count().should_be(0);
                rsb.rsid1.Errors.Count().should_be(0);
                rsb.rsid2.Errors.Count().should_be(0);
                rsb.rsid3.Errors.Count().should_be(0);
            };

            it["validates_confirmed_retailsalesinvoice"] = () =>
            {
                rsb.rsi1.IsConfirmed.should_be_true();
                rsb.rsi2.IsConfirmed.should_be_true();
                rsb.rsi3.IsConfirmed.should_be_true();

            };

            it["validates_paid_retailsalesinvoice"] = () =>
            {
                rsb.rsi1.IsPaid.should_be_true();
                rsb.rsi2.IsPaid.should_be_true();
                rsb.rsi3.IsPaid.should_be_true();

                rsb.rsi1.IsFullPayment.should_be_false();
                rsb.rsi2.IsFullPayment.should_be_true();
                rsb.rsi3.IsFullPayment.should_be_true();
                
            };

            it["validates_receivables_and_receiptvouchers"] = () =>
            {
                Receivable receivables1 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi1.Id);
                Receivable receivables2 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi2.Id);
                Receivable receivables3 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi3.Id);

                IList<ReceiptVoucherDetail> receiptVoucherDetails1 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables1.Id);
                IList<ReceiptVoucherDetail> receiptVoucherDetails2 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables2.Id);
                IList<ReceiptVoucherDetail> receiptVoucherDetails3 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables3.Id);

                foreach (var receiptVoucherDetail in receiptVoucherDetails1)
                {
                    receiptVoucherDetail.IsConfirmed.should_be_true();
                    receivables1.RemainingAmount.should_be(rsb.rsi1.Total - receiptVoucherDetail.Amount);
                }

                foreach (var receiptVoucherDetail in receiptVoucherDetails2)
                {
                    receiptVoucherDetail.IsConfirmed.should_be_true();
                    receivables2.RemainingAmount.should_be(rsb.rsi2.Total - receiptVoucherDetail.Amount);
                    
                }
                
                foreach (var receiptVoucherDetail in receiptVoucherDetails3)
                {
                    receiptVoucherDetail.IsConfirmed.should_be_true();
                    receivables3.RemainingAmount.should_be(rsb.rsi3.Total - receiptVoucherDetail.Amount);
                    receivables3.PendingClearanceAmount.should_be(receiptVoucherDetail.Amount);
                }
                receiptVoucherDetails1.Count().should_be(1);
                receiptVoucherDetails2.Count().should_be(1);
                receiptVoucherDetails3.Count().should_be(1);
            };

            context["when_unpaid_retailsalesinvoice"] = () =>
            {
                before = () =>
                {
                    rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi1, rsb._receiptVoucherService, rsb._receiptVoucherDetailService, rsb._cashBankService,
                                                                rsb._receivableService, rsb._cashMutationService, rsb._generalLedgerJournalService, rsb._accountService, rsb._closingService);
                    rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi2, rsb._receiptVoucherService, rsb._receiptVoucherDetailService, rsb._cashBankService,
                                                                rsb._receivableService, rsb._cashMutationService, rsb._generalLedgerJournalService, rsb._accountService, rsb._closingService);
                    rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi3, rsb._receiptVoucherService, rsb._receiptVoucherDetailService, rsb._cashBankService,
                                                                rsb._receivableService, rsb._cashMutationService, rsb._generalLedgerJournalService, rsb._accountService, rsb._closingService);
                    rsb.rsi1.Errors.Count().should_be(0);
                    rsb.rsi2.Errors.Count().should_be(0);
                    rsb.rsi3.Errors.Count().should_be(0);
                    rsb.rsid1.Errors.Count().should_be(0);
                    rsb.rsid2.Errors.Count().should_be(0);
                    rsb.rsid3.Errors.Count().should_be(0);
                };

                it["validates_unpaid_retailsalesinvoice"] = () =>
                {
                    rsb.rsi1.IsPaid.should_be_false();
                    rsb.rsi2.IsPaid.should_be_false();
                    rsb.rsi3.IsPaid.should_be_false();

                    rsb.rsi2.IsFullPayment.should_be_false();
                    rsb.rsi3.IsFullPayment.should_be_false();

                    rsb.rsi1.AmountPaid.should_be(0);
                    rsb.rsi2.AmountPaid.should_be(0);
                    rsb.rsi3.AmountPaid.should_be(0);
                };

                context["when_unconfirm_retailsalesinvoice"] = () =>
                {
                    before = () =>
                    {
                        rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi1, rsb._retailSalesInvoiceDetailService, rsb._receivableService, rsb._receiptVoucherDetailService,
                                                                       rsb._warehouseItemService, rsb._warehouseService, rsb._itemService,
                                                                       rsb._barringService, rsb._stockMutationService, rsb._closingService);
                        rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi2, rsb._retailSalesInvoiceDetailService, rsb._receivableService, rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService, rsb._itemService, rsb._barringService, rsb._stockMutationService, rsb._closingService);
                        rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi3, rsb._retailSalesInvoiceDetailService, rsb._receivableService, rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService, rsb._itemService, rsb._barringService, rsb._stockMutationService, rsb._closingService);
                        rsb.rsi1.Errors.Count().should_be(0);
                        rsb.rsi2.Errors.Count().should_be(0);
                        rsb.rsi3.Errors.Count().should_be(0);
                        rsb.rsid1.Errors.Count().should_be(0);
                        rsb.rsid2.Errors.Count().should_be(0);
                        rsb.rsid3.Errors.Count().should_be(0);
                    };

                    it["validates_unconfirmed_retailsalesinvoice"] = () =>
                    {
                        rsb.rsi1.IsConfirmed.should_be_false();
                        rsb.rsi2.IsConfirmed.should_be_false();
                        rsb.rsi3.IsConfirmed.should_be_false();
                    };

                };

            };
        }
    }
}