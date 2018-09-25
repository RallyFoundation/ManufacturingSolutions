using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using QA.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.New, "Zip")]
    public class NewZipCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The array containing the paths to the files to be zipped.")]
        public string[] FilesToZip { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path to the file as the zip destination.")]
        public string ZippedFilePath { get; set; }

        [Parameter(Position = 2, Mandatory = false, HelpMessage = "The virtual path in the zip file.")]
        public string VirtualPathInZip { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            CommonUtility.CreateZip(FilesToZip, ZippedFilePath, VirtualPathInZip);

            this.WriteObject("Done.");
        }
    }
}
