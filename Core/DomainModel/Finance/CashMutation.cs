using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CashMutation
    {
        public int Id { get; set; }
        public int CashBankId { get; set; }
        public int Status { get; set; }
        
        public string SourceDocumentType { get; set; }
        public int SourceDocumentId { get; set; }
        public string SourceDocumentCode { get; set; }

        public decimal Amount { get; set; }
        public DateTime MutationDate { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual CashBank CashBank { get; set; }
        public Dictionary<String, String> Errors { get; set; }

    }
}