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
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;


namespace DIS.Presentation.KMT.ViewModel
{
    public class KeyTypeConfigurationsViewModel : ViewModelBase
    {
        private IKeyTypeConfigurationProxy proxy;
        private bool isBusy;
        private KeyTypeConfiguration currentKeyTypeConfiguration;
        private string min;
        private string max;
        private string msPartNumber;
        private bool enableSelectType;
        private KeyType? keyType;
        private ObservableCollection<KeyTypeConfiguration> keysConfigs;
        private Dictionary<KeyType, string> keyTypes;

        public KeyTypeConfigurationsViewModel(IKeyTypeConfigurationProxy proxyParam)
        {
            this.proxy = proxyParam;
            IsSelectKeyTypeUnmapped = false;

            KeyTypes = new Dictionary<Data.DataContract.KeyType, string>();
            KeyTypes.Add(KeyType.Standard, KeyType.Standard.ToString());
            KeyTypes.Add(KeyType.MBR, KeyType.MBR.ToString());
            KeyTypes.Add(KeyType.MAT, KeyType.MAT.ToString());

            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    Load();
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
            });

            IsSaved = true;
        }

        #region public property

        public Dictionary<KeyType, string> KeyTypes
        {
            get { return this.keyTypes; }
            set
            {
                this.keyTypes = value;
                RaisePropertyChanged("KeyTypes");
            }
        }

        public ObservableCollection<KeyTypeConfiguration> KeyTypeConfigurations
        {
            get { return this.keysConfigs; }
            set
            {
                this.keysConfigs = value;
                RaisePropertyChanged("KeyTypeConfigurations");
            }
        }

        public KeyTypeConfiguration SelectedKeyTypeConfiguration
        {
            get { return this.currentKeyTypeConfiguration; }
            set
            {
                this.currentKeyTypeConfiguration = value;
                this.InitConfigUI();
                RaisePropertyChanged("SelectedKeyTypeConfiguration");
                RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsSelected
        {
            get { return this.SelectedKeyTypeConfiguration != null; }
        }

        public bool IsSelectKeyTypeUnmapped
        {
            get { return this.enableSelectType; }
            set { enableSelectType = value; RaisePropertyChanged("IsSelectKeyTypeUnmapped"); }
        }

        public event EventHandler IsBusyChanged;

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

        public bool IsChanged
        {
            get
            {
                if (this.SelectedKeyTypeConfiguration != null)
                {
                    return Minimum != Convert.ToString(this.SelectedKeyTypeConfiguration.Minimum)
                           || Maxmum != Convert.ToString(this.SelectedKeyTypeConfiguration.Maximum)
                           || CurrentKeyType != this.SelectedKeyTypeConfiguration.KeyType;
                }
                else
                    return false;
            }
        }

        public string MsPartNumber
        {
            get
            {
                return this.msPartNumber;
            }
            set
            {
                this.msPartNumber = value;
                RaisePropertyChanged("MsPartNumber");
            }
        }

        public KeyType? CurrentKeyType
        {
            get { return this.keyType; }
            set
            {
                this.keyType = value;
                RaisePropertyChanged("CurrentKeyType");
            }
        }

        public string Maxmum
        {
            get
            {
                return this.max;
            }
            set
            {
                this.max = value;
                RaisePropertyChanged("Maxmum");
            }
        }

        public string Minimum
        {
            get
            {
                return this.min;
            }
            set
            {
                this.min = value;
                RaisePropertyChanged("Minimum");
            }
        }

        #endregion

        #region public method

        /// <summary>
        /// Save configuration to DB
        /// </summary>
        public void Save()
        {
            if (!this.IsChanged)
                return;
            if (!Build())
                return;

            IsBusy = true;
            IsSaved = false;

            WorkInBackground((s, e) =>
            {
                try
                {
                    bool canMap = this.SelectedKeyTypeConfiguration.KeyType.HasValue && this.IsSelectKeyTypeUnmapped;
                    proxy.UpdateKeyTypeConfiguration(SelectedKeyTypeConfiguration, canMap);
                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                           string.Format("{0} configuration was changed,key Type :{1}, Maxmum:{2}, Minmum:{3}",
                               this.MsPartNumber, CurrentKeyType, this.Maxmum, this.Minimum), KmtConstants.CurrentDBConnectionString);
                    Load();
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

        #region private method

        private void Load()
        {
            this.KeyTypeConfigurations = new ObservableCollection<KeyTypeConfiguration>(proxy.GetKeyTypeConfigurations(KmtConstants.HeadQuarterId));
            this.InitConfigUI();
        }

        /// <summary>
        /// build configuration according UI input
        /// </summary>
        /// <returns></returns>
        private bool Build()
        {
            if (!ValidateConfig())
                return false;

            if (this.currentKeyTypeConfiguration != null)
            {
                if (!string.IsNullOrEmpty(this.max))
                    this.currentKeyTypeConfiguration.Maximum = Convert.ToInt32(this.max);
                if (!string.IsNullOrEmpty(this.min))
                    this.currentKeyTypeConfiguration.Minimum = Convert.ToInt32(this.min);
                if (IsSelectKeyTypeUnmapped)
                {
                    System.Windows.MessageBoxResult r = System.Windows.MessageBox.Show(ResourcesOfR6.KeyTypeConfigurationVM_SaveConfirm,
                        MergedResources.Common_Confirmation, System.Windows.MessageBoxButton.YesNo);
                    if (r != System.Windows.MessageBoxResult.Yes)
                        return false;
                    this.currentKeyTypeConfiguration.KeyType = this.CurrentKeyType;
                }
            }
            else
                return false;
            return true;
        }

        private bool ValidateConfig()
        {
            int max;
            int min;
            max = Convert.ToInt32(Maxmum);
            min = Convert.ToInt32(Minimum);
            if (!(max > min))
            {
                ValidationHelper.ShowMessageBox(MergedResources.KeysStockConfigViewModel_RangeError, MergedResources.Common_Error);
                return false;
            }
            else if (string.IsNullOrEmpty(MsPartNumber))
            {
                ValidationHelper.ShowMessageBox(MergedResources.KeysStockConfigViewModel_MsPartNumberNotSelect, MergedResources.Common_Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initial UI according to selected configuration
        /// </summary>
        private void InitConfigUI()
        {
            if (this.currentKeyTypeConfiguration != null)
            {
                this.MsPartNumber = currentKeyTypeConfiguration.LicensablePartNumber;
                this.CurrentKeyType = currentKeyTypeConfiguration.KeyType;

                if (currentKeyTypeConfiguration.KeyType != null || KmtConstants.IsFactoryFloor)
                    IsSelectKeyTypeUnmapped = false;
                else
                    IsSelectKeyTypeUnmapped = true;

                this.Minimum = Convert.ToString(currentKeyTypeConfiguration.Minimum);
                this.Maxmum = Convert.ToString(currentKeyTypeConfiguration.Maximum);
            }
            else
            {
                this.MsPartNumber = null;
                this.Minimum = null;
                this.Maxmum = null;
            }
        }

        #endregion
    }
}
