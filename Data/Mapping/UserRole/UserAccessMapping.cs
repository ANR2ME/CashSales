using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class UserAccessMapping : EntityTypeConfiguration<UserAccess>
    {

        public UserAccessMapping()
        {
            HasKey(u => u.Id);
            HasRequired(u => u.UserMenu)
                .WithMany(u => u.UserAccesses)
                .HasForeignKey(u => u.UserMenuId)
                .WillCascadeOnDelete(true);
            HasRequired(u => u.UserAccount)
                .WithMany(u => u.UserAccesses)
                .HasForeignKey(u => u.UserAccountId)
                .WillCascadeOnDelete(true);
            Ignore(u => u.Errors);
        }
    }
}
