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

namespace WcfService.Contracts
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ComputerBuildReportAck
    {
        [DataMember(Order = 1)]
        public Guid MSReportUniqueID { get; set; }

        [DataMember(Order = 2)]
        public Guid CustomerReportUniqueID { get; set; }

        [DataMember(Order = 3)]
        public DateTime MSReceivedDateUTC { get; set; }

        [DataMember(Order = 4)]
        public string SoldToCustomerID { get; set; }

        [DataMember(Order = 5)]
        public string ReceivedFromCustomerID { get; set; }

        [DataMember(Order = 6)]
        public int CBRAckFileTotal { get; set; }

        [DataMember(Order = 7)]
        public int CBRAckFileNumber { get; set; }

        [DataMember(Order = 8)]
        public FailedValidationResult[] FailedValidations { get; set; }

        [DataMember(Order = 9)]
        public SuccessfulValidationResult[] SuccessfulValidations { get; set; }
    }
}
