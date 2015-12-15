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
using DIS.Business.Library;
using DIS.Business.Proxy;
using DIS.Common.Utility;
using DIS.Data.DataContract;
using DIS.Presentation.KMT.Commands;
using DIS.Presentation.KMT.Properties;
using Microsoft.ServiceModel.Web;
using System.Linq;

namespace DIS.Presentation.KMT.ViewModel
{
    public class AutoDiagnoseEventArgs
    {
        private bool isAutoDiagnose;
        public bool IsAutoDiagnose
        {
            get { return this.isAutoDiagnose; }
            set { this.isAutoDiagnose = value; }
        }

        public AutoDiagnoseEventArgs(bool isAuto)
        {
            this.isAutoDiagnose = isAuto;
        }
    }

    public class DiagnosticViewModel : ViewModelBase
    {
        private object resultLock = new object();
        private IConfigProxy configProxy;

        #region Binding fields

        private bool isBusy;
        private string result;
        private bool isAutoDiagnostic;
        private Subsidiary selectSubsidiary;
        private ObservableCollection<Subsidiary> subsidiaries;

        #endregion

        #region Commands

        private DelegateCommand testInternalCommand;
        private DelegateCommand testUpLevelSystemCommand;
        private DelegateCommand testDownLevelSystemCommand;
        private DelegateCommand testMicrosoftCommand;
        private DelegateCommand copyCommand;
        private DelegateCommand exitCommand;
        private DelegateCommand autoDiagnoseChangedCommand;

        public event AutoDiagnoseEventHandler AutoDiagnoseChanged;

        public ICommand AutoDiagnoseChangedCommand
        {
            get
            {
                if (autoDiagnoseChangedCommand == null)
                    autoDiagnoseChangedCommand = new DelegateCommand(this.UpdateAutoDiagnosticSwitch);
                return autoDiagnoseChangedCommand;
            }
        }

        public ICommand TestInternalCommand
        {
            get
            {
                if (testInternalCommand == null)
                    testInternalCommand = new DelegateCommand(TestInternal);
                return testInternalCommand;
            }
        }

        public ICommand TestUpLevelSystemCommand
        {
            get
            {
                if (testUpLevelSystemCommand == null)
                    testUpLevelSystemCommand = new DelegateCommand(TestUpLevelSystem);
                return testUpLevelSystemCommand;
            }
        }

        public ICommand TestDownLevelSystemCommand
        {
            get
            {
                if (testDownLevelSystemCommand == null)
                    testDownLevelSystemCommand = new DelegateCommand(TestDownLevelSystem);
                return testDownLevelSystemCommand;
            }
        }

        public ICommand TestMicrosoftCommand
        {
            get
            {
                if (testMicrosoftCommand == null)
                    testMicrosoftCommand = new DelegateCommand(TestMicrosoft);
                return testMicrosoftCommand;
            }
        }

        public ICommand CopyCommand
        {
            get
            {
                if (copyCommand == null)
                    copyCommand = new DelegateCommand(Copy, () => { return !string.IsNullOrEmpty(Result); });
                return copyCommand;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new DelegateCommand(Exit);
                return exitCommand;
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBtnEnble");
                RaisePropertyChanged("IsBusy");
            }
        }

        public bool IsAutoDiagnostic
        {
            get { return this.isAutoDiagnostic; }
            set
            {
                this.isAutoDiagnostic = value;
                RaisePropertyChanged("IsAutoDiagnostic");
            }
        }

        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }

        public bool IsBtnEnble
        {
            get { return !IsBusy; }
        }

        public ObservableCollection<Subsidiary> Subsidiaries
        {
            get { return subsidiaries; }
            set
            {
                subsidiaries = value;
                RaisePropertyChanged("Subsidiaries");
            }
        }

        public Subsidiary SelectedSubsidiary
        {
            get { return this.selectSubsidiary; }
            set
            {
                this.selectSubsidiary = value;
                RaisePropertyChanged("SelectedSubsidiary");
            }
        }

