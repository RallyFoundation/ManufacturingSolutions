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
	public class LogMap : EntityTypeConfiguration<Log>
	{
		public LogMap()
		{
			// Primary Key
			this.HasKey(t => t.LogId);

			// Properties
			this.Property(t => t.SeverityName)
				.IsRequired()
				.HasMaxLength(32);
				
			this.Property(t => t.Title)
				.IsRequired()
				.HasMaxLength(256);
				
			this.Property(t => t.MachineName)
				.IsRequired()
				.HasMaxLength(32);
				
			this.Property(t => t.AppDomainName)
				.IsRequired()
				.HasMaxLength(512);
				
			this.Property(t => t.ProcessId)
				.IsRequired()
				.HasMaxLength(256);
				
			this.Property(t => t.ProcessName)
				.IsRequired()
				.HasMaxLength(512);
				
			this.Property(t => t.ThreadName)
				.HasMaxLength(512);
				
			this.Property(t => t.Win32ThreadId)
				.HasMaxLength(128);
				
			this.Property(t => t.Message)
				.HasMaxLength(1500);
				
			// Table & Column Mappings
			this.ToTable("Log");
			this.Property(t => t.LogId).HasColumnName("LogID");
			this.Property(t => t.EventId).HasColumnName("EventID");
			this.Property(t => t.Priority).HasColumnName("Priority");
			this.Property(t => t.SeverityName).HasColumnName("Severity");
			this.Property(t => t.Title).HasColumnName("Title");
			this.Property(t => t.TimestampUtc).HasColumnName("Timestamp");
			this.Property(t => t.MachineName).HasColumnName("MachineName");
			this.Property(t => t.AppDomainName).HasColumnName("AppDomainName");
			this.Property(t => t.ProcessId).HasColumnName("ProcessID");
			this.Property(t => t.ProcessName).HasColumnName("ProcessName");
			this.Property(t => t.ThreadName).HasColumnName("ThreadName");
			this.Property(t => t.Win32ThreadId).HasColumnName("Win32ThreadId");
			this.Property(t => t.Message).HasColumnName("Message");
			this.Property(t => t.FormattedMessage).HasColumnName("FormattedMessage");
		}
	}
}

