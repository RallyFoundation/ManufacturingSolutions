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
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.ReportKeysView
{
    /// <summary>
    /// Interaction logic for KeyReportSearch.xaml
    /// </summary>
    public partial class ReportKeysSelection : Page
    {
        public ReportKeysViewModel VM { get; private set; }

        public ReportKeysSelection(ReportKeysViewModel vm)
        {
            InitializeComponent();
            VM = vm;
            DataContext = VM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Select;
            this.searchControl.DataContext = vm.SearchControlVM;
            keysTab.LoadNextPage += new EventHandler((s, e) => VM.LoadUpKeys());
            keysTab.SortingByColumn += new Controls.KeysTabControl.SortingEventHandler((s, e) => VM.SortingByColumn(e.SortMemberPath));
        }
    }
}
