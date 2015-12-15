using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DIS.Management.Deployment.Model
{
    [XmlRoot("unattend")]
    public class Installation
    {
        [XmlElement("applicationRoot")]
        public string ApplicationRoot { get; set; }

        [XmlElement("installationType")]
        public InstallationType InstallationType { get; set; }

        [XmlElement("mode")]
        public InstallationMode InstallationMode { get; set; }

        [XmlArray("settings")]
        [XmlArrayItem("component")]
        public Component[] Components { get; set; }

        public Component GetComponent(string Name) 
        {
            if (this.Components != null)
            {
                return this.Components.FirstOrDefault((c) => c.Name.ToLower() == Name.ToLower());
            }

            return null;
        }

        public bool ValidateComponentCloudCachingPolicy(out Component[] conflictingComponents, out DISConfigurationCloud.Client.CachingPolicy cachingPolicy) 
        {
            List<Component> components = null;

            cachingPolicy = DISConfigurationCloud.Client.CachingPolicy.RemoteOnly;

            if (this.Components != null)
            {
                Component[] comps = this.Components.Where((o) => ((o != null) && (o.AppSetting != null))).ToArray();

                if (comps != null)
                {
                    cachingPolicy = comps[0].AppSetting.CloudConfigurationCachingPolicy;

                    components = new List<Component>();

                    for (int i = 0; i < comps.Length; i++)
                    {
                        if (comps[i].AppSetting.CloudConfigurationCachingPolicy != cachingPolicy)
                        {
                            components.Add(comps[i]);
                        }
                    }
                }

                if ((components != null) && (components.Count > 0))
                {
                    conflictingComponents = components.ToArray();

                    return false;
                }
            }

            conflictingComponents = null;
            return true;
        }

        public bool ValidateComponentCloudConnection(out Component[] problemComponents) 
        {
            List<Component> components = null;

            if (this.Components != null)
            {
                components = new List<Component>();

                Component[] comps = this.Components.Where((o) => ((o != null) && (o.AppSetting != null))).ToArray();

                if (comps != null)
                {
                    DISConfigurationCloud.Client.Manager manager = new DISConfigurationCloud.Client.Manager(false, null);

                    string testResult = null;

                    for (int i = 0; i < comps.Length; i++)
                    {
                        DISConfigurationCloud.Client.ModuleConfiguration.ServicePoint = DISConfigurationCloud.Client.ModuleConfiguration.GetServicePoint(comps[i].AppSetting.ConfigurationCloudServerAddress, "/Services/DISConfigurationCloud.svc");
                        DISConfigurationCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = String.Format("{0}:{1}", comps[i].AppSetting.ConfigurationCloudUserName, comps[i].AppSetting.ConfigurationCloudPassword);

                        try
                        {
                            testResult = manager.Test();
                        }
                        catch (Exception ex)
                        {
                            testResult = "Error!";
                        }

                        if (String.IsNullOrEmpty(testResult) || (testResult.ToLower() != "hello!") || (testResult.ToLower() == "error!"))
                        {
                            components.Add(comps[i]);
                        }
                    }
                }

                if ((components != null) && (components.Count > 0))
                {
                    problemComponents = components.ToArray();
                    return false;
                }
            }

            problemComponents = null;
            return true;
        } 
    }

    public enum InstallationType 
    {
        Cloud = 0,
        Oem = 1,
        Tpi = 2,
        FactoryFloor = 4
    }

    public enum InstallationMode 
    {
        Centralized = 0,
        Decentralized = 1
    }
}
