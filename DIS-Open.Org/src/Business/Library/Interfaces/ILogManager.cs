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
using System.Threading;
using DIS.Data.DataContract;

namespace DIS.Business.Library
{
    public interface ILogManager
    {
        List<Category> GetCategories();

        Log GetLogById(int logId);

        PagedList<Log> GetSystemLogs(int severity, DateTime? from, DateTime? to, int pageNumber, int pageSize, string sortBy, bool sortByDesc);

        PagedList<Log> GetOperationLogs(string userName, DateTime? from, DateTime? to, int pageNumber, int pageSize, string sortBy, bool sortByDesc);

        int ExportLogs(string categoryName, DateTime? from, DateTime? to, bool shouldDeleteFromDb, string outputPath, CancellationTokenSource cancellationTokenSource);
    }
}