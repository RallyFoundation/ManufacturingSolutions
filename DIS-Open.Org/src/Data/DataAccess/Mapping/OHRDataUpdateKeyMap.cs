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
    public class OHRDataUpdateKeyMap : EntityTypeConfiguration<OhrKey>
    {
        public OHRDataUpdateKeyMap()
        {
            // Primary Key
            this.HasKey(t => new { t.CustomerUpdateUniqueID, t.KeyId, t.Name });

            // Properties
            this.Property(t => t.KeyId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.ReasonCode)
                .HasMaxLength(4);

            this.Property(t => t.ReasonCodeDescription)
                .HasMaxLength(160);

            // Table & Column Mappings
            this.ToTable("DataUpdateReportKey");
            this.Property(t => t.CustomerUpdateUniqueID).HasColumnName("CustomerUpdateUniqueID");
            this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.ReasonCode).HasColumnName("ReasonCode");
            this.Property(t => t.ReasonCodeDescription).HasColumnName("ReasonCodeDescription");

            // Relationships
            this.HasRequired(t => t.Ohr)
                .WithMany(t => t.OhrKeys)
                .HasForeignKey(d => d.CustomerUpdateUniqueID);
            this.HasRequired(t => t.KeyInfo)
                .WithMany(t => t.OHRDataUpdateKeys)
                .HasForeignKey(d => d.KeyId);

        }
    }
}
