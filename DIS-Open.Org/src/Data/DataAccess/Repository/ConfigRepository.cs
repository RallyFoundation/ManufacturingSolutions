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
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Text;
using DIS.Data.DataContract;

namespace DIS.Data.DataAccess.Repository {
    public class ConfigRepository : RepositoryBase, IConfigRepository 
    {
        public ConfigRepository() : base() 
        {

        }

        public ConfigRepository(string ConnectionString) : base(ConnectionString) 
        {

        }
        public Configuration GetConfiguration(string name) {
            using (var context = GetContext()) {
                return context.Configurations.SingleOrDefault(c => c.Name == name);
            }
        }

        public void InsertConfiguration(Configuration config) {
            using (var context = GetContext()) {
                context.Configurations.Add(config);
                context.SaveChanges();
            }
        }

        public void UpdateConfiguration(Configuration config) {
            using (var context = GetContext()) {
                context.Configurations.Attach(config);
                context.Entry(config).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void TestDatabaseConnection()
        {
            using (var context = GetContext())
            {
                context.Configurations.Count();
            }
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }
    }
}
