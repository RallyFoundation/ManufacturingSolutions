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
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RevertKeysViewModel : TemplateViewModelBase
    {
        #region Private Members

        private ISubsidiaryProxy subProxy = null;
        private bool isAllSelected;
        private string operateMsg;
        private ObservableCollection<Subsidiary> subsidiarys = null;

        #endregion

        #region Contructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyProxy"></param>
        /// <param name="ssProxy"></param>
        public RevertKeysViewModel(IKeyProxy keyProxy, ISubsidiaryProxy ssProxy)
            : base(keyProxy)
        {
            this.subProxy = ssProxy;
            base.WindowTitle = MergedResources.Revert_RevertTxt;
            InitializeCollections();
        }

        #endregion

        #region Public Propertys

        /// <summary>
        /// List of Contracts associated with a SoldTo customer
        /// </summary>
        public ObservableCollection<Subsidiary> Subsidiarys
        {
            get { return subsidiarys; }
        }

        /// <summary>
        /// is select all
        /// </summary>
        public bool IsAllSelected
        {
            get { return isAllSelected; }
            set
            {
                isAllSelected = value;
                foreach (var keyInfo in Keys)
                {
                    if (!keyInfo.IsSelected)
                        keyInfo.IsSelected = true;
                    else
                        keyInfo.IsSelected = false;
                }
                RaisePropertyChanged("IsAllSelected");
            }
        }

        /// <summary>
        /// operate message
        /// </summary>
        public string OperateMsg
        {
            get { return operateMsg; }
            set
            {
                operateMsg = value;
                RaisePropertyChanged("OperateMsg");
            }
        }

        #endregion

        #region Private Members

        private void InitializeCollections()
        {
            InitKeyTypes();
            InitKeystates();
            TabIndex = KmtConstants.SecondTab;
            this.InitView();
            base.SearchAllKeys();
        }

        /// <summary>
        /// init UI View
        /// </summary>
        protected override void InitView()
        {
            base.InitView();
            this.IsNextButtonVisible = false;
            this.IsExecuteButtonVisible = true;
        }

        /// <summary>
        /// search keys to revert
        /// </summary>
        /// <returns></returns>
        protected override List<KeyInfoModel> SearchKeys()
        {
            List<KeyInfo> searchkeys = base.keyProxy.SearchKeysToRevert(KeySearchCriteria);
            if (searchkeys == null && searchkeys.Count <= 0)
                return null;
            else
                return searchkeys.ToKeyInfoModel().ToList();
        }

        /// <summary>
        /// search key group to revert,
        /// </summary>
        protected override void SearchKeyGroups()
        {
        }

        /// <summary>
        /// validate keys to revert
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateKeys()
        {
            if (this.CurrentPageIndex == 0)
            {
                if (!base.ValidateKeys())
                    return false;

                if (Keys.Where(k => k.IsSelected).Any(k => !string.IsNullOrEmpty(k.keyInfo.HardwareHash)))
                {
                    Keys = new ObservableCollection<KeyInfoModel>(Keys.Where(k => k.IsSelected));
                    GoToNextPage();
                    return false; // return false for further validation
                }

                return true;
            }
            else if (this.CurrentPageIndex == 1)
            {
                if (string.IsNullOrEmpty(this.OperateMsg))
                {
                    MessageBox.Show(ResourcesOfR6.RevertKey_InputMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (this.OperateMsg.Length > 200)
                {
                    MessageBox.Show(MergedResources.Revert_MsgLength, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            else
                return true;
        }

        /// <summary>
        /// validate key group to revert
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateKeyGroups()
        {
            return base.ValidateKeyGroups();
        }

        /// <summary>
        /// exec key revert 
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();
            result = keyProxy.RevertKeys(Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList(), OperateMsg, KmtConstants.LoginUser.LoginId);
            KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);
            SummaryText = string.Format(MergedResources.Revert_ResultMsg, result.Count(r => !r.Failed).ToString(), result.Count(r => r.Failed).ToString());
        }

        /// <summary>
        /// finish UI display
        /// </summary>
        protected override void GoToFinalPage()
        {
            base.IsExecuteButtonVisible = false;
            base.IsPreviousButtonVisible = false;
            base.IsCancelButtonVisible = false;
            base.IsFinishButtonVisible = true;
            CurrentPageIndex = StepPages.Count - 1;
        }

        /// <summary>
        /// Previous operation display
        /// </summary>
        protected override void GoToPreviousPage()
        {
            base.GoToPreviousPage();
            base.IsNextButtonVisible = false;
            base.IsExecuteButtonVisible = true;
            base.SearchAllKeys();
        }

        //init key states
        private void InitKeystates()
        {
            List<string> standardKeyStates = new List<string>() { MergedResources.Common_All, KeyState.Consumed.ToString(), KeyState.Bound.ToString() };
            List<string> MBRKeyStates = new List<string>() { KeyState.ActivationEnabled.ToString() };

            base.SearchControlVM.keyStates = new Dictionary<string, List<string>>();
            base.SearchControlVM.keyStates.Add(KeyType.Standard.ToString(), standardKeyStates);
            base.SearchControlVM.keyStates.Add(KeyType.MBR.ToString(), MBRKeyStates);
            base.SearchControlVM.SelectedKeyState = MergedResources.Common_All;
        }

        //init key types
        private void InitKeyTypes()
        {
            base.SearchControlVM.keyTypes = new ObservableCollection<string>();
            base.SearchControlVM.keyTypes.Add(MergedResources.Common_All);
            base.SearchControlVM.keyTypes.Add(KeyType.Standard.ToString());
            base.SearchControlVM.keyTypes.Add(KeyType.MBR.ToString());
        }

        #endregion
    }
}
