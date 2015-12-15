using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.IO;
using System.Xml;
using DISConfigurationCloud.Client;
using DISConfigurationCloud.Contract;
using DISConfigurationCloud.Utility;
using DIS.Management.Deployment.Model;

namespace DIS.Management.Deployment
{
    [Cmdlet(VerbsCommon.Get, "InstallationInfo")]
    public class GetInstallationInfoCmdlet : Cmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "FileContent", HelpMessage = "The content of the unattend file.")]
        public string UnattendXml { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "FilePath", HelpMessage = "The path to the unattend file.")]
        public string UnattendFileName { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "FilePath", HelpMessage = "The encoding of the unattend file content.")]
        public Encoding UnattendFileEncoding { get; set; }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();

            XmlDocument xmlDoc = new XmlDocument();

            if (!String.IsNullOrEmpty(this.UnattendXml))
            {
                xmlDoc.LoadXml(this.UnattendXml);
            }
            else if((!String.IsNullOrEmpty(this.UnattendFileName)) && (File.Exists(this.UnattendFileName)))
            {
                using (FileStream stream = new FileStream(this.UnattendFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, this.UnattendFileEncoding))
                    {
                        xmlDoc.Load(reader);
                    }
                }
            }

            XmlUtility xmlUtility = new XmlUtility();

            Type[] types = new Type[] 
            {
                typeof(Component),
                typeof(Component[]),
                typeof(ApplicationServerSetting),
                typeof(DatabaseServerSetting),
                typeof(ApplicationSetting),
                typeof(ApplicationPoolIdentityType),
                typeof(DatabaseAuthenticationMode),
                typeof(InstallationType),
                typeof(InstallationMode),
                typeof(CachingPolicy)
            };

            Installation installation = xmlUtility.XmlDeserialize(xmlDoc.InnerXml, typeof(Installation), types, "utf-8") as Installation;

            this.WriteObject(installation);
        }
    }
}
