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
using DIS.Presentation.KMT.ViewModel.Key;

namespace DIS.Presentation.KMT.RecallKeysView
{

    /// <summary>
    /// Interaction logic for AssignKeysSelection.xaml
    /// </summary>
    public partial class RecallKeysSelection : Page
    {
        private RecallKeysViewModel VM { get; set; }

        public RecallKeysSelection()
        {
            InitializeComponent();
        }

        public RecallKeysSelection(RecallKeysViewModel vm)
        {
            InitializeComponent();
            VM = vm;
            DataContext = vm;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Select;
            this.searchControl.DataContext = vm.SearchControlVM;
            keysTab.LoadNextPage += new EventHandler((s, e) => VM.LoadUpKeys());
            keysTab.SortingByColumn += new Controls.KeysTabControl.SortingEventHandler((s, e) => VM.SortingByColumn(e.SortMemberPath));
        }
    }
}
