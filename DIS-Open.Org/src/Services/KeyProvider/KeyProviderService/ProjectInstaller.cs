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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.IO;


namespace DIS.Services.KeyProviderService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        //protected override void OnBeforeInstall(IDictionary savedState)
        //{
        //    base.OnBeforeInstall(savedState);

        //    string path = Assembly.GetExecutingAssembly().Location + ".config";
        //    string name = serviceInstaller.ServiceName;
        //    if (File.Exists(path))
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(path);
        //        string installType = doc.SelectSingleNode("configuration/appSettings/add[@key='ServiceNameSuffix']").Attributes["value"].Value;
        //        name += " - " + installType;
        //    }
        //    serviceInstaller.ServiceName = name;
        //    serviceInstaller.DisplayName = name;
        //}

        public string GetContextParameter(string key)
        {
            string sValue = "";
            try
            {
                sValue = this.Context.Parameters[key].ToString();
            }
            catch
            {
                sValue = "";
            }

            return sValue;
        }

        //protected override void OnBeforeUninstall(IDictionary savedState)
        //{
        //    base.OnBeforeUninstall(savedState);
        //    string path = Assembly.GetExecutingAssembly().Location + ".config";
        //    string name = serviceInstaller.ServiceName;
        //    if (File.Exists(path))
        //    {
        //        Console.WriteLine(path);
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(path);
        //        string installType = doc.SelectSingleNode("configuration/appSettings/add[@key='ServiceNameSuffix']").Attributes["value"].Value;
        //        name += " - " + installType;
        //        Console.WriteLine(name);
        //    }
        //    serviceInstaller.ServiceName = name;
        //    serviceInstaller.DisplayName = name;
        //}
    }
}
