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
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ControlsViewModel;
using System.Windows.Input;
using System.Windows;
using DIS.Presentation.KMT.Behaviors;

namespace DIS.Presentation.KMT.ViewModel.Key
{
    /// <summary>
    /// ViewModel of EditKeysOptionalInfo
    /// </summary>
    public sealed class EditKeysOptionalInfoViewModel : ViewModelBase
    {
        #region private fields

        private IKeyProxy keyProxy;

        private string sortBy;
        private bool isDesc;
        private KeySearchCriteria crit;
        private string zpc_model_sku;
        private string zoem_ext_id;
        private string zmauf_geo_loc;
        private string zpgm_elig_values;
        private string zchannel_rel_id;
        //add for V1.6
        private List<string> zfrm_factor_cl1s;
        private List<string> zfrm_factor_cl2s;
        private string selectedzfrm_factor_cl1;
        private string selectedzfrm_factor_cl2;
        private string zscreen_size;
        private List<string> ztouch_screens;
        private string selectedztouch_screen;

        private bool isBusy;
        private bool canEdit;
        private KeyInfoModel selectedKey;
        private OemOptionalInfo optionalInfo = new OemOptionalInfo();
        private ObservableCollection<KeyInfoModel> keys = null;

        private DelegateCommand saveCommand;
        private DelegateCommand deleteCommand;
        private DelegateCommand clearCommand;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public EditKeysOptionalInfoViewModel(IKeyProxy keyProxy, List<KeyInfo> keys)
        {
            this.keyProxy = keyProxy;
            LoadZfactorCl1s();
            LoadZtouchScerrens();
            SCVM = new SearchControlViewModel();
            SCVM.KeyTypesVisibility = Visibility.Collapsed;
            SCVM.SearchKeys += new EventHandler(SearchKeys);
            SearchKeys(null, null);

            if (keys != null)
            {
                Keys.Clear();
                foreach (var m in keys.ToKeyInfoModel())
                {
                    Keys.Add(m);
                }
            }
        }

        #region public property

        /// <summary>
        /// the ui searchcontrol view model
        /// </summary>
        public SearchControlViewModel SCVM { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new DelegateCommand(this.SaveOptionalInfo);
                return saveCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new DelegateCommand(this.DeleteOptionInfo);
                return deleteCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                    clearCommand = new DelegateCommand(Clear);
                return clearCommand;
            }
        }

