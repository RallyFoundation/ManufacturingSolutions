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

using System.ComponentModel;
using System.Windows;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.View.Log
{
    /// <summary>
    /// Interaction logic for ExportLogView.xaml
    /// </summary>
    public partial class ExportLogs : Window
    {
        public ExportLogsViewModel VM { get; private set; }

        public ExportLogs(ILogProxy logProxy)
        {
            this.InitializeComponent();

            VM = new ExportLogsViewModel(logProxy);
            VM.View = this;
            DataContext = VM;

            Closing += new CancelEventHandler((s, e) =>
            {
                VM.CancelExporting();
            });
        }
    }
}
