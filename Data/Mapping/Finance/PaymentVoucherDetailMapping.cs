using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PaymentVoucherDetailMapping : EntityTypeConfiguration<PaymentVoucherDetail>
    {
        public PaymentVoucherDetailMapping()
        {
            HasKey(pvd => pvd.Id);
            HasRequired(pvd => pvd.PaymentVoucher)
                .WithMany(pv => pv.PaymentVoucherDetails)
                .HasForeignKey(pvd => pvd.PaymentVoucherId);
            HasRequired(pvd => pvd.Payable)
                .WithMany(p => p.PaymentVoucherDetails)
                .HasForeignKey(pvd => pvd.PayableId);
            Ignore(i => i.Errors);
        }
    }
}
