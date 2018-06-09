using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Facade;

namespace QA.PowerShell.Validation
{
    [Cmdlet(VerbsCommon.Set, "Data")]
    public class SetDataCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = false, HelpMessage = "The key for the data to add into the pairs.")]
        public string Key { get; set; }
        [Parameter(Position = 1, Mandatory = false, HelpMessage = "The value for the data to add into the pairs.")]
        public object Value { get; set; }

        protected override void ProcessRecord()
        {
            Facade.Facade.SetData(Key, Value);

            this.WriteObject(Facade.Facade.Data);
        }
    }
}
