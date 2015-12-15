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
    /// <summary>
    /// interface of ConfigRepository
    /// </summary>
    public interface IConfigRepository
    {
        Configuration GetConfiguration(string name);

        void InsertConfiguration(Configuration config);

        void UpdateConfiguration(Configuration config);

        void TestDatabaseConnection();

        string GetDBConnectionString();
    }
}
