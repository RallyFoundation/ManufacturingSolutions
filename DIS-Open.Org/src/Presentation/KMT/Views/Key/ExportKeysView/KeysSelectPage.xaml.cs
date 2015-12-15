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
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DIS.Presentation.KMT.ExportKeysView
{
    /// <summary>
    /// Interaction logic for KeysSelectPage.xaml
    /// </summary>
    public partial class KeysSelectPageView : Page
    {
        public ExportKeysViewModel VM { get; private set; }

        public KeysSelectPageView(ExportKeysViewModel vm)
        {
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
            this.searchControl.DataContext = vm.SearchControlVM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_SelectKeys;
        }

        private void DataGridTextColumn_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }

        }

        private void ExportByKeys_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            ScrollBar sb = (ScrollBar)e.OriginalSource;
            if (sb.Orientation == Orientation.Vertical
                && e.NewValue == sb.Maximum && e.ScrollEventType == ScrollEventType.ThumbTrack)
            {
                VM.LoadUpKeys();
            }
        }

        private void ExportByKeys_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)e.OriginalSource;
            if (e.VerticalChange > 0
                && e.VerticalOffset == sv.ScrollableHeight)
            {
                VM.LoadUpKeys();
            }
        }

        private void ExportByKeys_Sorting(object sender, DataGridSortingEventArgs e)
        {
            VM.SortingByColumn(e.Column.SortMemberPath);
            e.Handled = true;
        }

    }
}
