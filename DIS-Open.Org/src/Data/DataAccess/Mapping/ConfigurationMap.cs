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
	public class ConfigurationMap : EntityTypeConfiguration<Configuration>
	{
		public ConfigurationMap()
		{
			// Primary Key
			this.HasKey(t => t.ConfigurationId);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(50);
				
			this.Property(t => t.Value)
				.IsRequired();
				
			this.Property(t => t.Type)
				.IsRequired()
				.HasMaxLength(50);
				
			// Table & Column Mappings
			this.ToTable("Configuration");
			this.Property(t => t.ConfigurationId).HasColumnName("ConfigurationID");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.Value).HasColumnName("Value");
			this.Property(t => t.Type).HasColumnName("Type");
		}
	}
}

