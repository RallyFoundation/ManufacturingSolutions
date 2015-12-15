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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    public class SystemSettingViewModel : ViewModelBase
    {
        #region Priviate & Protected member variables

        private IConfigProxy configProxy;
        private IHeadQuarterProxy hqProxy;

        private bool isBusy;
        private string fulfillmentInterval;
        private string reportInterval;
        private ServiceConfig internalServiceConfig;
        private string internalServiceHost;
        private string internalServicePort;
        private bool isAutoFulfillment;
        private bool isAutoReport;
        private bool isRequireOHRData;
        private string oldTimeLine;
        private Visibility isMsServiceVisible;
        private string certificateSubject;
        private string msServiceConfig;
        private string sourceFulfillmentInterval;
        private string sourceReportInterval;
        private string sourceInternalServiceHost;
        private string sourceInternalServicePort;
        private bool sourceIsAutoFulfillment;
        private bool sourceIsAutoReport;
        private bool sourceIsRequireOHRData;
        private string sourceOldTimeLine;
        private string sourceCertSubject;
        private string sourceMsServiceConfig;
        private string sourceThumbprint;
        private string thumbprint;
        private DisCert selectedCert;
        private DelegateCommand selectCertCommand;

        private bool isAutoReportChanged
        {
            get { return IsAutoReport != sourceIsAutoReport; }
        }

        private bool isRequireOHRDataChanged
        {
            get { return IsRequireOHRData != sourceIsRequireOHRData; }
        }

        private bool isAutoFulfillmentChanged
        {
            get { return IsAutoFulfillment != sourceIsAutoFulfillment; }
        }

        private bool isFulfillIntervalChanged
        {
            get
            {
                return sourceFulfillmentInterval != FulfillmentInterval;
            }
        }

        private bool IsMsServiceConfiglChanged
        {
            get
            {
                return sourceMsServiceConfig != MsServiceConfig;
            }
        }

        private bool isInternalServiceChanged
        {
            get
            {
                return sourceInternalServiceHost != internalServiceHost || sourceInternalServicePort != internalServicePort;
            }
        }

        private bool isReportIntervalChanged
        {
            get
            {
                return sourceReportInterval != ReportInterval;
            }
        }

        private bool isOldTimeLineChanged
        {
            get { return sourceOldTimeLine != OldTimeLine; }
        }

        private bool isCertChanged { get { return sourceThumbprint != thumbprint; } }

        #endregion

        #region Constructors & Dispose

        public SystemSettingViewModel(IConfigProxy configProxy, IHeadQuarterProxy hqProxy)
        {
            this.configProxy = configProxy;
            this.hqProxy = hqProxy;

            if (KmtConstants.IsFactoryFloor)
                this.IsMsServiceVisible = Visibility.Collapsed;
            else if (KmtConstants.IsTpiCorp && (KmtConstants.CurrentHeadQuarter != null && KmtConstants.CurrentHeadQuarter.IsCentralizedMode == true))
                this.IsMsServiceVisible = Visibility.Collapsed;
            else
                this.IsMsServiceVisible = Visibility.Visible;
            LoadConfigurations();
            IsSaved = true;
        }

        #endregion

        #region Public Properties

        public event EventHandler IsBusyChanged;

        public ICommand SelectCertCommand
        {
            get
            {
                if (selectCertCommand == null)
                    selectCertCommand = new DelegateCommand(() =>
                    {
                        this.ShowCertificatePicker();
                    },
                    () => { return true; }
                    );
                return selectCertCommand;
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                if (IsBusyChanged != null)
                    IsBusyChanged(this, new EventArgs());
                RaisePropertyChanged("IsBusy");
            }
        }

        public bool IsSaved { get; private set; }

        public Visibility IsMsServiceVisible
        {
            get { return isMsServiceVisible; }
            set
            {
                isMsServiceVisible = value;
                RaisePropertyChanged("IsMsServiceVisible");
            }
        }

        public bool IsAutoFulfillment
        {
            get { return this.isAutoFulfillment; }
            set
            {
                this.isAutoFulfillment = value;
                RaisePropertyChanged("IsAutoFulfillment");
            }
        }

        public string CertificateSubject
        {
            get { return this.certificateSubject; }
            set
            {
                this.certificateSubject = value;
                RaisePropertyChanged("CertificateSubject");
            }
        }

        public bool IsAutoReport
        {
            get
            {
                return this.isAutoReport;
            }
            set
            {
                this.isAutoReport = value;
                RaisePropertyChanged("IsAutoReport");
            }
        }

        public bool IsRequireOHRData
        {
            get { return this.isRequireOHRData; }
            set
            {
                this.isRequireOHRData = value;
                RaisePropertyChanged("IsRequireOHRData");
            }
        }

        public bool IsChanged
        {
            get
            {
                return isFulfillIntervalChanged
                    || isInternalServiceChanged
                    || isReportIntervalChanged
                    || isOldTimeLineChanged
                    || isAutoFulfillmentChanged
                    || isAutoReportChanged
                    || isRequireOHRDataChanged
                    || isCertChanged
                    || IsMsServiceConfiglChanged;

            }
        }

        public string FulfillmentInterval
        {
            get
            {
                return fulfillmentInterval;
            }
            set
            {
                fulfillmentInterval = value.Trim();
                RaisePropertyChanged("FulfillmentInterval");
            }
        }

        public string ReportInterval
        {
            get { return this.reportInterval; }
            set
            {
                this.reportInterval = value.Trim();
                RaisePropertyChanged("ReportInterval");
            }
        }

        public string InternalServiceHost
        {
            get { return internalServiceHost; }
            set
            {
                internalServiceHost = value.Trim();
                RaisePropertyChanged("InternalServiceHost");
            }
        }

        public string InternalServicePort
        {
            get { return internalServicePort; }
            set
            {
                internalServicePort = value.Trim();
                RaisePropertyChanged("InternalServicePort");
            }
        }

        public string MsServiceConfig
        {
            get { return msServiceConfig; }
            set
            {
                msServiceConfig = value.Trim();
                RaisePropertyChanged("MsServiceConfig");
            }
        }

        public string OldTimeLine
        {
            get { return this.oldTimeLine; }
            set
            {
                this.oldTimeLine = value;
                RaisePropertyChanged("OldTimeLine");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// save configurations to DB
        /// </summary>
        public void Save()
        {
            if (!IsChanged)
                return;

            if (!ValidateConfigurations())
                return;

            IsBusy = true;
            IsSaved = false;
            WorkInBackground((s, e) =>
            {
                try
                {
                    if (isFulfillIntervalChanged)
                    {
                        int interval = int.Parse(FulfillmentInterval);
                        configProxy.UpdateFulfillmentInterval(interval);
                        sourceFulfillmentInterval = FulfillmentInterval;
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Fulfillment Interval was changed to {0} minutes.",
                                interval), KmtConstants.CurrentDBConnectionString);
                    }
                    if (isReportIntervalChanged)
                    {
                        int interval = int.Parse(ReportInterval);
                        configProxy.UpdateReportInterval(interval);
                        sourceReportInterval = ReportInterval;
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Report Interval was changed to {0} minutes.",
                                interval), KmtConstants.CurrentDBConnectionString);
                    }
                    if (isOldTimeLineChanged)
                    {
                        int days = int.Parse(OldTimeLine);
                        configProxy.UpdateOldTimeline(days);
                        sourceOldTimeLine = OldTimeLine;
                        KmtConstants.OldTimeline = days;
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Old Timeline was changed to {0} days.",
                                days), KmtConstants.CurrentDBConnectionString);
                    }
                    if (isAutoFulfillmentChanged)
                    {
                        configProxy.UpdateAutoFulfillmentSwitch(IsAutoFulfillment);
                        sourceIsAutoFulfillment = IsAutoFulfillment;
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Get Fulfillment automatically was changed to {0}.",
                                IsAutoFulfillment.ToString()), KmtConstants.CurrentDBConnectionString);
                    }
                    if (isAutoReportChanged)
                    {
                        configProxy.UpdateAutoReportSwitch(IsAutoReport);
                        sourceIsAutoReport = IsAutoReport;
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Report automatically was changed to {0}.",
                                IsAutoReport.ToString()), KmtConstants.CurrentDBConnectionString);
                    }
                    if (isRequireOHRDataChanged)
                    {
                        configProxy.UpdateRequireOHRDataSwitch(IsRequireOHRData);
                        sourceIsRequireOHRData = IsRequireOHRData;
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Is Require OHR Data was changed to {0}.",
                                IsRequireOHRData.ToString()), KmtConstants.CurrentDBConnectionString);
                    }
                    if (isInternalServiceChanged)
                    {
                        configProxy.UpdateInternalServiceConfig(internalServiceConfig);
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Internal web service url was changed to {0}.",
                                internalServiceConfig.ServiceHostUrl), KmtConstants.CurrentDBConnectionString);
                        sourceInternalServiceHost = InternalServiceHost;
                        sourceInternalServicePort = InternalServicePort;
                    }
                    if (IsMsServiceConfiglChanged)
                    {
                        configProxy.UpdateMsServiceConfig(new ServiceConfig() { ServiceHostUrl = MsServiceConfig });
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Ms web service url was changed to {0}.",
                                MsServiceConfig), KmtConstants.CurrentDBConnectionString);
                        sourceMsServiceConfig = MsServiceConfig;
                    }
                    if (isCertChanged && this.selectedCert != null)
                    {
                        if (KmtConstants.IsOemCorp)
                            configProxy.UpdateCertificateSubject(this.selectedCert);
                        else if (KmtConstants.IsTpiCorp)
                        {

                            //if (KmtConstants.CurrentHeadQuarter == null)
                            //{
                            //    this.hqProxy.InsertHeadQuarter(new HeadQuarter()
                            //    {
                            //        CertSubject = this.selectedCert.Subject,
                            //        CertThumbPrint = this.selectedCert.ThumbPrint,
                            //        IsCentralizedMode = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsTpiInCentralizedMode")), //true,--Change to suppor multiple customer context - Rally
                            //        IsCarbonCopy = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsTpiUsingCarbonCopy"))//false --Change to suppor multiple customer context - Rally
                            //    });
                            //}

                            KmtConstants.CurrentHeadQuarter.CertSubject = this.selectedCert.Subject;
                            KmtConstants.CurrentHeadQuarter.CertThumbPrint = this.selectedCert.ThumbPrint;
                            hqProxy.UpdateHeadQuarter(KmtConstants.CurrentHeadQuarter);
                        }
                        sourceCertSubject = CertificateSubject;
                        sourceThumbprint = thumbprint;
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Microsoft Certificate Subject was changed to {0}", this.selectedCert.Subject), KmtConstants.CurrentDBConnectionString);
                        configProxy.UpdateMsServiceEnabledSwitch(true);
                    }
                    LoadConfigurations();
                    IsSaved = true;
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
            });
        }

        #endregion

        #region Private & Protected methods

        /// <summary>
        ///show certificate picker
        /// </summary>
        private void ShowCertificatePicker()
        {
            Window parent = GetCurrentWindow();
            IntPtr windowHandle = new WindowInteropHelper(parent).Handle;

            X509Certificate2Collection selectedCerts =
                X509Certificate2UI.SelectFromCollection(EncryptionHelper.GetMSCertificates(),
                ResourcesOfR6.CertVM_CertPickerTitle,
                "",
                X509SelectionFlag.SingleSelection,
                windowHandle);
            if (selectedCerts.Count > 0)
            {
                X509Certificate2 sc = selectedCerts[0];
                selectedCert = new DisCert
                {
                    Subject = sc.Subject,
                    ThumbPrint = sc.Thumbprint
                };
                this.CertificateSubject = selectedCert.Subject;
                this.thumbprint = selectedCert.ThumbPrint;
            }
        }

        /// <summary>
        /// load system configurations from DB
        /// </summary>
        private void LoadConfigurations()
        {
            FulfillmentInterval = ((int)configProxy.GetFulfillmentInterval()).ToString();

            ReportInterval = ((int)configProxy.GetReportInterval()).ToString();

            internalServiceConfig = configProxy.GetInternalServiceConfig();
            Uri internalUri = new Uri(internalServiceConfig.ServiceHostUrl);
            InternalServiceHost = internalUri.Host;
            InternalServicePort = internalUri.Port.ToString();

            IsAutoFulfillment = configProxy.GetCanAutoFulfill();
            IsAutoReport = configProxy.GetCanAutoReport();
            IsRequireOHRData = configProxy.GetRequireOHRData();
            OldTimeLine = ((int)configProxy.GetOldTimeline()).ToString();
            MsServiceConfig = configProxy.GetMsServiceConfig().ServiceHostUrl;

            DisCert cert = configProxy.GetCertificateSubject();
            if (KmtConstants.IsOemCorp)
            {
                CertificateSubject = cert.Subject;
                thumbprint = cert.ThumbPrint;
            }

            if (KmtConstants.IsTpiCorp && KmtConstants.CurrentHeadQuarter != null)
            {
                CertificateSubject = KmtConstants.CurrentHeadQuarter.CertSubject;
                thumbprint = KmtConstants.CurrentHeadQuarter.CertThumbPrint;
            }

            sourceCertSubject = CertificateSubject;
            sourceFulfillmentInterval = FulfillmentInterval;
            sourceReportInterval = ReportInterval;
            sourceInternalServiceHost = InternalServiceHost;
            sourceInternalServicePort = InternalServicePort;
            sourceIsAutoFulfillment = IsAutoFulfillment;
            sourceIsAutoReport = IsAutoReport;
            sourceIsRequireOHRData = IsRequireOHRData;
            sourceOldTimeLine = OldTimeLine;
            sourceMsServiceConfig = MsServiceConfig;
            sourceThumbprint = thumbprint;
        }

        private bool ValidateConfigurations()
        {
            int interval0;
            if (!int.TryParse(FulfillmentInterval, out interval0)
                || (interval0 < KmtConstants.MinInterval)
                || (interval0 > KmtConstants.MaxInterval))
            {
                ValidationHelper.ShowMessageBox(string.Format(MergedResources.DataPollingIntervalInvalid, KmtConstants.MinInterval, KmtConstants.MaxInterval), MergedResources.Common_Error);
                return false;
            }

            int interval1;
            if (!int.TryParse(ReportInterval, out interval1)
                || (interval1 < KmtConstants.MinInterval)
                || (interval1 > KmtConstants.MaxInterval))
            {
                ValidationHelper.ShowMessageBox(string.Format(MergedResources.DataPollingIntervalInvalid, KmtConstants.MinInterval, KmtConstants.MaxInterval), MergedResources.Common_Error);
                return false;
            }

            int days;
            if (!int.TryParse(OldTimeLine, out days)
                || (days < KmtConstants.MinLifeDays)
                || (days > KmtConstants.MaxLifeDays))
            {
                ValidationHelper.ShowMessageBox(string.Format(MergedResources.Configuration_InvalidLifeDays, KmtConstants.MinLifeDays, KmtConstants.MaxLifeDays), MergedResources.Common_Error);
                return false;
            }

            internalServiceConfig.ServiceHostUrl = ValidationHelper.GetWebServiceUrl(InternalServiceHost, InternalServicePort);

            if (internalServiceConfig.ServiceHostUrl == null)
                return false;

            if (string.IsNullOrEmpty(msServiceConfig))
                return false;

            return true;
        }

        #endregion
    }
}
