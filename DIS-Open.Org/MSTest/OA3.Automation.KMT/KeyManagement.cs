using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OA3.Automation.Lib;
using OA3.Automation.Lib.Log;
using OA3.Automation.Lib.UI;
using System.Reflection;
using System.Windows.Automation;

namespace OA3.Automation.KMT
{
    [TestClass]
    public class UnitTest1
    {
        string assginNum = "1";
        string OEM_Title = "Key Management Tool - Corporate Key Inventory";
        string TPI_title = "Key Management Tool - Factory Key Inventory";
        string FF_title = "Key Management Tool - Factory Floor Key Inventory";
        string OEM_path = @"C:\Program Files (x86)\DIS Solution\OEM\Key Management Tool\DIS.Presentation.KMT.exe";
        string TPI_path = @"C:\Program Files (x86)\DIS Solution\TPI\Key Management Tool\DIS.Presentation.KMT.exe";
        string FF_path = @"C:\Program Files (x86)\DIS Solution\Factory Floor\Key Management Tool\DIS.Presentation.KMT.exe";
        string productKey = "";

        #region Launch DIS
        [Priority(3)]
        [TestMethod]
        public void LaunchDIS_OEM()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                CommTestCase.launchDIS(OEM_path, "Login", CommTestCase.LoginType.Success, OEM_Title, startTime);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
                Verification.AssertKMTResponse(false, "Launch OEM KMT application.");
            }

        }

        [Priority(3)]
        [TestMethod]
        public void LaunchDIS_TPI()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                CommTestCase.launchDIS(TPI_path, "Login", CommTestCase.LoginType.Success, TPI_title, startTime);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
                Verification.AssertKMTResponse(false, "Launch TPI KMT application.");
            }

        }

        [Priority(3)]
        [TestMethod]
        public void LaunchDIS_FactoryFloor()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                CommTestCase.launchDIS(FF_path, "Login", CommTestCase.LoginType.Success, FF_title, startTime);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
                Verification.AssertKMTResponse(false, "Launch FF KMT application.");
            }

        }

        #endregion

        #region OEM
        /// <summary>
        /// OEM assign to TPI
        /// </summary>
        [TestMethod]
        public void TPI_Recall_OEM()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                MainWindow OEMmainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
                CommTestCase.AssignKeys(OEMmainWindow, ref resultCell, assginNum, ref productKey);
                TPI_getKeys_OEM();
                MainWindow TPImainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                CommTestCase.RecallKeys(TPImainWindow, ref resultCell, productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
            Verification.AssertKMTResponse(resultCell, MethodBase.GetCurrentMethod().Name + " Test case ");
            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
            FF_Recall_TPI();
        }

        /// <summary>
        /// OEM report CBR to MS
        /// </summary>
        [TestMethod]
        public void OEM_Report_MS()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
                CommTestCase.ReportKeys(mainWindow, assginNum, ref resultCell, productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
            Verification.AssertKMTResponse(resultCell, MethodBase.GetCurrentMethod().Name + " Test case ");
            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
        }

        #endregion

        #region TPI
        /// <summary>
        /// TPI get keys from OEM
        /// </summary>
        [TestMethod]
        public void TPI_getKeys_OEM()
        {
            bool result = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                CommTestCase.GetKeys(mainWindow, assginNum, ref result);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }

        }

        /// <summary>
        /// TPI report CBR to OEM
        /// </summary>
        [TestMethod]
        public void TPI_Report_OEM()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                CommTestCase.ReportKeys(mainWindow, assginNum, ref resultCell, productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
            Verification.AssertKMTResponse(resultCell, MethodBase.GetCurrentMethod().Name + " Test case ");

            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
            OEM_Report_MS();
        }

        /// <summary>
        /// TPI report CBR to MS
        /// </summary>
        [TestMethod]
        public void TPI_Report_MS()
        {
            bool resultCell = false;
            MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
            CommTestCase.ReportKeys(mainWindow, assginNum, ref resultCell, productKey);
        }
        #endregion

        #region Factory Floor

        /// <summary>
        /// TPI get keys from OEM
        /// </summary>
        [TestMethod]
        public void FF_getKeys_TPI()
        {
            bool result = false;
            try
            {
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                CommTestCase.GetKeys(mainWindow, assginNum, ref result);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
        }

        [TestMethod]
        public void FF_Recall_TPI()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                MainWindow OEMmainWindow = new MainWindow(AutomationElement.RootElement, OEM_Title);
                CommTestCase.AssignKeys(OEMmainWindow, ref resultCell, assginNum, ref productKey);
                TPI_getKeys_OEM();
                MainWindow TPImainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                CommTestCase.AssignKeys(TPImainWindow, ref resultCell, assginNum, ref productKey);
                FF_getKeys_TPI();
                MainWindow FFmainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                CommTestCase.RecallKeys(FFmainWindow, ref resultCell, productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
            Verification.AssertKMTResponse(resultCell, MethodBase.GetCurrentMethod().Name + " Test case ");

            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
            FF_RevertKeys();
        }

        [TestMethod]
        public void FF_RevertKeys()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                MainWindow TPImainWindow = new MainWindow(AutomationElement.RootElement, TPI_title);
                CommTestCase.AssignKeys(TPImainWindow, ref resultCell, assginNum, ref productKey);
                FF_getKeys_TPI();
                CommTestCase.SimulationRegister(productKey);
                MainWindow mainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                CommTestCase.RevertKeys(mainWindow, assginNum, ref resultCell, productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }

            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
            FF_Report_TPI();
        }

        [TestMethod]
        public void FF_Report_TPI()
        {
            DateTime startTime = DateTime.Now;
            bool resultCell = false;
            try
            {
                if (productKey != "")
                {
                    CommTestCase.SimulationRegister(productKey);
                }
                MainWindow FFmainWindow = new MainWindow(AutomationElement.RootElement, FF_title);
                CommTestCase.ReportKeys(FFmainWindow, assginNum, ref resultCell, productKey);
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
            Verification.AssertKMTResponse(resultCell, MethodBase.GetCurrentMethod().Name + " Test case ");

            TimeSpan spendTime = DateTime.Now - startTime;
            CommTestCase.WriteSummary(MethodBase.GetCurrentMethod().Name, spendTime.Seconds.ToString());
            TPI_Report_OEM();
        }

        #endregion


    }
}
