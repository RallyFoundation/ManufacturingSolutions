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
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Notification
{
    /// <summary>
    /// Interaction logic for SystemStateNotificationView.xaml
    /// </summary>
    public partial class SystemStateNotificationView : Window
    {
        public SystemStateNotificationViewModel VM { get; private set; }

        public SystemStateNotificationView(string errorTitle, string result)
        {
            InitializeComponent();
            VM = new SystemStateNotificationViewModel(errorTitle, result);
            VM.View = this;
            this.DataContext = VM;
        }
    }
}
