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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DIS.Data.DataContract
{
    [DataContract]
    public partial class KeyInfo
    {
        public KeyInfo()
        {
            this.OemOptionalInfo = new OemOptionalInfo();
            this.KeysDuplicated = new List<KeyDuplicated>();
            this.KeyHistories = new List<KeyHistory>();
            this.KeyOperationHistories = new List<KeyOperationHistory>();
            this.CbrKeys = new List<CbrKey>();
            this.ReturnReportKeys = new List<ReturnReportKey>();
            this.OHRDataUpdateKeys = new List<OhrKey>();
        }

        [DataMember, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long KeyId { get; set; }
        [DataMember]
        public string ProductKey { get; set; }
        public byte KeyStateId { get { return (byte)KeyState; } private set { KeyState = (KeyState)value; } }
        public string KeyStateName { get { return KeyState.ToString(); } private set { Enum.Parse(typeof(KeyState), value); } }
        [DataMember]
        public string HardwareHash { get; set; }
        [DataMember]
        public string OemPartNumber { get; set; }
        [DataMember]
        public string SoldToCustomerName { get; set; }
        [DataMember]
        public Nullable<System.Guid> OrderUniqueId { get; set; }
        [DataMember]
        public string SoldToCustomerId { get; set; }
        [DataMember]
        public string CallOffReferenceNumber { get; set; }
        [DataMember]
        public string OemPoNumber { get; set; }
        [DataMember]
        public string MsOrderNumber { get; set; }
        [DataMember]
        public string LicensablePartNumber { get; set; }
        [DataMember]
        public Nullable<int> Quantity { get; set; }
        [DataMember]
        public string SkuId { get; set; }
        [DataMember]
        public string ReturnReasonCode { get; set; }
        [DataMember]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [DataMember]
        public Nullable<int> MsOrderLineNumber { get; set; }
        [DataMember]
        public Nullable<System.DateTime> OemPoDateUtc { get; set; }
        [DataMember]
        public string ShipToCustomerId { get; set; }
        [DataMember]
        public string ShipToCustomerName { get; set; }
        [DataMember]
        public string LicensableName { get; set; }
        [DataMember]
        public string OemPoLineNumber { get; set; }
        [DataMember]
        public string CallOffLineNumber { get; set; }
        [DataMember]
        public Nullable<bool> FulfillmentResendIndicator { get; set; }
        [DataMember]
        public string FulfillmentNumber { get; set; }
        [DataMember]
        public Nullable<System.DateTime> FulfilledDateUtc { get; set; }
        [DataMember]
        public Nullable<System.DateTime> FulfillmentCreateDateUtc { get; set; }
        [DataMember]
        public string EndItemPartNumber { get; set; }
        [DataMember]
        public string TrackingInfo { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        public string ZPC_MODEL_SKU { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZPC_MODEL_SKU; } set { OemOptionalInfo.ZPC_MODEL_SKU = value; } }
        public string ZOEM_EXT_ID { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZOEM_EXT_ID; } set { OemOptionalInfo.ZOEM_EXT_ID = value; } }
        public string ZMANUF_GEO_LOC { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZMANUF_GEO_LOC; } set { OemOptionalInfo.ZMANUF_GEO_LOC = value; } }
        public string ZPGM_ELIG_VALUES { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZPGM_ELIG_VALUES; } set { OemOptionalInfo.ZPGM_ELIG_VALUES = value; } }
        public string ZCHANNEL_REL_ID { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZCHANNEL_REL_ID; } set { OemOptionalInfo.ZCHANNEL_REL_ID = value; } }
        
        public string ZFRM_FACTOR_CL1 { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZFRM_FACTOR_CL1; } set { OemOptionalInfo.ZFRM_FACTOR_CL1 = value; } }
        public string ZFRM_FACTOR_CL2 { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZFRM_FACTOR_CL2; } set { OemOptionalInfo.ZFRM_FACTOR_CL2 = value; } }
        public string ZSCREEN_SIZE { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZSCREEN_SIZE; } set { OemOptionalInfo.ZSCREEN_SIZE = value; } }
        public string ZTOUCH_SCREEN { get { return OemOptionalInfo == null ? null : OemOptionalInfo.ZTOUCH_SCREEN; } set { OemOptionalInfo.ZTOUCH_SCREEN = value; } }
        public string Tags { get; set; }
        public string Description { get; set; }
        
        public ICollection<KeyDuplicated> KeysDuplicated { get; set; }
        public ICollection<KeyHistory> KeyHistories { get; set; }
        [DataMember]
        public KeyInfoEx KeyInfoEx { get; set; }
        public ICollection<CbrKey> CbrKeys { get; set; }
        public ICollection<ReturnReportKey> ReturnReportKeys { get; set; }
        public ICollection<KeyOperationHistory> KeyOperationHistories { get; set; }
        public ICollection<OhrKey> OHRDataUpdateKeys { get; set; }

        [DataMember, NotMapped]
        public KeyState KeyState { get;  set; }

        [DataMember, NotMapped]
        public OemOptionalInfo OemOptionalInfo { get; set; }

        [NotMapped]
        public bool IsCentralized {
            get {
                return SoldToCustomerId == ShipToCustomerId;
            }
        }

        [NotMapped]
        public DateTime FulfilledDate
        {
            get
            {
                DateTimeOffset offset = new DateTimeOffset(FulfilledDateUtc.Value, TimeZoneInfo.Utc.GetUtcOffset(FulfilledDateUtc.Value));
                return offset.LocalDateTime;
            }
        }

        public void UpdateOhrData(string name, string value)
        {
            switch (name)
            {
                case OemOptionalInfo.ZFrmFactorCl1Name:
                    ZFRM_FACTOR_CL1 = value;
                    break;
                case OemOptionalInfo.ZFrmFactorCl2Name:
                    ZFRM_FACTOR_CL2 = value;
                    break;
                case OemOptionalInfo.ZTouchScreenName:
                    ZTOUCH_SCREEN = value;
                    break;
                case OemOptionalInfo.ZScreenSizeName:
                    ZSCREEN_SIZE = value;
                    break;
                case OemOptionalInfo.ZPcModelSkuName:
                    ZPC_MODEL_SKU = value;
                    break;
            }
        }
    }

    public class KeyInfoComparer : IEqualityComparer<KeyInfo> {
        public bool Equals(KeyInfo x, KeyInfo y) {
            return x.KeyId == y.KeyId;
        }

        public int GetHashCode(KeyInfo obj) {
            return obj.ProductKey.GetHashCode();
        }
    }
}

