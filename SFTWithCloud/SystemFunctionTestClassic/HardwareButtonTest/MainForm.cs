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
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace HardwareButtonTest
{
    public partial class MainForm : Form
    {

        #region Fields

        private static ResourceManager LocRM;
        private static int BtnDownNum = 1;
        private List<string> BtnControls = new List<string>();

        #endregion //Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);
            InitializeComponent();
            InitializeKeys();
            SetString();
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
        }

        #endregion //Constructor

        /// <summary>
        /// Initialize hardware Keys from program arguments
        /// Keycode references: https://msdn.microsoft.com/en-us/library/system.windows.forms.keys%28v=vs.110%29.aspx
        /// </summary>
        private void InitializeKeys()
        {
            if (Program.ProgramArgs == null || Program.ProgramArgs.Count < 2)
                return;
            if (Program.ProgramArgs.Count > 2)
                BtnDownNum = Int32.Parse(Program.ProgramArgs[2]);

            string[] ButtonList = Program.ProgramArgs[1].Split(',');
            for (int i = 0; i < ButtonList.Length; i++)
            {
                Label newLabel = new Label();
                newLabel.Name = ButtonList[i];
                newLabel.Text = ButtonList[i];
                newLabel.BackColor = Color.YellowGreen;
                newLabel.AutoSize = true;
                newLabel.Margin = new System.Windows.Forms.Padding(10);
                flowLayoutPanel1.Controls.Add(newLabel);
                for (int j = 0; j < BtnDownNum; j++)
                {
                    BtnControls.Add(ButtonList[i]);
                }
            }
        }

        /// <summary>
        /// Control.KeyDown Event handler. Removes the key control when correct key is pressed.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            int keyInt = (int) e.KeyCode;
            Keys k = (Keys)keyInt;
            Control[] _control = flowLayoutPanel1.Controls.Find(k.ToString(), true);
            if (_control.Length > 0)
            {
                string tempControlName = k.ToString().ToLower();
                BtnControls.Remove(tempControlName);
                if (!BtnControls.Contains(tempControlName))
                {
                    Control c = _control[0];
                    c.Dispose();
                }
            }

            if (flowLayoutPanel1.Controls.Count == 0)
            {
                Program.ExitApplication(0);
            }
        }


        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("HardwareButton") + LocRM.GetString("Test");
            ExitLbl.Text = LocRM.GetString("Exit");
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Exit button. Test exits with result = Fail
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ExitLbl_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
        }



    }
}
