using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ReceivableMapping : EntityTypeConfiguration<Receivable>
    {
        public ReceivableMapping()
        {
            HasKey(r => r.Id);
            HasRequired(r => r.Contact)
                .WithMany()
                .HasForeignKey(r => r.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(r => r.ReceiptVoucherDetails)
                .WithRequired(rvd => rvd.Receivable)
                .HasForeignKey(rvd => rvd.ReceivableId);
            Ignore(r => r.Errors);
        }
    }
}
