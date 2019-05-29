using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Facade;

namespace QA.PowerShell.Validation
{
    [Cmdlet(VerbsData.Initialize, "Data")]
    public class InitializeDataCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The path to the data input file.")]
        public string Path { get; set; }

        protected override void ProcessRecord()
        {
            if (!String.IsNullOrEmpty(Path))
            {
                Global.DefaultDataPath = Path;
            }

            Facade.Facade.InstantiateInputData();

            this.WriteObject(Facade.Facade.Data);
        }
    }
}
