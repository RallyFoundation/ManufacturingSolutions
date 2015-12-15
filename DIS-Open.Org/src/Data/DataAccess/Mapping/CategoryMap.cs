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
	public class CategoryMap : EntityTypeConfiguration<Category>
	{
		public CategoryMap()
		{
			// Primary Key
			this.HasKey(t => t.CategoryId);

			// Properties
			this.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(64);
				
			// Table & Column Mappings
			this.ToTable("Category");
			this.Property(t => t.CategoryId).HasColumnName("CategoryID");
			this.Property(t => t.CategoryName).HasColumnName("CategoryName");
		}
	}
}

