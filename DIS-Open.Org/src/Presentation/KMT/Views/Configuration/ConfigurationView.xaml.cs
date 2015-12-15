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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.ViewModel;
using DIS.Business.Proxy;
using System.Windows.Forms;

namespace DIS.Presentation.KMT.View.Configuration
{
    /// <summary>
    /// Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public ConfigurationViewModel VM { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ConfigurationChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configProxy"></param>
        /// <param name="ssProxy"></param>
        /// <param name="hqProxy"></param>
        /// <param name="userProxy"></param>
        /// <param name="stockProxy"></param>
        /// <param name="keyProxy"></param>
        public ConfigurationView(IConfigProxy configProxy, ISubsidiaryProxy ssProxy, IHeadQuarterProxy hqProxy,
            IUserProxy userProxy, IKeyTypeConfigurationProxy stockProxy, IKeyProxy keyProxy, int? pageIndex)
        {
            InitializeComponent();
            this.reSizeWnd();
            VM = new ConfigurationViewModel(configProxy,ssProxy,hqProxy,userProxy,stockProxy,keyProxy,pageIndex);
            VM.View = this;
            DataContext = VM;
            Closed += this.ConfigurationView_Closed;
        }

        /// <summary>
        ///get screen size to adjust options window
        /// </summary>
        private void reSizeWnd()
        {
            double h = SystemParameters.PrimaryScreenHeight;
            if (h <= 768.0)
                this.grdRoot.Height = 680;
        }

       private void ConfigurationView_Closed(object sender, EventArgs e)
        {
            if (ConfigurationChanged != null)
                ConfigurationChanged(sender, e);
        }
    }
}
