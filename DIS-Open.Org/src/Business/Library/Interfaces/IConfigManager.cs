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

using DIS.Data.DataContract;

namespace DIS.Business.Library
{
    /// <summary>
    /// Configuration Management
    /// </summary>
    public interface IConfigManager
    {
        bool GetIsAutoDiagnostic();

        double GetFulfillmentInterval();

        double GetReportInterval();

        bool GetCanAutoFulfill();

        bool GetCanAutoReport();

        bool GetRequireOHRData();

        bool GetIsMsServiceEnabled();

        bool GetIsCarbonCopyEnabled();

        ServiceConfig GetTestingServiceConfig();

        ServiceConfig GetInternalServiceConfig();

        ServiceConfig GetMsServiceConfig();

        ProxySetting GetProxySetting();

        int GetOldTimeline();

        bool GetIsEncryptExportedFile();

        DisCert GetCertificateSubject();

        void UpdateAutoDiagnosticSwitch(bool isAutoDiagnostic);

        void UpdateFulfillmentInterval(int intervalInMinute);

        void UpdateReportInterval(int intervalInMinute);

        void UpdateAutoFulfillmentSwitch(bool isAutoFulfillmentOn);

        void UpdateAutoReportSwitch(bool isAutoReportOn);

        void UpdateRequireOHRDataSwitch(bool isRequireOHRData);

        void UpdateMsServiceEnabledSwitch(bool isMsServiceEnabled);

        void UpdateTestingServiceConfig(ServiceConfig serviceConfig);

        void UpdateInternalServiceConfig(ServiceConfig serviceConfig);

        void UpdateMsServiceConfig(ServiceConfig serviceConfig);

        void UpdateProxySetting(ProxySetting proxySetting);

        void UpdateOldTimeline(int days);

        void UpdateEncryptExportedFileSwitch(bool isEncrypt);

        void UpdateCertificateSubject(DisCert certificateSubject);

        DiagnosticResult TestDatabaseConnection();

        
    }
}
