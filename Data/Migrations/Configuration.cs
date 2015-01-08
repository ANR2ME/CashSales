namespace Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Context.OffsetPrintingSuppliesEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Data.Context.OffsetPrintingSuppliesEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            foreach (var x in context.CashSalesInvoices.Where(x => x.PaymentDate == null && x.IsPaid))
            {
                x.PaymentDate = x.ConfirmationDate;
            }

            foreach (var x in context.CashSalesReturns.Where(x => x.PaymentDate == null && x.IsPaid))
            {
                x.PaymentDate = x.ConfirmationDate;
            }

            //context.SaveChanges();
        }
    }
}
