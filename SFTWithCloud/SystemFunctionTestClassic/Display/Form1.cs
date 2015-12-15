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

namespace Display
{
    
    public partial class Form1 : Form
    {
        private static ResourceManager LocRM;
        int iCount;
        /// <summary>
        /// Initializes a new instance of the Form1 form class.
        /// </summary>
        public Form1()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            
            InitializeComponent();
            SetString();
            iCount = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            this.tableLayoutPanel1.Enabled = false;
            this.tableLayoutPanel1.Visible = false;

        }
        
        
        /// <summary>
        /// Control.Click Event handler. It will display different color while we keep clicking the screen.
        /// The initial cplor is green.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void Form1_Click(object sender, EventArgs e)
        {
            switch (iCount)
            {
                case 0:
                    this.BackColor = Color.FromArgb(0, 0, 255); // Blue
                    break;
                case 1:
                    this.BackColor = Color.FromArgb(255, 0, 0); // Red
                    break;
                case 2:
                    this.BackColor = Color.FromArgb(255, 255, 255); // White
                    break;
                case 3:
                    this.BackColor = Color.FromArgb(0, 0, 0); // Black
                    break;
                case 4:
                    this.BackColor = Color.FromArgb(0, 0, 128); // Dark Blue
                    break;
                case 5:
                    this.BackColor = Color.FromArgb(255 ,255, 0); // Yellow
                    break;
                case 6:
                    this.BackColor = Color.FromArgb(69 ,0, 68); // Purple
                    break;
                case 7:
                    this.BackColor = Color.FromArgb(255, 116, 21); // Orange
                    break;
                case 8:
                    this.tableLayoutPanel1.Enabled = true;
                    this.tableLayoutPanel1.Visible = true;
                    foreach (Control c in this.tableLayoutPanel1.Controls)
                    {
                        c.Enabled = true;
                        c.Visible = true;
                    }
                    break;
                default:
                    iCount = 999;
                    break;
            }
            System.Diagnostics.Debug.WriteLine(this.tableLayoutPanel1.Enabled.ToString());
            iCount++;
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
            this.Text = LocRM.GetString("Display");
            PASS.Text = LocRM.GetString("Pass");
            FAIL.Text = LocRM.GetString("Fail");
        }

    }
}
