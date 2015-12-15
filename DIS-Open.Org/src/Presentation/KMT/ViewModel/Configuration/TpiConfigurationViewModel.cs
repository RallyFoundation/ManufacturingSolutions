using System;
using DIS.Business.Operation;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;
using System.Windows;

namespace DIS.Presentation.KMT.ViewModel
{
    public class TpiConfigurationViewModel : SubsidiariesViewModelBase
    {
        #region Priviate & Protected member variables

        private IConfigProxy configProxy;

        private string getKeysInterval;
        private ServiceConfig oemServiceConfig;
        private ServiceConfig internalServiceConfig;
        private string oemServiceHost;
        private string oemServicePort;
        private string internalServiceHost;
        private string internalServicePort;

        private bool isInternalServiceChanged
        {
            get
            {
                return !configProxy.GetInternalServiceConfig().Equals(internalServiceConfig);
            }
        }

        private bool isGetKeysIntervalChanged
        {
            get
            {
                return ((int)configProxy.GetKeysInterval()).ToString() != GetKeysInterval;
            }
        }

        private bool isOemServiceChanged
        {
            get
            {
                return !configProxy.GetServiceConfig().Equals(oemServiceConfig);
            }
        }

        #endregion

        #region Constructors & Dispose

        public TpiConfigurationViewModel()
            : this(null, null)
        {
        }

        public TpiConfigurationViewModel(IConfigProxy configProxy, ISubsidiaryProxy ssProxy)
            : base(ssProxy)
        {
            if (configProxy == null)
                this.configProxy = new ConfigProxy();
            else
                this.configProxy = configProxy;

            LoadConfigurations();
        }

        #endregion

        #region Public Properties

        public bool IsChanged
        {
            get
            {
                return isGetKeysIntervalChanged || isOemServiceChanged || isInternalServiceChanged;
            }
        }

        public string GetKeysInterval
        {
            get
            {
                return getKeysInterval;
            }
            set
            {
                getKeysInterval = value.Trim();
                RaisePropertyChanged("GetKeysInterval");
            }
        }

        public ServiceConfig OemServiceConfig
        {
            get { return oemServiceConfig; }
            set
            {
                oemServiceConfig = value;
                RaisePropertyChanged("OemServiceConfig");
            }
        }

        public string OemServiceHost
        {
            get { return oemServiceHost; }
            set
            {
                oemServiceHost = value.Trim();
                RaisePropertyChanged("OemServiceHost");
            }
        }

        public string OemServicePort
        {
            get { return oemServicePort; }
            set
            {
                oemServicePort = value.Trim();
                RaisePropertyChanged("OemServicePort");
            }
        }

        public ServiceConfig InternalServiceConfig
        {
            get { return internalServiceConfig; }
            set
            {
                internalServiceConfig = value;
                RaisePropertyChanged("InternalServiceConfig");
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

        #endregion

        #region Public Methods

        public bool Save()
        {
            oemServiceConfig.ServiceHostUrl = ValidationHelper.GetWebServiceUrl(OemServiceHost, OemServicePort, true);
            internalServiceConfig.ServiceHostUrl = ValidationHelper.GetWebServiceUrl(InternalServiceHost, InternalServicePort, false);
            if (!ValidateConfigurations()
                || string.IsNullOrEmpty(oemServiceConfig.ServiceHostUrl)
                || string.IsNullOrEmpty(internalServiceConfig.ServiceHostUrl))
                return false;

            if (!IsChanged)
                return true;

            MessageBoxResult r = MessageBox.Show(Resources.SavingConfigurationConfirmation,
                Resources.ConfirmationTitle, MessageBoxButton.YesNo);
            if (r != MessageBoxResult.Yes)
                return false;

            WorkInBackground((s, e) =>
            {
                try
                {
                    if (isGetKeysIntervalChanged)
                    {
                        int interval = int.Parse(GetKeysInterval);
                        configProxy.UpdateGetKeysInterval(interval);
                        MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                            string.Format("Keys polling interval was changed to {0} minutes.",
                                interval));
                    }
                    if (isOemServiceChanged)
                    {
                        configProxy.UpdateServiceConfig(oemServiceConfig);
                        MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                            string.Format("CKI web service url was changed to {0}. Username is {1}.",
                                oemServiceConfig.ServiceHostUrl, oemServiceConfig.UserName));
                    }
                    if (isInternalServiceChanged)
                    {
                        configProxy.UpdateInternalServiceConfig(internalServiceConfig);
                        MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                            string.Format("Internal web service url was changed to {0}.",
                                internalServiceConfig.ServiceHostUrl));
                    }
                }
                catch (Exception ex)
                {
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex);
                }
            });
            return true;
        }

        #endregion

        #region Private & Protected methods

        private void LoadConfigurations()
        {
            GetKeysInterval = ((int)configProxy.GetKeysInterval()).ToString();
            oemServiceConfig = configProxy.GetServiceConfig();
            internalServiceConfig = configProxy.GetInternalServiceConfig();

            Uri oemUri = new Uri(OemServiceConfig.ServiceHostUrl);
            OemServiceHost = oemUri.Host;
            OemServicePort = oemUri.Port.ToString();

            Uri internalUri = new Uri(InternalServiceConfig.ServiceHostUrl);
            InternalServiceHost = internalUri.Host;
            InternalServicePort = internalUri.Port.ToString();
        }

        private bool ValidateConfigurations()
        {
            int interval;
            if (!int.TryParse(GetKeysInterval, out interval) 
                || (interval < Constant.MinInterval)
                || (interval > Constant.MaxInterval))
            {
                MessageBox.Show(string.Format(Resources.DataPollingIntervalInvalid, Constant.MinInterval, Constant.MaxInterval));
                return false;
            }
            return true;
        }

        #endregion
    }
}
