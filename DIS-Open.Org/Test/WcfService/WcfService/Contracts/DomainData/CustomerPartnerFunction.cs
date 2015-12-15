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
using System.Diagnostics;
using System.Runtime.Serialization;

namespace WcfService.Contracts.DomainData
{
    [DataContract(Namespace = "http://schemas.ms.it.oem/digitaldistribution/2010/10")]
    public class CustomerPartnerFunction
    {
        /// <summary>   
        /// Gets or sets the customer NBR.
        /// </summary>
        /// <value>The customer NBR.</value>
        [DataMember(Order = 1)]
        public string CustomerNumber
        {
            get;
            set;
        }        
   
        /// <summary>
        /// Gets or sets the partner function code.
        /// </summary>
        /// <value>The partner function code.</value>
        [DataMember(Order = 2)]
        public string PartnerFunctionCode
        {
            get;
            set;
        }       

        /// <summary>
        /// Gets or sets the partner customer NBR.
        /// </summary>
        /// <value>The partner customer NBR.</value>
        [DataMember(Order = 3)]
        public string PartnerCustomerNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <returns></returns>
        public static string GetKey()
        {
            return (new StackFrame().GetMethod().DeclaringType.ToString());
        }
    }
}
