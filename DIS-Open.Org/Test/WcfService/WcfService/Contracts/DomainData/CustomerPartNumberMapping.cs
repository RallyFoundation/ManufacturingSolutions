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

#region Declaratives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
#endregion

namespace WcfService.Contracts.DomainData
{
    /// <summary>
    /// CustomerPartNumberMapping class is used to populate the CustomerPartNumberMapping relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class CustomerPartNumberMapping
    {
        /// <summary>
        /// Gets or Sets the Customer OEM SAP Number
        /// </summary>
        [DataMember(Order = 1)]
        public string CustomerNumber { get; set; }
        /// <summary>
        /// Gets or Sets the Customer Part Number
        /// </summary>
        [DataMember(Order = 2)]
        public string OEMPartNumber { get; set; }
        /// <summary>
        /// Gets or Sets the Customer Part Name
        /// </summary>
        [DataMember(Order = 3)]
        public string OEMPartName { get; set; }
        /// <summary>
        /// Gets or Sets the MS Licensable Part Number
        /// </summary>
        [DataMember(Order = 4)]
        public string MSPartNumber { get; set; }
        /// <summary>
        /// Gets or Sets the MS Licensable Part Name
        /// </summary>
        [DataMember(Order = 5)]
        public string MSPartName { get; set; }
        /// <summary>
        /// Gets or Sets the Effective Start Date of Mapping
        /// </summary>
        [DataMember(Order = 6)]
        public DateTime EffectiveStartDate { get; set; }
        /// <summary>
        /// Gets or Sets the Effective End Date of Mapping
        /// </summary>
        [DataMember(Order = 7)]
        public DateTime EffectiveEndDate { get; set; }
    }
}
