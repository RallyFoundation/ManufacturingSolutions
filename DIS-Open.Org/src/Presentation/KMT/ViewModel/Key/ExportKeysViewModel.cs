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
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.ExportKeysView;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.ViewModel.ControlsViewModel;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;

namespace DIS.Presentation.KMT.ViewModel
{
    public sealed class ExportKeysViewModel : TemplateViewModelBase
    {
        #region Private fields

        private const string defaultExportFolderName = "ExportKey Files";
        private const string defaultOA3ToolFolderName = "keyShare";
        private string defaultFileName = string.Format("{0:yyyyMMddhhmmss}.xml", DateTime.Now);
        private string targetFileName = string.Empty;
        private const string strBackSpace = "\b";
        private const string strZero = "0";
        private bool isRequireOHRData;

        private string fileLabelTxt;
        private Constants.ExportType exportType;
        private IConfigProxy configProxy;
        private ISubsidiaryProxy subProxy;
        private IHeadQuarterProxy headquarterProxy;
        public ReturnKeysListViewModel ReturnKeysListModelVM = null;

        #region search condition

        private string fileName;
        private string keySelectTit;
        private DateTime? beginExportDate;
        private DateTime? endExportDate;
        private string searchLogExportTo;
        private string searchLogFileName;
        private bool isDescLog = false;
        private string sortColumnLog = string.Empty;

        #endregion


        #region export type select

        private bool isKeysChecked;
        private bool isReKeysChecked;
        private bool isKeysReportChecked;
        private bool isReKeysReportChecked;

        private bool isToolKeyChecked;
        private bool isReToolKeyChecked;
        private bool isCBRChecked;
        private bool isReCBRChecked;

        private bool isDuplicateCBRChecked;
        private bool isReturnKeyChecked;
        private bool isReReturnKeyChecked;

        //Fix for Bug#126 - Rally, Dec. 16, 2014
        private bool isInCompatibleMode;

        #endregion

        #region UI element visibility and  enable

        private bool rbKeyVisibility;
        private bool rbReportVisibility;
        private bool rbToolKeyVisibility;
        private bool rbCBRVisibility;
        private bool rbDuplicateCBRVisibility;

        private bool isDlSEnable;
        private bool isUlSEnable;
        private bool isMSEnable;

        private bool subsidiaryVisibility;
        private bool encryptCheckedVisibility;
        private bool headQuarterVisibility;
        private bool reNoteVisibility;
        private bool isEncryptChecked;

        #endregion

        #region Data Collection

        private ObservableCollection<Subsidiary> subsidiarys = null;
        private Subsidiary selectedSubsidiary = null;
        private ObservableCollection<HeadQuarter> headQuarters = null;
        private HeadQuarter selectedHeadQuarter = null;
        private ObservableCollection<KeyExportLogModel> keyLogCollection = null;
        private KeyExportLogModel keyLogSelected;
        private List<Cbr> allCBR = null;
        private ObservableCollection<CbrKey> cBRCollention = null;

        #endregion

        #region icommand

        private ICommand browseCommand;
        private ICommand logSearchCommand;
        private DelegateCommand viewCommand;
        private DelegateCommand nextCommand;

        #endregion

        #endregion

        #region Binding properties

        /// re-export log Is encrypted column display
        public event EventHandler ExportTypeChanged;

        public bool IsKeysChecked
        {
            get { return isKeysChecked; }
            set
            {
                isKeysChecked = value;
                if (isKeysChecked)
                    this.exportType = Constants.ExportType.FulfilledKeys;
                RaisePropertyChanged("IsKeysChecked");
            }
        }

        public bool IsReKeysChecked
        {
            get { return isReKeysChecked; }
            set
            {
                isReKeysChecked = value;
                if (isReKeysChecked)
                    this.exportType = Constants.ExportType.ReFulfilledKeys;
                if (ExportTypeChanged != null)
                    ExportTypeChanged(this, new EventArgs());
                RaisePropertyChanged("IsReKeysChecked");
            }
        }

        public bool IsKeysReportChecked
        {
            get { return isKeysReportChecked; }
            set
            {
                isKeysReportChecked = value;
                if (isKeysReportChecked)
                    this.exportType = Constants.ExportType.ReportKeys;
                RaisePropertyChanged("IsKeysReportChecked");
            }
        }

        public bool IsReKeysReportChecked
        {
            get { return isReKeysReportChecked; }
            set
            {
                isReKeysReportChecked = value;
                if (isReKeysReportChecked)
                    this.exportType = Constants.ExportType.ReReportKeys;
                if (ExportTypeChanged != null)
                    ExportTypeChanged(this, new EventArgs());
                RaisePropertyChanged("IsReKeysReportChecked");
            }
        }

        public bool IsToolKeyChecked
        {
            get { return isToolKeyChecked; }
            set
            {
                isToolKeyChecked = value;
                if (isToolKeyChecked)
                    this.exportType = Constants.ExportType.ToolKeys;
                RaisePropertyChanged("IsToolKeyChecked");
            }
        }

        public bool IsReToolKeyChecked
        {
            get { return isReToolKeyChecked; }
            set
            {
                isReToolKeyChecked = value;
                if (isReToolKeyChecked)
                    this.exportType = Constants.ExportType.ReToolKeys;
                if (ExportTypeChanged != null)
                    ExportTypeChanged(this, new EventArgs());
                RaisePropertyChanged("IsReToolKeyChecked");
            }
        }

        public bool IsCBRChecked
        {
            get { return isCBRChecked; }
            set
            {
                isCBRChecked = value;
                if (isCBRChecked)
                    this.exportType = Constants.ExportType.CBR;
                RaisePropertyChanged("IsCBRChecked");
            }
        }

        public bool IsReCBRChecked
        {
            get { return isReCBRChecked; }
            set
            {
                isReCBRChecked = value;
                if (isReCBRChecked)
                    this.exportType = Constants.ExportType.ReCBR;
                if (ExportTypeChanged != null)
                    ExportTypeChanged(this, new EventArgs());
                RaisePropertyChanged("IsReCBRChecked");
            }
        }

        public bool IsDuplicateCBRChecked
        {
            get { return isDuplicateCBRChecked; }
            set
            {
                isDuplicateCBRChecked = value;
                if (isDuplicateCBRChecked)
                    this.exportType = Constants.ExportType.DuplicateCBR;
                RaisePropertyChanged("IsDuplicateCBRChecked");
            }
        }

