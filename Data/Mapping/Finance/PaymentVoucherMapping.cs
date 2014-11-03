using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PaymentVoucherMapping : EntityTypeConfiguration<PaymentVoucher>
    {
        public PaymentVoucherMapping()
        {
            HasKey(pv => pv.Id);
            HasRequired(pv => pv.Contact)
                .WithMany()
                .HasForeignKey(pv => pv.ContactId)
                .WillCascadeOnDelete(false);
            HasMany(pv => pv.PaymentVoucherDetails)
                .WithRequired(pvd => pvd.PaymentVoucher)
                .HasForeignKey(pvd => pvd.PaymentVoucherId)
                .WillCascadeOnDelete(true);
            HasRequired(pv => pv.CashBank)
                .WithMany()
                .HasForeignKey(pv => pv.CashBankId)
                .WillCascadeOnDelete(true);
            Ignore(pv => pv.Errors);
        }
    }
}
