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
using System;
using System.Windows.Controls.Primitives;

namespace DIS.Presentation.KMT.Controls
{
    /// <summary>
    /// Interaction logic for KeyOperationResultsControl.xaml
    /// </summary>
    public partial class OhrKeyDetailsControl : UserControl
    {
        public event EventHandler LoadNextPage;
        public event DIS.Presentation.KMT.Controls.KeysTabControl.SortingEventHandler SortingByColumn;
        /// <summary>
        /// 
        /// </summary>
        public OhrKeyDetailsControl()
        {
            InitializeComponent();
        }

        private void ExportByKeys_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

            ScrollBar sb = (ScrollBar)e.OriginalSource;

            if (sb.Orientation == Orientation.Vertical
                && e.NewValue == sb.Maximum && e.ScrollEventType == ScrollEventType.ThumbTrack)
            {

                if (LoadNextPage != null)
                    LoadNextPage(this, new EventArgs());
            }

        }

        private void ExportByKeys_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)e.OriginalSource;

            if (e.VerticalChange > 0
                && e.VerticalOffset == sv.ScrollableHeight)
            {

                if (LoadNextPage != null)
                    LoadNextPage(this, new EventArgs());
            }
        }

        private void ExportByKeys_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (SortingByColumn != null)
                SortingByColumn(this, new DIS.Presentation.KMT.Controls.KeysTabControl.SortingEventArgs(e.Column.SortMemberPath));
            e.Handled = true;
        }
    }
}
