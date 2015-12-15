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
using System.Windows.Controls;
using System.Windows.Input;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Views;

namespace DIS.Presentation.KMT.ViewModel.ViewModelBases
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WizardViewModelBase : ViewModelBase
    {
        #region Private Members

        private string windowTitle = string.Empty;

        private bool isPreviousButtonVisible;
        private bool isNextButtonVisible;
        private bool isExecuteButtonVisible;
        private bool isFinishButtonVisible;
        private bool isCancelButtonVisible;
        private bool isViewButtonVisible;

        private bool isExecuteButtonEnable = true;

        private DelegateCommand previousCommand;
        private DelegateCommand nextCommand;
        private DelegateCommand cancelCommand;
        private DelegateCommand finishCommand;
        private DelegateCommand executeCommand;
        private DelegateCommand viewCommand;

        private ObservableCollection<Page> stepPages = null;
        private int currentPageIndex;

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public WizardViewModelBase()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CurrentPageIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                windowTitle = value;
                RaisePropertyChanged("WindowTitle");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Page> StepPages
        {
            get { return stepPages; }
            set
            {
                stepPages = value;
                RaisePropertyChanged("StepPages");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
            set
            {
                currentPageIndex = value;
                if (CurrentPageIndexChanged != null)
                    CurrentPageIndexChanged(this, new EventArgs());
                RaisePropertyChanged("CurrentPageIndex");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPreviousButtonVisible
        {
            get { return isPreviousButtonVisible; }
            set
            {
                isPreviousButtonVisible = value;
                RaisePropertyChanged("IsPreviousButtonVisible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNextButtonVisible
        {
            get { return isNextButtonVisible; }
            set
            {
                isNextButtonVisible = value;
                RaisePropertyChanged("IsNextButtonVisible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExecuteButtonVisible
        {
            get { return isExecuteButtonVisible; }
            set
            {
                isExecuteButtonVisible = value;
                RaisePropertyChanged("IsExecuteButtonVisible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFinishButtonVisible
        {
            get { return isFinishButtonVisible; }
            set
            {
                isFinishButtonVisible = value;
                RaisePropertyChanged("IsFinishButtonVisible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCancelButtonVisible
        {
            get { return isCancelButtonVisible; }
            set
            {
                isCancelButtonVisible = value;
                RaisePropertyChanged("IsCancelButtonVisible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsViewButtonVisible
        {
            get { return isViewButtonVisible; }
            set
            {
                isViewButtonVisible = value;
                RaisePropertyChanged("IsViewButtonVisible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExecuteButtonEnable
        {
            get { return isExecuteButtonEnable; }
            set 
            {
                isExecuteButtonEnable = value;
                RaisePropertyChanged("IsExecuteButtonEnable");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand PreviousCommand
        {
            get
            {
                if (previousCommand == null)
                {
                    previousCommand = new DelegateCommand(GoToPreviousPage);
                }
                return previousCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new DelegateCommand(GoToNextPage);
                }
                return nextCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICommand ExecuteCommand
        {
            get
            {
                if (executeCommand == null)
                {
                    executeCommand = new DelegateCommand(Execute);
                }
                return executeCommand;
            }
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public ICommand FinishCommand
        {
            get
            {
                if (finishCommand == null)
                {
                    finishCommand = new DelegateCommand(Cancel);
                }
                return finishCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICommand ViewCommand
        {
            get
            {
                if (viewCommand == null)
                    viewCommand = new DelegateCommand(() => { ViewKeys(); });
                return viewCommand;
            }
        }

        #endregion

        #region protected Members

        /// <summary>
        ///Initialize view a default action
        /// </summary>
        protected virtual void InitView()
        {
            this.IsExecuteButtonVisible = true;
            this.IsCancelButtonVisible = true;
            this.IsNextButtonVisible = false;
            this.IsPreviousButtonVisible = false;
            this.IsFinishButtonVisible = false;
            this.IsViewButtonVisible = false;
            if (StepPages != null && StepPages.Count > 0)
                this.CurrentPageIndex = 0;
        }

        /// <summary>
        /// View to the next default action
        /// </summary>
        protected virtual void GoToNextPage()
        {
            if (StepPages[CurrentPageIndex].IsLoaded)
            {
                this.IsPreviousButtonVisible = true;
                this.IsExecuteButtonVisible = true;
                this.IsCancelButtonVisible = true;
                this.IsNextButtonVisible = false;
                this.IsFinishButtonVisible = false;
                this.CurrentPageIndex = ++CurrentPageIndex;
            }
        }

        /// <summary>
        /// View to the next final action
        /// </summary>
        protected virtual void GoToFinalPage()
        {
            if (StepPages[CurrentPageIndex].IsLoaded)
            {
                this.IsFinishButtonVisible = true;
                this.IsCancelButtonVisible = false;
                this.IsNextButtonVisible = false;
                this.IsPreviousButtonVisible = false;
                this.IsExecuteButtonVisible = false;
                this.CurrentPageIndex = ++CurrentPageIndex;
            }
        }

        /// <summary>
        /// View to the Previous default action
        /// </summary>
        protected virtual void GoToPreviousPage()
        {
            if (StepPages[CurrentPageIndex].IsLoaded)
            {
                this.IsCancelButtonVisible = true;
                this.IsNextButtonVisible = true;
                this.IsPreviousButtonVisible = false;
                this.IsExecuteButtonVisible = false;
                this.IsFinishButtonVisible = false;
                this.CurrentPageIndex = --CurrentPageIndex;
            }
        }

        /// <summary>
        /// Execute keys,classes should override this and provide a method for the implementation execute keys logic
        /// </summary>
        protected abstract void Execute();

        #endregion;

        #region Private Members
        
        /// <summary>
        /// 
        /// </summary>
        protected virtual void ViewKeys() { }

        private void Cancel()
        {
            RequestClose();
        }

        private void RequestClose()
        {
            View.Close();
        }

        #endregion

    }
}
