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
	public class UserHeadQuarterMap : EntityTypeConfiguration<UserHeadQuarter>
	{
		public UserHeadQuarterMap()
		{
			// Primary Key
			this.HasKey(t => new { t.UserId, t.HeadQuarterId });

			// Properties
			this.Property(t => t.UserId)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
				
			this.Property(t => t.HeadQuarterId)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
				
			// Table & Column Mappings
			this.ToTable("UserHeadQuarter");
			this.Property(t => t.UserId).HasColumnName("UserID");
			this.Property(t => t.HeadQuarterId).HasColumnName("HeadQuarterID");
			this.Property(t => t.IsDefault).HasColumnName("IsDefault");

			// Relationships
			this.HasRequired(t => t.HeadQuarter)
				.WithMany(t => t.UserHeadQuarters)
				.HasForeignKey(d => d.HeadQuarterId);
				
			this.HasRequired(t => t.User)
				.WithMany(t => t.UserHeadQuarters)
				.HasForeignKey(d => d.UserId);
				
		}
	}
}

