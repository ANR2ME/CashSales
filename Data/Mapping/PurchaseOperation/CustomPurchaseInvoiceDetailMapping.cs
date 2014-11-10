using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class CustomPurchaseInvoiceDetailMapping : EntityTypeConfiguration<CustomPurchaseInvoiceDetail>
    {
        public CustomPurchaseInvoiceDetailMapping()
        {
            HasKey(cpid => cpid.Id);
            HasRequired(cpid => cpid.CustomPurchaseInvoice)
                .WithMany(cpi => cpi.CustomPurchaseInvoiceDetails)
                .HasForeignKey(cpid => cpid.CustomPurchaseInvoiceId)
                .WillCascadeOnDelete(true); // Need to be True if CustomPurchaseInvoice table is deleted first (ie. when CashBank table deleted leading to CustomPurchaseInvoice deletion also)
            Ignore(cpid => cpid.Errors);
        }
    }
}
