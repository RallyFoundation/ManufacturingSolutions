using DllComponent;
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
using System.Threading;
using System.Windows.Forms;


namespace FormsBrightness
{
    public partial class FormBrightness : Form
    {

        private static ResourceManager LocRM;
        /// <summary>
        /// Initializes a new instance of the FormBrightness form class.
        /// </summary>
        public FormBrightness()
        {
            
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            InitializeComponent();
            SetString();
            
        }

        /// <summary>
        /// A thread function for brightness and it will call the function in the DllComponent.
        /// </summary>
        private void _ThreadFunction()
        {
            System.Threading.Thread.Sleep(500);
            Int32 hr;
            CoreComponent component = null;
            try
            {
                component = new CoreComponent();
                hr = component.TestBrightness();
            }
            finally
            {
                if (component != null)
                {
                    component.Dispose();
                    component = null;
                }
            }
            PassBtn.Visible = true;
            FailBtn.Visible = true;
            RetryBtn.Visible = true;
            
        }
        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            this.Text = LocRM.GetString("Brightness");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            RetryBtn.Text = LocRM.GetString("Retry");
            BrightnessLab.Text = LocRM.GetString("Brightness_text");
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
        /// <summary>
        /// Control.Click Event handler. Where control is the Fail button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void button2_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
        }

        private void FormBrightness_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            PassBtn.Visible = false;
            FailBtn.Visible = false;
            RetryBtn.Visible = false;
            Thread BrightnessThread = new Thread(_ThreadFunction);
            BrightnessThread.Start();
            
          
        }
        /// <summary>
        /// Control.Click Event handler. Where control is the Retry button
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void RetryBtn_Click(object sender, EventArgs e)
        {
            PassBtn.Visible = false;
            FailBtn.Visible = false;
            RetryBtn.Visible = false;
            Thread BrightnessThread = new Thread(_ThreadFunction);
            BrightnessThread.Start();
        }

    }
}
