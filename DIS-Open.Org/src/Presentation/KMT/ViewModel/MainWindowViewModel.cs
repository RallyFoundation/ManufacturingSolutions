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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.NotificationViews;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.View.Configuration;
using DIS.Presentation.KMT.View.Log;
using DIS.Presentation.KMT.Views;
using DIS.Presentation.KMT.Views.Configuration;
using DIS.Presentation.KMT.Views.EditKeysOptionalInfoView;
using DIS.Presentation.KMT.Views.Key.AssignKeysView;
using DIS.Presentation.KMT.Views.Key.ExportKeysView;
using DIS.Presentation.KMT.Views.Key.ImportKeysView;
using DIS.Presentation.KMT.Views.Key.RecallKeysView;
using DIS.Presentation.KMT.Views.Key.ReportKeysView;
using DIS.Presentation.KMT.Views.Key.ReturnKeysView;
using DIS.Presentation.KMT.Views.Key.RevertKeysView;
using DIS.Presentation.KMT.Views.Key.UnAssignKeysView;
using DIS.Presentation.KMT.Views.Notification;
using Microsoft.CSharp.RuntimeBinder;
using DIS.Presentation.KMT.Views.Key.OhrDataUpdateView;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CheckAvailableEventHandler(object sender, CheckAvailableEventArgs e);

    public delegate void AutoDiagnoseEventHandler(object sender, AutoDiagnoseEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class CheckAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public bool CanExecute { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SearchUserEventHandler(object sender, SearchUserEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class SearchUserEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Role Role { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        public SearchUserEventArgs(string userName, Role role)
        {
            UserName = userName;
            Role = role;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LogTabChangedEventHandler(object sender, LogTabChangedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class LogTabChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int LogTabIndex { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logTabIndex"></param>
        public LogTabChangedEventArgs(int logTabIndex)
        {
            LogTabIndex = logTabIndex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        public const int KeyPageIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        public const int UserPageIndex = 1;

        /// <summary>
        /// 
        /// </summary>
        public const int LogPageIndex = 2;

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return keyManagement.VM.IsBusy || userManagement.VM.IsBusy || logViewer.VM.IsBusy;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public new Window View
        {
            get { return view; }
            set { view = keyManagement.VM.View = userManagement.VM.View = logViewer.VM.View = value; }
        }

        #region Private Fields

        private Dictionary<string, string> locationNameMap = new Dictionary<string, string>()
        {
           
            { InstallType.Oem.ToString(), "Corporate Key Inventory" },
            { InstallType.Tpi.ToString(), "Factory Key Inventory" },
            { InstallType.FactoryFloor.ToString(), "Factory Floor Key Inventory" }
        };
        private readonly SolidColorBrush hasNoNotification = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush hasNotifications = new SolidColorBrush(Colors.Red);

        private ObservableCollection<string> logTypes = new ObservableCollection<string>();

        private IConfigProxy configProxy;
        private IKeyProxy keyProxy;
        private IUserProxy userProxy;
        private ILogProxy logProxy;
        private ISubsidiaryProxy ssProxy;
        private IHeadQuarterProxy hqProxy;
        private IKeyTypeConfigurationProxy stockProxy;

        private Window view;
        private Frame mainFrame;
        private KeyManagement keyManagement = null;
        private UserManagement userManagement = null;
        private ViewLogs logViewer = null;
        private NotificationWindow notificationWindow = null;
        private DiagnosticResult internalDiagnosticResult;

        private int ribbonTabIndex;
        private SolidColorBrush notificationColor;

        private bool isAutoReportDisabled;

        private string searchUserName;
        private Role selectedRole;
        private string currentUserAndRole;
        private string notificationHeader;
        private ObservableCollection<Role> roles;
        private int selectedLogTab;
        private bool isEnableGetKeys;
        private ICommand exportKeysCommand;
        private ICommand importKeysCommand;
        private ICommand getKeysCommand;
        private ICommand reportKeysCommand;
        private ICommand markAllocatedCommand;
        private ICommand assignKeysCommand;
        private ICommand ohrKeysCommand;
        private ICommand unassignKeysCommand;
        private ICommand recallKeysCommand;
        private ICommand returnKeysCommand;
        private ICommand editOptionalInfoCommand;
        private ICommand refreshKeysCommand;

        private ICommand createUserCommand;
        private ICommand editUserCommand;
        private ICommand deleteUserCommand;
        private ICommand refreshUsersCommand;

        private ICommand exportLogsCommand;
        private ICommand refreshLogsCommand;

        private ICommand openNotificationCommand;
        private ICommand openSettingsCommand;
        private ICommand openDiagnosticCommand;
        private ICommand openHelpCommand;
        private ICommand exitCommand;
        private ICommand aboutCommand;

        private ICommand closeCommand;

        private bool isUlsEnableAndManager = false;

        #endregion

        #region Public Events
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CurrentUserRoleChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler RefreshSubsidiaries;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler GetKeys;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler RefreshKeys;

        /// <summary>
        /// 
        /// </summary>
        public event CheckAvailableEventHandler CheckUserSelected;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CreateUser;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler EditUser;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler DeleteUser;

        /// <summary>
        /// 
        /// </summary>
        public event SearchUserEventHandler RefreshUsers;

        /// <summary>
        /// 
        /// </summary>
        public event LogTabChangedEventHandler LogTabChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler RefreshLogs;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainFrame"></param>
        public MainWindowViewModel(Frame mainFrame)
            : this(mainFrame, null, null, null, null, null, null, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainFrame"></param>
        /// <param name="configProxy"></param>
        /// <param name="keyProxy"></param>
        /// <param name="userProxy"></param>
        /// <param name="logProxy"></param>
        /// <param name="ssProxy"></param>
        /// <param name="hqProxy"></param>
        /// <param name="stockProxy"></param>
        public MainWindowViewModel(Frame mainFrame, IConfigProxy configProxy, IKeyProxy keyProxy, IUserProxy userProxy,
            ILogProxy logProxy, ISubsidiaryProxy ssProxy, IHeadQuarterProxy hqProxy, IKeyTypeConfigurationProxy stockProxy)
        {
            //Adding support to multiple customer context - Rally
            //this.configProxy = configProxy ?? new ConfigProxy(KmtConstants.LoginUser);
            //this.keyProxy = keyProxy ?? new KeyProxy(KmtConstants.LoginUser, KmtConstants.HeadQuarterId);
            //this.userProxy = userProxy ?? new UserProxy();
            //this.logProxy = logProxy ?? new LogProxy();
            //this.ssProxy = ssProxy ?? new SubsidiaryProxy();
            //this.hqProxy = hqProxy ?? new HeadQuarterProxy();
            //this.stockProxy = stockProxy ?? new KeyTypeConfigurationProxy();

            this.configProxy = configProxy ?? new ConfigProxy(KmtConstants.LoginUser, KmtConstants.CurrentDBConnectionString, KmtConstants.CurrentConfigurationID, KmtConstants.CurrentCustomerID);
            this.keyProxy = keyProxy ?? new KeyProxy(KmtConstants.LoginUser, KmtConstants.HeadQuarterId, KmtConstants.CurrentDBConnectionString, KmtConstants.CurrentConfigurationID, KmtConstants.CurrentCustomerID);
            this.userProxy = userProxy ?? new UserProxy(KmtConstants.CurrentDBConnectionString);
            this.logProxy = logProxy ?? new LogProxy(KmtConstants.CurrentDBConnectionString);
            this.ssProxy = ssProxy ?? new SubsidiaryProxy(KmtConstants.CurrentDBConnectionString);
            this.hqProxy = hqProxy ?? new HeadQuarterProxy(KmtConstants.CurrentDBConnectionString);
            this.stockProxy = stockProxy ?? new KeyTypeConfigurationProxy(KmtConstants.CurrentDBConnectionString);

            this.mainFrame = mainFrame;
            keyManagement = new KeyManagement(this, this.keyProxy, this.configProxy, this.ssProxy, this.hqProxy);
            userManagement = new UserManagement(this, this.userProxy);
            logViewer = new ViewLogs(this, this.logProxy);
            InitializeNotificationSystem();

            RibbonTabIndex = KeyPageIndex;
            InitializeRoles();
            LogTypes.Add(MergedResources.Common_SystemLog);
            LogTypes.Add(MergedResources.Common_OperationLog);
            OnCurrentUserRoleChanged();
            OnAutoReportChanged(null, null);
        }

        #endregion

        #region Binding Properties

        /// <summary>
        /// 
        /// </summary>
        public string WindowTitle
        {
            get
            {
                if (KmtConstants.CloudCustomers != null)
                {
                    foreach (var customer in KmtConstants.CloudCustomers)
                    {
                        if (customer.ID.ToLower() == KmtConstants.CurrentCustomerID.ToLower())
                        {
                            return string.Format("Key Management Tool - {0} : {1}", KmtConstants.InventoryName, customer.Name);
                        }
                    }
                }

                return string.Format("Key Management Tool - {0}", KmtConstants.InventoryName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NotificationHeader
        {
            get { return notificationHeader; }
            set
            {
                notificationHeader = value;
                RaisePropertyChanged("NotificationHeader");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<string> LogTypes
        {
            get { return logTypes; }
            set
            {
                logTypes = value;
                RaisePropertyChanged("LogTypes");
            }
        }

        /// <summary>
        /// Selected Tab index
        /// </summary>
        public int RibbonTabIndex
        {
            get
            {
                return ribbonTabIndex;
            }
            set
            {
                ribbonTabIndex = value;
                RibbonTabChanged();
                RaisePropertyChanged("RibbonTabIndex");
            }
        }

        /// <summary>
        /// Current user and role
        /// </summary>
        public string CurrentUserAndRole
        {
            get
            {
                return currentUserAndRole;
            }
            set
            {
                currentUserAndRole = value;
                RaisePropertyChanged("CurrentUserAndRole");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAutoReportDisabled
        {
            get { return isAutoReportDisabled; }
            set
            {
                isAutoReportDisabled = value;
                RaisePropertyChanged("IsAutoReportDisabled");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush NotificationColor
        {
            get { return notificationColor; }
            set
            {
                notificationColor = value;
                RaisePropertyChanged("NotificationColor");
            }
        }

        /// <summary>
        /// For setting visibility of CKI DIS.Presentation.KMT
        /// </summary>
        public bool IsUlsEnableAndManager
        {
            get
            {
                isUlsEnableAndManager = KmtConstants.CurrentHeadQuarter != null && (KmtConstants.IsFactoryFloor || (KmtConstants.IsTpiCorp && KmtConstants.CurrentHeadQuarter.IsCentralizedMode)) && IsManager;
                return isUlsEnableAndManager;
            }
            set
            {
                isUlsEnableAndManager = value;
                RaisePropertyChanged("IsUlsEnableAndManager");
            }
        }

        public bool IsUlsEnabledAndInDeCentralizeModeAndManager
        {
            get
            {
                return IsManager && (KmtConstants.IsOemCorp || (KmtConstants.IsTpiCorp && KmtConstants.CurrentHeadQuarter != null && !KmtConstants.CurrentHeadQuarter.IsCentralizedMode));
            }
        }

        /// <summary>
        /// For setting visibility of CKI DIS.Presentation.KMT
        /// </summary>
        public bool IsManagerOfOEMOrFactoryFloorOrUlsEnableAndManager
        {
            get
            {
                return IsManagerOfFFKI || IsManagerOfOEM || IsUlsEnableAndManager;
            }
        }

        /// <summary>
        /// For setting visibility of FFKI DIS.Presentation.KMT
        /// </summary>
        public bool IsFFKI
        {
            get
            {
                return KmtConstants.IsFactoryFloor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFFKIOrManagerOfFKI
        {
            get
            {
                return IsManager;
            }
        }

        /// <summary>
        /// For setting visibility of Operator DIS.Presentation.KMT
        /// </summary>
        public bool IsManager
        {
            get
            {
                return KmtConstants.LoginUser.RoleName == Constants.ManagerRoleName;
            }
        }

        public bool IsManagerOfFKIOrFF
        {
            get
            {
                return !KmtConstants.IsOemCorp && IsManager;
            }
        }

        /// <summary>
        /// For setting visibility of TPI DIS.Presentation.KMT
        /// </summary>
        public bool IsCKIOrFKI
        {
            get
            {
                return !KmtConstants.IsFactoryFloor;
            }
        }

        /// <summary>
        /// For setting visibility of CKI DIS.Presentation.KMT
        /// </summary>
        public bool IsManagerOfCKIOrFKI
        {
            get
            {
                return !KmtConstants.IsFactoryFloor && IsManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsManagerOfFFKI
        {
            get
            {
                return KmtConstants.IsFactoryFloor && IsManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsManagerOfOEM
        {
            get
            {
                return KmtConstants.IsOemCorp && IsManager;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnableGetKeys
        {
            get { return this.isEnableGetKeys; }
            set
            {
                this.isEnableGetKeys = value;
                RaisePropertyChanged("IsEnableGetKeys");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SearchUserName
        {
            get
            {
                return searchUserName;
            }
            set
            {
                searchUserName = value;
                RaisePropertyChanged("SearchUserName");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Role SelectedRole
        {
            get
            {
                return selectedRole;
            }
            set
            {
                selectedRole = value;
                RaisePropertyChanged("SelectedRole");
            }
        }

        /// <summary>
        /// Collection to bind to and from DIS.Presentation.KMT
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
                RaisePropertyChanged("Roles");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedLogTab
        {
            get
            {
                return selectedLogTab;
            }
            set
            {
                selectedLogTab = value;
                OnLogTabChanged();
                RaisePropertyChanged("SelectedLogTab");
            }
        }

        #endregion

        #region Binding Command Properties

        /// <summary>
        /// Export Keys Command Object
        /// </summary>
        public ICommand ExportKeysCommand
        {
            get
            {
                if (exportKeysCommand == null)
                    exportKeysCommand = new DelegateCommand(OnExportKeys);
                return exportKeysCommand;
            }
        }

        /// <summary>
        ///  Import Keys Command Object
        /// </summary>
        public ICommand ImportKeysCommand
        {
            get
            {
                if (importKeysCommand == null)
                    importKeysCommand = new DelegateCommand(OnImportKeys);
                return importKeysCommand;
            }
        }

        /// <summary>
        /// Assing Keys Command Object
        /// </summary>
        public ICommand GetKeysCommand
        {
            get
            {
                if (getKeysCommand == null)
                    getKeysCommand = new DelegateCommand(OnGetKeys);
                return getKeysCommand;
            }
        }

        /// <summary>
        ///  UnAssing Keys Command Object
        /// </summary>
        public ICommand ReportKeysCommand
        {
            get
            {
                if (reportKeysCommand == null)
                    reportKeysCommand = new DelegateCommand(OnReportKeys);
                return reportKeysCommand;
            }
        }

        /// <summary>
        /// Mark as Return Command
        /// </summary>
        public ICommand MarkAllocatedCommand
        {
            get
            {
                if (markAllocatedCommand == null)
                    markAllocatedCommand = new DelegateCommand(OnRevertKeys);
                return markAllocatedCommand;
            }
        }

        /// <summary>
        /// Assing Keys Command Object
        /// </summary>
        public ICommand AssignKeysCommand
        {
            get
            {
                if (assignKeysCommand == null)
                    assignKeysCommand = new DelegateCommand(OnAssignKeys);
                return assignKeysCommand;
            }
        }

        /// <summary>
        /// Ohr data update Command Object
        /// </summary>
        public ICommand OhrCommand
        {
            get
            {
                if (ohrKeysCommand == null)
                    ohrKeysCommand = new DelegateCommand(OnOhrKeys);
                return ohrKeysCommand;
            }
        }

        /// <summary>
        ///  UnAssing Keys Command Object
        /// </summary>
        public ICommand UnassignKeysCommand
        {
            get
            {
                if (unassignKeysCommand == null)
                    unassignKeysCommand = new DelegateCommand(OnUnassignKeys);
                return unassignKeysCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RecallKeysCommand
        {
            get
            {
                if (recallKeysCommand == null)
                    recallKeysCommand = new DelegateCommand(OnRecallKeys);
                return recallKeysCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ReturnKeysCommand
        {
            get
            {
                if (returnKeysCommand == null)
                    returnKeysCommand = new DelegateCommand(OnReturnKeys);
                return returnKeysCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand EditOptionalInfoCommand
        {
            get
            {
                if (editOptionalInfoCommand == null)
                    editOptionalInfoCommand = new DelegateCommand(OnEditOptionalInfo);
                return editOptionalInfoCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RefreshKeysCommand
        {
            get
            {
                if (refreshKeysCommand == null)
                    refreshKeysCommand = new DelegateCommand(OnRefreshKeys);
                return refreshKeysCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CreateUserCommand
        {
            get
            {
                if (createUserCommand == null)
                    createUserCommand = new DelegateCommand(OnCreateUser);
                return createUserCommand;
            }
        }


        /// <summary>
        /// Edit user
        /// </summary>
        public ICommand EditUserCommand
        {
            get
            {
                if (editUserCommand == null)
                    editUserCommand = new DelegateCommand(OnEditUser, OnCheckUserSelected);
                return editUserCommand;
            }
        }

        /// <summary>
        /// delete user
        /// </summary>
        public ICommand DeleteUserCommand
        {
            get
            {
                if (deleteUserCommand == null)
                    deleteUserCommand = new DelegateCommand(OnDeleteUser, OnCheckUserSelected);
                return deleteUserCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RefreshUsersCommand
        {
            get
            {
                if (refreshUsersCommand == null)
                    refreshUsersCommand = new DelegateCommand(OnRefreshUsers);
                return refreshUsersCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ExportLogsCommand
        {
            get
            {
                if (exportLogsCommand == null)
                    exportLogsCommand = new DelegateCommand(OnExportLogs);
                return exportLogsCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand RefreshLogsCommand
        {
            get
            {
                if (refreshLogsCommand == null)
                    refreshLogsCommand = new DelegateCommand(OnRefreshLogs);
                return refreshLogsCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenNotificationCommand
        {
            get
            {
                if (openNotificationCommand == null)
                    openNotificationCommand = new DelegateCommand(OnOpenNotification);
                return openNotificationCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenSettingsCommand
        {
            get
            {
                if (openSettingsCommand == null)
                    openSettingsCommand = new DelegateCommand(OnOpenSettings);
                return openSettingsCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenDiagnosticCommand
        {
            get
            {
                if (openDiagnosticCommand == null)
                    openDiagnosticCommand = new DelegateCommand(OnOpenDiagnostic);
                return openDiagnosticCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenHelpCommand
        {
            get
            {
                if (openHelpCommand == null)
                    openHelpCommand = new DelegateCommand(OnOpenHelp);
                return openHelpCommand;
            }
        }

        /// <summary>
        /// About
        /// </summary>
        public ICommand AboutCommand
        {
            get
            {
                if (aboutCommand == null)
                    aboutCommand = new DelegateCommand(OnOpenAbout);
                return aboutCommand;
            }
        }

        /// <summary>
        /// Exit App Commond
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new DelegateCommand(App.Current.Shutdown);
                return exitCommand;
            }
        }

        /// <summary>
        /// Close the Window Commond - Rally Sept. 1st, 2014
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (this.closeCommand == null)
                {
                    this.closeCommand = new DelegateCommand(() => 
                    {
                        Dispatch(() =>
                        {
                            LoginWindow loginWindow = new LoginWindow();

                            View.Visibility = Visibility.Collapsed;
                            
                            KmtConstants.CurrentConfigurationID = null;
                            KmtConstants.CurrentCustomerID = null;
                            KmtConstants.CurrentDBConnectionString = null;
                            KmtConstants.CurrentHeadQuarter = null;

                            loginWindow.Show();
                        });
                    });
                }

                return this.closeCommand;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void OnCurrentUserRoleChanged()
        {
            if (CurrentUserRoleChanged != null)
                CurrentUserRoleChanged(this, new EventArgs());

            CurrentUserAndRole = string.Format("{0} : {1}", KmtConstants.LoginUser.RoleName, KmtConstants.LoginUser.LoginId);

            RaisePropertyChanged("IsManager");
            RaisePropertyChanged("IsManagerOfFFKI");
            RaisePropertyChanged("IsFFKIOrManagerOfFKI");
            RaisePropertyChanged("IsNotCKIButManager");
            RaisePropertyChanged("IsManagerOfCKIOrFKI");
        }

        #region Internal Methods

        private void InitializeRoles()
        {
            SelectedRole = new Role { RoleName = MergedResources.Common_All };
            Roles = new ObservableCollection<Role>(userProxy.GetRoles());
            Roles.Insert(0, SelectedRole);
        }

        private void RibbonTabChanged()
        {
            Page page = null;
            switch (RibbonTabIndex)
            {
                case KeyPageIndex:
                    page = keyManagement;
                    OnRefreshKeys();
                    break;
                case UserPageIndex:
                    page = userManagement;
                    OnRefreshUsers();
                    break;
                case LogPageIndex:
                    page = logViewer;
                    OnRefreshLogs();
                    break;
            }
            mainFrame.Navigate(page);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnCurrentHeadQuarterChanged()
        {
            keyProxy.CurrentHeadQuarterId = KmtConstants.HeadQuarterId;

            //Adding support to multiple customer context - Rally
            //this.stockProxy = new KeyTypeConfigurationProxy();
            this.stockProxy = new KeyTypeConfigurationProxy(KmtConstants.CurrentDBConnectionString);
        }

        private void OnLogTabChanged()
        {
            if (LogTabChanged != null)
                LogTabChanged(this, new LogTabChangedEventArgs(SelectedLogTab));
        }

        private bool OnCheckUserSelected()
        {
            CheckAvailableEventArgs e = new CheckAvailableEventArgs();
            if (CheckUserSelected != null)
                CheckUserSelected(this, e);
            return e.CanExecute;
        }

        private void OnExportKeys()
        {
            ExportWizard exportWizard = new ExportWizard(keyProxy, configProxy, hqProxy, ssProxy);
            exportWizard.Owner = View;
            exportWizard.ShowDialog();
            if (exportWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnImportKeys()
        {
            ImportWizard ImportWizard = new ImportWizard(keyProxy, ssProxy, configProxy, hqProxy);
            ImportWizard.Owner = View;
            ImportWizard.ShowDialog();
            if (ImportWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }


        private void OnGetKeys()
        {
            if (GetKeys != null)
            {
                GetKeys(this, new EventArgs());
                RaisePropertyChanged("IsUlsEnableAndManager");

                CheckKeyTypeConfigurations(this, new NotificationEventArgs(notificationWindow.VM.Notifications));
            }
        }

        private void OnReportKeys()
        {
            ReportKeysWizard reportWizard = new ReportKeysWizard(keyProxy,configProxy);
            reportWizard.Owner = View;
            reportWizard.ShowDialog();
            if (reportWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnRevertKeys()
        {
            RevertKeysWizard revertWizard = new RevertKeysWizard(keyProxy, ssProxy);
            revertWizard.Owner = View;
            revertWizard.ShowDialog();
            if (revertWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnAssignKeys()
        {
            AssignKeysWizard assignWizard = new AssignKeysWizard(keyProxy, ssProxy);
            assignWizard.Owner = View;
            assignWizard.ShowDialog();

            if (assignWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnOhrKeys()
        {
            OhrDataUpdateWizard ohrWizard = new OhrDataUpdateWizard(keyProxy);
            ohrWizard.Owner = View;
            ohrWizard.ShowDialog();

            if (ohrWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnUnassignKeys()
        {
            UnAssignKeysWizard unAssignWizard = new UnAssignKeysWizard(keyProxy, ssProxy);
            unAssignWizard.Owner = View;
            unAssignWizard.ShowDialog();

            if (unAssignWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnRecallKeys()
        {
            RecallKeysWizard recallWizard = new RecallKeysWizard(keyProxy);
            recallWizard.Owner = View;
            recallWizard.ShowDialog();

            if (recallWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnReturnKeys()
        {
            ReturnKeysWizard returnWizard = new ReturnKeysWizard(keyProxy);
            returnWizard.Owner = View;
            returnWizard.ShowDialog();

            if (returnWizard.btnFinish.Visibility == Visibility.Visible)
                OnRefreshKeys();
        }

        private void OnEditOptionalInfo()
        {
            EditKeysOptionalInfo window = new EditKeysOptionalInfo(keyProxy);
            window.Owner = View;
            window.ShowDialog();
            if (window.VM.IsOptionalInfoChanged)
                OnRefreshKeys();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRefreshKeys()
        {
            if (RefreshKeys != null)
                RefreshKeys(this, new EventArgs());
        }

        private void OnRefreshSubsidiaries()
        {
            if (RefreshSubsidiaries != null)
                RefreshSubsidiaries(this, new EventArgs());
        }

        private void OnCreateUser()
        {
            if (CreateUser != null)
                CreateUser(this, new EventArgs());
        }

        private void OnEditUser()
        {
            if (EditUser != null)
                EditUser(this, new EventArgs());
        }

        private void OnDeleteUser()
        {
            if (DeleteUser != null)
                DeleteUser(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRefreshUsers()
        {
            if (RefreshUsers != null)
                RefreshUsers(this, new SearchUserEventArgs(SearchUserName, SelectedRole));
        }

        private void OnExportLogs()
        {
            ExportLogs window = new ExportLogs(logProxy);
            window.Owner = View;
            window.ShowDialog();
            OnRefreshLogs();
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnRefreshLogs()
        {
            if (RefreshLogs != null)
                RefreshLogs(this, new EventArgs());
        }

        private void OnOpenNotification()
        {
            if (notificationWindow.Owner == null)
                notificationWindow.Owner = View;
            notificationWindow.ShowDialog();
        }

        private void OnOpenSettings()
        {
            ConfigurationView window = new ConfigurationView(configProxy, ssProxy, hqProxy, userProxy, stockProxy, keyProxy, null);
            window.Owner = View;
            window.ConfigurationChanged += OnAutoReportChanged;
            window.ShowDialog();
            OnRefreshSubsidiaries();
        }

        private void OnAutoReportChanged(object sender, EventArgs e)
        {
            IsAutoReportDisabled = !(configProxy.GetCanAutoReport());
            HeadQuarter hq = KmtConstants.CurrentHeadQuarter;
            IsEnableGetKeys = ((hq != null && hq.IsCentralizedMode)
                || configProxy.GetIsMsServiceEnabled());
        }

        private void OnOpenDiagnostic()
        {
            DiagnosticWindow window = new DiagnosticWindow(configProxy, ssProxy);
            window.Owner = View;
            window.VM.AutoDiagnoseChanged += new AutoDiagnoseEventHandler(OnAutoDiagnoseChanged);
            window.ShowDialog();
        }

        private void OnAutoDiagnoseChanged(object sender, AutoDiagnoseEventArgs e)
        {
            RegisterSystemCheck(e.IsAutoDiagnose);
        }

        /// <summary>
        /// 
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        public void OnOpenHelp()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(GetHelpDocPath());
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            Process.Start(startInfo);
        }

        private void OnOpenAbout()
        {
            About window = new About();
            window.Owner = View;
            window.ShowDialog();
        }

        private string GetHelpDocPath()
        {
            const string folderName = "Assets";
            const string fileName = "Help.chm";
            string currentLocalizedFile = fileName;
            if (!KmtConstants.CurrentCulture.Name.Equals("en-US", StringComparison.OrdinalIgnoreCase))
                currentLocalizedFile = string.Format("Help.{0}.chm", KmtConstants.CurrentCulture.Name);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName, currentLocalizedFile);
            return File.Exists(path) ? path : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName, fileName);
        }

        #endregion

        #region Check notifications

        /// <summary>
        /// 
        /// </summary>
        public void InitializeNotificationSystem()
        {
            notificationWindow = new NotificationWindow();
            NotificationsChanged();
            notificationWindow.VM.Notifications.CollectionChanged += (s, e) => { NotificationsChanged(); };
            notificationWindow.VM.Check += new NotificationEventHandler(CheckRefulfillments);
            notificationWindow.VM.Check += new NotificationEventHandler(CheckReAcknowledgement);
            notificationWindow.VM.Check += new NotificationEventHandler(CheckDuplicatedCbr);
            notificationWindow.VM.Check += new NotificationEventHandler(CheckKeysExpired);
            notificationWindow.VM.Check += new NotificationEventHandler(CheckKeyTypeConfigurations);
            notificationWindow.VM.Check += new NotificationEventHandler(CheckOhrData);
            notificationWindow.VM.Check += new NotificationEventHandler(CheckDatabaseDiskFull);
            notificationWindow.VM.Check += new NotificationEventHandler(CheckConfirmedOhrKeys);
            RegisterSystemCheck(configProxy.GetIsAutoDiagnostic());
        }

        private void NotificationsChanged()
        {
            int notificationCount = notificationWindow.VM.Notifications.Count;
            NotificationHeader = string.Format(MergedResources.Notification_YouHaveNotifications, notificationCount);
            if (notificationCount > 0)
            {
                NotificationColor = hasNotifications;
                Dispatch(() =>
                {
                    if (!notificationWindow.VM.ShouldNotificationNotPopup && !notificationWindow.IsVisible && !IsCurrentWindowBusy())
                        OnOpenNotification();
                });
            }
            else
            {
                NotificationColor = hasNoNotification;
            }
        }

        private void CheckOhrData(object sender, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = NotificationCategory.OhrDataMissed;
                if (configProxy.GetRequireOHRData())
                {
                    List<KeyInfo> keys = keyProxy.GetBoundKeysWithoutOhrData();
                    Dispatch(() =>
                    {
                        if (keys != null && keys.Any())
                            e.Push(new Notification(category,
                                string.Format(ResourcesOfRTMv1_6.OhrDataMissedFormat, keys.Count),
                                typeof(EditKeysOptionalInfo), () =>
                                {
                                    CheckOhrData(sender, e);
                                    OnRefreshKeys();
                                }, keyProxy, keys, false));
                        else
                            e.Pop(category);
                    });
                }
                else
                    e.Pop(category);
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private void CheckKeyTypeConfigurations(object sender, NotificationEventArgs e)
        {
            try
            {
                List<KeyTypeConfiguration> configs = stockProxy.GetKeyTypeConfigurations(KmtConstants.HeadQuarterId);
                Dispatch(() =>
                {
                    NotificationCategory keyTypeUnmappedCategory = NotificationCategory.KeyTypeUnmapped;
                    if (configs.Any(c => !c.KeyType.HasValue))
                            e.Push(new Notification(keyTypeUnmappedCategory,
                            ResourcesOfR6.Notification_UpmapKeyPartNumber,
                            typeof(ConfigurationView),
                            () =>
                            {
                                CheckKeyTypeConfigurations(sender, e);
                                OnRefreshKeys();
                            },
                                //configProxy, ssProxy, hqProxy, userProxy, null, keyProxy, 2)//Fixed for mutiple customer context support - Rally - Sept.4, 2014
                            configProxy, ssProxy, hqProxy, userProxy, this.stockProxy, keyProxy, 2)
                            );
                    else
                        e.Pop(keyTypeUnmappedCategory);

                    NotificationCategory quantityOutOfRangeCategory = NotificationCategory.QuantityOutOfRange;
                    List<KeyTypeConfiguration> configsOutOfRange = configs.Where(c => c.AvailiableKeysCount < c.Minimum || c.AvailiableKeysCount > c.Maximum).ToList();
                    if (configsOutOfRange.Count > 0)
                        e.Push(new Notification(quantityOutOfRangeCategory,
                            string.Format(MergedResources.Notification_KeysStockOutOfRangeMessage, configsOutOfRange.Count),
                            typeof(KeysStockNotificationView), null, configsOutOfRange));
                    else
                        e.Pop(quantityOutOfRangeCategory);
                });
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private void CheckRefulfillments(object sender, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = NotificationCategory.ReFulfillment;
                List<FulfillmentInfo> infoes = keyProxy.GetFailedFulfillments(false);
                Dispatch(() =>
                {
                    if (infoes.Count > 0)
                        e.Push(new Notification(category,
                            string.Format(MergedResources.Notification_ReFulfillmentMessage, infoes.Count),
                            typeof(ReFulfillmentNotificationView), null, infoes, keyProxy));
                    else
                        e.Pop(category);
                });
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private void CheckReAcknowledgement(object sender, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = NotificationCategory.ReAcknowledgement;
                List<Cbr> cbrs = keyProxy.GetFailedCbrs();
                Dispatch(() =>
                {
                    if (cbrs.Count > 0)
                        e.Push(new Notification(category,
                            string.Format(MergedResources.Notification_ReAcknowledgeMessage, cbrs.Count),
                            typeof(ReAcknowledgementNotificationView), null, cbrs));
                    else
                        e.Pop(category);
                });
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private void CheckDuplicatedCbr(object sender, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = NotificationCategory.DuplicatedCbr;
                List<Cbr> cbrs = keyProxy.GetCbrsDuplicated().FindAll(cbr => cbr.CbrDuplicated != null);
                cbrs.ForEach(cbr =>
                {
                    Dispatch(() =>
                    {
                        if (cbrs.Count > 0)
                            e.Push(new Notification(category,
                                string.Format(MergedResources.ExportDuplicateCBRNotificationViewModel_DuplicateCBRsMessage, cbr.CbrKeys.Count),
                                typeof(ExportDuplicateCBRNotificationView), () => { CheckDuplicatedCbr(sender, e); }, cbr, keyProxy));
                        else
                            e.Pop(category);
                    });
                });


            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private void CheckKeysExpired(object sender, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = NotificationCategory.OldTimelineExceed;
                List<KeyInfo> keysExpired = keyProxy.SearchExpiredKeys(KmtConstants.OldTimeline);
                Dispatch(() =>
                {
                    if (keysExpired.Count > 0)
                        e.Push(new Notification(category,
                            string.Format(MergedResources.KeyManagementViewModel_OldTimelineExceedMessage, keysExpired.Count),
                            typeof(KeysExpiredNotificationView), null, keysExpired, KmtConstants.OldTimeline));
                    else
                        e.Pop(category);
                });
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private void CheckConfirmedOhrKeys(object sender, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = NotificationCategory.ConfirmedOhrs;
                List<Ohr> ohrs = keyProxy.GetConfirmedOhrs();
                Dispatch(() =>
                {
                    if (ohrs.Count > 0)
                        e.Push(new Notification(category,
                            ResourcesOfRTMv1_8.OhrUpdateViewModel_Message,
                            typeof(OhrKeysNotificationView), 
                            () => 
                                {
                                    keyProxy.UpdateOhrAfterNotification(ohrs);
                                }, ohrs, keyProxy));
                    else
                        e.Pop(category);
                });
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private void CheckDatabaseDiskFull(object sender, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = NotificationCategory.SystemError_DabaseDiskFull;
                DiagnosticResult result = configProxy.TestDatabaseDiskFull();
                Dispatch(() =>
                {
                    if (result.DiagnosticResultType == DiagnosticResultType.Error)
                    {
                        Notification no = new Notification(category,
                            ResourcesOfRTMv1_8.Notification_DatabaseDiskFull,
                            null, () => {
                                configProxy.DatabaseDiskFullReport();
                                e.Pop(category);
                            }, null);
                        no.ButtonContent = MergedResources.Common_Clear;
                        e.Push(no);
                    }
                    else
                        e.Pop(category);
                });
            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        private bool IsCurrentWindowBusy()
        {
            try
            {
                dynamic current = null;
                WindowCollection windows = App.Current.Windows;
                for (int i = 0; i < windows.Count; i++)
                {
                    if (!(windows[windows.Count - 1 - i] is NotificationWindow))
                    {
                        current = windows[windows.Count - 1 - i];
                        break;
                    }
                }
                return current.VM.IsBusy;
            }
            catch (RuntimeBinderException)
            {
                return false;
            }
        }

        #endregion

        #region System state notifications

        /// <summary>
        /// Register system state check
        /// </summary>
        /// <param name="enabled"></param>
        private void RegisterSystemCheck(bool enabled)
        {
            if (enabled)
                notificationWindow.VM.SystemCheck += OnCheckSystemStatus;
            else
            {
                notificationWindow.VM.SystemCheck -= OnCheckSystemStatus;
                PopSystemErrorOnUnCheckSystemStatus();
            }
        }

        private void PopSystemErrorOnUnCheckSystemStatus()
        {
            Dispatch(() =>
            {
                ObservableCollection<Notification> notifications = notificationWindow.VM.Notifications;
                NotificationCategory[] systemNotifications = new NotificationCategory[]
                {
                    NotificationCategory.SystemError_DatePolling,
                    NotificationCategory.SystemError_DownLevelSystem,
                    NotificationCategory.SystemError_Internal,
                    NotificationCategory.SystemError_MSConnection,
                    NotificationCategory.SystemError_UpLevelSystem,
                    NotificationCategory.SystemError_Unknow,
                    NotificationCategory.SystemError_DataBaseError,
                    NotificationCategory.SystemError_KeyProviderServiceError,
                };
                var removed = notifications.Where(n => systemNotifications.Contains(n.Category)).ToList();

                foreach (var systemError in removed)
                {
                    notifications.Remove(systemError);
                }
            });
        }

        private void OnCheckSystemStatus(object sender, NotificationEventArgs e)
        {
            CheckDatabaseSystemState(sender, e);
            CheckInternalSystemState(sender, e);
            CheckDataPollingSystemState(sender, e);

            if (KmtConstants.IsFactoryFloor)
                CheckKeyProviderServiceSystemState(sender, e);
        }

        /// <summary>
        /// test database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckDatabaseSystemState(object sender, NotificationEventArgs e)
        {
            var result = configProxy.TestDatabaseConnection();
            SetSystemState(NotificationCategory.SystemError_DataBaseError, result, ResourcesOfR6.Notification_DataBaseErrorMessage, e);
        }

        /// <summary>
        ///test Internal web service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckInternalSystemState(object sender, NotificationEventArgs e)
        {
            string errorMessage = string.Empty;
            internalDiagnosticResult = configProxy.TestInternalConnection();
            SetSystemState(NotificationCategory.SystemError_Internal, internalDiagnosticResult, ResourcesOfR6.Notification_InternalErrorMessage, e);
        }

        /// <summary>
        /// test data polling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckDataPollingSystemState(object sender, NotificationEventArgs e)
        {
            if (!IsUnknownError(NotificationCategory.SystemError_DatePolling, ResourcesOfR6.Notification_DatePollingErrorMessage, e))
            {
                string errorMessage = string.Empty;
                var result = configProxy.TestDataPollingService();
                SetSystemState(NotificationCategory.SystemError_DatePolling, result, ResourcesOfR6.Notification_DatePollingErrorMessage, e);
            }
        }

        /// <summary>
        /// test key provider service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckKeyProviderServiceSystemState(object sender, NotificationEventArgs e)
        {
            if (!IsUnknownError(NotificationCategory.SystemError_KeyProviderServiceError, ResourcesOfR6.Notification_KeyProviderServiceErrorMessage, e))
            {
                var result = configProxy.TestKeyProviderService();
                SetSystemState(NotificationCategory.SystemError_KeyProviderServiceError, result, ResourcesOfR6.Notification_KeyProviderServiceErrorMessage, e);
            }
        }

        /// <summary>
        /// Set error when internal web service dose not work
        /// </summary>
        /// <param name="e"></param>
        private bool IsUnknownError(NotificationCategory notificationCategory, string errorTitle, NotificationEventArgs e)
        {
            if (internalDiagnosticResult.DiagnosticResultType == DiagnosticResultType.Error)
            {
                SetSystemState(notificationCategory, new DiagnosticResult()
                {
                    Exception = null,
                    DiagnosticResultType = DiagnosticResultType.Error
                },
                    errorTitle,
                    e);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// create system state notification
        /// </summary>
        /// <param name="diagnosticResult"></param>
        /// <param name="errorTitle"></param>
        /// <param name="e"></param>
        private void SetSystemState(NotificationCategory notificationCategory, DiagnosticResult diagnosticResult, string errorTitle, NotificationEventArgs e)
        {
            try
            {
                NotificationCategory category = notificationCategory;
                string errorMessage = null;
                Dispatch(() =>
                {
                    if ((category != NotificationCategory.SystemError_KeyProviderServiceError && category != NotificationCategory.SystemError_DatePolling) || diagnosticResult.Exception == null)
                        errorMessage = diagnosticResult.Exception == null ? ResourcesOfR6.Notification_UnknowMessage : diagnosticResult.Exception.ToString();

                    if (diagnosticResult.DiagnosticResultType == DiagnosticResultType.Error)
                        e.Push(new Notification(category,
                            errorTitle,
                            string.IsNullOrEmpty(errorMessage) ? null : typeof(SystemStateNotificationView), null, errorTitle, errorMessage));
                    else
                        e.Pop(category);
                });

            }
            catch (Exception ex)
            {
                MessageLogger.LogSystemError(MessageLogger.GetMethodName(), ex.GetTraceText(), KmtConstants.CurrentDBConnectionString);
            }
        }

        #endregion
    }
}
