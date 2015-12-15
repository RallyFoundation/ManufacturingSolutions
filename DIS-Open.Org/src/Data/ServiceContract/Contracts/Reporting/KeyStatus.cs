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
    /// KeyStatus class is used to populate the KeyStatus relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class KeyStatus
    {
        /// <summary>
        /// Get or set ProductKey ID
        /// </summary>
        [DataMember(Order = 1)]
        public string ProductKeyID { get; set; }

        /// <summary>
        /// Get or set Error Code
        /// </summary>
        [DataMember(Order = 2)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Get or set Error Message
        /// </summary>
        [DataMember(Order = 3)]
        public string ErrorMessage { get; set; }
    }
}
