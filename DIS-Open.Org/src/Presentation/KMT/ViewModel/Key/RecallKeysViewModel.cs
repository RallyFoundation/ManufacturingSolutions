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

namespace DIS.Presentation.KMT.ViewModel.Key
{
    /// <summary>
    /// 
    /// </summary>
    public class RecallKeysViewModel : TemplateViewModelBase
    {
        #region Contructor

        /// <summary>
        /// RecallKeysViewModel constructor
        /// </summary>
        public RecallKeysViewModel(IKeyProxy keyProxy):base(keyProxy)
        {
            base.WindowTitle = MergedResources.MainWindow_RecallKeys;
            InitializeUICollections();
        }

        #endregion

        #region Private Members

        private void InitializeUICollections()
        {
            base.InitView();
            base.SearchAllKeys();
        }

        #endregion

        #region Override Members

        /// <summary>
        /// search keys to recall
        /// </summary>
        /// <returns></returns>
        protected override List<KeyInfoModel>  SearchKeys()
        {
            List<KeyInfo> searchkeys = base.keyProxy.SearchRecallKeys(KeySearchCriteria);
            if (searchkeys == null && searchkeys.Count <= 0)
                return null;
            else
                return searchkeys.ToKeyInfoModel().ToList(); 
        }

        /// <summary>
        /// search key groups to recall
        /// </summary>
        protected override void  SearchKeyGroups()
        {
            KeyGroups = new ObservableCollection<KeyGroupModel>(base.keyProxy.SearchRecallKeyGroups(KeySearchCriteria).ToKeyGroupModel());
        }

        /// <summary>
        /// exec recall keys
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();

            if (base.TabIndex == KmtConstants.FirstTab)
                result = base.keyProxy.SendKeysForRecalling(base.KeyGroups.Select(k => k.KeyGroup).Where(k => k.Quantity > 0).ToList());
            else
                result = base.keyProxy.SendKeysForRecalling(base.Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList());

            base.KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);
            base.SummaryText = string.Format(MergedResources.RecallKeysViewModel_RecallKeysResult,
                    base.KeyOperationResults.Where(r => !r.Failed).Count(),
                    base.KeyOperationResults.Where(r => r.Failed).Count());
        }

        #endregion
    }
}
