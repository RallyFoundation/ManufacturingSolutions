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
using DIS.Business.Library;
using DIS.Data.DataContract;

namespace DIS.Business.Proxy
{
    public interface IConfigProxy : IConfigManager
    {
        DiagnosticResult TestInternalConnection();

        DiagnosticResult TestUpLevelSystemConnection(int hqId);

        DiagnosticResult TestDownLevelSystemConnection(int ssId);

        DiagnosticResult TestMsConnection(int? hqId);

        DiagnosticResult TestDataPollingService();

        DiagnosticResult TestKeyProviderService();

        DiagnosticResult TestDatabaseDiskFull();

        void DataPollingServiceReport();

        void KeyProviderServiceReport();

        void DatabaseDiskFullReport();
    }
}
