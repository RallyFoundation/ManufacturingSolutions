using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management.Automation;
using Newtonsoft.Json;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.Get, "JsonFromObject")]
    public class GetJsonFromObjectCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The instance of object to convert to json.")]
        public object ObjectInstance { get; set; }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "The name of the type of object to convert to json.")]
        public string TypeName { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            Type type = Type.GetType(TypeName);

            string result = "";

            JsonSerializer serializer = new JsonSerializer();

            using (StringWriter stringWriter = new StringWriter())
            {
                using (JsonTextWriter writer = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(stringWriter, ObjectInstance, type);
                }

                result = stringWriter.ToString();
            }

            this.WriteObject(result);
        }
    }
}
