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

namespace DIS.Data.DataContract
{
    /// <summary>
    /// KeyState DataContract
    /// </summary>
    public enum KeyState
    {
        Invalid = 0,

        Fulfilled = 1,
        Consumed = 2,
        Bound = 3,
        NotifiedBound = 4,
        Returned = 5,
        ReportedBound = 7,
        ReportedReturn = 8,
        ActivationEnabled = 9,
        ActivationDenied = 10,

        Assigned = 11,
        Retrieved = 12,

        ActivationEnabledPendingUpdate = 13,
    }
}
