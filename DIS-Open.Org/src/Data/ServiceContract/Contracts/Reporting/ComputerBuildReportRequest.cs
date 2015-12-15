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
using System;

namespace DIS.Data.ServiceContract
{
    /// <summary>
    /// BindingReportRequest class the revelant fields that the caller has to populate and submit.
    /// </summary>
    [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ComputerBuildReportRequest 
    {
        /// <summary>
        /// Gets or Sets the SoldTo CustomerID
        /// </summary>
        [DataMember(Order = 1)]
        public Guid CustomerReportUniqueID { get; set; }

        /// <summary>
        /// Gets or Sets the Businessname
        /// </summary>
        [DataMember(Order = 2)]
        public string SoldToCustomerID { get; set; }

        /// <summary>
        /// Gets or Sets the ReceivedFrom CustomerID
        /// </summary>
        [DataMember(Order = 3)]
        public string ReceivedFromCustomerID { get; set; }

        [DataMember(Order = 4)]
        public int TotalLineItems { get; set; }

        /// <summary>
        /// Gets or Sets the Keys
        /// </summary>
        [DataMember(Order = 5)]
        public Binding[] Bindings { get; set; }
    }
}
