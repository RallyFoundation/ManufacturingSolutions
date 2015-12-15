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
    public enum CbrStatus {
        // CBR was generated, but not been sent
        Generated = 0,
        // CBR is reporting
        Sent = 7,
        // CBR was sent and MS GUID was retrieved
        Reported = 1,
        // CBR ack was ready
        Ready = 4,
        // CBR Ack was received
        Completed = 2,
        // CBR Ack was failed to receive
        Failed = 3,
    }
}
