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
    public class ExportParameters
    {
        public string OutputPath { get; set; }
        public Constants.ExportType ExportType { get; set; }
        public User CreateBy { get; set; }
        public object Keys { get; set; }
        public bool IsEncrypted { get; set; }
        public object To { get; set; }
        public string UserName { get; set; }
        public string AccessKey { get; set; }
        
        //To support export in compatible mode - Rally, Dec.16, 2014
        public bool IsInCompatibleMode { get; set; }

        public string TransformationXSLT { get; set; }
    }
}
