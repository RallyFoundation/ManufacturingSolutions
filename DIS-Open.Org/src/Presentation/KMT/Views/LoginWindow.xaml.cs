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

using System.Windows;
using System.Collections.Generic;
using DIS.Presentation.KMT.ViewModel;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        protected List<Customer> customers = new List<Customer>();
        public LoginViewModel VM { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            VM = new LoginViewModel();
            VM.View = this;
            DataContext = VM;
            //txtUserName.Focus();

            this.comboBoxCustomers.Focus();
        }

        private void comboBoxCustomers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Customer customer = this.comboBoxCustomers.SelectedItem as Customer;

            if (customer != null)
            {
                KmtConstants.CurrentCustomerID = customer.ID;

                if (customer.Configurations != null)
                {
                    foreach (var configuration in customer.Configurations)
                    {
                        if (configuration.ConfigurationType.ToString().ToLower() == DIS.Data.DataContract.Constants.InstallType.ToString().ToLower())
                        {
                            KmtConstants.CurrentConfigurationID = configuration.ID;
                            KmtConstants.CurrentDBConnectionString = configuration.DbConnectionString;
                            break;
                        }
                    }
                }
            }
        }

        private void comboBoxCustomers_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.getCloudCustomers();

            this.initControls();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.isCloudConnectionAvialable())
                {
                    this.getCloudCustomers();
                    this.initControls();
                }
                else
                {
                    string errorMessage = DIS.Presentation.KMT.Properties.MergedResources.DISConfigurationCloud_ConnectivityErrorMessage;

                    errorMessage = string.Format(errorMessage, DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint);

                    string errorTitle = DIS.Presentation.KMT.Properties.MergedResources.DISConfigurationCloud_ConnectivityErrorTitle;

                    if (MessageBox.Show(errorMessage, errorTitle, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                    {
                        this.Close();
                        App.Current.Shutdown();
                    }
                }
            }
            catch (System.Exception ex)
            {
                string errorMessage = DIS.Presentation.KMT.Properties.MergedResources.DISConfigurationCloud_ConfigurationErrorMessage;

                string errorTitle = DIS.Presentation.KMT.Properties.MergedResources.DISConfigurationCloud_ConfigurationErrorTitle;

                DIS.Common.Utility.TracingHelper.Trace(new object[] { ex }, "DISConfigurationCloudClientTraceSource");

                if (MessageBox.Show(errorMessage, errorTitle, MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    this.Close();
                    App.Current.Shutdown();
                }
            }
        }

        private bool isCloudConnectionAvialable() 
        {
            if (DISConfigurationCloud.Client.ModuleConfiguration.CachingPolicy == CachingPolicy.LocalOnly)
            {
                return true;
            }

            string testResult = "";

            try
            {
                IManager manager = new Manager(DISConfigurationCloud.Client.ModuleConfiguration.IsTracingEnabled, DISConfigurationCloud.Client.ModuleConfiguration.TraceSourceName);

                testResult = manager.Test();
            }
            catch (System.Exception ex)
            {
                testResult = ex.ToString();
            }

            return (testResult == "Hello!");
        }

        private void getCloudCustomers()
        {
            Customer[] custs = null;

            IManager manager = new Manager(DISConfigurationCloud.Client.ModuleConfiguration.IsTracingEnabled, DISConfigurationCloud.Client.ModuleConfiguration.TraceSourceName);

            custs = manager.GetCustomers();

            List<Customer> customerList = custs != null ? new List<Customer>(custs) : new List<Customer>();

            int matchCount = -1;

            for (int i = 0; i < customerList.Count; i++)
            {
                if (customerList[i].Configurations == null)
                {
                    customerList.RemoveAt(i);
                    i--;
                }
                else
                {
                    matchCount = 0;

                    foreach (var configuration in customerList[i].Configurations)
                    {
                        if (configuration.ConfigurationType.ToString().ToLower() == DIS.Data.DataContract.Constants.InstallType.ToString().ToLower())
                        {
                            matchCount++;
                        }
                    }

                    if (matchCount == 0)
                    {
                        customerList.RemoveAt(i);
                        i--;
                    }
                }
            }

            this.customers = new List<Customer>(customerList.ToArray());

            this.customers.Sort(new DISConfigurationCloud.Contract.CustomerNameComparer() { SortDirection = ComparisonSortDirection.Ascending});

            KmtConstants.CloudCustomers = new List<Customer>(customerList.ToArray());
        }

        private void initControls() 
        {
            this.comboBoxCustomers.ItemsSource = this.customers;

            if ((this.customers != null) && (this.customers.Count > 0))
            {
                this.comboBoxCustomers.SelectedIndex = 0;

                this.comboBoxCustomers_SelectionChanged(this, null);

                this.lblNewCustomer.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                KmtConstants.CurrentDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KeyStoreContext"].ConnectionString;
                this.comboBoxCustomers.Visibility = System.Windows.Visibility.Collapsed;
                this.lblCustomer.Visibility = System.Windows.Visibility.Visible;
                this.lblNewCustomer.Visibility = System.Windows.Visibility.Visible;

                //When there is not any business, user should not be able to login - Rally Nov 25, 2014 (Bug #39)
                this.txtUserName.IsEnabled = false;
                this.pwdPwd.IsEnabled = false;
                this.btnLogin.IsEnabled = false;
            }
            
            if(this.customers.Count == 1)
            {
                this.comboBoxCustomers.Visibility = System.Windows.Visibility.Collapsed;
                this.lblCustomer.Visibility = System.Windows.Visibility.Visible;
                this.lblNewCustomer.Visibility = System.Windows.Visibility.Visible;
                this.lblNewCustomer.Content = this.customers[0].Name;
            }
        }
    }
}