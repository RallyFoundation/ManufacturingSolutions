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

using System.Windows;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.ViewModel.Key;

namespace DIS.Presentation.KMT.Views.Key.DuplicateKeysView
{
    public partial class DuplicateKeys : Window
    {
        public DuplicateKeysViewModel VM {get;private set;}

        public DuplicateKeys(IKeyProxy keyProxy, ISubsidiaryProxy ssProxy)
        {
            InitializeComponent();
            VM = new DuplicateKeysViewModel(keyProxy,ssProxy);
            VM.View = this;
            DataContext = VM;
        }
    }
}
