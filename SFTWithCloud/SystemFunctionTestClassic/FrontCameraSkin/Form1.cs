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
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace FrontCameraSkin
{
    
    public partial class Form1 : Form
    {
        private static ResourceManager LocRM;
        /// <summary>
        /// Initializes a new instance of the Form1 form class.
        /// </summary>
        public Form1()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            InitializeComponent();
            SetString();

            CameraTest();
           
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
        /// Control.Click Event handler. Where control is the Pass button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true; 
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            this.Text = LocRM.GetString("FrontCamera");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            ResetBtn.Text = LocRM.GetString("Retry");
            
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Retry button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            CameraTest();
            this.Show();
        }
        /// <summary>
        /// Execute the FrontCamera.exe that generate from FrontCaptureEngine project.
        /// </summary>
        void CameraTest()
        {
            string PhotoPath;
            string filePath = System.Environment.CurrentDirectory;
            string Camera_filename = "FrontCamera.exe";
            string camera_friendlyName = null;

            // Delete *.jpg, if it exits
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            PhotoPath = path + "\\CameraTest.jpg";
            if (System.IO.File.Exists(PhotoPath))
                System.IO.File.Delete(PhotoPath);
            
            if (Program.ProgramArgs!= null) {
                if (Program.ProgramArgs.Count > 1)
                    camera_friendlyName = Program.ProgramArgs[1];
            }

            //Excute camera FrontCamera.exe
            Process proc = null;
            ProcessStartInfo startInfo = null;

            int ReturnVal = 0;
            try
            {
                proc = new Process();
                startInfo = new ProcessStartInfo
                {
                    FileName = Camera_filename,
                    Arguments = camera_friendlyName,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                };
                proc.StartInfo = startInfo;

                proc.Start();
                proc.WaitForExit();
                ReturnVal = proc.ExitCode;
                proc.Close();
                startInfo = null;
            }
            finally
            {
                if (proc != null)
                {
                    proc.Dispose();
                    proc = null;
                }
            }

            // Show photo on the Screen
            if (ReturnVal == 999)
            {
                PassBtn.Visible = false;
                PhotoPath = System.Windows.Forms.Application.StartupPath + "\\ConnectToCameraFail.jpg";
            }
            else
                PassBtn.Visible = true;


            Bitmap bmPhoto = null;
            if (!System.IO.File.Exists(PhotoPath)) return;
            try
            {
                bmPhoto = new Bitmap(PhotoPath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.LoadAsync(PhotoPath);
            }
            catch (Exception ex)
            {
                DllLog.Log.LogError("Cannot load image or close the *Camera.exe invalid. : " + ex.ToString());
            }
            finally
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                    bmPhoto.Dispose();
                    bmPhoto = null;
                }
            }
            
            
            
        }

    }
}
