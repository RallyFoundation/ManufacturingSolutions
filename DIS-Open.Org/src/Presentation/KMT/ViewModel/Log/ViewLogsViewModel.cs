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
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewLogsViewModel : ViewModelBase
    {
        #region Private fields

        private const int systemTabIndex = 0;
        private const int operationTabIndex = 1;
        private string sortedBy = "TimestampUtc";
        private bool sortedByDesc = true;

        private ILogProxy logProxy;

        private int currentTab = 0;
        private bool isBusy = false;

        private int systemLogSeverity;
        private DateTime? systemLogDateFrom;
        private DateTime? systemLogDateTo;
        private int systemLogCurrentPage = 1;
        private int systemLogPageSize = KmtConstants.DefaultPageSize;
        private int systemLogPageCount = 0;
        private ObservableCollection<Log> systemLogs;
        private Log selectedSystemLog;

        private DateTime? operationLogDateFrom;
        private DateTime? operationLogDateTo;
        private int operationLogCurrentPage = 1;
        private int operationLogPageSize = KmtConstants.DefaultPageSize;
        private int operationLogPageCount = 0;
        private string operationLogUserName;
        private bool isOperationLogUserNameReadOnly;
        private ObservableCollection<Log> operationLogs;
        private Log selectedOperationLog;

        private ICommand filterCommand;
        private ICommand changePageCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SelectedLogChanged;

        /// <summary>
        /// 
        /// </summary>
        public int CurrentTab
        {
            get
            {
                return currentTab;
            }
            set
            {
                currentTab = value;
                RaisePropertyChanged("CurrentTab");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCriticalChecked
        {
            get
            {
                return (systemLogSeverity & (int)TraceEventType.Critical) != 0;
            }
            set
            {
                SeverityCheckBoxChanged(value, TraceEventType.Critical);
                RaisePropertyChanged("IsCriticalChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsErrorChecked
        {
            get
            {
                return (systemLogSeverity & (int)TraceEventType.Error) != 0;
            }
            set
            {
                SeverityCheckBoxChanged(value, TraceEventType.Error);
                RaisePropertyChanged("IsErrorChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsWarningChecked
        {
            get
            {
                return (systemLogSeverity & (int)TraceEventType.Warning) != 0;
            }
            set
            {
                SeverityCheckBoxChanged(value, TraceEventType.Warning);
                RaisePropertyChanged("IsWarningChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInformationChecked
        {
            get
            {
                return (systemLogSeverity & (int)TraceEventType.Information) != 0;
            }
            set
            {
                SeverityCheckBoxChanged(value, TraceEventType.Information);
                RaisePropertyChanged("IsInformationChecked");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime? SystemLogDateFrom
        {
            get
            {
                return systemLogDateFrom;
            }
            set
            {
                systemLogDateFrom = value;
                RaisePropertyChanged("SystemLogDateFrom");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? SystemLogDateTo
        {
            get
            {
                return systemLogDateTo;
            }
            set
            {
                systemLogDateTo = value;
                RaisePropertyChanged("SystemLogDateTo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SystemLogCurrentPage
        {
            get
            {
                return systemLogCurrentPage;
            }
            set
            {
                systemLogCurrentPage = value;
                RaisePropertyChanged("SystemLogCurrentPage");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SystemLogPageSize
        {
            get
            {
                return systemLogPageSize;
            }
            set
            {
                if (systemLogPageSize != value)
                {
                    systemLogPageSize = value;
                    RaisePropertyChanged("SystemLogPageSize");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SystemLogPageCount
        {
            get
            {
                return systemLogPageCount == 0 ? 1 : systemLogPageCount;
            }
            set
            {
                systemLogPageCount = value;
                RaisePropertyChanged("SystemLogPageCount");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Log> SystemLogs
        {
            get
            {
                return systemLogs;
            }
            set
            {
                systemLogs = value;
                RaisePropertyChanged("SystemLogs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Log SelectedSystemLog {
            get {
                if (selectedSystemLog != null && selectedSystemLog.FormattedMessage == null)
                    selectedSystemLog = logProxy.GetLogById(selectedSystemLog.LogId);
                return selectedSystemLog;
            }
            set {
                selectedSystemLog = value;
                OnSelectedLogChanged();
                RaisePropertyChanged("SelectedSystemLog");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? OperationLogDateFrom
        {
            get
            {
                return operationLogDateFrom;
            }
            set
            {
                operationLogDateFrom = value;
                RaisePropertyChanged("OperationLogDateFrom");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? OperationLogDateTo
        {
            get
            {
                return operationLogDateTo;
            }
            set
            {
                operationLogDateTo = value;
                RaisePropertyChanged("OperationLogDateTo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OperationLogCurrentPage
        {
            get
            {
                return operationLogCurrentPage;
            }
            set
            {
                operationLogCurrentPage = value;
                RaisePropertyChanged("OperationLogCurrentPage");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OperationLogPageSize
        {
            get
            {
                return operationLogPageSize;
            }
            set
            {
                if (operationLogPageSize != value)
                {
                    operationLogPageSize = value;
                    RaisePropertyChanged("OperationLogPageSize");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OperationLogPageCount
        {
            get
            {
                return operationLogPageCount == 0 ? 1 : operationLogPageCount;
            }
            set
            {
                operationLogPageCount = value;
                RaisePropertyChanged("OperationLogPageCount");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OperationLogUserName
        {
            get { return operationLogUserName; }
            set
            {
                operationLogUserName = value;
                RaisePropertyChanged("OperationLogUserName");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOperationLogUserNameReadOnly
        {
            get { return isOperationLogUserNameReadOnly; }
            set
            {
                isOperationLogUserNameReadOnly = value;
                RaisePropertyChanged("IsOperationLogUserNameReadOnly");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Log> OperationLogs
        {
            get
            {
                return operationLogs;
            }
            set
            {
                operationLogs = value;
                RaisePropertyChanged("OperationLogs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Log SelectedOperationLog {
            get {
                if (selectedOperationLog != null && selectedOperationLog.FormattedMessage == null)
                    selectedOperationLog = logProxy.GetLogById(selectedOperationLog.LogId);
                return selectedOperationLog;
            }
            set {
                selectedOperationLog = value;
                OnSelectedLogChanged();
                RaisePropertyChanged("SelectedOperationLog");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SortedBy
        {
            get { return this.sortedBy; }
            set { this.sortedBy = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SortByDesc
        {
            get { return this.sortedByDesc; }
            set
            {
                this.sortedByDesc = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand FilterCommand
        {
            get
            {
                if (filterCommand == null)
                    filterCommand = new DelegateCommand(() =>
                    {
                        if (CurrentTab == systemTabIndex)
                            SystemLogCurrentPage = 1;
                        else if (CurrentTab == operationTabIndex)
                            OperationLogCurrentPage = 1;
                        Search();
                    });
                return filterCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ChangePageCommand
        {
            get
            {
                if (changePageCommand == null)
                    changePageCommand = new DelegateCommand(() =>
                    {
                        Search();
                    });
                return changePageCommand;
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

        #endregion

        #region Constructors & Dispose

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logProxy"></param>
        public ViewLogsViewModel(ILogProxy logProxy)
        {
            this.logProxy = logProxy;
            IsCriticalChecked = IsErrorChecked = IsWarningChecked = IsInformationChecked = true;
            SystemLogDateFrom = SystemLogDateTo = OperationLogDateFrom = OperationLogDateTo = DateTime.Today;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void ResetMenu()
        {
            if (KmtConstants.IsManager)
            {
                OnTabChanged(systemTabIndex);
                IsOperationLogUserNameReadOnly = false;
                OperationLogUserName = string.Empty;
            }
            else
            {
                OnTabChanged(operationTabIndex);
                IsOperationLogUserNameReadOnly = true;
                OperationLogUserName = KmtConstants.LoginUser.LoginId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            Search();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabIndex"></param>
        public void OnTabChanged(int tabIndex)
        {
            CurrentTab = tabIndex;
            Search();
        }

        #endregion

        #region Private & Protected methods

        private void SeverityCheckBoxChanged(bool isChecked, TraceEventType type)
        {
            if (isChecked)
                systemLogSeverity |= (int)type;
            else
                systemLogSeverity &= ~(int)type;
        }

        private void Search() {
            if ((CurrentTab == systemTabIndex && ValidationHelper.ValidateDateRange(SystemLogDateFrom, SystemLogDateTo)) ||
                (CurrentTab == operationTabIndex && ValidationHelper.ValidateDateRange(OperationLogDateFrom, OperationLogDateTo))) {
                IsBusy = true;
                WorkInBackground((s, e) => {
                    try {
                        if (CurrentTab == systemTabIndex) {
                            PagedList<Log> logs = logProxy.GetSystemLogs(systemLogSeverity,
                                SystemLogDateFrom, SystemLogDateTo, SystemLogCurrentPage, SystemLogPageSize, SortedBy, SortByDesc);
                            SystemLogs = new ObservableCollection<Log>(logs);
                            SystemLogPageCount = logs.PageCount;
                        }
                        else {
                            PagedList<Log> logs = logProxy.GetOperationLogs(OperationLogUserName,
                                OperationLogDateFrom, OperationLogDateTo, OperationLogCurrentPage, OperationLogPageSize, SortedBy, SortByDesc);
                            OperationLogs = new ObservableCollection<Log>(logs);
                            OperationLogPageCount = logs.PageCount;
                        }
                        IsBusy = false;
                    }
                    catch (Exception ex) {
                        IsBusy = false;
                        ex.ShowDialog();
                        ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                    }
                });
            }
        }

        private void OnSelectedLogChanged()
        {
            if (SelectedLogChanged != null)
                SelectedLogChanged(this, new EventArgs());
        }

        #endregion
    }
}
