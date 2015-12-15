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
    /// Contract class is used to populate the contract relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class Contract
    {
        /// <summary>
        /// Gets or Sets the Ordering Customer Agreement Number
        /// </summary>
        [DataMember(Order = 1)]
        public string ContractNumber { get; set; }
        /// <summary>
        /// Gets or Sets the Agreement Type Code (i.e. OSRY)
        /// </summary>
        [DataMember(Order = 2)]
        public string ContractType { get; set; }
        /// <summary>
        /// Gets or Sets the Agreement Type Name
        /// </summary>
        [DataMember(Order = 3)]
        public string ContractTypeName { get; set; }
        /// <summary>
        /// Gets or Sets the Agreement Start Date
        /// </summary>
        [DataMember(Order = 4)]
        public DateTime EffectiveStartDate { get; set; }
        /// <summary>
        /// Gets or Sets the Agreement End Date
        /// </summary>
        [DataMember(Order = 5)]
        public DateTime EffectiveEndDate { get; set; }
        /// <summary>
        /// Gets or Sets the Agreement Status Code (i.e., 01, 6A)
        /// </summary>
        [DataMember(Order = 6)]
        public string ContractStatus { get; set; }
        /// <summary>
        /// Gets or Sets the ContractLicensables
        /// </summary>
        [DataMember(Order = 7)]
        public ContractLicensable[] ContractLicensables { get; set; }
    }
}
