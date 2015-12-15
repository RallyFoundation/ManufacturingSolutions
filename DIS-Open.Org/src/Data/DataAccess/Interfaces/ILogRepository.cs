﻿//*********************************************************
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
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository
{
    /// <summary>
    /// interface of LogRepository
    /// </summary>
    public interface ILogRepository
    {
        List<Category> GetCategories();
        PagedList<Log> SearchLogs(LogSearchCriteria criteria);
        Log GetLogById(int logId);
        void DeleteLogs(List<Log> logs);

        string GetDBConnectionString();
    }
}
