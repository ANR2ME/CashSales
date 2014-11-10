using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempStockAdjustmentDetail
    {
        public int Id { get; set; }
        public int StockAdjustmentId { get; set; }
        public int ItemId { get; set; }
        public string Code { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        
        //public virtual Item Item { get; set; }
        //public virtual StockAdjustment StockAdjustment { get; set; }
        //public Dictionary<String, String> Errors { get; set; }
    }
}