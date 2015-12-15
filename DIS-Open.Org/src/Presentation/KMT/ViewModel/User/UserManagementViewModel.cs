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
using System.Windows.Threading;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Properties;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using System;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// View Model class for User Management View
    /// </summary>
    public class UserManagementViewModel : ViewModelBase
    {
        #region Private Members

        private IUserProxy userProxy;

        private ObservableCollection<User> users;
        private User selectedUser;
        private ObservableCollection<User> selectedUserCollection;
        private bool isBusy;

        #endregion

        #region Public Propertys

        private bool hasUserBeenSelected;

        /// <summary>
        /// Contains list of Users data
        /// </summary>
        public ObservableCollection<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                RaisePropertyChanged("Users");
            }
        }

        /// <summary>
        /// For Busy Indicator if task is long running 
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Selected User details on window loading
        /// </summary>
        public User SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                if (selectedUser != null)
                {
                    hasUserBeenSelected = true;
                    EnableUserSelected(SelectedUser);
                }
                else
                {
                    hasUserBeenSelected = false;
                }
                RaisePropertyChanged("SelectedUser");
            }
        }

        /// <summary>
        /// Contains list of Users data
        /// </summary>
        public ObservableCollection<User> SelectedUserCollection
        {
            get
            {
                return selectedUserCollection;
            }
        }

        /// <summary>
        /// For setting visibility of Operator DIS.Presentation.KMT
        /// </summary>
        public bool IsOperationVisible
        {
            get
            {
                return KmtConstants.LoginUser.RoleName == Constants.ManagerRoleName;
            }
        }

        #endregion

        #region Constrcutors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProxy"></param>
        public UserManagementViewModel(IUserProxy userProxy)
        {
            this.userProxy = userProxy;
            users = new ObservableCollection<User>();
            selectedUserCollection = new ObservableCollection<User>();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Selection changed  event for each selection in user items datagrid
        /// Used to persist selected items information between page navigations
        /// </summary>
        private void EnableUserSelected(User SelectedUser)
        {
            if (this.selectedUserCollection != null)
                this.selectedUserCollection.Clear();
            if (null != SelectedUser)
            {
                selectedUserCollection.Add(SelectedUser);
                //Notify History DIS.Presentation.KMT and Pager
                base.RaisePropertyChanged("SelectedUserCollection");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void CheckUserSelected(CheckAvailableEventArgs e)
        {
            e.CanExecute = hasUserBeenSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreaterUser()
        {
            Window parent = GetCurrentWindow();
            CreateNewUser window = new CreateNewUser(userProxy, null, UserOperation.Add);
            window.Owner = parent;
            window.ShowDialog();
            if (window.DialogResult.HasValue && window.DialogResult.Value)
                RefreshUsers(string.Empty, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void EditUser()
        {
            Window parent = GetCurrentWindow();
            CreateNewUser window = new CreateNewUser(userProxy, SelectedUser, UserOperation.Edit);
            window.Owner = parent;
            window.ShowDialog();
            if (window.DialogResult.HasValue && window.DialogResult.Value)
                RefreshUsers(string.Empty, null);
        }

        /// <summary>
        /// delete user from db
        /// </summary>
        public void DeleteUser() {
            try {
                if (SelectedUser != null) {
                    if (SelectedUser.UserId != KmtConstants.LoginUser.UserId) {
                        if (MessageBox.Show(GetCurrentWindow(), MergedResources.UserManagementViewModel_DeleteConfirm, MergedResources.Common_Confirmation, MessageBoxButton.YesNo) == MessageBoxResult.Yes) {

                            userProxy.DeleteUser(SelectedUser);
                            MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, string.Format("The User {0} was been deleted.", SelectedUser.LoginId), KmtConstants.CurrentDBConnectionString);
                            RefreshUsers(string.Empty, null);
                            hasUserBeenSelected = false;
                        }
                    }
                    else {
                        MessageBox.Show(GetCurrentWindow(), MergedResources.UserManagementViewModel_DeleteCurrentUser, MergedResources.Common_Information, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex) {
                ex.ShowDialog();
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
            }
        }

        /// <summary>
        /// update users information
        /// </summary>
        public void RefreshUsers(string loginId, int? roleId)
        {
            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    roleId = roleId == 0 ? null : roleId;
                    Users = new ObservableCollection<User>(userProxy.GetUsers(loginId, roleId));
                    if (selectedUser != null)
                    {
                        foreach (var user in users)
                        {
                            if (user.UserId == selectedUser.UserId)
                            {
                                Dispatch(() =>
                                {
                                    SelectedUser = user;
                                });
                                break;
                            }
                        }
                    }
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
            });
        }

        #endregion
    }
}
