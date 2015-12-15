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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.ViewModel;
using DIS.Business.Proxy;


namespace DIS.Presentation.KMT.Views.Configuration
{
    /// <summary>
    /// Interaction logic for CertPicker.xaml
    /// </summary>
    public partial class CertPicker : Window
    {
        public CertPickerViewModel VM { get; private set; }

        public CertPicker(IConfigProxy proxy,IHeadQuarterProxy hqProxy)
        {
            InitializeComponent();
            VM = new CertPickerViewModel(proxy,hqProxy);
            VM.View = this;
            this.DataContext = VM;
        }
    }
}
