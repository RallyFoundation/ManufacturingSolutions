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
	public class SubsidiaryMap : EntityTypeConfiguration<Subsidiary>
	{
		public SubsidiaryMap()
		{
			// Primary Key
			this.HasKey(t => t.SsId);

			// Properties
			this.Property(t => t.DisplayName)
				.HasMaxLength(20);

            this.Property(t => t.ServiceHostUrl)
                .HasMaxLength(200);

            this.Property(t => t.UserName)
                .HasMaxLength(10);
	
			this.Property(t => t.AccessKey)
				.HasMaxLength(50);
				
			this.Property(t => t.Type)
				.IsRequired()
				.HasMaxLength(20);
				
			this.Property(t => t.Description)
				.HasMaxLength(50);
				
			// Table & Column Mappings
			this.ToTable("Subsidiary");
			this.Property(t => t.SsId).HasColumnName("SSID");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.ServiceHostUrl).HasColumnName("ServiceHostUrl");
            this.Property(t => t.UserName).HasColumnName("UserName");
			this.Property(t => t.AccessKey).HasColumnName("AccessKey");
			this.Property(t => t.Type).HasColumnName("Type");
			this.Property(t => t.Description).HasColumnName("Description");
		}
	}
}

