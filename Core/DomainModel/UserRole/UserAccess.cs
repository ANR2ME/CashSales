using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class UserAccess
    {
        public int Id { get; set; }
        public int UserMenuId { get; set; }
        public int UserAccountId { get; set; }
        
        public bool AllowView { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowConfirm { get; set; }
        public bool AllowUnconfirm { get; set; }
        public bool AllowPaid { get; set; }
        public bool AllowUnpaid { get; set; }
        public bool AllowReconcile { get; set; }
        public bool AllowUnreconcile { get; set; }
        public bool AllowPrint { get; set; }
        public bool AllowUndelete { get; set; }
        public bool AllowSpecialPricing { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual UserAccount UserAccount { get; set; }
        public virtual UserMenu UserMenu { get; set; }
        public Dictionary<String, String> Errors { get; set; }
    }
}