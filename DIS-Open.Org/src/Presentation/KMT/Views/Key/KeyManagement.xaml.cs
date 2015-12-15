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

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Interaction logic for KeyManager.xaml
    /// </summary>
    public partial class KeyManagement : Page
    {
        public KeyManagementViewModel VM { get; private set; }

        public KeyManagement(MainWindowViewModel mainVm, IKeyProxy keyProxy, IConfigProxy configProxy,
            ISubsidiaryProxy ssProxy, IHeadQuarterProxy hqProxy)
        {
            InitializeComponent();
            VM = new KeyManagementViewModel(keyProxy, configProxy, ssProxy, hqProxy);
            DataContext = VM;
            mainVm.GetKeys += new EventHandler((s, e) => VM.GetKeys());
            mainVm.RefreshKeys += new EventHandler((s, e) => VM.Refresh());
            mainVm.RefreshSubsidiaries += new EventHandler((s, e) => VM.LoadSubSidiary());

            if (KmtConstants.IsFactoryFloor)
            { 
                grdKeyList.Columns[9].Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void grdKeyList_Sorting(object sender, DataGridSortingEventArgs e)
        {
            VM.SortingByColumn(e.Column.SortMemberPath);
            e.Handled = true;
        }

        private void grdKeyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.VM.KeySelectionChanged();
        }
    }
}
