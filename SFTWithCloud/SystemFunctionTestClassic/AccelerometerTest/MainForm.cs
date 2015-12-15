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
using System.Resources;
using System.Windows.Forms;
using Windows.Devices.Sensors;


namespace AccelerometerTest
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
            InitializeAccelerometer();
            SetString();

            Label.CheckForIllegalCrossThreadCalls = false;
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Landscape;
        }

        #endregion // Constructor

        /// <summary>
        /// Initialize Accelerometer timer and update Accelerometer every 500ms
        /// </summary>
        private void InitializeAccelerometer()
        {
            //Set Timer to update Accelerometer
            _timer.Interval = 500;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateAccelerometer);
            _timer.Start();
        }

        /// <summary>
        /// Update Accelerometer readings event and update UI
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void UpdateAccelerometer(object sender, EventArgs e)
        {
            try
            {
                Accelerometer accelero = Accelerometer.GetDefault();
                if (accelero != null)
                {
                    XPanel.Text = LocRM.GetString("XAxis") + ": " + 
                        String.Format("{0,5:0.00}", accelero.GetCurrentReading().AccelerationX) + "G";
                    YPanel.Text = LocRM.GetString("YAxis") + ": " + 
                        String.Format("{0,5:0.00}", accelero.GetCurrentReading().AccelerationY) + "G";
                    ZPanel.Text = LocRM.GetString("ZAxis") + ": " +
                        String.Format("{0,5:0.00}", accelero.GetCurrentReading().AccelerationZ) + "G";

                    //set the x,y of red panel
                    double x = Math.Min(1, accelero.GetCurrentReading().AccelerationX);
                    double y = Math.Min(1, accelero.GetCurrentReading().AccelerationY);
                    double square = x * x + y * y;
                    if (square > 1)
                    {
                        x /= Math.Sqrt(square);
                        y /= Math.Sqrt(square);
                    }
                    double locationX = Math.Max(0, 100 + 100 * x);
                    locationX = Math.Min(200, 100 + 100 * x);
                    double locationY = Math.Max(0, 100 - 100 * y);
                    locationY = Math.Min(200, 100 - 100 * y);
                    this.RedPanel.Location = new Point((int) locationX, (int) locationY);
                }
                else
                {
                    XPanel.Text = LocRM.GetString("NotFound");
                    _timer.Stop();
                }
            }
            catch (Exception ex)
            {
                _timer.Stop();
                DllLog.Log.LogError(ex.ToString());
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
            Title.Text = LocRM.GetString("Accelerometer") + LocRM.GetString("Test");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
        }


    }
}
