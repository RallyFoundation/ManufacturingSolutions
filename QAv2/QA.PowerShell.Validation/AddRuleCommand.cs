using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Facade;
using QA.Model;

namespace QA.PowerShell.Validation
{
    [Cmdlet(VerbsCommon.Add, "Rule")]
    public class AddRuleCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The rule instance to be added to the rule pairs.")]
        public ValidationRuleItem RuleItem { get; set; }

        protected override void ProcessRecord()
        {
            Facade.Facade.AddRule(RuleItem);

            this.WriteObject(Facade.Facade.Rules);
        }
    }
}
