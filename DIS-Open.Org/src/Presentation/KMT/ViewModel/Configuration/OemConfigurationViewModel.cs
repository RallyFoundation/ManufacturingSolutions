using System;
using DIS.Business.Operation;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;
using System.Windows;

namespace DIS.Presentation.KMT.ViewModel
{
    public class OemConfigurationViewModel : SubsidiariesViewModelBase
    {
        #region Priviate & Protected member variables

        private IConfigProxy configProxy;

        private string fulfillOrderInterval;
        private ServiceConfig msServiceConfig;
        private ServiceConfig internalServiceConfig;
        private string internalServiceHost;
        private string internalServicePort;

        private bool isFulfillIntervalChanged
        {
            get
            {
                return ((int)configProxy.GetFulfillOrderInterval()).ToString() != FulfillOrderInterval;
            }
        }

        private bool isMsServiceChanged
        {
            get
            {
                return !configProxy.GetMsServiceConfig().Equals(msServiceConfig);
            }
        }

        private bool isInternalServiceChanged
        {
            get
            {
                return !configProxy.GetInternalServiceConfig().Equals(internalServiceConfig);
            }
        }

        #endregion

        #region Constructors & Dispose

        public OemConfigurationViewModel()
            : this(null, null)
        {
        }

        public OemConfigurationViewModel(IConfigProxy configProxy, ISubsidiaryProxy ssProxy)
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
                return isFulfillIntervalChanged || isMsServiceChanged || isInternalServiceChanged;
            }
        }

        public string FulfillOrderInterval
        {
            get
            {
                return fulfillOrderInterval;
            }
            set
            {
                fulfillOrderInterval = value.Trim();
                RaisePropertyChanged("FulfillOrderInterval");
            }
        }

        public ServiceConfig MsServiceConfig
        {
            get { return msServiceConfig; }
            set
            {
                msServiceConfig = value;
                RaisePropertyChanged("MsServiceConfig");
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
            internalServiceConfig.ServiceHostUrl = ValidationHelper.GetWebServiceUrl(InternalServiceHost, InternalServicePort, false);
            if (!ValidateConfigurations() || string.IsNullOrEmpty(internalServiceConfig.ServiceHostUrl))
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
                    if (isFulfillIntervalChanged)
                    {
                        int interval = int.Parse(FulfillOrderInterval);
                        configProxy.UpdateFulfillOrderInterval(interval);
                        MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                            string.Format("Order fulfillment interval was changed to {0} minutes.",
                                interval));
                    }
                    if (isMsServiceChanged)
                    {
                        configProxy.UpdateMsServiceConfig(msServiceConfig);
                        MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                            string.Format("Microsoft web service configuration was changed. Username is {0}.",
                                msServiceConfig.UserName));
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
            FulfillOrderInterval = ((int)configProxy.GetFulfillOrderInterval()).ToString();
            MsServiceConfig = configProxy.GetMsServiceConfig();
            InternalServiceConfig = configProxy.GetInternalServiceConfig();

            Uri internalUri = new Uri(InternalServiceConfig.ServiceHostUrl);
            InternalServiceHost = internalUri.Host;
            InternalServicePort = internalUri.Port.ToString();
        }

        private bool ValidateConfigurations()
        {
            int interval;
            if (!int.TryParse(FulfillOrderInterval, out interval) 
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
