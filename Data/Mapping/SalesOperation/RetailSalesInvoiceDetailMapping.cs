using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class RetailSalesInvoiceDetailMapping : EntityTypeConfiguration<RetailSalesInvoiceDetail>
    {
        public RetailSalesInvoiceDetailMapping()
        {
            HasKey(rsid => rsid.Id);
            HasRequired(rsid => rsid.RetailSalesInvoice)
                .WithMany(rsi => rsi.RetailSalesInvoiceDetails)
                .HasForeignKey(rsid => rsid.RetailSalesInvoiceId)
                .WillCascadeOnDelete(true); // Need to be True if RetailSalesInvoice table is deleted first (ie. when CashBank table deleted leading to RetailSalesInvoice deletion also)
            HasRequired(rsid => rsid.PriceMutation)
                .WithMany()
                .HasForeignKey(rsid => rsid.PriceMutationId)
                .WillCascadeOnDelete(false);
            Ignore(rsid => rsid.Errors);
        }
    }
}
