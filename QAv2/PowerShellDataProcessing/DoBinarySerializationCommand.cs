using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management.Automation;
using QA.Utility;

namespace PowerShellDataProcessing
{
    [Cmdlet("Do", "BinarySerialization")]
    public class DoBinarySerializationCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The instance of the objcet to serialize.")]
        public object ObjectInstance { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The path to the file to hold the serialization output.")]
        public string FilePath { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            byte[] bytes = CommonUtility.BinarySerialize(ObjectInstance);

            using (FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            this.WriteObject("Done!");
        }
    }
}
