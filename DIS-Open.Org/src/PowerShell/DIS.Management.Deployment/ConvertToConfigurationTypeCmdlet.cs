using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DIS.Management.Deployment.Model;
using DISConfigurationCloud.Contract;

namespace DIS.Management.Deployment
{
    [Cmdlet("Convert", "ToConfigurationType")]
    public class ConvertToConfigurationTypeCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The value of type DIS.Management.Deployment.Model.InstallationType to be converted. ")]
        public InstallationType Value { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            switch (this.Value)
            {
                case InstallationType.Oem:
                    this.WriteObject(ConfigurationType.OEM);
                    break;
                case InstallationType.Tpi:
                    this.WriteObject(ConfigurationType.TPI);
                    break;
                case InstallationType.FactoryFloor:
                    this.WriteObject(ConfigurationType.FactoryFloor);
                    break;
                case InstallationType.Cloud:
                    this.WriteObject("Cloud");
                    break;
                default:
                    break;
            }
        }
    }
}
