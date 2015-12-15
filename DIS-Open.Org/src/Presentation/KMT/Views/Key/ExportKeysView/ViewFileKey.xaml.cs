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
using System.Windows.Documents;
using DIS.Business.Proxy;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.ExportKeysView
{
    /// <summary>
    /// Interaction logic for ViewFileKey.xaml
    /// </summary>
    public partial class ViewFileKey : Window
    {
        public ViewFileKey(int logId, IKeyProxy keyProxy)
        {
            InitializeComponent();
            List<KeyInfo> keys = keyProxy.GetLogFileKeys(logId);
            this.fileKeyGrid.ItemsSource = keys;
        }
    }
}
