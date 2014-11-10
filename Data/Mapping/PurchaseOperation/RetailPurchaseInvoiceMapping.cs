using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class RetailPurchaseInvoiceMapping : EntityTypeConfiguration<RetailPurchaseInvoice>
    {
        public RetailPurchaseInvoiceMapping()
        {
            HasKey(psi => psi.Id);
            HasMany(psi => psi.RetailPurchaseInvoiceDetails)
                .WithRequired(psid => psid.RetailPurchaseInvoice)
                .HasForeignKey(psid => psid.RetailPurchaseInvoiceId);
            HasRequired(psi => psi.CashBank)
                .WithMany()
                .HasForeignKey(psi => psi.CashBankId)
                .WillCascadeOnDelete(true);  // Need to be True if CashBank Table is deleted before RetailPurchaseInvoice table deleted
            HasRequired(psi => psi.Warehouse)
                .WithMany()
                .HasForeignKey(psi => psi.WarehouseId)
                .WillCascadeOnDelete(false);
            HasRequired(psi => psi.Contact)
                .WithMany()
                .HasForeignKey(psi => psi.ContactId)
                .WillCascadeOnDelete(false);
            Ignore(psi => psi.Errors);
        }
    }
}
