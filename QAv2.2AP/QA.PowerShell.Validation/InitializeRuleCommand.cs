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
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The path to the default rule configuratoin file.")]
        public string DefaultRulePath { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "The path to the user rule configuratoin file.")]
        public string UserRulePath { get; set; }


        protected override void ProcessRecord()
        {
            if (!String.IsNullOrEmpty(DefaultRulePath))
            {
                Global.DefaultRuleConfigPath = DefaultRulePath;
            }

            if (!String.IsNullOrEmpty(UserRulePath))
            {
                Global.UserRuleConfigPath = UserRulePath;
            }

            Facade.Facade.InitializeRules();

            this.WriteObject(Facade.Facade.Rules);
        }
    }
}