        public bool IsReturnKeyChecked
        {
            get { return isReturnKeyChecked; }
            set
            {
                isReturnKeyChecked = value;
                if (isReturnKeyChecked)
                    this.exportType = Constants.ExportType.ReturnKeys;
                RaisePropertyChanged("IsReturnKeyChecked");
            }
        }

        public bool IsReReturnKeyChecked
        {
            get { return isReReturnKeyChecked; }
            set
            {
                isReReturnKeyChecked = value;
                if (isReReturnKeyChecked)
                    this.exportType = Constants.ExportType.ReReturnKeys;
                RaisePropertyChanged("IsReReturnKeyChecked");
            }
        }

        public bool IsDlSEnable
        {
            get { return isDlSEnable; }
            set
            {
                isDlSEnable = value;
                RaisePropertyChanged("IsDlSEnable");
            }
        }

        public bool IsUlSEnable
        {
            get { return isUlSEnable; }
            set
            {
                isUlSEnable = value;
                RaisePropertyChanged("IsUlSEnable");
            }
        }

        public bool IsMSEnable
        {
            get { return isMSEnable; }
            set
            {
                isMSEnable = value;
                RaisePropertyChanged("IsMSEnable");
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        public bool IsInCompatibleMode 
        {
            get { return this.isInCompatibleMode; }
            set 
            {
                this.isInCompatibleMode = value;
                this.RaisePropertyChanged("IsInCompatibleMode");
            }
        }

        //FilePath label txt:FilePath/Directory
        public string FileLabelTxt
        {
            get { return fileLabelTxt; }
            set
            {
                fileLabelTxt = value;
                RaisePropertyChanged("FileLabelTxt");
            }
        }

        public string DLSExport_KeysTxt
        {
            get
            {
                if (KmtConstants.IsTpiCorp)
                    return MergedResources.Export_KeysTxtTpi;
                else
                    return MergedResources.Export_KeysTxt;
            }
        }

        public string DLSExport_ReKeysTxt
        {
            get
            {
                if (KmtConstants.IsTpiCorp)
                    return MergedResources.Export_ReKeysTxtTpi;
                else
                    return MergedResources.Export_ReKeysTxt;
            }
        }

        public string ULSExport_ReportKeysTxt
        {
            get
            {
                if (KmtConstants.IsTpiCorp)
                    return MergedResources.Export_ReportKeysTxtTpi;
                else
                    return MergedResources.Export_ReportKeysTxt;
            }
        }

        public string ULSExport_ReReportKeysTxt
        {
            get
            {
                if (KmtConstants.IsTpiCorp)
                    return MergedResources.Export_ReReportKeysTxtTpi;
                else
                    return MergedResources.Export_ReReportKeysTxt;
            }
        }

        public bool SubsidiaryVisibility
        {
            get { return subsidiaryVisibility; }
            set
            {
                subsidiaryVisibility = value;
                RaisePropertyChanged("SubsidiaryVisibility");
            }
        }

        public bool HeadQuarterVisibility
        {
            get { return headQuarterVisibility; }
            set
            {
                headQuarterVisibility = value;
                RaisePropertyChanged("HeadQuarterVisibility");
            }
        }

        public bool RbKeyVisibility
        {
            get { return rbKeyVisibility; }
            set
            {
                rbKeyVisibility = value;
                RaisePropertyChanged("RbKeyVisibility");
            }
        }

        public bool RbReportVisibility
        {
            get { return rbReportVisibility; }
            set
            {
                rbReportVisibility = value;
                RaisePropertyChanged("RbReportVisibility");
            }
        }

        public bool RbToolKeyVisibility
        {
            get { return rbToolKeyVisibility; }
            set
            {
                rbToolKeyVisibility = value;
                RaisePropertyChanged("RbToolKeyVisibility");
            }
        }

        public bool RbCBRVisibility
        {
            get { return rbCBRVisibility; }
            set
            {
                rbCBRVisibility = value;
                RaisePropertyChanged("RbCBRVisibility");
            }
        }

        public bool RbDuplicateCBRVisibility
        {
            get { return rbDuplicateCBRVisibility; }
            set
            {
                rbDuplicateCBRVisibility = value;
                RaisePropertyChanged("RbDuplicateCBRVisibility");
            }
        }

        public bool EncryptCheckedVisibility
        {
            get { return encryptCheckedVisibility; }
            set
            {
                encryptCheckedVisibility = value;
                RaisePropertyChanged("EncryptCheckedVisibility");
            }
        }

        public bool ReNoteVisibility
        {
            get { return reNoteVisibility; }
            set
            {
                reNoteVisibility = value;
                RaisePropertyChanged("ReNoteVisibility");
            }
        }

        public string KeySelectTit
        {
            get { return this.keySelectTit; }
            set
            {
                keySelectTit = value;
                RaisePropertyChanged("KeySelectTit");
            }
        }

        public bool IsEncryptChecked
        {
            get { return this.isEncryptChecked; }
            set
            {
                this.isEncryptChecked = value;
                RaisePropertyChanged("IsEncryptChecked");
            }
        }

        public ObservableCollection<Subsidiary> Subsidiarys
        {
            get { return subsidiarys; }
        }

        public ObservableCollection<HeadQuarter> HeadQuarters
        {
            get { return headQuarters; }
        }

        public Subsidiary SelectedSubsidiary
        {
            get { return selectedSubsidiary; }
            set
            {
                selectedSubsidiary = value;
                RaisePropertyChanged("SelectedSubsidiary");
                //generate default filename
                GenerateTargetFileName();
            }
        }

        public HeadQuarter SelectedHeadQuarter
        {
            get { return selectedHeadQuarter; }
            set
            {
                selectedHeadQuarter = value;
                RaisePropertyChanged("SelectedHeadQuarter");
                //generate default filename
                GenerateTargetFileName();
            }
        }

        public string SelectedTPI
        {
            get { return SelectedSubsidiary.DisplayName; }
        }

        public DateTime? BeginExportDate
        {
            get { return beginExportDate; }
            set
            {
                beginExportDate = value;
                RaisePropertyChanged("BeginExportDate");
            }
        }

        public DateTime? EndExportDate
        {
            get { return endExportDate; }
            set
            {
                endExportDate = value;
                RaisePropertyChanged("EndExportDate");
            }
        }

        public string SearchLogExportTo
        {
            get { return searchLogExportTo; }
            set
            {
                searchLogExportTo = value;
                RaisePropertyChanged("SearchLogExportTo");
            }
        }

        public string SearchLogFileName
        {
            get { return searchLogFileName; }
            set
            {
                searchLogFileName = value;
                RaisePropertyChanged("SearchLogFileName");
            }
        }

        public ObservableCollection<CbrKey> CBRCollection
        {
            get { return cBRCollention; }
            set
            {
                cBRCollention = value;
                RaisePropertyChanged("CBRCollection");
            }
        }

        public ObservableCollection<KeyExportLogModel> KeyLogCollection
        {
            get { return keyLogCollection; }
            set
            {
                keyLogCollection = value;
                RaisePropertyChanged("KeyLogCollection");
            }
        }

        public KeyExportLogModel KeyLogSelected
        {
            get
            {
                //generate export log default fileName
                if (keyLogSelected != null && (this.exportType == Constants.ExportType.ReFulfilledKeys || this.exportType == Constants.ExportType.ReReportKeys || this.exportType == Constants.ExportType.ReCBR || this.exportType == Constants.ExportType.ReToolKeys || this.exportType == Constants.ExportType.ReReturnKeys))
                {
                    if (string.IsNullOrEmpty(FileName))
                        FileName = Path.Combine(Directory.GetCurrentDirectory(), defaultExportFolderName, targetFileName, keyLogSelected.keyExportLog.FileName);
                    else
                        FileName = FileName.Replace(Path.GetFileName(FileName), keyLogSelected.keyExportLog.FileName);
                }
                return keyLogSelected;
            }
            set
            {
                keyLogSelected = value;
                RaisePropertyChanged("KeyLogSelected");
            }
        }

        public ICommand BrowseCommand
        {
            get
            {
                if (browseCommand == null)
                    browseCommand = new DelegateCommand(() =>
                    {
                        if (this.exportType == Constants.ExportType.ToolKeys)
                            ChooseExportPath();
                        else
                            ChooseLocation();
                    }, () =>
                    {
                        return (this.exportType == Constants.ExportType.FulfilledKeys || this.exportType == Constants.ExportType.ReportKeys
                            || this.exportType == Constants.ExportType.ToolKeys || this.exportType == Constants.ExportType.CBR || this.exportType == Constants.ExportType.ReturnKeys) || (this.KeyLogSelected != null);
                    });
                return browseCommand;
            }
        }

        public override ICommand ViewCommand
        {
            get
            {
                if (viewCommand == null)
                    viewCommand = new DelegateCommand(() => { ViewKeys(); },
                    () => { return (KeyLogSelected != null); });
                return viewCommand;
            }
        }

        public override ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                    nextCommand = new DelegateCommand(() => { GoToNextPage(); },
                    () =>
                    {
                        return IsKeysChecked || IsKeysReportChecked || IsCBRChecked || IsToolKeyChecked || IsReKeysChecked || IsReKeysReportChecked || IsReCBRChecked || IsReToolKeyChecked
                            || IsReturnKeyChecked || IsReReturnKeyChecked;
                    });
                return nextCommand;
            }
        }

