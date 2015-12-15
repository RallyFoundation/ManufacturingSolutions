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
using DIS.Presentation.KMT.ExportKeysView;
using DIS.Presentation.KMT.ViewModel;

namespace DIS.Presentation.KMT.Views.Key.ExportKeysView
{
    public class ExportWizard : StepWizard
    {
        public new ExportKeysViewModel VM { get; private set; }

        public ExportWizard(IKeyProxy keyProxy, IConfigProxy configProxy, IHeadQuarterProxy hqProxy, ISubsidiaryProxy ssProxy)
        {
            VM = new ExportKeysViewModel(keyProxy, configProxy, hqProxy,ssProxy);
            VM.View = this;

            VM.StepPages.Add(new TypeSelectPageView(VM));
            InitViewModel(VM);
        }
    }
}
