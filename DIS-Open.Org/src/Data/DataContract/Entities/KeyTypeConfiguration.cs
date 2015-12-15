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

namespace DIS.Data.DataContract {
    public class KeyTypeConfiguration 
    {
        public int KeyTypeConfigurationId { get; set; }
        public int? HeadQuarterId { get; set; }
        public string LicensablePartNumber { get; set; }
        public int? Maximum { get; set; }
        public int? Minimum { get; set; }
        public Nullable<int> KeyTypeId { get; set; }
        [NotMapped]
        public KeyType? KeyType
        {
            get { return (KeyType?)KeyTypeId; }
            set { KeyTypeId = (int?)value; }
        }
        [NotMapped]
        public int AvailiableKeysCount { get; set; }
    }
}