        /// <summary>
        /// export log search command
        /// </summary>
        public ICommand LogSearchCommand
        {
            get
            {
                if (logSearchCommand == null)
                    logSearchCommand = new DelegateCommand(() => { SearchLogs(); });
                return logSearchCommand;
            }
        }

        #endregion

        #region Constructors

        public ExportKeysViewModel(IKeyProxy keyProxy, IConfigProxy configProxy, IHeadQuarterProxy hqProxy, ISubsidiaryProxy ssProxy)
            : base(keyProxy)
        {
            this.subProxy = ssProxy;
            this.headquarterProxy = hqProxy;
            this.configProxy = configProxy;
            base.WindowTitle = MergedResources.Export_WinTit;
            if (ReturnKeysListModelVM == null)
                ReturnKeysListModelVM = new ReturnKeysListViewModel();
            InitializeCollections();
            InitKeystates();
            InitRbEnable();
            if (ExportTypeChanged != null)
                ExportTypeChanged(this, new EventArgs());
            isRequireOHRData = configProxy.GetRequireOHRData();
        }

        /// <summary>
        /// init UI display 
        /// </summary>
        private void InitializeCollections()
        {
            this.InitView();
            this.RbDuplicateCBRVisibility = false;
            if (KmtConstants.IsOemCorp)
            {
                this.RbKeyVisibility = true;
                this.RbReportVisibility = false;
                this.RbToolKeyVisibility = false;
                this.RbCBRVisibility = true;
                this.HeadQuarterVisibility = false;
                InitSubsidiarys();
            }
            else if (KmtConstants.IsFactoryFloor)
            {
                this.RbKeyVisibility = false;
                this.RbReportVisibility = true;
                this.RbToolKeyVisibility = true;
                this.RbCBRVisibility = false;
                this.HeadQuarterVisibility = false;
                InitHeadQuarter();
            }
            else
            {
                this.RbKeyVisibility = true;
                if (KmtConstants.CurrentHeadQuarter != null && KmtConstants.CurrentHeadQuarter.IsCentralizedMode)
                    this.RbReportVisibility = true;
                else
                    this.RbReportVisibility = false;
                this.RbToolKeyVisibility = false;
                this.RbCBRVisibility = true;
                InitSubsidiarys();
                InitHeadQuarter();
            }
            this.IsEncryptChecked = configProxy.GetIsEncryptExportedFile();
        }

        /// <summary>
        /// Init Type select page visibility
        /// </summary>
        protected override void InitView()
        {
            base.InitView();
            this.IsNextButtonVisible = true;
            this.IsExecuteButtonVisible = false;
            this.SubsidiaryVisibility = false;
        }

        //init rbcheck is enable
        private void InitRbEnable()
        {
            if (headQuarters != null && headQuarters.Count > 0)
                IsUlSEnable = true;
            else
                IsUlSEnable = false;
            if (subsidiarys != null && subsidiarys.Count > 0)
                IsDlSEnable = true;
            else
                IsDlSEnable = false;
            IsMSEnable = configProxy.GetIsMsServiceEnabled();

            if (!IsUlSEnable)
                RbReportVisibility = false;
            if (!IsDlSEnable)
                RbKeyVisibility = false;
            if (KmtConstants.IsTpiCorp)
            {
                if (KmtConstants.CurrentHeadQuarter != null && KmtConstants.CurrentHeadQuarter.IsCentralizedMode)
                    RbCBRVisibility = false;
                else
                    RbCBRVisibility = true;
            }
        }

