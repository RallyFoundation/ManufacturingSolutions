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

using DIS.Business.Proxy;
using DIS.Presentation.KMT.ReportKeysView;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Key.ReportKeysView
{
    public class ReportKeysWizard : StepWizard
    {
        public new ReportKeysViewModel VM { get; private set; }

        public ReportKeysWizard(IKeyProxy keyProxy,IConfigProxy configProxy)
        {
            VM = new ReportKeysViewModel(keyProxy,configProxy);
            VM.View = this;
            VM.StepPages.Add(new ReportKeysSelection(VM));
            VM.StepPages.Add(new ReportKeysSummary(VM));
            InitViewModel(VM);
        }
    }
}
