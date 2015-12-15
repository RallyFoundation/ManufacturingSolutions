using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OA3.Automation.Lib.UI;
using System.Windows.Automation;
using OA3.Automation.Lib.Log;
using OA3.Automation.Lib;
using System.Configuration;
using System.Reflection;

namespace OA3.Automation.KMT
{
    [TestClass]
    public class OEM_KeyManagement
    {
        string OEM_Title = ConfigurationManager.AppSettings["OEM_MainTitle"].Trim();
        string OEM_path = ConfigurationManager.AppSettings["OEM_path"].Trim();

        /// <summary>
        /// 
        /// </summary>
        [Priority(1)]
        [TestMethod]
        public void LaunchDIS_OEM()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                CommTestCase.launchDIS(OEM_path, "Login - Corporate Key Inventory", CommTestCase.LoginType.Success, OEM_Title, startTime);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
                Verification.AssertKMTResponse(false, "Launch OEM KMT application.");
            }

        }

        [TestMethod]
        public void OEM_getKeys()
        {
            bool resultCell = false;
            MainWindow OEMmainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
            if (OEMmainWindow.MainElement == null)
            {
                LaunchDIS_OEM();
                OEMmainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
            }
            CommTestCase.AssignKeys(OEMmainWindow, ref resultCell, ConfigurationManager.AppSettings["assginNum"].Trim(), CommTestCase.productKey);
        }

        /// <summary>
        /// OEM assign to TPI
        /// </summary>
        [TestMethod]
        public void OEM_Assign_TPI()
        {
            bool resultCell = false;
            MainWindow OEMmainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
            if (OEMmainWindow.MainElement == null)
            {
                LaunchDIS_OEM();
                OEMmainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
            }
            CommTestCase.AssignKeys(OEMmainWindow, ref resultCell, ConfigurationManager.AppSettings["assginNum"].Trim(), CommTestCase.productKey);
        }




        /// <summary>
        /// OEM report CBR to MS
        /// </summary>
        [Priority(7)]
        [TestMethod]
        public void OEM_Report_MS()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
                if (mainWindow.MainElement == null)
                {
                    LaunchDIS_OEM();
                    mainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
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

    }
}
