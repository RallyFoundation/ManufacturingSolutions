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
	public class RoleMap : EntityTypeConfiguration<Role>
	{
		public RoleMap()
		{
			// Primary Key
			this.HasKey(t => t.RoleId);

			// Properties
			this.Property(t => t.RoleName)
				.IsRequired()
				.HasMaxLength(20);
				
			this.Property(t => t.Description)
				.HasMaxLength(200);
				
			// Table & Column Mappings
			this.ToTable("Role");
			this.Property(t => t.RoleId).HasColumnName("RoleID");
			this.Property(t => t.RoleName).HasColumnName("RoleName");
			this.Property(t => t.Description).HasColumnName("Description");

			// Relationships
			this.HasMany(t => t.Users)
			    .WithMany(t => t.Roles)
				.Map(m =>
                    {
                        m.ToTable("UserRole");
                        m.MapLeftKey("RoleID");
                        m.MapRightKey("UserID");
                    });
					
		}
	}
}

