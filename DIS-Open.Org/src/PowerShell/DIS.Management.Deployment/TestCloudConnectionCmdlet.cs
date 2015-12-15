using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DISConfigurationCloud.Client;

namespace DIS.Management.Deployment
{
    [Cmdlet("Test", "CloudConnection")]
    public class TestCloudConnectionCmdlet : Cmdlet
    {
        [Parameter(Position=0, Mandatory=true, HelpMessage="The service point that the could resides.")]
        public string ServicePoint { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The user name for connecting to the could.")]
        public string UserName { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "The passwrod for connecting to the could.")]
        public string Password { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = DISConfigurationCloud.Client.ModuleConfiguration.GetServicePoint(this.ServicePoint, "/Services/DISConfigurationCloud.svc");

            DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = String.Format("{0}:{1}", this.UserName, this.Password);

            Manager manager = new Manager(false, null);

            string result = manager.Test();

            this.WriteObject(result);
        }
    }
}
