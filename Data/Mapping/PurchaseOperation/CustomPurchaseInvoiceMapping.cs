using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class CustomPurchaseInvoiceMapping : EntityTypeConfiguration<CustomPurchaseInvoice>
    {
        public CustomPurchaseInvoiceMapping()
        {
            HasKey(cpi => cpi.Id);
            HasMany(cpi => cpi.CustomPurchaseInvoiceDetails)
                .WithRequired(cpid => cpid.CustomPurchaseInvoice)
                .HasForeignKey(cpid => cpid.CustomPurchaseInvoiceId);
            HasRequired(cpi => cpi.CashBank)
                .WithMany()
                .HasForeignKey(cpi => cpi.CashBankId)
                .WillCascadeOnDelete(true);  // Need to be True if CashBank Table is deleted before CustomPurchaseInvoice table deleted
            HasRequired(cpi => cpi.Warehouse)
                .WithMany()
                .HasForeignKey(cpi => cpi.WarehouseId)
                .WillCascadeOnDelete(false);
            HasRequired(cpi => cpi.Contact)
                .WithMany()
                .HasForeignKey(cpi => cpi.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(cpi => cpi.Errors);
        }
    }
}
