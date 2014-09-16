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

    public class SpecCashSalesInvoice : nspec
    {
        public CashSalesBuilder b;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                b = new CashSalesBuilder();
                b.PopulateData();
            }
        }

        void cashsalesinvoice_validation()
        {
            it["validates_all_variables"] = () =>
            {
                b.localWarehouse.Errors.Count().should_be(0);
                b.contact.Errors.Count().should_be(0);
                b.contact2.Errors.Count().should_be(0);
                b.contact3.Errors.Count().should_be(0);
                b.blanket1.Errors.Count().should_be(0);
                b.blanket2.Errors.Count().should_be(0);
                b.blanket3.Errors.Count().should_be(0);
                b.cashBank.Errors.Count().should_be(0);
                b.cashBank2.Errors.Count().should_be(0);
                b.csi1.Errors.Count().should_be(0);
                b.csi2.Errors.Count().should_be(0);
                b.csi3.Errors.Count().should_not_be(0);
                b.csid1.Errors.Count().should_be(0);
                b.csid2.Errors.Count().should_be(0);
                b.csid3.Errors.Count().should_be(0);
            };

            it["validates_confirmed_cashsalesinvoice"] = () =>
            {
                b.csi1.IsConfirmed.should_be_true();
                b.csi2.IsConfirmed.should_be_true();
                b.csi3.IsConfirmed.should_be_true();

            };

            it["validates_paid_cashsalesinvoice"] = () =>
            {
                b.csi1.IsPaid.should_be_true();
                b.csi2.IsPaid.should_be_true();
                b.csi3.IsPaid.should_be_false();

                b.csi1.IsFullPayment.should_be_false();
                b.csi2.IsFullPayment.should_be_true();
                b.csi3.IsFullPayment.should_be_false();

                b.csid1.CoGS.should_be(100 * 10000); // Quantity=100, AvgPrice=10000
                b.csid1.Amount.should_be(100 * 10000 * (100-50)/100); // SellingPrice=10000, QuantityPricing discount = 50% for quantity>=51
                b.csid2.CoGS.should_be(30 * 20000); // Quantity=30, AvgPrice=20000
                b.csid2.Amount.should_be(30 * 20000 * (100 - 10) / 100); // SellingPrice=20000, QuantityPricing discount = 10% for quantity=20-40, 25% for q=30-50
                b.csid4.Amount.should_be(10 * 30000); // SellingPrice=30000, not existed QuantityPricing for Q=10
            };

            it["validates_receivables_and_receiptvouchers"] = () =>
            {
                Receivable receivable1 = b._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, b.csi1.Id);
                Receivable receivable2 = b._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, b.csi2.Id);
                Receivable receivable3 = b._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, b.csi3.Id);

                IList<ReceiptVoucherDetail> receiptVoucherDetails1 = b._receiptVoucherDetailService.GetObjectsByReceivableId(receivable1.Id);
                IList<ReceiptVoucherDetail> receiptVoucherDetails2 = b._receiptVoucherDetailService.GetObjectsByReceivableId(receivable2.Id);
                IList<ReceiptVoucherDetail> receiptVoucherDetails3 = b._receiptVoucherDetailService.GetObjectsByReceivableId(receivable3.Id);

                foreach (var receiptVoucherDetail in receiptVoucherDetails1)
                {
                    receiptVoucherDetail.IsConfirmed.should_be_true();
                    receivable1.RemainingAmount.should_be(b.csi1.Total - (receiptVoucherDetail.Amount + receivable1.AllowanceAmount));
                }

                foreach (var receiptVoucherDetail in receiptVoucherDetails2)
                {
                    receiptVoucherDetail.IsConfirmed.should_be_true();
                    receivable2.RemainingAmount.should_be(b.csi2.Total - (receiptVoucherDetail.Amount + receivable2.AllowanceAmount));
                    
                }

                foreach (var receiptVoucherDetail in receiptVoucherDetails3)
                {
                    receiptVoucherDetail.IsConfirmed.should_be_true();
                    receivable3.RemainingAmount.should_be(b.csi3.Total - (receiptVoucherDetail.Amount + receivable3.AllowanceAmount));

                }

                receiptVoucherDetails1.Count().should_be(1);
                receiptVoucherDetails2.Count().should_be(1);
                receiptVoucherDetails3.Count().should_be(0);
            };

            context["when_unpaid_cashsalesinvoice"] = () =>
            {
                before = () =>
                {
                    b._cashSalesInvoiceService.UnpaidObject(b.csi1, b._receiptVoucherService, b._receiptVoucherDetailService, b._cashBankService, b._receivableService, b._cashMutationService,
                                                            b._cashSalesReturnService, b._generalLedgerJournalService, b._accountService, b._closingService);
                    b._cashSalesInvoiceService.UnpaidObject(b.csi2, b._receiptVoucherService, b._receiptVoucherDetailService, b._cashBankService, b._receivableService, b._cashMutationService,
                                                            b._cashSalesReturnService, b._generalLedgerJournalService, b._accountService, b._closingService);
                    b._cashSalesInvoiceService.UnpaidObject(b.csi3, b._receiptVoucherService, b._receiptVoucherDetailService, b._cashBankService, b._receivableService, b._cashMutationService,
                                                            b._cashSalesReturnService, b._generalLedgerJournalService, b._accountService, b._closingService);
                    b.csi1.Errors.Count().should_not_be(0); // Already have CashSalesReturn
                    b.csi2.Errors.Count().should_be(0);
                    b.csi3.Errors.Count().should_not_be(0);
                    b.csid1.Errors.Count().should_be(0);
                    b.csid2.Errors.Count().should_be(0);
                    b.csid3.Errors.Count().should_be(0);
                };

                it["validates_unpaid_cashsalesinvoice"] = () =>
                {
                    b.csi1.IsPaid.should_be_true(); // Already have CashSalesReturn
                    b.csi2.IsPaid.should_be_false();
                    b.csi3.IsPaid.should_be_false();

                    b.csi2.IsFullPayment.should_be_false();
                    b.csi3.IsFullPayment.should_be_false();

                    b.csi1.AmountPaid.should_not_be(0);
                    b.csi2.AmountPaid.should_be(0);
                    //b.csi3.AmountPaid.should_be_null();
                };

                context["when_unconfirm_cashsalesinvoice"] = () =>
                {
                    before = () =>
                    {
                        b._cashSalesInvoiceService.UnconfirmObject(b.csi1, b._cashSalesInvoiceDetailService, b._receivableService, b._receiptVoucherDetailService, b._warehouseItemService, b._warehouseService,
                                                                   b._itemService, b._barringService, b._stockMutationService, b._generalLedgerJournalService, b._accountService, b._closingService);
                        b._cashSalesInvoiceService.UnconfirmObject(b.csi2, b._cashSalesInvoiceDetailService, b._receivableService, b._receiptVoucherDetailService, b._warehouseItemService, b._warehouseService,
                                                                   b._itemService, b._barringService, b._stockMutationService, b._generalLedgerJournalService, b._accountService, b._closingService);
                        b._cashSalesInvoiceService.UnconfirmObject(b.csi3, b._cashSalesInvoiceDetailService, b._receivableService, b._receiptVoucherDetailService, b._warehouseItemService, b._warehouseService,
                                                                   b._itemService, b._barringService, b._stockMutationService, b._generalLedgerJournalService, b._accountService, b._closingService);
                        b.csi1.Errors.Count().should_not_be(0);
                        b.csi2.Errors.Count().should_be(0);
                        b.csi3.Errors.Count().should_be(0);
                        b.csid1.Errors.Count().should_be(0);
                        b.csid2.Errors.Count().should_be(0);
                        b.csid3.Errors.Count().should_be(0);
                    };

                    it["validates_unconfirmed_cashsalesinvoice"] = () =>
                    {
                        b.csi1.IsConfirmed.should_be_true();
                        b.csi2.IsConfirmed.should_be_false();
                        b.csi3.IsConfirmed.should_be_false();
                    };

                };

            };

            context["when_creating_cashsalesreturn"] = () =>
            {
                before = () =>
                {
                    b.csr1.Errors.Count().should_be(0);
                    b.csrd1.Errors.Count().should_be(0);
                };

                it["validates_confirmed_cashsalesreturn"] = () =>
                {
                    b.csr1.IsConfirmed.should_be_true();
                };

                it["validates_paid_cashsalesreturn"] = () =>
                {
                    b.csr1.IsPaid.should_be_true();

                    b.csr1.Total.should_be(b.csrd1.Quantity * (b.csid1.Amount/b.csid1.Quantity)); // Item price = 10000
                };

                it["validates_payables_and_paymentvouchers"] = () =>
                {
                    Payable payable1 = b._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CashSalesReturn, b.csr1.Id);
                    
                    IList<PaymentVoucherDetail> paymentVoucherDetails1 = b._paymentVoucherDetailService.GetObjectsByPayableId(payable1.Id);
                    
                    foreach (var paymentVoucherDetail in paymentVoucherDetails1)
                    {
                        paymentVoucherDetail.IsConfirmed.should_be_true();
                        payable1.RemainingAmount.should_be(b.csr1.Total - (paymentVoucherDetail.Amount + payable1.AllowanceAmount));
                    }

                    paymentVoucherDetails1.Count().should_be(1);
                    
                };

                it["validates_unpaid_cashsalesinvoice_with_cashsalesreturn"] = () =>
                {
                    b._cashSalesInvoiceService.UnpaidObject(b.csi1, b._receiptVoucherService, b._receiptVoucherDetailService, b._cashBankService, b._receivableService, b._cashMutationService,
                                                            b._cashSalesReturnService, b._generalLedgerJournalService, b._accountService, b._closingService);
                    
                    b.csi1.Errors.Count().should_not_be(0);
                };

                context["when_unpaid_cashsalesreturn"] = () =>
                {
                    before = () =>
                    {
                        b._cashSalesReturnService.UnpaidObject(b.csr1, b._paymentVoucherService, b._paymentVoucherDetailService, b._cashBankService, b._payableService, b._cashMutationService,
                                                               b._generalLedgerJournalService, b._accountService, b._closingService);
                        b.csr1.Errors.Count().should_be(0);
                        b.csrd1.Errors.Count().should_be(0);
                    };

                    it["validates_unpaid_cashsalesreturn"] = () =>
                    {
                        b.csr1.IsPaid.should_be_false();
                    };

                    context["when_unconfirm_cashsalesreturn"] = () =>
                    {
                        before = () =>
                        {
                            b._cashSalesReturnService.UnconfirmObject(b.csr1, b._cashSalesReturnDetailService, b._cashSalesInvoiceDetailService, b._payableService, b._paymentVoucherDetailService, b._warehouseItemService, b._warehouseService, b._itemService, b._barringService, b._stockMutationService, b._closingService);
                            
                            b.csr1.Errors.Count().should_be(0);

                            b.csrd1.Errors.Count().should_be(0);
                        };

                        it["validates_unconfirmed_cashsalesreturn"] = () =>
                        {
                            b.csr1.IsConfirmed.should_be_false();
                        };

                        it["validates_delete_cashsalesreturn"] = () =>
                        {
                            b._cashSalesReturnDetailService.SoftDeleteObject(b.csrd1, b._cashSalesReturnService);
                            b._cashSalesReturnService.SoftDeleteObject(b.csr1, b._cashSalesReturnDetailService);
                            b.csrd1.Errors.Count().should_be(0);
                            b.csr1.Errors.Count().should_be(0);
                        };

                        it["validates_unpaid_cashsalesinvoice_after_cashsalesreturn_deleted"] = () =>
                        {
                            b._cashSalesReturnDetailService.SoftDeleteObject(b.csrd1, b._cashSalesReturnService);
                            b._cashSalesReturnService.SoftDeleteObject(b.csr1, b._cashSalesReturnDetailService);
                            b._cashSalesInvoiceService.UnpaidObject(b.csi1, b._receiptVoucherService, b._receiptVoucherDetailService, b._cashBankService, b._receivableService,
                                                                    b._cashMutationService, b._cashSalesReturnService, b._generalLedgerJournalService, b._accountService, b._closingService);

                            b.csi1.Errors.Count().should_be(0);
                        };

                    };

                };

            };
        }
    }
}