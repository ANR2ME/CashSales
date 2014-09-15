using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class UserMenuMapping : EntityTypeConfiguration<UserMenu>
    {

        public UserMenuMapping()
        {
            HasKey(u => u.Id);
            HasMany(u => u.UserAccesses)
                .WithRequired(a => a.UserMenu)
                .HasForeignKey(a => a.UserMenuId)
                .WillCascadeOnDelete(true);
            Ignore(u => u.Errors);
        }
    }
}
