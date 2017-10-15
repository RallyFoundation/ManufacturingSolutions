using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MetroFramework;
using MetroFramework.Forms;
using Gecko;
using Newtonsoft.Json;

namespace WebViewPlus
{
    public partial class FormWebView : MetroForm
    {
        public FormWebView()
        {
            InitializeComponent();

            this.initGeckoComponent();
            this.initGeckoWebBrowser();
        }

        public FormWebView(MetroForm Caller, string Url)
        {
            InitializeComponent();

            this.caller = Caller;
            this.url = Url;
            this.initGeckoComponent();
            this.initGeckoWebBrowser();
        }

        public void Navigate(string Url)
        {
            this.url = Url;
            this.geckoWebBrowser.Navigate(this.url);
        }

        public void Transform()
        {
            
        }

        public string Url { get { return this.url; } }

        private string appRootDir;
        private string url;
        private GeckoWebBrowser geckoWebBrowser;

        private MetroForm caller;

        private void initGeckoComponent()
        {
            appRootDir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(appRootDir, "FireFox"));
        }

        private void initGeckoWebBrowser()
        {
            this.geckoWebBrowser = new GeckoWebBrowser();
            this.geckoWebBrowser.Dock = DockStyle.Fill;
            this.geckoWebBrowser.Name = "GeckoBrowser";
            this.geckoWebBrowser.DocumentCompleted += GeckoWebBrowser_DocumentCompleted;
            this.geckoWebBrowser.Parent = this;
        }

        private void uploadFile(string fileInfoJson)
        {
            string jsonValue = fileInfoJson;

            jsonValue = jsonValue.Substring(jsonValue.IndexOf("{"));
            jsonValue = jsonValue.Substring(0, (jsonValue.LastIndexOf("}") + 1));

            JsonSerializer serializer = new JsonSerializer();
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonValue));
            ViewModels.FileViewModel fileInfo = serializer.Deserialize(reader, typeof(ViewModels.FileViewModel)) as ViewModels.FileViewModel;

            if (fileInfo.Url.EndsWith("/FFU"))
            {
                this.openFileDialogFile.Filter = "Full Flash Update (FFU) Image Files (*.ffu)|*.ffu|All Files (*.*)|*.*";
            }
            else
            {
                this.openFileDialogFile.Filter = "Windows Image Files (*.wim)|*.wim|All Files (*.*)|*.*";
            }

            if (this.openFileDialogFile.ShowDialog() == DialogResult.OK)
            {
                fileInfo.Path = this.openFileDialogFile.FileName;

                FormFileUploadProgress formFileUpload = new FormFileUploadProgress(fileInfo);

                if (formFileUpload.ShowDialog(this) == DialogResult.OK)
                {
                    string message = String.Format("File \"{0}\" successfully uploaded to \"{1}\"!", fileInfo.Path, fileInfo.Url);

                    if (MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        //using (Gecko.AutoJSContext context = new AutoJSContext(this.geckoWebBrowser.Window))
                        //{
                        //    context.EvaluateScript("RefreshData();", this.geckoWebBrowser.Window.DomWindow);
                        //}
                    }
                }
            }
        }

        private void writeFile(string fileInfoJson)
        {
            string jsonValue = fileInfoJson;

            jsonValue = jsonValue.Substring(jsonValue.IndexOf("{"));
            jsonValue = jsonValue.Substring(0, (jsonValue.LastIndexOf("}") + 1));

            JsonSerializer serializer = new JsonSerializer();
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonValue));
            ViewModels.FileViewModel fileInfo = serializer.Deserialize(reader, typeof(ViewModels.FileViewModel)) as ViewModels.FileViewModel;

            if (String.IsNullOrEmpty(fileInfo.Path))
            {
                SaveFileDialog fileDialog = new SaveFileDialog();

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileInfo.Path = fileDialog.FileName;
                } 
            }

            if (!Path.IsPathRooted(fileInfo.Path))
            {
                if (!fileInfo.Path.StartsWith("\\"))
                {
                    fileInfo.Path = "\\" + fileInfo.Path;
                }

                fileInfo.Path = appRootDir + fileInfo.Path;
            }

            using (FileStream fileStream = new FileStream(fileInfo.Path, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(fileInfo.Content);
                }
            }

            MessageBox.Show(String.Format("Successfully write content to file \"{0}\"", fileInfo.Path), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveFile(string sourceFilePath)
        {
            string destFilePath = "";

            string fileFilter = "All Files(*.*)|*.*";

            if ((!String.IsNullOrEmpty(sourceFilePath)) && File.Exists(sourceFilePath))
            {
                string fileExtension = Path.GetExtension(sourceFilePath);

                //if (!String.IsNullOrEmpty(fileExtension))
                //{
                //    fileFilter = fileExtension.Substring(1) + " Files|(*" + fileExtension + ")|*" + fileExtension + "|" + fileFilter;
                //}

                SaveFileDialog fileDialog = new SaveFileDialog()
                {
                     Filter = fileFilter,
                     FileName = sourceFilePath,
                     OverwritePrompt = true,
                     DefaultExt = fileExtension,
                     AddExtension = true
                };

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    destFilePath = fileDialog.FileName;
                    File.Copy(sourceFilePath, destFilePath);

                    MessageBox.Show(String.Format("Successfully saved file \"{0}\" to \"{1}\"", sourceFilePath, destFilePath), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }        
        }

        private void runApp(string param)
        {
            string[] parameters = param.Split(new string[] { "|"}, StringSplitOptions.None);
            string appName = parameters[0], args = parameters[1];
            Utility.StartProcess(appName, args, true, false);
        }

        private void setDocumentElementAttribute(string argumentString)
        {
            string[] args = argumentString.Split(new string[] { "," }, StringSplitOptions.None);
            string filePath = args[0], xPath = args[1], attributeName = args[2], attributeValue = args[3];
            Utility.SetHtmlDocumentAttributeValue(filePath, xPath, attributeName, attributeValue);

            MessageBox.Show(String.Format("Successfully write content to file \"{0}\"", filePath), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GeckoWebBrowser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            this.Text = e.Window.Document.Title;
            this.Refresh();
        }

        private void FormWebView_Load(object sender, EventArgs e)
        {
            this.geckoWebBrowser.AddMessageEventListener("CloseWindow", (string param) => this.Close());
            this.geckoWebBrowser.AddMessageEventListener("ShowMessageInfoBox", (string param) => MessageBox.Show(param, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information));
            this.geckoWebBrowser.AddMessageEventListener("ShowMessageWarningBox", (string param) => MessageBox.Show(param, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning));
            this.geckoWebBrowser.AddMessageEventListener("ShowMessageErrorBox", (string param) =>  MessageBox.Show(param, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
            this.geckoWebBrowser.AddMessageEventListener("WriteFile", (string param) => this.writeFile(param));
            this.geckoWebBrowser.AddMessageEventListener("UploadFile", (string param) => this.uploadFile(param));
            this.geckoWebBrowser.AddMessageEventListener("SaveFile", (string param) => this.saveFile(param));
            this.geckoWebBrowser.AddMessageEventListener("RunApp", (string param) => this.runApp(param));
            this.geckoWebBrowser.AddMessageEventListener("SetDocumentAttribute", (string param) => this.setDocumentElementAttribute(param));

            if (!String.IsNullOrEmpty(this.url))
            {
                this.geckoWebBrowser.Navigate(this.url);
            }
        }

        private void FormWebView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.caller != null)
            {
                e.Cancel = true;
                this.caller.Visible = true;
                this.Visible = false;           
            }
        }
    }
}
