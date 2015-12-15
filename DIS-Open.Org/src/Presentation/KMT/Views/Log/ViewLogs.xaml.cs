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
using System.Windows.Controls;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.View.Log
{
    public partial class ViewLogs
    {
        public ViewLogsViewModel VM { get; private set; }

        public ViewLogs(MainWindowViewModel mainVm, ILogProxy logProxy)
        {
            this.InitializeComponent();

            VM = new ViewLogsViewModel(logProxy);
            VM.SelectedLogChanged += new EventHandler((s, e) =>
            {
                txtSystemLogDetail.ScrollToHome();
                txtOperationLogDetail.ScrollToHome();
            });
            DataContext = VM;

            mainVm.LogTabChanged += new LogTabChangedEventHandler((s, e) =>
            {
                VM.OnTabChanged(e.LogTabIndex);
            });
            mainVm.RefreshLogs += new EventHandler((s, e) => { VM.Refresh(); });
            mainVm.CurrentUserRoleChanged += new EventHandler((s, e) => { VM.ResetMenu(); });
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            VM.SortByDesc = !VM.SortByDesc;
            if (!string.IsNullOrEmpty(e.Column.SortMemberPath))
            {
                VM.SortedBy = e.Column.SortMemberPath;
                VM.Refresh();
            }
        }
    }
}
