using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class ValidCombMapping : EntityTypeConfiguration<ValidComb>
    {
        public ValidCombMapping()
        {
            HasKey(vc => vc.Id);
            HasRequired(vc => vc.Account)
                .WithMany(a => a.ValidCombs)
                .HasForeignKey(vc => vc.AccountId);
            HasRequired(vc => vc.Closing)
                .WithMany(c => c.ValidCombs)
                .HasForeignKey(vc => vc.ClosingId);
            Ignore(vc => vc.Errors);
        }
    }
}
