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
	public class ComputerBuildReportKeyMap : EntityTypeConfiguration<CbrKey>
	{
		public ComputerBuildReportKeyMap()
		{
			// Primary Key
            this.HasKey(t => new { CustomerReportUniqueId = t.CustomerReportUniqueId, KeyId = t.KeyId });

			// Properties
			this.Property(t => t.HardwareHash)
				.IsRequired()
				.HasMaxLength(512);
				
			this.Property(t => t.ReasonCode)
				.HasMaxLength(2);
				
			this.Property(t => t.ReasonCodeDescription)
				.HasMaxLength(160);
				
			// Table & Column Mappings
			this.ToTable("ComputerBuildReportKey");
			this.Property(t => t.CustomerReportUniqueId).HasColumnName("CustomerReportUniqueID");
			this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
			this.Property(t => t.HardwareHash).HasColumnName("HardwareHash");
			this.Property(t => t.OemOptionalInfo).HasColumnName("OEMOptionalInfo");
			this.Property(t => t.ReasonCode).HasColumnName("ReasonCode");
			this.Property(t => t.ReasonCodeDescription).HasColumnName("ReasonCodeDescription");

             //Relationships
            this.HasRequired(t => t.Cbr)
                .WithMany(t => t.CbrKeys)
                .HasForeignKey(d => d.CustomerReportUniqueId);

            this.HasRequired(t => t.KeyInfo)
                .WithMany(t => t.CbrKeys)
                .HasForeignKey(d => d.KeyId);
				
		}
	}
}

