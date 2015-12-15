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

namespace DIS.Presentation.KMT.Controls
{
    /// <summary>
    /// Interaction logic for ProgressIndicator.xaml
    /// </summary>
    public partial class ProgressIndicator : UserControl
    {
        #region Constructors & Dispose
        
        /// <summary>
        /// 
        /// </summary>
        public ProgressIndicator()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// IsIndeterminate
        /// </summary>
        public bool IsIndeterminate
        {
            get
            {
                return progressBar.IsIndeterminate;
            }
            set
            {
                progressBar.IsIndeterminate = value;
            }
        }

        /// <summary>
        /// Text to be displayed
        /// </summary>
        public string Text
        {
            get
            {
                return textBlock.Text;
            }
            set
            {
                textBlock.Text = value;
            }
        }

        #endregion
    }
}
