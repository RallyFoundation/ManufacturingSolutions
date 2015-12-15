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
using System.Windows.Forms;

namespace KeyboardTest
{
    public partial class MainForm : Form
    {
        #region Fields

        //Total number of hidden keys is 9
        private static int HiddenKeys = 9;

        #endregion //Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            SetConfigKeys();

            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion //Constructor


        /// <summary>
        /// Enable configurable keys from program arguments
        /// </summary>
        private void SetConfigKeys()
        {
            if (Program.ProgramArgs == null || Program.ProgramArgs.Count < 2)
            {
                return;
            }

            string[] configKeys = Program.ProgramArgs[1].Split(',');
            for (int i = 0; i < configKeys.Length; i++)
            {
                Control[] _control = this.Controls.Find(configKeys[i], true);
                if (_control.Length > 0)
                {
                    Control c = _control[0];
                    c.Visible = true;
                    HiddenKeys--;
                }
            }
        }

        /// <summary>
        /// Control.KeyDown Event handler. When a key is pressed down, remove key from the form.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string _key = e.KeyCode.ToString();
            Debug.WriteLine(_key);
            if (e.KeyCode == Keys.ShiftKey)
            {
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.LShiftKey)))
                {
                    Shift_L.Dispose();
                }
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.RShiftKey)))
                {
                    if (Shift_R.Visible)
                        Shift_R.Dispose();
                }
            }
            else if (e.KeyCode == Keys.ControlKey)
            {
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.LControlKey)))
                {
                    Ctrl_L.Dispose();
                }
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.RControlKey)))
                {
                    if (Ctrl_R.Visible)
                        Ctrl_R.Dispose();
                }
            }
            else if (e.KeyCode == Keys.Menu)
            {
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.LMenu)))
                {
                    Alt_L.Dispose();
                }
                if (Convert.ToBoolean(GetAsyncKeyState(Keys.RMenu)))
                {
                    if (Alt_R.Visible)
                        Alt_R.Dispose();
                }
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
            {
                Win.Dispose();
            }
            else
            {
                Control[] _control = this.Controls.Find(_key, true);
                if (_control.Length > 0)
                {
                    Control c = _control[0];
                    if (c.Visible)
                        c.Dispose();
                }
            }

            if ((tableLayoutPanel1.Controls.Count - HiddenKeys) == 1)
            {
                TestPass();
            }
        }


        /// <summary>
        /// Exit the test with result = Pass
        /// </summary>
        private static void TestPass()
        {
            System.Threading.Thread.Sleep(1000);
            Program.ExitApplication(0);
        }


        /// <summary>
        /// Control.Click Event handler. Where control is the Exit button. Test exits with result = Fail
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ExitLabel_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

    }
}
