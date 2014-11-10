using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempUserMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        //public Dictionary<String, String> Errors { get; set; }
        //public ICollection<UserAccess> UserAccesses { get; set; }

    }
}