using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using DIS.Management.Deployment.Model;
using DISConfigurationCloud.Contract;

namespace DIS.Management.Deployment
{
    [Cmdlet("Convert", "ToInstallationType")]
    public class ConvertToInstallationTypeCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The value of type DISConfigurationCloud.Contract.ConfigurationType to be converted. ")]
        public ConfigurationType Value { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            switch (this.Value)
            {
                case ConfigurationType.OEM:
                    this.WriteObject(InstallationType.Oem);
                    break;
                case ConfigurationType.TPI:
                    this.WriteObject(InstallationType.Tpi);
                    break;
                case ConfigurationType.FactoryFloor:
                    this.WriteObject(InstallationType.FactoryFloor);
                    break;
                default:
                    break;
            }
        }
    }
}
