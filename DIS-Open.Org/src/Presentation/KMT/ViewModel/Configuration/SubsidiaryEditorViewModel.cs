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
using System.Linq;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Behaviors;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// View Model class for Create SubSidiary View
    /// </summary>
    public class SubsidiaryEditorViewModel : ViewModelBase
    {
        #region Private Members

        private const int accessKeyLength = 10;

        private ISubsidiaryProxy ssProxy;
        private List<string> subsidiaryTypes;
        private bool isSaving = false;

        private ICommand saveCommand;
        private ICommand cancelCommand;
        private ICommand generateCommand;

        #region DLS config

        private string dlsName;
        private string type;
        private string host;
        private string port;
        private string userName;
        private string accessKey;
        private string description;

        #endregion

        private bool isBusy;
        private bool editMode;

        #endregion

        #region Construct

        /// <summary>
        /// public constrcutor
        /// </summary>
        /// <param name="dispatcher">Dispatcher instance to send updates to DIS.Presentation.KMT. This should be passed null from automated unit tests.</param>
        public SubsidiaryEditorViewModel(ISubsidiaryProxy ssProxy, Subsidiary ss)
        {
            this.ssProxy = ssProxy;

            SubsidiaryTypes = new List<string>();
            
            SubsidiaryTypes.Add(EnumHelper.GetFieldDecription(InstallType.FactoryFloor.GetType(), InstallType.FactoryFloor.ToString()));
            if (KmtConstants.IsOemCorp)
                SubsidiaryTypes.Insert(0, EnumHelper.GetFieldDecription(InstallType.Tpi.GetType(), InstallType.Tpi.ToString()));

            if (ss == null)
            {
                Subsidiary = new Subsidiary();

                EditMode = false;
                WndTitle = MergedResources.CreateSubSidiaryViewModel_Title_New;
            }
            else
            {
                Subsidiary = ss;

                EditMode = true;
                WndTitle = MergedResources.DLSEditorView_Edit;
            }
            Initialize(Subsidiary);
        }

        #endregion

        #region Public Property

        /// <summary>
        /// indicate if background worker is end
        /// </summary>
        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                this.isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public bool EditMode
        {
            get { return this.editMode; }
            set
            {
                this.editMode = value;
                RaisePropertyChanged("EditMode");
            }
        }

        public bool IsSaved { get; set; }

        public string WndTitle { get; set; }

        public Subsidiary Subsidiary { get; set; }

        public string DLSName
        {
            get
            {
                return dlsName;
            }
            set
            {
                dlsName = value;
                RaisePropertyChanged("DLSName");
            }
        }

        public string DLSType
        {
            get { return type; }
            set
            {
                type = value;
                RaisePropertyChanged("DLSType");
            }
        }

        public string Host
        {
            get { return host; }
            set
            {
                host = value;
                RaisePropertyChanged("Host");
            }
        }

        public string Port
        {
            get { return this.port; }
            set
            {
                port = value;
                RaisePropertyChanged("Port");
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }

        /// <summary>
        /// access DLS UserName
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        /// <summary>
        ///access DLS AccessKey
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
        /// Contains TPICorp and FactoryFloor
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
                    generateCommand = new DelegateCommand(GenerateAccessKey);
                }
                return generateCommand;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize UI according selected subsidiary
        /// </summary>
        /// <param name="current"></param>
        private void Initialize(Subsidiary current)
        {
            try
            {
                DLSName = current.DisplayName;
                DLSType = current.Type;
                if (!string.IsNullOrEmpty(current.ServiceHostUrl))
                {
                    Uri serviceUrl = new Uri(current.ServiceHostUrl);
                    Host = serviceUrl.Host;
                    Port = serviceUrl.Port.ToString(); ;
                }
                UserName = current.UserName;
                AccessKey = current.AccessKey;
                Description = current.Description;

                if (EditMode == false)
                {
                    DLSType = SubsidiaryTypes[0];
                    this.GenerateAccessKey();
                }
            }
            catch (Exception ex)
            {
                ex.ShowDialog();
                ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
            }
        }

        /// <summary>
        /// Save or Edit the Subsidiary to DB
        /// </summary>
        private void Save()
        {
            if (!Build())
                return;

            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    if (!EditMode)
                    {
                        ssProxy.InsertSubsidiary(Subsidiary);
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Down Level System {0} has been added.", Subsidiary.DisplayName), KmtConstants.CurrentDBConnectionString);
                    }
                    else
                    {
                        ssProxy.UpdateSubsidiary(Subsidiary);
                        MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                            string.Format("Down Level System {0} has been updated.", Subsidiary.DisplayName), KmtConstants.CurrentDBConnectionString);
                    }
                    IsSaved = true;
                    IsBusy = false;
                    Dispatch(() => { Cancel(); });
                }
                catch (Exception ex)
                {
                    IsSaved = false;
                    IsBusy = false;
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
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
        /// generate access key randomly
        /// </summary>
        private void GenerateAccessKey()
        {
            AccessKey = HashHelper.GenerateRandomString(accessKeyLength);
        }

        private bool Build()
        {
            Subsidiary.DisplayName = this.DLSName == null ? null : this.DLSName.Trim();
            Subsidiary.Description = Description == null ? Description : Description.Trim();
            Subsidiary.UserName = this.UserName == null ? null : this.UserName.Trim();
            Subsidiary.AccessKey = AccessKey;
            Subsidiary.Type = DLSType;
           
            string error = ValidateSubsidiary();
            if (error != null)
            {
                ValidationHelper.ShowMessageBox(error, MergedResources.Common_Error);
                return false;
            }

            if (!string.IsNullOrEmpty(Host) || !string.IsNullOrEmpty(Port))
            {
                string serviceHostUrl = ValidationHelper.GetWebServiceUrl(Host, Port);
                if (serviceHostUrl != null)
                {                    
                    Subsidiary.ServiceHostUrl = serviceHostUrl;
                }
                else
                    return false;
            }
            else
            {               
                Subsidiary.ServiceHostUrl = String.Empty;
            }
            return true;
        }

        private string ValidateSubsidiary()
        {
            List<Subsidiary> subsidiaries = ssProxy.GetSubsidiaries();

            if (string.IsNullOrEmpty(Subsidiary.DisplayName))
                return ResourcesOfR6.DLSVM_DLSNameNotEmpty;
            if (subsidiaries.Any(ss => ss.DisplayName == Subsidiary.DisplayName && ss.SsId != Subsidiary.SsId))
                return ResourcesOfR6.DLSVM_DLSNameExists;
            if (subsidiaries.Any(ss => ss.UserName == Subsidiary.UserName && ss.SsId != Subsidiary.SsId))
                return ResourcesOfR6.DLSVM_UserNameExists;
            return null;
        }

        #endregion
    }
}
