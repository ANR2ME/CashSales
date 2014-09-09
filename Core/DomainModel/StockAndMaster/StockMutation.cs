using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class StockMutation
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Nullable<int> WarehouseId { get; set; }
        public Nullable<int> WarehouseItemId { get; set; }
        public int ItemCase { get; set; }
        public int Status { get; set; }

        public string SourceDocumentType { get; set; }
        public string SourceDocumentDetailType { get; set; }
        public int SourceDocumentId { get; set; }
        public int SourceDocumentDetailId { get; set; }
        public string SourceDocumentCode { get; set; }
        public string SourceDocumentDetailCode { get; set; }

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        public virtual WarehouseItem WarehouseItem { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Item Item { get; set; }
        public Dictionary<String, String> Errors { get; set; }

    }
}