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
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Geolocation;

namespace GeolocationTest
{
    public partial class MainForm : Form
    {

        #region Fields

        private static ResourceManager LocRM;
        
        #endregion //Fields

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            InitializeComponent();
            SetString();

            InitializeGeolocation();
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion //Contructor

        /// <summary>
        /// Initialize Geolocation and get position
        /// </summary>
        private async void InitializeGeolocation()
        {
            try
            {
                var locator = new Windows.Devices.Geolocation.Geolocator();
                locator.DesiredAccuracy = PositionAccuracy.High;
                locator.DesiredAccuracyInMeters = 10;
                var task = locator.GetGeopositionAsync();
                while (task.Status == Windows.Foundation.AsyncStatus.Started)
                {
                    await Task.Delay(300);
                }
                Geoposition position = null;
                if (task.Status == Windows.Foundation.AsyncStatus.Completed)
                {
                    position = task.GetResults();
                }
                else 
                {
                    StatusLbl.Text = task.Status.ToString();
                    if (task.Status == Windows.Foundation.AsyncStatus.Error)
                        StatusLbl.Text = StatusLbl.Text + ": "+ task.ErrorCode.Message;
                    return;
                }

                StatusLbl.Text = LocRM.GetString("Status") + ": " + locator.LocationStatus.ToString();
                if (position != null)
                {
                    StatusLbl.Text += " [" + position.Coordinate.PositionSource.ToString() + "]";
                    AccuracyLbl.Text = LocRM.GetString("GeolocationAccuracy") + ": " + position.Coordinate.Accuracy.ToString();
                    LatitudeLbl.Text = LocRM.GetString("GeolocationLatitude") + ": " + position.Coordinate.Point.Position.Latitude.ToString();
                    LongitudeLbl.Text = LocRM.GetString("GeolocationLongitude") + ": " + position.Coordinate.Point.Position.Longitude.ToString();
                }

            }
            catch (Exception e)
            {
                Log.LogError(e.ToString());
                AccuracyLbl.Text = LocRM.GetString("Error");
            }
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
        /// Control.Click Event handler. Where control is the Pass button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void PassBtn_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(0);
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Retry button.
        /// Reset/Retry and restart the test.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            SetString();
            InitializeGeolocation();
        }

        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("GPS") + LocRM.GetString("Test");
            StatusLbl.Text = LocRM.GetString("Wait");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            ResetBtn.Text = LocRM.GetString("Retry");

            AccuracyLbl.Text = "";
            LatitudeLbl.Text = "";
            LongitudeLbl.Text = "";

        }

    }
}
