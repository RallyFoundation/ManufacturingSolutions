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
using System.Windows.Controls;

namespace DIS.Presentation.KMT.Controls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {   
        /// <summary>
        /// 
        /// </summary>
        public SearchControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility CbStateVisibility
        {
            get { return (Visibility)GetValue(CbStateVisibilityProperty); }
            set
            {
                SetValue(CbStateVisibilityProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CbStateVisibilityProperty = DependencyProperty.Register("CbStateVisibilityProperty", typeof(Visibility), typeof(SearchControl), new UIPropertyMetadata(Visibility.Hidden,
                                                                                                                                     OnCbStateVisibilityChanged));

        private static void OnCbStateVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchControl searchControl = d as SearchControl;
            if (e.NewValue != null)
                searchControl.cbState.Visibility = (Visibility)e.NewValue;
        }
    }
}
