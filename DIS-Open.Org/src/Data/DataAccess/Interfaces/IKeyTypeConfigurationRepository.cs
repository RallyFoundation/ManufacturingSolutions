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
using System.Data.Objects;
using DIS.Data.DataContract;
using System.Data.Entity;

namespace DIS.Data.DataAccess.Repository
{
   public interface IKeyTypeConfigurationRepository
    {
       List<KeyTypeConfiguration> GetKeyTypeConfigurations(int? hqId);

       void UpdateKeyTypeConfiguration(KeyTypeConfiguration config, bool shouldUpdateKeys);

       string GetDBConnectionString();
   }
}
