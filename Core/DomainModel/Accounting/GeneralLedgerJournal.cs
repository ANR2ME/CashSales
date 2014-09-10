using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class GeneralLedgerJournal
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        
        public string SourceDocument { get; set; }
        public int SourceDocumentId { get; set; }
        public int Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Account Account { get; set; }
    }
}
