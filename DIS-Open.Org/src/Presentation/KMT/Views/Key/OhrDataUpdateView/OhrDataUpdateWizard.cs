using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIS.Business.Proxy;
using DIS.Presentation.KMT.ViewModel;
using DIS.Presentation.KMT.OhrDataUpdateView;

namespace DIS.Presentation.KMT.Views.Key.OhrDataUpdateView
{
    public class OhrDataUpdateWizard : StepWizard
    {
        public new OhrDataUpdateViewModel VM { get; private set;}

        public OhrDataUpdateWizard(IKeyProxy keyProxy)
        {
            VM = new OhrDataUpdateViewModel(keyProxy);
            VM.View = this;

            VM.StepPages.Add(new OhrKeysEdit(VM));
            VM.StepPages.Add(new OhrKeysSummary(VM));
            InitViewModel(VM);
        }
    }
}
