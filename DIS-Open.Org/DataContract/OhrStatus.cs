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

namespace DIS.Data.DataContract
{
    public enum OhrStatus
    {
        // OHR data was generated, but not been sent
        Generated = 0,
        // OHR data was sent and MS GUID was retrieved
        Unconfirmed = 1,
        // OHR data ack was ready
        Ready = 4,
        // OHR data Ack was received
        Confirmed = 2,
        // OHR data Ack was failed to receive
        Failed = 3,
        // After user review the notification
        ReadMark = 5,
    }
}
