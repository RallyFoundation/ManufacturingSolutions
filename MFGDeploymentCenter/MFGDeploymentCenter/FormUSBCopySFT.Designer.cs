namespace MfgSolutionsDeploymentCenter
{
    partial class FormUSBCopySFT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUSBCopySFT));
            this.metroLinkBack = new MetroFramework.Controls.MetroLink();
            this.metroComboBoxRemovableDisks = new MetroFramework.Controls.MetroComboBox();
            this.metroButtonBeginCopy = new MetroFramework.Controls.MetroButton();
            this.metroLabelChoose = new MetroFramework.Controls.MetroLabel();
            this.metroProgressBarCopyProgress = new MetroFramework.Controls.MetroProgressBar();
            this.metroLabelCurrentItem = new MetroFramework.Controls.MetroLabel();
            this.metroLabelMESCloudURL = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxMESCloudURL = new MetroFramework.Controls.MetroTextBox();
            this.metroTextBoxMESCloudUserName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelMESCloudUserName = new MetroFramework.Controls.MetroLabel();
            this.metroLabelMESCloudPassword = new MetroFramework.Controls.MetroLabel();
            this.metroLabelSystemArchitecture = new MetroFramework.Controls.MetroLabel();
            this.metroRadioButtonSysArcx86 = new MetroFramework.Controls.MetroRadioButton();
            this.metroRadioButtonSystArcAmd64 = new MetroFramework.Controls.MetroRadioButton();
            this.metroLabelShowPassword = new MetroFramework.Controls.MetroLabel();
            this.metroToggleShowPassword = new MetroFramework.Controls.MetroToggle();
            this.metroTextBoxMESCloudPassword = new System.Windows.Forms.TextBox();
            this.panelMES = new System.Windows.Forms.Panel();
            this.panelBasic = new System.Windows.Forms.Panel();
            this.flowLayoutPanelMain = new System.Windows.Forms.FlowLayoutPanel();
            this.metroLabelMESSwitch = new MetroFramework.Controls.MetroLabel();
            this.metroToggleMESSwitch = new MetroFramework.Controls.MetroToggle();
            this.panelMES.SuspendLayout();
            this.panelBasic.SuspendLayout();
            this.flowLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroLinkBack
            // 
            this.metroLinkBack.BackgroundImage = global::MfgSolutionsDeploymentCenter.Properties.Resources.MB_0006_back;
            this.metroLinkBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.metroLinkBack.Location = new System.Drawing.Point(20, 60);
            this.metroLinkBack.Name = "metroLinkBack";
            this.metroLinkBack.Size = new System.Drawing.Size(45, 45);
            this.metroLinkBack.TabIndex = 3;
            this.metroLinkBack.UseSelectable = true;
            this.metroLinkBack.Click += new System.EventHandler(this.metroLinkBack_Click);
            // 
            // metroComboBoxRemovableDisks
            // 
            this.metroComboBoxRemovableDisks.FormattingEnabled = true;
            this.metroComboBoxRemovableDisks.ItemHeight = 23;
            this.metroComboBoxRemovableDisks.Location = new System.Drawing.Point(0, 73);
            this.metroComboBoxRemovableDisks.Name = "metroComboBoxRemovableDisks";
            this.metroComboBoxRemovableDisks.Size = new System.Drawing.Size(417, 29);
            this.metroComboBoxRemovableDisks.TabIndex = 4;
            this.metroComboBoxRemovableDisks.UseSelectable = true;
            // 
            // metroButtonBeginCopy
            // 
            this.metroButtonBeginCopy.Location = new System.Drawing.Point(424, 73);
            this.metroButtonBeginCopy.Name = "metroButtonBeginCopy";
            this.metroButtonBeginCopy.Size = new System.Drawing.Size(86, 29);
            this.metroButtonBeginCopy.TabIndex = 5;
            this.metroButtonBeginCopy.Text = "Deploy";
            this.metroButtonBeginCopy.UseSelectable = true;
            this.metroButtonBeginCopy.Click += new System.EventHandler(this.metroButtonBeginCopy_Click);
            // 
            // metroLabelChoose
            // 
            this.metroLabelChoose.AutoSize = true;
            this.metroLabelChoose.Location = new System.Drawing.Point(0, 46);
            this.metroLabelChoose.Name = "metroLabelChoose";
            this.metroLabelChoose.Size = new System.Drawing.Size(160, 19);
            this.metroLabelChoose.TabIndex = 6;
            this.metroLabelChoose.Text = "Choose a removable disk:";
            // 
            // metroProgressBarCopyProgress
            // 
            this.metroProgressBarCopyProgress.Location = new System.Drawing.Point(0, 111);
            this.metroProgressBarCopyProgress.Name = "metroProgressBarCopyProgress";
            this.metroProgressBarCopyProgress.Size = new System.Drawing.Size(509, 26);
            this.metroProgressBarCopyProgress.Step = 1;
            this.metroProgressBarCopyProgress.TabIndex = 7;
            this.metroProgressBarCopyProgress.Visible = false;
            // 
            // metroLabelCurrentItem
            // 
            this.metroLabelCurrentItem.AutoSize = true;
            this.metroLabelCurrentItem.FontSize = MetroFramework.MetroLabelSize.Small;
            this.metroLabelCurrentItem.Location = new System.Drawing.Point(0, 153);
            this.metroLabelCurrentItem.Name = "metroLabelCurrentItem";
            this.metroLabelCurrentItem.Size = new System.Drawing.Size(0, 0);
            this.metroLabelCurrentItem.TabIndex = 8;
            this.metroLabelCurrentItem.Visible = false;
            this.metroLabelCurrentItem.WrapToLine = true;
            // 
            // metroLabelMESCloudURL
            // 
            this.metroLabelMESCloudURL.AutoSize = true;
            this.metroLabelMESCloudURL.Location = new System.Drawing.Point(0, 0);
            this.metroLabelMESCloudURL.Name = "metroLabelMESCloudURL";
            this.metroLabelMESCloudURL.Size = new System.Drawing.Size(81, 19);
            this.metroLabelMESCloudURL.TabIndex = 9;
            this.metroLabelMESCloudURL.Text = "Service URL:";
            // 
            // metroTextBoxMESCloudURL
            // 
            this.metroTextBoxMESCloudURL.Lines = new string[] {
        "http://MyMESServer:8919"};
            this.metroTextBoxMESCloudURL.Location = new System.Drawing.Point(1, 26);
            this.metroTextBoxMESCloudURL.MaxLength = 32767;
            this.metroTextBoxMESCloudURL.Name = "metroTextBoxMESCloudURL";
            this.metroTextBoxMESCloudURL.PasswordChar = '\0';
            this.metroTextBoxMESCloudURL.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxMESCloudURL.SelectedText = "";
            this.metroTextBoxMESCloudURL.Size = new System.Drawing.Size(509, 23);
            this.metroTextBoxMESCloudURL.TabIndex = 10;
            this.metroTextBoxMESCloudURL.Text = "http://MyMESServer:8919";
            this.metroTextBoxMESCloudURL.UseSelectable = true;
            // 
            // metroTextBoxMESCloudUserName
            // 
            this.metroTextBoxMESCloudUserName.Lines = new string[] {
        "MES"};
            this.metroTextBoxMESCloudUserName.Location = new System.Drawing.Point(1, 85);
            this.metroTextBoxMESCloudUserName.MaxLength = 32767;
            this.metroTextBoxMESCloudUserName.Name = "metroTextBoxMESCloudUserName";
            this.metroTextBoxMESCloudUserName.PasswordChar = '\0';
            this.metroTextBoxMESCloudUserName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxMESCloudUserName.SelectedText = "";
            this.metroTextBoxMESCloudUserName.Size = new System.Drawing.Size(509, 23);
            this.metroTextBoxMESCloudUserName.TabIndex = 12;
            this.metroTextBoxMESCloudUserName.Text = "MES";
            this.metroTextBoxMESCloudUserName.UseSelectable = true;
            // 
            // metroLabelMESCloudUserName
            // 
            this.metroLabelMESCloudUserName.AutoSize = true;
            this.metroLabelMESCloudUserName.Location = new System.Drawing.Point(1, 60);
            this.metroLabelMESCloudUserName.Name = "metroLabelMESCloudUserName";
            this.metroLabelMESCloudUserName.Size = new System.Drawing.Size(78, 19);
            this.metroLabelMESCloudUserName.TabIndex = 11;
            this.metroLabelMESCloudUserName.Text = "User Name:";
            // 
            // metroLabelMESCloudPassword
            // 
            this.metroLabelMESCloudPassword.AutoSize = true;
            this.metroLabelMESCloudPassword.Location = new System.Drawing.Point(1, 119);
            this.metroLabelMESCloudPassword.Name = "metroLabelMESCloudPassword";
            this.metroLabelMESCloudPassword.Size = new System.Drawing.Size(66, 19);
            this.metroLabelMESCloudPassword.TabIndex = 13;
            this.metroLabelMESCloudPassword.Text = "Password:";
            // 
            // metroLabelSystemArchitecture
            // 
            this.metroLabelSystemArchitecture.AutoSize = true;
            this.metroLabelSystemArchitecture.Location = new System.Drawing.Point(0, 9);
            this.metroLabelSystemArchitecture.Name = "metroLabelSystemArchitecture";
            this.metroLabelSystemArchitecture.Size = new System.Drawing.Size(127, 19);
            this.metroLabelSystemArchitecture.TabIndex = 15;
            this.metroLabelSystemArchitecture.Text = "System Architecture:";
            // 
            // metroRadioButtonSysArcx86
            // 
            this.metroRadioButtonSysArcx86.AutoSize = true;
            this.metroRadioButtonSysArcx86.Location = new System.Drawing.Point(136, 13);
            this.metroRadioButtonSysArcx86.Name = "metroRadioButtonSysArcx86";
            this.metroRadioButtonSysArcx86.Size = new System.Drawing.Size(40, 15);
            this.metroRadioButtonSysArcx86.TabIndex = 16;
            this.metroRadioButtonSysArcx86.Text = "x86";
            this.metroRadioButtonSysArcx86.UseSelectable = true;
            // 
            // metroRadioButtonSystArcAmd64
            // 
            this.metroRadioButtonSystArcAmd64.AutoSize = true;
            this.metroRadioButtonSystArcAmd64.Checked = true;
            this.metroRadioButtonSystArcAmd64.Location = new System.Drawing.Point(185, 13);
            this.metroRadioButtonSystArcAmd64.Name = "metroRadioButtonSystArcAmd64";
            this.metroRadioButtonSystArcAmd64.Size = new System.Drawing.Size(59, 15);
            this.metroRadioButtonSystArcAmd64.TabIndex = 17;
            this.metroRadioButtonSystArcAmd64.TabStop = true;
            this.metroRadioButtonSystArcAmd64.Text = "amd64";
            this.metroRadioButtonSystArcAmd64.UseSelectable = true;
            // 
            // metroLabelShowPassword
            // 
            this.metroLabelShowPassword.AutoSize = true;
            this.metroLabelShowPassword.Location = new System.Drawing.Point(1, 172);
            this.metroLabelShowPassword.Name = "metroLabelShowPassword";
            this.metroLabelShowPassword.Size = new System.Drawing.Size(111, 19);
            this.metroLabelShowPassword.TabIndex = 18;
            this.metroLabelShowPassword.Text = "Display Password:";
            // 
            // metroToggleShowPassword
            // 
            this.metroToggleShowPassword.AutoSize = true;
            this.metroToggleShowPassword.Location = new System.Drawing.Point(118, 174);
            this.metroToggleShowPassword.Name = "metroToggleShowPassword";
            this.metroToggleShowPassword.Size = new System.Drawing.Size(80, 17);
            this.metroToggleShowPassword.TabIndex = 19;
            this.metroToggleShowPassword.Text = "Off";
            this.metroToggleShowPassword.UseSelectable = true;
            this.metroToggleShowPassword.CheckedChanged += new System.EventHandler(this.metroToggleShowPassword_CheckedChanged);
            // 
            // metroTextBoxMESCloudPassword
            // 
            this.metroTextBoxMESCloudPassword.BackColor = System.Drawing.Color.White;
            this.metroTextBoxMESCloudPassword.Location = new System.Drawing.Point(1, 148);
            this.metroTextBoxMESCloudPassword.Name = "metroTextBoxMESCloudPassword";
            this.metroTextBoxMESCloudPassword.Size = new System.Drawing.Size(509, 20);
            this.metroTextBoxMESCloudPassword.TabIndex = 20;
            this.metroTextBoxMESCloudPassword.UseSystemPasswordChar = true;
            // 
            // panelMES
            // 
            this.panelMES.AutoSize = true;
            this.panelMES.Controls.Add(this.metroLabelMESCloudURL);
            this.panelMES.Controls.Add(this.metroTextBoxMESCloudPassword);
            this.panelMES.Controls.Add(this.metroTextBoxMESCloudURL);
            this.panelMES.Controls.Add(this.metroToggleShowPassword);
            this.panelMES.Controls.Add(this.metroLabelMESCloudUserName);
            this.panelMES.Controls.Add(this.metroLabelShowPassword);
            this.panelMES.Controls.Add(this.metroTextBoxMESCloudUserName);
            this.panelMES.Controls.Add(this.metroLabelMESCloudPassword);
            this.flowLayoutPanelMain.SetFlowBreak(this.panelMES, true);
            this.panelMES.Location = new System.Drawing.Point(3, 3);
            this.panelMES.Name = "panelMES";
            this.panelMES.Size = new System.Drawing.Size(513, 194);
            this.panelMES.TabIndex = 21;
            this.panelMES.Visible = false;
            // 
            // panelBasic
            // 
            this.panelBasic.AutoSize = true;
            this.panelBasic.Controls.Add(this.metroLabelSystemArchitecture);
            this.panelBasic.Controls.Add(this.metroComboBoxRemovableDisks);
            this.panelBasic.Controls.Add(this.metroRadioButtonSystArcAmd64);
            this.panelBasic.Controls.Add(this.metroButtonBeginCopy);
            this.panelBasic.Controls.Add(this.metroRadioButtonSysArcx86);
            this.panelBasic.Controls.Add(this.metroLabelChoose);
            this.panelBasic.Controls.Add(this.metroProgressBarCopyProgress);
            this.panelBasic.Controls.Add(this.metroLabelCurrentItem);
            this.flowLayoutPanelMain.SetFlowBreak(this.panelBasic, true);
            this.panelBasic.Location = new System.Drawing.Point(3, 203);
            this.panelBasic.Name = "panelBasic";
            this.panelBasic.Size = new System.Drawing.Size(513, 153);
            this.panelBasic.TabIndex = 22;
            // 
            // flowLayoutPanelMain
            // 
            this.flowLayoutPanelMain.AutoSize = true;
            this.flowLayoutPanelMain.Controls.Add(this.panelMES);
            this.flowLayoutPanelMain.Controls.Add(this.panelBasic);
            this.flowLayoutPanelMain.Location = new System.Drawing.Point(78, 136);
            this.flowLayoutPanelMain.Name = "flowLayoutPanelMain";
            this.flowLayoutPanelMain.Size = new System.Drawing.Size(1038, 359);
            this.flowLayoutPanelMain.TabIndex = 23;
            // 
            // metroLabelMESSwitch
            // 
            this.metroLabelMESSwitch.AutoSize = true;
            this.metroLabelMESSwitch.Location = new System.Drawing.Point(78, 111);
            this.metroLabelMESSwitch.Name = "metroLabelMESSwitch";
            this.metroLabelMESSwitch.Size = new System.Drawing.Size(118, 19);
            this.metroLabelMESSwitch.TabIndex = 24;
            this.metroLabelMESSwitch.Text = "Upload Test Result:";
            // 
            // metroToggleMESSwitch
            // 
            this.metroToggleMESSwitch.AutoSize = true;
            this.metroToggleMESSwitch.Location = new System.Drawing.Point(200, 113);
            this.metroToggleMESSwitch.Name = "metroToggleMESSwitch";
            this.metroToggleMESSwitch.Size = new System.Drawing.Size(80, 17);
            this.metroToggleMESSwitch.TabIndex = 25;
            this.metroToggleMESSwitch.Text = "Off";
            this.metroToggleMESSwitch.UseSelectable = true;
            this.metroToggleMESSwitch.CheckedChanged += new System.EventHandler(this.metroToggleMESSwitch_CheckedChanged);
            // 
            // FormUSBCopySFT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 560);
            this.Controls.Add(this.metroToggleMESSwitch);
            this.Controls.Add(this.metroLabelMESSwitch);
            this.Controls.Add(this.flowLayoutPanelMain);
            this.Controls.Add(this.metroLinkBack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormUSBCopySFT";
            this.Resizable = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUSBCopy_FormClosing);
            this.panelMES.ResumeLayout(false);
            this.panelMES.PerformLayout();
            this.panelBasic.ResumeLayout(false);
            this.panelBasic.PerformLayout();
            this.flowLayoutPanelMain.ResumeLayout(false);
            this.flowLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLink metroLinkBack;
        private MetroFramework.Controls.MetroComboBox metroComboBoxRemovableDisks;
        private MetroFramework.Controls.MetroButton metroButtonBeginCopy;
        private MetroFramework.Controls.MetroLabel metroLabelChoose;
        private MetroFramework.Controls.MetroProgressBar metroProgressBarCopyProgress;
        private MetroFramework.Controls.MetroLabel metroLabelCurrentItem;
        private MetroFramework.Controls.MetroLabel metroLabelMESCloudURL;
        private MetroFramework.Controls.MetroTextBox metroTextBoxMESCloudURL;
        private MetroFramework.Controls.MetroTextBox metroTextBoxMESCloudUserName;
        private MetroFramework.Controls.MetroLabel metroLabelMESCloudUserName;
        private MetroFramework.Controls.MetroLabel metroLabelMESCloudPassword;
        private MetroFramework.Controls.MetroLabel metroLabelSystemArchitecture;
        private MetroFramework.Controls.MetroRadioButton metroRadioButtonSysArcx86;
        private MetroFramework.Controls.MetroRadioButton metroRadioButtonSystArcAmd64;
        private MetroFramework.Controls.MetroLabel metroLabelShowPassword;
        private MetroFramework.Controls.MetroToggle metroToggleShowPassword;
        private System.Windows.Forms.TextBox metroTextBoxMESCloudPassword;
        private System.Windows.Forms.Panel panelMES;
        private System.Windows.Forms.Panel panelBasic;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMain;
        private MetroFramework.Controls.MetroLabel metroLabelMESSwitch;
        private MetroFramework.Controls.MetroToggle metroToggleMESSwitch;
    }
}