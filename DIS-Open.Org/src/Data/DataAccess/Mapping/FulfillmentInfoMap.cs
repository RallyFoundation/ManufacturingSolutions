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
	public class FulfillmentInfoMap : EntityTypeConfiguration<FulfillmentInfo>
	{
		public FulfillmentInfoMap()
		{
			// Primary Key
			this.HasKey(t => t.FulfillmentNumber);

			// Properties
			this.Property(t => t.FulfillmentNumber)
				.IsRequired()
				.IsFixedLength()
				.HasMaxLength(10);
				
			this.Property(t => t.SoldToCustomerId)
				.IsRequired()
				.IsFixedLength()
				.HasMaxLength(10);

            this.Property(t => t.Tags)
                .HasMaxLength(200);
				
			// Table & Column Mappings
			this.ToTable("FulfillmentInfo");
			this.Property(t => t.FulfillmentNumber).HasColumnName("FulfillmentNumber");
			this.Property(t => t.SoldToCustomerId).HasColumnName("SoldToCustomerID");
			this.Property(t => t.OrderUniqueId).HasColumnName("OrderUniqueID");
			this.Property(t => t.FulfillmentStatusId).HasColumnName("FulfillmentStatus");
			this.Property(t => t.ResponseData).HasColumnName("ResponseData");
            this.Property(t => t.CreatedDateUtc).HasColumnName("CreatedDateUTC");
            this.Property(t => t.ModifiedDateUtc).HasColumnName("ModifiedDateUTC");
            this.Property(t => t.Tags).HasColumnName("Tags");
		}
	}
}

