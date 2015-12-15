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

namespace InclinometerTest
{
    public partial class MainForm : Form
    {
        #region Fields

        private static ResourceManager LocRM;
        System.Timers.Timer _timer = new System.Timers.Timer();

        #endregion //Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            InitializeComponent();
            InitializeInclinoMeter();
            SetString();
            Label.CheckForIllegalCrossThreadCalls = false;
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion //Constructor


        /// <summary>
        /// Initialize InclinoMeter timer and update Accelerometer every 500ms
        /// </summary>
        private void InitializeInclinoMeter()
        {
            //Set Timer to update Accelerometer
            _timer.Interval = 500;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateInclinoMeter);
            _timer.Start();

        }

        /// <summary>
        /// Update InclinoMeter readings event and update UI
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void UpdateInclinoMeter(object sender, EventArgs e)
        {
            try
            {
                Inclinometer inclino = Inclinometer.GetDefault();
                if (inclino != null)
                {
                    PitchLbl.Text = LocRM.GetString("XDegree") + ": " +
                        String.Format("{0,5:0.00}", inclino.GetCurrentReading().PitchDegrees.ToString()) + " (°)";
                    RollLbl.Text = LocRM.GetString("YDegree") + ": " +
                        String.Format("{0,5:0.00}", inclino.GetCurrentReading().RollDegrees.ToString()) + " (°)";
                    YawLbl.Text = LocRM.GetString("ZDegree") + ": " +
                        String.Format("{0,5:0.00}", inclino.GetCurrentReading().YawDegrees.ToString()) + " (°)";
                }
                else
                {
                    PitchLbl.Text = LocRM.GetString("NotFound");

                    _timer.Stop();
                }
                
            }
            catch (Exception ex)
            {
                _timer.Stop();
                PitchLbl.Text = LocRM.GetString("Error");
                Log.LogError(ex.ToString());
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
            Title.Text = LocRM.GetString("Inclinometer") + LocRM.GetString("Test");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
        }

    }
}
