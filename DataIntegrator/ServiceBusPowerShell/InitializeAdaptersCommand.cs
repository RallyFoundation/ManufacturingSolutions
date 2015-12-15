using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using DataIntegrator;

namespace ServiceBusPowerShell
{
    [Cmdlet("Init", "Adapters")]
    public class InitializeAdaptersCommand : Cmdlet
    {
        [Parameter(Position=0, Mandatory=true, HelpMessage="The physical location where the adapter configuration files are located. It can be a file system directory.")]
        public string MountPoint { get; set; }

        [Parameter(Position=1, Mandatory=true, HelpMessage="The pattern which the adapter configuration files look like.")]
        public string SearchPattern { get; set; }

        [Parameter(Position=2, Mandatory = false, HelpMessage = "The encoding name of the adapter configuration file.")]
        public string EncodingName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();
            Manager manager = new Manager();
            manager.InitializeAdapters(this.MountPoint, this.SearchPattern, this.EncodingName);

            Console.WriteLine("Adapters initialized.");
        }
    }
}
