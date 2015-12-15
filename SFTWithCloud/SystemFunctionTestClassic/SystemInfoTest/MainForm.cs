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
using System.Drawing;
using System.Globalization;
using System.Management;
using System.Resources;
using System.Windows.Forms;

namespace SystemInfoTest
{
    public partial class MainForm : Form
    {
        #region Fields

        private static ResourceManager LocRM;
        private static bool canCompare = false; //if input was given to compare with system

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            InitializeComponent();
            InitializeSysInfo();
            SetString();

            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion // Constructor

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Program.ProgramArgs.Length > 1)
            {
                canCompare = true; //input was given. 
            }

            if (canCompare)
            {
                PassFail(CheckPassFail());
            }
        }



        /// <summary>
        /// Initialize all system info data and Pass/Fail this test
        /// </summary>
        private void InitializeSysInfo()
        {
            BiosLbl.Text = GetBios();
            ProcessorLbl.Text = GetCPU();
            RamLbl.Text = GetRAM();
            CdriveLbl.Text = GetDiskSize();
        }


        /// <summary>
        /// Checks system info. 
        /// Compare system info with argument inputs. If same return True, else False
        /// Return: (Bool) Result
        /// </summary>
        private bool CheckPassFail()
        {
            bool results = true;
            string [] checkList = Program.ProgramArgs;
            
            if (checkList.Length < 5) return false;

            if (checkList[0].Replace(" ", string.Empty) == BiosLbl.Text.Replace(" ", string.Empty))
            {
                BiosLbl.ForeColor = Color.YellowGreen;
            }
            else
            {
                BiosLbl.ForeColor = Color.Crimson;
                Log.LogComment(Log.LogLevel.Warning, "System bios: " + BiosLbl.Text);
                results = false;
            }

            if (checkList[2].Replace(" ", string.Empty) == RamLbl.Text.Replace(" ", string.Empty))
            {
                RamLbl.ForeColor = Color.YellowGreen;
            }
            else
            {
                RamLbl.ForeColor = Color.Crimson;
                Log.LogComment(Log.LogLevel.Warning, "Memory: " + RamLbl.Text);
                results = false;
            }

            if (checkList[3].Replace(" ", string.Empty) == ProcessorLbl.Text.Replace(" ", string.Empty))
            {
                ProcessorLbl.ForeColor = Color.YellowGreen;
            }
            else
            {
                ProcessorLbl.ForeColor = Color.Crimson;
                Log.LogComment(Log.LogLevel.Warning, "Processor: " + ProcessorLbl.Text);
                results = false;
            }

            double driveSize = 0.0;
            double checkDriveSize = 0.0;
            int floatingSize = 0;
            //+- ( floating value * (0.01 ) * driveSize )
            try
            {
                driveSize = Convert.ToDouble(CdriveLbl.Text.Remove(CdriveLbl.Text.Length - 2)); //from system
                checkDriveSize = Convert.ToDouble(checkList[1].Remove(checkList[1].Length - 2)); //from input
                floatingSize = Convert.ToInt32(checkList[4]);

                if ((checkDriveSize >= driveSize - floatingSize * 0.01 * driveSize) && (checkDriveSize <= driveSize + floatingSize * 0.01 * driveSize))
                {
                    CdriveLbl.ForeColor = Color.YellowGreen;
                }
                else
                {
                    CdriveLbl.ForeColor = Color.Crimson;
                    Log.LogComment(Log.LogLevel.Warning, "Disk size: " + CdriveLbl.Text);
                    results = false;
                }
            }
            catch
            {
                //cannot cast to double. return
                CdriveLbl.ForeColor = Color.Crimson;
                results = false;
            }
            
            return results;

        }



        /// <summary>
        /// Gets system Bios info.
        /// Return: (string) Bios info
        /// </summary>
        private static string GetBios()
        {
            string biosManufacturer = "";
            string biosVersion = "";
            DateTime biosReleaseDate = DateTime.Now;

            ManagementObjectSearcher search1 = new ManagementObjectSearcher("select * from Win32_BIOS");

            foreach (ManagementObject obj in search1.Get())
            {
                biosManufacturer = obj["Manufacturer"].ToString();
                biosVersion = obj["SMBIOSBIOSVersion"].ToString();
                string releaseDate = obj["ReleaseDate"].ToString().Substring(0, 8);
                biosReleaseDate = DateTime.ParseExact(releaseDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            string result = string.Format("{0} {1}, {2}", biosManufacturer, biosVersion, biosReleaseDate.ToString("M/dd/yyyy"));
            return result;
        }


        /// <summary>
        /// Gets system CPU info.
        /// Return: (string) CPU info
        /// </summary>
        private static string GetCPU()
        {
            string cpuName = "";

            ManagementObjectSearcher search1 = new ManagementObjectSearcher("select * from Win32_Processor");

            foreach (ManagementObject obj in search1.Get())
            {
                cpuName = obj["Name"].ToString();

            }

            string result = cpuName;
            return result;
        }

        /// <summary>
        /// Gets system RAM info.
        /// Return: (string) RAM info
        /// </summary>
        private static string GetRAM()
        {
            ulong ramCapacity = 0;

            ManagementObjectSearcher search1 = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");

            foreach (ManagementObject obj in search1.Get())
            {
                ramCapacity += (ulong)obj["Capacity"];
            }

            string result = (ramCapacity / (1024 * 1024 * 1024) + "GB").ToString();
            return result;
        }

        /// <summary>
        /// Gets system disk size
        /// Return: (string) system disk size
        /// </summary>
        private static string GetDiskSize()
        {
            ManagementObject disk = new
            ManagementObject(@"Win32_DiskDrive.DeviceID='\\.\PHYSICALDRIVE0'");
            disk.Get();

            string result = ((ulong)disk["Size"]) / (1024 * 1024 * 1024) + "GB";
            return result;
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Pass button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void PassBtn_Click(object sender, EventArgs e)
        {
            PassFail(true);
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Fail button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void FailBtn_Click(object sender, EventArgs e)
        {
            PassFail(false);
        }

        private static void PassFail(bool result) {
            if (result) 
            {
                Program.ExitApplication(0);
            } else {
                Program.ExitApplication(255);
            }

        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("SystemInfo") + LocRM.GetString("Test");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
        }

    }
}
