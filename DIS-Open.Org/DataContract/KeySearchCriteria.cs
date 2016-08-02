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
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace DIS.Data.DataContract
{
    /// <summary>
    /// Search Criteria common class
    /// </summary>
    public class KeySearchCriteria : SearchCriteriaBase, ICloneable
    {
        public long KeyId
        {
            set
            {
                KeyIds = new List<long> { value };
            }
        }

        public KeyState KeyState
        {
            set
            {
                KeyStateIds = new List<byte> { (byte)value };
            }
        }
       
        public List<KeyState> KeyStates
        {
            set
            {
                KeyStateIds = value.Select(s => (byte)s).ToList();
            }
        }
       
        public List<long> KeyIds { get; set; }

        public List<byte> KeyStateIds { get; private set; }

        public string ProductKeyID { get; set; }

        public long? ProductKeyIDFrom { get; set; }

        public long? ProductKeyIDTo { get; set; }

        public string ProductKey { get; set; }

        public string HardwareHash { get; set; }

        public bool HasHardwareHash { get; set; }
       
        public string MsPartNumber { get; set; }

        /// <summary>
        /// OEM Part Number
        /// </summary>
        public string OemPartNumber { get; set; }

        public bool HasOemPartNumberNull { get; set; }
        /// <summary>
        /// Order Number
        /// </summary>
        public string MsOrderNumber { get; set; }

        /// <summary>
        /// OEM PO Number
        /// </summary>
        public string OemPoNumber { get; set; }

        public string ReferenceNumber { get; set; }

        public string ZPC_MODEL_SKU { get; set; }

        public string ZOEM_EXT_ID { get; set; }

        public string ZMAUF_GEO_LOC { get; set; }

        public string ZPGM_ELIG_VALUES { get; set; }

        public string ZCHANNEL_REL_ID { get; set; }

        public string ZFRM_FACTOR_CL1 { get; set; }

        public string ZFRM_FACTOR_CL2 { get; set; }

        public string ZSCREEN_SIZE { get; set; }

        public string ZTOUCH_SCREEN { get; set; }

        public string TrakingInfo { get; set; }

        public string OemRmaNumber { get; set; }

        /// <summary>
        /// OemRmaDate from
        /// </summary>
        public DateTime? OemRmaDateFrom { get; set; }

        /// <summary>
        /// OemRmaDate to
        /// </summary>
        public DateTime? OemRmaDateTo { get; set; }

        public ReturnReportStatus? ReturnReportStatus { get; set; }

        public CbrStatus? CbrStatus { get; set; }

        /// <summary>
        /// Tpi or FactoryLine ID
        /// </summary>
        public int? SsId { get; set; }

        public int? HqId { get; set; }

        public bool? IsAssign { get; set; }

        /// <summary>
        /// Is key state synced
        /// </summary>
        public bool? IsInProgress { get; set; }

        public bool? HasNoCredit { get; set; }

        public KeyType? KeyType { get; set; }

        public bool ShouldIncludeHistories { get; set; }

        public bool? ShouldCarbonCopy { get; set; }

        public bool ShouldIncludeReturnReport { get; set; }

        public bool? HasOhrData { get; set; }

        public string SerialNumber { get; set; }

        public KeySearchCriteria()
        {
            SortBy = "KeyId";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public DateTime? OemRmaDateFromUtc
        {
            get
            {
                if (OemRmaDateFrom == null)
                    return null;
                else
                    return OemRmaDateFrom.Value.ToUniversalTime();
            }
        }

        public DateTime? OemRmaDateToUtc
        {
            get
            {
                if (OemRmaDateTo == null)
                    return null;
                else
                    return OemRmaDateTo.Value.ToUniversalTime();
            }
        }

    }
}
