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
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    public interface IFulfillmentRepository
    {
        FulfillmentInfo GetFirstFulfillment(FulfillmentStatus status);
        List<FulfillmentInfo> GetFulfillments(List<string> fulfillmentNumbers);
        List<FulfillmentInfo> GetFulfillments(FulfillmentStatus status, DateTime? modifiedFromUtc = null);
        void InsertFulfillments(List<FulfillmentInfo> infoes);
        void UpdateFulfillment(FulfillmentInfo fulfillmentInfo, KeyStoreContext context = null);

        string GetDBConnectionString();
    }
}
