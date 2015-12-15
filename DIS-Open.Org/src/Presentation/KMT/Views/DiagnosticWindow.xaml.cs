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
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Interaction logic for DiagnosticWindow.xaml
    /// </summary>
    public partial class DiagnosticWindow : Window
    {
        public DiagnosticViewModel VM { get; private set; }

        public DiagnosticWindow(IConfigProxy configProxy, ISubsidiaryProxy ssProxy)
        {
            InitializeComponent();
            VM = new DiagnosticViewModel(configProxy, ssProxy);
            VM.View = this;
            DataContext = VM;
            PreviewKeyDown += new KeyEventHandler(CloseOnEscape);
        }

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                Close();
        }
    }

    
}
