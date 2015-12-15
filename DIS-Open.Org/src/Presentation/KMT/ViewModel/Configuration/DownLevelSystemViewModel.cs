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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;
using DIS.Presentation.KMT.View.Configuration;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class DownLevelSystemViewModel : ViewModelBase
    {
        #region private fields
        private const int maxSubsidiaryCount = 5;
        private ISubsidiaryProxy ssProxy;
        private Subsidiary selectedSubsidiary;
        private ObservableCollection<Subsidiary> subsidiaries;
        private bool isRemovingSubsidiary = false;
        private ICommand addSubsidiaryCommand;
        private ICommand editSubsidiaryCommand;
        private ICommand removeSubsidiaryCommand;
        private bool isChanged = false;

        #endregion

        #region public constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssProxy"></param>
        public DownLevelSystemViewModel(ISubsidiaryProxy ssProxy)
        {
            this.ssProxy = ssProxy;
            LoadSubsidiaries();
        }

        #endregion constructor

        #region public property

        /// <summary>
        /// Contains the Subsidiary detail
        /// </summary>
        public Subsidiary SelectedSubsidiary
        {
            get
            {
                return selectedSubsidiary;
            }
            set
            {
                selectedSubsidiary = value;
                RaisePropertyChanged("SelectedSubsidiary");
            }
        }

        /// <summary>
        /// Contains list of FactoryType i.e TPICorp or FactoryFloor
        /// </summary>
        public ObservableCollection<Subsidiary> Subsidiaries
        {
            get { return subsidiaries; }
            set
            {
                subsidiaries = value;
                RaisePropertyChanged("Subsidiaries");
            }
        }

        /// <summary>
        /// Command used on clicking Add button
        /// </summary>
        public ICommand AddSubsidiaryCommand
        {
            get
            {
                if (addSubsidiaryCommand == null)
                {
                    addSubsidiaryCommand = new DelegateCommand(AddSubsidiary);
                }
                return addSubsidiaryCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsChanged
        {
            get { return this.isChanged; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSaved { get { return true; } }

        /// <summary>
        /// Command used on clicking Edit button
        /// </summary>
        public ICommand EditSubsidiaryCommand
        {
            get
            {
                if (editSubsidiaryCommand == null)
                {
                    editSubsidiaryCommand = new DelegateCommand(
                        () => { EditSubsidiary(); }, () => { return SelectedSubsidiary != null; });
                }
                return editSubsidiaryCommand;
            }
        }

        /// <summary>
        /// Command used on clicking Remove button
        /// </summary>
        public ICommand RemoveSubsidiaryCommand
        {
            get
            {
                if (removeSubsidiaryCommand == null)
                {
                    removeSubsidiaryCommand = new DelegateCommand(
                            () => { RemoveSubsidiary(); }, () => { return (SelectedSubsidiary != null) && !isRemovingSubsidiary; });
                }
                return removeSubsidiaryCommand;
            }
        }

        #endregion

        #region private method

        private void LoadSubsidiaries()
        {
            Subsidiaries = new ObservableCollection<Subsidiary>(ssProxy.GetSubsidiaries());
        }

        /// <summary>
        /// Add or edit Subsidiary
        /// </summary>
        private void AddSubsidiary()
        {
            //Removing the 5-Subsidiary limit - Rally
            //if (KmtConstants.IsOemCorp && subsidiaries.Count >= maxSubsidiaryCount)
            //{
            //    ValidationHelper.ShowMessageBox(MergedResources.SubsidaryEditorViewModel_SubsidiaryCount, MergedResources.Common_Error);
            //    return;
            //}
            Window parent = GetCurrentWindow();
            SubsidiaryEditor editor = new SubsidiaryEditor(ssProxy);
            editor.Owner = parent;
            editor.ShowDialog();
            if (editor.VM.IsSaved)
            {
                LoadSubsidiaries();
                isChanged = true;
            }
        }

        /// <summary>
        /// Edit the selected Subsidiary from list
        /// </summary>
        private void EditSubsidiary()
        {
            Window parent = GetCurrentWindow();
            SubsidiaryEditor editor = new SubsidiaryEditor(ssProxy, SelectedSubsidiary);
            editor.Owner = parent;
            editor.ShowDialog();
            if (editor.VM.IsSaved)
            {
                LoadSubsidiaries();
                isChanged = true;
            }
        }

        /// <summary>
        /// Remove the selected Subsidiary from list
        /// </summary>
        private void RemoveSubsidiary()
        {
            System.Windows.MessageBoxResult r = System.Windows.MessageBox.Show(MergedResources.DLSViewModel_Cancel,
              MergedResources.Common_Confirmation, System.Windows.MessageBoxButton.YesNo);
            if (r != System.Windows.MessageBoxResult.Yes)
                return;
            isRemovingSubsidiary = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    ssProxy.DeleteSubsidiary(SelectedSubsidiary.SsId);
                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId,
                        string.Format("Down Level System {0} has been removed.", SelectedSubsidiary.DisplayName), KmtConstants.CurrentDBConnectionString);
                    LoadSubsidiaries();
                    isRemovingSubsidiary = false;
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    isRemovingSubsidiary = false;
                    ex.ShowDialog(MergedResources.ConfigViewModel_DeleteSubSidiary);
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
            });
        }

        #endregion private method
    }
}
