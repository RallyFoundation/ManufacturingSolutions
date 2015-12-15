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
using System.Windows.Controls.Primitives;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.ExportKeysView
{
    /// <summary>
    /// Interaction logic for KeysLogSelectPage.xaml
    /// </summary>
    public partial class KeysLogSelectPage : Page
    {
        public ExportKeysViewModel VM { get; private set; }

        public KeysLogSelectPage(ExportKeysViewModel vm)
        {
            InitializeComponent();
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
            //re-export log Is Encrypted display
            vm.ExportTypeChanged += new EventHandler((s, e) =>
            {
                if (this.VM.IsReCBRChecked == true || this.VM.IsReToolKeyChecked == true)
                    this.dgKeyLogs.Columns[3].Visibility = Visibility.Collapsed;
                else
                    this.dgKeyLogs.Columns[3].Visibility = Visibility.Visible;
            });
        }

        private void ExportLogs_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            ScrollBar sb = (ScrollBar)e.OriginalSource;
            if (sb.Orientation == Orientation.Vertical
                && e.NewValue == sb.Maximum && e.ScrollEventType == ScrollEventType.ThumbTrack)
            {
                VM.LoadUpLogs();
            }
        }

        private void ExportLogs_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)e.OriginalSource;
            if (e.VerticalChange > 0
                && e.VerticalOffset == sv.ScrollableHeight)
            {
                VM.LoadUpLogs();
            }
        }

        private void ExportLogs_Sorting(object sender, DataGridSortingEventArgs e)
        {
            VM.LogSortByColum(e.Column.SortMemberPath);
            e.Handled = true;
        }
    }
}
