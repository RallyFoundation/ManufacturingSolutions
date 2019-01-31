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
using System.Xml;
using Utility;
using VamtAPIConfigurator.ViewModels;

namespace VamtAPIConfigurator
{
    public partial class FormMain : Form
    {
        private string currentFilePath = "config.xml";

        public FormMain()
        {
            InitializeComponent();
        }

        public FormMain(string ConfigFilePath)
        {
            this.currentFilePath = ConfigFilePath;
            InitializeComponent();
        }

        private void saveConfig(string url, string domainName, string domainUserName,string domainPassword)
        {
            ConfigurationViewModel config = new ConfigurationViewModel()
            {
                VamtApiServicePoint = url,
                VamtDomainName = domainName,
                VamtDomainUserName = domainUserName,
                VamtDomainPassword = domainPassword
            };

            string xml = XmlUtility.XmlSerialize(config, null, "utf-8");

            using (FileStream stream = new FileStream(this.currentFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(xml);
                }
            }
        }

        private void loadConfig()
        {
            ConfigurationViewModel config = null;

            string xml = "<configurationItems/>";

            using (FileStream stream = new FileStream(this.currentFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    xml = reader.ReadToEnd();
                }
            }

            config = XmlUtility.XmlDeserialize(xml, typeof(ConfigurationViewModel), null, "utf-8") as ConfigurationViewModel;

            this.textBoxUrl.Text = config.VamtApiServicePoint;
            this.textBoxVamtDomainLDAPUrl.Text = config.VamtDomainName;
            this.textBoxDomainUserName.Text = config.VamtDomainUserName;
            this.textBoxVamtDomainUserPassword.Text = config.VamtDomainPassword;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.textBoxUrl.Text))
            {
                MessageBox.Show("Url is required!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }


            this.saveConfig(this.textBoxUrl.Text, this.textBoxVamtDomainLDAPUrl.Text, this.textBoxDomainUserName.Text, this.textBoxVamtDomainUserPassword.Text);

            string message = String.Format("VAMT API configuration settings successfully saved to \"{0}\"", this.currentFilePath);

            if (MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                this.statusStripLabel1.Text = this.currentFilePath;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.saveConfig(this.textBoxUrl.Text);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.loadConfig();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                object message = HttpUtility.Get(this.textBoxUrl.Text, new Authentication() { Type = AuthenticationType.Custom }, null);

                if (message != null)
                {
                    MessageBox.Show(message.ToString(), "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                string[] adParams = LdapUtility.ParseLdapUrl(this.textBoxVamtDomainLDAPUrl.Text);

                if ((adParams != null) && (adParams.Length > 0))
                {
                    string dn = "";
                    string server = adParams[0];
                    string user = this.textBoxDomainUserName.Text;
                    string pwd = this.textBoxVamtDomainUserPassword.Text;
                    string filter = String.Format("(&(objectCategory=person)(objectClass=user)(cn={0}))", user);

                    if (adParams.Length > 1)
                    {
                        for (int i = 1; i < adParams.Length; i++)
                        {
                            dn += "dc=" + adParams[i];

                            if (i != (adParams.Length - 1))
                            {
                                dn += ",";
                            }
                        }
                    }

                    var conn = LdapUtility.Connect(user, pwd, "ntlm", server, true, 4000);

                    var result = LdapUtility.Query(conn, filter, dn, "subtree");

                    if ((result != null) && (result is XmlDocument))
                    {
                        string dsml = (result as XmlDocument).InnerXml;

                        MessageBox.Show(dsml, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }      
        }
    }
}
