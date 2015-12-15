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
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;
using Microsoft.Win32;

namespace DIS.Presentation.KMT.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportDuplicateCBRNotificationViewModel : ViewModelBase
    {
        private IKeyProxy keyProxy = null;
        private ObservableCollection<CbrKey> cBRCollention = null;
        private string fileName = Path.Combine(Directory.GetCurrentDirectory(),string.Format("Keys_{0:yyyy_MM_dd_hh_mm_ss}.xml", DateTime.Now));
        private Cbr allCBR = null;
        private bool isExportChecked = false;

        private ICommand browseCommand;
        private ICommand executeCommand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duplicateCBRs"></param>
        /// <param name="keyProxy"></param>
        public ExportDuplicateCBRNotificationViewModel(Cbr duplicateCBRs, IKeyProxy keyProxy)
        {
            this.keyProxy = keyProxy;
            this.allCBR = duplicateCBRs;
            this.CBRCollection = new ObservableCollection<CbrKey>(allCBR.CbrKeys);
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<CbrKey> CBRCollection
        {
            get { return cBRCollention; }
            set
            {
                cBRCollention = value;
                RaisePropertyChanged("CBRCollection");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExportChecked
        {
            get { return isExportChecked; }
            set
            {
                isExportChecked = value;
                RaisePropertyChanged("IsExportChecked");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand BrowseCommand
        {
            get
            {
                if (browseCommand == null)
                    browseCommand = new DelegateCommand(() => { ChooseLocation(); });
                return browseCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ExecuteCommand
        {
            get
            {
                if (executeCommand == null)
                    executeCommand = new DelegateCommand(() => { Execute(); });
                return executeCommand;
            }
        }

        private void ChooseLocation()
        {
            SaveFileDialog exportSaveFileDialog = new SaveFileDialog();
            exportSaveFileDialog.FileName = Path.GetFileName(FileName);
            exportSaveFileDialog.Title = MergedResources.Common_Save;
            exportSaveFileDialog.Filter = MergedResources.Common_XmlFilter;
            if (exportSaveFileDialog.ShowDialog() == true)
                FileName = exportSaveFileDialog.FileName;
        }

        private void Execute()
        {
            if (string.IsNullOrEmpty(this.FileName))
            {
                MessageBox.Show(MergedResources.ExportKeysViewModel_SelectFileMsg, MergedResources.Common_Warning);
                return;
            }
            try
            {

                var exportCbr = allCBR;
                var summaryText = "";

                if (exportCbr.CbrKeys.Count > 0)
                {
                    var results = keyProxy.ExportDuplicatedCbr(exportCbr, this.FileName, KmtConstants.LoginUser.LoginId);
                    summaryText = string.Format(MergedResources.Export_resultMsg, results.Count(k => !k.Failed).ToString(), results.Count(k => k.Failed).ToString());
                    MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, summaryText, KmtConstants.CurrentDBConnectionString);
                }
                else if (IsExportChecked)
                {
                    summaryText = MergedResources.ExportDuplicateCBRNotificationViewModel_ExportCBRAgain;
                }

                var messageResult = MessageBox.Show(summaryText, MergedResources.Common_Information);
                if (messageResult == MessageBoxResult.OK && exportCbr.CbrKeys.Count != 0)
                    this.View.Close();
            }
            catch (Exception ex)
            {
                MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, ex.ToString(), KmtConstants.CurrentDBConnectionString);
                System.Windows.MessageBox.Show(MergedResources.Export_InvalidPath, MergedResources.Common_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
