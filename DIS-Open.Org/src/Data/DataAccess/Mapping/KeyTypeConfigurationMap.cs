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
    public class KeyTypeConfigurationMap : EntityTypeConfiguration<KeyTypeConfiguration>
    {
       public KeyTypeConfigurationMap() 
       {
           //// Primary Key
           this.HasKey(t => new {t.KeyTypeConfigurationId});
           
           // Properties
           this.Property(t => t.LicensablePartNumber)
               .IsRequired()
               .HasMaxLength(16);

           // Table & Column Mappings
           this.ToTable("KeyTypeConfiguration");
           this.Property(t => t.KeyTypeConfigurationId).HasColumnName("KeyTypeConfigurationId");
           this.Property(t => t.HeadQuarterId).HasColumnName("HeadQuarterId");
           this.Property(t => t.LicensablePartNumber).HasColumnName("LicensablePartNumber");
           this.Property(t => t.Maximum).HasColumnName("Maximum");
           this.Property(t => t.Minimum).HasColumnName("Minimum");
           this.Property(t => t.KeyTypeId).HasColumnName("KeyType");
       }
    }
}
