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
using System.Windows;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;

namespace DIS.Presentation.KMT.Views.Key
{
    /// <summary>
    /// Interaction logic for SetpWizard.xaml
    /// </summary>
    public partial class StepWizard : Window
    {
        public TemplateViewModelBase VM { get; private set; }

        public StepWizard()
        {
            InitializeComponent();
        }

        protected void InitViewModel(TemplateViewModelBase vm)
        {
            DataContext = vm;
            this.VM = vm;
            this.frameList.Navigate(vm.StepPages[vm.CurrentPageIndex]);
            vm.CurrentPageIndexChanged += new EventHandler((s, e) => this.frameList.Navigate(vm.StepPages[vm.CurrentPageIndex]));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (VM.IsBusy)
            {
                e.Cancel = true;
                MessageBox.Show(DIS.Presentation.KMT.Properties.MergedResources.InProcessingMsg, DIS.Presentation.KMT.Properties.MergedResources.Common_Warning);
            }
        }

    }
}
