using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OA3.Automation.Lib.UI;
using OA3.Automation.Lib.UI.Windows;
using OA3.Automation.Lib.Log;
using System.Threading;
using WPFAutomation.Core;
using OA3.Automation.Lib;
using System.Reflection;
using WPFAutomation.Core.Controls;
using System.Windows.Automation;
using OA3.Automation.Lib.KMT;
using System.Data.SqlClient;
using System.Configuration;

namespace OA3.Automation.KMT
{
    class CommTestCase
    {
        /// <summary>
        /// Save product key for work flow
        /// </summary>
        public static string productKey { get; set; }

        /// <summary>
        /// Login KMT
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="title"></param>
        public static void launchDIS(string appPath, string LoginTitle, LoginType type, string kmtTitle, DateTime startTime)
        {
            Driver.StartApplication(appPath);
            Thread.Sleep(1000);
            LoginWindow loginWindow = new Lib.UI.LoginWindow(AutomationElement.RootElement, LoginTitle);
            bool result = false;
            bool resultStr = false;
            if (type == LoginType.Fail)
            {
                loginWindow.TextBoxLoginID.SetValue(ReaderXML.LoginInfo.Case[ReaderXML.CurrentCase].Input["UserID"]);
                loginWindow.TextBoxPassword.SetValue(ReaderXML.LoginInfo.Case[ReaderXML.CurrentCase].Input["Password"]);
                loginWindow.BtnLogin.Click();
                string ExpectStr = ReaderXML.LoginInfo.Case[ReaderXML.CurrentCase].Expected["ErrorMsg"];
                string ActualStr = loginWindow.MsgBoxError.Text;
                resultStr = Helper.CompareTwoStr(ExpectStr, ActualStr);
                loginWindow.MsgBoxError.ClickOK();
                loginWindow.CanelButton.Click();
                Verification.AssertKMTResponse(resultStr, MethodBase.GetCurrentMethod().Name + "Test case ");
            }
            else
            {
                loginWindow.TextBoxLoginID.SetValue("admin");
                loginWindow.TextBoxPassword.SetValue("123");
                Thread.Sleep(500);
                System.Windows.Forms.SendKeys.SendWait("{RIGHT}");
                Thread.Sleep(600);
                loginWindow.BtnLogin.Click();
                Thread.Sleep(1000);
                int i = 0;
                if (loginWindow.ProgressBar.progressBar != null)
                {
                    do
                    {
                        loginWindow.ProgressBar.isFinish(3); i++;
                    } while (loginWindow.ProgressBar.progressBar != null && i < 3);
                }
                result = Helper.ExtractElement(AutomationElement.RootElement, kmtTitle) == null ? false : true;
                Verification.AssertKMTResponse(result, "Launch " + kmtTitle + " application.");
            }
            TimeSpan spendTime = DateTime.Now - startTime;
            TextLog.LogMessage("");
            TextLog.LogMessage("=============Launch DIS Spended Time: " + spendTime.Seconds.ToString() + "s" + "===================");
            TextLog.LogMessage("");
        }
        /// <summary>
        /// Export Keys
        /// </summary>
        /// <param name="mainWindow"></param>
        public static void ExportKeys(MainWindow mainWindow, string WindowTitle, exportType type, bool isEncrypt)
        {
            mainWindow.Ribbon.ExportButton.Click();
            ExportKeyWizard exportWindow = new ExportKeyWizard(mainWindow.MainElement, WindowTitle);
            TextLog.LogMessage("Loaded Export Window " + mainWindow.Title);
            Thread.Sleep(1000);
            bool resultCell = false;
            bool resultPath = false;
            if (type == exportType.ExportCBR)
            {
                exportWindow.ExportCBR.Click();
            }
            else
            {
                exportWindow.ExportKeys.Click();
            }
            Thread.Sleep(1000);
            exportWindow.NextButton.Click();

            //Set value
            exportWindow.Tab.SelectItem(0);
            if (exportWindow.DataGridByQuantity.GridPattern.Current.RowCount > 0)
            {
                TextLog.LogMessage("There are [" + exportWindow.DataGridByQuantity.RowCount.ToString() + "] keys in the DataGridByQuantity.");
                string expectCBRNum = ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Quantity"];
                exportWindow.DataGridByQuantity.SetValue(0, expectCBRNum);
                Thread.Sleep(1000);
                exportWindow.Tab.SelectItem(1);
                if (exportWindow.DataGridByID.GridPattern.Current.RowCount > 0)
                {
                    int CBRNum = Convert.ToInt32(expectCBRNum);
                    if (CBRNum > exportWindow.DataGridByID.RowCount)
                    {
                        CBRNum = exportWindow.DataGridByID.RowCount;
                        expectCBRNum = CBRNum.ToString();
                    }
                    for (int i = 1; i < CBRNum + 1; i++)
                    {
                        exportWindow.DataGridByID.CheckItem(i);
                    }

                    Thread.Sleep(1000);
                    TextLog.LogMessage("IsEncrypted the Export file.");
                    //Non-Encrypetd
                    bool IsChect = exportWindow.Encrypted.GetStatus() == "On" ? true : false;
                    if (!IsChect == isEncrypt)
                    {
                        exportWindow.Encrypted.Click();
                    }

                    string ExpectCell = exportWindow.DataGridByID.GetValue(0, 2);
                    string ExpectPath = exportWindow.Path.Text;
                    TextLog.LogMessage("Get Expect Value from DataGrid : [" + ExpectCell + "] \n " + ExpectPath);

                    exportWindow.OKButton.Click();
                    Thread.Sleep(1000);
                    if (!isEncrypt)
                    {
                        exportWindow.MessageboxWarning.ClickOK("1");
                        Thread.Sleep(1000);
                        exportWindow.OKButton.Click();
                    }
                    Thread.Sleep(500);
                    ProgressBarisFinish(exportWindow.ProgressBar, 3);
                    Thread.Sleep(500);
                    string ActualCell = exportWindow.DataGridSummary.GetValue(0, 1);
                    string ActualPath = exportWindow.TextBoxFileName.Text;
                    TextLog.LogMessage("Get Actual Value from DataGrid : [" + ActualCell + "] \n " + ActualPath);

                    ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Path"] = ActualPath;
                    ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Key"] = ActualCell;
                    resultCell = Helper.CompareTwoStr(ExpectCell, ActualCell);
                    resultPath = Helper.CompareTwoStr(ExpectPath, ActualPath);
                    Thread.Sleep(1000);
                    exportWindow.CloseButton.Click();
                }
                else
                {
                    TextLog.LogMessage("There not available key in the DataGridByID!");
                    Helper.CloseApp(exportWindow.MainElement);
                }

            }
            else
            {
                TextLog.LogMessage("There not available key in the DataGridByQuantity!");
                Helper.CloseApp(exportWindow.MainElement);
            }
            bool result = resultCell && resultPath == true ? true : false;
            Verification.AssertKMTResponse(result, ReaderXML.CurrentCase + "Test case ");

        }
        /// <summary>
        /// Export Keys
        /// </summary>
        /// <param name="mainWindow"></param>
        public static void ReExportKeys(MainWindow mainWindow, string WindowTitle)
        {
            mainWindow.Ribbon.ExportButton.Click();
            ExportKeyWizard exportWindow = new ExportKeyWizard(mainWindow.MainElement, WindowTitle);
            TextLog.LogMessage("Loaded Export Window " + mainWindow.Title);
            Thread.Sleep(1000);
            bool resultCell = false;
            bool resultPath = false;
            bool resultQuantity = false;
            exportWindow.RexportKeys.Click();
            Thread.Sleep(1000);

            exportWindow.NextButton.Click();

            //Set value

            if (exportWindow.DataGridReExport.RowCount > 0)
            {
                TextLog.LogMessage("There are [" + exportWindow.DataGridReExport.RowCount.ToString() + "] keys in the DataGrid ReExport.");
                string expectIndex = ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Quantity"];


                int index = Convert.ToInt32(expectIndex);
                if (index > exportWindow.DataGridReExport.RowCount)
                {
                    index = exportWindow.DataGridReExport.RowCount;
                    expectIndex = index.ToString();
                }
                exportWindow.RdbtnReExport.Click(--index);
                Thread.Sleep(600);
                string ExpectQuantity = exportWindow.DataGridReExport.GetValue(index, 3);
                string ExpectPath = exportWindow.Path.Text;
                TextLog.LogMessage("Get Expect Value from DataGrid : [" + ExpectQuantity + "] \n " + ExpectPath);
                exportWindow.ViewButton.Click();
                Thread.Sleep(1000);
                string ExpectCell = exportWindow.ExportView.DataGridViewFile.GetValue(0, 1);
                string ActualQuantity1 = exportWindow.ExportView.DataGridViewFile.RowCount.ToString();
                exportWindow.ExportView.CloseBtn.Click();
                exportWindow.OKButton.Click();

                Thread.Sleep(500);
                ProgressBarisFinish(exportWindow.ProgressBar, 3);
                string ActualCell = exportWindow.DataGridSummary.GetValue(0, 1);
                string ActualQuantity2 = exportWindow.DataGridSummary.RowCount.ToString();
                string ActualPath = exportWindow.TextBoxFileName.Text;
                TextLog.LogMessage("Get Actual Value from DataGrid : [View: " + ActualQuantity1 + " and Summary: " + ActualQuantity2 + "] \n " + ActualPath);
                // ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Path"] 

                resultCell = Helper.CompareTwoStr(ExpectCell, ActualCell);
                resultPath = Helper.CompareTwoStr(ExpectPath, ActualPath);
                resultQuantity = Helper.CompareTwoStr(ExpectQuantity, ActualQuantity2);
                Thread.Sleep(600);
                exportWindow.CloseButton.Click();
            }
            else
            {
                TextLog.LogMessage("There not available key in the DataGridByQuantity!");
                Helper.CloseApp(exportWindow.MainElement);
            }
            bool result = resultCell && resultPath && resultQuantity == true ? true : false;
            Verification.AssertKMTResponse(result, ReaderXML.CurrentCase + "Test case ");

        }

