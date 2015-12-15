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
using DIS.Presentation.KMT.ImportKeysView;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Key.ImportKeysView
{
    public class ImportWizard : StepWizard
    {
        public new ImportKeysViewModel VM { get; private set; }

        public ImportWizard(IKeyProxy keyProxy, ISubsidiaryProxy ssProxy, IConfigProxy configProxy, IHeadQuarterProxy hqProxy)
        {
            VM = new ImportKeysViewModel(keyProxy, ssProxy, configProxy, hqProxy);
            VM.View = this;

            VM.StepPages.Add(new TypeSelectPage(VM));
            VM.StepPages.Add(new ImportLocationSelectPage(VM));
            VM.StepPages.Add(new ImportResultPage(VM));
            InitViewModel(VM);
        }
    }
}