        public Visibility UpLevelSystemTestVisibility
        {
            get { return KmtConstants.IsOemCorp ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility MicrosoftTestVisibility
        {
            get { return KmtConstants.IsFactoryFloor || (KmtConstants.CurrentHeadQuarter != null && KmtConstants.CurrentHeadQuarter.IsCentralizedMode) ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility DLSTestingVisibility
        {
            get { return !KmtConstants.IsFactoryFloor ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        public DiagnosticViewModel(IConfigProxy configProxy, ISubsidiaryProxy ssProxy)
        {
            this.configProxy = configProxy;
            this.IsAutoDiagnostic = configProxy.GetIsAutoDiagnostic();
            this.Subsidiaries = new ObservableCollection<Subsidiary>(ssProxy.GetSubsidiaries());
            this.IsBusy = false;
            this.SelectedSubsidiary = this.Subsidiaries.FirstOrDefault();
        }

        #region Methods

        private void TestInternal()
        {
            RunTest(() =>
            {
                AppendResult(MergedResources.Diagnostic_TestingInternal);
                return configProxy.TestInternalConnection();
            });
        }

        private void TestUpLevelSystem()
        {
            RunTest(() =>
            {
                AppendResult(MergedResources.Diagnostic_TestingULS);
                if (KmtConstants.CurrentHeadQuarter == null)
                    throw new DisException(MergedResources.Diagnostic_NoHeadQuarter);
                return configProxy.TestUpLevelSystemConnection(KmtConstants.CurrentHeadQuarter.HeadQuarterId);
            });
        }

        private void TestDownLevelSystem()
        {
            RunTest(() =>
            {
                AppendResult(ResourcesOfR6.Diagnostic_TestingDLS);
                if (this.SelectedSubsidiary == null)
                    throw new DisException(ResourcesOfR6.DiagnosticVM_SSIdNotEmpty);
                return configProxy.TestDownLevelSystemConnection(this.SelectedSubsidiary.SsId);
            });
        }

        private void TestMicrosoft()
        {
            RunTest(() =>
            {
                AppendResult(MergedResources.Diagnostic_TestingMicrosoft);
                if (!configProxy.GetIsMsServiceEnabled())
                    throw new DisException(MergedResources.KeyManagementViewModel_FulfillmentDisableMsService);

                return configProxy.TestMsConnection(KmtConstants.CurrentHeadQuarter == null ? (int?)null : KmtConstants.CurrentHeadQuarter.HeadQuarterId);
            });
        }

        private void Copy()
        {
            Clipboard.SetText(Result);
        }

        private void Exit()
        {
            View.Close();
        }

        private void RunTest(Func<DiagnosticResult> action)
        {
            Result = string.Empty;
            IsBusy = true;
            WorkInBackground((s, e) =>
            {
                try
                {
                    DiagnosticResult result = action();
                    if (result.DiagnosticResultType == DiagnosticResultType.Ok)
                    {
                        AppendResult(MergedResources.Common_Success);
                    }
                    else
                    {
                        throw result.Exception;
                    }
                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    AppendResult(MergedResources.Common_Failed);
                    if (ex is WebProtocolException)
                        AppendResult(((WebProtocolException)ex).StatusCode.ToString());
                    AppendResult(ex.Message);
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
            });
        }

        private void AppendResult(string s)
        {
            lock (resultLock)
            {
                if (string.IsNullOrEmpty(Result))
                    Result = s;
                else
                    Result = string.Format("{0}{2}{2}{1}", Result, s, Environment.NewLine);
            }
        }

        private void UpdateAutoDiagnosticSwitch()
        {
            WorkInBackground((s, e) =>
            {
                try
                {
                    configProxy.UpdateAutoDiagnosticSwitch(this.IsAutoDiagnostic);
                    if (AutoDiagnoseChanged != null)
                        AutoDiagnoseChanged(this, new AutoDiagnoseEventArgs(this.IsAutoDiagnostic));
                }
                catch (Exception ex)
                {
                    ex.ShowDialog();
                    ExceptionHandler.HandleException(ex, KmtConstants.CurrentDBConnectionString);
                }
            });
        }

        #endregion
    }
}
