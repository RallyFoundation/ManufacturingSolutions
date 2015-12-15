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
using DIS.Presentation.KMT.RecallKeysView;
using DIS.Presentation.KMT.ViewModel.Key;

namespace DIS.Presentation.KMT.Views.Key.RecallKeysView
{
    public class RecallKeysWizard : StepWizard
    {
        public new RecallKeysViewModel VM { get; private set; }

        public RecallKeysWizard(IKeyProxy keyProxy)
        {
            VM = new RecallKeysViewModel(keyProxy);
            VM.View = this;
            VM.StepPages.Add(new RecallKeysSelection(VM));
            VM.StepPages.Add(new RecallKeysSummary(VM));
            InitViewModel(VM);
        }
    }
}
