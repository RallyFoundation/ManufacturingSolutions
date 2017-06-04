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
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DIS.Data.DataContract
{
    [DataContract]
    public class ReturnReportKey
    {
        public System.Guid CustomerReturnUniqueId { get; set; }
        [DataMember]
        public int OemRmaLineNumber { get; set; }
        [DataMember]
        public string ReturnTypeId { get; set; }
        [DataMember]
        public long KeyId { get; set; }
        [DataMember]
        public Nullable<int> MsReturnLineNumber { get; set; }
        [DataMember]
        public string LicensablePartNumber { get; set; }
        [DataMember]
        public string ReturnReasonCode { get; set; }
        [DataMember]
        public string ReturnReasonCodeDescription { get; set; }
        public byte PreProductKeyStateId { get; set; }
        public ReturnReport ReturnReport { get; set; }
        public KeyInfo KeyInfo { get; set; }
    }
}
