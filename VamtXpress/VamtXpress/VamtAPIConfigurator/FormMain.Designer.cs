namespace VamtAPIConfigurator
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelUrl = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.statusStripLabel1 = new System.Windows.Forms.StatusStrip();
            this.textBoxVamtDomainLDAPUrl = new System.Windows.Forms.TextBox();
            this.labelVamtDomainLDAPUrl = new System.Windows.Forms.Label();
            this.textBoxDomainUserName = new System.Windows.Forms.TextBox();
            this.labelDomainUserName = new System.Windows.Forms.Label();
            this.textBoxVamtDomainUserPassword = new System.Windows.Forms.TextBox();
            this.labelVamtDomainUserPassword = new System.Windows.Forms.Label();
            this.buttonTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(48, 35);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(70, 13);
            this.labelUrl.TabIndex = 0;
            this.labelUrl.Text = "Vamt API Url:";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(124, 32);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(306, 20);
            this.textBoxUrl.TabIndex = 1;
            this.textBoxUrl.Text = "http://vamt-server:9089/";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(357, 178);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // statusStripLabel1
            // 
            this.statusStripLabel1.Location = new System.Drawing.Point(0, 213);
            this.statusStripLabel1.Name = "statusStripLabel1";
            this.statusStripLabel1.Size = new System.Drawing.Size(444, 22);
            this.statusStripLabel1.TabIndex = 5;
            // 
            // textBoxVamtDomainLDAPUrl
            // 
            this.textBoxVamtDomainLDAPUrl.Location = new System.Drawing.Point(124, 69);
            this.textBoxVamtDomainLDAPUrl.Name = "textBoxVamtDomainLDAPUrl";
            this.textBoxVamtDomainLDAPUrl.Size = new System.Drawing.Size(306, 20);
            this.textBoxVamtDomainLDAPUrl.TabIndex = 7;
            this.textBoxVamtDomainLDAPUrl.Text = "ldap://vamt-server.mydomain.com";
            // 
            // labelVamtDomainLDAPUrl
            // 
            this.labelVamtDomainLDAPUrl.AutoSize = true;
            this.labelVamtDomainLDAPUrl.Location = new System.Drawing.Point(62, 72);
            this.labelVamtDomainLDAPUrl.Name = "labelVamtDomainLDAPUrl";
            this.labelVamtDomainLDAPUrl.Size = new System.Drawing.Size(54, 13);
            this.labelVamtDomainLDAPUrl.TabIndex = 6;
            this.labelVamtDomainLDAPUrl.Text = "LDAP Url:";
            // 
            // textBoxDomainUserName
            // 
            this.textBoxDomainUserName.Location = new System.Drawing.Point(124, 107);
            this.textBoxDomainUserName.Name = "textBoxDomainUserName";
            this.textBoxDomainUserName.Size = new System.Drawing.Size(306, 20);
            this.textBoxDomainUserName.TabIndex = 9;
            // 
            // labelDomainUserName
            // 
            this.labelDomainUserName.AutoSize = true;
            this.labelDomainUserName.Location = new System.Drawing.Point(14, 110);
            this.labelDomainUserName.Name = "labelDomainUserName";
            this.labelDomainUserName.Size = new System.Drawing.Size(102, 13);
            this.labelDomainUserName.TabIndex = 8;
            this.labelDomainUserName.Text = "Domain User Name:";
            // 
            // textBoxVamtDomainUserPassword
            // 
            this.textBoxVamtDomainUserPassword.Location = new System.Drawing.Point(124, 143);
            this.textBoxVamtDomainUserPassword.Name = "textBoxVamtDomainUserPassword";
            this.textBoxVamtDomainUserPassword.Size = new System.Drawing.Size(306, 20);
            this.textBoxVamtDomainUserPassword.TabIndex = 11;
            this.textBoxVamtDomainUserPassword.UseSystemPasswordChar = true;
            // 
            // labelVamtDomainUserPassword
            // 
            this.labelVamtDomainUserPassword.AutoSize = true;
            this.labelVamtDomainUserPassword.Location = new System.Drawing.Point(60, 146);
            this.labelVamtDomainUserPassword.Name = "labelVamtDomainUserPassword";
            this.labelVamtDomainUserPassword.Size = new System.Drawing.Size(56, 13);
            this.labelVamtDomainUserPassword.TabIndex = 10;
            this.labelVamtDomainUserPassword.Text = "Password:";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(276, 178);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 12;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 235);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.textBoxVamtDomainUserPassword);
            this.Controls.Add(this.labelVamtDomainUserPassword);
            this.Controls.Add(this.textBoxDomainUserName);
            this.Controls.Add(this.labelDomainUserName);
            this.Controls.Add(this.textBoxVamtDomainLDAPUrl);
            this.Controls.Add(this.labelVamtDomainLDAPUrl);
            this.Controls.Add(this.statusStripLabel1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.labelUrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VAMT API Configurator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.StatusStrip statusStripLabel1;
        private System.Windows.Forms.TextBox textBoxVamtDomainLDAPUrl;
        private System.Windows.Forms.Label labelVamtDomainLDAPUrl;
        private System.Windows.Forms.TextBox textBoxDomainUserName;
        private System.Windows.Forms.Label labelDomainUserName;
        private System.Windows.Forms.TextBox textBoxVamtDomainUserPassword;
        private System.Windows.Forms.Label labelVamtDomainUserPassword;
        private System.Windows.Forms.Button buttonTest;
    }
}

