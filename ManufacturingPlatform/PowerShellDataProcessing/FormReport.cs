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
//using MetroFramework.Forms;
//using Newtonsoft.Json;


namespace PowerShellDataProcessing
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();

            this.webBrowserReport.AllowWebBrowserDrop = false;
            this.webBrowserReport.IsWebBrowserContextMenuEnabled = false;
            this.webBrowserReport.WebBrowserShortcutsEnabled = false;
            this.webBrowserReport.ObjectForScripting = this;

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

        public void Navigate(string uri)
        {
            this.Uri = uri;
            //this.webBrowserReport.Url = new Uri(uri);
            this.webBrowserReport.Navigate(uri);
            this.webBrowserReport.Refresh();
        }

        public void DoClose()
        {
            this.Close();
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
            this.webBrowserReport.DocumentText = result;

            //MessageBox.Show(result);

            //this.webBrowserReport.Refresh();
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

            //if (e.Url.AbsoluteUri.StartsWith("#transform:"))
            //{
            //    e.Cancel = true;

            //    string tranformJsonString = e.Url.AbsoluteUri;
            //    tranformJsonString = tranformJsonString.Substring((tranformJsonString.IndexOf(":") + 1));

            //    string jsonValue = JsonConvert.ToString(tranformJsonString);

            //    jsonValue = jsonValue.Substring(jsonValue.IndexOf("{"));
            //    jsonValue = jsonValue.Substring(0, (jsonValue.LastIndexOf("}") + 1));

            //    JsonSerializer serializer = new JsonSerializer();
            //    JsonTextReader reader = new JsonTextReader(new StringReader(jsonValue));
            //    Object jsonObject = serializer.Deserialize(reader);

            //    TransformationParameter transParam = jsonObject as TransformationParameter;

            //    if ((transParam != null) && (transParam.XsltArguments != null))
            //    {
            //        if (this.XsltArguments == null)
            //        {
            //            this.XsltArguments = new Dictionary<string, object>();
            //        }

            //        foreach (string key in transParam.XsltArguments.Keys)
            //        {
            //            if (!this.XsltArguments.ContainsKey(key))
            //            {
            //                this.XsltArguments.Add(key, transParam.XsltArguments[key]);
            //            }
            //            else if(this.XsltArguments[key] != transParam.XsltArguments[key])
            //            {
            //                this.XsltArguments[key] = transParam.XsltArguments[key];
            //            }
            //        }
            //    }

            //    //this.XsltUri = 

            //    this.Format();
            //}

            //if (e.Url.AbsoluteUri.StartsWith("file:///") && e.Url.AbsoluteUri.EndsWith("#save"))
            //{
            //    e.Cancel = true;

            //    string sourcePath = e.Url.AbsoluteUri.Substring(8);
            //    sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf("#"));

            //    if (this.saveFileDialogSaveFile.ShowDialog() == DialogResult.OK)
            //    {
            //        string destPath = this.saveFileDialogSaveFile.FileName;

            //        //MessageBox.Show(sourcePath);

            //        //MessageBox.Show(destPath);

            //        File.Copy(sourcePath, destPath);

            //        MessageBox.Show(String.Format("File \"{0}\" successfully saved to: \"{1}\".", sourcePath, destPath), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
        }

        //private void FormReport_Load(object sender, EventArgs e)
        //{
        //    //this.webBrowserReport.AllowWebBrowserDrop = false;
        //    //this.webBrowserReport.IsWebBrowserContextMenuEnabled = false;
        //    //this.webBrowserReport.WebBrowserShortcutsEnabled = false;
        //    //this.webBrowserReport.ObjectForScripting = this;

        //    // Uncomment the following line when you are finished debugging.
        //    //webBrowser1.ScriptErrorsSuppressed = true;
        //}
    }
}
