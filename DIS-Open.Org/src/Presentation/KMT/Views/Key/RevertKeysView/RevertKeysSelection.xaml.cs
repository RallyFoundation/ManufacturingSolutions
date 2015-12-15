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

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.ViewModel;
using System.Windows.Controls.Primitives;

namespace DIS.Presentation.KMT.RevertKeysView
{
    /// <summary>
    /// Interaction logic for KeySelectView.xaml
    /// </summary>
    public partial class RevertKeysSelection : Page
    {
        public RevertKeysViewModel VM { get; private set; }

        public RevertKeysSelection(RevertKeysViewModel vm) 
        {
            this.InitializeComponent();
            this.VM = vm;
            DataContext = this.VM;
            this.searchControl.DataContext = VM.SearchControlVM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Select;
        }

        #region Private Members

        private void RevertByKeys_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

            ScrollBar sb = (ScrollBar)e.OriginalSource;

            if (sb.Orientation == Orientation.Vertical
                && e.NewValue == sb.Maximum && e.ScrollEventType == ScrollEventType.ThumbTrack)
            {

                VM.LoadUpKeys();
            }

        }

        private void RevertByKeys_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)e.OriginalSource;

            if (e.VerticalChange > 0
                && e.VerticalOffset == sv.ScrollableHeight)
            {

                VM.LoadUpKeys();
            }
        }

        private void RevertByKeys_Sorting(object sender, DataGridSortingEventArgs e)
        {
            VM.SortingByColumn(e.Column.SortMemberPath);
            e.Handled = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RevertKeysViewModel viewModel = (RevertKeysViewModel)this.DataContext;
            ObservableCollection<KeyInfoModel> keyInfoCollection = viewModel.Keys;
            if (keyInfoCollection != null && keyInfoCollection.Count > 0)
            {
                foreach (var keyInfo in keyInfoCollection)
                {
                    if (!keyInfo.IsSelected)
                        keyInfo.IsSelected = true;
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RevertKeysViewModel viewModel = (RevertKeysViewModel)this.DataContext;
            ObservableCollection<KeyInfoModel> keyInfoCollection = viewModel.Keys;
            if (keyInfoCollection != null && keyInfoCollection.Count > 0)
            {
                foreach (var keyInfo in keyInfoCollection)
                {
                    if (keyInfo.IsSelected)
                        keyInfo.IsSelected = false;
                }
            }
        }

        #endregion Private Members
    }
}
