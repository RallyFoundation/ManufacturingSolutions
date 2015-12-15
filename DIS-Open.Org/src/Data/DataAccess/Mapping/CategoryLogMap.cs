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
	public class CategoryLogMap : EntityTypeConfiguration<CategoryLog>
	{
		public CategoryLogMap()
		{
			// Primary Key
			this.HasKey(t => t.CategoryLogId);

			// Properties
			// Table & Column Mappings
			this.ToTable("CategoryLog");
			this.Property(t => t.CategoryLogId).HasColumnName("CategoryLogID");
			this.Property(t => t.CategoryId).HasColumnName("CategoryID");
			this.Property(t => t.LogId).HasColumnName("LogID");

			// Relationships
			this.HasRequired(t => t.Category)
				.WithMany(t => t.CategoryLogs)
				.HasForeignKey(d => d.CategoryId);
				
			this.HasRequired(t => t.Log)
				.WithMany(t => t.CategoryLogs)
				.HasForeignKey(d => d.LogId);
				
		}
	}
}

