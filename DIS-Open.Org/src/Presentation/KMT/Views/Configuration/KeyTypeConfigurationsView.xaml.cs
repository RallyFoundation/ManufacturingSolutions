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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DIS.Presentation.KMT.ViewModel;
using DIS.Business.Proxy;

namespace DIS.Presentation.KMT.View.Configuration
{
    /// <summary>
    /// Interaction logic for KeysStockNotificationSetting.xaml
    /// </summary>
    public partial class KeyTypeConfigurationsView : IConfigurationPage
    {
        public KeyTypeConfigurationsViewModel VM { get; private set; }

        public KeyTypeConfigurationsView(IKeyTypeConfigurationProxy stockProxy)
        {
            InitializeComponent();
            VM = new KeyTypeConfigurationsViewModel(stockProxy);
            DataContext = VM;
        }

        public bool IsSaved
        {
            get { return VM.IsSaved; }
        }

        public void Save()
        {
            VM.Save();
        }

        public bool IsBusy
        {
            get { return VM.IsBusy; }
            set { }
        }

        public bool CanSave 
        {
            get { return VM.IsChanged; }
        }

        public event EventHandler IsBusyChanged
        {
            add { VM.IsBusyChanged += value; }
            remove { VM.IsBusyChanged -= value; }
        }
    }
}
