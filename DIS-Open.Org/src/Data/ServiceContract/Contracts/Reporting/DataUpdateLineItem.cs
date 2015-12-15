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

namespace DIS.Data.ServiceContract
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class DataUpdateLineItem
    {
        [DataMember(Order = 1)]
        public long ProductKeyID { get; set; }

        [DataMember(Order = 2)]
        public OEMOptionalInfoExtendedProperty[] OEMOptionalInfo { get; set; }

        [DataMember(Order = 3)]
        public OEMHardwareReport OEMHardwareReport { get; set; }
   }
}
