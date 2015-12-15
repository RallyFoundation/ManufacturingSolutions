//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Mapping
{
    public class OHRDataUpdateMap : EntityTypeConfiguration<Ohr>
    {
        public OHRDataUpdateMap()
        {
            // Primary Key
            this.HasKey(t => t.CustomerUpdateUniqueId);

            // Properties
            this.Property(t => t.SoldToCustomerId)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ReceivedFromCustomerId)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("DataUpdateReport");
            this.Property(t => t.MsUpdateUniqueId).HasColumnName("MSUpdateUniqueID");
            this.Property(t => t.CustomerUpdateUniqueId).HasColumnName("CustomerUpdateUniqueID");
            this.Property(t => t.MsReceivedDateUtc).HasColumnName("MSReceivedDateUTC");
            this.Property(t => t.SoldToCustomerId).HasColumnName("SoldToCustomerID");
            this.Property(t => t.ReceivedFromCustomerId).HasColumnName("ReceivedFromCustomerID");
            this.Property(t => t.TotalLineItems).HasColumnName("TotalLineItems");
            this.Property(t => t.OhrStatusId).HasColumnName("OHRStatus");
            this.Property(t => t.CreatedDateUtc).HasColumnName("CreatedDateUTC");
            this.Property(t => t.ModifiedDateUtc).HasColumnName("ModifiedDateUTC");
        }
    }
}
