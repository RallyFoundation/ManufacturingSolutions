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
using System.Windows;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// View Model class for Assign Keys View
    /// </summary>
    public class AssignKeysViewModel : TemplateViewModelBase
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

        public AssignKeysViewModel(IKeyProxy keyProxy, ISubsidiaryProxy subProxy)
            : base(keyProxy)
        {
            base.WindowTitle = MergedResources.MainWindow_Assign;
            this.subProxy = subProxy;
            Initialize();
        }

        #endregion

        #region Public Propertys

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Subsidiary> Subsidiarys
        {
            get { return subsidiarys; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Subsidiary SelectedSubsidiary
        {
            get { return selectedSubsidiary; }
            set
            {
                selectedSubsidiary = value;
                RaisePropertyChanged("SelectedSubsidiary");
            }
        }

        #endregion

        #region Private Members

        private void Initialize()
        {
            SearchSubSidiary();
            base.InitView();
            base.SearchAllKeys();
        }

        private void SearchSubSidiary()
        {
            subsidiarys = new ObservableCollection<Subsidiary>(subProxy.GetSubsidiaries());
            if (subsidiarys.Count > 0)
                selectedSubsidiary = subsidiarys.First();
        }

        #endregion

        #region Override Members

        //search key groups to assign
        protected override void SearchKeyGroups()
        {
            KeyGroups = new ObservableCollection<KeyGroupModel>(base.keyProxy.SearchAssignKeyGroups(KeySearchCriteria).ToKeyGroupModel());
        }

        //search keys to assign 
        protected override List<KeyInfoModel> SearchKeys()
        {
            List<KeyInfo> searchkeys = base.keyProxy.SearchAssignKeys(KeySearchCriteria);
            if (searchkeys == null && searchkeys.Count <= 0)
                return null;
            else
                return searchkeys.ToKeyInfoModel().ToList();
        }

        //validate assign keys
        protected override bool ValidateKeys()
        {
            if (SelectedSubsidiary == null)
            {
                MessageBox.Show(MergedResources.Common_OperationNoSubsidiaryMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return base.ValidateKeys();
        }

        //validate assign key groups
        protected override bool ValidateKeyGroups()
        {
            if (SelectedSubsidiary == null)
            {
                MessageBox.Show(MergedResources.Common_OperationNoSubsidiaryMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
                return base.ValidateKeyGroups();
        }

        //assign keys
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();

            if (base.TabIndex == KmtConstants.FirstTab)
                result = base.keyProxy.AssignKeys(base.KeyGroups.Select(k => k.KeyGroup).Where(k => k.Quantity > 0).ToList(),
                    SelectedSubsidiary.SsId);
            else
                result = base.keyProxy.AssignKeys(base.Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList(), SelectedSubsidiary.SsId);

            base.KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);

            base.SummaryText = string.Format(MergedResources.AssignKeysViewModel_AssignKeysResult,
                    base.KeyOperationResults.Where(r => !r.Failed).Count(),
                    base.KeyOperationResults.Where(r => r.Failed).Count());
        }

        #endregion
    }
}

