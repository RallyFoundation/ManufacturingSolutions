using DllComponent;
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
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

namespace Wifi
{
    public partial class TestWifi : Form
    {
        private static ResourceManager LocRM;
        string[] _APList;
        int iSignalSpec = 50;
        int iIfConnection = 0;
        int iWiFiCount = 0;
        int[] WifiListSignal = new int[64];
        string[] APScan = new string[64];
        /// <summary>
        /// Initializes a new instance of the TestWifi form class.
        /// </summary>
        /// <param name="WiFiPara">WiFi AP lists and signal strength SPEC will need to be searched.</param>
        public TestWifi(string[] WiFiPara)
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly); // Set language
            InitializeComponent();
            SetString();
            if (WiFiPara != null && WiFiPara.Length >= 2)
            {
                if (WiFiPara[1].Length > 0) // For WiFi scanable name
                {
                    char[] tok = new Char[] { ',' };                                    // Split the AP list by ','
                    _APList = WiFiPara[1].Split(tok, StringSplitOptions.RemoveEmptyEntries); // Query WiFi informarion from Dll.
                    Log.LogComment(DllLog.Log.LogLevel.Info, "WiFi input para: " + WiFiPara[1]);
                }
                if (WiFiPara.Length >= 3)
                {
                    if (WiFiPara[2].Length > 0) // Wifi signal spec
                        iSignalSpec = Int32.Parse(WiFiPara[2].ToString(), CultureInfo.InvariantCulture);
                    Log.LogComment(DllLog.Log.LogLevel.Info, "WiFi input para: " + WiFiPara[2]);
                    if (WiFiPara[3].Length > 0) // Wifi if connection
                        iIfConnection = Int32.Parse(WiFiPara[3].ToString(), CultureInfo.InvariantCulture);
                    Log.LogComment(DllLog.Log.LogLevel.Info, "WiFi input para: " + WiFiPara[3]);
                }
            }

        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Pass button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(0);
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Fail button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
        }

        private void TestWiFi_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            WifiInformation(_APList, iIfConnection);
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("Wifi") + LocRM.GetString("Test");
            this.Text = LocRM.GetString("Wifi");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            RetryBtn.Text = LocRM.GetString("Retry");
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Retry button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void RetryBtn_Click(object sender, EventArgs e)
        {
            
            iWiFiCount = 0;
            WifiInfoGrid.Rows.Clear();
            WifiInformation(_APList, iIfConnection);
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        /// <param name="APList">WiFi AP list will need to be researched and signal strength SPEC. 
        /// If users configure the WiFi AP list and signal strength SPEC on SFTConfig.xml, this test will be auto-judge pass/fail.</param>
        public void WifiInformation(string[] APList, int iIfConnection)
        {
            string WiFiInfo;
            string ConnectAPSSID;
            bool bFlag = false;
            int APCheckCount = 0;
            CoreComponent component = null;
            try
            {
                component = new CoreComponent();
                if (APList == null)
                {  
                    ConnectAPSSID = null;
                }
                else
                    ConnectAPSSID = APList[0];

                Log.LogComment(DllLog.Log.LogLevel.Info, "The SSID for wifi connection:" + ConnectAPSSID);
                WiFiInfo = component.TestWiFi(ConnectAPSSID, iIfConnection);
                if (WiFiInfo.IndexOf("WiFi_Connect_Fail", StringComparison.Ordinal) >= 0)
                {
                    APCheckCount = 999;
                    Log.LogComment(DllLog.Log.LogLevel.Error, "The SSID for wifi connection status : Fail");
                }
                    
                    
            }
            finally
            {
                if (component != null)
                {
                    component.Dispose();
                    component = null;
                }
            }

            char[] tok = new Char[] { '\n' ,','};
            string[] split = WiFiInfo.Split(tok, StringSplitOptions.RemoveEmptyEntries); // Query WiFi informarion from Dll.
            // Parser the scaned AP and signal into array
            for (int i = 0; i < split.Length; i++) 
            {
                APScan[iWiFiCount] = split[i];
                i++;
                WifiListSignal[iWiFiCount] = Int32.Parse(split[i].ToString(), CultureInfo.InvariantCulture);
                iWiFiCount++;
            }
            // Add wifi signal amd SSID into dataview object
            for (int i = 0; i < iWiFiCount; i++)   
            {
                DataGridViewRowCollection rows = WifiInfoGrid.Rows;
                rows.Add(new Object[] { WifiListSignal[i], APScan[i] });
                Log.LogComment(DllLog.Log.LogLevel.Info,"Signal = " + WifiListSignal[i] + " , SSID = " + APScan[i]);
            }
            
            if (APList != null)
            {
                for (int k = 0; k < APList.Length; k++)
                {
                    for (int i = 0; i < iWiFiCount; i++)
                    {
                        if (APScan[i].IndexOf(APList[k], StringComparison.Ordinal) >= 0 && WifiListSignal[i] > iSignalSpec)
                        {
                            APCheckCount++;
                            bFlag = true;
                            break;
                        }
                    }
                }
                if (bFlag && APCheckCount == APList.Length)  // All listed AP need to be searched.
                    Program.ExitApplication(0);
                else
                    Program.ExitApplication(255);
            }


        }
    }
}


