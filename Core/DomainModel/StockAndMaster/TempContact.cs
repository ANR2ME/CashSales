using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class TempContact
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string PIC { get; set; }
        public string PICContactNo { get; set; }
        public string Email { get; set; }

        public int ContactGroupId { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        
        //public Dictionary<string, string> Errors { get; set; }
        //public virtual ContactGroup ContactGroup { get; set; }
        //public ICollection<Barring> Barrings { get; set; }
        //public ICollection<CoreIdentification> CoreIdentifications { get; set; }
        //public ICollection<BarringOrder> BarringOrders { get; set; }
        //public ICollection<SalesOrder> SalesOrders { get; set; }
        //public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
