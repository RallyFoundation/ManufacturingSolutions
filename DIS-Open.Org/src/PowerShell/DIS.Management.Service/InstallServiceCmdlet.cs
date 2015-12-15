using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;

namespace DIS.Management.Process
{
    [Cmdlet("Install", "Service")]
    public class InstallServiceCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The unique name of the service.")]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The friendly name of the service.")]
        public string DisplayName { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The path to the executable file of the service.")]
        public string Path { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = "Whether to start the service right after service installation.")]
        public bool IsStartingImmediately { get; set; }

        [Parameter(Position = 4, Mandatory = true, HelpMessage = "The start up type of the service.")]
        public ServiceBootFlag StartupType { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            //ServiceInstaller.InstallAndStart(this.Name, this.DisplayName, this.Path);

            ServiceInstaller.Install(this.Name, this.DisplayName, this.Path, this.IsStartingImmediately, this.StartupType);

            this.WriteObject(ServiceInstaller.ServiceIsInstalled(this.Name));
        }
    }
}
