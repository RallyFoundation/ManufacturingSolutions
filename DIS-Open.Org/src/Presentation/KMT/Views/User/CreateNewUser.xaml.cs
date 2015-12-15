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
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT
{
    /// <summary>
    /// Interaction logic for CreateNewUser.xaml
    /// </summary>
    public partial class CreateNewUser : Window
    {
        public CreateUserViewModel VM { get; private set; }

        public CreateNewUser(IUserProxy userProxy, User currentUser, UserOperation userOperation)
        {
            InitializeComponent();
            VM = new CreateUserViewModel(userProxy, currentUser, userOperation);
            VM.View = this;
            DataContext = VM;
        }

    }
}
