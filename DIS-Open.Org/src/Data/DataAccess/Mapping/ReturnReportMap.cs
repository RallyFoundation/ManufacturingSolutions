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


using System.Data.Entity.ModelConfiguration;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Mapping
{
    public class ReturnReportMap : EntityTypeConfiguration<ReturnReport>
    {
        public ReturnReportMap()
        {
            // Primary Key
            this.HasKey(t => t.CustomerReturnUniqueId);

            //Properties
            this.Property(t => t.SoldToCustomerId).IsRequired().HasMaxLength(10);
            this.Property(t => t.OemRmaNumber).IsRequired().HasMaxLength(35);


            //Table & Column Mappings
            this.ToTable("ReturnReport");
            this.Property(t => t.CustomerReturnUniqueId).HasColumnName("CustomerReturnUniqueID");
            this.Property(t => t.ReturnUniqueId).HasColumnName("ReturnUniqueID");
            this.Property(t => t.MsReturnNumber).HasColumnName("MSReturnNumber");
            this.Property(t => t.ReturnDateUTC).HasColumnName("ReturnDateUTC");
            this.Property(t => t.OemRmaDateUTC).HasColumnName("OEMRMADateUTC");
            this.Property(t => t.OemRmaNumber).HasColumnName("OEMRMANumber");
            this.Property(t => t.SoldToCustomerName).HasColumnName("SoldToCustomerName");
            this.Property(t => t.SoldToCustomerId).HasColumnName("SoldToCustomerID");
            this.Property(t => t.OemRmaDate).HasColumnName("OEMRMADate");
            this.Property(t => t.ReturnNoCredit).HasColumnName("ReturnNoCredit");
            this.Property(t => t.ReturnReportStatusId).HasColumnName("ReportStatus");
        }
    }
}
