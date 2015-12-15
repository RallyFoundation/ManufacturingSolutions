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
    public class KeySyncNotificationMap : EntityTypeConfiguration<KeySyncNotification>
    {
        public KeySyncNotificationMap()
		{
			// Primary Key
            this.HasKey( t => t.KeyId);
				
			// Table & Column Mappings
            this.ToTable("KeySyncNotification");
            this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
            this.Property(t => t.KeyStateId).HasColumnName("ProductKeyStateID");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
		}
    }
}
