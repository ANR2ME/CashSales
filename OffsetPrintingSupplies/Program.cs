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

                DataBuilder d = new DataBuilder();
                //PurchaseBuilder p = new PurchaseBuilder();
                //SalesBuilder s = new SalesBuilder();
                //RetailPurchaseBuilder rpb = new RetailPurchaseBuilder();
                //RetailSalesBuilder rsb = new RetailSalesBuilder();
                //CashSalesBuilder csb = new CashSalesBuilder();
                //CustomPurchaseBuilder cpb = new CustomPurchaseBuilder();

                AccountingFunction(d);
                //DataFunction(d);
                //PurchaseFunction(p);
                //SalesFunction(s);
                //RetailPurchaseFunction(rpb);
                //RetailSalesFunction(rsb);
                //CashSalesFunction(csb);
                //CustomPurchaseFunction(cpb);

                
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

            // End of Test
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        public static void RetailSalesFunction(RetailSalesBuilder rsb)
        {
            rsb.PopulateData();
        }

        public static void RetailPurchaseFunction(RetailPurchaseBuilder rpb)
        {
            rpb.PopulateData();
        }

        public static void CustomPurchaseFunction(CustomPurchaseBuilder cpb)
        {
            cpb.PopulateData();            
        }

        public static void DataFunction(DataBuilder d)
        {
            // d.PopulateData();

            d.PopulateUserRole();
            d.PopulateWarehouse();
            d.PopulateItem(); // 1. Stock Adjustment
            d.PopulateSingles();
            d.PopulateCashBank(); // 2. CashBankAdjustment, 3. CashBankMutation, 4. CashBankAdjustment (Negative)

            d.PopulateSales(); // 5. 3x Cash Invoice
            d.PopulateCashSales();

            d.PopulateValidComb(); // 7. Closing

            // End of Test
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        public static void AccountingFunction(DataBuilder d)
        {
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
}
