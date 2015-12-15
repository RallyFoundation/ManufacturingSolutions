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

using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Mapping
{
    public class KeyInfoExMap : EntityTypeConfiguration<KeyInfoEx>
    {
        public KeyInfoExMap()
        {
            // Primary Key
            this.HasKey(t => t.KeyId);

            // Table & Column Mappings
            this.ToTable("KeyInfoEx");
            this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
            this.Property(t => t.KeyTypeId).HasColumnName("KeyType");
            this.Property(t => t.SsId).HasColumnName("SSID");
            this.Property(t => t.HqId).HasColumnName("HQID");
            this.Property(t => t.IsInProgress).HasColumnName("IsInProgress");
            this.Property(t => t.ShouldCarbonCopy).HasColumnName("ShouldCarbonCopy");

            // Relationships
            this.HasRequired(t => t.KeyInfo)
                .WithOptional(t => t.KeyInfoEx);

            this.HasOptional(t => t.Subsidiary)
                .WithMany(t => t.KeyInfoExes)
                .HasForeignKey(d => d.SsId);

            this.HasOptional(t => t.HeadQuarter)
                .WithMany(t => t.KeyInfoExes)
                .HasForeignKey(d => d.HqId);
        }
    }
}

