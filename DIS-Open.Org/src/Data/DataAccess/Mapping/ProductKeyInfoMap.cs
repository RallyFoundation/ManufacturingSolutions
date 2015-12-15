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
	public class ProductKeyInfoMap : EntityTypeConfiguration<KeyInfo>
	{
		public ProductKeyInfoMap()
		{
			// Primary Key
            this.HasKey(t => t.KeyId);

			// Properties
			this.Property(t => t.ProductKey)
				.HasMaxLength(50);
				
			this.Property(t => t.KeyStateName)
				.HasMaxLength(20);
				
			this.Property(t => t.HardwareHash)
				.HasMaxLength(512);
				
			this.Property(t => t.OemPartNumber)
				.HasMaxLength(35);
				
			this.Property(t => t.SoldToCustomerName)
				.HasMaxLength(80);
				
			this.Property(t => t.SoldToCustomerId)
				.IsFixedLength()
				.HasMaxLength(10);
				
			this.Property(t => t.CallOffReferenceNumber)
				.HasMaxLength(35);
				
			this.Property(t => t.OemPoNumber)
				.HasMaxLength(35);
				
			this.Property(t => t.MsOrderNumber)
				.HasMaxLength(10);
				
			this.Property(t => t.LicensablePartNumber)
				.HasMaxLength(16);
				
			this.Property(t => t.SkuId)
				.HasMaxLength(50);
				
			this.Property(t => t.ReturnReasonCode)
				.HasMaxLength(10);
				
			this.Property(t => t.ShipToCustomerId)
				.IsFixedLength()
				.HasMaxLength(10);
				
			this.Property(t => t.ShipToCustomerName)
				.HasMaxLength(80);
				
			this.Property(t => t.LicensableName)
				.HasMaxLength(40);
				
			this.Property(t => t.OemPoLineNumber)
				.HasMaxLength(6);
				
			this.Property(t => t.CallOffLineNumber)
				.HasMaxLength(6);
				
			this.Property(t => t.FulfillmentNumber)
				.IsFixedLength()
				.HasMaxLength(10);
				
			this.Property(t => t.EndItemPartNumber)
				.HasMaxLength(18);
				
			this.Property(t => t.ZPC_MODEL_SKU)
				.HasMaxLength(64);
				
			this.Property(t => t.ZMANUF_GEO_LOC)
				.HasMaxLength(10);
				
			this.Property(t => t.ZPGM_ELIG_VALUES)
				.HasMaxLength(48);
				
			this.Property(t => t.ZOEM_EXT_ID)
				.HasMaxLength(16);
				
			this.Property(t => t.ZCHANNEL_REL_ID)
				.HasMaxLength(32);

            this.Property(t => t.ZFRM_FACTOR_CL1)
                .HasMaxLength(64);

            this.Property(t => t.ZFRM_FACTOR_CL2)
                .HasMaxLength(64);

            this.Property(t => t.ZSCREEN_SIZE)
                .HasMaxLength(32);
            this.Property(t => t.ZTOUCH_SCREEN)
                .HasMaxLength(32);

            this.Property(t => t.TrackingInfo)
                .HasMaxLength(1024);

            this.Property(t => t.Tags)
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            this.Property(t => t.SerialNumber).HasMaxLength(36);

			// Table & Column Mappings
			this.ToTable("ProductKeyInfo");
			this.Property(t => t.KeyId).HasColumnName("ProductKeyID");
			this.Property(t => t.ProductKey).HasColumnName("ProductKey");
			this.Property(t => t.KeyStateId).HasColumnName("ProductKeyStateID");
			this.Property(t => t.KeyStateName).HasColumnName("ProductKeyState");
			this.Property(t => t.HardwareHash).HasColumnName("HardwareID");
			this.Property(t => t.OemPartNumber).HasColumnName("OEMPartNumber");
			this.Property(t => t.SoldToCustomerName).HasColumnName("SoldToCustomerName");
			this.Property(t => t.OrderUniqueId).HasColumnName("OrderUniqueID");
			this.Property(t => t.SoldToCustomerId).HasColumnName("SoldToCustomerID");
			this.Property(t => t.CallOffReferenceNumber).HasColumnName("CallOffReferenceNumber");
			this.Property(t => t.OemPoNumber).HasColumnName("OEMPONumber");
			this.Property(t => t.MsOrderNumber).HasColumnName("MSOrderNumber");
			this.Property(t => t.LicensablePartNumber).HasColumnName("LicensablePartNumber");
			this.Property(t => t.Quantity).HasColumnName("Quantity");
			this.Property(t => t.SkuId).HasColumnName("SKUID");
			this.Property(t => t.ReturnReasonCode).HasColumnName("ReturnReasonCode");
			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
			this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
			this.Property(t => t.MsOrderLineNumber).HasColumnName("MSOrderLineNumber");
			this.Property(t => t.OemPoDateUtc).HasColumnName("OEMPODateUTC");
			this.Property(t => t.ShipToCustomerId).HasColumnName("ShipToCustomerID");
			this.Property(t => t.ShipToCustomerName).HasColumnName("ShipToCustomerName");
			this.Property(t => t.LicensableName).HasColumnName("LicensableName");
			this.Property(t => t.OemPoLineNumber).HasColumnName("OEMPOLineNumber");
			this.Property(t => t.CallOffLineNumber).HasColumnName("CallOffLineNumber");
			this.Property(t => t.FulfillmentResendIndicator).HasColumnName("FulfillmentResendIndicator");
			this.Property(t => t.FulfillmentNumber).HasColumnName("FulfillmentNumber");
			this.Property(t => t.FulfilledDateUtc).HasColumnName("FulfilledDateUTC");
			this.Property(t => t.FulfillmentCreateDateUtc).HasColumnName("FulfillmentCreateDateUTC");
			this.Property(t => t.EndItemPartNumber).HasColumnName("EndItemPartNumber");
			this.Property(t => t.ZPC_MODEL_SKU).HasColumnName("ZPC_MODEL_SKU");
			this.Property(t => t.ZMANUF_GEO_LOC).HasColumnName("ZMANUF_GEO_LOC");
			this.Property(t => t.ZPGM_ELIG_VALUES).HasColumnName("ZPGM_ELIG_VALUES");
			this.Property(t => t.ZOEM_EXT_ID).HasColumnName("ZOEM_EXT_ID");
			this.Property(t => t.ZCHANNEL_REL_ID).HasColumnName("ZCHANNEL_REL_ID");
            this.Property(t => t.ZFRM_FACTOR_CL1).HasColumnName("ZFRM_FACTOR_CL1");
            this.Property(t => t.ZFRM_FACTOR_CL2).HasColumnName("ZFRM_FACTOR_CL2");
            this.Property(t => t.ZSCREEN_SIZE).HasColumnName("ZSCREEN_SIZE");
            this.Property(t => t.ZTOUCH_SCREEN).HasColumnName("ZTOUCH_SCREEN");
            this.Property(t => t.TrackingInfo).HasColumnName("TrackingInfo");
            this.Property(t => t.Tags).HasColumnName("Tags");
            this.Property(t => t.Description).HasColumnName("Description");

            this.Property(t => t.SerialNumber).HasColumnName("SerialNumber");
		}
	}
}

