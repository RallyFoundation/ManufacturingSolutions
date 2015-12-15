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

namespace DIS.Presentation.KMT.ExportKeysView
{
    /// <summary>
    /// Interaction logic for ReturnKeyInputPage.xaml
    /// </summary>
    public partial class ReturnKeyInputPage : Page
    {
        public ExportKeysViewModel VM { get; private set; }
        public ReturnKeyInputPage(ExportKeysViewModel vm)
        {
            InitializeComponent();
            VM = vm;
            DataContext = VM.ReturnKeysListModelVM;
            Title = DIS.Presentation.KMT.Properties.ResourcesOfR6.Common_CreditSelect;
        }
    }
}
