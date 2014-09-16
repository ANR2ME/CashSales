using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class AccountMapping : EntityTypeConfiguration<Account>
    {
        public AccountMapping()
        {
            HasKey(a => a.Id);
            HasMany(a => a.GeneralLedgerJournals)
                .WithRequired(glj => glj.Account)
                .HasForeignKey(glj => glj.AccountId);
            HasMany(a => a.ValidCombs)
                .WithRequired(v => v.Account)
                .HasForeignKey(v => v.AccountId);
            HasOptional(a => a.Parent)
                .WithMany()
                .HasForeignKey(a => a.ParentId);
            Ignore(a => a.Errors);
        }
    }
}
