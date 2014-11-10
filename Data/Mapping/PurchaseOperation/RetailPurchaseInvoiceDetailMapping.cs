using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class RetailPurchaseInvoiceDetailMapping : EntityTypeConfiguration<RetailPurchaseInvoiceDetail>
    {
        public RetailPurchaseInvoiceDetailMapping()
        {
            HasKey(psid => psid.Id);
            HasRequired(psid => psid.RetailPurchaseInvoice)
                .WithMany(psi => psi.RetailPurchaseInvoiceDetails)
                .HasForeignKey(psid => psid.RetailPurchaseInvoiceId)
                .WillCascadeOnDelete(true); // Need to be True if RetailPurchaseInvoice table is deleted first (ie. when CashBank table deleted leading to RetailPurchaseInvoice deletion also)
            HasRequired(psid => psid.PriceMutation)
                .WithMany()
                .HasForeignKey(psid => psid.PriceMutationId)
                .WillCascadeOnDelete(false);
            Ignore(psid => psid.Errors);
        }
    }
}
