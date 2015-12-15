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

namespace TouchpadTest
{
    public partial class MainForm : Form
    {
        #region Fields

        private static ResourceManager LocRM;
        private int TotalObjs;

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
            TotalObjs = 5; //5 items to remove
        }

        #endregion // Constructor

        /// <summary>
        /// Event when Left click is caught by form. Dispose control if action correspond with control type {Left, right, double click}
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void Left_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Panel _panel = (Panel)sender;
                _panel.Dispose();
                TotalObjs--;
                TestPass();
            }
        }

        /// <summary>
        /// Event when Right click is caught by form. Dispose control if action correspond with control type {Left, right, double click}
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void Right_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Panel _panel = (Panel)sender;
                _panel.Dispose();
                TotalObjs--;
                TestPass();
            }
        }

        /// <summary>
        /// Event when Double click is caught by form. Dispose control if action correspond with control type {Left, right, double click}
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void Mouse_DoubleClick(object sender, MouseEventArgs e)
        {
            Panel _panel = (Panel)sender;
            _panel.Dispose();
            TotalObjs--;
            TestPass();
        }

        /// <summary>
        /// If all test controls are removed, exit application with result = Pass
        /// </summary>
        private void TestPass()
        {
            if (TotalObjs == 0)
            {
                System.Threading.Thread.Sleep(500);
                Program.ExitApplication(0);
            }
        }


        /// <summary>
        /// Control.Click Event handler. Where control is the Exit button, exit application with result = Fail
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
        }

        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("Touchpad") + LocRM.GetString("Test");
            LeftLbl1.Text = LocRM.GetString("TouchpadLeft");
            LeftLbl2.Text = LocRM.GetString("TouchpadLeft");
            RightLbl1.Text = LocRM.GetString("TouchpadRight");
            RightLbl2.Text = LocRM.GetString("TouchpadRight");
            DoubleLbl.Text = LocRM.GetString("TouchpadDouble");
            ExitBtn.Text = LocRM.GetString("Exit");
        }



    }
}
