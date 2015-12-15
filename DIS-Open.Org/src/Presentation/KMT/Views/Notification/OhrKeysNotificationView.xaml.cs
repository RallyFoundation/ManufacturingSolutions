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
using DIS.Business.Proxy;
using System;

namespace DIS.Presentation.KMT.Views.Notification
{
    /// <summary>
    /// Interaction logic for KeysExpiredNotificationView.xaml
    /// </summary>
    public partial class OhrKeysNotificationView : Window
    {
        public OhrKeysNotificationViewModel VM { get; private set; }

        public OhrKeysNotificationView(List<Ohr> ohrs, IKeyProxy keyProxy)
        {
            InitializeComponent();
            VM = new OhrKeysNotificationViewModel(ohrs, keyProxy);
            this.DataContext = VM;
            this.dgKeys.LoadNextPage += new EventHandler((s, e) => VM.LoadUpKeys());
            this.dgKeys.SortingByColumn += new Controls.KeysTabControl.SortingEventHandler((s, e) => VM.SortingByColumn(e.SortMemberPath));
        }
    }
}
