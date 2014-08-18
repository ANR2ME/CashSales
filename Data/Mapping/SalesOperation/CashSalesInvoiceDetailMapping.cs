﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Data.Mapping
{
    public class CashSalesInvoiceDetailMapping : EntityTypeConfiguration<CashSalesInvoiceDetail>
    {
        public CashSalesInvoiceDetailMapping()
        {
            HasKey(csid => csid.Id);
            HasRequired(csid => csid.CashSalesInvoice)
                .WithMany(csi => csi.CashSalesinvoiceDetails)
                .HasForeignKey(csid => csid.CashSalesInvoiceId)
                .WillCascadeOnDelete(true); // Need to be True if RetailSalesInvoice table is deleted first (ie. when CashBank table deleted leading to RetailSalesInvoice deletion also)
            HasRequired(csid => csid.PriceMutation)
                .WithMany()
                .HasForeignKey(csid => csid.PriceMutationId)
                .WillCascadeOnDelete(false);
            Ignore(csid => csid.Errors);
        }
    }
}
