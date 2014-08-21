﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class CashSalesInvoiceDetail
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int PriceMutationId { get; set; }
        public int CashSalesInvoiceId { get; set; }
        public string Code { get; set; }
        
        public int Quantity { get; set; }
        public decimal CoGS { get; set; }
        
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
            
        public Dictionary<string, string> Errors { get; set; }

        public virtual Item Item { get; set; }
        public virtual PriceMutation PriceMutation { get; set; }
        public virtual CashSalesInvoice CashSalesInvoice { get; set; }
        public virtual ICollection<CashSalesReturnDetail> CashSalesReturnDetails { get; set; }
    }
}