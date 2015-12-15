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
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using DIS.Business.Client;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Services.WebServiceLibrary;
using Microsoft.ServiceModel.Web;

namespace DIS.Services.TransferWebService
{
    [ServiceBehavior(MaxItemsInObjectGraph = int.MaxValue)]
    public class TransferService : ServiceBase, ITransferService
    {
        private const int reportCheckInterval = Constants.PulseInterval * 2;
        private IServiceClient serviceClient;

        //private string configurationId;
        //private string dbConnectionString;
        //private string customerId;

        public TransferService()
        {
            try
            {
                string systemId = GetRequestHeader(ServiceClient.SystemIdHeaderName);
                CallDirection callDirection = (CallDirection)Enum.Parse(typeof(CallDirection),
                    GetRequestHeader(ServiceClient.DirectionHeaderName));

                this.ConfigurationID = this.GetRequestHeader(DIS.Business.Client.ServiceClient.ConfigurationIdHeaderName);

                this.CustomerID = this.GetRequestHeader(DIS.Business.Client.ServiceClient.CustomerIdHeaderName);

                if (String.IsNullOrEmpty(this.CustomerID))
                {
                    string authHeader = this.GetRequestHeader(DIS.Business.Client.ServiceClient.AuthorizationHeaderName);
                    authHeader = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(authHeader));

                    authHeader = authHeader.Replace("Basic", string.Empty);

                    authHeader = (authHeader.Split(new string[] { ":" }, StringSplitOptions.None))[1];

                    if ((ModuleConfiguration.DISCloudBusinessReferences != null) && (ModuleConfiguration.DISCloudBusinessReferences.ContainsKey(authHeader)))
                    {
                        this.CustomerID = ModuleConfiguration.DISCloudBusinessReferences[authHeader];
                    }
                }

                if (String.IsNullOrEmpty(this.CustomerID))
                {
                    this.CustomerID = ModuleConfiguration.DefaultBusinessID;
                }

                if (!String.IsNullOrEmpty(this.ConfigurationID))
                {
                    if ((ModuleConfiguration.DISCloudConfigurations == null) || (!ModuleConfiguration.DISCloudConfigurations.ContainsKey(this.ConfigurationID)))
                    {
                        ModuleConfiguration.SyncConfigurations();
                    }

                    if ((ModuleConfiguration.DISCloudConfigurations != null) && (ModuleConfiguration.DISCloudConfigurations.ContainsKey(this.ConfigurationID)))
                    {
                        this.DBConnectionString = ModuleConfiguration.DISCloudConfigurations[this.ConfigurationID];
                    }
                }
                else
                {
                    this.DBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KeyStoreContext"].ConnectionString;
                }

                //MessageLogger.ResetLoggingConfiguration("KeyStoreContext", this.dbConnectionString);

                if (callDirection != CallDirection.None)
                {
                    //serviceClient = new ServiceClient(string.IsNullOrEmpty(systemId) ? null :
                    //    (int?)int.Parse(systemId), callDirection, null) 
                    //    { 
                    //        ShouldLogResponseData = true 
                    //    };

                    serviceClient = new ServiceClient(string.IsNullOrEmpty(systemId) ? null :
                        (int?)int.Parse(systemId), callDirection, null, this.ConfigurationID, this.DBConnectionString, this.CustomerID)
                    {
                        ShouldLogResponseData = true
                    };
                }
            }
            catch (Exception ex)
            {
                //if (!String.IsNullOrEmpty(this.dbConnectionString))
                //{
                    //MessageLogger.ResetLoggingConfiguration("KeyStoreContext", this.dbConnectionString);
                //    ExceptionHandler.HandleException(ex, this.DBConnectionString);
                //}
                //else
                //{
                //    //At this time, there is not a specific customer context to log the errors occurred, so a global log is used - Rally Sept. 11, 2014
                TracingHelper.Trace(new object[] { ex, string.Format("Business ID: {0}; ", this.CustomerID), string.Format("Configuration ID: {0}; ", this.ConfigurationID), string.Format("DB Connection String: {0}; ", this.DBConnectionString) }, "DISInternalAPITraceSource");
                //}
                
                throw;
            }
        }

