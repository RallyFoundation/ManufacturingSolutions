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
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;
using System;

namespace DIS.Presentation.KMT.OhrDataUpdateView
{
    /// <summary>
    /// Interaction logic for KeysSelectPage.xaml
    /// </summary>
    public partial class OhrKeysEdit : Page
    {
        public OhrDataUpdateViewModel VM { get; set; }

        public OhrKeysEdit(OhrDataUpdateViewModel vm)
        {
            InitializeComponent();

            VM = vm;
            DataContext = VM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Select;

            this.searchControl.DataContext = VM.SearchControlVM;
            this.dgKeys.LoadNextPage += new EventHandler((s, e) => VM.LoadUpKeys());
        }
    }
}
