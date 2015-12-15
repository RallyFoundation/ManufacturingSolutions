using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using DataIntegrator;

namespace ServiceBusPowerShell
{
    [Cmdlet(VerbsCommon.Set, "DefaultTraceSource")]
    public class SetDefaultTraceSourceCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The name of the default trace source.")]
        public string TraceSourceName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            Manager manager = new Manager();

            string result = manager.SetDefaultTraceSource(this.TraceSourceName);

            this.WriteObject(result, true);
        }
    }
}
