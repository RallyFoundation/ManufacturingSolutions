using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DIS.Business.Operation;
using DIS.Business.Proxy;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    public class HeadQuarterEditorViewModel:ViewModelBase
    {
        #region Private Members

        private const int maxSubsidiaryCount = 5;
        private const int maxCustomerNumberLength = 10;
        private const int passwordLength = 10;
        private const string charRange = @"abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$";

        private IHeadQuarterProxy headQuarterProxy;

        private List<string> subsidiaryTypes;
        private bool isSaving = false;

        private ICommand saveCommand;
        private ICommand cancelCommand;
        private ICommand generateCommand;
        private string customerNumber;
        private string userName;
        private string accessKey;
        private string subsidiaryType;

        #endregion

        #region Construct

        public HeadQuarterEditorViewModel(HeadQuarter ss = null)
            : this(ss, null)
        {
        }

        /// <summary>
        /// public constrcutor
        /// </summary>
        /// <param name="dispatcher">Dispatcher instance to send updates to DIS.Presentation.KMT. This should be passed null from automated unit tests.</param>
        public HeadQuarterEditorViewModel(HeadQuarter hq, IHeadQuarterProxy headQuarterProxy)
        {
            if (headQuarterProxy == null)
                this.headQuarterProxy = new HeadQuarterProxy();
            else
                this.headQuarterProxy = headQuarterProxy;

            
            if (hq == null)
            {
                HeadQuarter = new HeadQuarter();
                EditMode = false;
                Title = Resources.CreateHeadQuarterViewModel_Title_New;//later
                HeadContent = Resources.CreateHeadQuarterViewModel_HeadContent_Add;//later
            }
            else
            {
                HeadQuarter = hq;
                EditMode = true;
                Title = Resources.CreateHeadQuarterViewModel_Title_Edit;
                HeadContent = Resources.CreateHeadQuarterViewModel_HeadContent_Edit;
            }
            InitializeCollections(HeadQuarter);
        }

        private void InitializeCollections(HeadQuarter current)
        {
            try
            {
                CustomerNumber = current.CustomerNumber;
                AccessKey = current.AccessKey;
                UserName = current.Name;
            }
            catch (Exception ex)
            {
                ex.ShowDialog();
                ExceptionHandler.HandleException(ex);
            }
        }

        #endregion

        #region Public Property

        public HeadQuarter HeadQuarter { get; set; }

        public bool EditMode { get; set; }

        public bool IsSaved { get; set; }

        /// <summary>
        /// window title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// header content
        /// </summary>
        public string HeadContent { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string CustomerNumber
        {
            get
            {
                return customerNumber;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    customerNumber = value.Trim();
                RaisePropertyChanged("CustomerNumber");
            }
        }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    userName = value.Trim();
                RaisePropertyChanged("UserName");
            }
        }

        /// <summary>
        /// User AccessKey
        /// </summary>
        public string AccessKey
        {
            get
            {
                return accessKey;
            }
            set
            {
                accessKey = value;
                RaisePropertyChanged("AccessKey");
            }
        }

        /// <summary>
        /// Contains the FactoryType detail
        /// </summary>
        public string SelectedSubsidiaryType
        {
            get { return subsidiaryType; }
            set
            {
                subsidiaryType = value;
                RaisePropertyChanged("SelectedSubsidiaryType");
            }
        }

        /// <summary>
        /// Contains list of FactoryType i.e TPICorp or FactoryFloor
        /// </summary>
        public List<string> SubsidiaryTypes
        {
            get { return subsidiaryTypes; }
            set
            {
                subsidiaryTypes = value;
                RaisePropertyChanged("SubsidiaryTypes");
            }
        }

        /// <summary>
        /// Command used on clicking save button
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new DelegateCommand(Save, () => { return !isSaving; });
                }
                return saveCommand;
            }
        }

        /// <summary>
        /// Command used on clicking cancel button
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

        /// <summary>
        /// Command used on clicking generate button
        /// </summary>
        public ICommand GenerateCommand
        {
            get
            {
                if (generateCommand == null)
                {
                    generateCommand = new DelegateCommand(GeneratePassword);
                }
                return generateCommand;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Save or Edit the HeadQuarter to DB
        /// </summary>
        private void Save()
        {
            isSaving = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    Build();
                    string error = ValidateSubsidiary();
                    if (string.IsNullOrEmpty(error))
                    {
                        if (!EditMode)
                        {
                            headQuarterProxy.InsertHeadQuarter(HeadQuarter);
                            MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                                string.Format("HeadQuarter {0} was been added.", HeadQuarter.CustomerNumber));
                        }
                        else
                        {
                            headQuarterProxy.UpdateHeadQuarter(HeadQuarter);
                            MessageLogger.LogOperation(Constant.LoginUser.LoginId,
                                string.Format("HeadQuarter {0} was been updated.", HeadQuarter.CustomerNumber));
                        }
                        IsSaved = true;
                        Dispatch(() => { View.Close(); });
                    }
                    else
                        e.Result = error;
                }
                catch (Exception ex)
                {
                    IsSaved = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex);
                }
                isSaving = false;
            });
        }

        /// <summary>
        /// Close the window
        /// </summary>
        private void Cancel()
        {
            View.Close();
        }

        /// <summary>
        /// Auto Generate the AccessKey
        /// </summary>
        private void GeneratePassword()
        {
            StringBuilder pw = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < passwordLength; i++)
            {
                pw.Append(charRange[r.Next(charRange.Length)]);
            }
            AccessKey = pw.ToString();
        }

        private void Build()
        {
            HeadQuarter.CustomerNumber = customerNumber;
            HeadQuarter.Name= userName;
            HeadQuarter.AccessKey = AccessKey;
            HeadQuarter.HostUrl = "";
        }

        private string ValidateSubsidiary()
        {
            List<HeadQuarter> headQuarters = null;//= headQuarterProxy.GetHeadQuarters();

            if (string.IsNullOrEmpty(HeadQuarter.CustomerNumber) || string.IsNullOrEmpty(HeadQuarter.Name))
                return Resources.TpiConfigurationViewModel_SubsidiaryNotEmpty;
            if (!EditMode)
            {
                if (HeadQuarter.CustomerNumber.Length > maxCustomerNumberLength)
                    return Resources.TpiConfigurationViewModel_CustomerIDLengthOver;

                if (headQuarters.Count >= maxSubsidiaryCount)
                    return Resources.TpiConfigurationViewModel_SubsidiaryCount;

                if (headQuarters.Any(s => s.CustomerNumber == HeadQuarter.CustomerNumber))
                    return Resources.TpiConfigurationViewModel_SubsidiaryDuplicate;
            }
            else
            {
                if (headQuarters.Any(s => (s.CustomerNumber == HeadQuarter.CustomerNumber || s.Name == HeadQuarter.Name) && s.Id != HeadQuarter.Id))
                    return Resources.TpiConfigurationViewModel_SubsidiaryDuplicate;
            }
            return null;
        }

        #endregion
    }
}
