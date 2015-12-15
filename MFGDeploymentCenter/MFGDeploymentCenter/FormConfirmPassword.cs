using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace MfgSolutionsDeploymentCenter
{
    public partial class FormConfirmPassword : MetroForm
    {
        public FormConfirmPassword(List<object> ResultData)
        {
            InitializeComponent();

            this.resultData = ResultData;

            this.metroLabelDescription.Text += String.Format(" (.\\{0}):", Environment.UserName);
        }

        private List<object> resultData;

        private void metroButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.None;

            this.metroLabelWarning.Visible = false;

            string password = this.textBoxPassword.Text;

            string userName = Environment.UserName;

            if (!this.validatePassword(userName, password))
            {
                this.metroLabelWarning.Visible = true;

                return;
            }
            else
            {
                if (this.resultData != null)
                {
                    this.resultData.Add(userName);
                    this.resultData.Add(password);
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;

                this.Close();
            }
        }

        private bool validatePassword(string userName, string pwd) 
        {
            bool result = false;

            try
            {
                using (PrincipalContext context = new PrincipalContext(ContextType.Machine))
                {
                    result = context.ValidateCredentials(userName, pwd);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        private void metroToggleShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPassword.UseSystemPasswordChar = (!this.metroToggleShowPassword.Checked);
        }
    }
}
