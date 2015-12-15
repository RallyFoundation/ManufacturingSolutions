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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DIS.Presentation.KMT.ViewModel;
using DIS.Business.Proxy;

namespace DIS.Presentation.KMT {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        public static MainWindow Current { get; private set; }

        public MainWindowViewModel VM { get; private set; }

        public MainWindow() {
            InitializeComponent();

            Current = this;
            VM = new MainWindowViewModel(frmMain);
            VM.View = this;
            DataContext = VM;

            KeyDown += new KeyEventHandler(FunctionalKeyDown);
            Closed += new EventHandler((s, e) => { App.Current.Shutdown(); });

            frmMain.Navigated += new NavigatedEventHandler((s, e) => {
                Frame f = (Frame)s;
                if (f.CanGoBack)
                    f.RemoveBackEntry();
            });
        }

        private void FunctionalKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.F1)
                VM.OnOpenHelp();
            else if (e.Key == Key.F5) {
                switch (VM.RibbonTabIndex) {
                    case MainWindowViewModel.KeyPageIndex:
                        VM.OnRefreshKeys();
                        break;
                    case MainWindowViewModel.UserPageIndex:
                        VM.OnRefreshUsers();
                        break;
                    case MainWindowViewModel.LogPageIndex:
                        VM.OnRefreshLogs();
                        break;
                }
            }
        }
    }
}
