using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempQuantityPricing
    {
        public int Id { get; set; }
        public int ItemTypeId { get; set; }
        //public int PriceMutationId { get; set; }
        //public decimal Price { get; set; }
        public decimal Discount { get; set; }
        //public bool IsMinimumQuantity { get; set; }
        public int MinQuantity { get; set; }
        public bool IsInfiniteMaxQuantity { get; set; }
        public int MaxQuantity { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        //public Dictionary<string, string> Errors { get; set; }

        //public virtual ItemType ItemType { get; set; }
        //public virtual PriceMutation PriceMutation { get; set; }
    }
}