        #endregion

        #region search keys

        /// <summary>
        /// search keys to export
        /// </summary>
        /// <returns></returns>
        protected override List<KeyInfoModel> SearchKeys()
        {
            List<KeyInfo> searchkeys = null;
            switch (this.exportType)
            {
                case Constants.ExportType.FulfilledKeys:
                case Constants.ExportType.ToolKeys:
                    searchkeys = keyProxy.SearchFulfilledKeys(KeySearchCriteria);
                    break;
                case Constants.ExportType.ReportKeys:
                    searchkeys = keyProxy.SearchBoundKeys(KeySearchCriteria);
                    break;
                case Constants.ExportType.CBR:
                    searchkeys = keyProxy.SearchBoundKeysToMs(KeySearchCriteria);
                    break;
                case Constants.ExportType.ReturnKeys:
                    searchkeys = keyProxy.SearchToReturnKeys(KeySearchCriteria);
                    break;
                default:
                    break;
            }

            List<KeyInfoModel> returnKeys = new List<KeyInfoModel>();
            if (searchkeys == null && searchkeys.Count <= 0)
                returnKeys = null;
            else
                returnKeys = searchkeys.ToKeyInfoModel().ToList();
            return returnKeys;
            ////if (searchkeys != null && searchkeys.Count > 0)
            ////{
            ////    List<KeyInfoModel> newKeys = searchkeys.ToKeyInfoModel().ToList();
            ////    if (this.exportType == Constants.ExportType.ReturnKeys)
            ////    {
            ////        // this.ReturnKeysListModelVM.ReturnKeys.new ObservableCollection<ReturnKeyModel>(keys.Select(k => new ReturnKeyModel() { ReturnReportKey = k}).ToList());
            ////        if (this.ReturnKeysListModelVM.ReturnKeys == null)
            ////            this.ReturnKeysListModelVM.ReturnKeys = new ObservableCollection<ReturnKeyModel>();
            ////        if (isSearchFirstPage)
            ////        {
            ////            this.ReturnKeysListModelVM.ReturnKeys.Clear();
            ////            isSearchFirstPage = false;
            ////        }
            ////        if (newKeys != null && newKeys.Count > 0)
            ////        {
            ////            foreach (var key in newKeys)
            ////            {
            ////                if (!this.ReturnKeysListModelVM.ReturnKeys.Any(k => k.ReturnReportKey.keyInfo.KeyId == key.keyInfo.KeyId))
            ////                {
            ////                    key.PropertyChanged += base.OnKeySelectedChanged;
            ////                    this.ReturnKeysListModelVM.ReturnKeys.Add(new ReturnKeyModel { ReturnReportKey = key });
            ////                }
            ////            }
            ////            RaisePropertyChanged("ReturnKeys");
            ////        }
            ////        ReturnKeysListModelVM.AddkeyReturnTypeByCredit();
            ////    }
            ////    return newKeys;
            ////}
            ////else
            ////{
            ////    this.ReturnKeysListModelVM.ReturnKeys = null;
            ////    return null;
            ////}
        }

