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
	public class DuplicatedKeyMap : EntityTypeConfiguration<KeyDuplicated>
	{
		public DuplicatedKeyMap()
		{
			// Primary Key
			this.HasKey(t => t.KeyDuplicatedId);

			// Properties
			this.Property(t => t.ProductKey)
				.IsRequired()
				.HasMaxLength(29);
				
			// Table & Column Mappings
			this.ToTable("DuplicatedKey");
			this.Property(t => t.KeyDuplicatedId).HasColumnName("DuplicatedKeyID");
			this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
			this.Property(t => t.ProductKey).HasColumnName("ProductKey");
			this.Property(t => t.Handled).HasColumnName("Handled");
			this.Property(t => t.OperationId).HasColumnName("OperationID");

			// Relationships
			this.HasOptional(t => t.KeyOperationHistory)
				.WithMany(t => t.KeysDuplicated)
				.HasForeignKey(d => d.OperationId);
				
			this.HasRequired(t => t.KeyInfo)
				.WithMany(t => t.KeysDuplicated)
				.HasForeignKey(d => d.KeyId);
				
		}
	}
}

