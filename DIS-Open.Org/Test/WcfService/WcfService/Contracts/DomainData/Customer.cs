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
    public class Customer
    {             
        /// <summary>
        /// Gets or sets the OEMSAP number.
        /// </summary>
        /// <value>The OEMSAP number.</value>
        [DataMember(Order = 1)]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the OEM.
        /// </summary>
        /// <value>The name of the OEM.</value>
        [DataMember(Order = 2)]
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the OEM address1.
        /// </summary>
        /// <value>The OEM address1.</value>
        [DataMember(Order = 3)]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the OEM address2.
        /// </summary>
        /// <value>The OEM address2.</value>
        [DataMember(Order = 4)]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the OEM city.
        /// </summary>
        /// <value>The OEM city.</value>
        [DataMember(Order = 5)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the OEM state province.
        /// </summary>
        /// <value>The OEM state province.</value>
        [DataMember(Order = 6)]
        public string StateProvince { get; set; }

        /// <summary>
        /// Gets or sets the OEM postal code.
        /// </summary>
        /// <value>The OEM postal code.</value>
        [DataMember(Order = 7)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the OEM country.
        /// </summary>
        /// <value>The OEM country.</value>
        [DataMember(Order = 8)]
        public string CountryCode { get; set; }
       
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
