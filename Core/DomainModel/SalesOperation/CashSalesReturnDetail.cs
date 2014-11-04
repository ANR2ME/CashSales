using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CashSalesReturnDetail
    {
        public int Id { get; set; }
        public int CashSalesReturnId { get; set; }
        public int CashSalesInvoiceDetailId { get; set; }
        public string Code { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal TotalPrice { get; set; }
        public decimal CoGS { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
            
        public Dictionary<string, string> Errors { get; set; }

        public virtual CashSalesReturn CashSalesReturn { get; set; }
        public virtual CashSalesInvoiceDetail CashSalesInvoiceDetail { get; set; }
    }
}
