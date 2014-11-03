using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class MemorialMapping : EntityTypeConfiguration<Memorial>
    {
        public MemorialMapping()
        {
            HasKey(m => m.Id);
            HasMany(m => m.MemorialDetails)
                .WithRequired(md => md.Memorial)
                .HasForeignKey(md => md.MemorialId);
            Ignore(pr => pr.Errors);
        }
    }
}
