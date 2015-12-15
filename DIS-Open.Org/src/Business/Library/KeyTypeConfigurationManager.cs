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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataAccess;

namespace DIS.Business.Library
{
    public class KeyTypeConfigurationManager : IKeyTypeConfigurationManager
    {
        private IKeyTypeConfigurationRepository repository;

        public KeyTypeConfigurationManager() : this(new KeyTypeConfigurationRepository()) { }

        public KeyTypeConfigurationManager(string dbConnectionString)
        {
            this.repository = new KeyTypeConfigurationRepository(dbConnectionString);
        }


        public KeyTypeConfigurationManager(IKeyTypeConfigurationRepository r)
        {
            if (r == null)
                repository = new KeyTypeConfigurationRepository();
            else
                repository = r;
        }

        public List<KeyTypeConfiguration> GetKeyTypeConfigurations(int? hqId)
        {
            return repository.GetKeyTypeConfigurations(hqId);
        }

        public void UpdateKeyTypeConfiguration(KeyTypeConfiguration keyTypeConfiguration, bool shouldUpdateKeys)
        {
            if (keyTypeConfiguration == null)
                throw new ArgumentNullException("params error: keyTypeConfiguration is null");
            repository.UpdateKeyTypeConfiguration(keyTypeConfiguration, shouldUpdateKeys);
        }
    }
}
