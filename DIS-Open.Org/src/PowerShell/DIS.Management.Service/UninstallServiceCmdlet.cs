using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;

namespace DIS.Management.Process
{
    [Cmdlet("Uninstall", "Service")]
    public class UninstallServiceCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The unique name of the service.")]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            ServiceInstaller.Uninstall(this.Name);

            this.WriteObject((ServiceInstaller.ServiceIsInstalled(this.Name) == false));
        }
    }
}
