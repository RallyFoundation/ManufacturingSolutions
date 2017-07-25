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

namespace WDSManager
{
    public partial class FormWebView : MetroForm
    {
        public FormWebView(MetroForm Caller, string Url)
        {
            InitializeComponent();

            this.caller = Caller;
            this.url = Url;
            this.initCeckoComponent();
            this.initCeckoWebBrowser();
        }

        public void Navigate(string Url)
        {
            this.url = Url;
            this.geckoWebBrowser.Navigate(this.url);
        }

        public string Url { get { return this.url; } }

        private string appRootDir;
        private string url;
        private GeckoWebBrowser geckoWebBrowser;

        private MetroForm caller;

        private void initCeckoComponent()
        {
            appRootDir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(appRootDir, "FireFox"));
        }

        private void initCeckoWebBrowser()
        {
            this.geckoWebBrowser = new GeckoWebBrowser();
            this.geckoWebBrowser.Dock = DockStyle.Fill;
            this.geckoWebBrowser.Name = "GeckoBrowser";
            this.geckoWebBrowser.DocumentCompleted += GeckoWebBrowser_DocumentCompleted;
            this.geckoWebBrowser.Parent = this;
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

            MessageBox.Show("Done!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            this.geckoWebBrowser.Navigate(this.url);
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
