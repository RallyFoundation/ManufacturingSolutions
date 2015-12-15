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
using DIS.Presentation.KMT.ViewModel.ControlsViewModel;
using DIS.Presentation.KMT.Commands;

namespace DIS.Presentation.KMT.ViewModel
{
    public class ReturnKeysViewModel : TemplateViewModelBase
    {
        #region Public Properties

        public ReturnKeysListViewModel ReturnKeysListModelVM = null;

        /// <summary>
        /// 
        /// </summary>
        public string SendFailedText
        {
            get
            {
                return sendFailedText;
            }
            set
            {
                sendFailedText = value;
                RaisePropertyChanged("SendFailedText");
            }
        }

        #endregion

        #region Contructor

        /// <summary>
        /// ReturnKeysViewModel constructor
        /// </summary>
        /// <param name="keyProxy"></param>
        public ReturnKeysViewModel(IKeyProxy keyProxy)
            : base(keyProxy)
        {
            base.WindowTitle = ResourcesOfR6.ReturnKeysView_Title;
            Initialize();
        }

        #endregion

        #region Private Members

        private string sendFailedText;

        private void Initialize()
        {
            if (ReturnKeysListModelVM == null)
                ReturnKeysListModelVM = new ReturnKeysListViewModel();

            InitKeyTypes();
            InitKeystates();
            this.InitView();
        }

        // avoid perfermonse issue when user select all key states 
        private void InitKeyTypes()
        {
            List<string> keyTypes = new List<string>(){KeyType.Standard.ToString(), KeyType.MBR.ToString(), KeyType.MAT.ToString()};
            base.SearchControlVM.KeyTypes = new ObservableCollection<string>(keyTypes);
            base.SearchControlVM.SelectedKeyType = base.SearchControlVM.KeyTypes.First();
        }

        private void InitKeystates()
        {
            List<string> standardKeyStates = new List<string>() { MergedResources.Common_All, KeyState.Fulfilled.ToString(), KeyState.Bound.ToString(), KeyState.ActivationEnabled.ToString(), KeyState.ActivationDenied.ToString() };
            List<string> MBRKeyStates = new List<string>() { MergedResources.Common_All, KeyState.Fulfilled.ToString(), KeyState.ActivationEnabled.ToString() };
            List<string> MATKeyStates = new List<string>() { KeyState.Fulfilled.ToString() };

            base.SearchControlVM.keyStates = new Dictionary<string, List<string>>();
            base.SearchControlVM.keyStates.Add(KeyType.Standard.ToString(), standardKeyStates);
            base.SearchControlVM.keyStates.Add(KeyType.MBR.ToString(), MBRKeyStates);
            base.SearchControlVM.keyStates.Add(KeyType.MAT.ToString(), MATKeyStates);
            base.SearchControlVM.SelectedKeyState = MergedResources.Common_All;
        }

        #endregion

        #region Override Members

        protected override void InitView()
        {
            this.IsCancelButtonVisible = true;
            this.IsNextButtonVisible = true;
            this.IsExecuteButtonVisible = false;
            this.IsPreviousButtonVisible = false;
            this.IsFinishButtonVisible = false;
            if (StepPages != null && StepPages.Count > 0)
                this.CurrentPageIndex = 0;
        }

        /// <summary>
        /// next operation
        /// </summary>
        protected override void GoToNextPage()
        {
            if (ReturnKeysListModelVM.ValidateOemRmaNumberTxt() && ReturnKeysListModelVM.ValidateReturnNoCredit() && base.CurrentPageIndex == 0 && base.StepPages[CurrentPageIndex].IsLoaded)
            {                
                base.TabIndex = KmtConstants.SecondTab;               
                base.GoToNextPage();
            }
        }

        protected override void SearchKeyGroups() { }

        /// <summary>
        /// search keys to return
        /// </summary>
        /// <returns></returns>
        protected override List<KeyInfoModel> SearchKeys()
        {
            List<KeyInfo> searchkeys = base.keyProxy.SearchToReturnKeys(KeySearchCriteria);
            List<KeyInfoModel> returnKeys = new List<KeyInfoModel>();
            if (searchkeys == null && searchkeys.Count <= 0)
                returnKeys = null;
            else
                returnKeys = searchkeys.ToKeyInfoModel().ToList();
            return returnKeys;
        }

        //get keys by page
        protected override void GetPageKeys()
        {
            base.GetPageKeys();
            var newReturnKeys = new ObservableCollection<ReturnKeyModel>(base.Keys.Select(k => new ReturnKeyModel() { ReturnReportKey = k }).ToList());

            if (this.ReturnKeysListModelVM.ReturnKeys == null)
                this.ReturnKeysListModelVM.ReturnKeys = new ObservableCollection<ReturnKeyModel>();           
            if (isSearchFirstPage)
            {
                this.ReturnKeysListModelVM.ReturnKeys.Clear();
                isSearchFirstPage = false;
            }

            if (newReturnKeys != null && newReturnKeys.Count > 0)
            {
                foreach (var key in newReturnKeys)
                {
                    if (!this.ReturnKeysListModelVM.ReturnKeys.Any(k => k.ReturnReportKey.keyInfo.KeyId == key.ReturnReportKey.keyInfo.KeyId))
                    {
                        key.PropertyChanged += base.OnKeySelectedChanged;
                        this.ReturnKeysListModelVM.ReturnKeys.Add(key);
                    }
                }
                RaisePropertyChanged("ReturnKeys");
            }
        
            ReturnKeysListModelVM.AddkeyReturnTypeByCredit();

        }

        /// <summary>
        /// validate keys to return
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateKeys()
        {
            return ReturnKeysListModelVM.ValidateOemRmaNumberTxt() && ReturnKeysListModelVM.ValidateKeyStateNotice() && base.ValidateKeys();
        }

        /// <summary>
        /// exec return keys
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();
            ReturnReport returnReport = ReturnKeysListModelVM.ReturnKeys.Where(k => k.ReturnReportKey.IsSelected).ToReturnReport(ReturnKeysListModelVM.OemRmaNumber, ReturnKeysListModelVM.OemRmaDate.Value, ReturnKeysListModelVM.ReturnNoCredit);
            result = base.keyProxy.SaveGeneratedReturnReport(returnReport);

            base.KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);

            base.SummaryText = string.Format(ResourcesOfR6.ReturnKeysViewModel_ReturnSuccess,
                    base.KeyOperationResults.Where(r => !r.Failed).Count(),
                    base.KeyOperationResults.Where(r => r.Failed).Count());
            if (base.KeyOperationResults.Any(r => r.Failed))
            {
                this.SendFailedText = ResourcesOfR6.Common_ReportFailed;
            }
        }

        #endregion
    }
}
