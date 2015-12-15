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
using System.Windows.Shapes;
using DIS.Presentation.KMT.ViewModel;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.Views.Notification
{
    /// <summary>
    /// Interaction logic for ReportMsFaildNotificationView.xaml
    /// </summary>
    public partial class ReportMsFaildNotificationView : Window
    {
        public ReportMsFailedNotificationViewModel VM { get; private set; }

        public ReportMsFaildNotificationView(NotificationCategory category, List<KeyInfo> keys)
        {
            InitializeComponent();
            VM = new ReportMsFailedNotificationViewModel(category, keys);
            this.DataContext = VM;
        }
    }
}
