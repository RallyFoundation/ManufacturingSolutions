using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management.Automation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace PowerShellDataProcessing
{
    [Cmdlet(VerbsCommon.New, "ObjectFromJson")]
    public class NewObjectFromJsonCommand : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "The content of the JSON to create the object.")]
        public string JsonString { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            string jsonValue = JsonConvert.ToString(JsonString);

            //PSObject jsonObject = JsonConvert.DeserializeObject(jsonValue, typeof(PSObject)) as PSObject;

            //this.WriteObject(jsonObject);

            jsonValue = jsonValue.Substring(jsonValue.IndexOf("{"));
            jsonValue = jsonValue.Substring(0, (jsonValue.LastIndexOf("}") + 1));

            JsonSerializer serializer = new JsonSerializer();
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonValue));
            Object jsonObject = serializer.Deserialize(reader);

            //if (jsonObject != null)
            {
                StringWriter stringWriter = new StringWriter();
                JsonTextWriter writer = new JsonTextWriter(stringWriter);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                writer.IndentChar = ' ';
                serializer.Serialize(writer, jsonObject);

                writer.Flush();

                string result = stringWriter.ToString();

                writer.Close();

                result = result.Substring(result.IndexOf("{"));
                result = result.Substring(0, (result.LastIndexOf("}") + 1));

                
                this.WriteObject(result);
            }

            //this.WriteObject(jsonObject);
        }
    }
}
