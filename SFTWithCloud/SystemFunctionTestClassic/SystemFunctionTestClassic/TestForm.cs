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
using win81FactoryTest.Functions;
using win81FactoryTest.Setting;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MESCloud.Client;

namespace win81FactoryTest
{
    public partial class TestForm : Form
    {
        #region Fields

        private static ResourceManager LocRM;
        private static Boolean IsFinish;

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public TestForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Label.CheckForIllegalCrossThreadCalls = false;

            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(TestForm).Assembly);
            IsFinish = false;
            InitlializeTests();
            SetString();
        }

        #endregion // Constructor

        /// <summary>
        /// Initialize test phases and load default phase
        /// </summary>
        private void InitlializeTests()
        {
            ClearMenuPanel(); //initial clear of panels
            //load Phase in dropdown 
            string[] phaseList = ConfigSettings.GetAllPhase();
            for (int i = 0; i < phaseList.Length; i++)
            {
                PhaseDropDown.Items.Add(phaseList[i]);
            }
            //load the menu list with first phase in xml (default)
            LoadMenuList(ConfigSettings.GetTestSettingPhase(phaseList[0]));
            PhaseDropDown.SelectedIndex = 0;
        }

        /// <summary>
        /// Reads the Tests from config for the selected phase.
        /// Set the UI accordingly.
        /// </summary>
        private void LoadMenuList(string[] MenuList)
        {
            int length = MenuList.Length;
            if (MenuList.Length > this.MenuListTable.Controls.Count)
            {
                length = this.MenuListTable.Controls.Count;
            }

            for (int i = 0; i < length; i++)
            {
                Control c = this.MenuListTable.Controls[i];
                c.Name = MenuList[i];
                c.BackColor = Color.DimGray;
                c.Enabled = true;
                c.Text = LocRM.GetString(MenuList[i]);
            }
            InitializeTestResults();
        }

        /// <summary>
        /// Load test results from system Registry.
        /// Set the UI accordingly.
        /// </summary>
        private void InitializeTestResults()
        {
            foreach (string keyID in Program.SFTRegKey.GetValueNames())
            {
                string testResult = (string)Program.SFTRegKey.GetValue(keyID);
                Control[] _control = MenuListTable.Controls.Find(keyID, true);
                if (_control.Length > 0)
                {
                    Control c = _control[0];
                    AnimateResult(c, Convert.ToBoolean(testResult));
                }
            }
        }

        /// <summary>
        /// Color each test menu depends on the result
        /// </summary>
        private static void AnimateResult(Control c, bool result)
        {
            if (result)
            {
                c.BackColor = Color.YellowGreen;
            }
            else
            {
                c.BackColor = Color.LightCoral;
            }
        }

        /// <summary>
        /// Clear all test results on the UI
        /// </summary>
        private void ClearMenuPanel()
        {
            for (int i = 0; i < this.MenuListTable.Controls.Count; i++)
            {
                Control panelControl = this.MenuListTable.Controls[i];
                panelControl.Name = String.Empty;
                panelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                panelControl.Enabled = false;
                panelControl.Text = String.Empty;
            }
            IsFinish = false;
        }

        /// <summary>
        /// Runs all available tests
        /// </summary>
        private void RunAllTests()
        {
            //Rally, Jul 27, 2015;
            this.testResults = new Dictionary<string, bool>();

            for (int i = 0; i < this.MenuListTable.Controls.Count; i++)
            {
                Control test = this.MenuListTable.Controls[i];
                if (!String.IsNullOrEmpty(test.Name))
                {
                    //If 'Run All' is not complete, skip the ones with results
                     if (!IsFinish &&
                        test.BackColor != Color.DimGray)
                    {
                        continue;
                    }
                    DateTime startTest = System.DateTime.Now;
                    bool testResult = ExecuteTest.Run(test.Name);
                    AddTestRegistry(test.Name, testResult);
                    AnimateResult(test, testResult);

                    //Rally, Jul 27, 2015;
                    this.testResults.Add(test.Name, testResult);

                    //if test item fail: stop autorun
                    if (!testResult)
                    {
                        RunButton.Text = LocRM.GetString("Resume");
                        RunButton.BackColor = Color.YellowGreen;
                        return;
                    }
                }
            }
            //When Run All completes
            RunButton.Text = LocRM.GetString("RunAll");
            RunButton.BackColor = Color.YellowGreen;
            IsFinish = true;
        }



        /// <summary>
        /// Control.SelectedIndexChange Event handler. Where control is the Phase drop down menu.
        /// When phase is changed, redraw the UI.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void PhaseDropDown_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ClearMenuPanel();
            Control selected = (Control)sender;
            LoadMenuList(ConfigSettings.GetTestSettingPhase(selected.Text));

        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Run button.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void RunButton_Click_1(object sender, EventArgs e)
        {
            RunButton.BackColor = Color.LightCoral;
            RunAllTests();

            //Rally, Jul 27, 2015;
            this.processResult();
        }

        /// <summary>
        /// Control.Click Event handler. Where control is a single test menu item.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void TestMenu_Click(object sender, EventArgs e)
        {
            Control test = (Control)sender;
            if (!String.IsNullOrEmpty(test.Name))
            {
                DateTime startTest = System.DateTime.Now;
                bool testResult = ExecuteTest.Run(test.Name);
                AddTestRegistry(test.Name, testResult);
                AnimateResult(test, testResult);
            }
        }

        /// <summary>
        /// Control.Click Event handler. Where control is a settings.
        /// Will Run app "GenerateSystemSettings"
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            ExecuteTest.Run("GenerateSystemSettings");
        }

        /// <summary>
        /// Control.Click Event handler. Where control is a single test menu item.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ExitBtn_Click_1(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>
        /// Form.FormClosing Event handler.
        /// When this form closes, log the results.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.SFTRegKey.Close();
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the clear button.
        /// Clears results from Registry and redraws UI
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ClearResultBtn_Click(object sender, EventArgs e)
        {
            ClearMenuPanel();
            DeleteRegistryResult();//Clear result from registry
            LoadMenuList(ConfigSettings.GetTestSettingPhase(PhaseDropDown.SelectedItem.ToString()));
        }

        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            RunButton.Text = LocRM.GetString("RunAll");
            ClearResultBtn.Text = LocRM.GetString("Clear");
            ExitBtn.Text = LocRM.GetString("Exit");
            SettingsBtn.Text = "\uE115";
        }

        /// <summary>
        /// Add test result to system's Registry
        /// </summary>
        private static void AddTestRegistry(string testID, bool result)
        {
            Program.SFTRegKey.SetValue(testID, result);
        }

        /// <summary>
        /// Delete all results in system's Registry
        /// </summary>
        private static void DeleteRegistryResult()
        {
            foreach (string keyID in Program.SFTRegKey.GetValueNames())
            {
                Program.SFTRegKey.DeleteValue(keyID);
            }
        }

        #region CTECustomization

        private Dictionary<string, bool> testResults;
        private bool isAlwaysAutoRunning()
        {
            if (System.Configuration.ConfigurationManager.AppSettings.Get("IsAlwaysAutoRunning") == null)
            {
                return false;
            }

            string configValue = System.Configuration.ConfigurationManager.AppSettings.Get("IsAlwaysAutoRunning");

            if ((configValue.ToLower() == "true") || (configValue == "1"))
            {
                return true;
            }

            return false;
        }

        private bool isShowingPopupAfterAllTestsDone()
        {
            if (System.Configuration.ConfigurationManager.AppSettings.Get("IsShowingPopupAfterAllTestsDone") == null)
            {
                return false;
            }

            string configValue = System.Configuration.ConfigurationManager.AppSettings.Get("IsShowingPopupAfterAllTestsDone");

            if ((configValue.ToLower() == "true") || (configValue == "1"))
            {
                return true;
            }

            return false;
        }

        private bool isCallingExternalApp()
        {
            if (System.Configuration.ConfigurationManager.AppSettings.Get("IsCallingExternalApp") == null)
            {
                return false;
            }

            string configValue = System.Configuration.ConfigurationManager.AppSettings.Get("IsCallingExternalApp");

            if ((configValue.ToLower() == "true") || (configValue == "1"))
            {
                return true;
            }

            return false;
        }

        private bool isClosingMainApp()
        {
            if (System.Configuration.ConfigurationManager.AppSettings.Get("IsClosingMainApp") == null)
            {
                return false;
            }

            string configValue = System.Configuration.ConfigurationManager.AppSettings.Get("IsClosingMainApp");

            if ((configValue.ToLower() == "true") || (configValue == "1"))
            {
                return true;
            }

            return false;
        }

        private bool isSavingResultsToMESCloud()
        {
            if (System.Configuration.ConfigurationManager.AppSettings.Get("IsSavingTestResultsToMESCloud") == null)
            {
                return false;
            }

            string configValue = System.Configuration.ConfigurationManager.AppSettings.Get("IsSavingTestResultsToMESCloud");

            if ((configValue.ToLower() == "true") || (configValue == "1"))
            {
                return true;
            }

            return false;
        }

        private void startExternalApp(string arguments)
        {
            string externalAppPath = System.Configuration.ConfigurationManager.AppSettings.Get("ExternalApp");
            string externalAppParams = System.Configuration.ConfigurationManager.AppSettings.Get("ExternalAppParams");

            if (!String.IsNullOrEmpty(arguments))
            {
                if (externalAppParams.Contains("{") && externalAppParams.Contains("}"))
                {
                    externalAppParams = String.Format(externalAppParams, arguments);
                }
            }

            System.Diagnostics.Process process = new Process();

            process.StartInfo.FileName = externalAppPath;
            process.StartInfo.Arguments = externalAppParams;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.RedirectStandardError = false;
            process.StartInfo.RedirectStandardOutput = false;

            process.Start();
        }

        private void processResult()
        {
            string transactionID = Guid.NewGuid().ToString();

            //If all sub tests pass, automatically invoke the external app, and close / exit the test app. - Rally, May 25, 2015
            string configuredExternalAppStartFlag = System.Configuration.ConfigurationManager.AppSettings.Get("ExternalAppStartFlag");

            if (this.isSavingResultsToMESCloud())
            {
                try
                {
                    MESCloud.Client.ModuleConfiguration.ServicePoint = System.Configuration.ConfigurationManager.AppSettings.Get("MESCloudServicePoint");
                    MESCloud.Client.ModuleConfiguration.AuthorizationHeaderValue = String.Format("{0}:{1}", System.Configuration.ConfigurationManager.AppSettings.Get("MESCloudUserName"), System.Configuration.ConfigurationManager.AppSettings.Get("MESCloudPassword"));

                    Dictionary<string, string> resultItems = null;

                    if ((this.testResults != null) && (this.testResults.Count > 0))
                    {
                        resultItems = new Dictionary<string, string>();

                        foreach (string item in this.testResults.Keys)
                        {
                            resultItems.Add(item, this.testResults[item].ToString());
                        }
                    }

                    if (resultItems != null)
                    {
                        File.Copy("SFTClassicLog.txt", String.Format("{0}.log", transactionID));
                        byte[] rawData = File.ReadAllBytes(String.Format("{0}.log", transactionID));

                        new Manager(true, "MESCloudTraceSource").NewResult(transactionID, resultItems, rawData);

                        File.Delete("SFTClassicLog.txt");
                    }
                }
                catch (Exception ex)
                {

                    //throw;

                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            string startFlag = this.ifAllSubTestsPass() ? "1" : "0";

            if (configuredExternalAppStartFlag == startFlag)
            {
                if (this.isCallingExternalApp())
                {
                    this.startExternalApp(transactionID);
                }

                if (this.isClosingMainApp())
                {
                    //this.Close();

                    Application.Exit();
                }
            }
        }

        private bool ifAllSubTestsPass()
        {
            if ((this.testResults == null) || (this.testResults.Count <= 0))
            {
                return false;
            }

            foreach (var testName in this.testResults.Keys)
            {
                if (this.testResults[testName] == false)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
