namespace WindowsManufacturingStudio
{
    partial class FormCreateBootWinPE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateBootWinPE));
            this.metroLabelArchitecture = new MetroFramework.Controls.MetroLabel();
            this.metroRadioButtonX86 = new MetroFramework.Controls.MetroRadioButton();
            this.metroRadioButtonAmd64 = new MetroFramework.Controls.MetroRadioButton();
            this.metroLabelBootImageType = new MetroFramework.Controls.MetroLabel();
            this.metroLabelOutputDir = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxOutputLocation = new MetroFramework.Controls.MetroTextBox();
            this.metroButtonBrowse = new MetroFramework.Controls.MetroButton();
            this.metroComboBoxImageType = new MetroFramework.Controls.MetroComboBox();
            this.metroTileBack = new MetroFramework.Controls.MetroTile();
            this.metroTileCreate = new MetroFramework.Controls.MetroTile();
            this.folderBrowserDialogOutputLocation = new System.Windows.Forms.FolderBrowserDialog();
            this.metroLabelImageServerAddress = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxImageServerAddress = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBoxImageServerUsername = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelImageServerUsername = new MetroFramework.Controls.MetroLabel();
            this.metroLabelImageServerPassword = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxWDSAPIURL = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelWDSAPIURL = new MetroFramework.Controls.MetroLabel();
            this.metroToggleShowPassword = new MetroFramework.Controls.MetroToggle();
            this.metroTextBoxImageServerPassword = new MetroFramework.Controls.MetroTextBox();
            this.metroButtonTest = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroLabelArchitecture
            // 
            this.metroLabelArchitecture.AutoSize = true;
            this.metroLabelArchitecture.Location = new System.Drawing.Point(88, 181);
            this.metroLabelArchitecture.Name = "metroLabelArchitecture";
            this.metroLabelArchitecture.Size = new System.Drawing.Size(82, 19);
            this.metroLabelArchitecture.TabIndex = 0;
            this.metroLabelArchitecture.Text = "Architecture:";
            // 
            // metroRadioButtonX86
            // 
            this.metroRadioButtonX86.AutoSize = true;
            this.metroRadioButtonX86.Checked = true;
            this.metroRadioButtonX86.Location = new System.Drawing.Point(176, 181);
            this.metroRadioButtonX86.Name = "metroRadioButtonX86";
            this.metroRadioButtonX86.Size = new System.Drawing.Size(40, 15);
            this.metroRadioButtonX86.TabIndex = 1;
            this.metroRadioButtonX86.TabStop = true;
            this.metroRadioButtonX86.Tag = "";
            this.metroRadioButtonX86.Text = "x86";
            this.metroRadioButtonX86.UseVisualStyleBackColor = true;
            // 
            // metroRadioButtonAmd64
            // 
            this.metroRadioButtonAmd64.AutoSize = true;
            this.metroRadioButtonAmd64.Location = new System.Drawing.Point(223, 181);
            this.metroRadioButtonAmd64.Name = "metroRadioButtonAmd64";
            this.metroRadioButtonAmd64.Size = new System.Drawing.Size(59, 15);
            this.metroRadioButtonAmd64.TabIndex = 2;
            this.metroRadioButtonAmd64.Tag = "";
            this.metroRadioButtonAmd64.Text = "amd64";
            this.metroRadioButtonAmd64.UseVisualStyleBackColor = true;
            // 
            // metroLabelBootImageType
            // 
            this.metroLabelBootImageType.AutoSize = true;
            this.metroLabelBootImageType.Location = new System.Drawing.Point(88, 229);
            this.metroLabelBootImageType.Name = "metroLabelBootImageType";
            this.metroLabelBootImageType.Size = new System.Drawing.Size(80, 19);
            this.metroLabelBootImageType.TabIndex = 3;
            this.metroLabelBootImageType.Text = "Image Type:";
            // 
            // metroLabelOutputDir
            // 
            this.metroLabelOutputDir.AutoSize = true;
            this.metroLabelOutputDir.Location = new System.Drawing.Point(64, 283);
            this.metroLabelOutputDir.Name = "metroLabelOutputDir";
            this.metroLabelOutputDir.Size = new System.Drawing.Size(106, 19);
            this.metroLabelOutputDir.TabIndex = 6;
            this.metroLabelOutputDir.Text = "Output Location:";
            // 
            // metroTextBoxOutputLocation
            // 
            this.metroTextBoxOutputLocation.Location = new System.Drawing.Point(176, 275);
            this.metroTextBoxOutputLocation.Name = "metroTextBoxOutputLocation";
            this.metroTextBoxOutputLocation.ReadOnly = true;
            this.metroTextBoxOutputLocation.Size = new System.Drawing.Size(500, 32);
            this.metroTextBoxOutputLocation.TabIndex = 7;
            // 
            // metroButtonBrowse
            // 
            this.metroButtonBrowse.Location = new System.Drawing.Point(682, 275);
            this.metroButtonBrowse.Name = "metroButtonBrowse";
            this.metroButtonBrowse.Size = new System.Drawing.Size(100, 32);
            this.metroButtonBrowse.TabIndex = 8;
            this.metroButtonBrowse.Text = "Browse...";
            this.metroButtonBrowse.Click += new System.EventHandler(this.metroButtonBrowse_Click);
            // 
            // metroComboBoxImageType
            // 
            this.metroComboBoxImageType.FormattingEnabled = true;
            this.metroComboBoxImageType.ItemHeight = 23;
            this.metroComboBoxImageType.Items.AddRange(new object[] {
            "Multicast",
            "Full Flash Update (FFU)"});
            this.metroComboBoxImageType.Location = new System.Drawing.Point(176, 222);
            this.metroComboBoxImageType.Name = "metroComboBoxImageType";
            this.metroComboBoxImageType.Size = new System.Drawing.Size(606, 29);
            this.metroComboBoxImageType.TabIndex = 9;
            this.metroComboBoxImageType.SelectedIndexChanged += new System.EventHandler(this.metroComboBoxImageType_SelectedIndexChanged);
            // 
            // metroTileBack
            // 
            this.metroTileBack.Location = new System.Drawing.Point(35, 97);
            this.metroTileBack.Name = "metroTileBack";
            this.metroTileBack.Size = new System.Drawing.Size(75, 32);
            this.metroTileBack.TabIndex = 10;
            this.metroTileBack.Text = "Back";
            this.metroTileBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileBack.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileBack.Click += new System.EventHandler(this.metroTileBack_Click);
            // 
            // metroTileCreate
            // 
            this.metroTileCreate.Location = new System.Drawing.Point(131, 97);
            this.metroTileCreate.Name = "metroTileCreate";
            this.metroTileCreate.Size = new System.Drawing.Size(75, 32);
            this.metroTileCreate.TabIndex = 11;
            this.metroTileCreate.Text = "Create";
            this.metroTileCreate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileCreate.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileCreate.Click += new System.EventHandler(this.metroTileCreate_Click);
            // 
            // metroLabelImageServerAddress
            // 
            this.metroLabelImageServerAddress.AutoSize = true;
            this.metroLabelImageServerAddress.Location = new System.Drawing.Point(26, 330);
            this.metroLabelImageServerAddress.Name = "metroLabelImageServerAddress";
            this.metroLabelImageServerAddress.Size = new System.Drawing.Size(142, 19);
            this.metroLabelImageServerAddress.TabIndex = 12;
            this.metroLabelImageServerAddress.Text = "Image Server Address:";
            // 
            // metroTextBoxImageServerAddress
            // 
            this.metroTextBoxImageServerAddress.Location = new System.Drawing.Point(176, 324);
            this.metroTextBoxImageServerAddress.MaxLength = 100;
            this.metroTextBoxImageServerAddress.Name = "metroTextBoxImageServerAddress";
            this.metroTextBoxImageServerAddress.Size = new System.Drawing.Size(500, 32);
            this.metroTextBoxImageServerAddress.TabIndex = 13;
            // 
            // metroTextBoxImageServerUsername
            // 
            this.metroTextBoxImageServerUsername.Location = new System.Drawing.Point(177, 373);
            this.metroTextBoxImageServerUsername.MaxLength = 20;
            this.metroTextBoxImageServerUsername.Name = "metroTextBoxImageServerUsername";
            this.metroTextBoxImageServerUsername.Size = new System.Drawing.Size(500, 32);
            this.metroTextBoxImageServerUsername.TabIndex = 15;
            // 
            // metroLabelImageServerUsername
            // 
            this.metroLabelImageServerUsername.AutoSize = true;
            this.metroLabelImageServerUsername.Location = new System.Drawing.Point(13, 380);
            this.metroLabelImageServerUsername.Name = "metroLabelImageServerUsername";
            this.metroLabelImageServerUsername.Size = new System.Drawing.Size(154, 19);
            this.metroLabelImageServerUsername.TabIndex = 14;
            this.metroLabelImageServerUsername.Text = "Image Server Username:";
            // 
            // metroLabelImageServerPassword
            // 
            this.metroLabelImageServerPassword.AutoSize = true;
            this.metroLabelImageServerPassword.Location = new System.Drawing.Point(18, 429);
            this.metroLabelImageServerPassword.Name = "metroLabelImageServerPassword";
            this.metroLabelImageServerPassword.Size = new System.Drawing.Size(149, 19);
            this.metroLabelImageServerPassword.TabIndex = 16;
            this.metroLabelImageServerPassword.Text = "Image Server Password:";
            // 
            // metroTextBoxWDSAPIURL
            // 
            this.metroTextBoxWDSAPIURL.Location = new System.Drawing.Point(177, 476);
            this.metroTextBoxWDSAPIURL.Name = "metroTextBoxWDSAPIURL";
            this.metroTextBoxWDSAPIURL.Size = new System.Drawing.Size(500, 32);
            this.metroTextBoxWDSAPIURL.TabIndex = 19;
            // 
            // metroLabelWDSAPIURL
            // 
            this.metroLabelWDSAPIURL.AutoSize = true;
            this.metroLabelWDSAPIURL.Location = new System.Drawing.Point(75, 481);
            this.metroLabelWDSAPIURL.Name = "metroLabelWDSAPIURL";
            this.metroLabelWDSAPIURL.Size = new System.Drawing.Size(92, 19);
            this.metroLabelWDSAPIURL.TabIndex = 18;
            this.metroLabelWDSAPIURL.Text = "WDS API URL:";
            // 
            // metroToggleShowPassword
            // 
            this.metroToggleShowPassword.AutoSize = true;
            this.metroToggleShowPassword.Location = new System.Drawing.Point(682, 431);
            this.metroToggleShowPassword.Name = "metroToggleShowPassword";
            this.metroToggleShowPassword.Size = new System.Drawing.Size(80, 17);
            this.metroToggleShowPassword.TabIndex = 20;
            this.metroToggleShowPassword.Text = "Off";
            this.metroToggleShowPassword.UseVisualStyleBackColor = true;
            this.metroToggleShowPassword.CheckedChanged += new System.EventHandler(this.metroToggleShowPassword_CheckedChanged);
            // 
            // metroTextBoxImageServerPassword
            // 
            this.metroTextBoxImageServerPassword.Location = new System.Drawing.Point(177, 423);
            this.metroTextBoxImageServerPassword.MaxLength = 18;
            this.metroTextBoxImageServerPassword.Name = "metroTextBoxImageServerPassword";
            this.metroTextBoxImageServerPassword.PasswordChar = '●';
            this.metroTextBoxImageServerPassword.Size = new System.Drawing.Size(500, 32);
            this.metroTextBoxImageServerPassword.TabIndex = 17;
            this.metroTextBoxImageServerPassword.UseSystemPasswordChar = true;
            // 
            // metroButtonTest
            // 
            this.metroButtonTest.Location = new System.Drawing.Point(683, 476);
            this.metroButtonTest.Name = "metroButtonTest";
            this.metroButtonTest.Size = new System.Drawing.Size(100, 32);
            this.metroButtonTest.TabIndex = 21;
            this.metroButtonTest.Text = "Test";
            this.metroButtonTest.Click += new System.EventHandler(this.metroButtonTest_Click);
            // 
            // FormCreateBootWinPE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.metroButtonTest);
            this.Controls.Add(this.metroToggleShowPassword);
            this.Controls.Add(this.metroTextBoxWDSAPIURL);
            this.Controls.Add(this.metroLabelWDSAPIURL);
            this.Controls.Add(this.metroTextBoxImageServerPassword);
            this.Controls.Add(this.metroLabelImageServerPassword);
            this.Controls.Add(this.metroTextBoxImageServerUsername);
            this.Controls.Add(this.metroLabelImageServerUsername);
            this.Controls.Add(this.metroTextBoxImageServerAddress);
            this.Controls.Add(this.metroLabelImageServerAddress);
            this.Controls.Add(this.metroTileCreate);
            this.Controls.Add(this.metroTileBack);
            this.Controls.Add(this.metroComboBoxImageType);
            this.Controls.Add(this.metroButtonBrowse);
            this.Controls.Add(this.metroTextBoxOutputLocation);
            this.Controls.Add(this.metroLabelOutputDir);
            this.Controls.Add(this.metroLabelBootImageType);
            this.Controls.Add(this.metroRadioButtonAmd64);
            this.Controls.Add(this.metroRadioButtonX86);
            this.Controls.Add(this.metroLabelArchitecture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCreateBootWinPE";
            this.Text = "Create Boot Windows PE";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCreateBootWinPE_FormClosing);
            this.Load += new System.EventHandler(this.FormCreateBootWinPE_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabelArchitecture;
        private MetroFramework.Controls.MetroRadioButton metroRadioButtonX86;
        private MetroFramework.Controls.MetroRadioButton metroRadioButtonAmd64;
        private MetroFramework.Controls.MetroLabel metroLabelBootImageType;
        private MetroFramework.Controls.MetroLabel metroLabelOutputDir;
        private MetroFramework.Controls.MetroTextBox metroTextBoxOutputLocation;
        private MetroFramework.Controls.MetroButton metroButtonBrowse;
        private MetroFramework.Controls.MetroComboBox metroComboBoxImageType;
        private MetroFramework.Controls.MetroTile metroTileBack;
        private MetroFramework.Controls.MetroTile metroTileCreate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogOutputLocation;
        private MetroFramework.Controls.MetroLabel metroLabelImageServerAddress;
        private MetroFramework.Controls.MetroTextBox metroTextBoxImageServerAddress;
        private MetroFramework.Controls.MetroTextBox metroTextBoxImageServerUsername;
        private MetroFramework.Controls.MetroLabel metroLabelImageServerUsername;
        private MetroFramework.Controls.MetroLabel metroLabelImageServerPassword;
        private MetroFramework.Controls.MetroTextBox metroTextBoxWDSAPIURL;
        private MetroFramework.Controls.MetroLabel metroLabelWDSAPIURL;
        private MetroFramework.Controls.MetroToggle metroToggleShowPassword;
        private MetroFramework.Controls.MetroTextBox metroTextBoxImageServerPassword;
        private MetroFramework.Controls.MetroButton metroButtonTest;
    }
}