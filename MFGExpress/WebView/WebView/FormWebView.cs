using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Windows.Forms;
using System.Security.Permissions;

namespace WebView
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public partial class FormWebView : Form
    {
        public FormWebView()
        {
            InitializeComponent();

            this.webBrowserWebView.AllowWebBrowserDrop = false;
            this.webBrowserWebView.IsWebBrowserContextMenuEnabled = false;
            this.webBrowserWebView.WebBrowserShortcutsEnabled = false;
            this.webBrowserWebView.ObjectForScripting = this;

            // Uncomment the following line when you are finished debugging.
            //webBrowser1.ScriptErrorsSuppressed = true;
        }

        public string Uri { get; set; }

        public string XmlUri { get; set; }

        public string XsltUri { get; set; }

        public string Encoding { get; set; }

        public string AnchorParamName { get { return this.anchorParamName; } set {this.anchorParamName = value; } }

        public Dictionary<string, object> XsltArguments { get; set; }

        public Dictionary<string, object> XsltExtendedObjects { get; set; }

        private string anchorParamName = "anchor";

        private int startupMode = 0;

        public Dictionary<string, object> Settings = new Dictionary<string, object>()
        {
            {ModuleConfiguration.AppStateKey_AnchorParamName, "anchor"},
            {ModuleConfiguration.AppStateKey_Encoding, "" },
            {ModuleConfiguration.AppStateKey_StartupMode, ""},
            {ModuleConfiguration.AppStateKey_Uri, ""},
            {ModuleConfiguration.AppStateKey_XmlUri, ""},
            {ModuleConfiguration.AppStateKey_XsltArguments, null},
            {ModuleConfiguration.AppStateKey_XsltExtendedObjects, null},
            {ModuleConfiguration.AppStateKey_XsltUri, ""}
        };

        public void Navigate(string uri)
        {
            this.Uri = uri;
            this.webBrowserWebView.Navigate(uri);
            this.webBrowserWebView.Refresh();
        }

        public void DoClose()
        {
            this.Close();
        }

        public void DoSave(string FilePath)
        {
        }

        public void DoRun(string AppName, string Agrs)
        {
        }

        public void DoTransform(string Anchor, string XsltPath)
        {
            //MessageBox.Show(Anchor);

            if (!String.IsNullOrEmpty(Anchor))
            {
                if (this.XsltArguments == null)
                {
                    this.XsltArguments = new Dictionary<string, object>();
                }

                if (!this.XsltArguments.ContainsKey(this.anchorParamName))
                {
                    this.XsltArguments.Add(this.anchorParamName, Anchor);
                }
                else
                {
                    this.XsltArguments[this.anchorParamName] = Anchor;
                }
            }  

            if (!Path.IsPathRooted(XsltPath))
            {
                XsltPath = Path.GetDirectoryName(this.XsltUri) + "\\" + XsltPath;
            }

            //MessageBox.Show(XsltPath);

            this.transformDocument(this.XmlUri, XsltPath, this.XsltArguments, this.XsltExtendedObjects, this.Encoding);
        }

        public void Format()
        {
            this.transformDocument(this.XmlUri, this.XsltUri, this.XsltArguments, this.XsltExtendedObjects, this.Encoding);
        }

        private void transformDocument(string xmlUri, string xsltUri, IDictionary<string, object> xsltArgs, IDictionary<string, object> xsltExtObjs, string encoding)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(xmlUri);  

            XslCompiledTransform transform = new XslCompiledTransform();

            transform.Load(xsltUri);

            XsltArgumentList arguments = null;

            if (xsltArgs != null && xsltArgs.Count > 0)
            {
                if (arguments == null)
                {
                    arguments = new XsltArgumentList();
                }

                foreach (string key in xsltArgs.Keys)
                {
                    arguments.AddParam(key, String.Empty, xsltArgs[key]);
                }
            }

            if (xsltExtObjs != null)
            {
                if (arguments == null)
                {
                    arguments = new XsltArgumentList();
                }

                foreach (string key in xsltExtObjs.Keys)
                {
                    arguments.AddExtensionObject(key, xsltExtObjs[key]);
                }
            }

            MemoryStream stream = new MemoryStream();

            transform.Transform(xmlDocument, arguments, stream);

            byte[] bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);

            Encoding outputEncoding = String.IsNullOrEmpty(encoding) ? System.Text.Encoding.Default : System.Text.Encoding.GetEncoding(encoding);

            string result = outputEncoding.GetString(bytes);

            stream.Close();

            result = result.Substring(result.IndexOf("<"));
            result = result.Substring(0, result.LastIndexOf(">") + 1);

            //this.webBrowserReport.Url = new Uri(xmlUri);
            this.webBrowserWebView.DocumentText = result;
        }

        private void saveAppState()
        {
            string filePath = Application.StartupPath;

            if (!filePath.EndsWith("\\"))
            {
                filePath += "\\";
            }

            filePath += "appstate.json";

            this.Settings[ModuleConfiguration.AppStateKey_AnchorParamName] = this.AnchorParamName;
            this.Settings[ModuleConfiguration.AppStateKey_Encoding] = this.Encoding;
            this.Settings[ModuleConfiguration.AppStateKey_StartupMode] = this.startupMode;
            this.Settings[ModuleConfiguration.AppStateKey_Uri] = this.Uri;
            this.Settings[ModuleConfiguration.AppStateKey_XmlUri] = this.XmlUri;
            this.Settings[ModuleConfiguration.AppStateKey_XsltArguments] = this.XsltArguments;
            this.Settings[ModuleConfiguration.AppStateKey_XsltExtendedObjects] = this.XsltExtendedObjects;
            this.Settings[ModuleConfiguration.AppStateKey_XsltUri] = this.XsltUri;

            //byte[] bytes = Utility.JsonSerialize(this.Settings, new Type[] { typeof(Dictionary<string, object>) }, "root");//Utility.BinarySerialize(this.Settings);

            //using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            //{
            //    stream.Write(bytes, 0, bytes.Length);
            //}

            string jsonValue = Utility.JsonSerialize(this.Settings, typeof(Dictionary<string, object>));

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonValue);
                }
            }
        }

        private void loadAppState()
        {
            string filePath = Application.StartupPath;

            if (!filePath.EndsWith("\\"))
            {
                filePath += "\\";
            }

            filePath += "appstate.json";

            if (File.Exists(filePath))
            {
                //byte[] bytes = File.ReadAllBytes(filePath);

                //this.Settings = Utility.JsonDeserialize(bytes, typeof(Dictionary<string, object>), new Type[] { typeof(Dictionary<string, object>) }, "root") as Dictionary<string, object>; //Utility.BinaryDeserialize(bytes) as Dictionary<string, object>;

                string jsonValue = File.ReadAllText(filePath);

                this.Settings = Utility.JsonDeserialize<Dictionary<string, object>>(jsonValue) as Dictionary<string, object>;

                if (this.Settings != null)
                {
                    if (this.Settings.ContainsKey(ModuleConfiguration.AppStateKey_Uri))
                    {
                        this.Uri = (string)this.Settings[ModuleConfiguration.AppStateKey_Uri];
                    }
                    
                    this.XmlUri = (string)this.Settings[ModuleConfiguration.AppStateKey_XmlUri];
                    this.XsltUri = (string)this.Settings[ModuleConfiguration.AppStateKey_XsltUri];
                    this.Encoding = (string)this.Settings[ModuleConfiguration.AppStateKey_Encoding];
                    this.AnchorParamName = (string)this.Settings[ModuleConfiguration.AppStateKey_AnchorParamName];
                    this.XsltArguments = this.Settings[ModuleConfiguration.AppStateKey_XsltArguments] as Dictionary<string, object>;
                    this.XsltExtendedObjects = this.Settings[ModuleConfiguration.AppStateKey_XsltExtendedObjects] as Dictionary<string, object>;

                    if (this.Settings.ContainsKey(ModuleConfiguration.AppStateKey_StartupMode))
                    {
                        int.TryParse(this.Settings[ModuleConfiguration.AppStateKey_StartupMode].ToString(), out this.startupMode);
                    } 
                }
            }
        }

        private void webBrowserReport_FileDownload(object sender, EventArgs e)
        {
            //MessageBox.Show(e.ToString());
            
        }

        private void webBrowserReport_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            //MessageBox.Show(e.Url.AbsoluteUri);

            if (e.Url.AbsoluteUri.EndsWith("#close"))
            {
                e.Cancel = true;
                this.Close();
            }
        }

        private void FormWebView_Load(object sender, EventArgs e)
        {
            this.loadAppState();

            switch (this.startupMode)
            {
                case 0:
                    this.Navigate(this.Uri);
                    break;
                case 1:
                    this.Format();
                    break;
                default:
                    break;
            }
        }

        private void FormWebView_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.saveAppState();
        }
    }
}
