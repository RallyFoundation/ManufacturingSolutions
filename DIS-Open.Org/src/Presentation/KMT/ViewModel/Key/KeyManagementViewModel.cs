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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DIS.Common.Utility;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.Views.Key.DuplicateKeysView;
using DIS.Presentation.KMT.Models;
using System.Threading.Tasks;
using DIS.Presentation.KMT.Behaviors;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class KeyManagementViewModel : ViewModelBase
    {
        #region Private Members

        private IKeyProxy keyProxy;
        private IConfigProxy configProxy;
        private ISubsidiaryProxy ssProxy;
        private IHeadQuarterProxy hqProxy;

        private ObservableCollection<KeyInfoModel> keyInfoCollection;
        private ObservableCollection<Subsidiary> subsidiarys = null;
        private Subsidiary selectedSubsidiary = null;
        private Dictionary<DateTime, string> keyHistory = null;
        private KeyInfoModel keySelected;

        private ObservableCollection<string> keyStates = null;
        private ObservableCollection<string> keyTypes = null;
        private ObservableCollection<string> returnStates = null;
        private string selectedKeyState;
        private string selectedKeyType;
        private string selectReturnState;
        private string orderNumber = string.Empty;
        private string referenceNumber = string.Empty;
        private string msPartNumber = string.Empty;
        private string oemPartNumber = string.Empty;
        private string oemPoNumber = string.Empty;
        //private string productKeyID = string.Empty;
        private string productKeyIDFrom = null;
        private string productKeyIDTo = null;
        private string productKey = string.Empty;
        private string zpc_model_sku;
        private string zoem_ext_id = string.Empty;
        private string zmauf_geo_loc = string.Empty;
        private string zpgm_elig_values = string.Empty;
        private string zchannel_rel_id = string.Empty;
        //add for V1.6
        private List<string> zfrm_factor_cl1s;
        private List<string> zfrm_factor_cl2s;
        private string selectedzfrm_factor_cl1;
        private string selectedzfrm_factor_cl2;
        private string zscreen_size;
        private List<string> ztouch_screens;
        private string selectedztouch_screen;

        private string trakingInfo = string.Empty;
        private DateTime? startChangeStateDate = null;
        private DateTime? endChangeStateDate = null;
        private DateTime? startOemRMADate;
        private DateTime? endOemRMADate;
        private string hardwareHash = string.Empty;
        private string oemRmaNumber = string.Empty;
        private DelegateCommand searchCommand = null;

        public Dictionary<string, List<string>> keyStatus = null;

        private int currentPage = 1;
        private int pageSize = KmtConstants.DefaultPageSize;
        private int pageCount = 0;
        private int totalCount = 0;
        private DelegateCommand pageChangeCommand = null;

        private int currentPageHistory = 1;
        private bool isHistoryPaging;
        private int pageSizeHistory = KmtConstants.DefaultPageSize;
        private int pageCountHistory = 0;
        private DelegateCommand pageChangeHistoryCommand = null;

        private bool isBusy;

        private bool isDesc = false;
        private string sortColumn = string.Empty;

        private List<KeyInfo> selectedkeyInfoCollection
        {
            get { return keyInfoCollection.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList(); }
        }
        private DelegateCommand duplicateCommand;
        private string duplicateTxt;
        private string duplicatCBRTxt;
        private bool hasDuplicateKeys;
        private bool hasDuplicatCBRs;
        private Dictionary<string, string> keysDetailsValueCollection;
        private DelegateCommand clearCommand;
        private Visibility returnSelectVisibility = Visibility.Collapsed;

        //private DelegateCommand keySelectionChangedCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyProxy"></param>
        /// <param name="configProxy"></param>
        /// <param name="ssProxy"></param>
        /// <param name="hqProxy"></param>
        public KeyManagementViewModel(IKeyProxy keyProxy, IConfigProxy configProxy,
            ISubsidiaryProxy ssProxy, IHeadQuarterProxy hqProxy)
        {
            this.keyProxy = keyProxy;
            this.configProxy = configProxy;
            this.ssProxy = ssProxy;
            this.hqProxy = hqProxy;

            WorkInBackground((s, e) =>
            {
                LoadKeyStatesList();
                LoadSubSidiary();
                LoadKeyTypesList();
                LoadKeyRetuernState();
                LoadZfactorCl1s();
                LoadZfactorCl2s(null);
                this.selectedzfrm_factor_cl2 = this.zfrm_factor_cl2s.FirstOrDefault();
                LoadZtouchScerrens();
            });
        }

        #endregion

        #region Binding Properties

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
            set
            {
                totalCount = value;
                RaisePropertyChanged("TotalCount");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasDuplicatCBRs
        {
            get
            {
                return hasDuplicatCBRs;
            }
            set
            {
                hasDuplicatCBRs = value;
                RaisePropertyChanged("HasDuplicatCBRs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> KeysDetailsValueCollection
        {
            get
            {
                return keysDetailsValueCollection;
            }
            set
            {
                keysDetailsValueCollection = value;
                RaisePropertyChanged("KeysDetailsValueCollection");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HardWareHash
        {
            get
            {
                return hardwareHash;
            }
            set
            {
                hardwareHash = value.Trim();
                RaisePropertyChanged("HardWareHash");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasDuplicateKeys
        {
            get
            {
                return hasDuplicateKeys;
            }
            set
            {
                hasDuplicateKeys = value;
                RaisePropertyChanged("HasDuplicateKeys");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DuplicateTxt
        {
            get
            {
                return duplicateTxt;
            }
            set
            {
                duplicateTxt = value;
                RaisePropertyChanged("DuplicateTxt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DuplicateCBRTxt
        {
            get
            {
                return duplicatCBRTxt;
            }
            set
            {
                duplicatCBRTxt = value;
                RaisePropertyChanged("DuplicateCBRTxt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsManager
        {
            get
            {
                return KmtConstants.IsManager;
            }

        }

        /// <summary>
        /// Current Page Number for Key-List
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                RaisePropertyChanged("CurrentPage");
            }
        }

        /// <summary>
        /// Page Size for Key-List
        /// </summary>
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value;
                RaisePropertyChanged("PageSize");
            }
        }

        /// <summary>
        /// List of Subsidiarys
        /// </summary>
        public ObservableCollection<Subsidiary> Subsidiarys
        {
            get
            {
                return subsidiarys;
            }
            set
            {
                subsidiarys = value;
                RaisePropertyChanged("Subsidiarys");
            }
        }

        /// <summary>
        /// Subsidiary details on window loading
        /// </summary>
        public Subsidiary SelectedSubSidiary
        {
            get
            {
                return selectedSubsidiary;
            }
            set
            {
                selectedSubsidiary = value;
                RaisePropertyChanged("SelectedSubSidiary");
            }
        }

        /// <summary>
        /// Page Count For Key-List
        /// </summary>
        public int PageCount
        {
            get
            {
                return pageCount == 0 ? 1 : pageCount;
            }
            set
            {
                pageCount = value;
                RaisePropertyChanged("PageCount");
            }
        }

        /// <summary>
        /// Current Page Number for Key-History
        /// </summary>
        public int CurrentPageHistory
        {
            get
            {
                return currentPageHistory;
            }
            set
            {
                currentPageHistory = value;
                RaisePropertyChanged("CurrentPageHistory");
            }
        }

        /// <summary>
        /// Page Size for Key-History
        /// </summary>
        public int PageSizeHistory
        {
            get
            {
                return pageSizeHistory;
            }
            set
            {
                pageSizeHistory = value;
                RaisePropertyChanged("PageSizeHistory");
            }
        }

        /// <summary>
        /// Page Count for Key-History
        /// </summary>
        public int PageCountHistory
        {
            get
            {
                return pageCountHistory == 0 ? 1 : pageCountHistory;
            }
            set
            {
                pageCountHistory = value;
                RaisePropertyChanged("PageCountHistory");
            }
        }

        /// <summary>
        /// Pages Keys to Bind to Grid
        /// </summary>        
        public ObservableCollection<KeyInfoModel> KeyInfoCollection
        {
            get
            {
                return keyInfoCollection;
            }
            set
            {
                keyInfoCollection = value;
                RaisePropertyChanged("KeyInfoCollection");
            }
        }

        /// <summary>
        /// Order Types to create an order
        /// </summary>
        public ObservableCollection<string> KeyStatus
        {
            get
            {
                return keyStates;
            }
            set
            {
                keyStates = value;
                RaisePropertyChanged("KeyStatus");
            }
        }

        public ObservableCollection<string> KeyTypes
        {
            get
            {
                return keyTypes;
            }
            set
            {
                keyTypes = value;
                RaisePropertyChanged("KeyTypes");
            }
        }

        public ObservableCollection<string> ReturnStates
        {
            get
            {
                return returnStates;
            }
            set
            {
                returnStates = value;
                RaisePropertyChanged("ReturnStates");
            }
        }

        /// <summary>
        /// Visiablity to OEM and TPI
        /// </summary>
        public bool IsOEMandTPIVisible
        {
            get
            {
                if (!KmtConstants.IsFactoryFloor)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Key Status History for selected Kye in Main Keys Grid
        /// </summary>
        public Dictionary<DateTime, string> KeyHistoryInformation
        {
            get
            {
                return keyHistory;
            }
            set
            {
                keyHistory = value;
                RaisePropertyChanged("KeyHistoryInformation");
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

        public Visibility ReturnSelectVisibility
        {
            get
            {
                return returnSelectVisibility;
            }
            set
            {
                returnSelectVisibility = value;
                RaisePropertyChanged("ReturnSelectVisibility");
            }
        }

        /// <summary>
        /// Selected Key State
        /// </summary>
        public string SelectedKeyState
        {
            get
            {
                return selectedKeyState;
            }
            set
            {
                selectedKeyState = value;
                RaisePropertyChanged("SelectedKeyState");
                if (SelectedKeyState == KeyState.Returned.ToString())
                    ReturnSelectVisibility = Visibility.Visible;
                else
                {
                    ReturnSelectVisibility = Visibility.Collapsed;
                    if (ReturnStates != null)
                        SelectReturnState = ReturnStates.FirstOrDefault().ToString();
                }
            }
        }

        public string SelectedKeyType
        {
            get
            {
                return selectedKeyType;
            }
            set
            {
                selectedKeyType = value;
                RaisePropertyChanged("SelectedKeyType");
                if (keyStatus != null && keyStatus.Count > 0)
                {
                    if (selectedKeyType == null || selectedKeyType == MergedResources.Common_All)
                        KeyStatus = new ObservableCollection<string>(keyStatus.Where(k => k.Key == KeyType.Standard.ToString()).FirstOrDefault().Value);
                    else
                        KeyStatus = new ObservableCollection<string>(keyStatus.Where(k => k.Key == selectedKeyType).FirstOrDefault().Value);
                    this.SelectedKeyState = KeyStatus.FirstOrDefault();
                    RaisePropertyChanged("KeyStatus");
                    RaisePropertyChanged("SelectedKeyState");
                }
            }

        }

        public string SelectReturnState
        {
            get
            {
                return selectReturnState;
            }
            set
            {
                selectReturnState = value;
                RaisePropertyChanged("SelectReturnState");
                if (selectReturnState == KeyState.Returned.ToString())
                {
                    ReturnSelectVisibility = Visibility.Visible;
                }
            }

        }

        /// <summary>
        /// Gets the Selectem Item Event for KeyHistory of the Selected Row 
        /// </summary>
        /// For Command based selected  Changed Event
        public KeyInfoModel KeySelected
        {
            get
            {
                return keySelected;
            }
            set
            {
                keySelected = value;
                //if (isHistoryPaging)
                //    this.currentPageHistory = 1;
                //if (value != null)
                //{
                //    GetKeyHistoryById(keySelected.keyInfo);
                //    GetKeyDetails(keySelected);
                //}
                RaisePropertyChanged("KeySelected");
            }
        }

        /// <summary>
        /// Search Criteria:Order Number
        /// </summary>
        public string OrderNumber
        {
            get
            {
                return orderNumber;
            }
            set
            {
                orderNumber = value.Trim();
                RaisePropertyChanged("OrderNumber");
            }
        }

        /// <summary>
        /// Search Criteria:Referenc Number
        /// </summary>
        public string ReferenceNumber
        {
            get
            {
                return referenceNumber;
            }
            set
            {
                referenceNumber = value.Trim();
                RaisePropertyChanged("ReferenceNumber");
            }
        }

        /// <summary>
        /// Search Criteria:Ms Part Number
        /// </summary>
        public string MsPartNumber
        {
            get
            {
                return msPartNumber;
            }
            set
            {
                msPartNumber = value.Trim();
                RaisePropertyChanged("MsPartNumber");
            }
        }

        /// <summary>
        /// Search Criteria:OEM Part Number
        /// </summary>
        public string OEMPartNumber
        {
            get
            {
                return oemPartNumber;
            }
            set
            {
                oemPartNumber = value.Trim();
                RaisePropertyChanged("OEMPartNumber");
            }
        }

        /// <summary>
        /// Search Criteria:OEM Po Number
        /// </summary>
        public string OEMPoNumber
        {
            get
            {
                return oemPoNumber;
            }
            set
            {
                oemPoNumber = value.Trim();
                RaisePropertyChanged("OEMPoNumber");
            }
        }

        /// <summary>
        /// Search Criteria:Start Key Chang State Date
        /// </summary>
        public DateTime? StartChangeStateDate
        {
            get
            {
                return startChangeStateDate;
            }
            set
            {
                startChangeStateDate = value;
                RaisePropertyChanged("StartChangeStateDate");
            }
        }

        /// <summary>
        /// Search Criteria:End Key Chang State Date
        /// </summary>
        public DateTime? EndChangeStateDate
        {
            get
            {
                return endChangeStateDate;
            }
            set
            {
                endChangeStateDate = value;
                RaisePropertyChanged("EndChangeStateDate");
            }
        }

        /// <summary>
        /// Search Criteria:Start OemRMADate
        /// </summary>
        public DateTime? StartOemRMADate
        {
            get
            {
                return startOemRMADate;
            }
            set
            {
                startOemRMADate = value;
                RaisePropertyChanged("StartOemRMADate");
            }
        }

        /// <summary>
        /// Search Criteria:End OemRMADate
        /// </summary>
        public DateTime? EndOemRMADate
        {
            get
            {
                return endOemRMADate;
            }
            set
            {
                endOemRMADate = value;
                RaisePropertyChanged("EndOemRMADate");
            }
        }

        /// <summary>
        /// Search Criteria:Product Key ID
        /// </summary>
        //public string ProductKeyID
        //{
        //    get
        //    {
        //        return productKeyID;
        //    }
        //    set
        //    {
        //        productKeyID = value.Trim();
        //        RaisePropertyChanged("ProductKeyID");
        //    }
        //}

        public string ProductKeyIDFrom
        {
            get
            {
                return productKeyIDFrom;
            }
            set
            {
                long temp = -1;
                if (long.TryParse(value, out temp) && temp > 0)
                    productKeyIDFrom = value.Trim();
                else
                    productKeyIDFrom = string.Empty;
                RaisePropertyChanged("ProductKeyIDFrom");
            }
        }

        public string ProductKeyIDTo
        {
            get
            {
                return productKeyIDTo;
            }
            set
            {
                long temp = -1;
                if (long.TryParse(value, out temp) && temp > 0)
                    productKeyIDTo = value.Trim();
                else
                    productKeyIDTo = string.Empty;
                RaisePropertyChanged("ProductKeyIDTo");
            }
        }

        /// <summary>
        /// Search Criteria:Product Key
        /// </summary>
        public string ProductKey
        {
            get
            {
                return productKey;
            }
            set
            {
                productKey = value.Trim();
                RaisePropertyChanged("ProductKey");
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
            get
            {
                return this.selectedzfrm_factor_cl1;
            }
            set
            {
                this.selectedzfrm_factor_cl1 = value;
                LoadZfactorCl2s(value);
                RaisePropertyChanged("SelectedZFRM_FACTOR_CL1");
                RaisePropertyChanged("ZFRM_FACTOR_CL2s");
                this.selectedzfrm_factor_cl2 = zfrm_factor_cl2s.FirstOrDefault();
                RaisePropertyChanged("SelectedZFRM_FACTOR_CL2");
            }
        }

        public string SelectedZFRM_FACTOR_CL2
        {
            get
            {
                return this.selectedzfrm_factor_cl2;
            }
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
            get
            {
                return this.selectedztouch_screen;
            }
            set
            {
                this.selectedztouch_screen = value;
                RaisePropertyChanged("SelectedZTOUCH_SCREEN");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TrakingInfo
        {
            get { return this.trakingInfo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.trakingInfo = null;
                else
                    this.trakingInfo = value;
                RaisePropertyChanged("TrakingInfo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OemRmaNumber
        {
            get { return this.oemRmaNumber; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.oemRmaNumber = null;
                else
                    this.oemRmaNumber = value;
                RaisePropertyChanged("OemRmaNumber");
            }
        }


        #endregion

        #region Binding Command Properties

        /// <summary>
        /// Command used on clicking Search button
        /// </summary>
        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    // Enable or Disable Search button based on a bool flag
                    searchCommand = new DelegateCommand
                    (
                        () =>
                        {
                            Search(1);
                        }
                    );
                }
                return searchCommand;
            }
        }

        /// <summary>
        /// Command For Page Chage Event of Pager in Keys 
        /// </summary>
        public ICommand PageChangeCommand
        {
            get
            {
                if (pageChangeCommand == null)
                {
                    pageChangeCommand = new DelegateCommand(CurrentPageChange);
                }
                return pageChangeCommand;
            }
        }

        /// <summary>
        /// Command For Page Chage Event of Pager in Key History
        /// </summary>
        public ICommand PageChangeHistoryCommand
        {
            get
            {
                if (pageChangeHistoryCommand == null)
                {
                    pageChangeHistoryCommand = new DelegateCommand(CurrentPageChangeHistory);
                }
                return pageChangeHistoryCommand;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DuplicateCommand
        {
            get
            {
                if (duplicateCommand == null)
                {
                    duplicateCommand = new DelegateCommand(SearchDuplicateKeys);
                }
                return duplicateCommand;
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

        //public ICommand KeySelectionChangedCommand 
        //{
        //    get 
        //    {
        //        if (this.keySelectionChangedCommand == null)
        //        {
        //            this.keySelectionChangedCommand = new DelegateCommand(() =>
        //            {
                        
        //            });
        //        }

        //        return this.keySelectionChangedCommand;
        //    }
        //}

        #endregion

        #region Public Methods

        /// <summary>
        ///  Current Page Changed Handler For Selected Key's History Audit
        /// </summary>
        public void CurrentPageChangeHistory()
        {
            isHistoryPaging = true;
            //Get Page Changed for Key-History
            GetKeyHistoryById(this.KeySelected.keyInfo);
        }

        /// <summary>
        /// Current Page Changed Handler For Keys
        /// </summary>
        public void CurrentPageChange()
        {
            //Set Key-History PageCount to back to 1 during Key-List Page Change Event
            this.pageCountHistory = 1;

            //Call Search Keys function with new Page Information from Pager control
            Search(CurrentPage);
        }

        /// <summary>
        /// Main Window Button Click Event Handler for Get Keys
        /// </summary>
        public void GetKeys()
        {
            //Background thread for updating Key State to DB
            IsBusy = true;
            DispatcherFrame frame = new DispatcherFrame();
            WorkInBackground((s, e) =>
            {
                string resultMessage = string.Empty;
                switch (Constants.InstallType)
                {
                    case InstallType.Oem:
                        resultMessage = GetKeysByOem();
                        break;
                    case InstallType.Tpi:
                        resultMessage = GetKeysByTpi();
                        break;
                    case InstallType.FactoryFloor:
                        resultMessage = GetKeysByFF();
                        break;
                }
                e.Result = new Message(MergedResources.Common_Message, resultMessage);
                Search(CurrentPage);
                IsBusy = false;
            }, (s, e) =>
            {
                Message msg = (Message)e.Result;
                ValidationHelper.ShowMessageBox(msg.Content, msg.Title);
                frame.Continue = false;
            });
            Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// Refresh KeyManagement DIS.Presentation.KMT
        /// </summary>
        public void Refresh()
        {
            Search(CurrentPage);
        }

        public void KeySelectionChanged()
        {
            if (this.isHistoryPaging) 
            {
                this.currentPageHistory = 1;
            }

            if (this.keySelected != null)
            {
                this.GetKeyHistoryById(this.keySelected.keyInfo);
                this.GetKeyDetails(this.keySelected);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get OrderList data based on OrderStatusId
        /// </summary>
        private void Search(int pageNumber)
        {
            if (ValidationHelper.ValidateDateRange(StartChangeStateDate, EndChangeStateDate))
            {
                IsBusy = true;
                WorkInBackground((s, e) =>
                {
                    try
                    {
                        //Manage Search Criteria 
                        KeySearchCriteria searchCriteria = new KeySearchCriteria();
                        searchCriteria.ShouldIncludeHistories = true;
                        searchCriteria.PageSize = PageSize;
                        searchCriteria.PageNumber = pageNumber;
                        searchCriteria.OemPoNumber = OEMPoNumber;
                        searchCriteria.DateFrom = StartChangeStateDate;
                        searchCriteria.DateTo = EndChangeStateDate;
                        searchCriteria.OemRmaDateFrom = StartOemRMADate;
                        searchCriteria.OemRmaDateTo = EndOemRMADate;
                        searchCriteria.MsOrderNumber = OrderNumber;
                        searchCriteria.MsPartNumber = MsPartNumber;
                        searchCriteria.OemPartNumber = OEMPartNumber;
                        //searchCriteria.ProductKeyID = ProductKeyID;
                        if (!string.IsNullOrEmpty(ProductKeyIDFrom))
                            searchCriteria.ProductKeyIDFrom = long.Parse(ProductKeyIDFrom);
                        if (!string.IsNullOrEmpty(ProductKeyIDTo))
                            searchCriteria.ProductKeyIDTo = long.Parse(ProductKeyIDTo);
                        searchCriteria.ProductKey = ProductKey;
                        searchCriteria.HardwareHash = HardWareHash;
                        searchCriteria.ReferenceNumber = ReferenceNumber;
                        searchCriteria.ZPC_MODEL_SKU = ZPC_MODEL_SKU;
                        searchCriteria.ZCHANNEL_REL_ID = ZCHANNEL_REL_ID;
                        searchCriteria.ZMAUF_GEO_LOC = ZMAUF_GEO_LOC;
                        searchCriteria.ZOEM_EXT_ID = ZOEM_EXT_ID;
                        searchCriteria.ZPGM_ELIG_VALUES = ZPGM_ELIG_VALUES;
                        searchCriteria.ZFRM_FACTOR_CL1 = this.SelectedZFRM_FACTOR_CL1 == "All" ? string.Empty : this.SelectedZFRM_FACTOR_CL1;
                        searchCriteria.ZFRM_FACTOR_CL2 = this.SelectedZFRM_FACTOR_CL2 == "All" ? string.Empty : this.SelectedZFRM_FACTOR_CL2;
                        searchCriteria.ZSCREEN_SIZE = ZSCREEN_SIZE;
                        searchCriteria.ZTOUCH_SCREEN = this.SelectedZTOUCH_SCREEN == "All" ? string.Empty : this.SelectedZTOUCH_SCREEN;
                        searchCriteria.TrakingInfo = TrakingInfo;
                        searchCriteria.ShouldIncludeReturnReport = true;
                        searchCriteria.OemRmaNumber = OemRmaNumber;

                        if (!string.IsNullOrEmpty(sortColumn))
                        {
                            searchCriteria.SortBy = sortColumn;
                            searchCriteria.SortByDesc = isDesc;
                        }
                        if (!string.IsNullOrEmpty(selectedKeyState))
                        {
                            if (selectedKeyState != MergedResources.Common_All)
                                searchCriteria.KeyState = (KeyState)Enum.Parse(typeof(KeyState), selectedKeyState, true);
                        }
                        if (SelectedSubSidiary != null && SelectedSubSidiary.SsId != 0)
                        {
                            searchCriteria.SsId = SelectedSubSidiary.SsId;
                            if (SelectedSubSidiary.SsId == -1)
                            {
                                searchCriteria.SsId = null;
                                searchCriteria.IsAssign = false;
                            }
                        }
                        if (!string.IsNullOrEmpty(SelectedKeyType) && SelectedKeyType != MergedResources.Common_All)
                        {
                            searchCriteria.KeyType = (KeyType)Enum.Parse(typeof(KeyType), SelectedKeyType, true);
                        }
                        if (SelectedKeyType == MergedResources.Common_All)
                            searchCriteria.KeyType = null;

                        if (!string.IsNullOrEmpty(SelectReturnState) && SelectReturnState != ResourcesOfR6.Common_AllReturnedKeys && ReturnSelectVisibility == Visibility.Visible)
                        {
                            if (SelectReturnState == ResourcesOfR6.Common_ReturnedWithNoCredit)
                                searchCriteria.HasNoCredit = true;
                            else
                                searchCriteria.HasNoCredit = false;
                        }

                        //Search keys by search criteria
                        PagedList<KeyInfo> keyCollection = keyProxy.SearchKeys(searchCriteria);
                        if (keyCollection.PageCount > 0 && pageNumber > keyCollection.PageCount)
                        {
                            Dispatch(() =>
                            {
                                Search(keyCollection.PageCount);
                            });
                            return;
                        }
                        else
                        {
                            //Set Page Count from search 
                            CurrentPage = pageNumber;
                            PageCount = keyCollection.PageCount;
                            TotalCount = keyCollection.TotalCount;

                            //Fill key collection
                            KeyInfoCollection = new ObservableCollection<KeyInfoModel>(keyCollection.Select(k => new KeyInfoModel() { keyInfo = k }));
                        }

                        //Notify UI
                        KeysDetailsValueCollection = null;
                        KeyHistoryInformation = null;

                        IsBusy = false;
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        ex.ShowDialog();
                        ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the Details By SelectedKeyId property
        /// </summary>
        /// <param name="keySelected"></param>
        private void GetKeyDetails(KeyInfoModel keySelected)
        {
            if (keySelected != null)
            {
                //KeysDetailsValueCollection = new Dictionary<string, string>();

                this.keysDetailsValueCollection = new Dictionary<string, string>();

                this.keysDetailsValueCollection.Add(MergedResources.Common_ProductKeyID, keySelected.keyInfo.KeyId.ToString());
                this.keysDetailsValueCollection.Add(MergedResources.Common_ColumnProductKey, keySelected.keyInfo.ProductKey);
                this.keysDetailsValueCollection.Add(ResourcesOfR6.Common_KeyType1, keySelected.keyInfo.KeyInfoEx.KeyType.ToString());
                this.keysDetailsValueCollection.Add(MergedResources.Common_LicensablePartNumber, keySelected.keyInfo.LicensablePartNumber);
                this.keysDetailsValueCollection.Add(MergedResources.Common_OEMPartNumber, keySelected.keyInfo.OemPartNumber);
                this.keysDetailsValueCollection.Add(MergedResources.Common_ColumnStatus, keySelected.keyInfo.KeyState.ToString());
                this.keysDetailsValueCollection.Add(MergedResources.Common_ColumnHardwareHash, keySelected.keyInfo.HardwareHash);

                //Add serial number display - Rally Sept. 25, 2014
                this.keysDetailsValueCollection.Add(MergedResources.Common_ColumnHeaderSerialNumber, keySelected.keyInfo.SerialNumber);

                this.keysDetailsValueCollection.Add(MergedResources.Common_OEMPONumber, keySelected.keyInfo.OemPoNumber);
                this.keysDetailsValueCollection.Add(MergedResources.Common_CallOffReferenceNumber, keySelected.keyInfo.CallOffReferenceNumber);
                this.keysDetailsValueCollection.Add(MergedResources.Common_MSOrderNumber, keySelected.keyInfo.MsOrderNumber);
                this.keysDetailsValueCollection.Add(MergedResources.Common_MSFulfilledDate, keySelected.keyInfo.FulfilledDate.ToString());
                this.keysDetailsValueCollection.Add(MergedResources.EditOptionalInfo_PcModelSku, keySelected.keyInfo.ZPC_MODEL_SKU);
                this.keysDetailsValueCollection.Add(MergedResources.EditOptionalInfo_OemExtId, keySelected.keyInfo.ZOEM_EXT_ID);
                this.keysDetailsValueCollection.Add(MergedResources.EditOptionalInfo_ManGeo, keySelected.keyInfo.ZMANUF_GEO_LOC);
                this.keysDetailsValueCollection.Add(MergedResources.EditOptionalInfo_ProEligValue, keySelected.keyInfo.ZPGM_ELIG_VALUES);
                this.keysDetailsValueCollection.Add(MergedResources.EditOptionalInfo_ChaRelId, keySelected.keyInfo.ZCHANNEL_REL_ID);

                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_6.EditOptionalInfo_Factor1Name, keySelected.keyInfo.ZFRM_FACTOR_CL1);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_6.EditOptionalInfo_Factor2Name, keySelected.keyInfo.ZFRM_FACTOR_CL2);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_6.EditOptionalInfo_ScreenSizeName, keySelected.keyInfo.ZSCREEN_SIZE);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_6.EditOptionalInfo_TouchScreenName, TouchScreenEnumHelper.Convert(keySelected.keyInfo.ZTOUCH_SCREEN));

                this.keysDetailsValueCollection.Add(MergedResources.Common_TrackingInfo, keySelected.keyInfo.TrackingInfo);
                this.keysDetailsValueCollection.Add(MergedResources.UserManagement_CreatedDate, keySelected.keyInfo.CreatedDate.ToString());
                this.keysDetailsValueCollection.Add(ResourcesOfR6.ReturnKeysView_OEMRMANumber, keySelected.keyInfo.ReturnReportKeys.Count > 0 ? keySelected.keyInfo.ReturnReportKeys.Last().ReturnReport.OemRmaNumber : "");
                this.keysDetailsValueCollection.Add(ResourcesOfR6.ReturnKeysView_OEMRMADate, keySelected.keyInfo.ReturnReportKeys.Count > 0 ? keySelected.keyInfo.ReturnReportKeys.Last().ReturnReport.OemRmaDate.ToString() : "");
                var returnKeyResult = GetKeyInfoReturnReport(keySelected);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_6.ReturnTypeLabel, returnKeyResult.Item3);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_4.Common_ColumnWithoutCredit, returnKeyResult.Item1);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_4.Common_ColumnReturnReasonCode, returnKeyResult.Item2);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_4.Common_ColumnTags, keySelected.keyInfo.Tags);
                this.keysDetailsValueCollection.Add(ResourcesOfRTMv1_4.Common_ColumnDescription, keySelected.keyInfo.Description);

                this.KeysDetailsValueCollection = this.keysDetailsValueCollection;
            }
        }

        private Tuple<string, string, string> GetKeyInfoReturnReport(KeyInfoModel keySelected)
        {
            var lastReturnReport = keySelected.keyInfo.ReturnReportKeys
                .Select(k => k.ReturnReport)
                .OrderByDescending(r => r.ReturnDateUTC)
                .FirstOrDefault();

            if (lastReturnReport != null)
            {
                var returnkey = keySelected.keyInfo.ReturnReportKeys.Single(k => k.CustomerReturnUniqueId == lastReturnReport.CustomerReturnUniqueId);
                var withoutCredit = lastReturnReport.ReturnNoCredit.ToString();
                if (!string.IsNullOrEmpty(returnkey.ReturnReasonCode))
                    withoutCredit = (!returnkey.ReturnReasonCode.StartsWith("O")).ToString();
                return new Tuple<string, string, string>(withoutCredit, returnkey.ReturnReasonCode, returnkey.ReturnTypeId);
            }

            return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Gets the History By SelectedKeyId property
        /// </summary>
        private void GetKeyHistoryById(KeyInfo keyInfo)
        {
            //If Selected Key Count is exactly ONE, Then Get History
            if (null != keyInfo)
            {
                try
                {
                    //Get the Selected Key 
                    KeyHistoryInformation = GetKeyHistoryDetails(keyInfo);
                }
                catch (Exception ex)
                {
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
            }
        }

        private Dictionary<DateTime, string> GetKeyHistoryDetails(KeyInfo keyInfo)
        {
            Dictionary<DateTime, string> result = new Dictionary<DateTime, string>();

            var keyHistories = keyInfo.KeyHistories.OrderBy(k => k.StateChangeDate).ToList();
            keyHistories.ForEach(p => { result.Add(p.StateChangeDate, ((KeyState)p.KeyStateId).ToString()); });
            return result;
        }

        private void LoadDuplicateKeys()
        {
            var count = keyProxy.GetKeysDuplicated().Count;
            if (count > 0)
            {
                HasDuplicateKeys = true;
                DuplicateTxt = string.Format(Properties.MergedResources.KeyManagementViewModel_DuplicateMessage, count);
            }
            else
            {
                HasDuplicateKeys = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadSubSidiary()
        {
            if (KmtConstants.IsOemCorp || KmtConstants.IsTpiCorp)
            {
                Subsidiarys = new ObservableCollection<Subsidiary>(ssProxy.GetSubsidiaries());
                Subsidiarys.Insert(0, new Subsidiary() { SsId = 0, DisplayName = MergedResources.Common_All });
                Subsidiarys.Insert(1, new Subsidiary() { SsId = -1, DisplayName = ResourcesOfRTMv1_6.NoAssignLabel });
                //Select 'Show All' on First Load
                SelectedSubSidiary = Subsidiarys.FirstOrDefault();
            }
            else// Factory Floor
                Subsidiarys = new ObservableCollection<Subsidiary>();
        }

        private void LoadKeyStatesList()
        {

            List<string> standardKeyStates = new List<string>();
            List<string> MBRKeyStates = new List<string>();
            List<string> MATKeyStates = new List<string>();

            //Get States from Library into KeyStates property
            KeyStatus = new ObservableCollection<string>();
            keyStatus = new Dictionary<string, List<string>>();

            standardKeyStates.Add(MergedResources.Common_All);

            MBRKeyStates.Add(MergedResources.Common_All);

            MATKeyStates.Add(MergedResources.Common_All);

            //show OEM State
            if (KmtConstants.IsOemCorp)
            {
                standardKeyStates.Add(KeyState.Fulfilled.ToString());
                standardKeyStates.Add(KeyState.Assigned.ToString());
                standardKeyStates.Add(KeyState.Retrieved.ToString());
                standardKeyStates.Add(KeyState.Bound.ToString());
                standardKeyStates.Add(KeyState.ReportedBound.ToString());
                standardKeyStates.Add(KeyState.ActivationEnabled.ToString());
                standardKeyStates.Add(KeyState.ActivationDenied.ToString());
                standardKeyStates.Add(KeyState.ReportedReturn.ToString());
                standardKeyStates.Add(KeyState.Returned.ToString());
                standardKeyStates.Add(KeyState.ActivationEnabledPendingUpdate.ToString());

                MBRKeyStates.Add(KeyState.Fulfilled.ToString());
                MBRKeyStates.Add(KeyState.Assigned.ToString());
                MBRKeyStates.Add(KeyState.ActivationEnabled.ToString());
                MBRKeyStates.Add(KeyState.ReportedReturn.ToString());
                MBRKeyStates.Add(KeyState.Returned.ToString());

                MATKeyStates.Add(KeyState.Fulfilled.ToString());
                MATKeyStates.Add(KeyState.Assigned.ToString());
                MATKeyStates.Add(KeyState.ActivationEnabled.ToString());
                MATKeyStates.Add(KeyState.ReportedReturn.ToString());
                MATKeyStates.Add(KeyState.Returned.ToString());

            }
            //show TPI State
            if (KmtConstants.IsTpiCorp)
            {
                standardKeyStates.Add(KeyState.Fulfilled.ToString());
                standardKeyStates.Add(KeyState.Assigned.ToString());
                standardKeyStates.Add(KeyState.Retrieved.ToString());
                standardKeyStates.Add(KeyState.NotifiedBound.ToString());
                standardKeyStates.Add(KeyState.Bound.ToString());
                standardKeyStates.Add(KeyState.ActivationEnabled.ToString());
                standardKeyStates.Add(KeyState.ActivationDenied.ToString());
                standardKeyStates.Add(KeyState.Returned.ToString());
                if (!KmtConstants.CurrentHeadQuarter.IsCentralizedMode)
                {
                    standardKeyStates.Add(KeyState.ReportedBound.ToString());
                    standardKeyStates.Add(KeyState.ReportedReturn.ToString());
                    standardKeyStates.Add(KeyState.ActivationEnabledPendingUpdate.ToString());
                }

                MBRKeyStates.Add(KeyState.Fulfilled.ToString());
                MBRKeyStates.Add(KeyState.Assigned.ToString());
                MBRKeyStates.Add(KeyState.ActivationEnabled.ToString());
                MBRKeyStates.Add(KeyState.ReportedReturn.ToString());
                MBRKeyStates.Add(KeyState.Returned.ToString());

                MATKeyStates.Add(KeyState.Fulfilled.ToString());
                MATKeyStates.Add(KeyState.Assigned.ToString());
                MATKeyStates.Add(KeyState.ActivationEnabled.ToString());
                MATKeyStates.Add(KeyState.ReportedReturn.ToString());
                MATKeyStates.Add(KeyState.Returned.ToString());
            }
            //show FF State
            if (KmtConstants.IsFactoryFloor)
            {
                standardKeyStates.Add(KeyState.Fulfilled.ToString());
                standardKeyStates.Add(KeyState.Consumed.ToString());
                standardKeyStates.Add(KeyState.NotifiedBound.ToString());
                standardKeyStates.Add(KeyState.Bound.ToString());
                standardKeyStates.Add(KeyState.ActivationEnabled.ToString());
                standardKeyStates.Add(KeyState.ActivationDenied.ToString());
                standardKeyStates.Add(KeyState.Returned.ToString());

                MBRKeyStates.Add(KeyState.Fulfilled.ToString());
                MBRKeyStates.Add(KeyState.ActivationEnabled.ToString());
                MBRKeyStates.Add(KeyState.Returned.ToString());

                MATKeyStates.Add(KeyState.Fulfilled.ToString());
                MATKeyStates.Add(KeyState.Returned.ToString());
            }

            keyStatus.Add(KeyType.Standard.ToString(), standardKeyStates);
            keyStatus.Add(KeyType.MBR.ToString(), MBRKeyStates);
            keyStatus.Add(KeyType.MAT.ToString(), MATKeyStates);
            if (selectedKeyType == null || selectedKeyType == MergedResources.Common_All)
            {
                SelectedKeyState = MergedResources.Common_All;
                KeyStatus = new ObservableCollection<string>(keyStatus.Where(k => k.Key == KeyType.Standard.ToString()).FirstOrDefault().Value);
            }
        }

        private void LoadKeyRetuernState()
        {
            ReturnStates = new ObservableCollection<string>();
            ReturnStates.Add(ResourcesOfR6.Common_AllReturnedKeys);
            ReturnStates.Add(ResourcesOfR6.Common_ReturnedWithCredit);
            ReturnStates.Add(ResourcesOfR6.Common_ReturnedWithNoCredit);
            SelectReturnState = ReturnStates.First();
        }

        private void LoadKeyTypesList()
        {
            keyTypes = new ObservableCollection<string>();
            keyTypes.Add(MergedResources.Common_All);
            keyTypes.Add(KeyType.Standard.ToString());
            keyTypes.Add(KeyType.MBR.ToString());
            keyTypes.Add(KeyType.MAT.ToString());
            selectedKeyType = keyTypes.First();
        }

        private void SearchDuplicateKeys()
        {
            DuplicateKeys window = new DuplicateKeys(keyProxy, ssProxy);
            window.Owner = View;
            window.ShowDialog();
            if (window.DialogResult.HasValue && window.DialogResult.HasValue)
                LoadDuplicateKeys();
        }

        internal void SortingByColumn(string sortColumn)
        {
            this.sortColumn = sortColumn;
            isDesc = !isDesc;
            keyInfoCollection = null;
            Search(CurrentPage);
        }

        private string GetKeysByOem()
        {
            return FulfillmentMessage(FulfillKeysFromMS());
        }

        private string GetKeysByTpi()
        {
            if (KmtConstants.CurrentHeadQuarter.IsCentralizedMode)
                return GetKeysMessage(GetKeysFromULS());
            else
                return FulfillmentMessage(FulfillKeysFromMS());
        }

        private string GetKeysByFF()
        {
            return GetKeysMessage(GetKeysFromULS());
        }

        private string FulfillmentMessage(int fulfillResult)
        {
            string message = string.Empty;
            string fulfillmentSucc = string.Format(MergedResources.KeyManagementViewModel_FulfillmentSucc, fulfillResult);
            string fulfillmentFailed = string.Format(ResourcesOfR6.KeyManagementViewModel_FulfillmentFail);
            string fulfillmentNoKeys = string.Format(MergedResources.KeyManagementViewModel_FulfillmentNoKeys);
            string fulfillmentDisableMsService = string.Format(MergedResources.KeyManagementViewModel_FulfillmentDisableMsService);

            switch (fulfillResult)
            {
                case (int)GetKeyResult.DisableMsService:
                    if (KmtConstants.IsOemCorp)
                        message = fulfillmentDisableMsService;
                    break;
                case (int)GetKeyResult.NoQuanity:
                    message = fulfillmentNoKeys;
                    break;
                case (int)GetKeyResult.Error:
                    message = fulfillmentFailed;
                    break;
                default:
                    message = fulfillmentSucc;
                    break;
            }
            return message;
        }

        private string GetKeysMessage(int getKeysResult)
        {
            string message = string.Empty;
            string getKeysSucc = string.Format(MergedResources.KeyManagementViewModel_GetKeysSucc, getKeysResult, KmtConstants.CurrentHeadQuarter.DisplayName);
            string getKeysFailed = string.Format(MergedResources.KeyManagementViewModel_GetKeysFail, KmtConstants.CurrentHeadQuarter.DisplayName);
            string getKeysNoKeys = string.Format(MergedResources.KeyManagementViewModel_GetKeysNoKeys, KmtConstants.CurrentHeadQuarter.DisplayName);
            string getKeysNoHeadQuarter = string.Format(MergedResources.KeyManagementViewModel_GetKeyNoHeadQuarter);

            switch (getKeysResult)
            {
                case (int)GetKeyResult.NoHeadQuarter:
                    message = getKeysNoHeadQuarter;
                    break;
                case (int)GetKeyResult.NoQuanity:
                    message = getKeysNoKeys;
                    break;
                case (int)GetKeyResult.Error:
                    message = getKeysFailed;
                    break;
                case (int)GetKeyResult.DisableGetKeys:
                    break;
                default:
                    message = getKeysSucc;
                    break;
            }
            return message;
        }

        private int GetKeysFromULS()
        {
            int result = 0;
            try
            {
                if (KmtConstants.CurrentHeadQuarter != null)
                {
                    if (KmtConstants.CurrentHeadQuarter.IsCentralizedMode)
                    {
                        result = keyProxy.GetKeysFromUls();
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                                    string.Format("Get Keys was been called."), KmtConstants.CurrentDBConnectionString);
                    }
                    else
                        result = (int)GetKeyResult.DisableGetKeys;
                }
                else
                    result = (int)GetKeyResult.NoHeadQuarter;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                result = (int)GetKeyResult.Error;
            }
            return result;
        }

        private int FulfillKeysFromMS()
        {
            int result = 0;
            try
            {
                if (configProxy.GetIsMsServiceEnabled())
                {
                    result += keyProxy.FulfillOrder();
                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                        string.Format("Fulfillment Keys was been called."), KmtConstants.CurrentDBConnectionString);
                }
                else
                    result = (int)GetKeyResult.DisableMsService;
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                result = (int)GetKeyResult.Error;
            }
            return result;
        }

        internal enum GetKeyResult
        {
            NoQuanity = 0,
            Error = -1,
            NoHeadQuarter = -2,
            DisableMsService = -3,
            DisableGetKeys = -4,
        }

        private void LoadZfactorCl1s()
        {
            List<string> lists = new List<string>();
            lists.Add("All");
            OHRData.ZFRM_FACTORValue.Select(k => k.Key).ToList().ForEach(k => { lists.Add(k.ToString()); });
            this.zfrm_factor_cl1s = lists;
            this.selectedzfrm_factor_cl1 = lists.FirstOrDefault();
        }

        private void LoadZfactorCl2s(string factor1value)
        {
            List<string> lists = new List<string>();
            lists.Add("All");
            if (!string.IsNullOrEmpty(factor1value) && factor1value != "All")
                OHRData.ZFRM_FACTORValue.Where(k => k.Key.ToString() == factor1value).FirstOrDefault().Value.ForEach(k => { lists.Add(k.ToString()); });
            this.zfrm_factor_cl2s = lists;
        }

        private void LoadZtouchScerrens()
        {
            List<string> lists = new List<string>();
            lists.Add("All");
            OHRData.ZTOUCH_SCREENValue.ForEach(k => lists.Add(EnumHelper.GetFieldDecription(typeof(TouchEnum), k)));
            this.ztouch_screens = lists;
            this.selectedztouch_screen = lists.FirstOrDefault();
        }

        private void Clear()
        {
            OrderNumber = string.Empty;
            ReferenceNumber = string.Empty;
            MsPartNumber = string.Empty;
            OEMPartNumber = string.Empty;
            OEMPoNumber = string.Empty;
            //ProductKeyID = string.Empty;
            ProductKeyIDFrom = string.Empty;
            ProductKeyIDTo = string.Empty;
            ProductKey = string.Empty;
            ZCHANNEL_REL_ID = string.Empty;
            ZMAUF_GEO_LOC = string.Empty;
            ZOEM_EXT_ID = string.Empty;
            ZPC_MODEL_SKU = string.Empty;
            ZPGM_ELIG_VALUES = string.Empty;
            SelectedZFRM_FACTOR_CL1 = ZFRM_FACTOR_CL1s.FirstOrDefault();
            SelectedZFRM_FACTOR_CL2 = ZFRM_FACTOR_CL2s.FirstOrDefault();
            ZSCREEN_SIZE = string.Empty;
            SelectedZTOUCH_SCREEN = ZTOUCH_SCREENs.FirstOrDefault();
            HardWareHash = string.Empty;
            TrakingInfo = string.Empty;
            SelectedKeyState = KeyStatus.FirstOrDefault();
            SelectedSubSidiary = Subsidiarys.FirstOrDefault();
            StartChangeStateDate = null;
            EndChangeStateDate = null;
            this.SelectedKeyType = KeyTypes.FirstOrDefault().ToString();
            this.SelectReturnState = ReturnStates.FirstOrDefault().ToString();
            this.OemRmaNumber = string.Empty;
            this.StartOemRMADate = null;
            this.EndOemRMADate = null;
        }
        #endregion
    }
}
