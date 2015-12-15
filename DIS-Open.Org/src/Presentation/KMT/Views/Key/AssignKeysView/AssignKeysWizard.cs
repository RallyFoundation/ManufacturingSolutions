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
using DIS.Presentation.KMT.AssignKeysView;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Key.AssignKeysView
{
    public class AssignKeysWizard : StepWizard
    {
        public new AssignKeysViewModel VM { get; private set;}

        public AssignKeysWizard(IKeyProxy keyProxy,ISubsidiaryProxy subProxy)
        {
            VM = new AssignKeysViewModel(keyProxy,subProxy);
            VM.View = this;

            VM.StepPages.Add(new AssignKeysSelection(VM));
            VM.StepPages.Add(new AssignKeysSummary(VM));
            InitViewModel(VM);
        }
    }
}
