﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class BarringOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int WarehouseId { get; set; }
        public string Code { get; set; }

        public int QuantityOrdered { get; set; }
        public int QuantityRejected { get; set; }
        public int QuantityFinal { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> ConfirmationDate { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsJobScheduled { get; set; }
        public bool IsFinished { get; set; }
        public Nullable<DateTime> LastFinishDate { get; set; }
        public bool IsDelivered { get; set; }
        public Nullable<DateTime> LastDeliveryDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<BarringOrderDetail> BarringOrderDetails { get; set; }
    }
}