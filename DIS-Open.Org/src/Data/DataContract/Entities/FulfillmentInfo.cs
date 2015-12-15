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
using DIS.Common.Utility;

namespace DIS.Data.DataContract
{
    public class FulfillmentInfo
    {
        public string FulfillmentNumber { get; set; }
        public string SoldToCustomerId { get; set; }
        public System.Guid OrderUniqueId { get; set; }
        public byte FulfillmentStatusId { get; set; }
        public string ResponseData
        {
            get { return Keys == null ? null : Keys.ToDataContract(); }
            set { Keys = string.IsNullOrEmpty(value) ? null : value.FromDataContract<List<KeyInfo>>(); }
        }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime ModifiedDateUtc { get; set; }
        public string Tags { get; set; }

        [NotMapped]
        public DateTime ModifiedDate
        {
            get
            {
                DateTimeOffset offset = new DateTimeOffset(ModifiedDateUtc, TimeZoneInfo.Utc.GetUtcOffset(ModifiedDateUtc));
                return offset.LocalDateTime;
            }
        }
        [NotMapped]
        public FulfillmentStatus FulfillmentStatus
        {
            get { return (FulfillmentStatus)FulfillmentStatusId; }
            set { FulfillmentStatusId = (byte)value; }
        }
        [NotMapped]
        public List<KeyInfo> Keys { get; set; }
    }

    public class FulfillmentComparer : IEqualityComparer<FulfillmentInfo>
    {
        public bool Equals(FulfillmentInfo x, FulfillmentInfo y)
        {
            return x.FulfillmentNumber == y.FulfillmentNumber;
        }

        public int GetHashCode(FulfillmentInfo obj)
        {
            return obj.FulfillmentNumber.GetHashCode();
        }
    }
}

