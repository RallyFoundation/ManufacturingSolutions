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
   public interface IReturnKeyRepository
    {
       List<ReturnReport> SearchReturnKeys(ReturnKeySearchCriteria returnKeySearchCriteria);

       void InsertReturnReportAndKeys(ReturnReport returnKey);

       void UpdateReturnReport(ReturnReport returnKey, KeyStoreContext context = null);

       void UpdateReturnKeyAck(ReturnReport returnKey, KeyStoreContext context);

       ReturnReport GetReturnKey(Guid customerReportUniqueId, KeyStoreContext context);

       ReturnReport GetReturnKeyByOneKeyID(long keyId, KeyStoreContext context);

       ReturnReport GetReturnKeyByCustomerId(Guid customerReportUniqueId);

       string GetDBConnectionString();
   }
}
