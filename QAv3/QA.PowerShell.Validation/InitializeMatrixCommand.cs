using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Facade;
using QA.Core;

namespace QA.PowerShell.Validation
{
    [Cmdlet(VerbsData.Initialize, "Matrix")]
    public class InitializeMatrixCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The path to the default matrix configuratoin file.")]
        public string DefaultMatrixPath { get; set; }
        protected override void ProcessRecord()
        {
            if (!String.IsNullOrEmpty(DefaultMatrixPath))
            {
                Global.DefaultMatrixConfigPath = DefaultMatrixPath;
            }

            Facade.Facade.InitializeMatrix();

            this.WriteObject(Facade.Facade.Rules);
        }
    }
}
