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
	public class HeadQuarterMap : EntityTypeConfiguration<HeadQuarter>
	{
		public HeadQuarterMap()
		{
			// Primary Key
			this.HasKey(t => t.HeadQuarterId);

			// Properties
			this.Property(t => t.DisplayName)
				.HasMaxLength(20);

            this.Property(t => t.CertSubject)
                .HasMaxLength(128);
				
			this.Property(t => t.ServiceHostUrl)
				.HasMaxLength(200);
				
			this.Property(t => t.UserName)
				.HasMaxLength(10);
				
			this.Property(t => t.AccessKey)
				.HasMaxLength(50);
				
			this.Property(t => t.Description)
				.HasMaxLength(50);
				
			// Table & Column Mappings
			this.ToTable("HeadQuarter");
			this.Property(t => t.HeadQuarterId).HasColumnName("HeadQuarterID");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.CertSubject).HasColumnName("CertSubject");
            this.Property(t => t.CertThumbPrint).HasColumnName("CertThumbPrint");
			this.Property(t => t.ServiceHostUrl).HasColumnName("ServiceHostUrl");
			this.Property(t => t.UserName).HasColumnName("UserName");
			this.Property(t => t.AccessKey).HasColumnName("AccessKey");
			this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.IsCarbonCopy).HasColumnName("IsCarbonCopy");
            this.Property(t => t.IsCentralizedMode).HasColumnName("IsCentralizedMode");
		}
	}
}

