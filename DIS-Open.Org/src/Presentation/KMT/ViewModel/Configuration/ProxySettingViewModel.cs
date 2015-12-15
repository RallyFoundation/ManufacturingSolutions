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
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ProxySettingViewModel : ViewModelBase
    {
        #region private member

        private IConfigProxy configProxy;

        private ProxySetting proxySetting;
        private ProxySetting sourceSetting;
        private string host;
        private string port;
        private bool isBusy;
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ProxySettingViewModel(IConfigProxy configProxy)
        {
            this.configProxy = configProxy;
            loadProxySetting();
            IsSaved = true;
        }

        #region public property

        /// <summary>
        /// 
        /// </summary>
        public string Host
        {
            get { return this.host; }
            set
            {
                this.host = value;
                RaisePropertyChanged("Host");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Port
        {
            get { return this.port; }
            set
            {
                this.port = value;
                RaisePropertyChanged("Port");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ProxyType Type
        {
            get { return this.proxySetting.ProxyType; }
            set
            {
                this.proxySetting.ProxyType = value;
                RaisePropertyChanged("Type");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get { return this.proxySetting.ServiceConfig.UserName; }
            set
            {
                this.proxySetting.ServiceConfig.UserName = value;
                RaisePropertyChanged("UserName");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserKey
        {
            get { return this.proxySetting.ServiceConfig.UserKey; }
            set
            {
                this.proxySetting.ServiceConfig.UserKey = value;
                RaisePropertyChanged("UserKey");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool BypassProxyOnLocal
        {
            get { return this.proxySetting.BypassProxyOnLocal; }
            set 
            {
                this.proxySetting.BypassProxyOnLocal = value;
                RaisePropertyChanged("BypassProxyOnLocal");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler IsBusyChanged;

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsSaved { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged
        {
            get { return this.IsSettingChanged(); }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            if (!IsChanged)
                return;
            if (!this.ValidateProxySetting())
                return;
            IsBusy = true;
            IsSaved = false;

            WorkInBackground((s, e) =>
            {
                try
                {
                    configProxy.UpdateProxySetting(this.proxySetting);
                    string logMsg = string.Empty;
                    switch (this.proxySetting.ProxyType)
                    {
                        case ProxyType.Custom:
                            logMsg = string.Format("Proxy Setting  was changed to Customize, " +
                                " Host:{0},Port:{1}.",
                                this.Host, this.Port);
                            break;
                        case ProxyType.None:
                            logMsg = string.Format("Proxy Setting  was changed to None.");
                            break;
                        case ProxyType.Default:
                            logMsg = string.Format("Proxy Setting  was changed to Default.");
                            break;
                    }

                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, logMsg, KmtConstants.CurrentDBConnectionString);
                    loadProxySetting();
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

        #region private method

        private void loadProxySetting()
        {
            proxySetting = configProxy.GetProxySetting();
            sourceSetting = configProxy.GetProxySetting();
            sourceSetting.ServiceConfig.UserKey = string.Empty;
            proxySetting.ServiceConfig.UserKey = string.Empty;
            RaisePropertyChanged("UserKey");
            Uri u = new Uri(proxySetting.ServiceConfig.ServiceHostUrl);
            this.port = u.Port.ToString();
            this.host = u.Host;
        }

        private bool IsSettingChanged()
        {
            Uri u = new Uri(sourceSetting.ServiceConfig.ServiceHostUrl);
            return this.port != u.Port.ToString() ||
                this.host != u.Host ||
                this.Type != sourceSetting.ProxyType ||
                this.UserName != sourceSetting.ServiceConfig.UserName ||
                this.UserKey.Trim() != sourceSetting.ServiceConfig.UserKey.Trim() ||
                this.BypassProxyOnLocal != sourceSetting.BypassProxyOnLocal;
        }

        private bool ValidateProxySetting() 
        {
            if (this.proxySetting.ProxyType == ProxyType.Custom)
            {
                string hostUrl = ValidationHelper.GetProxyUrl(Host, Port, false);
                if (hostUrl == null)
                    return false;
                this.proxySetting.ServiceConfig.ServiceHostUrl = hostUrl;
                //if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserKey))
                //    return false;     
            }
            return true;
        }

        #endregion

    }
}
