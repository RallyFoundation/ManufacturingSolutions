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
using System.Resources;
using System.Windows.Forms;
using Windows.Devices.Sensors;

namespace GyrometerTest
{
    public partial class MainForm : Form
    {
        #region Fields

        private static Image imageX;
        private static Image imageY;
        private static Image imageZ;
        private static ResourceManager LocRM;
        System.Timers.Timer _timer = new System.Timers.Timer();

        #endregion //Fields

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            InitializeComponent();
            InitializeGyrometer();
            SetString();
            Label.CheckForIllegalCrossThreadCalls = false;
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Landscape;

            imageX = pictureBoxX.Image;
            imageY = pictureBoxY.Image;
            imageZ = pictureBoxZ.Image;
        }

        #endregion //Contructor

        /// <summary>
        /// Initialize Gyrometer timer and update Gyrometer every 500ms
        /// </summary>
        private void InitializeGyrometer()
        {
            //Set Timer to update Gyrometer
            _timer.Interval = 100;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateGyrometer);
            _timer.Start();
            
        }


        /// <summary>
        /// Update Gyrometer readings event and update UI
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void UpdateGyrometer(object sender, EventArgs e)
        {
            try
            {
                Gyrometer gyrometer = Gyrometer.GetDefault();
                if (gyrometer != null)
                {
                    XPanel.Text = LocRM.GetString("XAxis") + ": " +
                         String.Format("{0,5:0.00}", gyrometer.GetCurrentReading().AngularVelocityX) + "(°)/s";
                    YPanel.Text = LocRM.GetString("YAxis") + ": " +
                         String.Format("{0,5:0.00}", gyrometer.GetCurrentReading().AngularVelocityY) + "(°)/s";
                    ZPanel.Text = LocRM.GetString("ZAxis") + ": " +
                         String.Format("{0,5:0.00}", gyrometer.GetCurrentReading().AngularVelocityZ) + "(°)/s";

                    pictureBoxX.Image = Rotate(imageX, Math.Max(-135, Math.Min(135, gyrometer.GetCurrentReading().AngularVelocityX)));
                    pictureBoxY.Image = Rotate(imageY, Math.Max(-135, Math.Min(135, gyrometer.GetCurrentReading().AngularVelocityY)));
                    pictureBoxZ.Image = Rotate(imageZ, Math.Max(-135, Math.Min(135, gyrometer.GetCurrentReading().AngularVelocityZ)));
                }
                else
                {
                    XPanel.Text = LocRM.GetString("NotFound");
                    _timer.Stop();
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                XPanel.Text = LocRM.GetString("Error");
                _timer.Stop();

            }
        }


        /// <summary>
        /// Rotates the image
        /// </summary>
        /// <param name="image">image to rotate</param>
        /// <param name="angle">angle to rotate image by.</param>
        public Bitmap Rotate(Image image, double angle)
        {
            Bitmap bitmap = null;

            try
            {
                bitmap = new Bitmap(image.Width, image.Height);
                Graphics g = Graphics.FromImage(bitmap);

                g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
                g.RotateTransform((float)angle);
                g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
                g.DrawImage(image, image.Width / 2 - image.Height / 2, image.Height / 2 - image.Width / 2, image.Height, image.Width);
            }
            catch
            {
            }

            return bitmap;
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
            Title.Text = LocRM.GetString("Gyrometer") + LocRM.GetString("Test");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
        }

    }
}
