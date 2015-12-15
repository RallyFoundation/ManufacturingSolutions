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
    public class ComputerBuildReportMap : EntityTypeConfiguration<Cbr>
    {
        public ComputerBuildReportMap()
        {
            // Primary Key
            this.HasKey(t => t.CbrUniqueId);

            // Properties
            this.Property(t => t.SoldToCustomerId)
                .IsRequired()
                .HasMaxLength(10);
                
            this.Property(t => t.ReceivedFromCustomerId)
                .IsRequired()
                .HasMaxLength(10);
                
            // Table & Column Mappings
            this.ToTable("ComputerBuildReport");
            this.Property(t => t.MsReportUniqueId).HasColumnName("MSReportUniqueID");
            this.Property(t => t.CbrUniqueId).HasColumnName("CustomerReportUniqueID");
            this.Property(t => t.MsReceivedDateUtc).HasColumnName("MSReceivedDateUTC");
            this.Property(t => t.SoldToCustomerId).HasColumnName("SoldToCustomerID");
            this.Property(t => t.ReceivedFromCustomerId).HasColumnName("ReceivedFromCustomerID");
            this.Property(t => t.CbrAckFileTotal).HasColumnName("CBRAckFileTotal");
            this.Property(t => t.CbrAckFileNumber).HasColumnName("CBRAckFileNumber");
            this.Property(t => t.CbrStatusId).HasColumnName("CBRStatus");
            this.Property(t => t.CreatedDateUtc).HasColumnName("CreatedDateUTC");
            this.Property(t => t.ModifiedDateUtc).HasColumnName("ModifiedDateUTC");
        }
    }
}

