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
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace DIS.Data.DataAccess.Repository
{
    public class KeyTypeConfigurationRepository : RepositoryBase, IKeyTypeConfigurationRepository
    {
        private const int MaxKeyCounts = 5000;
        private const int MinKeyCounts = 0;

        public KeyTypeConfigurationRepository() : base() 
        {

        }

        public KeyTypeConfigurationRepository(string ConnectionString) : base(ConnectionString) 
        {
 
        }
        public void UpdateKeyTypeConfiguration(KeyTypeConfiguration config, bool shouldUpdateKeys)
        {
            using (var context = GetContext())
            {
                KeyTypeConfiguration configInDb = context.KeyTypeConfigurations.Single(c => c.KeyTypeConfigurationId == config.KeyTypeConfigurationId);
                context.Entry(configInDb).CurrentValues.SetValues(config);

                if (shouldUpdateKeys)
                {
                    string hqIdString = GetSearchCriteria(config.HeadQuarterId);
                    string sqlString = string.Format(@"UPDATE KeyInfoEx
                                            SET KeyType= {0}
                                            FROM KeyInfoEx ex JOIN ProductKeyInfo info ON info.ProductKeyID = ex.ProductKeyID
                                            WHERE info.LicensablePartNumber = '{1}' AND ex.HQID {2}",
                                             (int)configInDb.KeyType.Value, config.LicensablePartNumber, hqIdString);
                    context.Database.ExecuteSqlCommand(sqlString);
                }
                context.SaveChanges();
            }
        }

        public List<KeyTypeConfiguration> GetKeyTypeConfigurations(int? hqId)
        {
            using (var context = GetContext())
            {
                MappingKeyTypeConfiguration(context, hqId);

                return GetAllKeyTypeConfigurations(context, hqId);
            }
        }

        public string GetDBConnectionString()
        {
            return base.ConnectionString;
        }

        private List<KeyTypeConfiguration> GetAllKeyTypeConfigurations(KeyStoreContext context, int? hqId)
        {
            string hqIdString = GetSearchCriteria(hqId);
            string sql = string.Format(@"SELECT config.[KeyTypeConfigurationId]
                                          ,config.[HeadQuarterId]
                                          ,config.[LicensablePartNumber]
                                          ,config.[Maximum]
                                          ,config.[Minimum]
                                          ,config.[KeyType] AS KeyTypeId
                                          ,(SELECT COUNT(1) FROM ProductKeyInfo
                                            WHERE LicensablePartNumber = config.LicensablePartNumber and ProductKeyStateID = {0}) AS AvailiableKeysCount
                                      FROM [KeyTypeConfiguration] config 
                                      WHERE config.HeadQuarterId {1}
                                      GROUP BY config.[KeyTypeConfigurationId], config.[HeadQuarterId], config.[LicensablePartNumber], config.[Maximum], config.[Minimum], config.[KeyType]
                                      ORDER BY config.[LicensablePartNumber]",
                                        (int)KeyState.Fulfilled, hqIdString);

            var ret = Select<KeyTypeConfigurationWithAvailiableKeysCount>(context, sql);
            return ((IEnumerable)ret).Cast<KeyTypeConfiguration>().ToList();
        }

        private void MappingKeyTypeConfiguration(KeyStoreContext context, int? hqId)
        {
            string hqIdString = GetSearchCriteria(hqId);
            string sql = string.Format(@"SELECT INFO.LicensablePartNumber, EX.KeyType 
                                FROM ProductKeyInfo INFO JOIN KeyInfoEx EX ON INFO.ProductKeyID = EX.ProductKeyID
                                WHERE EX.HQID {0} 
                                AND NOT EXISTS ( SELECT 1 FROM KeyTypeConfiguration WHERE HeadQuarterId {0} AND LicensablePartNumber = info.LicensablePartNumber)
                                GROUP BY INFO.LicensablePartNumber, EX.KeyType", hqIdString);

            var maps = Select<LicensablePartNumberAndKeyTypeMap>(context, sql).ToList();

            if (maps.Any())
            {
                foreach (var map in maps)
                {
                    KeyTypeConfiguration config = new KeyTypeConfiguration
                    {
                        HeadQuarterId = hqId,
                        LicensablePartNumber = map.LicensablePartNumber,
                        Maximum = MaxKeyCounts,
                        Minimum = MinKeyCounts,
                        KeyType = (KeyType?)map.KeyType
                    };
                    context.KeyTypeConfigurations.Add(config);
                }

                context.SaveChanges();
            }
        }

        private static string GetSearchCriteria(int? hqId)
        {
            string criteria;
            if (hqId == null)
            {
                criteria = " IS NULL";
            }
            else
            {
                criteria = " = " + hqId.Value;
            }
            return criteria;
        }
    }

    internal class LicensablePartNumberAndKeyTypeMap
    {
        internal string LicensablePartNumber { get; set; }
        internal int? KeyType { get; set; }
    }

    internal class KeyTypeConfigurationWithAvailiableKeysCount : KeyTypeConfiguration
    {
    }
}
