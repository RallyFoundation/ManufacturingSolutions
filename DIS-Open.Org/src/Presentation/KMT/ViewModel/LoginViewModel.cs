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
using System.Threading;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using System.Windows;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// View Model class for Login View
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        #region Priviate & Protected member variables

        private const string loginIDPropertyName = "LoginId";
        private const string passwordPropertyName = "Password";
        private DelegateCommand loginCommand;
        private DelegateCommand cancelCommand;
        private DelegateCommand newCustomerCommand;
        private bool isBusy;

        #endregion

        public LoginViewModel() 
        {
        }

        #region Public Properties

        public string LoginTitle {
            get {
                return string.Format("{0} - {1}", MergedResources.Common_Login, KmtConstants.InventoryName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LoginId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Property for displaying Progress Bar
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
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Command used on clicking Cancel button
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new DelegateCommand(Cancel);
                }
                return cancelCommand;
            }
        }

        /// <summary>
        /// Command used on clicking Login button
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                {
                    loginCommand = new DelegateCommand(Login,
                        () => { return !string.IsNullOrEmpty(LoginId) && !string.IsNullOrEmpty(Password); });
                }
                return loginCommand;
            }
        }

        public ICommand NewCustomerCommand 
        {
            get 
            {
                if (this.newCustomerCommand == null)
                {
                    this.newCustomerCommand = new DelegateCommand(this.NewCustomer);
                }

                return this.newCustomerCommand;
            }
        }

        #endregion

        #region Private & Protected methods

        private void Login()
        {
            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    //Adding support to multiple customer context - Rally Sept. 1st, 2014
                    IUserProxy userProxy = new UserProxy(KmtConstants.CurrentDBConnectionString); //new UserProxy();
                    KmtConstants.LoginUser = userProxy.Login(LoginId, Password);
                    if (KmtConstants.LoginUser != null)
                    {
                        //MessageLogger.ResetLoggingConfiguration("KeyStoreContext", KmtConstants.CurrentDBConnectionString);

                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, "Logged in.", KmtConstants.CurrentDBConnectionString);

                        //Adding support to multiple customer context - Rally Sept. 1st, 2014
                        IConfigProxy configProxy = new ConfigProxy(KmtConstants.LoginUser, KmtConstants.CurrentDBConnectionString, KmtConstants.CurrentConfigurationID, KmtConstants.CurrentCustomerID); //new ConfigProxy(KmtConstants.LoginUser);
                        KmtConstants.OldTimeline = configProxy.GetOldTimeline();

                        //Adding support to multiple customer context - Rally Sept. 1st, 2014
                        IHeadQuarterProxy headQuarterProxy = new HeadQuarterProxy(KmtConstants.CurrentDBConnectionString); //new HeadQuarterProxy();

                        var headQuarters = headQuarterProxy.GetHeadQuarters(KmtConstants.LoginUser);

                        if ((headQuarters != null) && (headQuarters.Count > 0))
                        {
                            KmtConstants.CurrentHeadQuarter = headQuarters.Single(
                               hq => hq.UserHeadQuarters.First().IsDefault);
                        }
                        else if (KmtConstants.IsTpiCorp)//To support TPI decentralized mode in multiple customer context - Rally, Nov 1, 2014
                        {
                            KmtConstants.CurrentHeadQuarter = new HeadQuarter()
                            {
                                IsCentralizedMode = (int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsTpiInCentralizedMode")) == 1), //true,--Change to suppor multiple customer context - Rally
                                IsCarbonCopy = (int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IsTpiUsingCarbonCopy")) == 1)//false --Change to suppor multiple customer context - Rally
                            };

                            headQuarterProxy.InsertHeadQuarter(KmtConstants.CurrentHeadQuarter);
                        }

                        Dispatch(() =>
                        {
                            Thread.CurrentThread.CurrentUICulture = KmtConstants.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = KmtConstants.CurrentCulture;

                            App.Current.MainWindow = new MainWindow();
                            View.Close();
                            App.Current.MainWindow.Show();
                        });
                    }
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
        /// Close the window
        /// </summary>
        private void Cancel()
        {
            View.Close();
            App.Current.Shutdown();
        }

        private void NewCustomer() 
        {
            DISConfigurationCloud.Client.CachingPolicy cachingPolicy = ((DISConfigurationCloud.Client.CachingPolicy)(int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("CloudConfigCachePolicy"))));

            if (cachingPolicy == DISConfigurationCloud.Client.CachingPolicy.LocalOnly)
            {
                try
                {
                    string configCacheToolPath = System.Configuration.ConfigurationManager.AppSettings.Get("CloudConfigCacheToolPath");

                    System.Diagnostics.Process.Start(configCacheToolPath);

                    View.Close();
                    App.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    //ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);

                    //MessageBox.Show(ex.ToString(), "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    MessageBox.Show(MergedResources.DISConfigurationCloud_LocalConfigurationToolErrorMessage, MergedResources.DISConfigurationCloud_LocalConfigurationToolErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);

                    TracingHelper.Trace(new object[] { ex }, "DISConfigurationCloudClientTraceSource");
                }
            }
            else
            {
                try
                {
                    string url = DISConfigurationCloud.Client.ModuleConfiguration.GetServicePoint(System.Configuration.ConfigurationManager.AppSettings.Get("ConfigurationCloudServerAddress"), System.Configuration.ConfigurationManager.AppSettings.Get("NewCustomerUrl"));

                    System.Diagnostics.Process.Start("iexplore.exe", url);

                    View.Close();
                    App.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    ex.ShowDialog();
                    TracingHelper.Trace(new object[] { ex }, "DISConfigurationCloudClientTraceSource");
                }
            }
        }

        #endregion
    }
}
