using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DomainModel
{
    public partial class Closing
    {
        public int Id { get; set; }
        public int Period { get; set; } // month (closing date)
        public int YearPeriod { get; set; } // year (closing period)
        public DateTime BeginningPeriod { get; set; }
        public DateTime EndDatePeriod { get; set; }

        public bool IsClosed { get; set; }
        public Nullable<DateTime> ClosedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public ICollection<ValidComb> ValidCombs { get; set; }
    }
}
