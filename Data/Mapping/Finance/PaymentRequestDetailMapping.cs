using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PaymentRequestDetailMapping : EntityTypeConfiguration<PaymentRequestDetail>
    {
        public PaymentRequestDetailMapping()
        {
            HasKey(prd => prd.Id);
            HasRequired(prd => prd.Account)
                .WithMany()
                .HasForeignKey(prd => prd.AccountId)
                .WillCascadeOnDelete(false);
            HasRequired(prd => prd.PaymentRequest)
                .WithMany(pr => pr.PaymentRequestDetails)
                .HasForeignKey(prd => prd.PaymentRequestId);
            Ignore(prd => prd.Errors);
        }
    }
}