        /// <summary>
        /// Export Encrpted Keys
        /// </summary>
        /// <param name="mainWindow"></param>
        public static void ExportEncryptedKeys(MainWindow mainWindow, string windowTitle, exportType type)
        {
            ExportKeys(mainWindow, windowTitle, type, true);
        }

        /// <summary>
        /// Import Keys from ULS
        /// </summary>
        /// <param name="mainWindow"></param>
        public static void ImportKeyFromULS(MainWindow mainWindow, string windowTitle, importType type)
        {
            bool TestResult = false;
            mainWindow.Ribbon.ImportButton.Click();
            ImportKeyWizard importWindow = new ImportKeyWizard(mainWindow.MainElement, windowTitle);
            Thread.Sleep(1000);
            TextLog.LogMessage("Launch Import Window " + mainWindow.Title);

            switch (type)
            {
                case importType.ImportKeyOrCBR:
                    importWindow.ImportKeysCBR.Click();
                    break;
                case importType.ImportDupCBR:
                    importWindow.ImportDupCBR.Click();
                    break;
                case importType.ImportOAFile:
                    importWindow.ImportOAtool.Click();
                    break;
                default:
                    break;
            }

            Thread.Sleep(1000);
            TextLog.LogMessage("Choose Import Keys/CBR.");
            importWindow.NextButton.Click();
            if (ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Path"].Trim() == "")
            {
                TextLog.LogMessage("There are not any Data in the Path!");
                ReaderXML.testModule.Case[ReaderXML.CurrentCase].Result = TestResult == true ? TestCase.CaseResult.Pass : TestCase.CaseResult.Fail;
                Verification.AssertKMTResponse(TestResult, ReaderXML.CurrentCase + " Test case ");
                Helper.CloseApp(importWindow.MainElement);
                return;
            }
            importWindow.PathTextbox.SetValue(ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Path"]);
            TextLog.LogMessage("Set value to Path textBox.");
            Thread.Sleep(1000);
            importWindow.MainElement.SetFocus();
            if (importWindow.OKButton.isEnable == false)
            {
                int i = 0;
                do
                {
                    Thread.Sleep(1000);
                    i = i + 500;
                } while (importWindow.OKButton.isEnable == false && i < 5000);
                importWindow.MainElement.SetFocus();
            }
            if (importWindow.OKButton.isEnable == false)
            {
                TextLog.LogMessage("OK button is disable!!!");
                importWindow.CloseButton.Click();
                Verification.AssertKMTResponse(TestResult, ReaderXML.CurrentCase + " Test case ");
                return;
            }
            importWindow.OKButton.Click();
            TextLog.LogMessage("Import in progess....");
            Thread.Sleep(1000);
            ProgressBarisFinish(importWindow.ProgressBar, 3);
            string ActualcellVaule = "";
            string ExpectcellVaule = ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Key"];
            string ExpectFailReason = "";
            string ActualFailReason = "";
            string ActualTotal = "";
            string ExpectTotal = ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Quantity"];
            bool ValueResult = false;
            bool TotalResult = false;
            TextLog.LogMessage("Get Actual Cell from DataGrid : " + ExpectcellVaule + "\n" + ExpectFailReason);

            if (importWindow.DataGridSummary.GridPattern.Current.RowCount > 0)
            {
                ActualcellVaule = importWindow.DataGridSummary.GetValue(0, 1);
                TextLog.LogMessage("Get Actual Cell from DataGrid : " + ActualcellVaule);
                ActualTotal = importWindow.LabelTotalKeys.text.Trim();
                ExpectTotal = importWindow.DataGridSummary.GridPattern.Current.RowCount.ToString();
                ActualFailReason = importWindow.DataGridSummary.GetValue(0, 3);
                TextLog.LogMessage("Get Total from DataGrid , \n Actual: " + ActualTotal + " \n Expect: " + ExpectTotal);
                TextLog.LogMessage("Get FailReason from DataGrid , \n Actual: " + ActualFailReason + " \n Expect: " + ExpectFailReason);
                ValueResult = Helper.CompareTwoStr(ActualcellVaule, ExpectcellVaule);
                TotalResult = Helper.CompareTwoStr(ActualTotal, ExpectTotal);
                bool ReasonReasult = Helper.CompareTwoStr(ActualFailReason, ExpectFailReason);
                TextLog.LogMessage("Compare Total keys and Produc key.");
            }
            else
            {
                TextLog.LogMessage("There are not key in the Summary DataGrid!");

            }

            TestResult = ValueResult && TotalResult == true ? true : false;
            Verification.AssertKMTResponse(TestResult, ReaderXML.CurrentCase + " Test case ");
            importWindow.CloseButton.Click();
        }

        /// <summary>
        /// Import Wrong Format XML file
        /// </summary>
        public static void ImportWrongFormat(MainWindow mainWindow, string windowTitle)
        {
            mainWindow.Ribbon.ImportButton.Click();
            ImportKeyWizard importWindow = new ImportKeyWizard(mainWindow.MainElement, windowTitle);
            Thread.Sleep(1000);
            TextLog.LogMessage("Launch Import Window " + mainWindow.Title);
            importWindow.ImportKeysCBR.Click();
            Thread.Sleep(1000);
            importWindow.MainElement.SetFocus();
            TextLog.LogMessage("Choose Import Keys/CBR.");
            importWindow.NextButton.Click();
            importWindow.PathTextbox.SetValue(@"C:\Program Files (x86)\DIS Solution\OEM\Key Management Tool\ExportKey Files\Keys_2011_11_07_05_13_42.xml");
            TextLog.LogMessage("Set value to Path textBox.");
            importWindow.MainElement.SetFocus();
            Thread.Sleep(1000);
            importWindow.OKButton.Click();
            ProgressBarisFinish(importWindow.ProgressBar, 3);
            string ExpectErrorStr = "";
            Thread.Sleep(1000);
            string ActualErrorStr = importWindow.MessageBoxError.Text;
            TextLog.LogMessage("Compare two string  : Expect string [" + ExpectErrorStr + "], Actual string [" + ActualErrorStr + "]");
            bool Result = Helper.CompareTwoStr(ExpectErrorStr, ActualErrorStr) == true ? true : false;
            Verification.AssertKMTResponse(Result, MethodBase.GetCurrentMethod().Name + "Test case ");
            importWindow.CloseButton.Click();
        }
        /// <summary>
        /// Assing keys to DLS
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="windowTitle"></param>
        public static void AssignKeys(MainWindow mainWindow, ref bool resultCell, string AssignNum, string Produckey)
        {
            mainWindow.Ribbon.AssignKeys.Click();
            Thread.Sleep(1000);
            AssignKeyWizard assignKeyWindow = new AssignKeyWizard(mainWindow.MainElement, "Assign Keys");
            TextLog.LogMessage("Loaded the AssignKeys Window of " + mainWindow.Title);
            string ActualCell = "";
            if (assignKeyWindow.DataGridByQuantity.RowCount > 0)
            {
                int maxNum = Convert.ToInt32(assignKeyWindow.DataGridByQuantity.GetValue(0, 3));
                int inputNum = Convert.ToInt32(AssignNum);
                if (inputNum > maxNum)
                {
                    inputNum = maxNum;
                    AssignNum = maxNum.ToString();
                    TextLog.LogMessage("Input Keys quantity greater than Max number.");
                }
                assignKeyWindow.DataGridByQuantity.SetValue(0, "2");

                Thread.Sleep(1000);
                assignKeyWindow.Tab.SelectItem(1);
                string ExpectCell = "";
                if (assignKeyWindow.DataGridByID.GridPattern.Current.RowCount > 0)
                {
                    if (Produckey != "" && productKey != null)
                    {
                        for (int i = 0; i < assignKeyWindow.DataGridByID.GridPattern.Current.RowCount; i++)
                        {
                            if (assignKeyWindow.DataGridByID.GetValue(i, 2).Trim() == Produckey.Trim())
                            {
                                ActualCell = assignKeyWindow.DataGridByID.GetValue(i, 2).Trim();
                                TextLog.LogMessage("has find the produc Key from DataGrid : [" + ActualCell + "]");
                                assignKeyWindow.DataGridByID.CheckItem(++i);
                            }
                        }
                    }
                    else
                    {
                        assignKeyWindow.DataGridByID.CheckItem(1);
                        ExpectCell = assignKeyWindow.DataGridByID.GetValue(0, 2);
                        CommTestCase.productKey = ExpectCell;
                        TextLog.LogMessage("Get Expect Value from DataGrid : [" + ExpectCell + "]");
                    }

                }
                assignKeyWindow.OKButton.Click();
                Thread.Sleep(1000);
                ProgressBarisFinish(assignKeyWindow.ProgressBar, 3);
                Thread.Sleep(600);
                ActualCell = assignKeyWindow.DataGridSummary.GetValue(0, 1);
                TextLog.LogMessage("Get Actual Value from DataGrid : [" + ActualCell + "] ");
                resultCell = Helper.CompareTwoStr(ExpectCell, ActualCell);
                Produckey = ActualCell;

                string Total = SuccNum(assignKeyWindow.LibararySummary.text);

                if (Convert.ToInt32(Total) == inputNum)
                {
                    TextLog.LogMessage("The Eexpect number equal to Actual number [" + AssignNum + "]");
                    resultCell = true;
                    assignKeyWindow.CloseButton.Click();
                }
                else
                {
                    TextLog.LogMessage("The quantity of Keys not equal");
                }
            }
            else
            {
                TextLog.LogMessage("No keys in the DataGrid by Quantity of AssignKeys window!");
            }

         
        }

        /// <summary>
        /// Unassign keys from DLS
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="windowTitle"></param>
        public static void UnassignKeys(MainWindow mainWindow, string windowTitle)
        {
            mainWindow.Ribbon.UnassignKeys.Click();
            Thread.Sleep(1000);
            UnassignKeyWizard unassignWindow = new UnassignKeyWizard(mainWindow.MainElement, windowTitle);
            TextLog.LogMessage("Loaded the Unassign Window of " + mainWindow.Title);
            Thread.Sleep(300);
            unassignWindow.NextButton.Click();
            bool resultCell = false;
            bool resultNum = false;
            bool result = false;
            if (unassignWindow.Datagrid.RowCount > 0)
            {
                int UnassignNum = Convert.ToInt32(ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["AssignNum"]);
                for (int i = 1; i < unassignWindow.Datagrid.RowCount + 1; i++)
                {
                    unassignWindow.Datagrid.CheckItem(i);
                }
                string ExpectCell = unassignWindow.Datagrid.GetValue(0, 2);
                string ExpectNum = ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["AssignNum"];
                TextLog.LogMessage("Get Expect Cell from DataGrid : " + ExpectCell + "\n Expect Unassign Num :" + ExpectNum);
                Thread.Sleep(1000);
                unassignWindow.OKButton.Click();
                Thread.Sleep(1000);
                ProgressBarisFinish(unassignWindow.ProgressBar, 3);
                string actualCell = unassignWindow.DataGridSummary.GetValue(0, 1);
                string ActualNum = unassignWindow.Datagrid.RowCount.ToString();
                TextLog.LogMessage("Get Actual Cell from DataGrid : " + actualCell + "\n Actual Unassign Num: " + ActualNum);
                TextLog.LogMessage("Compare Product Key....");
                resultCell = Helper.CompareTwoStr(ExpectCell, actualCell);
                resultNum = Helper.CompareTwoStr(ExpectNum, ActualNum);
                unassignWindow.CloseButton.Click();
            }
            else
            {
                TextLog.LogMessage("No keys in the Unassign keys DataGrid.");
                Helper.CloseApp(unassignWindow.MainElement);
            }
            result = resultCell && resultNum == true ? true : false;
            Verification.AssertKMTResponse(result, MethodBase.GetCurrentMethod().Name + "Test case ");

        }

        /// <summary>
        /// Report keys to ULS (Online)
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="windowTitle"></param>
        public static void ReportKeys(MainWindow mainWindow, string ReportNum, ref bool ResultCell, string productKey)
        {
            mainWindow.Ribbon.ReportKeys.Click();
            Thread.Sleep(1000);
            ReportKeyWizard reportWindow = new ReportKeyWizard(mainWindow.MainElement, "Report Keys");
            TextLog.LogMessage("Loaded the Report Window of " + mainWindow.Title);
            string expectCell = ReportNum;
            string ActualProductkey = "";
            int expectNum = Convert.ToInt32(expectCell);
            bool ResultErrorMsg = true;
            //Set value
            reportWindow.Tab.SelectItem(0);
            if (reportWindow.DataGridByQuantity.GridPattern.Current.RowCount > 0)
            {
                reportWindow.DataGridByQuantity.SetValue(0, expectCell);
                Thread.Sleep(1000);
                reportWindow.Tab.SelectItem(1);
                if (reportWindow.DataGridByID.GridPattern.Current.RowCount > 0)
                {
                    if (productKey != "")
                    {
                        for (int i = 0; i < reportWindow.DataGridByID.GridPattern.Current.RowCount; i++)
                        {
                            if (reportWindow.DataGridByID.GetValue(i, 2).Trim() == productKey.Trim())
                            {
                                ActualProductkey = reportWindow.DataGridByID.GetValue(i, 2).Trim();
                                TextLog.LogMessage("Has find the produc Key from DataGrid : [" + ActualProductkey + "]");
                                reportWindow.DataGridByID.CheckItem(++i);
                                ResultCell = true;
                            }
                        }
                    }
                    else
                    {
                        if (expectNum > reportWindow.DataGridByID.RowCount)
                        {
                            expectNum = reportWindow.DataGridByID.RowCount;
                            expectCell = expectNum.ToString();
                        }
                        for (int i = 1; i < expectNum + 1; i++)
                        {
                            reportWindow.DataGridByID.CheckItem(i);
                        }
                    }

                    Thread.Sleep(1000);
                    reportWindow.OKButton.Click();
                    Thread.Sleep(100);

                    ProgressBarisFinish(reportWindow.ProgressBar, 3);
                    string Actualcell = reportWindow.DataGrid.RowCount.ToString();
                    ResultCell = Helper.CompareTwoStr(expectCell, Actualcell);
                    TextLog.LogMessage("Compare two cell value, Expect :[" + expectCell + "] Actual:[" + Actualcell + "]");
                    reportWindow.CloseButton.Click();
                }
                else
                {
                    TextLog.LogMessage("There not available key in the DataGridByID!");
                    //  Helper.CloseApp(reportWindow.MainElement);
                }

            }
            else
            {
                TextLog.LogMessage("There not available key in the DataGridByQuantity!");
                // Helper.CloseApp(reportWindow.MainElement);
            }

            bool result = ResultCell && ResultErrorMsg == true ? true : false;
        }

        /// <summary>
        /// Recall keys to ULS
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="windowTitle"></param>
        public static void RecallKeys(MainWindow mainWindow, ref bool resultCell, string productKey)
        {
            int preTotal = Convert.ToInt32(mainWindow.TextBlockTotal.Text);
            mainWindow.Ribbon.RecallButton.Click();
            Thread.Sleep(1000);
            RecallKeyWindow reportWindow = new RecallKeyWindow(mainWindow.MainElement, "Recall Keys");
            TextLog.LogMessage("Loaded the Recall Window of " + mainWindow.Title);
            TextLog.LogMessage("Recall expect produckey,[" + productKey + "]");
            string ActualCell = "";
            //Set value
            reportWindow.Tab.SelectItem(1);
            ProgressBarisFinish(mainWindow.ProgressBar, 3);
            if (reportWindow.DataGridByID.GridPattern.Current.RowCount > 0)
            {
                if (productKey != "")
                {
                    for (int i = 0; i < reportWindow.DataGridByID.GridPattern.Current.RowCount; i++)
                    {
                        if (reportWindow.DataGridByID.GetValue(i, 2).Trim() == productKey.Trim())
                        {
                            ActualCell = reportWindow.DataGridByID.GetValue(i, 2).Trim();
                            TextLog.LogMessage("Has find the produc Key from DataGrid : [" + ActualCell + "]");
                            reportWindow.DataGridByID.CheckItem(++i);
                        }
                    }
                }
                else
                {
                    ActualCell = reportWindow.DataGridByID.GetValue(0, 2).Trim();
                    TextLog.LogMessage("Get the fist produc Key from DataGrid : [" + ActualCell + "]");
                    reportWindow.DataGridByID.CheckItem(1);
                }
            }
            else
            {
                TextLog.LogMessage("No key in the DataGrid!");
            }

            if (ActualCell == "")
            {
                TextLog.LogMessage("Not find the expect product key in the DataGrid!");
            }
            else
            {
                reportWindow.OKButton.Click();
                Thread.Sleep(1000);
                ProgressBarisFinish(reportWindow.ProgressBar, 3);
                reportWindow.CloseButton.Click();
                resultCell = true;
                TextLog.LogMessage("Recall Actual produckey,[" + ActualCell + "]");
            }
          
        }

        /// <summary>
        /// Revert keys
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="windowTitle"></param>
        public static void RevertKeys(MainWindow mainWindow, string RevertNum, ref bool result, string productKey)
        {
            mainWindow.Ribbon.RevertButton.Click();
            Thread.Sleep(1000);
            RevertKeyWindow reportWindow = new RevertKeyWindow(mainWindow.MainElement, "Revert Keys");
            TextLog.LogMessage("Loaded the Revert Window " + mainWindow.Title);
            string ActualCell = "";
            bool resultCell = false;
            bool resultmsg = false;
            //Set value
            if (reportWindow.DataGrid.GridPattern.Current.RowCount > 0)
            {
                int ReverNum = Convert.ToInt32(RevertNum);
                int maxNum = reportWindow.DataGrid.RowCount;
                if (ReverNum > maxNum)
                {
                    ReverNum = maxNum;
                    RevertNum = ReverNum.ToString();
                }
                if (productKey != "")
                {
                    for (int i = 0; i < reportWindow.DataGrid.GridPattern.Current.RowCount; i++)
                    {
                        if (reportWindow.DataGrid.GetValue(i, 2).Trim() == productKey.Trim())
                        {
                            ActualCell = reportWindow.DataGrid.GetValue(i, 2).Trim();
                            TextLog.LogMessage("Has find the produc Key from DataGrid : [" + ActualCell + "]");
                            reportWindow.DataGrid.CheckItem(++i);
                            resultCell = true;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < ReverNum + 1; i++)
                    {
                        reportWindow.DataGrid.CheckItem(1);
                    }
                    resultCell = true;
                }
                Thread.Sleep(800);
                reportWindow.OKButton.Click();
                Thread.Sleep(600);
                reportWindow.OKButton.Click();
                Thread.Sleep(1000);

                string Expectmsg = "please input operate message!";
                string Actualmsg = reportWindow.MsgWaring.Text;
                reportWindow.MsgWaring.ClickOK();
                reportWindow.TxtBoxReason.SetValue("Reason for test!");
                reportWindow.OKButton.Click();
                Thread.Sleep(1000);
                ProgressBarisFinish(reportWindow.ProgressBar, 3);
                resultmsg = Helper.CompareTwoStr(Expectmsg, Actualmsg);
                TextLog.LogMessage("Compare two Error Message, Expect :[" + Expectmsg + "] \n Actual:[" + Actualmsg + "]");
                reportWindow.CloseButton.Click();
            }
            else
            {
                TextLog.LogMessage("There not available key in the DataGrid!");

            }
            result = resultCell && resultmsg == true ? true : false;
            Verification.AssertKMTResponse(result, MethodBase.GetCurrentMethod().Name + "Test case ");
        }

        /// <summary>
        /// The Search of Main window 
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="windowTitle"></param>
        public static void MainWindowSearch(MainWindow mainWindow)
        {

            TextLog.LogMessage("====Test case of Search in the Main Window :");
            mainWindow.Status.ExpandCollapse(ComboBox.isexpand.Expand);
            int index = Convert.ToInt32(ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["Status"]);
            mainWindow.Status.SelectItem(index);
            mainWindow.SearchButton.Click();
            Thread.Sleep(1000);
            ProgressBarisFinish(mainWindow.ProgressBar, 3);
            TextLog.LogMessage("Choose first item in the Key list.");
            if (mainWindow.DatagridKeyList.GridPattern.Current.RowCount > 0)
            {
                string ExpectOEMPoNum = ReaderXML.testModule.Case[ReaderXML.CurrentCase].Input["OEMPoNum"];
                mainWindow.DatagridKeyList.SelectItem(0);
                string ActualOEMPONum = mainWindow.KeyDetail.GetValue(6, 1);
                TextLog.LogMessage("ExpectOEMPoNum Value : " + ExpectOEMPoNum);
                TextLog.LogMessage("Get Actual Value from DataGrid : " + ActualOEMPONum);
                bool result = WPFAutomation.Core.Helper.CompareTwoStr(ExpectOEMPoNum, ActualOEMPONum);
                Verification.AssertKMTResponse(result, ReaderXML.CurrentCase + " Test case");
            }
            else
            {
                TextLog.LogMessage("There are not search result in the Key list.");
            }
            TextLog.LogMessage("====Test case of Search in the Main Window. *End*");
            TextLog.LogMessage("Closed Main Window");
        }

        /// <summary>
        /// 
        /// </summary>
        public static void MainWindowDataGrid(MainWindow mainWindow)
        {
            bool result = false;
            TextLog.LogMessage("Choose first item in the Key list.");
            if (mainWindow.DatagridKeyList.GridPattern.Current.RowCount > 0)
            {
                mainWindow.ComboboxPage.ExpandCollapse(ComboBox.isexpand.Expand);
                int pageNum = Convert.ToInt32(mainWindow.ComboboxPage.SelectItem(2));
                TextLog.LogMessage("Get Expect Value from DataGrid : " + pageNum);
                Thread.Sleep(1000);
                ProgressBarisFinish(mainWindow.ProgressBar, 3);
                string ActualVaule = mainWindow.DatagridKeyList.GridPattern.Current.RowCount.ToString();
                TextLog.LogMessage("Get Actual Value from DataGrid : " + ActualVaule);
                int TotalKeys = Convert.ToInt32(mainWindow.TextBlockTotal.Text);
                TextLog.LogMessage("Get Total Keys from DataGrid : " + TotalKeys);
                if (TotalKeys < pageNum)
                {
                    TextLog.LogMessage("Total Keys less than page num!");
                    result = WPFAutomation.Core.Helper.CompareTwoStr(TotalKeys.ToString(), ActualVaule);
                }
                else
                {
                    result = WPFAutomation.Core.Helper.CompareTwoStr(pageNum.ToString(), ActualVaule);
                }
            }
            else
            {
                TextLog.LogMessage("There are not search result in the Key list.");
            }
            Verification.AssertKMTResponse(result, MethodBase.GetCurrentMethod().Name + " Test case");

        }

        /// <summary>
        /// 
        /// </summary>
        enum KeyStatus { Fulfilled, Assigned, Retrieved, Bound, ReportedBound };

        /// <summary>
        /// Get keys from ULS
        /// </summary>
        /// <param name="mainWindow"></param>
        public static void GetKeys(MainWindow mainWindow, string AssignNum, ref bool result)
        {
            int preTotal = 0;
            try
            {
                preTotal = Convert.ToInt32(mainWindow.TextBlockTotal.Text);
            }
            catch (Exception)
            {
            }

            mainWindow.Ribbon.GetKeys.Click();
            Thread.Sleep(1000);
            ProgressBarisFinish(mainWindow.ProgressBar, 3);
            mainWindow.MsgBoxGetKeys.ClickOK();
            int getNum = Convert.ToInt32(mainWindow.TextBlockTotal.Text) - preTotal;
            TextLog.LogMessage("Get [" + getNum.ToString() + "] Keys from ULS.");
            if (getNum == Convert.ToInt32(AssignNum))
            {
                result = true;
            }
            else
            {
                TextLog.LogMessage("Expect key :" + AssignNum.ToString() + "\n Actual Key :" + getNum.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="timeout"></param>
        private static void ProgressBarisFinish(ProgressBar progressBar, int timeout)
        {
            int i = 0;
            if (progressBar.progressBar != null)
            {
                do
                {
                    progressBar.isFinish(timeout); i++;
                } while (progressBar.progressBar != null && i < 4);

            }
        }

        /// <summary>
        /// Change Keys status from Fulfilled to Bound
        /// </summary>
        public static void SimulationRegister(string productKey,string type)
        {
            try
            {
                if (productKey == "")
                {
                    return;
                }
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectKMT"].ConnectionString.Trim()))
                {
                      string sqlquery
                        = "update [FactoryFloorKeyStore].[dbo].[ProductKeyInfo] set ProductKeyStateID=3,ProductKeyState='Bound',HardwareID='Just for Test!' where ProductKey=N'" + productKey + "'";
                    switch (type)
                    {
                        case "Bound": sqlquery = "update [FactoryFloorKeyStore].[dbo].[ProductKeyInfo] set ProductKeyStateID=3,ProductKeyState='Bound',HardwareID='Just for Test!' where ProductKey=N'" + productKey + "'";
                            break;
                        case "Consumed": sqlquery = "update [FactoryFloorKeyStore].[dbo].[ProductKeyInfo] set ProductKeyStateID=2,ProductKeyState='Consumed',HardwareID='Just for Test!' where ProductKey=N'" + productKey + "'";
                            break;
                        
                    }
                 
                    SqlCommand comm = new SqlCommand(sqlquery, conn);
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                TextLog.LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="spendTiem"></param>
        public static void WriteSummary(string methodName, string spendTiem)
        {
            TextLog.LogMessage("");
            TextLog.LogMessage(
                "===================Test case :" + methodName + " | Spended Time: " + spendTiem + "s \n ===================");
            TextLog.LogMessage("");
        }

        /// <summary>
        /// Get Success number from summary
        /// </summary>
        /// <param name="Summary"></param>
        /// <returns></returns>

        private static string SuccNum(string Summary)
        {
            int i = 0;
            string Total = "";
            while (true)
            {
                if (char.IsDigit(Summary[i]))
                {
                    Total += Summary[i];
                }
                else if (Total.Trim() != "" && !char.IsDigit(Summary[i]))
                {
                    break;
                }
                ++i;
            }
            return Total;
        }

        /// <summary>
        /// 
        /// </summary>
        public enum exportType { ExportKeys, ExportCBR };

        /// <summary>
        /// 
        /// </summary>
        public enum importType { ImportKeyOrCBR, ImportDupCBR, ImportOAFile };

        public enum LoginType { Default, Success, Fail };

        public enum KMTType { OEM, TPI, FactoryFloor };
    }
}
