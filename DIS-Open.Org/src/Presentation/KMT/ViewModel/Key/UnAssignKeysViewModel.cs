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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;
using DIS.Presentation.KMT.Commands;
using System.Windows.Input;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// View Model class for UnAssign Keys View
    /// </summary>
    public sealed class UnAssignKeysViewModel : TemplateViewModelBase
    {
        #region Private Members

        private ISubsidiaryProxy subProxy = null;
        private ObservableCollection<Subsidiary> subsidiarys = null;
        private Subsidiary selectedSubsidiary = null;

        #endregion

        #region Contructor

        /// <summary>
        /// AssignKeysViewModel constructor
        /// </summary>
        public UnAssignKeysViewModel(IKeyProxy keyProxy, ISubsidiaryProxy subProxy)
            : base(keyProxy)
        {
            base.WindowTitle = MergedResources.MainWindow_Unassign;
            this.subProxy = subProxy;
            Initialize();
        }
        #endregion


        #region Public Propertys

        public ObservableCollection<Subsidiary> Subsidiarys
        {
            get { return subsidiarys; }
        }

        public Subsidiary SelectedSubsidiary
        {
            get { return selectedSubsidiary; }
            set
            {
                selectedSubsidiary = value;
                RaisePropertyChanged("SelectedSubsidiary");
            }
        }

        private DelegateCommand nextCommand;
        public override ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                    nextCommand = new DelegateCommand(() => { GoToNextPage(); },
                    () =>
                    {
                        return this.SelectedSubsidiary != null;
                    });
                return nextCommand;
            }
        }

        #endregion

        #region Private Members

        private void Initialize()
        {
            SearchSubSidiary();
            this.InitView();
        }

        private void SearchSubSidiary()
        {
            subsidiarys = new ObservableCollection<Subsidiary>(subProxy.GetSubsidiaries());
            if (subsidiarys.Count > 0)
                selectedSubsidiary = subsidiarys.First();
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

        //go to next page
        protected override void GoToNextPage()
        {
            base.GoToNextPage();
            base.TabIndex = KmtConstants.SecondTab;//only by keys
            base.SearchAllKeys();
        }

        //search key groups to assign
        protected override void SearchKeyGroups() { }

        //search keys to assign
        protected override List<KeyInfoModel> SearchKeys()
        {
            if (SelectedSubsidiary == null)
                return null;
            base.KeySearchCriteria.SsId = SelectedSubsidiary.SsId;
            base.KeySearchCriteria.PageSize = 0;
            List<KeyInfo> searchkeys = keyProxy.SearchUnassignKeys(KeySearchCriteria);
            if (searchkeys == null && searchkeys.Count <= 0)
                return null;
            else
                return searchkeys.ToKeyInfoModel().ToList();
        }

        //validate keys
        protected override bool ValidateKeys()
        {
            return SelectedSubsidiary != null && base.ValidateKeys();
        }

        //validate key groups
        protected override bool ValidateKeyGroups()
        {
            return true;
        }

        /// <summary>
        /// unAssign keys
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();
            result = keyProxy.UnassignKeys(base.Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList());
            base.KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);
            base.SummaryText = base.SummaryText = string.Format(MergedResources.UnassignKeysViewModel_UnassignKeysResult,
                    base.KeyOperationResults.Where(r => !r.Failed).Count(),
                    base.KeyOperationResults.Where(r => r.Failed).Count());
        }

        #endregion
    }
}
