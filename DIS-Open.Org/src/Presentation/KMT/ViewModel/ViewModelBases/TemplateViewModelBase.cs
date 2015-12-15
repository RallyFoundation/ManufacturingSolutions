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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ControlsViewModel;

namespace DIS.Presentation.KMT.ViewModel.ViewModelBases
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TemplateViewModelBase : WizardViewModelBase
    {
        #region Private Members

        private bool isBusy = false;
        private string summaryText;
        private int tabIndex;
        private bool isDesc = false;
        private string sortColumn = string.Empty;

        private ObservableCollection<KeyInfoModel> keys = null;
        private ObservableCollection<KeyGroupModel> keyGroups = null;

        //Set quantity of selected Group
        private KeyGroupModel selectedKeyGroup = null;
        private ObservableCollection<KeyOperationResult> keyOperationResults = null;

        private KeySearchCriteria keySearchCriteria = null;
        
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyProxy"></param>
        public TemplateViewModelBase(IKeyProxy keyProxy)
        {
            this.keyProxy = keyProxy;
            if (keys == null)
                keys = new ObservableCollection<KeyInfoModel>();

            if (StepPages == null)
                StepPages = new ObservableCollection<Page>();

            SearchControlVM = new SearchControlViewModel();
            SearchControlVM.SearchKeys += new EventHandler((s, e) =>
            {
                if (keys != null && keys.Count > 0)
                    keys.Clear();
                isSearchFirstPage = true;
                SearchAllKeys();
            });
        }

        #endregion

        #region Public Properties

        protected bool isSearchFirstPage = false;

        /// <summary>
        /// 
        /// </summary>
        protected IKeyProxy keyProxy = null;

        /// <summary>
        /// 
        /// </summary>
        public SearchControlViewModel SearchControlVM { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public KeyGroupModel SelectedKeyGroup
        {
            get { return selectedKeyGroup; }
            set
            {
                selectedKeyGroup = value;
                if (selectedKeyGroup != null)
                    selectedKeyGroup.OnSelected();
                RaisePropertyChanged("SelectedKeyGroup");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public KeySearchCriteria KeySearchCriteria
        {
            get { return keySearchCriteria; }
            set { keySearchCriteria = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TabIndex
        {
            get { return tabIndex; }
            set
            {
                int oldtabIndex = tabIndex;
                tabIndex = value;
                if (oldtabIndex == KmtConstants.FirstTab || oldtabIndex == KmtConstants.SecondTab)
                    TabSelectionChanged();
                RaisePropertyChanged("TabIndex");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SummaryText
        {
            get
            {
                return summaryText;
            }
            set
            {
                summaryText = value;
                RaisePropertyChanged("SummaryText");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<KeyGroupModel> KeyGroups
        {
            get { return this.keyGroups; }
            set
            {
                this.keyGroups = value;
                RaisePropertyChanged("KeyGroups");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<KeyInfoModel> Keys
        {
            get { return keys; }
            set
            {
                keys = value;
                RaisePropertyChanged("Keys");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<KeyOperationResult> KeyOperationResults
        {
            get { return keyOperationResults; }
            set
            {
                keyOperationResults = value;
                RaisePropertyChanged("KeyOperationResults");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllChecked
        {
            get
            {
                if (Keys == null || Keys.Count <= 0)
                    return false;
                else
                    return Keys.All(k => k.IsSelected);
            }
            set
            {
                if (Keys != null && Keys.Count > 0)
                {
                    foreach (var key in Keys)
                    {
                        key.IsSelected = value;
                    }
                    RaisePropertyChanged("IsAllChecked");
                }
            }
        }

        #endregion

        #region search keys

        /// <summary>
        /// Perform a query of all key
        /// </summary>
        /// <param name="startIndex"></param>
        protected void SearchAllKeys(int startIndex = 0)
        {
            KeySearchCriteria = SearchControlVM.FillSearchCriteria();
            if (!ValidationHelper.ValidateDateRange(KeySearchCriteria.DateFrom, KeySearchCriteria.DateTo))
                return;
            KeySearchCriteria.StartIndex = startIndex;
            if (!string.IsNullOrEmpty(sortColumn))
            {
                KeySearchCriteria.SortBy = sortColumn;
                KeySearchCriteria.SortByDesc = isDesc;
            }
            if (TabIndex == KmtConstants.FirstTab)
                SearchKeyGroups();
            else if (TabIndex == KmtConstants.SecondTab)
                GetPageKeys();

            if ((keys != null && Keys.Count > 0) || (keyGroups != null && keyGroups.Count > 0))
                base.IsExecuteButtonEnable = true;
            else
                base.IsExecuteButtonEnable = false;

            KeySearchCriteria = null;
            RaisePropertyChanged("IsAllChecked");
        }

        /// <summary>
        /// Get key of the current page
        /// </summary>
        protected virtual void GetPageKeys()
        {
            List<KeyInfoModel> newkeys = SearchKeys();
            if (Keys == null)
                keys = new ObservableCollection<KeyInfoModel>();
            if (newkeys != null && newkeys.Count > 0)
            {
                foreach (var key in newkeys)
                {
                    if (!Keys.Any(k => k.keyInfo.KeyId == key.keyInfo.KeyId))
                    {
                        key.PropertyChanged += OnKeySelectedChanged;
                        Keys.Add(key);
                    }
                }
                RaisePropertyChanged("Keys");
            }
        }

        /// <summary>
        /// Query the current key collection
        /// </summary>
        protected abstract void SearchKeyGroups();

        /// <summary>
        /// Query the current keys
        /// </summary>
        /// <returns></returns>
        protected abstract List<KeyInfoModel> SearchKeys();

        /// <summary>
        /// Classes should override this and provide a method for the implementation execute keys logic
        /// </summary>
        protected abstract void ProcessExecuteKeys();

        #endregion

        #region template method

        /// <summary>
        /// Execute keys,classes can override this and provide a method for the implementation execute keys logic
        /// </summary>
        protected override void Execute()
        {
            if (!ValidateExecuteKeys())
                return;

            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    ProcessExecuteKeys();
                    RaisePropertyChanged("SummaryText");

                    var message = SummaryText + System.Environment.NewLine + System.Environment.NewLine;
                    var succeedMessage = MergedResources.Common_succeedKeys + System.Environment.NewLine + System.Environment.NewLine;
                    var failedMessage = MergedResources.Common_failedKeys + System.Environment.NewLine + System.Environment.NewLine;

                    foreach (var result in KeyOperationResults)
                    {
                        if (result.Failed)
                        {
                            if (result.Key != null) //To support batch import and orginal file tracking -- Rally, Nov 24, 2014
                            {
                                failedMessage += result.Key.ProductKey + "," + MergedResources.Common_FailedType + result.FailedType + System.Environment.NewLine;
                            }
                            else
                            {
                                failedMessage += result.OriginalFileName + "," + MergedResources.Common_FailedType + result.FailedType + System.Environment.NewLine;
                            }
                        }
                        else 
                        {
                            succeedMessage += result.Key.ProductKey + System.Environment.NewLine;
                        }
                    }

                    message = message + succeedMessage + System.Environment.NewLine + failedMessage;
                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, message, KmtConstants.CurrentDBConnectionString);
                    IsBusy = false;
                    Dispatch(() =>
                    {
                        GoToFinalPage();
                    });
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

        #region validations

        /// <summary>
        /// Validate keys
        /// </summary>
        /// <returns></returns>
        private bool ValidateExecuteKeys()
        {
            bool flag = false;
            if (TabIndex == KmtConstants.FirstTab)
            {
                flag = ValidateKeyGroups();
            }
            else if (tabIndex == KmtConstants.SecondTab)
            {
                flag = ValidateKeys();
            }
            else
            {
                flag = ValidateOthers();
            }
                return flag;
        }

        /// <summary>
        /// Validate by keys input
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateKeys()
        {
            bool flag = false;
            if (Keys == null || Keys.Count <= 0)
            {
                MessageBox.Show(MergedResources.Common_OperationNoKeysMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var selectKeys = Keys.Where(k => k.IsSelected);
            if (selectKeys.Count() <= 0)
            {
                MessageBox.Show(MergedResources.Common_SelectKeysMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (selectKeys.Count() > Constants.BatchLimit)
            {
                MessageBox.Show(MergedResources.Common_OperationLimitQualityMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Validate key groups input
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateKeyGroups()
        {
            bool result = true;
            if (KeyGroups == null || KeyGroups.Count() <= 0)
            {
                MessageBox.Show(MergedResources.Common_OperationNoKeysMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (KeyGroups.Where(k => k.KeyGroup.Quantity > 0).Sum(k => k.KeyGroup.Quantity) <= 0)
            {
                MessageBox.Show(MergedResources.Common_OperationNoInputQuantityMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach (var keyInfo in KeyGroups)
            {
                int actualQuantity = keyInfo.KeyGroup.Quantity;
                if (actualQuantity < 0)
                {
                    MessageBox.Show(MergedResources.Common_OperationNoInputQuantityMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if (keyInfo.KeyGroup.Quantity > keyInfo.KeyGroup.AvailableKeysCount)
                {
                    MessageBox.Show(MergedResources.Common_OperationQuantityOver, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            if (KeyGroups.Where(k => k.KeyGroup.Quantity > 0).Sum(k => k.KeyGroup.Quantity) > Constants.BatchLimit)
            {
                MessageBox.Show(MergedResources.Common_OperationLimitQualityMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return result;
        }

        /// <summary>
        /// Validate others
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateOthers()
        {
            return true;
        }

        #endregion

        #region protected Members

        protected void OnKeySelectedChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("IsAllChecked");
        }

        #endregion

        #region internal Members

        internal void LoadUpKeys()
        {
            SearchAllKeys(Keys.Count);
        }

        internal virtual void SortingByColumn(string sortColumn)
        {
            this.sortColumn = sortColumn;
            isDesc = !isDesc;
            keys = null;
            SearchAllKeys();
        }

        internal void TabSelectionChanged()
        {
            if (keySearchCriteria != null || keys == null || keys.Count == 0 || KeyGroups == null || KeyGroups.Count == 0)
            {
                if (keys != null && keys.Count > 0)
                    Keys.Clear();
                SearchAllKeys();
            }
        }

        #endregion
    }
}

