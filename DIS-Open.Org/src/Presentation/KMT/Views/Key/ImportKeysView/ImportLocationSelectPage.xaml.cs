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

using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.ImportKeysView
{
    public partial class ImportLocationSelectPage
    {
        public ImportKeysViewModel VM { get; private set; }

        public ImportLocationSelectPage(ImportKeysViewModel vm)
        {
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
        }

        private void listBoxSelectedFiles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.RemovedItems != null)
            {
                foreach (var item in e.RemovedItems)
                {
                    if (this.VM.ExcludedFiles.Contains(item.ToString()))
                    {
                        this.VM.ExcludedFiles.Remove(item.ToString());
                    }
                }
            }

            if (e.AddedItems != null)
            {
                foreach (var item in e.AddedItems)
                {
                    if (!this.VM.ExcludedFiles.Contains(item.ToString()))
                    {
                        this.VM.ExcludedFiles.Add(item.ToString());
                    }
                }
            }

            this.VM.ExcludedFiles = new System.Collections.Generic.List<string>(this.VM.ExcludedFiles.ToArray());
        }
    }
}
