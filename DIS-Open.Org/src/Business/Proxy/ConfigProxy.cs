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
using DIS.Business.Client;
using DIS.Business.Library;
using DIS.Data.DataContract;
using Microsoft.ServiceModel.Web;
using DIS.Common.Utility;
using System.Net;

namespace DIS.Business.Proxy
{
    public class ConfigProxy : ConfigManager, IConfigProxy
    {
        private IFulfillmentManager fulfillmentManager;
        private User user;
        private string dbConnectionString;
        private string cloudConfigurationId;
        private string cloudCustomerId;

        public ConfigProxy(User user)
            : this(user, new FulfillmentManager())
        { }

        public ConfigProxy(User user, string dbConnectionString, string configurationId, string customerId) : base(dbConnectionString)
        {
            this.user = user;

            this.dbConnectionString = dbConnectionString;
            this.cloudConfigurationId = configurationId;
            this.cloudCustomerId = customerId;

            this.fulfillmentManager = new FulfillmentManager(dbConnectionString);
        } 

        public ConfigProxy(User user, IFulfillmentManager fulfillmentManager)
        {
            this.user = user;

            if (fulfillmentManager == null)
                this.fulfillmentManager = new FulfillmentManager();
            else
                this.fulfillmentManager = fulfillmentManager;
        }

        #region Diagnostic

        public DiagnosticResult TestInternalConnection()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            return DiagnosticConnectionState(() => internalClient.TestInternal());
        }

        public DiagnosticResult TestUpLevelSystemConnection(int hqId)
        {
            ServiceClient client = new ServiceClient(hqId, CallDirection.Internal | CallDirection.UpLevelSystem, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            return DiagnosticConnectionState(() => client.TestExternal());
        }

        public DiagnosticResult TestDownLevelSystemConnection(int ssId)
        {
            ServiceClient client = new ServiceClient(ssId, CallDirection.Internal | CallDirection.DownLevelSystem, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            return DiagnosticConnectionState(() => client.TestExternal());
        }

        public DiagnosticResult TestDataPollingService()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            return DiagnosticConnectionState(() => internalClient.TestDataPollingService());
        }

        public DiagnosticResult TestKeyProviderService()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            return DiagnosticConnectionState(() => internalClient.TestKeyProviderService());
        }

        public DiagnosticResult TestMsConnection(int? hqId)
        {
            ServiceClient client = new ServiceClient(hqId, CallDirection.Internal | CallDirection.Microsoft, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            return DiagnosticConnectionState(() =>
                {
                    List<FulfillmentInfo> fulfillments = client.GetFulfilments();
                    if (fulfillments.Count > 0)
                        fulfillmentManager.SaveAvailableFulfillments(fulfillments);
                });
        }

        public DiagnosticResult TestDatabaseDiskFull()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            DiagnosticResult diagnosticResult = new DiagnosticResult();
            try
            {
                bool result = internalClient.TestDatabaseDiskFull();
                diagnosticResult.DiagnosticResultType = result ? DiagnosticResultType.Error : DiagnosticResultType.Ok;
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.Message, this.dbConnectionString);
            }
            return diagnosticResult;
        }

        public void DataPollingServiceReport()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            internalClient.DataPollingServiceReport();
        }

        public void KeyProviderServiceReport()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            internalClient.KeyProviderServiceReport();
        }

        public void DatabaseDiskFullReport()
        {
            ServiceClient internalClient = new ServiceClient(null, CallDirection.Internal, user, this.cloudConfigurationId, this.dbConnectionString, this.cloudCustomerId);
            internalClient.DatabaseDiskFullReport(false);
        }

        #endregion

        private DiagnosticResult DiagnosticConnectionState(Action action)
        {
            DiagnosticResult diagnosticResult = new DiagnosticResult();
            try
            {
                action();
                diagnosticResult.DiagnosticResultType = DiagnosticResultType.Ok;
            }
            catch (Exception ex)
            {
                diagnosticResult.DiagnosticResultType = DiagnosticResultType.Error;
                diagnosticResult.Exception = ex;
            }
            return diagnosticResult;
        }

    }
}