        #region Microsoft web services

        public List<FulfillmentInfo> GetFulfillments()
        {
            return HandleException<List<FulfillmentInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.GetFulfilments());
        }

        public List<KeyInfo> FulfillKeys(string fulfillmentId)
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.FulfillKeys(fulfillmentId));
        }

        public Guid ReportCbr(Cbr request)
        {
            return HandleException<Guid>(MessageLogger.GetMethodName(), () =>
                serviceClient.ReportCbr(request));
        }

        public Cbr SearchSubmittedCbr(Cbr cbr)
        {
            return HandleException<Cbr>(MessageLogger.GetMethodName(), () =>
                serviceClient.SearchSubmittedCbr(cbr));
        }

        public Guid[] RetrieveCbrAcks()
        {
            return HandleException<Guid[]>(MessageLogger.GetMethodName(), () =>
                serviceClient.RetrieveCbrAcks());
        }

        public Cbr RetrieveCbrAck(Cbr cbr)
        {
            return HandleException<Cbr>(MessageLogger.GetMethodName(), () => 
                serviceClient.RetrieveCbrAck(cbr));
        }

        public Guid ReportReturn(ReturnReport request)
        {
            return HandleException<Guid>(MessageLogger.GetMethodName(), () =>
                serviceClient.ReportReturn(request));
        }

        public ReturnReport SearchSubmittedReturn(ReturnReport returnReport)
        {
            return HandleException<ReturnReport>(MessageLogger.GetMethodName(), () =>
                serviceClient.SearchSubmittedReturn(returnReport));
        }

        public Guid[] RetrieveReturnAcks()
        {
            return HandleException<Guid[]>(MessageLogger.GetMethodName(), () =>
                serviceClient.RetrieveReturnReportAcks());
        }

        public ReturnReport RetrieveReturnAck(ReturnReport request)
        {
            return HandleException<ReturnReport>(MessageLogger.GetMethodName(), () =>
                serviceClient.RetrievReturnReportAck(request));
        }

        public Guid ReportOhr(Ohr request)
        {
            return HandleException<Guid>(MessageLogger.GetMethodName(), () =>
                serviceClient.ReportOhr(request));
        }

        public Guid[] RetrieveOhrAcks()
        {
            return HandleException<Guid[]>(MessageLogger.GetMethodName(), () =>
            serviceClient.RetrieveOhrAcks());
        }

        public Ohr RetrieveOhrAck(Ohr ohr)
        {
            return HandleException<Ohr>(MessageLogger.GetMethodName(), () =>
                serviceClient.RetrieveOhrAck(ohr));
        }

        #endregion

        #region ULS web services

        public void TestInternal()
        {
        }

        public void TestExternal()
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.TestExternal();
            });
        }

        public void TestDataPollingService()
        {
            //CalculatePeriodOfTime(Global.DpsLastReportTime);
            //Rally - Mar. 23, 2015
            DateTime period = Global.DpsLastReportTime;

            if (!String.IsNullOrEmpty(this.ConfigurationID))
            {
                period = Global.GetDPSLastReportTime(this.ConfigurationID);
            }

            CalculatePeriodOfTime(period);
        }

        public void TestKeyProviderService()
        {
            //CalculatePeriodOfTime(Global.KpsLastReportTime);
            //Rally - Mar. 23, 2015
            DateTime period = Global.KpsLastReportTime;

            if (!String.IsNullOrEmpty(this.ConfigurationID))
            {
                period = Global.GetKPSLastReportTime(this.ConfigurationID);
            }

            CalculatePeriodOfTime(period);
        }

        public void DataPollingServiceReport()
        {
            //Rally - Mar. 23, 2015
            if (String.IsNullOrEmpty(this.ConfigurationID))
            {
                Global.DpsLastReportTime = DateTime.Now;
            }
            else
            {
                Global.SetDPSLastReportTime(this.ConfigurationID, DateTime.Now);
            }
        }

        public void KeyProviderServiceReport()
        {
            //Rally - Mar. 23, 2015
            if (String.IsNullOrEmpty(this.ConfigurationID))
            {
                Global.KpsLastReportTime = DateTime.Now;
            }
            else
            {
                Global.SetKPSLastReportTime(this.ConfigurationID, DateTime.Now);
            }
        }

        public bool TestDatabaseDiskFull()
        {
            return CheckDatabaseDiskFullError();
        }

        public void DatabaseDiskFullReport(bool isFull)
        {
            //Global.IsDatabaseDiskFull = isFull;
            //Rally - Mar. 23, 2015

            if (String.IsNullOrEmpty(this.ConfigurationID)) 
            {
                Global.IsDatabaseDiskFull = isFull;
            }
            else
            {
                Global.SetIsDatabaseDiskFullFlagValue(this.ConfigurationID, isFull);
            }
        }

        /// <summary>
        /// Get Product Keys from OEM's
        /// </summary>
        /// <param name="shipToCustomNumber">Identity the TPI</param>
        public List<KeyInfo> GetKeys()
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.GetKeys());
        }

        public void SyncKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.SyncKeys(request);
            });
        }

        public List<KeyInfo> ReportKeys(List<KeyInfo> request)
        {
            return HandleException<List<KeyInfo>>(MessageLogger.GetMethodName(), () =>
                serviceClient.ReportKeys(request));
        }

        public void RecallKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.RecallKeys(request);
            });
        }

        public void CarbonCopyFulfilledKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyFulfilledKeys(request);
            });
        }

        public void CarbonCopyReportedKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyReportedKeys(request);
            });
        }

        public void CarbonCopyReturnReportedKeys(List<KeyInfo> request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyReturnReportedKeys(request);
            });
        }

        public void CarbonCopyReturnReport(ReturnReport request)
        {
            HandleException(MessageLogger.GetMethodName(), () =>
            {
                serviceClient.CarbonCopyReturnReport(request);
            });
        }

        public long[] SendKeySyncNotifications(List<KeySyncNotification> request)
        {
            return HandleException<long[]>(MessageLogger.GetMethodName(), () =>
                serviceClient.SendKeySyncNotifications(request));
        }

        #endregion

        private void HandleException(string methodName, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                //if (!String.IsNullOrEmpty(this.dbConnectionString))
                //{
                //    MessageLogger.ResetLoggingConfiguration("KeyStoreContext", this.dbConnectionString);
                //}

                ExceptionHandler.HandleException(ex, this.DBConnectionString);
                if (ex is WebProtocolException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = ((WebProtocolException)ex).StatusCode;
                    throw;
                }
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, methodName, ex);
            }
        }

        private T HandleException<T>(string methodName, Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                //if (!String.IsNullOrEmpty(this.dbConnectionString))
                //{
                //    MessageLogger.ResetLoggingConfiguration("KeyStoreContext", this.dbConnectionString);
                //}

                ExceptionHandler.HandleException(ex, this.DBConnectionString);
                if (ex is WebProtocolException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = ((WebProtocolException)ex).StatusCode;
                    throw;
                }
                else
                    throw new WebProtocolException(HttpStatusCode.InternalServerError, methodName, ex);
            }
        }

        private void CalculatePeriodOfTime(DateTime period)
        {
            TimeSpan elplseTime = DateTime.Now.Subtract(period);
            if (elplseTime.TotalMilliseconds > reportCheckInterval)
                throw new WebProtocolException(HttpStatusCode.InternalServerError);
        }

        private bool CheckDatabaseDiskFullError()
        {
            //return Global.IsDatabaseDiskFull;
            //Rally - Mar. 23, 2015

            if (String.IsNullOrEmpty(this.ConfigurationID))
            {
                return Global.IsDatabaseDiskFull;
            }

            return Global.GetIsDatabaseDiskFullFlagValue(this.ConfigurationID);
        }
    }

    public static class Global
    {
        /// <summary>
        /// Global variable storing important stuff.
        /// </summary>
        static DateTime dpsLastReportTime;
        static DateTime kpsLastReportTime;
        static bool isDatabaseDiskFull;

        static IDictionary<string, DateTime> dpsLastReportTimes;
        static IDictionary<string, DateTime> kpsLastReportTimes;
        static IDictionary<string, bool> isDatabaseDiskFullFlags;

        /// <summary>
        /// Get or set the static important data.
        /// </summary>
        public static DateTime DpsLastReportTime
        {
            get
            {
                return dpsLastReportTime;
            }
            set
            {
                dpsLastReportTime = value;
            }
        }

        /// <summary>
        /// Get or set the static important data.
        /// </summary>
        public static DateTime KpsLastReportTime
        {
            get
            {
                return kpsLastReportTime;
            }
            set
            {
                kpsLastReportTime = value;
            }
        }

        public static bool IsDatabaseDiskFull
        {
            get
            {
                return isDatabaseDiskFull;
            }
            set
            {
                isDatabaseDiskFull = value;
            }
        }

        public static DateTime GetDPSLastReportTime(string ConfigurationID) 
        {
            if ((dpsLastReportTimes == null) || (!dpsLastReportTimes.ContainsKey(ConfigurationID)))
            {
                return DateTime.MinValue;
            }

            return dpsLastReportTimes[ConfigurationID];
        }

        public static DateTime SetDPSLastReportTime(string ConfigurationID, DateTime ReportTime) 
        {
            if (dpsLastReportTimes == null)
            {
                dpsLastReportTimes = new SortedDictionary<string, DateTime>();
            }

            if (!dpsLastReportTimes.ContainsKey(ConfigurationID))
            {
                dpsLastReportTimes.Add(ConfigurationID, ReportTime);
            }
            else
            {
                dpsLastReportTimes[ConfigurationID] = ReportTime;
            }

            return dpsLastReportTimes[ConfigurationID];
        }

        public static DateTime GetKPSLastReportTime(string ConfigurationID)
        {
            if ((kpsLastReportTimes == null) || (!kpsLastReportTimes.ContainsKey(ConfigurationID)))
            {
                return DateTime.MinValue;
            }

            return kpsLastReportTimes[ConfigurationID];
        }

        public static DateTime SetKPSLastReportTime(string ConfigurationID, DateTime ReportTime)
        {
            if (kpsLastReportTimes == null)
            {
                kpsLastReportTimes = new SortedDictionary<string, DateTime>();
            }

            if (!kpsLastReportTimes.ContainsKey(ConfigurationID))
            {
                kpsLastReportTimes.Add(ConfigurationID, ReportTime);
            }
            else
            {
                kpsLastReportTimes[ConfigurationID] = ReportTime;
            }

            return kpsLastReportTimes[ConfigurationID];
        }

        public static bool GetIsDatabaseDiskFullFlagValue(string ConfigurationID)
        {
            if ((isDatabaseDiskFullFlags == null) || (!isDatabaseDiskFullFlags.ContainsKey(ConfigurationID)))
            {
                return false;
            }

            return isDatabaseDiskFullFlags[ConfigurationID];
        }

        public static bool SetIsDatabaseDiskFullFlagValue(string ConfigurationID, bool flagValue)
        {
            if (isDatabaseDiskFullFlags == null)
            {
                isDatabaseDiskFullFlags = new SortedDictionary<string, bool>();
            }

            if (!isDatabaseDiskFullFlags.ContainsKey(ConfigurationID))
            {
                isDatabaseDiskFullFlags.Add(ConfigurationID, flagValue);
            }
            else
            {
                isDatabaseDiskFullFlags[ConfigurationID] = flagValue;
            }

            return isDatabaseDiskFullFlags[ConfigurationID];
        }
    }
}
