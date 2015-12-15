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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Key.ReturnKeysView
{
    /// <summary>
    /// Interaction logic for ReturnKeysSelection.xaml
    /// </summary>
    public partial class ReturnKeysSelection : Page
    {
        public ReturnKeysViewModel VM { get; set; }

        public ReturnKeysSelection(ReturnKeysViewModel vm)
        {
            InitializeComponent();
            VM = vm;
            DataContext = VM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Select;
            this.searchControl.DataContext = VM.SearchControlVM;
            this.returnKeysListControl.DataContext = VM.ReturnKeysListModelVM;
            returnKeysListControl.LoadNextPage += new EventHandler((s, e) => VM.LoadUpKeys());
            returnKeysListControl.SortingByColumn += new Controls.ReturnKeysListControl.SortingEventHandler((s, e) => VM.SortingByColumn(e.SortMemberPath));
        }

        #region Private Members

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

        #endregion
    }
}
