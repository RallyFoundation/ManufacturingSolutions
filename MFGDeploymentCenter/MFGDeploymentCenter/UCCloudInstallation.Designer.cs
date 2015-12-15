namespace MfgSolutionsDeploymentCenter
{
    partial class UCCloudInstallation
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.metroLabelDISHome = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxDISHome = new MetroFramework.Controls.MetroTextBox();
            this.metroButtonBrowse = new MetroFramework.Controls.MetroButton();
            this.metroLabelDBInstance = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxDBInstance = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBoxDBLoginName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelDBLoginName = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxDBLoginPassword = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelDBLoginPassword = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroLabelDISHome
            // 
            this.metroLabelDISHome.AutoSize = true;
            this.metroLabelDISHome.Location = new System.Drawing.Point(4, 4);
            this.metroLabelDISHome.Name = "metroLabelDISHome";
            this.metroLabelDISHome.Size = new System.Drawing.Size(71, 19);
            this.metroLabelDISHome.TabIndex = 0;
            this.metroLabelDISHome.Text = "DIS Home:";
            // 
            // metroTextBoxDISHome
            // 
            this.metroTextBoxDISHome.Lines = new string[] {
        "C:\\Program Files\\DIS Solution\\v2.0"};
            this.metroTextBoxDISHome.Location = new System.Drawing.Point(8, 26);
            this.metroTextBoxDISHome.MaxLength = 32767;
            this.metroTextBoxDISHome.Name = "metroTextBoxDISHome";
            this.metroTextBoxDISHome.PasswordChar = '\0';
            this.metroTextBoxDISHome.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxDISHome.SelectedText = "";
            this.metroTextBoxDISHome.Size = new System.Drawing.Size(224, 23);
            this.metroTextBoxDISHome.TabIndex = 1;
            this.metroTextBoxDISHome.Text = "C:\\Program Files\\DIS Solution\\v2.0";
            this.metroTextBoxDISHome.UseSelectable = true;
            // 
            // metroButtonBrowse
            // 
            this.metroButtonBrowse.Location = new System.Drawing.Point(237, 26);
            this.metroButtonBrowse.Name = "metroButtonBrowse";
            this.metroButtonBrowse.Size = new System.Drawing.Size(75, 23);
            this.metroButtonBrowse.TabIndex = 2;
            this.metroButtonBrowse.Text = "Change...";
            this.metroButtonBrowse.UseSelectable = true;
            this.metroButtonBrowse.Click += new System.EventHandler(this.metroButtonBrowse_Click);
            // 
            // metroLabelDBInstance
            // 
            this.metroLabelDBInstance.AutoSize = true;
            this.metroLabelDBInstance.Location = new System.Drawing.Point(4, 61);
            this.metroLabelDBInstance.Name = "metroLabelDBInstance";
            this.metroLabelDBInstance.Size = new System.Drawing.Size(132, 19);
            this.metroLabelDBInstance.TabIndex = 3;
            this.metroLabelDBInstance.Text = " SQL Server Instance:";
            // 
            // metroTextBoxDBInstance
            // 
            this.metroTextBoxDBInstance.Lines = new string[] {
        "localhost"};
            this.metroTextBoxDBInstance.Location = new System.Drawing.Point(8, 84);
            this.metroTextBoxDBInstance.MaxLength = 32767;
            this.metroTextBoxDBInstance.Name = "metroTextBoxDBInstance";
            this.metroTextBoxDBInstance.PasswordChar = '\0';
            this.metroTextBoxDBInstance.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxDBInstance.SelectedText = "";
            this.metroTextBoxDBInstance.Size = new System.Drawing.Size(304, 23);
            this.metroTextBoxDBInstance.TabIndex = 4;
            this.metroTextBoxDBInstance.Text = "localhost";
            this.metroTextBoxDBInstance.UseSelectable = true;
            // 
            // metroTextBoxDBLoginName
            // 
            this.metroTextBoxDBLoginName.Lines = new string[] {
        "DIS"};
            this.metroTextBoxDBLoginName.Location = new System.Drawing.Point(8, 147);
            this.metroTextBoxDBLoginName.MaxLength = 32767;
            this.metroTextBoxDBLoginName.Name = "metroTextBoxDBLoginName";
            this.metroTextBoxDBLoginName.PasswordChar = '\0';
            this.metroTextBoxDBLoginName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxDBLoginName.SelectedText = "";
            this.metroTextBoxDBLoginName.Size = new System.Drawing.Size(304, 23);
            this.metroTextBoxDBLoginName.TabIndex = 6;
            this.metroTextBoxDBLoginName.Text = "DIS";
            this.metroTextBoxDBLoginName.UseSelectable = true;
            // 
            // metroLabelDBLoginName
            // 
            this.metroLabelDBLoginName.AutoSize = true;
            this.metroLabelDBLoginName.Location = new System.Drawing.Point(4, 124);
            this.metroLabelDBLoginName.Name = "metroLabelDBLoginName";
            this.metroLabelDBLoginName.Size = new System.Drawing.Size(158, 19);
            this.metroLabelDBLoginName.TabIndex = 5;
            this.metroLabelDBLoginName.Text = " SQL Server Login Name:";
            // 
            // metroTextBoxDBLoginPassword
            // 
            this.metroTextBoxDBLoginPassword.Lines = new string[] {
        "D!S@OMSG.msft"};
            this.metroTextBoxDBLoginPassword.Location = new System.Drawing.Point(8, 205);
            this.metroTextBoxDBLoginPassword.MaxLength = 32767;
            this.metroTextBoxDBLoginPassword.Name = "metroTextBoxDBLoginPassword";
            this.metroTextBoxDBLoginPassword.PasswordChar = '●';
            this.metroTextBoxDBLoginPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxDBLoginPassword.SelectedText = "";
            this.metroTextBoxDBLoginPassword.Size = new System.Drawing.Size(304, 23);
            this.metroTextBoxDBLoginPassword.TabIndex = 8;
            this.metroTextBoxDBLoginPassword.Text = "D!S@OMSG.msft";
            this.metroTextBoxDBLoginPassword.UseSelectable = true;
            this.metroTextBoxDBLoginPassword.UseSystemPasswordChar = true;
            // 
            // metroLabelDBLoginPassword
            // 
            this.metroLabelDBLoginPassword.AutoSize = true;
            this.metroLabelDBLoginPassword.Location = new System.Drawing.Point(4, 182);
            this.metroLabelDBLoginPassword.Name = "metroLabelDBLoginPassword";
            this.metroLabelDBLoginPassword.Size = new System.Drawing.Size(176, 19);
            this.metroLabelDBLoginPassword.TabIndex = 7;
            this.metroLabelDBLoginPassword.Text = " SQL Server Login Password:";
            // 
            // UCCloudInstallation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.metroTextBoxDBLoginPassword);
            this.Controls.Add(this.metroLabelDBLoginPassword);
            this.Controls.Add(this.metroTextBoxDBLoginName);
            this.Controls.Add(this.metroLabelDBLoginName);
            this.Controls.Add(this.metroTextBoxDBInstance);
            this.Controls.Add(this.metroLabelDBInstance);
            this.Controls.Add(this.metroButtonBrowse);
            this.Controls.Add(this.metroTextBoxDISHome);
            this.Controls.Add(this.metroLabelDISHome);
            this.Name = "UCCloudInstallation";
            this.Size = new System.Drawing.Size(318, 243);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabelDISHome;
        private MetroFramework.Controls.MetroTextBox metroTextBoxDISHome;
        private MetroFramework.Controls.MetroButton metroButtonBrowse;
        private MetroFramework.Controls.MetroLabel metroLabelDBInstance;
        private MetroFramework.Controls.MetroTextBox metroTextBoxDBInstance;
        private MetroFramework.Controls.MetroTextBox metroTextBoxDBLoginName;
        private MetroFramework.Controls.MetroLabel metroLabelDBLoginName;
        private MetroFramework.Controls.MetroTextBox metroTextBoxDBLoginPassword;
        private MetroFramework.Controls.MetroLabel metroLabelDBLoginPassword;
    }
}
