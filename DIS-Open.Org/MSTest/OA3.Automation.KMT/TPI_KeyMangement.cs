using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OA3.Automation.Lib.UI;
using System.Windows.Automation;
using OA3.Automation.Lib.Log;
using OA3.Automation.Lib;
using System.Reflection;
using System.Configuration;

namespace OA3.Automation.KMT
{
    [TestClass]
    public class TPI_KeyMangement
    {
        string TPI_title = ConfigurationManager.AppSettings["TPI_MainTitle"].Trim();
        string TPI_path = ConfigurationManager.AppSettings["TPI_path"].Trim();

        /// <summary>
        /// 
        /// </summary>
        [Priority(1)]
        [TestMethod]
        public void LaunchDIS_TPI()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                CommTestCase.launchDIS(TPI_path, "Login - Factory Key Inventory", CommTestCase.LoginType.Success, TPI_title, startTime);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
                Verification.AssertKMTResponse(false, "Launch TPI KMT application.");
            }

        }

        /// <summary>
        /// TPI get keys from OEM
        /// </summary>
        [Priority(1)]
        [TestMethod]
        public void TPI_getKeys_OEM()
        {
            bool result = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                if (mainWindow.MainElement == null)
                {
                    LaunchDIS_TPI();
                    mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                }
                CommTestCase.GetKeys(mainWindow, ConfigurationManager.AppSettings["assginNum"].Trim(), ref result);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }

        }

        [TestMethod]
        public void TPI_Assign_DLS()
        {
            bool resultCell = false;
            MainWindow TPImainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
            if (TPImainWindow.MainElement == null)
            {
                LaunchDIS_TPI();
                TPImainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
            }
            CommTestCase.AssignKeys(TPImainWindow, ref resultCell, ConfigurationManager.AppSettings["assginNum"].Trim(), CommTestCase.productKey);
        }
        /// <summary>
        /// TPI racall keys to OEM (Centralized)
        /// </summary>
        [Priority(2)]
        [TestMethod]
        public void TPI_Recall_OEM()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                TPI_getKeys_OEM();
                MainWindow TPImainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                if (TPImainWindow.MainElement == null)
                {
                    LaunchDIS_TPI();
                    TPImainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                }
                CommTestCase.RecallKeys(TPImainWindow, ref resultCell, CommTestCase.productKey);
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
        /// TPI report CBR to OEM
        /// </summary>
        [Priority(6)]
        [TestMethod]
        public void TPI_Report_OEM()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                if (mainWindow.MainElement == null)
                {
                    LaunchDIS_TPI();
                    mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                }
                CommTestCase.ReportKeys(mainWindow, ConfigurationManager.AppSettings["assginNum"].Trim(), ref resultCell, CommTestCase.productKey);
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
        /// TPI report CBR to MS
        /// </summary>
        [Priority(1)]
        [TestMethod]
        public void TPI_Report_MS()
        {
            bool resultCell = false;
            MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
            if (mainWindow.MainElement == null)
            {
                LaunchDIS_TPI();
                mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
            }
            CommTestCase.ReportKeys(mainWindow, ConfigurationManager.AppSettings["assginNum"].Trim(), ref resultCell, CommTestCase.productKey);
        }
    }
}
