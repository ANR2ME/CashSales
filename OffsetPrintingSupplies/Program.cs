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

                DataFunction(d);
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
            d.PopulateData();

            // End of Test
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
