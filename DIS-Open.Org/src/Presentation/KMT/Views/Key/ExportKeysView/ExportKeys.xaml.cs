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
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.ExportKeysView
{
    /// <summary>
    /// Interaction logic for ExportKeys.xaml
    /// </summary>
    public partial class ExportKeysView : Window
    {
        public ExportKeysViewModel VM { get; private set; }

        public ExportKeysView()
        {
            InitializeComponent();
            VM = new ExportKeysViewModel();
            VM.View = this;
            DataContext = VM;

            TypeSelectPageView typeSelectPage = new TypeSelectPageView(VM);
            this.frame_typeselect.Navigate(typeSelectPage);

            KeysSelectPageView keySlectPage = new KeysSelectPageView(VM);
            this.frame_keysselect.Navigate(keySlectPage);

            KeysLogSelectPage keysLogEelectPage = new KeysLogSelectPage(VM);
            this.frame_keyslogselect.Navigate(keysLogEelectPage);

            ExportDuplicateCBR duplicateCBRPage = new ExportDuplicateCBR(VM);
            this.frame_DuplicateCBRslect.Navigate(duplicateCBRPage);

            FinishPageView finishPage = new FinishPageView(VM);
            this.frame_finish.Navigate(finishPage);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this.Window.VM.IsBusy ==true)
            {
                e.Cancel = true;
                MessageBox.Show(DIS.Presentation.KMT.Properties.MergedResources.KeyExport_NotCloseMsg);
            }
        }
       
    }
}
