using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class ValidComb
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ClosingId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public virtual Account Account { get; set; }
        public virtual Closing Closing { get; set; }
    }
}
