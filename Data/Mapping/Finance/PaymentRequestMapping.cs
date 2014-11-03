using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PaymentRequestMapping : EntityTypeConfiguration<PaymentRequest>
    {
        public PaymentRequestMapping()
        {
            HasKey(pr => pr.Id);
            HasRequired(pr => pr.Contact)
                .WithMany()
                .HasForeignKey(pr => pr.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(pr => pr.PaymentRequestDetails)
                .WithRequired(prd => prd.PaymentRequest)
                .HasForeignKey(prd => prd.PaymentRequestId);
            Ignore(pr => pr.Errors);
        }
    }
}
