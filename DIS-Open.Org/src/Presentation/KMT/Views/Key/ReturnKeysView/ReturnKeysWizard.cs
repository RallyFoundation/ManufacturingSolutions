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

namespace DIS.Presentation.KMT.Views.Key.ReturnKeysView
{
    public class ReturnKeysWizard : StepWizard
    {
        public new ReturnKeysViewModel VM { get; private set; }

        public ReturnKeysWizard(IKeyProxy keyProxy)
        {
            VM = new ReturnKeysViewModel(keyProxy);
            VM.View = this;
            VM.StepPages.Add(new ReturnKeysCreditSelect(VM));
            VM.StepPages.Add(new ReturnKeysSelection(VM));
            VM.StepPages.Add(new ReturnKeysSummary(VM));
            InitViewModel(VM);
        }
    }
}
