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
    [Cmdlet(VerbsData.Initialize, "Rule")]
    public class InitializeRuleCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The path to the rule configuratoin file.")]
        public string Path { get; set; }


        protected override void ProcessRecord()
        {
            if (!String.IsNullOrEmpty(Path))
            {
                Global.DefaultRuleConfigPath = Path;
            }

            Facade.Facade.InitializeRules();

            this.WriteObject(Facade.Facade.Rules);
        }
    }
}
