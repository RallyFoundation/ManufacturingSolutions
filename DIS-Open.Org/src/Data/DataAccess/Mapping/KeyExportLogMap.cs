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
	public class KeyExportLogMap : EntityTypeConfiguration<KeyExportLog>
	{
		public KeyExportLogMap()
		{
			// Primary Key
			this.HasKey(t => t.ExportLogId);

			// Properties	
			this.Property(t => t.ExportTo)
				.IsRequired()
				.HasMaxLength(20);
				
			this.Property(t => t.ExportType)
				.IsRequired()
				.HasMaxLength(20);
				
			this.Property(t => t.FileName)
				.IsRequired()
				.HasMaxLength(300);
				
			this.Property(t => t.FileContent)
				.IsRequired();
				
			// Table & Column Mappings
			this.ToTable("KeyExportLog");
			this.Property(t => t.ExportLogId).HasColumnName("ExportLogID");
			this.Property(t => t.ExportTo).HasColumnName("ExportTo");
			this.Property(t => t.ExportType).HasColumnName("ExportType");
			this.Property(t => t.KeyCount).HasColumnName("KeyCount");
			this.Property(t => t.FileName).HasColumnName("FileName");
			this.Property(t => t.IsEncrypted).HasColumnName("IsEncrypted");
			this.Property(t => t.FileContent).HasColumnName("FileContent");
		}
	}
}

