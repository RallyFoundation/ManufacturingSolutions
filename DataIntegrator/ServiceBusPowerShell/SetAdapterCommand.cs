using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using DataIntegrator;

namespace ServiceBusPowerShell
{
    [Cmdlet(VerbsCommon.Set, "Adapter")]
    public class SetAdapterCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The name of the adapter.")]
        public string AdapterName { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "The encoding name of the adapter configuration file.")]
        public string EncodingName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            Manager manager = new Manager();

            IAdapter adapter = manager.SetAdapter(this.AdapterName, this.EncodingName);

            this.WriteObject(adapter, true);
        }
    }
}
