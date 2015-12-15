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

namespace Video
{
    
    public partial class Form1 : Form
    {
        private static ResourceManager LocRM;
        WMPLib.IWMPMediaCollection medialist;
        WMPLib.IWMPMedia mediaSRC;
        string path;

        /// <summary>
        /// Initializes a new instance of the Form1 form class.
        /// </summary>
        /// <param name="videoPath">The path of media file and it can be set on the SFTConfig.xml.</param>
        public Form1(string videoPath)
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            InitializeComponent();
            SetString();
            path = videoPath;
            Log.LogComment(DllLog.Log.LogLevel.Info, "Video path: " + videoPath);
        }
        /// <summary>
        /// Play the media.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            try 
            {
                medialist = axWindowsMediaPlayer1.mediaCollection;
                mediaSRC = medialist.add(path);
                axWindowsMediaPlayer1.currentPlaylist.appendItem(mediaSRC);
                axWindowsMediaPlayer1.fullScreen = false; // set the screen on fill screen or not.
            }
            catch (Exception ex)
            {
                DllLog.Log.LogError(ex.ToString());
                ErrorLbl.Text = ex.ToString();
                ErrorLbl.Visible = true;
                PassBtn.Visible = false;
            }  
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                Program.ExitApplication(255);
                Application.Exit();
            }
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Pass button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void PASS_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(0);
            Application.Exit();
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Fail button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void FAIL_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
            Application.Exit();
            
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            this.Text = LocRM.GetString("ExternalDisplay");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            ErrorLbl.Text = LocRM.GetString("ExternalDisplay_Err");
        }



    }
}
