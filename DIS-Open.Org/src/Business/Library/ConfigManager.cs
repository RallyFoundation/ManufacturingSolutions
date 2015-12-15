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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DIS.Common.Utility;
using DIS.Data.DataAccess.Repository;
using DIS.Data.DataContract;

namespace DIS.Business.Library
{
    /// <summary>
    /// ConfigManager class
    /// </summary>
    public class ConfigManager : IConfigManager
    {
        #region Private & protected methods

        private const string fulfillmentIntervalName = "FulfillmentInterval";
        private const string reportIntervalName = "ReportInterval";
        private const string isAutoFulfillmentOnName = "IsAutoFulfillmentOn";
        private const string isAutoReportOnName = "IsAutoReportOn";
        private const string isRequireOHRDataName = "IsRequireOHRData";
        private const string isMsServiceEnabledConfigName = "IsMsServiceEnabled";
        private const string internalServiceConfigName = "InternalServiceConfig";
        private const string msServiceConfigName = "MsServiceConfig";
        private const string testingServiceConfigName = "TestingServiceConfig";
        private const string proxySettingConfigName = "ProxySetting";
        private const string oldTimelineName = "OldTimeline";
        private const string isEncryptExportedFile = "IsEncryptExportedFile";
        private const string certificateSubjectConfigName = "CertificateSubject";
        private const string isCarbonCopyEnabledConfigName = "IsCarbonCopyEnabled";
        private const string isAutoDiagnosticName = "IsAutoDiagnostic";

        private IConfigRepository configRepository;

        #endregion

        #region Constructors & Dispose

        public ConfigManager()
            : this(new ConfigRepository())
        {
        }

        public ConfigManager(string dbConnectionString) 
        {
            this.configRepository = new ConfigRepository(dbConnectionString);
        }

        public ConfigManager(IConfigRepository configRepository)
        {
            if (configRepository == null)
                this.configRepository = new ConfigRepository();
            else
                this.configRepository = configRepository;
        }

        #endregion

        #region public Methods

        public double GetFulfillmentInterval()
        {
            return GetIntervalInMinute(GetConfigurationValue<int>(fulfillmentIntervalName));
        }

        public bool GetIsAutoDiagnostic() 
        {
            return GetConfigurationValue<bool>(isAutoDiagnosticName);
        }

        public double GetReportInterval()
        {
            return GetIntervalInMinute(GetConfigurationValue<int>(reportIntervalName));
        }

        public bool GetCanAutoFulfill()
        {
            return GetConfigurationValue<bool>(isAutoFulfillmentOnName);
        }

        public bool GetIsMsServiceEnabled()
        {
            return GetConfigurationValue<bool>(isMsServiceEnabledConfigName);
        }

        public bool GetCanAutoReport()
        {
            return GetConfigurationValue<bool>(isAutoReportOnName);
        }

        public bool GetRequireOHRData()
        {
            return GetConfigurationValue<bool>(isRequireOHRDataName);
        }

        public ServiceConfig GetTestingServiceConfig()
        {
            return GetConfigurationValue<ServiceConfig>(testingServiceConfigName);
        }

        public ServiceConfig GetInternalServiceConfig()
        {
            return GetConfigurationValue<ServiceConfig>(internalServiceConfigName);
        }

        public ServiceConfig GetMsServiceConfig()
        {
            return GetConfigurationValue<ServiceConfig>(msServiceConfigName);
        }

        public ProxySetting GetProxySetting()
        {
            return GetConfigurationValue<ProxySetting>(proxySettingConfigName);
        }

        public int GetOldTimeline()
        {
            return GetConfigurationValue<int>(oldTimelineName);
        }

        public DisCert GetCertificateSubject()
        {
            return GetConfigurationValue<DisCert>(certificateSubjectConfigName);
        }

        public bool GetIsEncryptExportedFile()
        {
            return GetConfigurationValue<bool>(isEncryptExportedFile);
        }

        public bool GetIsCarbonCopyEnabled()
        {
            return GetConfigurationValue<bool>(isCarbonCopyEnabledConfigName);
        }

