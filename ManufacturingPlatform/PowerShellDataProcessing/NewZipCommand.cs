﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Platform.DAAS.OData.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.New, "Zip")]
    public class NewZipCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The array containing the paths to the files to be zipped.")]
        public string[] FilesToZip { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path to the file as the zip destination.")]
        public string ZippedFilePath { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            CommonUtility.CreateZip(FilesToZip, ZippedFilePath);

            this.WriteObject("Done.");
        }
    }
}
