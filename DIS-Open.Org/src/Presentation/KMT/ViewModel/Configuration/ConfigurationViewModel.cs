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
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.View.Configuration;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationViewModel : ViewModelBase
    {
        #region private fields

        private Dictionary<string, IConfigurationPage> configPages;
        private bool isManager = KmtConstants.LoginUser.RoleName == Constants.ManagerRoleName;
        private DelegateCommand saveCommand;
        private DelegateCommand cancelCommand;
        private DelegateCommand applyCommand;
        private KeyValuePair<string, IConfigurationPage> selectedPage;
        private bool willCloseWindow;
        private IConfigProxy configProxy;
        private ISubsidiaryProxy ssProxy;
        private IHeadQuarterProxy hqProxy;
        private IUserProxy userProxy;
        private IKeyTypeConfigurationProxy stockProxy;
        private IKeyProxy keyProxy;
        
        private EventHandler isBusyChangedEventHandler;

        #endregion private fields

        #region constuctor

        public ConfigurationViewModel(IConfigProxy configProxyParam, ISubsidiaryProxy ssProxyParam, IHeadQuarterProxy hqProxyParam,
            IUserProxy userProxyParam, IKeyTypeConfigurationProxy stockProxyParam, IKeyProxy keyProxyParam, int? pageIndex)
        {
            ssProxy = ssProxyParam ?? new SubsidiaryProxy();
            configProxy = configProxyParam ?? new ConfigProxy(KmtConstants.LoginUser);
            userProxy = userProxyParam ?? new UserProxy();
            hqProxy = hqProxyParam ?? new HeadQuarterProxy();
            stockProxy = stockProxyParam ?? new KeyTypeConfigurationProxy();
            keyProxy = keyProxyParam ?? new KeyProxy(KmtConstants.LoginUser, KmtConstants.HeadQuarterId);
            this.LoadPages();
            if (pageIndex != null && pageIndex < ConfigPages.Count)
                this.SelectedConfigPage = ConfigPages.ElementAt(pageIndex.Value);
            else
                this.SelectedConfigPage = ConfigPages.ElementAt(0);
        }

        #endregion constructor

        #region public property

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get { return configPages.Any(c => c.Value != null && c.Value.IsBusy); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, IConfigurationPage> ConfigPages
        {
            get { return this.configPages; }
            set
            {
                this.configPages = value;
                RaisePropertyChanged("ConfigPages");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public KeyValuePair<string, IConfigurationPage> SelectedConfigPage
        {
            get
            {
                return this.selectedPage;
            }
            set
            {
                selectedPage = value;
                RaisePropertyChanged("SelectedConfigPage");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new DelegateCommand(() =>
                    {
                        if (configPages.Any(c => c.Value.CanSave))
                        {
                            willCloseWindow = true;
                            CallFrameSave();
                        }
                        else
                            View.Close();
                    });
                return saveCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new DelegateCommand(View.Close);
                return cancelCommand;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ApplyCommand
        {
            get
            {
                if (applyCommand == null)
                    applyCommand = new DelegateCommand(() =>
                    {
                        willCloseWindow = false;
                        CallFrameSave();
                    });
                return applyCommand;
            }
        }
        #endregion public property

        #region private method

        /// <summary>
        /// load configuration pages according to install type and operator
        /// </summary>
        /// <param name="configProxy"></param>
        /// <param name="ssProxy"></param>
        /// <param name="hqProxy"></param>
        /// <param name="userProxy"></param>
        /// <param name="stockProxy"></param>
        /// <param name="keyProxy"></param>
        public void LoadPages()
        {
            this.ConfigPages = new Dictionary<string, IConfigurationPage>();
            if (isManager)
            {
                switch (Constants.InstallType)
                {
                    case InstallType.Oem:
                        {
                            this.ConfigPages.Add(Properties.MergedResources.Configuration_Setting_System, new SystemSettingView(configProxy, hqProxy));
                            this.ConfigPages.Add(MergedResources.Common_ProxySetting, new ProxySettingView(configProxy));
                            this.ConfigPages.Add(ResourcesOfR6.KeyTypeConfigurationView_Title, new KeyTypeConfigurationsView(stockProxy));
                            this.ConfigPages.Add(Properties.MergedResources.DLSView_DLSSetting, new DownLevelSystemView(ssProxy, View));
                            this.ConfigPages.Add(Properties.MergedResources.Configuration_Setting_Account, new AccountSetting(userProxy));
                        }
                        break;
                    case InstallType.Tpi:
                        {
                            this.ConfigPages.Add(Properties.MergedResources.Configuration_Setting_System, new SystemSettingView(configProxy, hqProxy));
                            this.ConfigPages.Add(MergedResources.Common_ProxySetting, new ProxySettingView(configProxy));
                            this.ConfigPages.Add(ResourcesOfR6.KeyTypeConfigurationView_Title, new KeyTypeConfigurationsView(stockProxy));
                            this.ConfigPages.Add(Properties.MergedResources.UpLevelSystemView_ULSSetting, new UpLevelSystemView(configProxy, hqProxy, keyProxy));
                            this.ConfigPages.Add(Properties.MergedResources.DLSView_DLSSetting, new DownLevelSystemView(ssProxy, View));
                            this.ConfigPages.Add(Properties.MergedResources.Configuration_Setting_Account, new AccountSetting(userProxy));
                        }
                        break;
                    case InstallType.FactoryFloor:
                        {
                            this.ConfigPages.Add(Properties.MergedResources.Configuration_Setting_System, new SystemSettingView(configProxy, hqProxy));
                            this.ConfigPages.Add(MergedResources.Common_ProxySetting, new ProxySettingView(configProxy));
                            this.ConfigPages.Add(ResourcesOfR6.KeyTypeConfigurationView_Title, new KeyTypeConfigurationsView(stockProxy));
                            this.ConfigPages.Add(Properties.MergedResources.UpLevelSystemView_ULSSetting, new UpLevelSystemView(configProxy, hqProxy, keyProxy));
                            this.ConfigPages.Add(Properties.MergedResources.Configuration_Setting_Account, new AccountSetting(userProxy));
                        }
                        break;
                }
            }
            else
            {
                this.ConfigPages.Add(Properties.MergedResources.Configuration_Setting_Account, new AccountSetting(userProxy));
            }

            foreach (var p in configPages)
            {
                IConfigurationPage config = (IConfigurationPage)p.Value;
                config.IsBusyChanged -= isBusyChangedEventHandler;
                isBusyChangedEventHandler = new EventHandler(OnIsBusyChanged);
                config.IsBusyChanged += isBusyChangedEventHandler;
            }
        }

        private void CallFrameSave()
        {
            foreach (var p in configPages)
            {
                IConfigurationPage config = (IConfigurationPage)p.Value;
                if (config.CanSave)
                    config.Save();
            }
        }

        private void OnIsBusyChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("IsBusy");
            Dispatch(() =>
            {
                applyCommand.RaiseCanExecuteChanged();
                if (willCloseWindow && this.configPages.All(c => !c.Value.IsBusy && c.Value.IsSaved))
                    View.Close();
            });
        }

        #endregion
    }
}