        //get keys by page
        protected override void GetPageKeys()
        {
            base.GetPageKeys();
            if (this.exportType == Constants.ExportType.ReturnKeys)
            {
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

        }


        /// <summary>
        /// search keygroup to export
        /// </summary>
        protected override void SearchKeyGroups()
        {
            List<KeyGroup> searchkeys = null;
            switch (this.exportType)
            {
                case Constants.ExportType.FulfilledKeys:
                case Constants.ExportType.ToolKeys:
                    searchkeys = keyProxy.SearchFulfilledKeyGroups(KeySearchCriteria);
                    break;
                case Constants.ExportType.ReportKeys:
                    searchkeys = keyProxy.SearchBoundKeyGroups(KeySearchCriteria);
                    break;
                case Constants.ExportType.CBR:
                    searchkeys = keyProxy.SearchBoundKeyGroupsToMs(KeySearchCriteria);
                    break;
                case Constants.ExportType.ReturnKeys:
                    KeyGroups = null;
                    return;
                default:
                    break;
            }
            KeyGroups = new ObservableCollection<KeyGroupModel>(searchkeys.ToKeyGroupModel());
        }

        #endregion

        #region validate keys

        /// <summary>
        /// validate keys group to export
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateKeyGroups()
        {
            if (!base.ValidateKeyGroups())
                return false;
            string TPI = string.Empty;
            if (SelectedSubsidiary != null)
                TPI = this.SelectedSubsidiary.DisplayName;
            if (string.IsNullOrEmpty(TPI) && exportType == Constants.ExportType.FulfilledKeys)
            {
                System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_SelectTPIMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!ValidateFilePath())
                return false;
            if (!ConfirmAndTit())
                return false;
            return true;
        }

        /// <summary>
        /// validate keys to export
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateKeys()
        {
            if (this.exportType == Constants.ExportType.ReturnKeys)
            {
                if (this.ReturnKeysListModelVM.ReturnKeys == null || this.ReturnKeysListModelVM.ReturnKeys.Count <= 0)
                {
                    System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_NoKeysMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if (this.ReturnKeysListModelVM.ReturnKeys.Where(k => k.ReturnReportKey.IsSelected).Count() <= 0)
                {
                    System.Windows.MessageBox.Show(MergedResources.Common_SelectKeysMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                return ReturnKeysListModelVM.ValidateOemRmaNumberTxt() && ReturnKeysListModelVM.ValidateKeyStateNotice();
            }
            if (!base.ValidateKeys())
                return false;

            string TPI = string.Empty;
            if (SelectedSubsidiary != null)
                TPI = this.SelectedSubsidiary.DisplayName;
            string ReportTo = string.Empty;
            if (SelectedHeadQuarter != null)
                ReportTo = this.SelectedHeadQuarter.DisplayName;

            if (string.IsNullOrEmpty(TPI) && exportType == Constants.ExportType.FulfilledKeys)
            {
                System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_SelectTPIMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrEmpty(ReportTo) && exportType == Constants.ExportType.ReportKeys)
            {
                System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_SelectTPIMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!ValidateFilePath())
                return false;
            if (!ConfirmAndTit())
                return false;
            return true;
        }

        /// <summary>
        ///validate export log select
        /// </summary>
        /// <returns></returns>
        protected override bool ValidateOthers()
        {
            if (this.exportType == Constants.ExportType.ReFulfilledKeys || this.exportType == Constants.ExportType.ReReportKeys || this.exportType == Constants.ExportType.ReToolKeys || this.exportType == Constants.ExportType.ReCBR || this.exportType == Constants.ExportType.ReReturnKeys)
            {
                if (ValidateLogkeys())
                {
                    if ((this.exportType == Constants.ExportType.ReFulfilledKeys || this.exportType == Constants.ExportType.ReReportKeys) && this.KeyLogSelected.keyExportLog.IsEncrypted == false)
                    {
                        System.Windows.MessageBox.Show(MergedResources.Export_SelectEncryptedLogMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                    return false;
            }
            else if (this.exportType == Constants.ExportType.DuplicateCBR)
            {
                if (this.CBRCollection == null || this.CBRCollection.Count <= 0)
                {
                    System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_NoKeysMsg, MergedResources.Common_Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///message of export keys need to confirm
        /// </summary>
        /// <returns></returns>
        private bool ConfirmAndTit()
        {
            if ((this.exportType == Constants.ExportType.FulfilledKeys || this.exportType == Constants.ExportType.ReportKeys))
            {
                //Add a prompt to user have them be aware that when compatible mode is selected, the serial number field will not be exported along with - Rally, Dec. 22, 2014
                if (this.IsInCompatibleMode)
                {
                    if (System.Windows.MessageBox.Show(MergedResources.ExportKey_CompatibleFormatConfirmMsg, MergedResources.ExportKey_CompatibleFormatComfirmTitle, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                    { 
                        return false; 
                    }
                }

                if (!this.IsEncryptChecked)
                {
                    MessageBoxResult encrykey = System.Windows.MessageBox.Show(string.Format(MergedResources.Export_ConfirmToMsg, GetExportTarget()).ToString() + "," + System.Environment.NewLine + MergedResources.ExportKey_NonEncryptMsg + "." + System.Environment.NewLine + MergedResources.ExportKey_ConfirmMsg, MergedResources.Common_Confirmation, MessageBoxButton.OKCancel);
                    if (encrykey != MessageBoxResult.OK)
                        return false;
                }
                else
                {
                    MessageBoxResult confirmkey = System.Windows.MessageBox.Show(string.Format(MergedResources.Export_ConfirmToMsg, GetExportTarget()).ToString() + "," + System.Environment.NewLine + MergedResources.ExportKey_ConfirmMsg, MergedResources.Common_Confirmation, MessageBoxButton.OKCancel);
                    if (confirmkey != MessageBoxResult.OK)
                        return false;
                }
            }
            if (this.exportType == Constants.ExportType.CBR)
            {
                MessageBoxResult confirmkey = System.Windows.MessageBox.Show(string.Format(MergedResources.Export_ConfirmToMsg, MergedResources.Export_Ms).ToString() + "," + System.Environment.NewLine + MergedResources.ExportKey_ConfirmMsg, MergedResources.Common_Confirmation, MessageBoxButton.OKCancel);
                if (confirmkey != MessageBoxResult.OK)
                    return false;

                if ((TabIndex==0&&keyProxy.SearchBoundKeysToReport(base.KeyGroups.Where(k => k.KeyGroup.Quantity > 0).Select(k => k.KeyGroup).ToList()).Any(k => !k.OemOptionalInfo.HasOHRData)) ||(TabIndex==1&&base.Keys.Where(k => k.IsSelected).Any(k => !k.keyInfo.OemOptionalInfo.HasOHRData)))
                {
                    if (isRequireOHRData)
                    {
                        System.Windows.MessageBox.Show(ResourcesOfRTMv1_6.EditOptionalInfo_RequireOHRDataMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        MessageBoxResult confirm = System.Windows.MessageBox.Show(ResourcesOfRTMv1_6.EditOptionalInfo_MissOHRDataMsg, MergedResources.Common_Confirmation, MessageBoxButton.OKCancel);
                        if (confirm != MessageBoxResult.OK)
                            return false;
                    }
                }

            }
            return true;
        }

        //validate logkeys to export
        private bool ValidateLogkeys()
        {
            if (this.KeyLogCollection == null || this.KeyLogCollection.Count <= 0)
            {
                System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_NoKeysMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (this.KeyLogSelected == null)
            {
                System.Windows.MessageBox.Show(MergedResources.Export_SelectLogMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrEmpty(this.FileName))
            {
                System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_SelectFileMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!ValidateFilePath())
                return false;
            return true;
        }

        //validate filePath of export keys
        private bool ValidateFilePath()
        {
            if (string.IsNullOrEmpty(this.FileName))
            {
                System.Windows.MessageBox.Show(MergedResources.ExportKeysViewModel_SelectFileMsg, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            try
            {
                string fullpath = this.FileName;
                if (this.exportType == Constants.ExportType.ToolKeys)
                {
                    fullpath = Path.Combine(fullpath, "OA3.xml");
                }
                string filePath = Path.GetDirectoryName(fullpath);
                string filename = Path.GetFileName(fullpath);
                char[] invalidPathChars = Path.GetInvalidPathChars();
                char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
                if (filePath.ToCharArray().Intersect(invalidPathChars).Count() > 0)
                {
                    System.Windows.MessageBox.Show(MergedResources.Export_InvalidPath, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if (filename.ToCharArray().Intersect(invalidFileNameChars).Count() > 0)
                {
                    System.Windows.MessageBox.Show(MergedResources.Export_InvalidPath, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(MergedResources.Export_InvalidPath, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        #endregion

        #region view logfile keys

        /// <summary>
        /// view export log file contains keys
        /// </summary>
        protected override void ViewKeys()
        {
            if (!ValidateLogkeys())
                return;
            ViewFileKey viewwindow = new ExportKeysView.ViewFileKey(this.KeyLogSelected.keyExportLog.ExportLogId, keyProxy);
            viewwindow.Owner = this.View;
            viewwindow.ShowDialog();
        }

        #endregion

        #region process keys

        /// <summary>
        /// exec export keys
        /// </summary>
        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> results = null;
            switch (this.exportType)
            {
                case Constants.ExportType.FulfilledKeys:
                case Constants.ExportType.ReportKeys:
                case Constants.ExportType.ToolKeys:
                case Constants.ExportType.CBR:
                case Constants.ExportType.ReturnKeys:
                    results = ExportKeys();
                    break;
                case Constants.ExportType.ReFulfilledKeys:
                case Constants.ExportType.ReReportKeys:
                case Constants.ExportType.ReToolKeys:
                case Constants.ExportType.ReCBR:
                case Constants.ExportType.ReReturnKeys:
                    results = keyProxy.ReExportKeys(this.KeyLogSelected.keyExportLog.ExportLogId, FileName);
                    break;
                default:
                    break;
            }
            SummaryText = string.Format(MergedResources.Export_resultMsg, results.Count(k => !k.Failed).ToString(), results.Count(k => k.Failed).ToString());
            KeyOperationResults = new ObservableCollection<KeyOperationResult>(results);
        }

        private List<KeyOperationResult> ExportKeys()
        {
            List<KeyOperationResult> results = null;
            object target = null;
            string UserName = string.Empty;
            string AccessKey = string.Empty;
            if (this.exportType == Constants.ExportType.FulfilledKeys)
            {
                target = SelectedSubsidiary;
                UserName = SelectedSubsidiary.UserName;
                AccessKey = SelectedSubsidiary.AccessKey;
            }
            else if (this.exportType == Constants.ExportType.ReportKeys)
            {
                target = SelectedHeadQuarter;
                UserName = SelectedHeadQuarter.UserName;
                AccessKey = selectedHeadQuarter.AccessKey;
            }
            if (this.exportType == Constants.ExportType.ToolKeys || this.exportType == Constants.ExportType.CBR || this.exportType == Constants.ExportType.ReturnKeys)
                IsEncryptChecked = false;
            ExportParameters exportParameters = new ExportParameters()
            {
                ExportType = exportType,
                OutputPath = FileName,
                IsEncrypted = IsEncryptChecked,
                To = target,
                UserName = UserName,
                AccessKey = AccessKey,
                CreateBy = KmtConstants.LoginUser,

                IsInCompatibleMode = this.isInCompatibleMode,//Fix for Bug#126, Rally, Dec.16, 2014
                TransformationXSLT = KmtConstants.XSLT_ULSKeyExportCompitable //Fix for Bug#126, Rally, Dec.16, 2014
            };

            //export return keys
            if (this.exportType == Constants.ExportType.ReturnKeys)
                results = keyProxy.ExportReturnKeys(this.ReturnKeysListModelVM.ReturnKeys.ToReturnReport(this.ReturnKeysListModelVM.OemRmaNumber, this.ReturnKeysListModelVM.OemRmaDate.Value, this.ReturnKeysListModelVM.ReturnNoCredit), exportParameters);
            else
            {
                if (TabIndex == 0)
                    exportParameters.Keys = KeyGroups.Where(k => k.KeyGroup.Quantity > 0).Select(k => k.KeyGroup).ToList();
                else if (TabIndex == 1)
                    exportParameters.Keys = Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList();
                results = base.keyProxy.ExportKeys(exportParameters);
            }

            if (results.Any(k => k.Failed))
            {
                this.FileName = string.Empty;
            }
            if (!results.Any(k => k.Failed) && EncryptCheckedVisibility)
                configProxy.UpdateEncryptExportedFileSwitch(this.IsEncryptChecked);
            return results;
        }

        #endregion

        #region page change process

        /// <summary>
        /// next operation
        /// </summary>
        protected override void GoToNextPage()
        {
            if (this.CurrentPageIndex == 0)
            {
                InitStepPagesByExportType();
                TypePageNextVisibility();
            }
            else if (this.CurrentPageIndex == 1 && this.exportType == Constants.ExportType.ReturnKeys)
            {
                if (ReturnKeysListModelVM.ValidateOemRmaNumberTxt() && ReturnKeysListModelVM.ValidateReturnNoCredit())
                    TypePageNextVisibility();
            }
        }

        /// <summary>
        /// previous operation
        /// </summary>
        protected override void GoToPreviousPage()
        {
            if ((this.CurrentPageIndex == 1 && this.exportType != Constants.ExportType.ReturnKeys) || (this.CurrentPageIndex == 2 && this.exportType == Constants.ExportType.ReturnKeys))
            {
                SearchControlVM.BeginMsFulfiledDate = null;
                SearchControlVM.EndMsFulfiledDate = null;
                SearchControlVM.KeyTypesVisibility = Visibility.Visible;
                if (this.exportType == Constants.ExportType.ReFulfilledKeys || this.exportType == Constants.ExportType.ReReportKeys
                    || this.exportType == Constants.ExportType.ReCBR || this.exportType == Constants.ExportType.ReReturnKeys || this.exportType == Constants.ExportType.ReToolKeys)
                    this.IsViewButtonVisible = false;
                if (this.TabIndex == 3)
                    TabIndex = KmtConstants.FirstTab;
                base.GoToPreviousPage();
                if (this.StepPages.Count == 3)
                {
                    this.StepPages.Remove(StepPages[2]);
                    this.StepPages.Remove(StepPages[1]);
                }
                if (this.exportType == Constants.ExportType.ReturnKeys)
                    this.IsPreviousButtonVisible = true;
            }
            else if (this.CurrentPageIndex == 1 && this.exportType == Constants.ExportType.ReturnKeys)
            {
                base.GoToPreviousPage();
                this.CurrentPageIndex = 0;
                this.StepPages.Remove(StepPages[3]);
                this.StepPages.Remove(StepPages[2]);
                this.StepPages.Remove(StepPages[1]);
            }
        }

        /// <summary>
        ///  export finish visibility  
        /// </summary>
        protected override void GoToFinalPage()
        {
            base.GoToFinalPage();
            this.CurrentPageIndex = this.CurrentPageIndex;
            this.IsCancelButtonVisible = false;
            this.IsViewButtonVisible = false;
            this.IsFinishButtonVisible = true;
        }

        #endregion

        /// <summary>
        /// export Type select page next Visibility
        /// </summary>
        public void TypePageNextVisibility()
        {
            if (this.exportType == Constants.ExportType.FulfilledKeys || this.exportType == Constants.ExportType.ReportKeys)
                targetFileName = "To_" + GetExportTarget() + "_At_" + defaultFileName;
            else if (this.exportType == Constants.ExportType.CBR || this.exportType == Constants.ExportType.ReturnKeys)
                targetFileName = "To_MS_" + this.exportType.ToString() + "_At_" + defaultFileName;
            else
                targetFileName = string.Empty;
            FileName = Path.Combine(Directory.GetCurrentDirectory(), defaultExportFolderName, targetFileName);
            if (this.exportType == Constants.ExportType.ToolKeys)
                this.FileLabelTxt = MergedResources.Common_Directory;
            else
                this.FileLabelTxt = MergedResources.Common_FilePath;

            if (IsKeysReportChecked)
                SearchControlVM.KeyTypesVisibility = Visibility.Collapsed;
            this.IsPreviousButtonVisible = true;
            this.IsExecuteButtonVisible = true;
            this.IsNextButtonVisible = false;
            this.IsCancelButtonVisible = true;
            this.IsFinishButtonVisible = false;
            switch (exportType)
            {
                case Constants.ExportType.FulfilledKeys:
                    this.SubsidiaryVisibility = true;
                    this.EncryptCheckedVisibility = true;
                    this.HeadQuarterVisibility = false;
                    this.CurrentPageIndex = 1;
                    Keys = null;
                    SearchAllKeys(0);
                    break;
                case Constants.ExportType.ReportKeys:
                    this.SubsidiaryVisibility = false;
                    this.EncryptCheckedVisibility = true;
                    this.HeadQuarterVisibility = true;
                    this.CurrentPageIndex = 1;
                    Keys = null;
                    SearchAllKeys(0);
                    break;
                case Constants.ExportType.CBR:
                    this.SubsidiaryVisibility = false;
                    this.EncryptCheckedVisibility = false;
                    this.HeadQuarterVisibility = false;
                    this.CurrentPageIndex = 1;
                    Keys = null;
                    SearchAllKeys(0);
                    break;
                case Constants.ExportType.ToolKeys:
                    this.FileName = Path.Combine(Directory.GetCurrentDirectory(), defaultOA3ToolFolderName, defaultFileName);
                    FileName = Path.GetDirectoryName(FileName);
                    this.EncryptCheckedVisibility = false;
                    this.HeadQuarterVisibility = false;
                    this.CurrentPageIndex = 1;
                    Keys = null;
                    SearchAllKeys(0);
                    break;
                case Constants.ExportType.DuplicateCBR:
                    this.HeadQuarterVisibility = false;
                    this.CurrentPageIndex = 1;
                    break;
                case Constants.ExportType.ReturnKeys:
                    InitReturnKeyTypes();//Merged from v1.9 Rally, Sept. 26, 2014
                    if (this.CurrentPageIndex == 0)
                    {
                        this.IsExecuteButtonVisible = false;
                        this.IsNextButtonVisible = true;
                    }
                    else
                    {
                        this.SubsidiaryVisibility = true;
                        this.EncryptCheckedVisibility = true;
                        this.HeadQuarterVisibility = false;
                        Keys = null;
                        this.TabIndex = 1;
                        SearchAllKeys(0);
                    }
                    this.CurrentPageIndex = this.CurrentPageIndex + 1;
                    break;
                case (Constants.ExportType.ReFulfilledKeys):
                case (Constants.ExportType.ReReportKeys):
                case (Constants.ExportType.ReToolKeys):
                case (Constants.ExportType.ReCBR):
                case (Constants.ExportType.ReReturnKeys):
                    TypePageReKeyNextVisibility();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// type page rekey init display
        /// </summary>
        private void TypePageReKeyNextVisibility()
        {
            this.FileName = string.Empty;
            this.KeyLogCollection = null;
            this.TabIndex = 3;
            BeginExportDate = DateTime.Now.AddMonths(-3);
            EndExportDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            SearchLogExportTo = string.Empty;
            SearchLogFileName = string.Empty;
            SearchLogs();
            this.EncryptCheckedVisibility = false;
            this.HeadQuarterVisibility = false;
            this.IsViewButtonVisible = true;
            this.IsExecuteButtonEnable = true;
            if (this.exportType == Constants.ExportType.ReFulfilledKeys || this.exportType == Constants.ExportType.ReReportKeys)
                ReNoteVisibility = true;
            else
                ReNoteVisibility = false;
            this.CurrentPageIndex = 1;
        }

        //search export log
        private void SearchLogs(int startIndex = 0)
        {
            if (!ValidationHelper.ValidateDateRange(BeginExportDate, EndExportDate))
                return;
            if (startIndex == 0)
                this.KeyLogCollection = null;
            ExportLogSearchCriteria logSearch = new ExportLogSearchCriteria();
            List<KeyExportLog> logList = new List<KeyExportLog>();
            string reExportType = string.Empty;
            switch (this.exportType)
            {
                case Constants.ExportType.ReFulfilledKeys:
                    reExportType = Constants.ExportType.FulfilledKeys.ToString();
                    break;
                case Constants.ExportType.ReReportKeys:
                    reExportType = Constants.ExportType.ReportKeys.ToString();
                    break;
                case Constants.ExportType.ReCBR:
                    reExportType = Constants.ExportType.CBR.ToString();
                    break;
                case Constants.ExportType.ReToolKeys:
                    reExportType = Constants.ExportType.ToolKeys.ToString();
                    break;
                case Constants.ExportType.ReDuplicateCBR:
                    reExportType = Constants.ExportType.DuplicateCBR.ToString();
                    break;
                case Constants.ExportType.ReReturnKeys:
                    reExportType = Constants.ExportType.ReturnKeys.ToString();
                    break;
                default:
                    break;
            }
            logSearch.StartIndex = startIndex;
            if (!string.IsNullOrEmpty(sortColumnLog))
            {
                logSearch.SortBy = sortColumnLog;
                logSearch.SortByDesc = isDescLog;
            }
            logSearch.ExportTypes = new List<string>() { reExportType };
            logSearch.PageSize = KmtConstants.DefaultPageSize;
            logSearch.PageSize = 15;
            logSearch.DateFrom = BeginExportDate;
            logSearch.DateTo = EndExportDate.Value.Date.AddDays(1).AddSeconds(-1);
            logSearch.ExportTo = SearchLogExportTo;
            logSearch.FileName = SearchLogFileName;
            logList = base.keyProxy.SearchExportLogs(logSearch);

            if (this.KeyLogCollection == null)
                this.KeyLogCollection = new ObservableCollection<KeyExportLogModel>();
            if (logList != null && logList.Count > 0)
            {
                foreach (var key in logList)
                {
                    if (this.exportType == Constants.ExportType.ReToolKeys || this.exportType == Constants.ExportType.ReCBR)
                        key.IsEncrypted = false;
                    if (!this.KeyLogCollection.Any(k => k.keyExportLog.ExportLogId == key.ExportLogId))
                        this.KeyLogCollection.Add(new KeyExportLogModel() { keyExportLog = key });
                }
            }
            RaisePropertyChanged("KeyLogCollection");
        }

        internal void LoadUpLogs()
        {
            SearchLogs(this.KeyLogCollection.Count);
        }

        internal void LogSortByColum(string sortColumn)
        {
            this.sortColumnLog = sortColumn;
            isDescLog = !isDescLog;
            this.KeyLogCollection = null;
            SearchLogs();
        }

        internal void LoadReturnKeys()
        {
            base.LoadUpKeys();
            var newReturnKeys = new ObservableCollection<ReturnKeyModel>(base.Keys.Select(k => new ReturnKeyModel() { ReturnReportKey = k }).ToList());

            if (this.ReturnKeysListModelVM.ReturnKeys == null)
                this.ReturnKeysListModelVM.ReturnKeys = new ObservableCollection<ReturnKeyModel>();

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

        #region Private Method

        //display/hide left panel pages title
        private void InitStepPagesByExportType()
        {
            switch (this.exportType)
            {
                case (Constants.ExportType.FulfilledKeys):
                case (Constants.ExportType.ReportKeys):
                case (Constants.ExportType.CBR):
                case (Constants.ExportType.ToolKeys):
                    this.StepPages.Add(new KeysSelectPageView(this));
                    this.StepPages.Add(new FinishPageView(this));
                    break;
                case Constants.ExportType.ReturnKeys:
                    this.StepPages.Add(new ReturnKeyInputPage(this));
                    this.StepPages.Add(new SelectReturnKeyView(this));
                    this.StepPages.Add(new FinishPageView(this));
                    break;
                default:
                    this.StepPages.Add(new KeysLogSelectPage(this));
                    this.StepPages.Add(new FinishPageView(this));
                    break;
            }
        }

        //get export file target
        private string GetExportTarget()
        {
            string exportTarget = string.Empty;
            switch (this.exportType)
            {
                case Constants.ExportType.FulfilledKeys:
                    exportTarget = (this.SelectedSubsidiary != null ? this.SelectedSubsidiary.DisplayName : string.Empty);
                    break;
                case Constants.ExportType.ReportKeys:
                    exportTarget = (this.SelectedHeadQuarter != null ? this.SelectedHeadQuarter.DisplayName : string.Empty);
                    break;
                default:
                    break;
            }
            return exportTarget;
        }

        private void GenerateTargetFileName()
        {
            //generate default filename
            if (!string.IsNullOrEmpty(this.FileName))
            {
                targetFileName = "To_" + GetExportTarget() + "_At_" + defaultFileName;
                FileName = FileName.Replace(Path.GetFileName(FileName), targetFileName);
            }
        }

        //get duplicate CBR
        private bool ShowDuplicatedCBRs()
        {
            allCBR = base.keyProxy.GetCbrsDuplicated();
            if (allCBR != null && allCBR.Count > 0)
            {
                this.CBRCollection = new ObservableCollection<CbrKey>(allCBR.SelectMany(cbr => cbr.CbrKeys));
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// choose location
        /// </summary>
        private void ChooseLocation()
        {
            if (!ValidateFilePath())
                return;
            Microsoft.Win32.SaveFileDialog exportSaveFileDialog = new Microsoft.Win32.SaveFileDialog();
            exportSaveFileDialog.FileName = Path.GetFileName(FileName);
            exportSaveFileDialog.Title = MergedResources.Common_Save;
            exportSaveFileDialog.Filter = MergedResources.Common_XmlFilter;
            if (exportSaveFileDialog.ShowDialog() == true)
                FileName = exportSaveFileDialog.FileName;
        }

        private void ChooseExportPath()
        {
            if (!ValidateFilePath())
                return;
            using (FolderBrowserDialog fbdDes = new FolderBrowserDialog())
            {
                fbdDes.RootFolder = Environment.SpecialFolder.MyComputer;
                fbdDes.SelectedPath = FileName;
                fbdDes.ShowNewFolderButton = true;
                if (fbdDes.ShowDialog() == DialogResult.OK)
                    FileName = fbdDes.SelectedPath;
            }
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

        //Merged from v1.9 - Rally, Sept. 26, 2014
        private void InitKeyTypes()
        {
            List<string> keyTypes = new List<string>() { KeyType.Standard.ToString(), KeyType.MBR.ToString(), KeyType.MAT.ToString() };
            base.SearchControlVM.KeyTypes = new ObservableCollection<string>(keyTypes);
            base.SearchControlVM.SelectedKeyType = base.SearchControlVM.KeyTypes.First();
        }

        //Merged from v1.9 - Rally, Sept. 26, 2014
        // avoid perfermonse issue when user select all key states 
        private void InitReturnKeyTypes()
        {
            List<string> keyTypes = new List<string>() { KeyType.Standard.ToString(), KeyType.MBR.ToString(), KeyType.MAT.ToString() };
            base.SearchControlVM.KeyTypes = new ObservableCollection<string>(keyTypes);
            base.SearchControlVM.SelectedKeyType = base.SearchControlVM.KeyTypes.First();
        }

        private void InitSubsidiarys()
        {
            subsidiarys = new ObservableCollection<Subsidiary>(subProxy.GetSubsidiaries());
            if (subsidiarys.Count > 0)
                SelectedSubsidiary = subsidiarys.First();
        }

        private void InitHeadQuarter()
        {
            headQuarters = new ObservableCollection<HeadQuarter>(headquarterProxy.GetHeadQuarters());
            if (headQuarters != null && headQuarters.Count > 0)
                SelectedHeadQuarter = headQuarters.First();
        }

        internal void Log_Checked()
        {
            FileName = FileName.Replace(Path.GetFileName(FileName), this.keyLogCollection.Where(log => log.IsSelected).First().keyExportLog.FileName);
        }

        #endregion
    }
}
