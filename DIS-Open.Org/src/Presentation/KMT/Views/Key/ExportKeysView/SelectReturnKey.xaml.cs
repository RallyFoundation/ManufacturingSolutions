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
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.ExportKeysView
{
    /// <summary>
    /// Interaction logic for SelectReturnKey.xaml
    /// </summary>
    public partial class SelectReturnKeyView : Page
    {
        public ExportKeysViewModel VM { get; private set; }
        public SelectReturnKeyView(ExportKeysViewModel vm)
        {
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
            this.searchControl.DataContext = vm.SearchControlVM;
            this.returnKeysListControl.DataContext = VM.ReturnKeysListModelVM;
            returnKeysListControl.LoadNextPage += new System.EventHandler((s, e) => VM.LoadUpKeys());
            returnKeysListControl.SortingByColumn += new Controls.ReturnKeysListControl.SortingEventHandler((s, e) => VM.SortingByColumn(e.SortMemberPath));
        }

        private void ReturnByKeys_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            ScrollBar sb = (ScrollBar)e.OriginalSource;
            if (sb.Orientation == Orientation.Vertical
                && e.NewValue == sb.Maximum && e.ScrollEventType == ScrollEventType.ThumbTrack)
            {
                VM.LoadUpKeys();
            }
        }

        private void ReturnByKeys_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)e.OriginalSource;
            if (e.VerticalChange > 0
                && e.VerticalOffset == sv.ScrollableHeight)
            {
                VM.LoadUpKeys();
            }
        }

        private void ReturnByKeys_Sorting(object sender, DataGridSortingEventArgs e)
        {
            VM.SortingByColumn(e.Column.SortMemberPath);
            e.Handled = true;
        }
    }
}
