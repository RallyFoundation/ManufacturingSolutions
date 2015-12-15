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
using System.ComponentModel;

namespace DIS.Data.DataContract {
    [Flags]
    public enum InstallType {

        [DescriptionAttribute("OEM")]
        Oem = 1,

        [DescriptionAttribute("TPI")]
        Tpi = 2,

        [DescriptionAttribute("Factory Floor")]
        FactoryFloor = 4,

        Uls = 3,
        Dls = 6,
    }
}
