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
using DIS.Business.Proxy;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : Page
    {
        public UserManagementViewModel VM { get; private set; }

        public UserManagement(MainWindowViewModel mainVm, IUserProxy userProxy)
        {
            InitializeComponent();
            VM = new UserManagementViewModel(userProxy);
            DataContext = VM;

            mainVm.CheckUserSelected += new CheckAvailableEventHandler((s, e) => VM.CheckUserSelected(e));
            mainVm.CreateUser += new EventHandler((s, e) => VM.CreaterUser());
            mainVm.EditUser += new EventHandler((s, e) => VM.EditUser());
            mainVm.DeleteUser += new EventHandler((s, e) => VM.DeleteUser());
            mainVm.RefreshUsers += new SearchUserEventHandler((s, e) => VM.RefreshUsers(e.UserName, e.Role.RoleId));
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Roles")
                e.Cancel = true;
        }
    }
}
