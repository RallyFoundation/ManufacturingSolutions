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
    [Obsolete]
    public class KeyOperationSearch : SearchCriteriaBase
    {
        public string MSPartNumber { get; set; }
        public string OEMPartNumber { get; set; }
        public string OEMPONumber { get; set; }
        public Subsidiary SubSidiay { get; set; }
        public string SourceCustomerNumber { get; set; }
        public string MyCustomerNumber { get; set; }
        public List<KeyState> KeyStates { get; set; }
    }
}
