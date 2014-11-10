using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempGroupItemPrice
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ContactGroupId { get; set; }
        public int PriceMutationId { get; set; }
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        //public Dictionary<string, string> Errors { get; set; }

        //public virtual Item Item { get; set; }
        //public virtual ContactGroup ContactGroup { get; set; }
        //public virtual PriceMutation PriceMutation { get; set; }
    }
}
