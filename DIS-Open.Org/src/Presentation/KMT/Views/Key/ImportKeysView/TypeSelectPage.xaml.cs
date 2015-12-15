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
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.ImportKeysView
{
    /// <summary>
    /// Interaction logic for TypeSelectPage.xaml
    /// </summary>
    public partial class TypeSelectPage : Page
    {
        public ImportKeysViewModel VM { get; private set; }

        public TypeSelectPage(ImportKeysViewModel vm)
        {
            InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.ExportKeysViewModel_SelectType;
        }
    }
}
