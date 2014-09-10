using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Account
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public int Group { get; set; }
        public int Level { get; set; }
        public Nullable<int> ParentId { get; set; }
        public bool IsLegacy { get; set; }
        public bool IsLeaf { get; set; }
        public bool IsCashBankAccount { get; set; }

        public string LegacyCode { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        //public virtual ValidComb ValidComb { get; set; }
        //public virtual Closing Closing { get; set; }
        public virtual Account Parent { get; set; } 
        public virtual ICollection<GeneralLedgerJournal> GeneralLedgerJournals { get; set; }
    }
}
