using DllLog;
using Microsoft.Win32;
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
using System.Globalization;
using System.Windows.Forms;
using win81FactoryTest.Setting;

namespace win81FactoryTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetLanguage();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm());

        }

        #region Fields
        public static RegistryKey SFTRegKey = Registry.CurrentUser.CreateSubKey("SFTClassic");
        public const String TestSettingsFile = @"SFTConfig.xml";
        #endregion //Fields

        /// <summary>
        /// Set application UICulture from TestSettings.GetLang()
        /// </summary>
        private static void SetLanguage()
        {
            string langCode = ConfigSettings.GetLang();
            try 
            {
                CultureInfo newCulture = new CultureInfo(langCode);
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name != newCulture.Name)
                {
                    CultureInfo.DefaultThreadCurrentCulture = newCulture;
                    CultureInfo.DefaultThreadCurrentUICulture = newCulture;
                }
            }
            catch (CultureNotFoundException e)
            {
                Log.LogError("CultureNotFoundException: " + e.ToString());
            }
        }

    }
}
