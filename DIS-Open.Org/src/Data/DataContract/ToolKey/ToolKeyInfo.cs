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

namespace DIS.Data.DataContract.OA3ToolKeyInfo
{
   public class Key
    {
       public string ProductKey { get; set; }
       public long ProductKeyID { get; set; }
       public byte ProductKeyState { get; set; }
       public string ProductKeyPartNumber { get; set; }

       //To support serial number mapping - Rally Sept. 22, 2014
       public string SerialNumber { get; set; }
    }
}
