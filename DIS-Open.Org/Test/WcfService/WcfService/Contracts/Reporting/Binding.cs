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

using System.Runtime.Serialization;

namespace WcfService.Contracts
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class Binding
    {
        [DataMember(Order = 1)]
        public long ProductKeyID { get; set; }

        [DataMember(Order = 2)]
        public string HardwareHash { get; set; }

        [DataMember(Order = 3)]
        public OEMOptionalInfoExtendedProperty[] OEMOptionalInfo { get; set; }
    }
}