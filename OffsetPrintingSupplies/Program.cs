using Core.DomainModel;
using Core.Interface.Service;
using Data.Context;
using Data.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestValidation;
using Validation.Validation;

namespace OffsetPrintingSupplies
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();

                //DataBuilder d = new DataBuilder();
                //PurchaseBuilder p = new PurchaseBuilder();
                //SalesBuilder s = new SalesBuilder();
                //RetailPurchaseBuilder rpb = new RetailPurchaseBuilder();
                //RetailSalesBuilder rsb = new RetailSalesBuilder();
                //CashSalesBuilder csb = new CashSalesBuilder();
                //CustomPurchaseBuilder cpb = new CustomPurchaseBuilder();

                //DataFunction(d);
                //PurchaseFunction(p);
                //SalesFunction(s);
                //RetailPurchaseFunction(rpb);
                //RetailSalesFunction(rsb);
                //CashSalesFunction(csb);
                //CustomPurchaseFunction(cpb);

                DataBuilder d = new DataBuilder();
                d.PopulateWarehouse();
                d.PopulateItem();
                d.PopulateSingles();
                d.PopulateBuilders();
                d.PopulateBarring();
                d.PopulateWarehouseMutationForRollerIdentificationAndRecovery();

                d.warehouseMutationOrder = d._warehouseMutationOrderService.SoftDeleteObject(d.warehouseMutationOrder, d._warehouseMutationOrderDetailService);

                d.warehouseMutationOrder = d._warehouseMutationOrderService.ConfirmObject(d.warehouseMutationOrder, DateTime.Today, d._warehouseMutationOrderDetailService, d._itemService, d._barringService, d._warehouseItemService, d._stockMutationService);
                d.warehouseMutationOrder = d._warehouseMutationOrderService.SoftDeleteObject(d.warehouseMutationOrder, d._warehouseMutationOrderDetailService);
                
            }
        }

        public static void PurchaseFunction(PurchaseBuilder p)
        {
            p.PopulateData();

            //---

        }

        public static void SalesFunction(SalesBuilder s)
        {
            s.PopulateData();
        }

        public static void CashSalesFunction(CashSalesBuilder b)
        {
            b.PopulateData();

            // ---
            Receivable receivables1 = b._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, b.csi1.Id);
            Receivable receivables2 = b._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, b.csi2.Id);
            Receivable receivables3 = b._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.CashSalesInvoice, b.csi3.Id);

            IList<ReceiptVoucherDetail> receiptVoucherDetails1 = b._receiptVoucherDetailService.GetObjectsByReceivableId(receivables1.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails2 = b._receiptVoucherDetailService.GetObjectsByReceivableId(receivables2.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails3 = b._receiptVoucherDetailService.GetObjectsByReceivableId(receivables3.Id);

            foreach (var receiptVoucherDetail in receiptVoucherDetails1)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("1:FALSE");
            }

            foreach (var receiptVoucherDetail in receiptVoucherDetails2)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("2:FALSE");
            }

            foreach (var receiptVoucherDetail in receiptVoucherDetails3)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("3:FALSE");
            }
            // ---
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            // ---
            b._cashSalesInvoiceService.UnpaidObject(b.csi1, b._receiptVoucherService, b._receiptVoucherDetailService,
                                                        b._cashBankService, b._receivableService, b._cashMutationService, b._cashSalesReturnService,
                                                        b._generalLedgerJournalService, b._accountService);
            b._cashSalesInvoiceService.UnpaidObject(b.csi2, b._receiptVoucherService, b._receiptVoucherDetailService,
                                                        b._cashBankService, b._receivableService, b._cashMutationService, b._cashSalesReturnService,
                                                        b._generalLedgerJournalService, b._accountService);
            b._cashSalesInvoiceService.UnpaidObject(b.csi3, b._receiptVoucherService, b._receiptVoucherDetailService,
                                                        b._cashBankService, b._receivableService, b._cashMutationService, b._cashSalesReturnService,
                                                        b._generalLedgerJournalService, b._accountService);

            b._cashSalesInvoiceService.UnconfirmObject(b.csi1, b._cashSalesInvoiceDetailService, b._receivableService,
                                                           b._receiptVoucherDetailService, b._warehouseItemService, b._warehouseService,
                                                           b._itemService, b._barringService, b._stockMutationService,
                                                           b._generalLedgerJournalService, b._accountService);
            b._cashSalesInvoiceService.UnconfirmObject(b.csi2, b._cashSalesInvoiceDetailService, b._receivableService,
                                                           b._receiptVoucherDetailService, b._warehouseItemService, b._warehouseService,
                                                           b._itemService, b._barringService, b._stockMutationService,
                                                           b._generalLedgerJournalService, b._accountService);
            b._cashSalesInvoiceService.UnconfirmObject(b.csi3, b._cashSalesInvoiceDetailService, b._receivableService,
                                                           b._receiptVoucherDetailService, b._warehouseItemService, b._warehouseService,
                                                           b._itemService, b._barringService, b._stockMutationService,
                                                           b._generalLedgerJournalService, b._accountService);

            // End of Test
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        public static void RetailSalesFunction(RetailSalesBuilder rsb)
        {
            rsb.PopulateData();

            // ---
            Receivable receivables1 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi1.Id);
            Receivable receivables2 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi2.Id);
            Receivable receivables3 = rsb._receivableService.GetObjectBySource(Core.Constants.Constant.ReceivableSource.RetailSalesInvoice, rsb.rsi3.Id);

            IList<ReceiptVoucherDetail> receiptVoucherDetails1 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables1.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails2 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables2.Id);
            IList<ReceiptVoucherDetail> receiptVoucherDetails3 = rsb._receiptVoucherDetailService.GetObjectsByReceivableId(receivables3.Id);

            foreach (var receiptVoucherDetail in receiptVoucherDetails1)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("1:FALSE");
            }

            foreach (var receiptVoucherDetail in receiptVoucherDetails2)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("2:FALSE");
            }

            foreach (var receiptVoucherDetail in receiptVoucherDetails3)
            {
                if (!receiptVoucherDetail.IsConfirmed) Console.WriteLine("3:FALSE");
            }
            // ---
            rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi1, rsb._receiptVoucherService, rsb._receiptVoucherDetailService, 
                                                        rsb._cashBankService, rsb._receivableService, rsb._cashMutationService,
                                                        rsb._generalLedgerJournalService, rsb._accountService);
            rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi2, rsb._receiptVoucherService, rsb._receiptVoucherDetailService,
                                                        rsb._cashBankService, rsb._receivableService, rsb._cashMutationService,
                                                        rsb._generalLedgerJournalService, rsb._accountService);
            rsb._retailSalesInvoiceService.UnpaidObject(rsb.rsi3, rsb._receiptVoucherService, rsb._receiptVoucherDetailService,
                                                        rsb._cashBankService, rsb._receivableService, rsb._cashMutationService,
                                                        rsb._generalLedgerJournalService, rsb._accountService);

            rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi1, rsb._retailSalesInvoiceDetailService, rsb._receivableService, 
                                                           rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService, 
                                                           rsb._itemService, rsb._barringService, rsb._stockMutationService);
            rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi2, rsb._retailSalesInvoiceDetailService, rsb._receivableService,
                                                           rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService,
                                                           rsb._itemService, rsb._barringService, rsb._stockMutationService);
            rsb._retailSalesInvoiceService.UnconfirmObject(rsb.rsi3, rsb._retailSalesInvoiceDetailService, rsb._receivableService,
                                                           rsb._receiptVoucherDetailService, rsb._warehouseItemService, rsb._warehouseService,
                                                           rsb._itemService, rsb._barringService, rsb._stockMutationService);
        }

        public static void RetailPurchaseFunction(RetailPurchaseBuilder rpb)
        {
            rpb.PopulateData();

            // ---
            Payable payables1 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi1.Id);
            Payable payables2 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi2.Id);
            Payable payables3 = rpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.RetailPurchaseInvoice, rpb.rpi3.Id);

            IList<PaymentVoucherDetail> paymentVoucherDetails1 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables1.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails2 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables2.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails3 = rpb._paymentVoucherDetailService.GetObjectsByPayableId(payables3.Id);

            foreach (var paymentVoucherDetail in paymentVoucherDetails1)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("1:FALSE");
            }

            foreach (var paymentVoucherDetail in paymentVoucherDetails2)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("2:FALSE");
            }

            foreach (var paymentVoucherDetail in paymentVoucherDetails3)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("3:FALSE");
            }
            // ---
            rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi1, rpb._paymentVoucherService, rpb._paymentVoucherDetailService,
                                                        rpb._cashBankService, rpb._payableService, rpb._cashMutationService,
                                                        rpb._generalLedgerJournalService, rpb._accountService);
            rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi2, rpb._paymentVoucherService, rpb._paymentVoucherDetailService,
                                                        rpb._cashBankService, rpb._payableService, rpb._cashMutationService,
                                                        rpb._generalLedgerJournalService, rpb._accountService);
            rpb._retailPurchaseInvoiceService.UnpaidObject(rpb.rpi3, rpb._paymentVoucherService, rpb._paymentVoucherDetailService,
                                                        rpb._cashBankService, rpb._payableService, rpb._cashMutationService,
                                                        rpb._generalLedgerJournalService, rpb._accountService);

            rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi1, rpb._retailPurchaseInvoiceDetailService, rpb._payableService,
                                                           rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService,
                                                           rpb._itemService, rpb._barringService, rpb._stockMutationService);
            rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi2, rpb._retailPurchaseInvoiceDetailService, rpb._payableService,
                                                           rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService,
                                                           rpb._itemService, rpb._barringService, rpb._stockMutationService);
            rpb._retailPurchaseInvoiceService.UnconfirmObject(rpb.rpi3, rpb._retailPurchaseInvoiceDetailService, rpb._payableService,
                                                           rpb._paymentVoucherDetailService, rpb._warehouseItemService, rpb._warehouseService,
                                                           rpb._itemService, rpb._barringService, rpb._stockMutationService);
        }

        public static void CustomPurchaseFunction(CustomPurchaseBuilder cpb)
        {
            cpb.PopulateData();

            // ---
            Payable payables1 = cpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, cpb.cpi1.Id);
            Payable payables2 = cpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, cpb.cpi2.Id);
            Payable payables3 = cpb._payableService.GetObjectBySource(Core.Constants.Constant.PayableSource.CustomPurchaseInvoice, cpb.cpi3.Id);

            IList<PaymentVoucherDetail> paymentVoucherDetails1 = cpb._paymentVoucherDetailService.GetObjectsByPayableId(payables1.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails2 = cpb._paymentVoucherDetailService.GetObjectsByPayableId(payables2.Id);
            IList<PaymentVoucherDetail> paymentVoucherDetails3 = cpb._paymentVoucherDetailService.GetObjectsByPayableId(payables3.Id);

            foreach (var paymentVoucherDetail in paymentVoucherDetails1)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("1:FALSE");
            }

            foreach (var paymentVoucherDetail in paymentVoucherDetails2)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("2:FALSE");
            }

            foreach (var paymentVoucherDetail in paymentVoucherDetails3)
            {
                if (!paymentVoucherDetail.IsConfirmed) Console.WriteLine("3:FALSE");
            }
            // ---
            cpb._customPurchaseInvoiceService.UnpaidObject(cpb.cpi1, cpb._paymentVoucherService, cpb._paymentVoucherDetailService,
                                                        cpb._cashBankService, cpb._payableService, cpb._cashMutationService, 
                                                        cpb._generalLedgerJournalService, cpb._accountService);
            cpb._customPurchaseInvoiceService.UnpaidObject(cpb.cpi2, cpb._paymentVoucherService, cpb._paymentVoucherDetailService,
                                                        cpb._cashBankService, cpb._payableService, cpb._cashMutationService,
                                                        cpb._generalLedgerJournalService, cpb._accountService);
            cpb._customPurchaseInvoiceService.UnpaidObject(cpb.cpi3, cpb._paymentVoucherService, cpb._paymentVoucherDetailService,
                                                        cpb._cashBankService, cpb._payableService, cpb._cashMutationService,
                                                        cpb._generalLedgerJournalService, cpb._accountService);

            cpb._customPurchaseInvoiceService.UnconfirmObject(cpb.cpi1, cpb._customPurchaseInvoiceDetailService, cpb._payableService,
                                                           cpb._paymentVoucherDetailService, cpb._warehouseItemService, cpb._warehouseService,
                                                           cpb._itemService, cpb._barringService, cpb._stockMutationService, cpb._priceMutationService,
                                                           cpb._generalLedgerJournalService, cpb._accountService);
            cpb._customPurchaseInvoiceService.UnconfirmObject(cpb.cpi2, cpb._customPurchaseInvoiceDetailService, cpb._payableService,
                                                           cpb._paymentVoucherDetailService, cpb._warehouseItemService, cpb._warehouseService,
                                                           cpb._itemService, cpb._barringService, cpb._stockMutationService, cpb._priceMutationService,
                                                           cpb._generalLedgerJournalService, cpb._accountService);
            cpb._customPurchaseInvoiceService.UnconfirmObject(cpb.cpi3, cpb._customPurchaseInvoiceDetailService, cpb._payableService,
                                                           cpb._paymentVoucherDetailService, cpb._warehouseItemService, cpb._warehouseService,
                                                           cpb._itemService, cpb._barringService, cpb._stockMutationService, cpb._priceMutationService,
                                                           cpb._generalLedgerJournalService, cpb._accountService);
        }

        public static void DataFunction(DataBuilder d)
        {
            d.PopulateData();

            PaymentRequest nocontact = new PaymentRequest()
            {
                RequestedDate = DateTime.Now,
                Description = "Tanpa Kontak",
                DueDate = DateTime.Today.AddDays(14),
                Amount = 200000,
            };
            nocontact = d._paymentRequestService.CreateObject(nocontact, d._contactService);

            d._paymentRequestService.ConfirmObject(d.paymentRequest1, DateTime.Now, d._payableService);

            Console.WriteLine(d.admin.Errors.Count());
            Console.WriteLine(d.user.Errors.Count());
            Console.WriteLine(d.menudata.Errors.Count());
            Console.WriteLine(d.menufinance.Errors.Count());
            Console.WriteLine(d.admindata.Errors.Count());
            Console.WriteLine(d.adminfinance.Errors.Count());

            string oldpassword = "sysadmin";
            //d.admin.Password = "sysadmin123";
            d.admin = d._userAccountService.UpdateObjectPassword(d.admin, oldpassword, "sysadmin123", "sysadmin123");
            Console.WriteLine(d.admin.Errors.Count());

            d.user.Username = "Adam";
            d._userAccountService.CreateObject(d.user);
            d.admin = d._userAccountService.SoftDeleteObject(d.admin, d.user.Id);
            Console.WriteLine(d.admin.Errors.Count());

            // End of Test
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
