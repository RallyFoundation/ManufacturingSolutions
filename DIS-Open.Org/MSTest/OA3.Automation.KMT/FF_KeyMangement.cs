using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OA3.Automation.Lib.UI;
using System.Reflection;
using System.Windows.Automation;
using OA3.Automation.Lib.Log;
using OA3.Automation.Lib;
using System.Configuration;

namespace OA3.Automation.KMT
{
    [TestClass]
    public class FF_KeyMangement
    {
        string FF_title = ConfigurationManager.AppSettings["FF_MainTitle"].Trim();
        string FF_path = ConfigurationManager.AppSettings["FF_path"].Trim();

        /// <summary>
        /// 
        /// </summary>
        [Priority(1)]
        [TestMethod]
        public void LaunchDIS_FactoryFloor()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                CommTestCase.launchDIS(FF_path, "Login - Factory Floor Key Inventory", CommTestCase.LoginType.Success, FF_title, startTime);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
                Verification.AssertKMTResponse(false, "Launch FF KMT application.");
            }

        }
        /// <summary>
        /// TPI get keys from OEM
        /// </summary>
        [Priority(1)]
        [TestMethod]
        public void FF_getKeys_TPI()
        {
            bool result = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                if (mainWindow.MainElement == null)
                {
                    LaunchDIS_FactoryFloor();
                    mainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                }
                CommTestCase.GetKeys(mainWindow, ConfigurationManager.AppSettings["assginNum"].Trim(), ref result);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Factory Floor recall keys to TPI
        /// </summary>
        [Priority(3)]
        [TestMethod]
        public void FF_Recall_TPI()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                //Factory Floor
                FF_getKeys_TPI();
                MainWindow FFmainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                if (FFmainWindow.MainElement == null)
                {
                    LaunchDIS_FactoryFloor();
                    FFmainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                }
                CommTestCase.RecallKeys(FFmainWindow, ref resultCell, CommTestCase.productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
            Verification.AssertKMTResponse(resultCell, MethodBase.GetCurrentMethod().Name + " Test case ");

            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());

        }

        /// <summary>
        /// Revert Keys from Consumed/Bound to Fulfilled
        /// </summary>
        [Priority(4)]
        [TestMethod]
        public void FF_RevertKeys()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {

                //Factory Floor
                FF_getKeys_TPI();
          
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                if (mainWindow.MainElement == null)
                {
                    LaunchDIS_FactoryFloor();
                    mainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                }
                CommTestCase.RevertKeys(mainWindow, ConfigurationManager.AppSettings["assginNum"].Trim(), ref resultCell, CommTestCase.productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }

            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
            FF_Report_TPI();
        }

        /// <summary>
        /// Factory Floor report CBR to TPI
        /// </summary>
        [Priority(5)]
        [TestMethod]
        public void FF_Report_TPI()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                if (CommTestCase.productKey != "")
                {
                    CommTestCase.SimulationRegister(CommTestCase.productKey, "Bound");
                }
                MainWindow FFmainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                if (FFmainWindow.MainElement == null)
                {
                    LaunchDIS_FactoryFloor();
                    FFmainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                }
                CommTestCase.ReportKeys(FFmainWindow, ConfigurationManager.AppSettings["assginNum"].Trim(), ref resultCell, CommTestCase.productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
            Verification.AssertKMTResponse(resultCell, MethodBase.GetCurrentMethod().Name + " Test case ");

            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
        }
    }
}
