using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using DataIntegrator;

namespace ServiceBusPowerShell
{
    [Cmdlet(VerbsCommon.Get, "Adapter")]
    public class GetAdapterCommand : Cmdlet
    {
        [Parameter(Position=0, Mandatory=true, HelpMessage="The name of the adapter.")]
        public string AdapterName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();
            Manager manager = new Manager();

            IAdapter adapter = manager.GetAdapter(this.AdapterName);

            this.WriteObject(adapter, true);
        }
    }
}
