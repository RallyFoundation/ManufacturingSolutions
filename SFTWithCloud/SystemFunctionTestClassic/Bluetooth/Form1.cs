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


namespace Bluetooth
{
    public partial class TestBluetooth : Form
    {
        private static ResourceManager LocRM;
        int iCount = 0;
        int TestDuration = 5;
        string[] _BTPara;
        /// <summary>
        ///  It is the main function for scanning BT devices and will call the TestBT function in the DllComponent.
        /// </summary>
        /// <param name="BTPara">BT device names list will need to be scaned.</param>   
        public TestBluetooth(string[] BTPara)
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            InitializeComponent();
            SetString();
            if (BTPara != null && BTPara.Length >= 2)
            {
                if (BTPara[1].Length > 0) // For BT scanable name
                {
                    char[] tok = new Char[] { ',' };
                    _BTPara = BTPara[1].ToString().Split(tok, StringSplitOptions.RemoveEmptyEntries); // Query BT informarion from Dll.
                    Log.LogComment(DllLog.Log.LogLevel.Info, "BT input para: " + BTPara[1]);
                }
                if (BTPara.Length >= 3)
                {
                    if (BTPara[2].Length > 0) // BT scan delay time 
                        TestDuration = Int32.Parse(BTPara[2].ToString(), CultureInfo.InvariantCulture);

                    Log.LogComment(DllLog.Log.LogLevel.Info, "BT input para: " + BTPara[2]);
                }

            }
            PassBtn.Visible = false;
            FailBtn.Visible = false;
            RetryBtn.Visible = false;

                      
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
        /// <summary>
        /// Control.Click Event handler. Where control is the Retry button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void RetryBtn_Click(object sender, EventArgs e)
        {
            PassBtn.Visible = false;
            FailBtn.Visible = false;
            iCount +=1;
            string szTemp = "===== Retry " + iCount + "=====\n";
            this.BTInFoList.Items.Add(szTemp);
            Application.DoEvents();
            BTScan(_BTPara);
            PassBtn.Visible = true;
            FailBtn.Visible = true;
        }

        private void TestBluetooth_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            
            this.BTInFoList.Items.Add("========BT Scanning ========");
            Application.DoEvents();
            BTScan(_BTPara);

            PassBtn.Visible = true;
            FailBtn.Visible = true;
            RetryBtn.Visible = true;
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("Bluetooth") + LocRM.GetString("Test");
            this.Text = LocRM.GetString("Bluetooth");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            RetryBtn.Text = LocRM.GetString("Retry");
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        /// <param name="BTPara">BT device names list will need to be scaned. 
        /// If users configure the BT devices list and scan time on SFTConfig.xml, this test will be auto-judge pass/fail.</param>
        private void BTScan(string[] BTPara)
        {
            string BTInfo;
            bool bFlag = false;
            CoreComponent component = null;

            try
            {
                component = new CoreComponent();
                BTInfo = component.TestBT(TestDuration);
            }
            finally
            {
                if (component != null)
                {
                    component.Dispose();
                    component = null;
                }
            }

            char[] tok = new Char[] { '\n' };
            string[] split = BTInfo.Split(tok, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < split.Length; i++)
            {
                this.BTInFoList.Items.Add(split[i]);
                this.BTInFoList.Items.Add("\n");
                Log.LogComment(DllLog.Log.LogLevel.Info, "Scaned BT device: " + split[i]);
            }

            if (BTPara != null) // If BTlist para is not empty, test item will auto-judge pass/fail
            {
                for (int i = 0; i < BTPara.Length; i++)
                {
                    if (BTInfo.IndexOf(BTPara[i], StringComparison.Ordinal) >= 0)
                        bFlag = true;
                    else
                    {
                        bFlag = false;
                        break;
                    }
                }
                if (bFlag)
                   Program.ExitApplication(0);
                else
                   Program.ExitApplication(255);
             }

        }

    }
}
