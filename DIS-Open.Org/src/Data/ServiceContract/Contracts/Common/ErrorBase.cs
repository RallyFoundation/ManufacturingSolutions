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
    /// Error class is used to populate the Error relevant information
    /// </summary>
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public abstract class ErrorBase
    {
        /// <summary>
        /// Gets or Sets the ErrorCode
        /// </summary>
        [DataMember(Order = 1)]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or Sets the ErrorMessage
        /// </summary>
        [DataMember(Order = 2)]
        public string ErrorMessage { get; set; }
    }
}
