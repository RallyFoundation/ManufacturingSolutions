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
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Notification
{
    /// <summary>
    /// Interaction logic for ReFulfillmentNotificationView.xaml
    /// </summary>
    public partial class ReFulfillmentNotificationView : Window
    {
        public ReFulfillmentNotificationViewModel VM { get; private set; }

        public ReFulfillmentNotificationView(List<FulfillmentInfo> fulfillmentInfoes, IKeyProxy keyProxy)
        {
            InitializeComponent();
            VM = new ReFulfillmentNotificationViewModel(fulfillmentInfoes, keyProxy);
            VM.View = this;
            this.DataContext = VM;
        }
    }
}
