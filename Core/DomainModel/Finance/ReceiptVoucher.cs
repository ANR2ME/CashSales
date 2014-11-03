using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ReceiptVoucher
    {
        public int Id { get; set; }
        public int ContactId { get; set; } // Nullable ?
        public int CashBankId { get; set; }
        public string Code { get; set; }
        public DateTime ReceiptDate { get; set; }
        public bool IsGBCH { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        //public bool IsBank { get; set; } // Moved to CashBank
        public bool IsReconciled { get; set; }
        public Nullable<DateTime> ReconciliationDate { get; set; }

        public decimal TotalAmount { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<ReceiptVoucherDetail> ReceiptVoucherDetails { get; set; }

        public Dictionary<String, String> Errors { get; set; }
    }
}
