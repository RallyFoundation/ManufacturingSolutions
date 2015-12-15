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
    /// Binding Report Status class the revelant fields that the caller has to populate and submit.
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class ReportStatus
    {
        /// <summary>
        /// Get or set Status
        /// </summary>
        [DataMember(Order = 1)]
        public int Status { get; set; }

        /// <summary>
        /// Get or set KeyStatus
        /// </summary>
        [DataMember(Order = 2)]
        public KeyStatus[] Results { get; set; }

        /// <summary>
        /// Get or set Error Code
        /// </summary>
        [DataMember(Order = 3)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Get or set Error Message
        /// </summary>
        [DataMember(Order = 4)]
        public string ErrorMessage { get; set; }
    }
}
