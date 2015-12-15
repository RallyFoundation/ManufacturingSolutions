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

using System.Windows.Controls;
using DIS.Presentation.KMT.ViewModel.Key;

namespace DIS.Presentation.KMT.RecallKeysView
{
    /// <summary>
    /// Interaction logic for AssignKeysSummary.xaml
    /// </summary>
    public partial class RecallKeysSummary : Page
    {
        public RecallKeysViewModel VM { get; private set; }

        public RecallKeysSummary(RecallKeysViewModel vm)
        {
            InitializeComponent();
            VM = vm;
            this.DataContext = vm;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Summary;
        }
    }
}
