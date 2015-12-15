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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using Windows.Devices.Enumeration;

namespace RemovableDeviceTest
{
    public partial class MainForm : Form
    {
        #region Fields

        private static ResourceManager LocRM;
        private Windows.Devices.Enumeration.DeviceWatcher watcher = null;
        private Dictionary<string, string> deviceTypeList = new Dictionary<string, string>();
        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            InitializeComponent();
            SetString();
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;

            //Initialize device watcher
            watcher = Windows.Devices.Enumeration.DeviceInformation.CreateWatcher(Windows.Devices.Enumeration.DeviceClass.PortableStorageDevice);

            watcher.Added += watcher_Added;
            watcher.Removed += watcher_Removed;
            watcher.EnumerationCompleted += watcher_EnumerationCompleted;
            watcher.Start();
        }
        #endregion // Constructor


        private void watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            if (!sender.Equals(watcher)) return;
            if (Program.ProgramArgs == null) return;
            if (Program.ProgramArgs.Length != deviceTypeList.Count || Program.ProgramArgs.Length == 0) return;
            for (int i = 0; i < deviceTypeList.Count; i++)
            {
                string drive = string.Format("{0}:\\", Program.ProgramArgs[i]);
                if(!deviceTypeList.ContainsKey(drive)) return;
            }
            Log.LogComment(Log.LogLevel.Info, "Input correct: " + string.Join(",", (Program.ProgramArgs)));
            Program.ExitApplication(0);
        }

        private void watcher_Added(Windows.Devices.Enumeration.DeviceWatcher sender, Windows.Devices.Enumeration.DeviceInformation args)
        {
            string name = GetPortableDeviceName(args.Id);
            string type = GetPortableDeviceType(args.Id);
            if (!deviceTypeList.ContainsKey(name)) 
            { 
                deviceTypeList.Add(GetPortableDeviceName(args.Id), GetPortableDeviceType(args.Id));
            }
            this.BeginInvoke((Action)(() =>
            {
                //perform on the UI thread
                PrintDrives();
            }));
        }

        private void watcher_Removed(Windows.Devices.Enumeration.DeviceWatcher sender, Windows.Devices.Enumeration.DeviceInformationUpdate args)
        {
            Debug.WriteLine(args.Id);
            this.BeginInvoke((Action)(() =>
            {
                //perform on the UI thread
                PrintDrives();
            }));
        }

        public static string GetPortableDeviceName(string deviceId)
        {
            try
            {
                Windows.Storage.StorageFolder rootFolder = Windows.Devices.Portable.StorageDevice.FromId(deviceId);
                return rootFolder.Name;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                return "";
            }
        }

        public static string GetPortableDeviceType(string deviceId)
        {
            char[] seperator = { '#' };
            string[] attributes = deviceId.Split(seperator, 4);
            string typeStr = "(UNKNOWN)";

            if (attributes[0] == @"\\?\STORAGE" && attributes[1] == "Volume")
            {
                if (attributes[2] == "_??_SD")
                {
                    typeStr = "SD";
                }
                else if (attributes[2] == "_??_USBSTOR")
                {
                    return "USB_STORAGE";
                }
                else if (attributes[2].StartsWith("{"))
                {
                    return "LOCAL_STORAGE";
                }
            }
            else if (attributes[0] == @"\\?\USB")
            {
                return "USB";
            }
            return typeStr;
        }

        /// <summary>
        /// Get all removeables drivers on system. 
        /// Compares the drive letter from config to the drives found on system. If equal then Pass test, else Fail test
        /// </summary>
        private void PrintDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives(); //get all drives on system
            try
            {
                this.DrivesPanel.Controls.Clear();

                for (int i = 0; i < allDrives.Length; i++)
                {
                    DriveInfo d = allDrives[i];

                    //If this drive is not Removable device, continue to next
                    if (d.DriveType != DriveType.Removable)
                    {
                        continue;
                    }

                    ExternalDrive extDrive; //User Control object - ExternalDrive
                    if (d.IsReady == true)
                    {
                        string driveType = d.DriveType.ToString();
                        if (deviceTypeList.ContainsKey(d.Name))
                        {
                            driveType = deviceTypeList[d.Name];
                        }
                        extDrive = new ExternalDrive(
                           d.Name, d.VolumeLabel, driveType,
                           d.AvailableFreeSpace.ToString(), d.TotalFreeSpace.ToString(), d.TotalSize.ToString());
                        this.DrivesPanel.Controls.Add(extDrive);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
           
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Pass button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void PassBtn_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(0);
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Fail button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void FailBtn_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
        }

        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            TitleLbl.Text = LocRM.GetString("RemovableDevice") + LocRM.GetString("Test");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
        }


    }
}
