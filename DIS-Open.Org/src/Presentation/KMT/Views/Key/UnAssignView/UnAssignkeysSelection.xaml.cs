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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.ViewModel;
using DIS.Presentation.KMT.Models;

namespace DIS.Presentation.KMT.UnAssignView
{
    /// <summary>
    /// Interaction logic for ProductkeyselectView.xaml
    /// </summary>
    public partial class UnAssignkeysSelection : Page
    {
        public UnAssignKeysViewModel VM { get;set; }
       
        public UnAssignkeysSelection(UnAssignKeysViewModel vm)
        {          
            InitializeComponent();
            VM = vm;
            DataContext = VM;
            Title = DIS.Presentation.KMT.Properties.MergedResources.Common_Select;
            this.searchControl.DataContext = VM.SearchControlVM;
        }
    }
}
