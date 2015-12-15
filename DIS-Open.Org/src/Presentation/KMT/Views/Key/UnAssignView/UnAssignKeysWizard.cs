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
using DIS.Presentation.KMT.UnAssignView;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Key.UnAssignKeysView
{
    public class UnAssignKeysWizard : StepWizard
    {
        public new UnAssignKeysViewModel VM { get; private set; }

        public UnAssignKeysWizard(IKeyProxy keyProxy, ISubsidiaryProxy subProxy)
        {
            VM = new UnAssignKeysViewModel(keyProxy, subProxy);
            VM.View = this;
            VM.StepPages.Add(new UnAssignTargetSelect(VM));
            VM.StepPages.Add(new UnAssignkeysSelection(VM));
            VM.StepPages.Add(new UnAssignkeysSummary(VM));
            InitViewModel(VM);
        }
    }
}
