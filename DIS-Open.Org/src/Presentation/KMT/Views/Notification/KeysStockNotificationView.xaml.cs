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

using System.Collections.Generic;
using System.Windows;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.NotificationViews
{
    /// <summary>
    /// Interaction logic for KeysStockNotificationView.xaml
    /// </summary>
    public partial class KeysStockNotificationView : Window
    {
        public KeysStockNotificationViewModel VM { get; private set; }

        public KeysStockNotificationView(List<KeyTypeConfiguration> configs)
        {
            InitializeComponent();
            VM = new KeysStockNotificationViewModel(configs);
            VM.View = this;
            this.DataContext = VM;
        }
    }
}
