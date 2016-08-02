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
using System.Runtime.Serialization;

namespace DIS.Data.DataContract
{
    [DataContract]
	public class CbrKey
    {
        [DataMember]
		public System.Guid CustomerReportUniqueId { get; set; }
        [DataMember]
		public long KeyId { get; set; }
        [DataMember]
		public string HardwareHash { get; set; }
        [DataMember]
		public string OemOptionalInfo { get; set; }
        [DataMember]
		public string ReasonCode { get; set; }
        [DataMember]
		public string ReasonCodeDescription { get; set; }
        public Cbr Cbr { get; set; }
		public KeyInfo KeyInfo { get; set; }
	}
}

