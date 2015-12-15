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
using DIS.Business.Proxy;

namespace DIS.Presentation.KMT.View.Configuration
{
    /// <summary>
    /// Interaction logic for DownLevelSystemView.xaml
    /// </summary>
    public partial class DownLevelSystemView : IConfigurationPage
    {
        /// <summary>
        /// 
        /// </summary>
        public DownLevelSystemViewModel VM { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CanSave
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSaved
        {
            get { return VM.IsSaved; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssProxy"></param>
        /// <param name="parent"></param>
        public DownLevelSystemView(ISubsidiaryProxy ssProxy, Window parent)
        {
            InitializeComponent();
            VM = new DownLevelSystemViewModel(ssProxy);
            VM.View = parent;
            DataContext = VM;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get { return false; }
            set { IsBusyChanged(this, new EventArgs()); }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler IsBusyChanged;
    }
}
