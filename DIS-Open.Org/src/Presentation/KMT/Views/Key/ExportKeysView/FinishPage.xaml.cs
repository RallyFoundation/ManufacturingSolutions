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
using System.IO;

namespace DIS.Presentation.KMT.ExportKeysView
{
    /// <summary>
    /// Interaction logic for FinishPage.xaml
    /// </summary>
    public partial class FinishPageView : Page
    {
        public ExportKeysViewModel VM { get; private set; }

        public FinishPageView(ExportKeysViewModel vm)
        {
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Summary;
        }

        //open exported file folder
        private void btOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.VM.FileName))
            {
                string filePath = this.VM.IsToolKeyChecked ? this.VM.FileName : Path.GetDirectoryName(this.VM.FileName);
                System.Diagnostics.Process.Start("explorer.exe", filePath);
            }
        }
    }
}
