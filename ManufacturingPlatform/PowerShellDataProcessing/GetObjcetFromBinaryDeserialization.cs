using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management.Automation;
using Platform.DAAS.OData.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Get, "ObjcetFromBinaryDeserialization")]
    public class GetObjcetFromBinaryDeserialization : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The path to the file to hold the serialization output.")]
        public string FilePath { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            byte[] bytes = File.ReadAllBytes(FilePath);

            object objectInstance = CommonUtility.BinaryDeserialize(bytes);

            this.WriteObject(objectInstance);
        }
    }
}
