using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIS.Presentation.KMT.ViewModel.ViewModelBases;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.Properties;
using System.Collections.ObjectModel;
using DIS.Presentation.KMT.Models;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.ViewModel.ControlsViewModel;
using DIS.Presentation.KMT.Commands;
using System.Windows;
using DIS.Presentation.KMT.Behaviors;

namespace DIS.Presentation.KMT.ViewModel
{
    public class OhrDataUpdateViewModel : TemplateViewModelBase
    {
        #region private fields

        //add for V1.6
        private List<string> zfrm_factor_cl1s;
        private List<string> zfrm_factor_cl2s;
        private List<string> ztouch_screens;

        private bool canEdit;
        private KeyInfoModel selectedKey;
        private DelegateCommand clearCommand;
        #endregion

        #region Contructor

        public OhrDataUpdateViewModel(IKeyProxy keyProxy)
            : base(keyProxy)
        {
            base.WindowTitle = ResourcesOfRTMv1_8.MainWindow_OHR;
            Initialize();
        }

        #endregion

        #region Private Members

        private void LoadZfactorCl1s()
        {
            List<string> lists = new List<string>();
            lists.Add(string.Empty);
            OHRData.ZFRM_FACTORValue.Select(k => k.Key).ToList().ForEach(k => { lists.Add(k.ToString()); });
            this.zfrm_factor_cl1s = lists;
        }

        private void LoadZfactorCl2s(object sender, ZfrmFactorCl1PropertyChangedEventArgs e)
        {
            LoadZfactorCl2s(e.Value);
        }

        private void LoadZfactorCl2s(string factor1value)
        {
            List<string> lists = new List<string>();
            lists.Add(string.Empty);
            if (!string.IsNullOrEmpty(factor1value))
                OHRData.ZFRM_FACTORValue.Where(k => k.Key.ToString() == factor1value).FirstOrDefault().Value.ForEach(k => { lists.Add(k.ToString()); });
            this.zfrm_factor_cl2s = lists;
            RaisePropertyChanged("ZFRM_FACTOR_CL2s");
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
            SelectedKey.ZFRM_FACTOR_CL1 = null;
            SelectedKey.ZFRM_FACTOR_CL2 = null;
            SelectedKey.ZTOUCH_SCREEN = null;
            SelectedKey.ZSCREEN_SIZE = null;
            SelectedKey.ZPC_MODEL_SKU = null;
        }

        private void Initialize()
        {
            SearchControlVM.KeyTypesVisibility = Visibility.Collapsed;
            LoadZfactorCl1s();
            LoadZtouchScerrens();
            base.TabIndex = KmtConstants.SecondTab;
            base.InitView();
            SearchOhrKeys();
        }

        private void SearchOhrKeys()
        {
            KeySearchCriteria = base.SearchControlVM.FillSearchCriteria();
            if (!ValidationHelper.ValidateDateRange(KeySearchCriteria.DateFrom, KeySearchCriteria.DateTo))
                return;

            this.GetPageKeys();

            if (Keys != null && Keys.Count > 0)
                base.IsExecuteButtonEnable = true;
            else
                base.IsExecuteButtonEnable = false;

            KeySearchCriteria = null;
            RaisePropertyChanged("IsAllChecked");
        }

        #endregion

        #region public property

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
                this.selectedKey.ZfrmFactorCl1PropertyChanged += this.LoadZfactorCl2s;
                this.LoadZfactorCl2s(this.selectedKey.ZFRM_FACTOR_CL1);
                CanEdit = true;
                RaisePropertyChanged("SelectedKey");
            }
        }

        public bool CanEdit
        {
            get { return this.canEdit; }
            set
            {
                this.canEdit = value;
                RaisePropertyChanged("CanEdit");
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

        public List<string> ZTOUCH_SCREENs
        {
            get { return this.ztouch_screens; }
        }

        #endregion

        #region Override Members

        protected override void SearchKeyGroups()
        {
            KeyGroups = new ObservableCollection<KeyGroupModel>();
        }

        protected override List<KeyInfoModel> SearchKeys()
        {
            List<KeyInfo> searchkeys = base.keyProxy.SearchOhrKeysToMs(KeySearchCriteria);
            if (searchkeys == null && searchkeys.Count <= 0)
                return null;
            else
                return searchkeys.ToKeyInfoModel().ToList();
        }

        protected override bool ValidateKeys()
        {
            var keyInfoes = base.Keys.Where(k => k.IsSelected).Select(k => k.keyInfo);
            if (keyInfoes.Any(k => string.IsNullOrEmpty(k.ZFRM_FACTOR_CL1) ||
                string.IsNullOrEmpty(k.ZFRM_FACTOR_CL2) ||
                string.IsNullOrEmpty(k.ZPC_MODEL_SKU) ||
                string.IsNullOrEmpty(k.ZSCREEN_SIZE) ||
                string.IsNullOrEmpty(k.ZTOUCH_SCREEN)))
            {
                MessageBox.Show(ResourcesOfRTMv1_8.OhrUpdateViewModel_InvalideOhrData, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return base.ValidateKeys();
        }

        protected override bool ValidateKeyGroups()
        {
            return base.ValidateKeyGroups();

        }

        protected override void ProcessExecuteKeys()
        {
            List<KeyOperationResult> result = new List<KeyOperationResult>();

            result = base.keyProxy.SendOhrKeys(base.Keys.Where(k => k.IsSelected).Select(k => k.keyInfo).ToList());

            base.KeyOperationResults = new ObservableCollection<KeyOperationResult>(result);

            base.SummaryText = string.Format(ResourcesOfRTMv1_8.OhrUpdateViewModel_OhrResult,
                    base.KeyOperationResults.Where(r => !r.Failed).Count(),
                    base.KeyOperationResults.Where(r => r.Failed).Count());
        }

        #endregion
    }
}
