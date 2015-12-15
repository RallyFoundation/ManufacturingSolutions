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
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.View.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AccountSetting : IConfigurationPage
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateUserViewModel VM { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CanSave
        {
            get { return VM.IsChanged; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSaved
        {
            get { return VM.IsSaved; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProxy"></param>
        public AccountSetting(IUserProxy userProxy)
        {
            this.InitializeComponent();

            VM = new CreateUserViewModel(userProxy, KmtConstants.LoginUser, UserOperation.SetAccount);
            DataContext = VM;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            VM.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get { return VM.IsBusy; }
            set {  }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler IsBusyChanged
        {
            add { VM.IsBusyChanged += value; }
            remove { VM.IsBusyChanged -= value; }
        }
    }
}
