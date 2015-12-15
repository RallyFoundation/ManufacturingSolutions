//*********************************************************
//
// Copyright (c) Microsoft 2011. All rights reserved.
// This code is licensed under your Microsoft OEM Services support
//    services description or work order.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Web.Configuration;

namespace DIS.Common.Utility
{
    /// <summary>
    /// Provides configuration section of encryption
    /// </summary>
    public static class ProtectConfigurationSection
    {
        private const string customProtectionProvider = "X509ProtectedConfigProvider";
        private const string webConfigFileName = "Web.config";
        private const string connectionStringName = "KeyStoreContext";

        /// <summary>
        /// Encrypt connection string in config file
        /// </summary>
        /// <param name="config"></param>
        /// <param name="connectionString"></param>
        public static void ProtectConfigFile(Configuration config, string connectionString)
        {
            ConnectionStringsSection section = config.ConnectionStrings;
            if (section != null)
            {
                if (section.ConnectionStrings[connectionStringName] != null)
                {
                    section.ConnectionStrings[connectionStringName].ConnectionString = connectionString;
                    section.SectionInformation.ProtectSection(customProtectionProvider);
                    section.SectionInformation.ForceSave = true;
                    config.Save(ConfigurationSaveMode.Minimal);
                }
            }
        }

        /// <summary>
        /// Encrypt connection string in config file
        /// </summary>
        /// <param name="config"></param>
        public static void ProtectConfigFile(Configuration config)
        {
            ConfigurationSection section = config.ConnectionStrings;
            if (section != null)
            {
                if (!section.SectionInformation.IsProtected)
                {
                    if (!section.ElementInformation.IsLocked)
                    {
                        section.SectionInformation.ProtectSection(customProtectionProvider);
                        section.SectionInformation.ForceSave = true;
                        config.Save(ConfigurationSaveMode.Minimal);
                    }
                }
            }
        }

        /// <summary>
        /// Return the config file's type.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>
        /// true: is web.config
        /// false: is app.config
        /// </returns>
        public static bool IsWebConfiguration(string fileName)
        {
            return string.Compare(webConfigFileName,Path.GetFileName(fileName), true) == 0 ? true : false;
        }

        /// <summary>
        /// Open the app.config file 
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static Configuration OpenExeConfiguration(string configPath)
        {
            string exePath = Path.ChangeExtension(configPath, null);
            return ConfigurationManager.OpenExeConfiguration(exePath);
        }

        /// <summary>
        /// Open the web.config file
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static Configuration OpenWebConfiguration(string configPath)
        {
            VirtualDirectoryMapping vdm = new VirtualDirectoryMapping(Path.GetDirectoryName(configPath), true);
            WebConfigurationFileMap wcfm = new WebConfigurationFileMap();
            wcfm.VirtualDirectories.Add("/", vdm);
            return WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");
        }
    }
}
