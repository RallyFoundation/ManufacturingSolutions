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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using DIS.Business.Library;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// View Model class for Create User View
    /// </summary>
    public class CreateUserViewModel : DataErrorInfoViewModelBase
    {
        #region Private Members

        private IUserProxy userProxy;

        private DelegateCommand saveCommand;
        private DelegateCommand cancelCommand;

        private ObservableCollection<Role> roles = null;
        private ObservableCollection<Language> langs = null;
        private Language selectedLang = null;
        private Role selectedRole = null;
        private User currentUser = null;
        private UserOperation userOperation;
        private bool loginIdReadOnly;
        private bool isChanged;
        #region Field
        private const string loginIDPropertyName = "LoginID";
        private const string passwordPropertyName = "NewPassword";
        private const string confirmPasswordPropertyName = "ConfirmPassword";
        private const string rolePropertyName = "Role";
        private DateTime userCreateDate;

        private string loginId = string.Empty;
        private string password = string.Empty;
        private string newPassword = string.Empty;
        private string confirmPassword = string.Empty;
        private string department = string.Empty;
        private string phone = string.Empty;
        private string email = string.Empty;
        private string title = string.Empty;
        private string position = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private bool isBusy;
        bool isLanguageChanged;
        #endregion

        #endregion

        #region Construct

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userProxy"></param>
        /// <param name="currentUser"></param>
        /// <param name="userOperation"></param>
        public CreateUserViewModel(IUserProxy userProxy, User currentUser, UserOperation userOperation)
        {
            this.userProxy = userProxy;

            this.userOperation = userOperation;
            if (userOperation == UserOperation.Edit)
            {
                Title = MergedResources.Common_EditUser;
                this.currentUser = currentUser;
                this.LoginIdReadOnly = true;
            }
            else if (userOperation == UserOperation.Add)
            {
                Title = MergedResources.Common_AddUser;
                this.currentUser = new User();
                this.LoginIdReadOnly = false;
            }
            else
            {
                this.currentUser = currentUser;
                this.LoginIdReadOnly = true;
            }
            InitializeCollections(this.currentUser);
            isLanguageChanged = false;
            IsChanged = false;
            IsSaved = true;
        }


        #endregion

        #region Public Property

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler IsBusyChanged;

        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged
        {
            get { return this.isChanged; }
            set
            {
                this.isChanged = value;
                RaisePropertyChanged("IsChanged");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSaved { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool LoginIdReadOnly
        {
            get
            {
                return this.loginIdReadOnly;
            }
            set
            {
                this.loginIdReadOnly = value;
                RaisePropertyChanged("LoginIdReadOnly");
            }
        }

        /// <summary>
        /// window title
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        /// <summary>
        /// Collection to bind to and from DIS.Presentation.KMT Roles
        /// </summary>
        public ObservableCollection<Role> Roles
        {
            get
            {
                return roles;
            }
            set
            {
                roles = value;
                RaisePropertyChanged(rolePropertyName);
            }
        }

        /// <summary>
        /// collection to bind to DIS.Presentation.KMT Languages
        /// </summary>
        public ObservableCollection<Language> Languages
        {
            get { return langs; }
            set
            {
                langs = value;
                RaisePropertyChanged("Languages");
            }
        }

        /// <summary>
        /// Selected Role Name
        /// </summary>
        public string SelectedRoleName
        {
            get
            {
                return SelectedRole.RoleName;
            }
        }

        /// <summary>
        /// Selected Role
        /// </summary>
        public Role SelectedRole
        {
            get
            {
                return selectedRole;
            }
            set
            {
                var originalValue = selectedRole;
                if (value != selectedRole)
                {
                    IsChanged = true;
                    selectedRole = value;
                    RaisePropertyChanged(rolePropertyName);
                }
            }
        }

        /// <summary>
        /// selected lang
        /// </summary>
        public Language SelectedLanguage
        {
            get { return selectedLang; }
            set
            {
                if (value != selectedLang)
                {
                    selectedLang = value;
                    IsChanged = true;
                    isLanguageChanged = true;
                    RaisePropertyChanged("SelectedLanguage");
                }
            }
        }

        /// <summary>
        /// LoginID
        /// </summary>
        public string LoginID
        {
            get
            {
                return loginId;
            }
            set
            {
                if (value != null)
                    loginId = value.Trim();
                else
                    loginId = value;
                IsChanged = true;
                RaisePropertyChanged("LoginID");
            }
        }

        /// <summary>
        /// Position
        /// </summary>
        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position != value)
                    IsChanged = true;
                if (value != null)
                    position = value.Trim();
                RaisePropertyChanged("Position");
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        public string CurrentPassword
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                IsChanged = true;
                RaisePropertyChanged("CurrentPassword");
            }
        }

        /// <summary>
        /// user New Password
        /// </summary>
        public string NewPassword
        {
            get
            {
                return newPassword;
            }
            set
            {
                newPassword = value;
                IsChanged = true;
                RaisePropertyChanged(passwordPropertyName);
                RaisePropertyChanged(confirmPasswordPropertyName);
            }
        }

        /// <summary>
        /// Confirm Password
        /// </summary>
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }
            set
            {
                IsChanged = true;
                confirmPassword = value;
                RaisePropertyChanged(passwordPropertyName);
                RaisePropertyChanged(confirmPasswordPropertyName);
            }
        }

        /// <summary>
        /// user Language
        /// </summary>
        public string Language
        {
            get { return selectedLang.LanguageCode; }
        }

        /// <summary>
        /// user FirstName
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (firstName != value)
                    IsChanged = true;
                if (value != null)
                    firstName = value.Trim();
                RaisePropertyChanged("FirstName");
            }
        }

        /// <summary>
        /// User LastName
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (value != lastName)
                    IsChanged = true;
                if (value != null)
                    lastName = value.Trim();
                RaisePropertyChanged("LastName");
            }
        }

        /// <summary>
        /// User Department
        /// </summary>
        public string Department
        {
            get
            {
                return department;
            }
            set
            {
                if (value != department)
                    IsChanged = true;
                if (value != null)
                    department = value.Trim();
                RaisePropertyChanged("Department");
            }
        }

        /// <summary>
        /// User Phone
        /// </summary>
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (value != phone)
                    IsChanged = true;
                if (value != null)
                    phone = value.Trim();
                RaisePropertyChanged("Phone");
            }
        }

        /// <summary>
        /// User Email
        /// </summary>
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value != email)
                    IsChanged = true;
                if (value != null)
                    email = value.Trim();
                RaisePropertyChanged("Email");
            }
        }

        /// <summary>
        /// 
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
                if (IsBusyChanged != null)
                    IsBusyChanged(this, new EventArgs());
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// Command used on clicking Cancel button
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new DelegateCommand(Cancel);
                }
                return cancelCommand;
            }
        }

        /// <summary>
        /// Command used on clicking Cancel button
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new DelegateCommand(() => Save());
                }
                return saveCommand;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected override string[] ValidatedProperties
        {
            get
            {
                return new string[] { loginIDPropertyName, passwordPropertyName, confirmPasswordPropertyName };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override string ValidateProperties(string propertyName)
        {
            switch (propertyName)
            {
                case loginIDPropertyName:
                    return ValidateLoginId();
                case passwordPropertyName:
                    return ValidatePassword();
                case confirmPasswordPropertyName:
                    return ValidateConfirmPassword();
                default:
                    Debug.Fail("Unexpected property being validated on CreateAccountPageViewModel: " + propertyName);
                    return null;
            }
        }

        /// <summary>
        /// Loads default data on window loading
        /// </summary>
        /// <param name="currentUser"></param>
        private void InitializeCollections(User currentUser)
        {
            try
            {
                roles = new ObservableCollection<Role>(userProxy.GetRoles());
                langs = DIS.Presentation.KMT.Language.GetLangs();
                SelectedRole = currentUser.Role == null ? roles.ElementAt(0) : currentUser.Role;

                FirstName = currentUser.FirstName;
                LastName = currentUser.SecondName;
                Department = currentUser.Department;
                Position = currentUser.Position;
                Email = currentUser.Email;
                Phone = currentUser.Phone;
                LoginID = currentUser.LoginId;
                CurrentPassword = string.Empty;
                userCreateDate = currentUser.CreatedDate;
                if (string.IsNullOrEmpty(currentUser.Language))
                {
                    currentUser.Language = KmtConstants.CurrentCulture.IetfLanguageTag;
                }
                SelectedLanguage = langs.SingleOrDefault(l => l.LanguageCode == currentUser.Language)
                    ?? DIS.Presentation.KMT.Language.Default;
            }
            catch (Exception ex)
            {
                ex.ShowDialog();
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
            }
        }


        private string ValidateLoginId()
        {
            return GetPropertyValidationMessage(!string.IsNullOrEmpty(LoginID),
                MergedResources.Common_Required);
        }

        private string ValidatePassword()
        {
            return GetPropertyValidationMessage(!string.IsNullOrEmpty(NewPassword),
                MergedResources.Common_Required);
        }

        private string ValidateConfirmPassword()
        {
            return GetPropertyValidationMessage(NewPassword == ConfirmPassword,
                MergedResources.Common_NotMatch);
        }

        #region Private Methods

        private User BuildUser()
        {
            User user = new User();
            user.UserId = currentUser.UserId;
            user.LoginId = loginId;
            user.Phone = phone;
            user.Position = position;
            user.Roles.Clear();
            user.AddRole(selectedRole);
            user.FirstName = firstName;
            user.SecondName = lastName;
            user.Email = email;
            user.Department = department;
            user.Password = NewPassword;
            if (this.userOperation == UserOperation.Add)
                user.CreatedDate = DateTime.UtcNow;
            else
                user.CreatedDate = userCreateDate;
            user.UpdatedDate = DateTime.UtcNow;
            user.Language = Language;
            user.Salt = currentUser.Salt;
            user.PasswordVersion = PasswordVersionResolver.CurrentPasswordVersion;
            return user;
        }

        /// <summary>
        /// Save input user to DBS
        /// </summary>
        public void Save()
        {
            if (!IsChanged && UserOperation.SetAccount == userOperation)
                return;
            if (userOperation == UserOperation.Add || userOperation == UserOperation.Edit)
            {
                isLanguageChanged = false;
            }

            if (!this.ValidateUser())
                return;

            IsBusy = true;
            IsSaved = false;
            WorkInBackground((s, e) =>
            {
                try
                {
                    switch (userOperation)
                    {
                        case UserOperation.Add:
                            User newUser = BuildUser();
                            newUser.Language = null;
                            userProxy.AddUser(newUser);
                            MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, string.Format("added new user {0} .", loginId), KmtConstants.CurrentDBConnectionString);
                            break;
                        case UserOperation.Edit:
                            EditUser();
                            MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, string.Format("edited user {0} .", loginId), KmtConstants.CurrentDBConnectionString);
                            break;
                        case UserOperation.SetAccount:
                            SetAccount();
                            MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, string.Format("setted account ."), KmtConstants.CurrentDBConnectionString);
                            break;
                        default:
                            break;
                    }
                    
                    Dispatch(() =>
                    {
                        //clear password
                        NewPassword = string.Empty;
                        ConfirmPassword = string.Empty;
                        CurrentPassword = string.Empty;
                        if (isLanguageChanged)
                            ValidationHelper.ShowMessageBox(MergedResources.CreateUserViewModel_ApplyLanguageChange, MergedResources.Common_Message);
                        isLanguageChanged = false;
                        IsChanged = false;
                        if (this.View != null)
                        {
                            Close(true);
                        }
                        IsSaved = true;
                        IsBusy = false;
                    });
                }
                catch (Exception ex)
                {
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                    IsBusy = false;
                }
            });
        }

        private bool IsPropertyValueChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue) && string.IsNullOrEmpty(newValue))
                return false;
            else
                return oldValue != newValue;
        }

        private void SetAccount()
        {
            var user = BuildUser();
            userProxy.ChangeProfile(user);
            user.Language = null;
            KmtConstants.LoginUser = user;
        }

        private void EditUser()
        {
            var user = BuildUser();
            userProxy.EditUser(user);
            if (user.UserId == KmtConstants.LoginUser.UserId)
            {
                KmtConstants.LoginUser = user;
                MainWindow.Current.VM.OnCurrentUserRoleChanged();
            }
        }

        private void Cancel()
        {
            if (this.View != null)
            {
                Dispatch(() => { Close(false); });
            }
        }

        private void Close(bool ok)
        {
            if (ok)
                View.DialogResult = true;
            View.Close();
        }

        private bool ValidateUser()
        {

            string error = string.Empty;

            if (string.IsNullOrEmpty(LoginID))
            {
                error = string.Format(MergedResources.CreateUserViewModel_RequriedLoginId, MergedResources.Common_ColumnHeaderLoginID);
                ValidationHelper.ShowMessageBox(error.Trim(), MergedResources.Common_Error);
                return false;
            }

            if (UserOperation.Add == userOperation)
            {
                if (string.IsNullOrEmpty(NewPassword))
                {
                    error = string.Format(MergedResources.CreateUserViewModel_PasswordRequired);
                    ValidationHelper.ShowMessageBox(error.Trim(), MergedResources.Common_Error);
                    return false;
                }
            }

            //valid current password
            if (UserOperation.SetAccount == userOperation)
            {
                currentUser.Password = CurrentPassword;
                try
                {
                    if (!string.IsNullOrEmpty(CurrentPassword) || !string.IsNullOrEmpty(NewPassword) || !string.IsNullOrEmpty(ConfirmPassword))
                    {
                        userProxy.ValidateCurrentPassword(currentUser);
                    }

                    if (!string.IsNullOrEmpty(CurrentPassword) && string.IsNullOrEmpty(NewPassword) && string.IsNullOrEmpty(ConfirmPassword))
                    {
                        error = MergedResources.CreateUserViewModel_NewPasswordRequired;
                        ValidationHelper.ShowMessageBox(error.Trim(), MergedResources.Common_Error);
                        return false;
                    }
                }
                catch (Exception)
                {
                    error = string.Format(MergedResources.UserProxy_PasswordIncorrect);
                    ValidationHelper.ShowMessageBox(error.Trim(), MergedResources.Common_Error);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(NewPassword) || !string.IsNullOrEmpty(ConfirmPassword))
            {
                if (NewPassword != ConfirmPassword)
                {
                    error = string.Format(MergedResources.CreateUserViewModel_ConfirmPasswordNotMatch, MergedResources.Common_ColumnHeaderPassword);
                    ValidationHelper.ShowMessageBox(error.Trim(), MergedResources.Common_Error);
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
