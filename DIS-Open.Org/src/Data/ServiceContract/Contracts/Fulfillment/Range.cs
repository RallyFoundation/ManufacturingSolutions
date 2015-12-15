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

namespace DIS.Data.ServiceContract
{
    /// <summary>
    /// Data Contract to describe the ranges of keys within a Fulfillment
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class Range
    {
        /// <summary>
        /// Gets or sets the beginning product key ID.
        /// </summary>
        /// <value>The beginning product key ID.</value>
        [DataMember(Order = 1)]
        public long BeginningProductKeyID { get; set; }

        /// <summary>
        /// Gets or sets the ending product key ID.
        /// </summary>
        /// <value>The ending product key ID.</value>
        [DataMember(Order = 2)]
        public long EndingProductKeyID { get; set; }
    }
}