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

namespace DIS.Presentation.KMT.Views.Notification
{
    /// <summary>
    /// Interaction logic for KeysExpiredNotificationView.xaml
    /// </summary>
    public partial class KeysExpiredNotificationView : Window
    {
        public KeysExpiredNotificationViewModel VM { get; private set; }

        public KeysExpiredNotificationView(List<KeyInfo> keysExpired, int overDays)
        {
            InitializeComponent();
            VM = new KeysExpiredNotificationViewModel(keysExpired, overDays);
            this.DataContext = VM;
        }
    }
}
