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
using System.Resources;
using System.Windows.Forms;
using Windows.Devices.Sensors;

namespace OrientationTest
{
    public partial class MainForm : Form
    {
        #region Fields

        private static ResourceManager LocRM;
        System.Timers.Timer _timer = new System.Timers.Timer();

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            InitializeComponent();
            InitializaOrientation();
            SetString();
            Label.CheckForIllegalCrossThreadCalls = false;
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion //Constructor

        /// <summary>
        /// Initialize orientation timer and update orientation every 500ms
        /// </summary>
        private void InitializaOrientation()
        {
           //Set Timer to update orientation 
            _timer.Interval = 500;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateOrientation);
            _timer.Start();
        }

        /// <summary>
        /// Update orientation readings event and update UI
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void UpdateOrientation(object sender, EventArgs e)
        {
            try
            {
                OrientationSensor orientation = OrientationSensor.GetDefault();
                if (orientation != null)
                {
                    SensorQuaternion quaternion = orientation.GetCurrentReading().Quaternion;
                    XLabel.Text = LocRM.GetString("XAxis") + ": " + String.Format("{0,8:0.00000}", quaternion.X);
                    YLabel.Text = LocRM.GetString("YAxis") + ": " + String.Format("{0,8:0.00000}", quaternion.Y);
                    ZLabel.Text = LocRM.GetString("ZAxis") + ": " + String.Format("{0,8:0.00000}", quaternion.Z);
                    WLabel.Text = LocRM.GetString("WAxis") + ": " + String.Format("{0,8:0.00000}", quaternion.W);
                }
                else
                {
                    XLabel.Text = LocRM.GetString("NotFound");
                    _timer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                _timer.Stop();
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
            Title.Text = LocRM.GetString("Orientation") + LocRM.GetString("Test");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
        }
    }
}
