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
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Models;
using DIS.Presentation.KMT.Properties;

namespace DIS.Presentation.KMT.ViewModel {
    /// <summary>
    /// 
    /// </summary>
    public class ExportLogsViewModel : ViewModelBase {
        private ILogProxy logProxy;
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();

        private bool isBusy;
        private DateTime? from;
        private DateTime? to;
        private bool shouldDeleteFromDb;
        private CategoryModel selectedCategory;
        private string exportPath;

        private ICommand browseCommand;
        private ICommand exportCommand;
        private ICommand cancelCommand;

        /// <summary>
        /// 
        /// </summary>
        public ExportLogsViewModel(ILogProxy logProxy) {
            this.logProxy = logProxy;
            Categories = logProxy.GetCategories().Select(c => new CategoryModel() { Category = c }).ToList();
            Categories.Sort();
            SelectedCategory = Categories.FirstOrDefault();
            From = To = DateTime.Today;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CategoryModel> Categories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy {
            get { return this.isBusy; }
            set {
                this.isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? From {
            get { return from; }
            set {
                from = value;
                RaisePropertyChanged("From");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? To {
            get { return to; }
            set {
                to = value;
                RaisePropertyChanged("To");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShouldDeleteFromDb {
            get { return shouldDeleteFromDb; }
            set {
                shouldDeleteFromDb = value;
                RaisePropertyChanged("ShouldDeleteFromDb");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CategoryModel SelectedCategory {
            get { return selectedCategory; }
            set {
                selectedCategory = value;
                RaisePropertyChanged("SelectedCategory");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ExportPath {
            get { return exportPath; }
            set {
                exportPath = value;
                RaisePropertyChanged("ExportPath");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ExportCommand {
            get {
                if (exportCommand == null)
                    exportCommand = new DelegateCommand(() => { ExportLogs(); });
                return exportCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand BrowseCommand {
            get {
                if (browseCommand == null)
                    browseCommand = new DelegateCommand(() => { ChooseExportPath(); });
                return browseCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CancelCommand {
            get {
                if (cancelCommand == null)
                    cancelCommand = new DelegateCommand(() => {
                        View.Close();
                    });
                return cancelCommand;
            }
        }

        public void CancelExporting()
        {
            cancellationToken.Cancel();
            IsBusy = false;
        }

        private void ExportLogs() {
            if (ValidationHelper.ValidateDateRange(From, To)) {
                if (!this.ValidateDirectory())
                    return;
                IsBusy = true;
                WorkInBackground((s, e) => {
                    try {
                        int total = logProxy.ExportLogs(SelectedCategory.Category.CategoryName, From, To, ShouldDeleteFromDb, ExportPath, cancellationToken);
                        
                        if (!cancellationToken.IsCancellationRequested) {
                            Message msg = new Message();
                            if (total == 0) {
                                msg.Title = MergedResources.Common_Message;
                                msg.Content = MergedResources.NoLogsFound;
                            }
                            else {
                                msg.Title = MergedResources.Common_Success;
                                msg.Content = MergedResources.ExportLogsSucceed;
                                MessageLogger.LogOperation(KmtConstants.LoginUser.LoginId, 
                                    string.Format("{0} {1} logs(s) have been exported to {2}.",
                                        total, SelectedCategory.Category.CategoryName.ToLower(), ExportPath), KmtConstants.CurrentDBConnectionString);
                            }
                            e.Result = msg;
                            IsBusy = false;
                        }
                    }
                    catch (Exception ex) {
                        IsBusy = false;
                        ex.ShowDialog();
                        ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                    }
                });
            }
        }

        private bool ValidateDirectory() {
            try {
                if (string.IsNullOrEmpty(ExportPath)) {
                    ValidationHelper.ShowMessageBox(MergedResources.Common_SelectDirectory, MergedResources.Common_Error);
                    return false;
                }

                if (!Directory.Exists(ExportPath)) {
                    DirectoryInfo dir = Directory.CreateDirectory(ExportPath);
                    ExportPath = dir.FullName;
                }
            }
            catch (UnauthorizedAccessException) {
                ValidationHelper.ShowMessageBox(MergedResources.AccessFileDenied, MergedResources.Common_Error);
                return false;
            }
            catch (Exception) {
                ValidationHelper.ShowMessageBox(MergedResources.Export_InvalidPath, MergedResources.Common_Error);
                return false;
            }

            return true;
        }

        private void ChooseExportPath() {
            using (FolderBrowserDialog fbdDes = new FolderBrowserDialog()) {
                fbdDes.RootFolder = Environment.SpecialFolder.MyComputer;
                fbdDes.SelectedPath = ExportPath;
                fbdDes.ShowNewFolderButton = true;
                if (fbdDes.ShowDialog() == DialogResult.OK)
                    ExportPath = fbdDes.SelectedPath;
            }
        }
    }
}
