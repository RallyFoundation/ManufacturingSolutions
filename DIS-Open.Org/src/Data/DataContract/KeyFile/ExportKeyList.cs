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

namespace DIS.Data.DataContract
{
    /// <summary>
    ///Export Key Information  modified class for serialization
    /// </summary>
    public class ExportKeyList
    {
        /// <summary>
        /// key xml export from 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///  key xml export to 
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// key xml export date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// key xml export keys
        /// </summary>
        //public List<KeyInformation> Keys { get; set; }
        public List<ExportKeyInfo> Keys { get; set; }
    }
}
