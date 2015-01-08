using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CashSalesReturn
    {
        public int Id { get; set; }
        public int CashSalesInvoiceId { get; set; }
        public Nullable<DateTime> ReturnDate { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public Nullable<int> CashBankId { get; set; }
        public decimal Allowance { get; set; }
        public decimal Total { get; set; }
        public decimal CoGS { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual CashBank CashBank { get; set; }
        public virtual CashSalesInvoice CashSalesInvoice { get; set; }
        public virtual ICollection<CashSalesReturnDetail> CashSalesReturnDetails { get; set; }
    }
}
