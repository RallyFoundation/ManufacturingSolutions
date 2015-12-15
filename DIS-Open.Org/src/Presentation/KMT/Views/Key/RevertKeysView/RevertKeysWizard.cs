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
using DIS.Presentation.KMT.RevertKeysView;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Key.RevertKeysView
{
    public class RevertKeysWizard : StepWizard
    {
        public new RevertKeysViewModel VM { get; private set;}
        
        public RevertKeysWizard(IKeyProxy keyProxy,ISubsidiaryProxy ssProxy)
        {
            VM = new RevertKeysViewModel(keyProxy,ssProxy);
            VM.View = this;
            VM.StepPages.Add(new RevertKeysSelection(VM));
            VM.StepPages.Add(new RevertKeysOperateMsgView(VM));
            VM.StepPages.Add(new RevertKeysSummary(VM));
            InitViewModel(VM);
        }
    }
}
