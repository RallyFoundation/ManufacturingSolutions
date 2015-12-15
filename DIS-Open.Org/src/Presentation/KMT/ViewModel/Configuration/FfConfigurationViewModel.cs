using System;
using System.Windows;
using DIS.Business.Operation;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    public class FfConfigurationViewModel : ViewModelBase
    {
        #region Priviate & Protected member variables

        private IConfigProxy configProxy;

        private ServiceConfig serviceConfig;
        private string serviceHost;
        private string servicePort;

        private bool isServiceChanged
        {
            get
            {
                return !configProxy.GetServiceConfig().Equals(serviceConfig);
            }
        }

        #endregion

        #region Constructors & Dispose

        public FfConfigurationViewModel()
            : this(null)
        {
        }

        public FfConfigurationViewModel(IConfigProxy configProxy)
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
            get { return isServiceChanged; }
        }

        public ServiceConfig ServiceConfig
        {
            get { return serviceConfig; }
            set
            {
                serviceConfig = value;
                RaisePropertyChanged("ServiceConfig");
            }
        }

        public string ServiceHost
        {
            get { return serviceHost; }
            set
            {
                serviceHost = value.Trim();
                RaisePropertyChanged("ServiceHost");
            }
        }

        public string ServicePort
        {
            get { return servicePort; }
            set
            {
                servicePort = value.Trim();
                RaisePropertyChanged("ServicePort");
            }
        }

        #endregion

        #region Public Methods

        public bool Save()
        {
            ServiceConfig.ServiceHostUrl = ValidationHelper.GetWebServiceUrl(ServiceHost, ServicePort, true);
            if (string.IsNullOrEmpty(ServiceConfig.ServiceHostUrl))
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
                    if (isServiceChanged)
                    {
                        configProxy.UpdateServiceConfig(serviceConfig);
                        MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                            string.Format("CKI/FKI web service url was changed to {0}. Username is {1}.",
                                serviceConfig.ServiceHostUrl, serviceConfig.UserName));
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
            serviceConfig = configProxy.GetServiceConfig();
            Uri serviceUri = new Uri(ServiceConfig.ServiceHostUrl);
            ServiceHost = serviceUri.Host;
            ServicePort = serviceUri.Port.ToString();
        }

        #endregion
    }
}
