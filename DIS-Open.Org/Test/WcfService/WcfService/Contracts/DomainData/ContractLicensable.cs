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
    /// ContractLicensable class is used to populate the ContractLicensable relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ContractLicensable
    {
        /// <summary>
        /// Gets or Sets the Licensable Line Item No on the Agreement
        /// </summary>
        [DataMember(Order = 1)]
        public string ContractLineItem { get; set; }
        /// <summary>
        /// Gets or Sets the Licensable on the Agreement 
        /// </summary>
        [DataMember(Order = 2)]
        public string MaterialNumber { get; set; }
        /// <summary>
        /// Gets or Sets the Licensable Effective Start Date on Agreement
        /// </summary>
        [DataMember(Order = 3)]
        public DateTime EffectiveStartDate { get; set; }
        /// <summary>
        /// Gets or Sets the Licensable Effective End Date on Agreement
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime EffectiveEndDate { get; set; }
        /// <summary>
        /// Gets or Sets the Licensable Status on Agreement
        /// </summary>
        [DataMember(Order = 5)]
        public string LineItemStatus { get; set; }
        /// <summary>
        /// Gets or Sets the Unit of Measure (ex. PCO)
        /// </summary>
        [DataMember(Order = 6)]
        public string UOM { get; set; }
    }
}
