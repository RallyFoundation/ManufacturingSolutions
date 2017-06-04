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
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace DIS.Data.DataContract
{
    [DataContract]
    public class KeyInfoEx
    {
        public long KeyId { get; set; }
        public Nullable<int> KeyTypeId { get; set; }
        public int? SsId { get; set; }
        public int? HqId { get; set; }
        public bool IsInProgress { get; set; }
        public bool? ShouldCarbonCopy { get; set; }
        public virtual KeyInfo KeyInfo { get; set; }
        public virtual Subsidiary Subsidiary { get; set; }
        public virtual HeadQuarter HeadQuarter { get; set; }

        [DataMember, NotMapped]
        public KeyType? KeyType
        {
            get { return (KeyType?)KeyTypeId; }
            set { KeyTypeId = (int?)value; }
        }
    }
}

