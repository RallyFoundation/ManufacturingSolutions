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
	public class KeyOperationHistoryMap : EntityTypeConfiguration<KeyOperationHistory>
	{
		public KeyOperationHistoryMap()
		{
			// Primary Key
			this.HasKey(t => t.OperationId);

			// Properties
			this.Property(t => t.ProductKey)
				.IsRequired()
				.HasMaxLength(29);
				
			this.Property(t => t.HardwareHash)
				.HasMaxLength(512);
				
			this.Property(t => t.Operator)
				.IsRequired()
				.HasMaxLength(20);
				
			this.Property(t => t.Message)
				.IsRequired()
				.HasMaxLength(200);
				
			// Table & Column Mappings
			this.ToTable("KeyOperationHistory");
			this.Property(t => t.OperationId).HasColumnName("OperationID");
			this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
			this.Property(t => t.ProductKey).HasColumnName("ProductKey");
			this.Property(t => t.HardwareHash).HasColumnName("HardwareHash");
			this.Property(t => t.KeyStateFrom).HasColumnName("KeyStateFrom");
			this.Property(t => t.KeyStateTo).HasColumnName("KeyStateTo");
			this.Property(t => t.Operator).HasColumnName("Operator");
			this.Property(t => t.Message).HasColumnName("Message");
			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");

			// Relationships
			this.HasRequired(t => t.KeyInfo)
				.WithMany(t => t.KeyOperationHistories)
				.HasForeignKey(d => d.KeyId);
				
		}
	}
}

