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
using System.Transactions;
using DIS.Data.DataContract;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Data.EntityClient;
using System.Reflection;

namespace DIS.Data.DataAccess.Repository
{
    public abstract class RepositoryBase
    {
        protected const string deleteSqlCommandFormat = "DELETE FROM [{0}] WHERE [{1}] in ({2})";
        protected const string tempKeyIdName = "TempKeyId";

        private string connectionString = "";

        public string ConnectionString 
        {
            get { return this.connectionString; }
        }

        public RepositoryBase() 
        {

        }

        public RepositoryBase(string ConnectionString) 
        {
            this.connectionString = ConnectionString;
        }

        protected KeyStoreContext GetContext()
        {
            if (!String.IsNullOrEmpty(this.connectionString))
            {
                return KeyStoreContext.GetContext(this.connectionString);
            }
            else
            {
                return KeyStoreContext.GetContext();
            }
        }

        protected void UsingContext(ref KeyStoreContext context, Action action)
        {
            if (context == null)
                using (context = GetContext())
                {
                    action();
                    context.SaveChanges();
                }
            else
                action();
        }

        protected void DeleteEntities(string[] tableNames, string idColumnName, string idList)
        {
            using (var context = GetContext())
            {
                foreach (string tableName in tableNames)
                {
                    context.Database.ExecuteSqlCommand(string.Format(deleteSqlCommandFormat, tableName, idColumnName, idList));
                }
                context.SaveChanges();
            }
        }

        protected List<T> Select<T>(KeyStoreContext context, string sqlCommand, params object[] parameters )
        {
            return context.Database.SqlQuery<T>(sqlCommand, parameters).ToList();
        }

        private void SetupTempKeyIdTableCore(KeyStoreContext context, DataTable dt)
        {
            DbTransaction transaction = context.Transaction;
            context.Database.ExecuteSqlCommand(string.Format("DELETE {0}", tempKeyIdName));
            EntityConnection connection = (EntityConnection)transaction.Connection;
            SqlTransaction sqlTransaction = (SqlTransaction)transaction.GetType().InvokeMember("StoreTransaction",
                BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic,
                null, transaction, new object[0]);
            using (SqlBulkCopy bulk = new SqlBulkCopy(
                connection.StoreConnection as SqlConnection,
                SqlBulkCopyOptions.Default,
                sqlTransaction))
            {
                bulk.DestinationTableName = tempKeyIdName;
                bulk.WriteToServer(dt);
            }
        }

        private DataTable CreateTempKeyIdTable(long[] keyIds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("KeyId", typeof(long)));
            foreach (long keyId in keyIds)
            {
                dt.Rows.Add(keyId);
            }
            return dt;
        }

        private DataTable CreateTempKeyIdTable(List<KeyInfo> keys)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("KeyId", typeof(long)));
            dt.Columns.Add(new DataColumn("KeyState", typeof(byte)));
            dt.Columns.Add(new DataColumn("KeyType", typeof(int)));
            foreach (KeyInfo key in keys)
            {
                dt.Rows.Add(
                    key.KeyId,
                    key.KeyStateChanged ? key.KeyStateId : (byte?)null,
                    key.KeyInfoEx.KeyTypeId);
            }
            return dt;
        }

        protected void SetupTempKeyIdTable(KeyStoreContext context, long[] keyIds)
        {
            SetupTempKeyIdTableCore(context, CreateTempKeyIdTable(keyIds));
        }

        protected void SetupTempKeyIdTable(KeyStoreContext context, List<KeyInfo> keys)
        {
            SetupTempKeyIdTableCore(context, CreateTempKeyIdTable(keys));
        }
    }
}
