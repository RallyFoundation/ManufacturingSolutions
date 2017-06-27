using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Platform.DAAS.OData.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Open, "Zip")]
    public class OpenZipCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The path to the zip file as the source for the extraction.")]
        public string ZippedFilePath { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path to the destination location for the extraction.")]
        public string Destination { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            CommonUtility.ExtractZip(ZippedFilePath, Destination);

            this.WriteObject("Done.");
        }
    }
}
