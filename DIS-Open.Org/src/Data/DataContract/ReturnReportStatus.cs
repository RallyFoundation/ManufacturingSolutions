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

namespace DIS.Data.DataContract {
    public enum ReturnReportStatus
    {
        // Return Key Report was generated, but not been sent
        Generated = 0,
        // Return Key Report is reporting
        Sent = 7,
        // Return Key Report was sent and MS GUID was retrieved
        Reported = 1,
        // Return Key Report ack was ready
        Ready = 4,
        // Return Key Report Ack was received
        Completed = 2,
        // Return Key Report Ack was failed to receive
        Failed = 3,
        // Tpi should carbon copy return report data to oem
        ShouldCarbonCopy = 5,
        // Tpi carbon copy return report data finished
        CarbonCopyCompleted = 6,
    }
}
