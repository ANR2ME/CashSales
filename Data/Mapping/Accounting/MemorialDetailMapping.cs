using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class MemorialDetailMapping : EntityTypeConfiguration<MemorialDetail>
    {
        public MemorialDetailMapping()
        {
            HasKey(md => md.Id);
            HasRequired(md => md.Memorial)
                .WithMany(m => m.MemorialDetails)
                .HasForeignKey(md => md.MemorialId);
            HasRequired(md => md.Account)
                .WithMany()
                .HasForeignKey(md => md.AccountId)
                .WillCascadeOnDelete(false);
            Ignore(md => md.Errors);
        }
    }
}
