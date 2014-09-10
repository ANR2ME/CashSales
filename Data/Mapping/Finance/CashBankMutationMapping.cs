using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CashBankMutationMapping : EntityTypeConfiguration<CashBankMutation>
    {
        public CashBankMutationMapping()
        {
            HasKey(cbm => cbm.Id);
            HasRequired(cbm => cbm.SourceCashBank)
                .WithMany()
                .HasForeignKey(cbm => cbm.SourceCashBankId)
                .WillCascadeOnDelete(false);
            HasRequired(cbm => cbm.TargetCashBank)
                .WithMany()
                .HasForeignKey(cbm => cbm.TargetCashBankId)
                .WillCascadeOnDelete(false);
            Ignore(cbm => cbm.Errors);
        }
    }
}
