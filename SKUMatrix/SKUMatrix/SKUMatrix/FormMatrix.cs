using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using MetroFramework.Forms;
using Gecko;
using QA.Model;
using QA.Facade;
using QA.Utility;

namespace SKUMatrix
{
    public partial class FormMatrix : MetroForm
    {
        public FormMatrix()
        {
            InitializeComponent();

            this.initGeckoComponent();
            this.initGeckoWebBrowser();
            this.setDefaultUrl();
            this.initRepository();

            this.geckoWebBrowser.Navigate(this.url);
            this.metroTabControlMain.SelectedTab = this.metroTabPageHome;
        }

        public void Navigate(string Url)
        {
            this.url = Url;
            this.geckoWebBrowser.Navigate(this.url);
        }

        //private string oa3ToolReportFilePath = "";
        //private string logFilePath = "";

        private string currentPKPN = "";
        private string transactionId = "";
        private string outputPath = "Output";

        private string appRootDir;
        private string fxLibPath;
        private string url;
        private GeckoWebBrowser geckoWebBrowser;

        private void initGeckoComponent()
        {
            appRootDir = Path.GetDirectoryName(Application.ExecutablePath);
            fxLibPath = ConfigurationManager.AppSettings.Get("FxLibPath");
            Xpcom.Initialize(Path.Combine(appRootDir, fxLibPath));
        }

        private void initGeckoWebBrowser()
        {
            this.geckoWebBrowser = new GeckoWebBrowser();
            this.geckoWebBrowser.Dock = DockStyle.Fill;
            this.geckoWebBrowser.Name = "GeckoBrowser";
            this.geckoWebBrowser.DocumentCompleted += GeckoWebBrowser_DocumentCompleted;
            this.geckoWebBrowser.Parent = this.metroTabPageResult;
        }

        private void initRepository()
        {
            string outputFullPath = GetFullPath(this.outputPath);

            if (!Directory.Exists(outputFullPath))
            {
                Directory.CreateDirectory(outputFullPath);
            }
        }

        private void GeckoWebBrowser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            //this.Text = e.Window.Document.Title;
            //this.Refresh();
        }

        private void setDefaultUrl()
        {
            string defaultPageUrl = ConfigurationManager.AppSettings.Get("DefaultPage");

            if (!string.IsNullOrEmpty(defaultPageUrl) && !defaultPageUrl.StartsWith("file://") && !Path.IsPathRooted(defaultPageUrl))
            {
                defaultPageUrl = appRootDir + "\\" + defaultPageUrl;
            }

            this.url = defaultPageUrl;
        }

