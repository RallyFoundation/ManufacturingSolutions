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
using System.Management;  // for ManagementObject. You will need to add System.Management into Reference as well.
using System.Resources;
using System.Windows.Forms;

//https://msdn.microsoft.com/en-us/library/aa394074%28v=vs.85%29.aspx

namespace Battery
{
    public partial class Form1 : Form
    {
        private static ResourceManager LocRM;
        System.Timers.Timer _timerChar = new System.Timers.Timer();
        float RemCap = 0;
        float RemCapOri = 0;
        float Charspeed = 0;
        float BatFullCap = 0;
        float BatPercentage = 0;
        float BatSpec = 0;
        TimeSpan Timecurr = new TimeSpan(DateTime.Now.Ticks);

        ManagementObjectSearcher Batsearcher1 = new ManagementObjectSearcher(@"\\localhost\root\wmi", "Select * From BatteryStatus where Voltage > 0"); // The Battery that is used currently.
        ManagementObjectSearcher Batsearcher2 = new ManagementObjectSearcher(@"\\localhost\root\wmi", "Select * From BatteryStatus where Voltage > 0"); // The Battery that is used currently.
        ManagementObjectSearcher BatInfo = new ManagementObjectSearcher(@"\\localhost\root\wmi", "Select * from BatteryFullChargedCapacity"); // The total capacity of the battery
        /// <summary>
        /// Initializes a new instance of the Form1 form class.
        /// </summary>
        public Form1()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            InitializeComponent();
            SetString();
            if (Program.ProgramArgs.Length > 0)
                BatSpec = float.Parse(Program.ProgramArgs[0].ToString(), CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Initialize Battery timer and update Battery every 2000ms
        /// </summary>
        private void InitializeBatteryStatus()
        {
            //Set Timer to update Charging status
            _timerChar.Interval = 2000;
            _timerChar.Elapsed += new System.Timers.ElapsedEventHandler(UpdateChargingRateStatus);
            _timerChar.Start();
        }
        /// <summary>
        /// Update Accelerometer readings event and update UI
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void UpdateChargingRateStatus(object sender, EventArgs e)
        {
            try 
            {
                // Charging Rate
                TimeSpan TimecurrEnd = new TimeSpan(DateTime.Now.Ticks);
                float TimeInterval = (float)TimecurrEnd.Subtract(Timecurr).Duration().TotalSeconds;

                ManagementObjectCollection Batcollection1 = Batsearcher1.Get();
                foreach (ManagementObject Batobj1 in Batcollection1)
                {
                    RemCap = (uint)Batobj1["RemainingCapacity"];
                    this.PwrStatus.Text = (bool)Batobj1["PowerOnline"] ? "Online" : "Offline";
                    this.ChargeStatus.Text = (bool)Batobj1["Charging"] ? "Charging" : "Discharging";
                }
                Batcollection1.Dispose();
                Charspeed = (RemCap - RemCapOri)*3600 / TimeInterval;
                this.CharRate.Text = Charspeed.ToString("0.0000", CultureInfo.InvariantCulture) + " Wh"; 
                BatPercentage = (RemCap / BatFullCap) * 100;
                this.BatCap.Text = BatPercentage.ToString("0.0", CultureInfo.InvariantCulture) + " %";
            }
            catch (Exception ex)
            {
                _timerChar.Stop();
                ErrorLbl.Text = ex.Message;
                DllLog.Log.LogError("UpdateChargingRateStatus: " + ex.Message);

            }
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Pass button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            _timerChar.Stop();
            Program.ExitApplication(0);
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Fail button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            _timerChar.Stop();
            Program.ExitApplication(255);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;

            this.CharRate.Text = "0" + " Wh";
            try
            {
                //Bat FUll capacity
                ManagementObjectCollection BatInfocollection = BatInfo.Get();
                foreach (ManagementObject BatInfoobj in BatInfocollection)
                {
                    BatFullCap = (uint)BatInfoobj["FullChargedCapacity"];
                }
                BatInfocollection.Dispose();
                // Charging Rate
                ManagementObjectCollection Batcollection2 = Batsearcher2.Get();
                foreach (ManagementObject Batobj2 in Batcollection2)
                {
                    RemCapOri = (uint)Batobj2["RemainingCapacity"];
                    this.PwrStatus.Text = (bool)Batobj2["PowerOnline"] ? "Online" : "Offline";
                    this.ChargeStatus.Text = (bool)Batobj2["Charging"] ? "Charging" : "Discharging";
                }
                Batcollection2.Dispose();
                BatPercentage = (RemCapOri / BatFullCap) * 100;
                this.BatCap.Text = BatPercentage.ToString("0.0", CultureInfo.InvariantCulture) + " %";

            }
            catch (Exception ex)
            {
                PassBtn.Visible = false;
                RetryBtn.Visible = false;
                PwrStatus.Text = LocRM.GetString("NotFound");
                ChargeStatus.Text = LocRM.GetString("NotFound");
                BatCap.Text = LocRM.GetString("NotFound");
                ErrorLbl.Text = ex.Message;
                DllLog.Log.LogError(ex.Message);


            }

            if (Program.ProgramArgs.Length > 0)
            {
                if (BatPercentage < BatSpec)
                    Program.ExitApplication(255);
                else
                    Program.ExitApplication(0);

            }
            else
                InitializeBatteryStatus();
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("Battery") + LocRM.GetString("Test");
            this.Text = LocRM.GetString("Battery");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            RetryBtn.Text = LocRM.GetString("Retry");
            BatteryLab.Text = LocRM.GetString("Battery_bat");
            PowerLab.Text = LocRM.GetString("Battery_pwrStatus");
            ChargeLab.Text = LocRM.GetString("Battery_chargeStatus");
            ChargingRateLab.Text = LocRM.GetString("Battery_chargeRate");
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Retry button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void RetryBtn_Click(object sender, EventArgs e)
        {
            _timerChar.Stop();
            TimeSpan TimeTemp = new TimeSpan(DateTime.Now.Ticks);
            Timecurr = TimeTemp;
            // Reset the charging battery information
            this.CharRate.Text = "0 Wh";
            RemCapOri = 0;
            RemCap = 0;
            ManagementObjectCollection Batcollection2 = Batsearcher2.Get();
            foreach (ManagementObject Batobj2 in Batcollection2)
            {
                RemCapOri = (uint)Batobj2["RemainingCapacity"];
            }
            Batcollection2.Dispose();
            InitializeBatteryStatus();
        }


    }

}

