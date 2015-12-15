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
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel.Key
{
    /// <summary>
    /// ViewModel of duplicateKeys
    /// </summary>
    public class DuplicateKeysViewModel : ViewModelBase
    {
        #region Private Members

        /// <summary>
        /// Key Manager Instance
        /// </summary>
        private IKeyProxy keyProxy = null;
        private ISubsidiaryProxy ssProxy = null;

        private ObservableCollection<KeyDuplicated> keyCollection = null;
        private DelegateCommand finishCommand;
        private DelegateCommand cancelCommand;
        private string summaryText = "";
        private bool isBusy = false;
        private string commentTxt = "";
        private KeyManagementViewModel ownerContext = null;

        #endregion

        #region Contructor

        /// <summary>
        /// DuplicateKeysViewModel constructor
        /// </summary>

        public DuplicateKeysViewModel(IKeyProxy keyProxy, ISubsidiaryProxy ssProxy)
        {
            this.keyProxy = keyProxy;
            this.ssProxy = ssProxy;
            InitializeCollections();
        }

        #endregion

        #region Public Propertys

        /// <summary>
        /// 
        /// </summary>
        public KeyManagementViewModel OwnerContext
        {
            get { return this.ownerContext; }
            set { this.ownerContext = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<KeyDuplicated> KeyCollection
        {
            get { return this.keyCollection; }
            set
            {
                this.keyCollection = value;
                RaisePropertyChanged("KeyCollection");
            }
        }

        /// <summary>
        /// Summary message shows on summary window
        /// </summary>
        public string SummaryText
        {
            get { return summaryText; }
        }

        /// <summary>
        /// Summary message shows on summary window
        /// </summary>
        public string CommentText
        {
            get { return commentTxt; }
            set
            {
                commentTxt = value;
                RaisePropertyChanged("CommentText");
            }
        }

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
        /// Command used on clicking Finish button
        /// </summary>
        public ICommand FinishCommand
        {
            get
            {
                if (finishCommand == null)
                {
                    finishCommand = new DelegateCommand(ProcessKeys);
                }
                return finishCommand;
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
        #endregion

        #region Private Members

        /// <summary>
        /// Loads default data on window loading
        /// </summary>
        private void InitializeCollections()
        {
            GetDuplicateKeys();
        }

        /// <summary>
        /// Get all duplicate keys
        /// </summary>
        private void GetDuplicateKeys()
        {
            keyCollection = new ObservableCollection<KeyDuplicated>(keyProxy.GetKeysDuplicated());
        }
        /// <summary>
        /// Process Duplicate Keys
        /// </summary>
        private void ProcessKeys()
        {
            if (KeyCollection.Where(k => k.ReuseOperation != ReuseOperation.None).Count() == 0)
            {
                MessageBox.Show(MergedResources.ProcessDuplicateKeysViewModel_NoKeysMsg, MergedResources.Common_Error);
                return;
            }
            if (string.IsNullOrEmpty(commentTxt))
            {
                MessageBox.Show(MergedResources.ProcessDuplicateKeysViewModel_NOReason, MergedResources.Common_Error);
                return;
            }
            try
            {
                keyProxy.HandleKeysDuplicated(new List<KeyDuplicated>(keyCollection), KmtConstants.LoginUser.LoginId, commentTxt);

                string msg = "";
                if (keyCollection.Where(k => k.ReuseOperation == ReuseOperation.Reuse).Count() > 0)
                    msg = string.Format(MergedResources.ProcessDuplicateKeysViewModel_HadleKeys, keyCollection.Where(k => k.ReuseOperation == ReuseOperation.Reuse).Count()) + Environment.NewLine;
                if (keyCollection.Where(k => k.ReuseOperation == ReuseOperation.Reuse).Count() > 0)
                    msg += string.Format(MergedResources.ProcessDuplicateKeysViewModel_HadleIgnoreKeys, keyCollection.Where(k => k.ReuseOperation == ReuseOperation.Ignore).Count());

                MessageBoxResult key = MessageBox.Show(
                msg,
                MergedResources.ProcessDuplicateKeysViewModel_MessageTitle,
                MessageBoxButton.OK);
                if (key == MessageBoxResult.OK)
                {
                    RequestClose();
                }

            }
            catch (Exception ex)
            {
                ex.ShowDialog();
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
            }
        }
        /// <summary>
        /// Close the window
        /// </summary>
        private void Cancel()
        {
            RequestClose();
        }

        private void RequestClose()
        {
            View.Close();
        }
        #endregion

    }
}
