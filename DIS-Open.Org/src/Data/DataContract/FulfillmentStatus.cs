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
    public enum FulfillmentStatus
    {
        // Got fulfillment number from Microsoft
        Ready = 0,
        // Got raw response data after fulfill API called
        Fulfilled = 1,
        // Parsed response data to KeyInfo and saved keys into database
        Completed = 2,
        // Fulfillment failed
        Failed = 3,
        // Update keys into fulfillment
        InProgress = 4,
    }
}
