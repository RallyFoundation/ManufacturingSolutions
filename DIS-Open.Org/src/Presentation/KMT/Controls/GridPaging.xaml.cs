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
using DIS.Presentation.KMT.Commands;

namespace DIS.Presentation.KMT.Controls
{
    /// <summary>
    /// GridPaging usercontrol for pagination
    /// </summary>
    public partial class GridPaging : UserControl
    {
        #region Private & Protected methods

        /// <summary>
        /// Get available Page Sizes from configuration
        /// </summary>
        private static string[] strArPageSizeValues = KmtConstants.PageSizeList;

        #endregion

        #region Constructors & Dispose

        /// <summary>
        /// Constructor for initialization
        /// </summary>
        public GridPaging()
        {
            InitializeComponent();
            if (strArPageSizeValues != null)
            {
                for (int i = 0; i < strArPageSizeValues.Length; i++)
                {
                    cmbPageNo.Items.Add(strArPageSizeValues[i]);
                }
                cmbPageNo.SelectedIndex = 0;
            }
            GeneratePages();
            EnableDisableButtons(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set
            {
                SetValue(TotalCountProperty, value);
            }
        }
        /// <summary>
        /// Page Count from Dependency property 
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set
            {
                SetValue(PageCountProperty, value);
            }
        }

        /// <summary>
        ///  Current Page from Dependency property 
        /// </summary>
        public int CurrentPageNumber
        {
            get { return (int)GetValue(CurrentPageNumberProperty); }
            set { SetValue(CurrentPageNumberProperty, value); }
        }

