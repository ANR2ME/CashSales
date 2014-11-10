using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempCashBankAdjustment
    {
        public int Id { get; set; }
        public int CashBankId { get; set; }

        public DateTime AdjustmentDate { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        //public virtual CashBank CashBank { get; set; }
        //public Dictionary<String, String> Errors { get; set; }
    }
}