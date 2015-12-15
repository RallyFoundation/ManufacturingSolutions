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
using System.Runtime.Serialization;

namespace DIS.Data.ServiceContract
{
    [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class DataUpdateRequest
    {
        [DataMember(Order = 1)]
        public Guid? CustomerUpdateUniqueID { get; set; }

        [DataMember(Order = 2)]
        public string SoldToCustomerID { get; set; }

        [DataMember(Order = 3)]
        public string ReceivedFromCustomerID { get; set; }

        [DataMember(Order = 4)]
        public int TotalLineItems { get; set; }

        [DataMember(Order = 5)]
        public DataUpdateLineItem[] DataUpdateLineItems { get; set; }
    }
}