        /// <summary>
        ///  Page Size from Dependency property 
        /// </summary>
        public int ItemsPerPage
        {
            get { return (int)GetValue(ItemsPerPageProperty); }
            set
            {
                SetValue(ItemsPerPageProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <see cref="CurrentPageNumber" /> dependency property
        /// </summary>
        public static DependencyProperty CurrentPageNumberProperty = DependencyProperty.Register("CurrentPageNumber", typeof(int), typeof(GridPaging), new UIPropertyMetadata(1,
                                                                                                                                     OnCurrentPageChanged));
        /// <summary>
        /// Identifies the <see cref="ItemsPerPage" /> dependency property
        /// </summary>
        public static DependencyProperty ItemsPerPageProperty = DependencyProperty.Register("ItemsPerPage", typeof(int), typeof(GridPaging), new UIPropertyMetadata(10,
                                                                                                                                     OnItemsPerPageChanged));
        /// <summary>
        /// Identifies the <see cref="PageCount" /> dependency property
        /// </summary>
        public static DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount", typeof(int), typeof(GridPaging), new UIPropertyMetadata(10,
                                                                                                                                     OnPageCountChanged));
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TotalCountProperty = DependencyProperty.Register("TotalCount", typeof(int), typeof(GridPaging), new UIPropertyMetadata(0,
                                                                                                                                     OnTotalCountChanged));
        /// <summary>
        /// Identifies the <see cref="PageChangedCommand" /> dependency property
        /// </summary>
        public static DependencyProperty PageChangedCommandProperty = DependencyProperty.Register("PageChangedCommand", typeof(DelegateCommand), typeof(GridPaging));

        /// <summary>
        /// Delegate Command for Page Changed event
        /// </summary>
        public DelegateCommand PageChangedCommand
        {
            get { return (DelegateCommand)GetValue(PageChangedCommandProperty); }
            set { SetValue(PageChangedCommandProperty, value); }
        }

        #endregion

        #region Private & Protected methods

        /// <summary>
        /// Set Paging information
        /// </summary>
        private void GeneratePages()
        {
            cmbPageNo.SelectedValue = (cmbPageNo.SelectedValue == null) ? KmtConstants.DefaultPageSize : cmbPageNo.SelectedValue;
            CurrentPageNumber = (CurrentPageNumber == 0) ? 1 : CurrentPageNumber;
            if (txtCurrentPageNo != null)
            {
                txtCurrentPageNo.Text = CurrentPageNumber.ToString();
            }
            ItemsPerPage = Convert.ToInt32(cmbPageNo.SelectedValue.ToString());
            if (txtPageCount != null)
            {
                txtPageCount.Text = (PageCount == 0) ? "1" : PageCount.ToString();
            }
        }

        /// <summary>
        /// Page Count Changed Event for DIS.Presentation.KMT
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridPaging objGrid = d as GridPaging;
            objGrid.txtPageCount.Text = e.NewValue.ToString();
            EnableDisableButtons(objGrid);
        }
        private static void OnTotalCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridPaging objGrid = d as GridPaging;
            if (e.NewValue != null)
            {
                objGrid.txtTotalCount.Text = e.NewValue.ToString();
                objGrid.lblTotal.Visibility = Visibility.Visible;
            }
            else
            {
                objGrid.txtTotalCount.Text = "";
                objGrid.lblTotal.Visibility = Visibility.Hidden;
            }
            EnableDisableButtons(objGrid);
        }
        /// <summary>
        /// Current Page Changed Event for DIS.Presentation.KMT
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridPaging objGrid = d as GridPaging;
            objGrid.txtCurrentPageNo.Text = e.NewValue.ToString();
            EnableDisableButtons(objGrid);
        }

        /// <summary>
        /// Page Size Changed Event for DIS.Presentation.KMT
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnItemsPerPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GridPaging objGrid = d as GridPaging;
            objGrid.cmbPageNo.Text = e.NewValue.ToString();
            EnableDisableButtons(objGrid);
        }

        /// <summary>
        /// Common Function to enable / disable paging buttons
        /// </summary>
        /// <param name="objGrid"></param>
        protected static void EnableDisableButtons(GridPaging objGrid)
        {
            if (null != objGrid.btnNext)
            {
                objGrid.btnNext.IsEnabled = true;
                objGrid.btnLast.IsEnabled = true;
                objGrid.btnFirst.IsEnabled = true;
                objGrid.btnPrev.IsEnabled = true;
                if (objGrid.txtPageCount.Text == "1")
                {
                    objGrid.btnNext.IsEnabled = false;
                    objGrid.btnLast.IsEnabled = false;
                    objGrid.btnFirst.IsEnabled = false;
                    objGrid.btnPrev.IsEnabled = false;
                }
                else if (objGrid.txtCurrentPageNo.Text == objGrid.txtPageCount.Text)
                {
                    objGrid.btnNext.IsEnabled = false;
                    objGrid.btnLast.IsEnabled = false;
                }
                else if (objGrid.txtCurrentPageNo.Text == "1")
                {
                    objGrid.btnPrev.IsEnabled = false;
                    objGrid.btnFirst.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// First page button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageCount > 0)
            {
                CurrentPageNumber = 1;
                GeneratePages();
            }
            if (PageChangedCommand != null)
            {
                PageChangedCommand.Execute();
            }
            EnableDisableButtons(this);
        }

        /// <summary>
        /// Previous page button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageCount > 0)
            {
                CurrentPageNumber = (CurrentPageNumber - 1) < 1 ? 1 : CurrentPageNumber - 1;
                GeneratePages();
            }
            if (PageChangedCommand != null)
            {
                PageChangedCommand.Execute();
            }
            EnableDisableButtons(this);
        }

        /// <summary>
        /// Next page button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageCount > 0)
            {
                CurrentPageNumber = (CurrentPageNumber + 1) > PageCount ? PageCount : CurrentPageNumber + 1;
                GeneratePages();
            }
            if (PageChangedCommand != null)
            {
                PageChangedCommand.Execute();
            }
            EnableDisableButtons(this);
        }

        /// <summary>
        /// Last page button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageCount > 0)
            {
                CurrentPageNumber = PageCount;
                GeneratePages();
            }
            if (PageChangedCommand != null)
            {
                PageChangedCommand.Execute();
            }
            EnableDisableButtons(this);
        }

        /// <summary>
        /// Page size changed event handler for page size dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPageNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PageCount > 0)
            {
                CurrentPageNumber = 1;
                GeneratePages();
            }
            if (PageChangedCommand != null)
            {
                PageChangedCommand.Execute();
            }
            EnableDisableButtons(this);
        }

        #endregion
    }
}
