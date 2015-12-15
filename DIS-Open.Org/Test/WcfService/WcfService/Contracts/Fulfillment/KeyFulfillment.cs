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

namespace WcfService.Contracts {
    /// <summary>
    /// Data Contract for a single Fulfillment Record
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class KeyFulfillment
    {
        [DataMember(Order = 1)]
        public Guid OrderUniqueID { get; set; }

        [DataMember(Order = 2)]
        public string MSOrderNumber { get; set; }

        [DataMember(Order = 3)]
        public int MSOrderLineNumber { get; set; }

        [DataMember(Order = 4)]
        public string OEMPartNumber { get; set; }

        [DataMember(Order = 5)]
        public string OEMPONumber { get; set; }

        [DataMember(Order = 6)]
        public DateTime OEMPODateUTC { get; set; }

        [DataMember(Order = 7)]
        public string SoldToCustomerID { get; set; }

        [DataMember(Order = 8)]
        public string SoldToCustomerName { get; set; }

        [DataMember(Order = 9)]
        public string ShipToCustomerID { get; set; }

        [DataMember(Order = 10)]
        public string ShipToCustomerName { get; set; }

        [DataMember(Order = 11)]
        public string CallOffReferenceNumber { get; set; }

        [DataMember(Order = 12)]
        public string LicensablePartNumber { get; set; }

        [DataMember(Order = 13)]
        public string LicensableName { get; set; }

        [DataMember(Order = 14)]
        public string OEMPOLineNumber { get; set; }

        [DataMember(Order = 15)]
        public string CallOffLineNumber { get; set; }

        [DataMember(Order = 16)]
        public bool FulfillmentResendIndicator { get; set; }

        [DataMember(Order = 17)]
        public string FulfillmentNumber { get; set; }

        [DataMember(Order = 18)]
        public DateTime FulfilledDateUTC { get; set; }

        [DataMember(Order = 19)]
        public DateTime FulfillmentCreateDateUTC { get; set; }

        [DataMember(Order = 20)]
        public string EndItemPartNumber { get; set; }

        [DataMember(Order = 21)]
        public int Quantity { get; set; }

        [DataMember(Order = 22)]
        public Range[] Ranges { get; set; }

        [DataMember(Order = 23)]
        public Key[] Keys { get; set; }
    }
}