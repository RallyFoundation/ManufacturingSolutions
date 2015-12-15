using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using DataIntegrator;

namespace ServiceBusPowerShell
{
    [Cmdlet(VerbsCommon.Show, "Adapters")]
    public class ListAdaptersCommand : Cmdlet
    {
        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            Manager manager = new Manager();
            string[] adapterNames = manager.ListAdapters();

            this.WriteObject(adapterNames, true);
        }
    }
}
