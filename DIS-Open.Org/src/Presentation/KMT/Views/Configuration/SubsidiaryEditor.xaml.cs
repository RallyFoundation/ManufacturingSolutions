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
using DIS.Presentation.KMT.ViewModel;
using DIS.Data.DataContract;
using DIS.Business.Proxy;

namespace DIS.Presentation.KMT.View.Configuration
{
    /// <summary>
    /// Interaction logic for AddSubsidiary.xaml
    /// </summary>
    public partial class SubsidiaryEditor : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public SubsidiaryEditorViewModel VM { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssProxy"></param>
        /// <param name="ss"></param>
        public SubsidiaryEditor(ISubsidiaryProxy ssProxy, Subsidiary ss = null)
        {
            InitializeComponent();

            VM = new SubsidiaryEditorViewModel(ssProxy, ss);
            VM.View = this;
            DataContext = VM;
        }
    }
}
