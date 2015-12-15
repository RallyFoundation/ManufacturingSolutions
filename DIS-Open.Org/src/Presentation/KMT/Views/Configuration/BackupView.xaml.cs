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

namespace DIS.Presentation.KMT.View.Configuration
{
    public partial class BackupView : IConfigurationPage
    {
        public BackupView()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        public bool CanSave
        {
            get { return false; }
        }

        public bool IsSaved
        {
            get { return true; }
        }

        public void Save()
        {
        }

        public bool IsBusy
        {
            get { return false; }
            set { IsBusyChanged(this, new EventArgs()); }
        }

        public event EventHandler IsBusyChanged;

        private void chk_backup_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }
    }
}
