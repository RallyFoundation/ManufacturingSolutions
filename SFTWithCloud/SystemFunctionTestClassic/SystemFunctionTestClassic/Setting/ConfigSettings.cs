using DllLog;
//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.IO;
using System.Xml;

namespace win81FactoryTest.Setting
{

    /// <summary>
    /// ConfigSettings: Reads data from SFTCnfig.xml file
    /// </summary>
    static class ConfigSettings
    {

        /// <summary>
        /// Gets language code from Config file
        /// </summary>
        public static string GetLang()
        {
            string xmlPath = Program.TestSettingsFile;
            string result = "en-US";

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode langNode = xmlDoc.SelectSingleNode(@"/FactoryTest/Lang");
                if (langNode != null)
                {
                    result = langNode.InnerText;
                } 
            }
            catch (IOException)
            {
                Log.LogError("GetLang: Cannot read XML file: " + xmlPath);
            }
            catch (Exception e)
            {
                Log.LogError("GetLang: "  + e.ToString());
            }
            return result;
        }

        /// <summary>
        /// Gets all phases from Config file
        /// </summary>
        public static string[] GetAllPhase()
        {
            string xmlPath = Program.TestSettingsFile;
            string[] result = new string[0];

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNodeList phaseList = xmlDoc.SelectNodes(@"/FactoryTest/Phase");
                result = new string[phaseList.Count];
                for (int i = 0; i < phaseList.Count; i++)
                {
                    result[i] = phaseList[i].Attributes["Name"].Value;
                }
            }
            catch (IOException)
            {
                Log.LogError("GetAllPhase: Cannot read XML file: " + xmlPath);
            }
            catch (Exception e)
            {
                Log.LogError("GetAllPhase: " + e.ToString());
            }
            return result;
        }

        /// <summary>
        /// Gets all test item from given phase [phaseName]
        /// </summary>
        public static string[] GetTestSettingPhase(string phaseName) {

            string xmlPath = Program.TestSettingsFile;
            string[] result = new string[0];
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNodeList phaseList = xmlDoc.SelectNodes(@"/FactoryTest/Phase");
                for (int i = 0; i< phaseList.Count; i++) {
                    if (phaseList[i].Attributes["Name"].Value.Equals(phaseName))
                    {
                        XmlNodeList menuNode = phaseList[i].SelectNodes(@"TestMenu/TestItem");
                        result = new string[menuNode.Count];
                        for (int j = 0; j < menuNode.Count; j++)
                        {
                            result[j] = menuNode[j].Attributes["Name"].Value;
                        }
                    }
                }
            }
            catch (IOException)
            {
                Log.LogError("GetTestSettingPhase: Cannot read XML file: " + xmlPath);
            }
            catch (Exception e)
            {
                Log.LogError("GetTestSettingPhase: " + e.ToString());
            }
            return result;
        }

        /// <summary>
        /// Gets test argument from given test name [testName]
        /// </summary>
        public static string GetTestArguments(string testName)
        {
            string xmlPath = Program.TestSettingsFile;
            string result = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode settingNode = xmlDoc.SelectSingleNode(@"/FactoryTest/TestSettings/" + testName);
                if (settingNode != null)
                {
                    result = settingNode.InnerText;
                }
            }
            catch (IOException)
            {
                Log.LogError("GetTestArguments: Cannot read XML file: " + xmlPath);

            }
            catch (Exception e)
            {
                Log.LogError("GetTestArguments: " + e.ToString());
            }

            string lang = GetLang();
            if (lang.Length < 1)
            {
                lang = "en-US";
            }
            return lang + " " + result;
        }

        /// <summary>
        /// Gets location path to save results xml file
        /// </summary>
        public static string GetResultPath()
        {
            string xmlPath = Program.TestSettingsFile;
            string result = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode settingNode = xmlDoc.SelectSingleNode(@"/FactoryTest/TestSettings/ResultFile");
                result = settingNode.InnerText;
            }
            catch (IOException)
            {
                Log.LogError("GetResultPath: Cannot read XML file: " + xmlPath);
            }
            catch (Exception e)
            {
                Log.LogError("GetResultPath: " + e.ToString());
            }

            return result;
        }

        /// <summary>
        /// Gets location path to the execution file of test [testName]
        /// </summary>
        public static string GetTestExes(string testName)
        {
            string xmlPath = Program.TestSettingsFile;
            string result = string.Empty;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode settingNode = xmlDoc.SelectSingleNode(@"/FactoryTest/TestPath/" + testName);
                result = settingNode.InnerText;
            }
            catch (IOException)
            {
                Log.LogError("GetTestExes: Cannot read XML file: " + xmlPath);
            }
            catch (Exception e)
            {
                Log.LogError("GetTestExes: " + e.ToString());
            }
            return result;
        }
    }
}
