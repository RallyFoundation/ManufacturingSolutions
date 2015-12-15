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
    public class ReturnReportKeyMap : EntityTypeConfiguration<ReturnReportKey>
    {
        public ReturnReportKeyMap()
        {
            //Primary Key
            this.HasKey(t => new { CustomerReturnUniqueId=t.CustomerReturnUniqueId, KeyId = t.KeyId });

            //Properties

            // Table & Column Mappings
            this.ToTable("ReturnReportKey");
            this.Property(t => t.CustomerReturnUniqueId).HasColumnName("CustomerReturnUniqueID");
            this.Property(t => t.OemRmaLineNumber).HasColumnName("OEMRMALineNumber");
            this.Property(t => t.ReturnTypeId).HasColumnName("ReturnTypeID");
            this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
            this.Property(t => t.MsReturnLineNumber).HasColumnName("MSReturnLineNumber");
            this.Property(t => t.LicensablePartNumber).HasColumnName("LicensablePartNumber");
            this.Property(t => t.ReturnReasonCode).HasColumnName("ReturnReasonCode");
            this.Property(t => t.ReturnReasonCodeDescription).HasColumnName("ReturnReasonCodeDescription");
            this.Property(t => t.PreProductKeyStateId).HasColumnName("PreProductKeyStateID");

            //Relationships
            this.HasRequired(t => t.ReturnReport)
                .WithMany(t => t.ReturnReportKeys)
                .HasForeignKey(d => d.CustomerReturnUniqueId);

            this.HasRequired(t => t.KeyInfo)
                .WithMany(t => t.ReturnReportKeys)
                .HasForeignKey(d => d.KeyId);
        }
    }
}
