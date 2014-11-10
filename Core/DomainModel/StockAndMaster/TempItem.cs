using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    /*
     * Created using Table per Type
     * http://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-2-table-per-type-tpt
     */

    public partial class TempItem
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        public string Sku { get; set; }

        public string Name { get; set; }
        public string Category { get; set; }
        public int UoMId { get; set; }
        public int Quantity { get; set; }
        public int PendingDelivery { get; set; }
        public int PendingReceival { get; set; }

        public string Description { get; set; }
        public decimal SellingPrice { get; set; }
        public int PriceMutationId { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal Margin { get; set; }
        //public decimal RoundingUp { get; set; }
        public virtual ICollection<StockMutation> StockMutations { get; set; }
        public virtual ICollection<PriceMutation> PriceMutations { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        //public Dictionary<string, string> Errors { get; set; }
        //public virtual UoM UoM { get; set; }
        //public virtual ItemType ItemType { get; set; }
        //public virtual ICollection<RecoveryAccessoryDetail> RecoveryAccessoryDetails { get; set; }
        //public virtual ICollection<WarehouseItem> WarehouseItems { get; set; }
    }
}
