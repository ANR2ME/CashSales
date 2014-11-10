using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempWarehouseItem
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public int PendingDelivery { get; set; }
        public int PendingReceival { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        //public Dictionary<string, string> Errors { get; set; }
        //public virtual Warehouse Warehouse { get; set; }
        //public virtual Item Item { get; set; }
    }
}