        private void metroTileOfflineCheck_Click(object sender, EventArgs e)
        {
            this.transactionId = Guid.NewGuid().ToString();

            if (this.openFileDialogDecoded4KHH.ShowDialog(this) == DialogResult.OK)
            {
                Global.DefaultDataPath = this.openFileDialogDecoded4KHH.FileName;
            }

            Facade.InstantiateInputData();

            if ((Facade.Data != null) && (Facade.Data.ContainsKey("ProductKeyPkPn")))
            {
                this.currentPKPN = Facade.Data["ProductKeyPkPn"].ToString();
                this.currentPKPN = currentPKPN.Substring((currentPKPN.IndexOf("]") + 1));

                string matrixFullPath = this.GetFullPath(String.Format("Matrix\\{0}\\matrix.json", this.currentPKPN));

                if (File.Exists(matrixFullPath))
                {
                    Global.DefaultMatrixConfigPath = matrixFullPath;
                    Facade.InitializeMatrix();

                    Facade.AddRule(new ValidationRuleItem() {  FieldName = "ProductKeyID", FieldValue = Facade.Data["ProductKeyID"].ToString(), GroupName = "Pricing", RuleType = RuleType.EqualTo, Enabled = true });

                    Facade.AddRule(new ValidationRuleItem() { FieldName = "ProductKeyPkPn", FieldValue = Facade.Data["ProductKeyPkPn"].ToString(), GroupName = "Pricing", RuleType = RuleType.EqualTo, Enabled = true });

                    Facade.ValidateData();    

                    if (Facade.Results != null)
                    {
                        string workDir = this.GetFullPath(String.Format("{0}\\{1}", this.outputPath, this.transactionId));

                        string resultXml = Facade.OutputResultXml();

                        string resultJson = JsonUtility.GetJsonFromXml(resultXml, true, false);

                        if (!String.IsNullOrEmpty(resultXml))
                        {
                            Directory.CreateDirectory(workDir);

                            string resultXmlPath = workDir + "\\Result.xml";

                            using (FileStream stream = new FileStream(resultXmlPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                            {
                                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                                {
                                    writer.Write(resultXml);
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(resultJson))
                        {
                            string resultJsonPath = workDir + "\\Result.json";

                            using (FileStream stream = new FileStream(resultJsonPath, FileMode.Create, FileAccess.Write, FileShare.Write))
                            {
                                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                                {
                                    writer.Write(resultJson);
                                }
                            }

                            using (FileStream stream = new FileStream("data.json", FileMode.Create, FileAccess.Write, FileShare.Write))
                            {
                                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                                {
                                    writer.Write("AppData=" + resultJson);
                                }
                            }
                        }

                        string matrixJson = "";

                        using (FileStream stream = new FileStream(matrixFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                matrixJson = reader.ReadToEnd();
                            }
                        }

                        using (FileStream stream = new FileStream("matrix.json", FileMode.Create, FileAccess.Write, FileShare.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                            {
                                writer.Write("Settings=" + matrixJson);
                            }
                        }

                        this.metroTabControlMain.SelectedTab = this.metroTabPageResult;
                        this.Navigate(this.url);
                    }
                }
                else
                {
                    string message = String.Format("The matrix for the PKPN of \"{0}\" does NOT exist! Please update the matrix data.", this.currentPKPN);
                    MessageBox.Show(message, "Matrix Does NOT Exist", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }        
            }
        }

        private string GetFullPath(string relativePath)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;

            if (rootPath.EndsWith("\\"))
            {
                rootPath = rootPath.Substring(0, (rootPath.Length - 1));
            }

            if (!relativePath.StartsWith("\\"))
            {
                relativePath = "\\" + relativePath;
            }

            return rootPath + relativePath;
        }

        //private string StartProcess(string AppPath, string AppParams, bool IsCreatingNewWindow, bool IsUsingShellExecute)
        //{
        //    Process process = new Process();

        //    process.StartInfo.FileName = AppPath;
        //    process.StartInfo.Arguments = AppParams;
        //    process.StartInfo.UseShellExecute = IsUsingShellExecute;
        //    process.StartInfo.RedirectStandardError = !IsUsingShellExecute;
        //    process.StartInfo.RedirectStandardOutput = !IsUsingShellExecute;
        //    process.StartInfo.CreateNoWindow = !IsCreatingNewWindow;

        //    process.Start();

        //    process.WaitForExit();

        //    string result = "";

        //    if (!IsUsingShellExecute)
        //    {
        //        using (process.StandardOutput)
        //        {
        //            result = process.StandardOutput.ReadToEnd();
        //        }
        //    }

        //    return result;
        //}

        //private void StartProcess(string AppPath, string AppParams, bool IsCreatingNewWindow, bool IsUsingShellExecute, string LogFileFullPath)
        //{
        //    Process process = new Process();

        //    process.StartInfo.FileName = AppPath;
        //    process.StartInfo.Arguments = AppParams;
        //    process.StartInfo.UseShellExecute = IsUsingShellExecute;
        //    process.StartInfo.RedirectStandardError = !IsUsingShellExecute;
        //    process.StartInfo.RedirectStandardOutput = !IsUsingShellExecute;
        //    process.StartInfo.CreateNoWindow = !IsCreatingNewWindow;

        //    if (!IsUsingShellExecute)
        //    {
        //        logFilePath = LogFileFullPath;

        //        process.OutputDataReceived += Process_OutputDataReceived;
        //        process.ErrorDataReceived += Process_ErrorDataReceived;
        //    }

        //    process.Start();

        //    process.BeginOutputReadLine();

        //    process.WaitForExit();

        //    //string result = "";

        //    //if (!IsUsingShellExecute)
        //    //{
        //    //    using (process.StandardOutput)
        //    //    {
        //    //        result = process.StandardOutput.ReadToEnd();
        //    //    }
        //    //}

        //    //return result;
        //}

        //private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(e.Data))
        //    {
        //        Console.WriteLine(e.Data);

        //        using (StreamWriter writer = File.AppendText(logFilePath))
        //        {
        //            writer.WriteLine(e.Data);
        //        }
        //    }
        //}

        //private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(e.Data))
        //    {
        //        Console.WriteLine(e.Data);
        //        //File.AppendAllText(LogFilePath, e.Data, Encoding.UTF8);

        //        using (StreamWriter writer = File.AppendText(logFilePath))
        //        {
        //            writer.WriteLine(e.Data);
        //        }
        //    }
        //}
    }
}