        /// <summary>
        /// the selected key
        /// </summary>
        public KeyInfoModel SelectedKey
        {
            get
            {
                return this.selectedKey;
            }
            set
            {
                this.selectedKey = value;
                this.InitOptionalInfo(selectedKey == null ? null : selectedKey.keyInfo);
                RaisePropertyChanged("SelectedKey");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanEdit
        {
            get { return this.canEdit; }
            set
            {
                this.canEdit = value;
                RaisePropertyChanged("CanEdit");
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
            get { return Keys.Any() && Keys.All(k => k.IsSelected); }
            set
            {
                foreach (var key in Keys)
                {
                    key.IsSelected = value;
                }
                RaisePropertyChanged("IsAllChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ZPC_MODEL_SKU
        {
            get { return this.zpc_model_sku; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.zpc_model_sku = null;
                else
                    this.zpc_model_sku = value;
                RaisePropertyChanged("ZPC_MODEL_SKU");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ZOEM_EXT_ID
        {
            get { return this.zoem_ext_id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.zoem_ext_id = null;
                else
                    this.zoem_ext_id = value;
                RaisePropertyChanged("ZOEM_EXT_ID");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ZMAUF_GEO_LOC
        {
            get { return this.zmauf_geo_loc; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.zmauf_geo_loc = null;
                else
                    this.zmauf_geo_loc = value;
                RaisePropertyChanged("ZMAUF_GEO_LOC");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ZPGM_ELIG_VALUES
        {
            get { return this.zpgm_elig_values; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.zpgm_elig_values = null;
                else
                    this.zpgm_elig_values = value;
                RaisePropertyChanged("ZPGM_ELIG_VALUES");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ZCHANNEL_REL_ID
        {
            get { return this.zchannel_rel_id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.zchannel_rel_id = null;
                else
                    this.zchannel_rel_id = value;
                RaisePropertyChanged("ZCHANNEL_REL_ID");
            }
        }

        public List<string> ZFRM_FACTOR_CL1s
        {
            get { return this.zfrm_factor_cl1s; }
        }
        public List<string> ZFRM_FACTOR_CL2s
        {
            get { return this.zfrm_factor_cl2s; }
        }

        public string SelectedZFRM_FACTOR_CL1
        {
            get { return this.selectedzfrm_factor_cl1; }
            set
            {
                this.selectedzfrm_factor_cl1 = value;
                LoadZfactorCl2s(value);
                RaisePropertyChanged("SelectedZFRM_FACTOR_CL1");
                RaisePropertyChanged("ZFRM_FACTOR_CL2s");

            }
        }

        public string SelectedZFRM_FACTOR_CL2
        {
            get { return this.selectedzfrm_factor_cl2; }
            set
            {
                this.selectedzfrm_factor_cl2 = value;
                RaisePropertyChanged("SelectedZFRM_FACTOR_CL2");
            }
        }

        public string ZSCREEN_SIZE
        {
            get { return this.zscreen_size; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.zscreen_size = null;
                else
                    this.zscreen_size = value;
                RaisePropertyChanged("ZSCREEN_SIZE");
            }
        }

        public List<string> ZTOUCH_SCREENs
        {
            get { return this.ztouch_screens; }
        }
        public string SelectedZTOUCH_SCREEN
        {
            get { return this.selectedztouch_screen; }
            set
            {
                this.selectedztouch_screen = value;
                RaisePropertyChanged("SelectedZTOUCH_SCREEN");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<KeyInfoModel> Keys
        {
            get
            {
                return this.keys;
            }
            set
            {
                this.keys = value;
                RaisePropertyChanged("Keys");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOptionalInfoChanged { get; set; }

        #endregion

        #region public method

        /// <summary>
        /// 
        /// </summary>
        public void LoadNextPage()
        {
            try
            {
                this.crit.StartIndex = Keys.Count;
                if (!string.IsNullOrEmpty(this.sortBy))
                    this.crit.SortBy = this.sortBy;
                this.crit.SortByDesc = this.isDesc;

                this.LoadKeys();
            }
            catch (Exception ex)
            {
                ex.ShowDialog();
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SortingByColumn(string sortColumn)
        {
            this.sortBy = sortColumn;
            this.isDesc = !isDesc;
            SearchKeys(null, null);
        }

        public void SelectSingleKey()
        {
            RaisePropertyChanged("IsAllChecked");

            if (this.Keys.Any(k => k.IsSelected))
                this.CanEdit = true;
            else
                this.CanEdit = false;
        }


        #endregion

        #region private method

        private void SelectOrUnSelectKeys(bool IsSelected)
        {
            if (Keys != null)
                foreach (KeyInfoModel km in Keys)
                    km.IsSelected = IsSelected;
        }

        private void SearchKeys(object sender, EventArgs e)
        {
            crit = SCVM.FillSearchCriteria();
            crit.KeyState = KeyState.Bound;
            if (!ValidationHelper.ValidateDateRange(crit.DateFrom, crit.DateTo))
                return;
            if (Keys == null)
                this.Keys = new ObservableCollection<KeyInfoModel>();
            this.Keys.Clear();
            this.LoadNextPage();
        }

        private string UpdateOptionalInfo()
        {
            List<KeyInfo> keyInfos = Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList();
            if (keyInfos.Count <= 0)
                return null;
            List<KeyOperationResult> results = keyProxy.UpdateOemOptionInfo(keyInfos, optionalInfo);
            string summary = string.Format(MergedResources.EditOptionalInfoViewModel_UpdateResult,
                results.Count(k => k.FailedType != KeyErrorType.None), results.Count(k => k.FailedType == KeyErrorType.None));
            IsOptionalInfoChanged = true;
            return summary;
        }

        private void InitOptionalInfo(KeyInfo key)
        {
            if (key != null)
            {
                this.ZCHANNEL_REL_ID = key.ZCHANNEL_REL_ID;
                this.ZMAUF_GEO_LOC = key.ZMANUF_GEO_LOC;
                this.ZOEM_EXT_ID = key.ZOEM_EXT_ID;
                this.ZPC_MODEL_SKU = key.ZPC_MODEL_SKU;
                this.ZPGM_ELIG_VALUES = key.ZPGM_ELIG_VALUES;

                this.SelectedZFRM_FACTOR_CL1 = key.ZFRM_FACTOR_CL1;
                this.SelectedZFRM_FACTOR_CL2 = key.ZFRM_FACTOR_CL2;
                this.ZSCREEN_SIZE = key.ZSCREEN_SIZE;
                this.SelectedZTOUCH_SCREEN = key.ZTOUCH_SCREEN;
            }
        }

        /// <summary>
        /// add or update optionalinfo
        /// </summary>
        private void SaveOptionalInfo()
        {
            if (!ValidateOptionalInfo())
                return;
            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    if (Keys != null)
                    {
                        optionalInfo.ZCHANNEL_REL_ID = string.IsNullOrEmpty(ZCHANNEL_REL_ID) ? null : ZCHANNEL_REL_ID;
                        optionalInfo.ZMANUF_GEO_LOC = string.IsNullOrEmpty(ZMAUF_GEO_LOC) ? null : ZMAUF_GEO_LOC;
                        optionalInfo.ZOEM_EXT_ID = string.IsNullOrEmpty(ZOEM_EXT_ID) ? null : ZOEM_EXT_ID;
                        optionalInfo.ZPC_MODEL_SKU = string.IsNullOrEmpty(ZPC_MODEL_SKU) ? null : ZPC_MODEL_SKU;
                        optionalInfo.ZPGM_ELIG_VALUES = string.IsNullOrEmpty(ZPGM_ELIG_VALUES) ? null : ZPGM_ELIG_VALUES;

                        optionalInfo.ZFRM_FACTOR_CL1 = string.IsNullOrEmpty(SelectedZFRM_FACTOR_CL1) ? null : SelectedZFRM_FACTOR_CL1;
                        optionalInfo.ZFRM_FACTOR_CL2 = string.IsNullOrEmpty(SelectedZFRM_FACTOR_CL2) ? null : SelectedZFRM_FACTOR_CL2;
                        optionalInfo.ZSCREEN_SIZE = string.IsNullOrEmpty(ZSCREEN_SIZE) ? null : ZSCREEN_SIZE;
                        optionalInfo.ZTOUCH_SCREEN = string.IsNullOrEmpty(SelectedZTOUCH_SCREEN) ? null : SelectedZTOUCH_SCREEN;
                        string result = UpdateOptionalInfo();
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                          string.Format("CBR's Oem Optional information was changed, " +
                                        " ZCHANNEL_REL_ID:{0}, ZMAUF_GEO_LOC:{1}, ZOEM_EXT_ID:{2}, ZPC_MODEL_SKU:{3} ," +
                                        " ZPGM_ELIG_VALUES:{4},ZFRM_FACTOR_CL1:{5},ZFRM_FACTOR_CL2:{6},ZSCREEN_SIZE:{7},ZTOUCH_SCREEN:{8} ",
                              this.ZCHANNEL_REL_ID, this.ZMAUF_GEO_LOC, this.ZOEM_EXT_ID, this.ZPC_MODEL_SKU, this.ZPGM_ELIG_VALUES, this.SelectedZFRM_FACTOR_CL1,
                              this.SelectedZFRM_FACTOR_CL2, this.ZSCREEN_SIZE, this.SelectedZTOUCH_SCREEN), KmtConstants.CurrentDBConnectionString);
                        Dispatch(() =>
                        {
                            if (result != null)
                                ValidationHelper.ShowMessageBox(result, Properties.MergedResources.Common_Message);
                            this.Clear();
                            this.keys.Clear();
                            this.crit.StartIndex = 0;
                            this.LoadKeys();
                            IsAllChecked = false;
                            CanEdit = false;
                            IsBusy = false;
                        });
                    }
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }

            });
        }

        /// <summary>
        /// delete cbrs optional info
        /// </summary>
        private void DeleteOptionInfo()
        {
            this.Clear();
            this.SaveOptionalInfo();
        }

        private void LoadKeys()
        {
            List<KeyInfoModel> newKeys = keyProxy.SearchBoundKeysToReport(crit).ToKeyInfoModel();
            foreach (var key in newKeys)
            {
                if (!this.keys.Contains(key))
                    keys.Add(key);
            }
        }

        private void LoadZfactorCl1s()
        {
            List<string> lists = new List<string>();
            lists.Add(string.Empty);
            OHRData.ZFRM_FACTORValue.Select(k => k.Key).ToList().ForEach(k => { lists.Add(k.ToString()); });
            this.zfrm_factor_cl1s = lists;
        }

        private void LoadZfactorCl2s(string factor1value)
        {
            List<string> lists = new List<string>();
            lists.Add(string.Empty);
            if (!string.IsNullOrEmpty(factor1value))
                OHRData.ZFRM_FACTORValue.Where(k => k.Key.ToString() == factor1value).FirstOrDefault().Value.ForEach(k => { lists.Add(k.ToString()); });
            this.zfrm_factor_cl2s = lists;
        }

        private void LoadZtouchScerrens()
        {
            List<string> lists = new List<string>();
            lists.Add(string.Empty);
            OHRData.ZTOUCH_SCREENValue.ForEach(k => lists.Add(EnumHelper.GetFieldDecription(typeof(TouchEnum), k)));
            this.ztouch_screens = lists;
        }

        private void Clear()
        {
            this.ZCHANNEL_REL_ID = null;
            this.ZMAUF_GEO_LOC = null;
            this.ZOEM_EXT_ID = null;
            this.ZPC_MODEL_SKU = null;
            this.ZPGM_ELIG_VALUES = null;

            this.SelectedZFRM_FACTOR_CL1 = string.Empty;
            this.SelectedZFRM_FACTOR_CL2 = string.Empty;
            this.ZSCREEN_SIZE = null;
            this.SelectedZTOUCH_SCREEN = string.Empty;
        }

        private bool ValidateOptionalInfo()
        {
            string error = null;
            if (!string.IsNullOrEmpty(this.ZPC_MODEL_SKU) && System.Text.UTF8Encoding.UTF8.GetBytes(this.ZPC_MODEL_SKU).Length > 64)
                error += MergedResources.EditOptionalInfoViewModel_PcModelSkuError;
            if (!string.IsNullOrEmpty(this.ZPGM_ELIG_VALUES) && System.Text.UTF8Encoding.UTF8.GetBytes(this.ZPGM_ELIG_VALUES).Length > 48)
                error += MergedResources.EditOptionalInfoViewModel_ProEligValueError;

            if (!string.IsNullOrEmpty(this.ZSCREEN_SIZE))
            {
                string DecimalRegEx = @"^[0-9]+(\.[0-9]+)?$";
                bool bl = System.Text.RegularExpressions.Regex.IsMatch(this.ZSCREEN_SIZE, DecimalRegEx);
                if (!bl)
                    error += ResourcesOfRTMv1_6.ScreenValueFormatMsg;
            }
            if (error != null)
            {
                ValidationHelper.ShowMessageBox(error, Properties.MergedResources.Common_Error);
                return false;
            }
            if (Keys.Where(k => k.IsSelected).Any(k => k.keyInfo.OemOptionalInfo.HasOHRData))
            {
                MessageBoxResult confirm = System.Windows.MessageBox.Show(ResourcesOfRTMv1_6.EditOptionalInfo_OverWriteOHRDataMsg, MergedResources.Common_Confirmation, MessageBoxButton.OKCancel);
                if (confirm != MessageBoxResult.OK)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
