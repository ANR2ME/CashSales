using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class UserAccountMapping : EntityTypeConfiguration<UserAccount>
    {

        public UserAccountMapping()
        {
            HasKey(u => u.Id);
            HasMany(u => u.UserAccesses)
                .WithRequired(a => a.UserAccount)
                .HasForeignKey(a => a.UserAccountId)
                .WillCascadeOnDelete(true);
            Ignore(u => u.Errors);
        }
    }
}
