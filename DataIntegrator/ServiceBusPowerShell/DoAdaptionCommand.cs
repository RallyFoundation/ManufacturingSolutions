using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using DataIntegrator;

namespace ServiceBusPowerShell
{
    [Cmdlet("Do", "Adaption")]
    public class DoAdaptionCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The name of the adapter.")]
        public string AdapterName { get; set; }

        [Parameter(Position = 1, Mandatory = false, HelpMessage = "Parameters that the adapter requires for adaption.", ValueFromPipeline = true)]
        public object[] AdapterParameters { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            Manager manager = new Manager();

            IAdapter adapter = manager.GetAdapter(this.AdapterName);

            object result = adapter.Adapt(this.AdapterParameters);

            this.WriteObject(result);
        }
    }
}
