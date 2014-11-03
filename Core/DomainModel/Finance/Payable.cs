using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Payable
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string PayableSource { get; set; }
        public int PayableSourceId { get; set; }
        public string PayableSourceCode { get; set; }
        public string Code { get; set; }

        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal PendingClearanceAmount { get; set; }
        public decimal AllowanceAmount { get; set; }
        public bool IsCompleted { get; set; }
        public Nullable<DateTime> CompletionDate { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual ICollection<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