        public void UpdateAutoDiagnosticSwitch(bool isAutoDiagnostic) 
        {
            UpdateConfiguration(isAutoDiagnosticName, isAutoDiagnostic);
        }

        public void UpdateFulfillmentInterval(int intervalInMinute)
        {
            UpdateConfiguration(fulfillmentIntervalName, GetIntervalInMs(intervalInMinute));
        }

        public void UpdateReportInterval(int intervalInMinute)
        {
            UpdateConfiguration(reportIntervalName, GetIntervalInMs(intervalInMinute));
        }

        public void UpdateAutoFulfillmentSwitch(bool isAutoFulfillmentOn)
        {
            UpdateConfiguration(isAutoFulfillmentOnName, isAutoFulfillmentOn);
        }

        public void UpdateAutoReportSwitch(bool isAutoReportOn)
        {
            UpdateConfiguration(isAutoReportOnName, isAutoReportOn);
        }

        public void UpdateRequireOHRDataSwitch(bool isRequireOHRData)
        {
            UpdateConfiguration(isRequireOHRDataName, isRequireOHRData);
        }

        public void UpdateMsServiceEnabledSwitch(bool isMsServiceEnabled)
        {
            UpdateConfiguration(isMsServiceEnabledConfigName, isMsServiceEnabled);
        }

        public void UpdateTestingServiceConfig(ServiceConfig serviceConfig)
        {
            UpdateConfiguration(testingServiceConfigName, serviceConfig);
        }

        public void UpdateInternalServiceConfig(ServiceConfig serviceConfig)
        {
            UpdateConfiguration(internalServiceConfigName, serviceConfig);
        }

        public void UpdateMsServiceConfig(ServiceConfig serviceConfig)
        {
            UpdateConfiguration(msServiceConfigName, serviceConfig);
        }

        public void UpdateProxySetting(ProxySetting proxySetting)
        {
            UpdateConfiguration(proxySettingConfigName, proxySetting);
        }

        public void UpdateOldTimeline(int days)
        {
            UpdateConfiguration(oldTimelineName, days);
        }

        public void UpdateEncryptExportedFileSwitch(bool isEncrypt)
        {
            UpdateConfiguration(isEncryptExportedFile, isEncrypt);
        }

        public void UpdateCertificateSubject(DisCert certificateSubject)
        {
            UpdateConfiguration(certificateSubjectConfigName, certificateSubject);
        }

        public DiagnosticResult TestDatabaseConnection()
        {
            DiagnosticResult result = new DiagnosticResult();
            try
            {
                configRepository.TestDatabaseConnection();
            }
            catch(Exception ex)
            {
                result.DiagnosticResultType = DiagnosticResultType.Error;
                result.Exception = ex;
            }
            return result;
        }

        #endregion

        #region Private & protected methods

        private T GetConfigurationValue<T>(string name) {
            Configuration cfg = configRepository.GetConfiguration(name);
            if (cfg == null)
                throw new ApplicationException(
                    string.Format("Configuration name {0} cannot be found.", name));
            else
                try {
                    return cfg.Value.FromXml<T>();
                }
                catch (Exception ex) {
                    ExceptionHandler.HandleException(ex, this.configRepository.GetDBConnectionString());
                    throw new DisException("Configuration schema is invalid.");
                }
        }

        private void UpdateConfiguration(string name, object value)
        {
            Configuration config = configRepository.GetConfiguration(name);
            if (config == null)
                throw new ApplicationException(
                    string.Format("Configuration name {0} cannot be found.", name));
            configRepository.UpdateConfiguration(new Configuration()
            {
                ConfigurationId = config.ConfigurationId,
                Name = name,
                Value = value.ToXml().Replace("encoding=\"utf-8\"", "encoding=\"utf-16\""),
                Type = value.GetType().FullName
            });
        }

        private double GetIntervalInMinute(int intervalInMs)
        {
            return (double)intervalInMs / (60 * 1000);
        }

        private int GetIntervalInMs(int intervalInMinute)
        {
            return intervalInMinute * 1000 * 60;
        }

        #endregion
    }
}
