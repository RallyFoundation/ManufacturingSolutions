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
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    public class UpLevelSystemViewModel : ViewModelBase
    {
        #region private members

        private IHeadQuarterProxy headQuarterProxy;

        private HeadQuarter selectedHeadQuarter;
        private ObservableCollection<HeadQuarter> headQuarters;

        private const int passwordLength = 10;


        private ServiceConfig serviceConfig = new ServiceConfig();

        private string ulsName;
        private string userName;
        private string accessKey;
        private string host;
        private string port;
        private string desc;
        private bool isCarbonCopy;

        private string sourceULSName;
        private string sourceUserName;
        private string sourceAccessKey;
        private string sourceHost;
        private string sourcePort;
        private string sourceDescription;
        private bool sourceCarbonCopy;
        private bool isCentralizedMode;

        private bool isBusy;

        #endregion

        public UpLevelSystemViewModel(IHeadQuarterProxy headQuarter)
        {
            this.headQuarterProxy = headQuarter;
            LoadHeadQuarters();
            IsSaved = true;
        }

        #region Public Property

        public string InstallMode
        {
            get
            {
                if (KmtConstants.CurrentHeadQuarter != null && !KmtConstants.IsFactoryFloor)
                    return (KmtConstants.CurrentHeadQuarter.IsCentralizedMode) ? ResourcesOfR6.ULSView_Centralize : ResourcesOfR6.ULSView_Decentralize;
                return "";
            }
        }

        public event EventHandler IsBusyChanged;

        public Visibility IsCarbonCopyVisible
        {
            get { return (KmtConstants.IsTpiCorp && !isCentralizedMode) ? Visibility.Visible : Visibility.Hidden; }
        }

        public bool EnableCorbonCopy
        {
            get { return KmtConstants.IsTpiCorp && KmtConstants.CurrentHeadQuarter != null && !KmtConstants.CurrentHeadQuarter.IsCentralizedMode; }
        }

        public HeadQuarter SelectedHeadQuarter
        {
            get
            {
                return selectedHeadQuarter;
            }
            set
            {
                if (value != null)
                {
                    selectedHeadQuarter = value;
                    RaisePropertyChanged("SelectedHeadQuarter");
                }
            }
        }

        public bool EditMode { get; set; }

        public bool IsSaved { get; private set; }

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

        public string ULSName
        {
            get { return this.ulsName; }
            set
            {
                this.ulsName = value;
                RaisePropertyChanged("ULSName");
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string AccessKey
        {
            get
            {
                return accessKey;
            }
            set
            {
                accessKey = value;
                RaisePropertyChanged("AccessKey");
            }
        }

        public string Host
        {
            get { return this.host; }
            set
            {
                this.host = value;
                RaisePropertyChanged("Host");
            }
        }

        public string Port
        {
            get { return this.port; }
            set
            {
                this.port = value;
                RaisePropertyChanged("Port");
            }
        }

        public string Description
        {
            get
            {
                return desc;
            }
            set
            {
                desc = value;
                RaisePropertyChanged("Description");
            }
        }

        public bool IsCarbonCopy
        {
            get
            {
                return isCarbonCopy;
            }
            set
            {
                isCarbonCopy = value;
                RaisePropertyChanged("IsCarbonCopy");
            }
        }

        public bool IsChanged
        {
            get
            {
                return IsPropertyValueChanged(sourceDescription, Description)
                            || IsPropertyValueChanged(sourceAccessKey, AccessKey)
                            || isCarbonCopy != sourceCarbonCopy
                            || IsPropertyValueChanged(sourceULSName, ULSName)
                            || IsPropertyValueChanged(sourceUserName, UserName)
                            || IsPropertyValueChanged(sourceHost, Host)
                            || IsPropertyValueChanged(sourcePort, Port);
            }
        }


        public ObservableCollection<HeadQuarter> HeadQuarters
        {
            get { return headQuarters; }
            set
            {
                headQuarters = value;
                RaisePropertyChanged("HeadQuarters");
            }
        }

        #endregion

        #region private method

        /// <summary>
        /// load headquarters from DB
        /// </summary>
        private void LoadHeadQuarters()
        {
            HeadQuarters = new ObservableCollection<HeadQuarter>(headQuarterProxy.GetHeadQuarters(KmtConstants.LoginUser));
            if (HeadQuarters.Count <= 0)
            {
                AddHeadQuarter();
            }
            else
            {
                SelectedHeadQuarter = HeadQuarters[0];
                Initialize(SelectedHeadQuarter);
                EditMode = true;
            }
        }

        /// <summary>
        /// Add SelectedHeadQuarter
        /// </summary>
        private void AddHeadQuarter()
        {
            SelectedHeadQuarter = new Data.DataContract.HeadQuarter();
            SelectedHeadQuarter.IsCarbonCopy = false;
            SelectedHeadQuarter.IsCentralizedMode = true;
            HeadQuarters.Add(SelectedHeadQuarter);
            this.Initialize(SelectedHeadQuarter);
            EditMode = false;
        }

        /// <summary>
        /// Edit the selected SelectedHeadQuarter from list
        /// </summary>
        private void EditHeadQuarter()
        {
            EditMode = true;
            this.Initialize(SelectedHeadQuarter);
        }

        /// <summary>
        /// Remove the selected SelectedHeadQuarter from list
        /// </summary>
        private void RemoveHeadQuarter()
        {
            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    headQuarterProxy.DeleteHeadQuarter(selectedHeadQuarter.HeadQuarterId);
                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                        string.Format("Up Level System {0} has been removed.", selectedHeadQuarter.DisplayName), KmtConstants.CurrentDBConnectionString);
                    LoadHeadQuarters();
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ex.ShowDialog(MergedResources.ConfigViewModel_DeleteSubSidiary);
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }

            });
        }

        /// <summary>
        /// Initialize UI according selected HeadQuarter
        /// </summary>
        /// <param name="current"></param>
        private void Initialize(HeadQuarter current)
        {
            try
            {
                ULSName = current.DisplayName;
                UserName = current.UserName;
                AccessKey = current.AccessKey;
                Description = current.Description;
                IsCarbonCopy = current.IsCarbonCopy;
                if (!string.IsNullOrEmpty(current.ServiceHostUrl))
                {
                    Uri uri = new Uri(current.ServiceHostUrl);
                    Host = uri.Host;
                    Port = uri.Port.ToString();
                }

                sourceULSName = current.DisplayName;
                sourceUserName = current.UserName;
                sourceHost = Host;
                sourcePort = Port;
                sourceDescription = current.Description;
                sourceAccessKey = AccessKey;
                sourceCarbonCopy = IsCarbonCopy;
                isCentralizedMode = current.IsCentralizedMode;
            }
            catch (Exception ex)
            {
                ex.ShowDialog();
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
            }
        }

        /// <summary>
        /// sava changes to DB
        /// </summary>
        public void Save()
        {
            if (!Build())
                return;

            IsBusy = true;
            IsSaved = false;
            WorkInBackground((s, e) =>
            {
                try
                {
                    if (!EditMode)
                    {
                        headQuarterProxy.InsertHeadQuarter(SelectedHeadQuarter);
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Up Level System {0} has been added.", SelectedHeadQuarter.DisplayName), KmtConstants.CurrentDBConnectionString);
                    }
                    else
                    {
                        headQuarterProxy.UpdateHeadQuarter(SelectedHeadQuarter);
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                             string.Format("Up Level System {0} has been updated.", SelectedHeadQuarter.DisplayName), KmtConstants.CurrentDBConnectionString);
                    }

                    KmtConstants.CurrentHeadQuarter = SelectedHeadQuarter;
                    if (MainWindow.Current != null)
                        MainWindow.Current.VM.OnCurrentHeadQuarterChanged();
                    LoadHeadQuarters();

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

        /// <summary>
        /// build a HeadQuarter according to UI input
        /// </summary>
        /// <returns></returns>
        private bool Build()
        {
            SelectedHeadQuarter.Description = Description == null ? null : Description.Trim(); ;
            SelectedHeadQuarter.AccessKey = AccessKey;
            SelectedHeadQuarter.UserName = UserName == null ? null : UserName.Trim();
            SelectedHeadQuarter.IsCarbonCopy = IsCarbonCopy;
            SelectedHeadQuarter.DisplayName = ULSName == null ? null : ULSName.Trim();

            if (!string.IsNullOrEmpty(Host) || !string.IsNullOrEmpty(Port))
            {
                serviceConfig.ServiceHostUrl = ValidationHelper.GetWebServiceUrl(Host.Trim(), Port.Trim());
                if (serviceConfig.ServiceHostUrl != null)
                {
                    serviceConfig.UserName = UserName;
                    serviceConfig.UserKey = AccessKey;
                    SelectedHeadQuarter.ServiceHostUrl = serviceConfig.ServiceHostUrl;
                }
                else
                    return false;
            }
            else
            {
                SelectedHeadQuarter.ServiceHostUrl = string.Empty;
            }

            string error = ValidateHeadQuarter();
            if (error != null)
            {
                ValidationHelper.ShowMessageBox(error, MergedResources.Common_Error);
                return false;
            }

            return true;
        }

        private bool IsPropertyValueChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue) && string.IsNullOrEmpty(newValue))
                return false;
            else
                return oldValue != newValue;
        }

        private string ValidateHeadQuarter()
        {
            List<HeadQuarter> headQuarters = headQuarterProxy.GetHeadQuarters(KmtConstants.LoginUser);
            if (string.IsNullOrEmpty(SelectedHeadQuarter.DisplayName))
                return ResourcesOfR6.ULSVM_ULSNameNotEmpty;
            if (headQuarters.Any(hq => hq.DisplayName == SelectedHeadQuarter.DisplayName && hq.HeadQuarterId != SelectedHeadQuarter.HeadQuarterId))
                return ResourcesOfR6.ULSVM_ULSNameExists;
            return null;
        }

        #endregion
    }
}
