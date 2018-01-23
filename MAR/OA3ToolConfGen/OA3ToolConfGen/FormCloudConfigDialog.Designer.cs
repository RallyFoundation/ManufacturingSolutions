namespace OA3ToolConfGen
{
    partial class FormCloudConfigDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCloudConfigDialog));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelDISConfiguraitonCloudUrl = new System.Windows.Forms.Label();
            this.textBoxDISConfiguraitonCloudUrl = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelBusiness = new System.Windows.Forms.Label();
            this.comboBoxBusiness = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.checkBoxShowPasswordContent = new System.Windows.Forms.CheckBox();
            this.textBoxDatabaseName = new System.Windows.Forms.TextBox();
            this.labelDatabaseName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(71, 196);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(157, 196);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelDISConfiguraitonCloudUrl
            // 
            this.labelDISConfiguraitonCloudUrl.Location = new System.Drawing.Point(12, 20);
            this.labelDISConfiguraitonCloudUrl.Name = "labelDISConfiguraitonCloudUrl";
            this.labelDISConfiguraitonCloudUrl.Size = new System.Drawing.Size(139, 23);
            this.labelDISConfiguraitonCloudUrl.TabIndex = 2;
            this.labelDISConfiguraitonCloudUrl.Text = "Database Server:";
            this.labelDISConfiguraitonCloudUrl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDISConfiguraitonCloudUrl
            // 
            this.textBoxDISConfiguraitonCloudUrl.Location = new System.Drawing.Point(157, 17);
            this.textBoxDISConfiguraitonCloudUrl.MaxLength = 150;
            this.textBoxDISConfiguraitonCloudUrl.Name = "textBoxDISConfiguraitonCloudUrl";
            this.textBoxDISConfiguraitonCloudUrl.Size = new System.Drawing.Size(197, 20);
            this.textBoxDISConfiguraitonCloudUrl.TabIndex = 3;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(157, 75);
            this.textBoxUserName.MaxLength = 150;
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(197, 20);
            this.textBoxUserName.TabIndex = 5;
            // 
            // labelUserName
            // 
            this.labelUserName.Location = new System.Drawing.Point(12, 78);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(139, 23);
            this.labelUserName.TabIndex = 4;
            this.labelUserName.Text = "User Name:";
            this.labelUserName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(12, 108);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(139, 23);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "Password:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(157, 105);
            this.textBoxPassword.MaxLength = 150;
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(197, 20);
            this.textBoxPassword.TabIndex = 7;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // labelBusiness
            // 
            this.labelBusiness.Location = new System.Drawing.Point(12, 160);
            this.labelBusiness.Name = "labelBusiness";
            this.labelBusiness.Size = new System.Drawing.Size(139, 23);
            this.labelBusiness.TabIndex = 8;
            this.labelBusiness.Text = "Business:";
            this.labelBusiness.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxBusiness
            // 
            this.comboBoxBusiness.DisplayMember = "Name";
            this.comboBoxBusiness.FormattingEnabled = true;
            this.comboBoxBusiness.Location = new System.Drawing.Point(157, 158);
            this.comboBoxBusiness.Name = "comboBoxBusiness";
            this.comboBoxBusiness.Size = new System.Drawing.Size(197, 21);
            this.comboBoxBusiness.TabIndex = 9;
            this.comboBoxBusiness.ValueMember = "ID";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(243, 196);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 10;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // checkBoxShowPasswordContent
            // 
            this.checkBoxShowPasswordContent.AutoSize = true;
            this.checkBoxShowPasswordContent.Location = new System.Drawing.Point(157, 132);
            this.checkBoxShowPasswordContent.Name = "checkBoxShowPasswordContent";
            this.checkBoxShowPasswordContent.Size = new System.Drawing.Size(142, 17);
            this.checkBoxShowPasswordContent.TabIndex = 11;
            this.checkBoxShowPasswordContent.Text = "Show Password Content";
            this.checkBoxShowPasswordContent.UseVisualStyleBackColor = true;
            this.checkBoxShowPasswordContent.CheckedChanged += new System.EventHandler(this.checkBoxShowPasswordContent_CheckedChanged);
            // 
            // textBoxDatabaseName
            // 
            this.textBoxDatabaseName.Location = new System.Drawing.Point(157, 46);
            this.textBoxDatabaseName.MaxLength = 150;
            this.textBoxDatabaseName.Name = "textBoxDatabaseName";
            this.textBoxDatabaseName.Size = new System.Drawing.Size(197, 20);
            this.textBoxDatabaseName.TabIndex = 13;
            // 
            // labelDatabaseName
            // 
            this.labelDatabaseName.Location = new System.Drawing.Point(12, 49);
            this.labelDatabaseName.Name = "labelDatabaseName";
            this.labelDatabaseName.Size = new System.Drawing.Size(139, 23);
            this.labelDatabaseName.TabIndex = 12;
            this.labelDatabaseName.Text = "Databse Name:";
            this.labelDatabaseName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FormCloudConfigDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 232);
            this.Controls.Add(this.textBoxDatabaseName);
            this.Controls.Add(this.labelDatabaseName);
            this.Controls.Add(this.checkBoxShowPasswordContent);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.comboBoxBusiness);
            this.Controls.Add(this.labelBusiness);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.textBoxDISConfiguraitonCloudUrl);
            this.Controls.Add(this.labelDISConfiguraitonCloudUrl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCloudConfigDialog";
            this.Text = "Configuration Server Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelDISConfiguraitonCloudUrl;
        private System.Windows.Forms.TextBox textBoxDISConfiguraitonCloudUrl;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelBusiness;
        private System.Windows.Forms.ComboBox comboBoxBusiness;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.CheckBox checkBoxShowPasswordContent;
        private System.Windows.Forms.TextBox textBoxDatabaseName;
        private System.Windows.Forms.Label labelDatabaseName;
    }
}