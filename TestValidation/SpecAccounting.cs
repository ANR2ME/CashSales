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

    public class SpecAccounting : nspec
    {
        DataBuilder d;

        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                // Penghitungan Valid Comb untuk periode January 2015
                // 1. Stock Adjustment
                // 2. Cash Bank Adjustment
                // 3. Cash Bank Mutation
                // 4. Cash Bank Adjustment (Negative)
                // 5. 3x Sales Invoice
                // 6. 3x Purchase Invoice
                // 7. Closing
                // 8. Check value in ValidComb

                d.PopulateUserRole();
                d.PopulateWarehouse();
                d.PopulateItem(); // 1. Stock Adjustment
                d.PopulateSingles();
                d.PopulateCashBank(); // 2. CashBankAdjustment, 3. CashBankMutation, 4. CashBankAdjustment (Negative)

                d.PopulateSales(); // 5. 3x Cash Invoice
            }
        }

        void data_validation()
        {

            it["validates_SpecAccounting_data"] = () =>
            {
                d.salesOrder1.IsDeliveryCompleted.should_be_true();
                d.salesOrder2.IsDeliveryCompleted.should_be_true();
                d.salesOrder3.IsDeliveryCompleted.should_be_true();
                d.deliveryOrder1.IsInvoiceCompleted.should_be_true();
                d.deliveryOrder2.IsInvoiceCompleted.should_be_true();
                d.deliveryOrder3.IsInvoiceCompleted.should_be_true();
                d.salesInvoice1.IsConfirmed.should_be_true();
                d.salesInvoice2.IsConfirmed.should_be_true();
                d.salesInvoice3.IsConfirmed.should_be_true();
            };

            context["when the closing account period"] = () =>
            {
                before = () =>
                {
                    d.PopulateValidComb(); // 7. Closing
                    d._closingService.CloseObject(d.thisMonthClosing, d._accountService, d._generalLedgerJournalService, d._validCombService);
                };

                it["validates_validcomb_amount"] = () =>
                {
                    d.thisMonthClosing.IsClosed.should_be_true();
                };

                it["validates_account_subtotal"] = () =>
                {
                    // Left Hand Side
                    // Assets
                    Account Asset, CashBankAccount, AccountReceivable, GBCHReceivable, Inventory;

                    Inventory = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Inventory);
                    GBCHReceivable = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.GBCHReceivable);
                    AccountReceivable = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.AccountReceivable);
                    Account kontan = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank + d.cashBank1.Id.ToString("D3"));
                    Account bca = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank + d.cashBank2.Id.ToString("D3"));
                    CashBankAccount = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBank);
                    Asset = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Asset);

                    decimal InventoryAmount = (d.sad1.Price * d.sad1.Quantity) + (d.sad2.Price * d.sad2.Quantity) +
                                              (d.sad3.Price * d.sad3.Quantity) + (d.sad4.Price * d.sad4.Quantity) +
                                              (d.sad5.Price * d.sad5.Quantity);
                    decimal GBCHReceivableAmount = 0;
                    decimal ReceivableAmount = (d.receiptVoucher1.TotalAmount + d.receiptVoucher2.TotalAmount + d.receiptVoucher3.TotalAmount) * (-1);
                    decimal KontanAmount = d.receiptVoucher1.TotalAmount + d.receiptVoucher2.TotalAmount + d.receiptVoucher3.TotalAmount +
                                           d.cashBankAdjustment.Amount + d.cashBankAdjustment2.Amount - d.cashBankMutation.Amount;
                    decimal bcaAmount = d.cashBankMutation.Amount;
                    decimal cashBankAmount = KontanAmount + bcaAmount;
                    decimal AssetAmount = InventoryAmount + ReceivableAmount + GBCHReceivableAmount + cashBankAmount;

                    // Expense
                    Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;

                    StockAdjustmentExpense = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.StockAdjustmentExpense);
                    SalesAllowance = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.SalesAllowanceExpense);
                    Discount = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.SalesDiscountExpense);
                    COGS = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.COGS);
                    CashBankAdjustmentExpense = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.CashBankAdjustmentExpense);
                    Expense = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Expense);

                    decimal StockAdjustmentExpenseAmount = 0;
                    decimal SalesAllowanceAmount = 0;
                    decimal DiscountAmount = 0;
                    decimal COGSAmount = 0;
                    decimal CBAdjustmentExpenseAmount = Math.Abs(d.cashBankAdjustment2.Amount);
                    decimal ExpenseAmount = CBAdjustmentExpenseAmount + COGSAmount + DiscountAmount + SalesAllowanceAmount +
                                            StockAdjustmentExpenseAmount;
                    // Right Hand Side
                    Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
                    Account Equity, OwnersEquity, EquityAdjustment;
                    Account Revenue;

                    EquityAdjustment = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.EquityAdjustment);
                    decimal EquityAdjustmentAmount = (d.sad1.Price * d.sad1.Quantity) + (d.sad2.Price * d.sad2.Quantity) +
                                                     (d.sad3.Price * d.sad3.Quantity) + (d.sad4.Price * d.sad4.Quantity) +
                                                     (d.sad5.Price * d.sad5.Quantity) + d.cashBankAdjustment.Amount;

                    OwnersEquity = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.OwnersEquity);
                    decimal OwnersEquityAmount = EquityAdjustmentAmount;
                    Equity = d._accountService.GetObjectByLegacyCode(Core.Constants.Constant.AccountLegacyCode.Equity);
                    decimal EquityAmount = OwnersEquityAmount;

                    d._validCombService.FindOrCreateObjectByAccountAndClosing(Inventory.Id, d.thisMonthClosing.Id).Amount.should_be(InventoryAmount);
                    d._validCombService.FindOrCreateObjectByAccountAndClosing(kontan.Id, d.thisMonthClosing.Id).Amount.should_be(KontanAmount);
                    d._validCombService.FindOrCreateObjectByAccountAndClosing(bca.Id, d.thisMonthClosing.Id).Amount.should_be(bcaAmount);
                    d._validCombService.FindOrCreateObjectByAccountAndClosing(CashBankAccount.Id, d.thisMonthClosing.Id).Amount.should_be(cashBankAmount);
                    d._validCombService.FindOrCreateObjectByAccountAndClosing(AccountReceivable.Id, d.thisMonthClosing.Id).Amount.should_be(ReceivableAmount);
                    d._validCombService.FindOrCreateObjectByAccountAndClosing(Asset.Id, d.thisMonthClosing.Id).Amount.should_be(AssetAmount);

                    d._validCombService.FindOrCreateObjectByAccountAndClosing(EquityAdjustment.Id, d.thisMonthClosing.Id).Amount.should_be(EquityAdjustmentAmount);
                    d._validCombService.FindOrCreateObjectByAccountAndClosing(OwnersEquity.Id, d.thisMonthClosing.Id).Amount.should_be(OwnersEquityAmount);
                    d._validCombService.FindOrCreateObjectByAccountAndClosing(Equity.Id, d.thisMonthClosing.Id).Amount.should_be(EquityAmount);
                };
            };
        }
    }
}