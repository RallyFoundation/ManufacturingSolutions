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
        public ValidationRuleItem RuleItem { get; set; }
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            Facade.Facade.AddRule(RuleItem);
        }
    }
}
