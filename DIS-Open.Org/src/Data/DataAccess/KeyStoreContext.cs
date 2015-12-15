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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DIS.Data.DataContract;
using DIS.Data.DataAccess.Mapping;
using System.Data.Common;
using System.Data.Objects;

namespace DIS.Data.DataAccess
{
    public class KeyStoreContext : DbContext
    {

        private bool isUsingTransaction;
        public KeyStoreContext() : base() 
        {

        }

        public KeyStoreContext(string nameOrConnectionString, bool useTransaction = true)
            : base(nameOrConnectionString) 
        {
            base.Configuration.LazyLoadingEnabled = false;
            base.Configuration.ProxyCreationEnabled = false;

            if (useTransaction) 
            {
                if (base.Database.Connection.ConnectionString != nameOrConnectionString)
                {
                    if (base.Database.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        base.Database.Connection.Close();
                    }

                    base.Database.Connection.ConnectionString = nameOrConnectionString;
                }

                if (this.connection.State != System.Data.ConnectionState.Open)
                {
                    this.connection.Open();
                }

                this.Transaction = this.connection.BeginTransaction();
            }
        }
        static KeyStoreContext()
        {
            Database.SetInitializer<KeyStoreContext>(null);
        }

        public static KeyStoreContext GetContext()
        {
            return new KeyStoreContext();
        }

        public static KeyStoreContext GetContext(string ConnectionString)
        {
            return new KeyStoreContext(ConnectionString);
        }

        public void ChangeConnection(string ConnectionString) 
        {
            if (!String.IsNullOrEmpty(ConnectionString))
            {
                this.connection.ConnectionString = ConnectionString;
                this.connection.Open();
            }
        }

        public KeyStoreContext(bool useTransaction = true)
        {
            base.Configuration.LazyLoadingEnabled = false;
            base.Configuration.ProxyCreationEnabled = false;

            if (useTransaction)
            {
                connection.Open();
                Transaction = connection.BeginTransaction();
            }
        }

        

        public override int SaveChanges()
        {
            //if (Transaction != null)
            //    Transaction.Commit();
            ////return base.SaveChanges();
            // fix bug for NEC issue.
            // Generate CBR and update key isInProccess not in the same transaction - Merged from v1.9 Rally Spet 26, 2014
            int result = base.SaveChanges();
            if (Transaction != null)
                Transaction.Commit();
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                connection.Close();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private DbConnection connection
        {
            get { return ((IObjectContextAdapter)this).ObjectContext.Connection; }
        }

        public DbTransaction Transaction
        {
            get;
            private set;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLog> CategoryLogs { get; set; }
        public DbSet<Cbr> Cbrs { get; set; }
        public DbSet<CbrKey> CbrKeys { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<CbrDuplicated> CbrsDuplicated { get; set; }
        public DbSet<KeyDuplicated> KeysDuplicated { get; set; }
        public DbSet<FulfillmentInfo> FulfillmentInfoes { get; set; }
        public DbSet<HeadQuarter> HeadQuarters { get; set; }
        public DbSet<KeyExportLog> KeyExportLogs { get; set; }
        public DbSet<KeyHistory> KeyHistories { get; set; }
        public DbSet<KeyInfoEx> KeyInfoExes { get; set; }
        public DbSet<KeyOperationHistory> KeyOperationHistories { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<KeyInfo> KeyInfoes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subsidiary> Subsidiaries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserHeadQuarter> UserHeadQuarters { get; set; }
        public DbSet<KeyTypeConfiguration> KeyTypeConfigurations { get; set; }
        public DbSet<KeySyncNotification> KeySyncNotifications { get; set; }
        public DbSet<ReturnReport> ReturnReports { get; set; }
        public DbSet<ReturnReportKey> ReturnReportKeys { get; set; }
        public DbSet<TempKeyId> TempKeyId { get; set; }
        public DbSet<Ohr> Ohrs { get; set; }
        public DbSet<OhrKey> OhrKeys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CategoryLogMap());
            modelBuilder.Configurations.Add(new ComputerBuildReportMap());
            modelBuilder.Configurations.Add(new ComputerBuildReportKeyMap());
            modelBuilder.Configurations.Add(new ConfigurationMap());
            modelBuilder.Configurations.Add(new DuplicatedComputerBuildReportMap());
            modelBuilder.Configurations.Add(new DuplicatedKeyMap());
            modelBuilder.Configurations.Add(new FulfillmentInfoMap());
            modelBuilder.Configurations.Add(new HeadQuarterMap());
            modelBuilder.Configurations.Add(new KeyExportLogMap());
            modelBuilder.Configurations.Add(new KeyHistoryMap());
            modelBuilder.Configurations.Add(new KeyInfoExMap());
            modelBuilder.Configurations.Add(new KeyOperationHistoryMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new ProductKeyInfoMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SubsidiaryMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserHeadQuarterMap());
            modelBuilder.Configurations.Add(new KeyTypeConfigurationMap());
            modelBuilder.Configurations.Add(new KeySyncNotificationMap());
            modelBuilder.Configurations.Add(new ReturnReportMap());
            modelBuilder.Configurations.Add(new ReturnReportKeyMap());
            modelBuilder.Configurations.Add(new TempKeyIdMap());
            modelBuilder.Configurations.Add(new OHRDataUpdateMap());
            modelBuilder.Configurations.Add(new OHRDataUpdateKeyMap());
        }
    }
}

