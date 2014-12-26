using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempCashBankMutation
    {
        public int Id { get; set; }
        
        public int SourceCashBankId { get; set; }
        public int TargetCashBankId { get; set; }
        //public string SourceCashBankName { get; set; }
        //public string TargetCashBankName { get; set; }
        public decimal Amount { get; set; }
        public string Code { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        //public virtual CashBank SourceCashBank { get; set; }
        //public virtual CashBank TargetCashBank { get; set; }
        //public Dictionary<string, string> Errors { get; set; }
    }
}
