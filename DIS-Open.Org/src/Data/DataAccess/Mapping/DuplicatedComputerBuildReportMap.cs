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
	public class DuplicatedComputerBuildReportMap : EntityTypeConfiguration<CbrDuplicated>
	{
		public DuplicatedComputerBuildReportMap()
		{
			// Primary Key
			this.HasKey(t => t.CbrUniqueId);

			// Properties
			// Table & Column Mappings
			this.ToTable("DuplicatedComputerBuildReport");
			this.Property(t => t.CbrUniqueId).HasColumnName("CustomerReportUniqueID");
			this.Property(t => t.IsExported).HasColumnName("IsExported");

			// Relationships
			this.HasRequired(t => t.Cbr)
				.WithOptional(t => t.CbrDuplicated);
				
		}
	}
}

