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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DIS.Presentation.KMT.ViewModel;
using DIS.Presentation.KMT.ViewModel.Key;
using DIS.Presentation.KMT.AssignKeysView;
using DIS.Presentation.KMT.ViewModel.ControlsViewModel;
using System.Windows.Controls.Primitives;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.Commands;
using DIS.Data.DataContract;

namespace DIS.Presentation.KMT.Views.EditKeysOptionalInfoView
{
    /// <summary>
    /// Interaction logic for EditKeysOptionalInfo.xaml
    /// </summary>
    public partial class EditKeysOptionalInfo : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public EditKeysOptionalInfoViewModel VM { get; private set; }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyProxy"></param>
        public EditKeysOptionalInfo(IKeyProxy keyProxy, List<KeyInfo> keys = null, bool allowSearching = true)
        {
            InitializeComponent();
            VM = new EditKeysOptionalInfoViewModel(keyProxy, keys);
            VM.View = this;

            // Hiden the search context depending on the parameter
            this.scCBRs.Visibility = allowSearching ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            DataContext = VM;
            this.scCBRs.DataContext = VM.SCVM;
        }

        private void Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

            ScrollBar sb = (ScrollBar)e.OriginalSource;
            if (sb.Orientation == Orientation.Vertical
                && e.NewValue == sb.Maximum && e.ScrollEventType == ScrollEventType.ThumbTrack)
            {
                VM.LoadNextPage();
            }
        }

        private void PageChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = (ScrollViewer)e.OriginalSource;
            if (e.VerticalChange > 0
                && e.VerticalOffset == sv.ScrollableHeight)
            {
                VM.LoadNextPage();
            }
        }

        private void SortChanged(object sender, DataGridSortingEventArgs e)
        {
            VM.SortingByColumn(e.Column.SortMemberPath);
            e.Handled = true;
        }

        private void SelectSingleKey(object sender, RoutedEventArgs e)
        {
            VM.SelectSingleKey();
        }
    }
}
