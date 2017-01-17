namespace OA3ToolConfGen
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageBasic = new System.Windows.Forms.TabPage();
            this.buttonTest = new System.Windows.Forms.Button();
            this.textBoxXMLResultFileOutputPath = new System.Windows.Forms.TextBox();
            this.labelXMLResultFileOutputPath = new System.Windows.Forms.Label();
            this.textBoxBinFileOutputPath = new System.Windows.Forms.TextBox();
            this.labelBinFileOuputPath = new System.Windows.Forms.Label();
            this.buttonMore = new System.Windows.Forms.Button();
            this.textBoxCloudConfigurationID = new System.Windows.Forms.TextBox();
            this.labelCloudConfigurationID = new System.Windows.Forms.Label();
            this.textBoxKeyProviderServicePortNumber = new System.Windows.Forms.TextBox();
            this.labelKPSPortNumber = new System.Windows.Forms.Label();
            this.textBoxKPSAddress = new System.Windows.Forms.TextBox();
            this.labelKPSAddress = new System.Windows.Forms.Label();
            this.tabPageParameters = new System.Windows.Forms.TabPage();
            this.ucParameterSN = new OA3ToolConfGen.UCParameter();
            this.ucParameterOPON = new OA3ToolConfGen.UCParameter();
            this.ucParameterOPN = new OA3ToolConfGen.UCParameter();
            this.ucParameterLPN = new OA3ToolConfGen.UCParameter();
            this.tabPageOHR = new System.Windows.Forms.TabPage();
            this.checkBoxOHRRequired = new System.Windows.Forms.CheckBox();
            this.ucohrOHRData = new OA3ToolConfGen.UCOHR();
            this.tabPagePreview = new System.Windows.Forms.TabPage();
            this.webBrowserPreview = new System.Windows.Forms.WebBrowser();
            this.tabPageCloud = new System.Windows.Forms.TabPage();
            this.webBrowserCloud = new System.Windows.Forms.WebBrowser();
            this.saveFileDialogSave = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogOpen = new System.Windows.Forms.OpenFileDialog();
            this.menuStripMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageBasic.SuspendLayout();
            this.tabPageParameters.SuspendLayout();
            this.tabPageOHR.SuspendLayout();
            this.tabPagePreview.SuspendLayout();
            this.tabPageCloud.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(624, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItemSaveAs,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItemSaveAs
            // 
            this.toolStripMenuItemSaveAs.Name = "toolStripMenuItemSaveAs";
            this.toolStripMenuItemSaveAs.Size = new System.Drawing.Size(123, 22);
            this.toolStripMenuItemSaveAs.Text = "Save As...";
            this.toolStripMenuItemSaveAs.Click += new System.EventHandler(this.toolStripMenuItemSaveAs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(120, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator,
            this.helpToolStripButton,
            this.toolStripSeparator3});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(624, 25);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStripMain.Location = new System.Drawing.Point(0, 419);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(624, 22);
            this.statusStripMain.TabIndex = 2;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageBasic);
            this.tabControlMain.Controls.Add(this.tabPageParameters);
            this.tabControlMain.Controls.Add(this.tabPageOHR);
            this.tabControlMain.Controls.Add(this.tabPagePreview);
            this.tabControlMain.Controls.Add(this.tabPageCloud);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 49);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(624, 370);
            this.tabControlMain.TabIndex = 3;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageBasic
            // 
            this.tabPageBasic.Controls.Add(this.buttonTest);
            this.tabPageBasic.Controls.Add(this.textBoxXMLResultFileOutputPath);
            this.tabPageBasic.Controls.Add(this.labelXMLResultFileOutputPath);
            this.tabPageBasic.Controls.Add(this.textBoxBinFileOutputPath);
            this.tabPageBasic.Controls.Add(this.labelBinFileOuputPath);
            this.tabPageBasic.Controls.Add(this.buttonMore);
            this.tabPageBasic.Controls.Add(this.textBoxCloudConfigurationID);
            this.tabPageBasic.Controls.Add(this.labelCloudConfigurationID);
            this.tabPageBasic.Controls.Add(this.textBoxKeyProviderServicePortNumber);
            this.tabPageBasic.Controls.Add(this.labelKPSPortNumber);
            this.tabPageBasic.Controls.Add(this.textBoxKPSAddress);
            this.tabPageBasic.Controls.Add(this.labelKPSAddress);
            this.tabPageBasic.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasic.Name = "tabPageBasic";
            this.tabPageBasic.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBasic.Size = new System.Drawing.Size(616, 344);
            this.tabPageBasic.TabIndex = 0;
            this.tabPageBasic.Text = "Basic";
            this.tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(467, 108);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 11;
            this.buttonTest.Text = "Test...";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // textBoxXMLResultFileOutputPath
            // 
            this.textBoxXMLResultFileOutputPath.Location = new System.Drawing.Point(268, 206);
            this.textBoxXMLResultFileOutputPath.MaxLength = 150;
            this.textBoxXMLResultFileOutputPath.Name = "textBoxXMLResultFileOutputPath";
            this.textBoxXMLResultFileOutputPath.Size = new System.Drawing.Size(193, 20);
            this.textBoxXMLResultFileOutputPath.TabIndex = 10;
            // 
            // labelXMLResultFileOutputPath
            // 
            this.labelXMLResultFileOutputPath.Location = new System.Drawing.Point(95, 209);
            this.labelXMLResultFileOutputPath.Name = "labelXMLResultFileOutputPath";
            this.labelXMLResultFileOutputPath.Size = new System.Drawing.Size(167, 23);
            this.labelXMLResultFileOutputPath.TabIndex = 9;
            this.labelXMLResultFileOutputPath.Text = "XML Result File Output Path:";
            this.labelXMLResultFileOutputPath.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxBinFileOutputPath
            // 
            this.textBoxBinFileOutputPath.Location = new System.Drawing.Point(268, 174);
            this.textBoxBinFileOutputPath.MaxLength = 150;
            this.textBoxBinFileOutputPath.Name = "textBoxBinFileOutputPath";
            this.textBoxBinFileOutputPath.Size = new System.Drawing.Size(193, 20);
            this.textBoxBinFileOutputPath.TabIndex = 8;
            // 
            // labelBinFileOuputPath
            // 
            this.labelBinFileOuputPath.Location = new System.Drawing.Point(95, 177);
            this.labelBinFileOuputPath.Name = "labelBinFileOuputPath";
            this.labelBinFileOuputPath.Size = new System.Drawing.Size(167, 23);
            this.labelBinFileOuputPath.TabIndex = 7;
            this.labelBinFileOuputPath.Text = "Bin File Output Path:";
            this.labelBinFileOuputPath.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonMore
            // 
            this.buttonMore.Location = new System.Drawing.Point(467, 139);
            this.buttonMore.Name = "buttonMore";
            this.buttonMore.Size = new System.Drawing.Size(75, 23);
            this.buttonMore.TabIndex = 6;
            this.buttonMore.Text = "More...";
            this.buttonMore.UseVisualStyleBackColor = true;
            this.buttonMore.Click += new System.EventHandler(this.buttonMore_Click);
            // 
            // textBoxCloudConfigurationID
            // 
            this.textBoxCloudConfigurationID.Location = new System.Drawing.Point(268, 141);
            this.textBoxCloudConfigurationID.MaxLength = 50;
            this.textBoxCloudConfigurationID.Name = "textBoxCloudConfigurationID";
            this.textBoxCloudConfigurationID.Size = new System.Drawing.Size(193, 20);
            this.textBoxCloudConfigurationID.TabIndex = 5;
            // 
            // labelCloudConfigurationID
            // 
            this.labelCloudConfigurationID.Location = new System.Drawing.Point(95, 144);
            this.labelCloudConfigurationID.Name = "labelCloudConfigurationID";
            this.labelCloudConfigurationID.Size = new System.Drawing.Size(167, 23);
            this.labelCloudConfigurationID.TabIndex = 4;
            this.labelCloudConfigurationID.Text = "Configuration ID:";
            this.labelCloudConfigurationID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxKeyProviderServicePortNumber
            // 
            this.textBoxKeyProviderServicePortNumber.Location = new System.Drawing.Point(268, 110);
            this.textBoxKeyProviderServicePortNumber.MaxLength = 6;
            this.textBoxKeyProviderServicePortNumber.Name = "textBoxKeyProviderServicePortNumber";
            this.textBoxKeyProviderServicePortNumber.Size = new System.Drawing.Size(193, 20);
            this.textBoxKeyProviderServicePortNumber.TabIndex = 3;
            // 
            // labelKPSPortNumber
            // 
            this.labelKPSPortNumber.Location = new System.Drawing.Point(81, 113);
            this.labelKPSPortNumber.Name = "labelKPSPortNumber";
            this.labelKPSPortNumber.Size = new System.Drawing.Size(181, 13);
            this.labelKPSPortNumber.TabIndex = 2;
            this.labelKPSPortNumber.Text = "Key Provider Service Port Number:";
            this.labelKPSPortNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxKPSAddress
            // 
            this.textBoxKPSAddress.Location = new System.Drawing.Point(268, 80);
            this.textBoxKPSAddress.MaxLength = 150;
            this.textBoxKPSAddress.Name = "textBoxKPSAddress";
            this.textBoxKPSAddress.Size = new System.Drawing.Size(193, 20);
            this.textBoxKPSAddress.TabIndex = 1;
            // 
            // labelKPSAddress
            // 
            this.labelKPSAddress.Location = new System.Drawing.Point(92, 83);
            this.labelKPSAddress.Name = "labelKPSAddress";
            this.labelKPSAddress.Size = new System.Drawing.Size(170, 13);
            this.labelKPSAddress.TabIndex = 0;
            this.labelKPSAddress.Text = "Key Provider Service Address:";
            this.labelKPSAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabPageParameters
            // 
            this.tabPageParameters.Controls.Add(this.ucParameterSN);
            this.tabPageParameters.Controls.Add(this.ucParameterOPON);
            this.tabPageParameters.Controls.Add(this.ucParameterOPN);
            this.tabPageParameters.Controls.Add(this.ucParameterLPN);
            this.tabPageParameters.Location = new System.Drawing.Point(4, 22);
            this.tabPageParameters.Name = "tabPageParameters";
            this.tabPageParameters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageParameters.Size = new System.Drawing.Size(616, 344);
            this.tabPageParameters.TabIndex = 1;
            this.tabPageParameters.Text = "Parameters";
            this.tabPageParameters.UseVisualStyleBackColor = true;
            // 
            // ucParameterSN
            // 
            this.ucParameterSN.DBConnectionString = null;
            this.ucParameterSN.Location = new System.Drawing.Point(111, 176);
            this.ucParameterSN.Name = "ucParameterSN";
            this.ucParameterSN.ParameterType = OA3ToolConfGen.ParameterType.SerialNumber;
            this.ucParameterSN.Size = new System.Drawing.Size(360, 31);
            this.ucParameterSN.TabIndex = 7;
            // 
            // ucParameterOPON
            // 
            this.ucParameterOPON.DBConnectionString = null;
            this.ucParameterOPON.Location = new System.Drawing.Point(111, 142);
            this.ucParameterOPON.Name = "ucParameterOPON";
            this.ucParameterOPON.ParameterType = OA3ToolConfGen.ParameterType.OEMPONumber;
            this.ucParameterOPON.Size = new System.Drawing.Size(360, 31);
            this.ucParameterOPON.TabIndex = 6;
            // 
            // ucParameterOPN
            // 
            this.ucParameterOPN.DBConnectionString = null;
            this.ucParameterOPN.Location = new System.Drawing.Point(111, 107);
            this.ucParameterOPN.Name = "ucParameterOPN";
            this.ucParameterOPN.ParameterType = OA3ToolConfGen.ParameterType.OEMPartNumber;
            this.ucParameterOPN.Size = new System.Drawing.Size(360, 31);
            this.ucParameterOPN.TabIndex = 5;
            // 
            // ucParameterLPN
            // 
            this.ucParameterLPN.DBConnectionString = null;
            this.ucParameterLPN.Location = new System.Drawing.Point(111, 73);
            this.ucParameterLPN.Name = "ucParameterLPN";
            this.ucParameterLPN.ParameterType = OA3ToolConfGen.ParameterType.LicensablePartNumber;
            this.ucParameterLPN.Size = new System.Drawing.Size(360, 31);
            this.ucParameterLPN.TabIndex = 4;
            // 
            // tabPageOHR
            // 
            this.tabPageOHR.Controls.Add(this.checkBoxOHRRequired);
            this.tabPageOHR.Controls.Add(this.ucohrOHRData);
            this.tabPageOHR.Location = new System.Drawing.Point(4, 22);
            this.tabPageOHR.Name = "tabPageOHR";
            this.tabPageOHR.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOHR.Size = new System.Drawing.Size(616, 344);
            this.tabPageOHR.TabIndex = 2;
            this.tabPageOHR.Text = "OHR";
            this.tabPageOHR.UseVisualStyleBackColor = true;
            // 
            // checkBoxOHRRequired
            // 
            this.checkBoxOHRRequired.AutoSize = true;
            this.checkBoxOHRRequired.Location = new System.Drawing.Point(145, 54);
            this.checkBoxOHRRequired.Name = "checkBoxOHRRequired";
            this.checkBoxOHRRequired.Size = new System.Drawing.Size(96, 17);
            this.checkBoxOHRRequired.TabIndex = 1;
            this.checkBoxOHRRequired.Text = "OHR Required";
            this.checkBoxOHRRequired.UseVisualStyleBackColor = true;
            this.checkBoxOHRRequired.CheckedChanged += new System.EventHandler(this.checkBoxOHRRequired_CheckedChanged);
            // 
            // ucohrOHRData
            // 
            this.ucohrOHRData.Enabled = false;
            this.ucohrOHRData.Location = new System.Drawing.Point(126, 77);
            this.ucohrOHRData.Name = "ucohrOHRData";
            this.ucohrOHRData.Size = new System.Drawing.Size(324, 201);
            this.ucohrOHRData.TabIndex = 0;
            // 
            // tabPagePreview
            // 
            this.tabPagePreview.Controls.Add(this.webBrowserPreview);
            this.tabPagePreview.Location = new System.Drawing.Point(4, 22);
            this.tabPagePreview.Name = "tabPagePreview";
            this.tabPagePreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePreview.Size = new System.Drawing.Size(616, 344);
            this.tabPagePreview.TabIndex = 3;
            this.tabPagePreview.Text = "Preview";
            this.tabPagePreview.UseVisualStyleBackColor = true;
            // 
            // webBrowserPreview
            // 
            this.webBrowserPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserPreview.Location = new System.Drawing.Point(3, 3);
            this.webBrowserPreview.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserPreview.Name = "webBrowserPreview";
            this.webBrowserPreview.Size = new System.Drawing.Size(610, 338);
            this.webBrowserPreview.TabIndex = 0;
            // 
            // tabPageCloud
            // 
            this.tabPageCloud.Controls.Add(this.webBrowserCloud);
            this.tabPageCloud.Location = new System.Drawing.Point(4, 22);
            this.tabPageCloud.Name = "tabPageCloud";
            this.tabPageCloud.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCloud.Size = new System.Drawing.Size(616, 344);
            this.tabPageCloud.TabIndex = 4;
            this.tabPageCloud.Text = "Configurations from Server";
            this.tabPageCloud.UseVisualStyleBackColor = true;
            // 
            // webBrowserCloud
            // 
            this.webBrowserCloud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserCloud.Location = new System.Drawing.Point(3, 3);
            this.webBrowserCloud.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserCloud.Name = "webBrowserCloud";
            this.webBrowserCloud.Size = new System.Drawing.Size(610, 338);
            this.webBrowserCloud.TabIndex = 0;
            // 
            // saveFileDialogSave
            // 
            this.saveFileDialogSave.Filter = "CFG Files|*.CFG|XML Files|*.xml";
            // 
            // openFileDialogOpen
            // 
            this.openFileDialogOpen.Filter = "CFG Files|*.CFG|XML Files|*.xml";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.Text = "OA3Tool Configuration Generator - CSI DIS 3.0 Special Edition";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageBasic.ResumeLayout(false);
            this.tabPageBasic.PerformLayout();
            this.tabPageParameters.ResumeLayout(false);
            this.tabPageOHR.ResumeLayout(false);
            this.tabPageOHR.PerformLayout();
            this.tabPagePreview.ResumeLayout(false);
            this.tabPageCloud.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageBasic;
        private System.Windows.Forms.TabPage tabPageParameters;
        private System.Windows.Forms.TabPage tabPageOHR;
        private System.Windows.Forms.TabPage tabPagePreview;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private UCParameter ucParameterSN;
        private UCParameter ucParameterOPON;
        private UCParameter ucParameterOPN;
        private UCParameter ucParameterLPN;
        private System.Windows.Forms.CheckBox checkBoxOHRRequired;
        private UCOHR ucohrOHRData;
        private System.Windows.Forms.TextBox textBoxKPSAddress;
        private System.Windows.Forms.Label labelKPSAddress;
        private System.Windows.Forms.TextBox textBoxKeyProviderServicePortNumber;
        private System.Windows.Forms.Label labelKPSPortNumber;
        private System.Windows.Forms.Label labelCloudConfigurationID;
        private System.Windows.Forms.TextBox textBoxCloudConfigurationID;
        private System.Windows.Forms.Button buttonMore;
        private System.Windows.Forms.TextBox textBoxBinFileOutputPath;
        private System.Windows.Forms.Label labelBinFileOuputPath;
        private System.Windows.Forms.TextBox textBoxXMLResultFileOutputPath;
        private System.Windows.Forms.Label labelXMLResultFileOutputPath;
        private System.Windows.Forms.WebBrowser webBrowserPreview;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSave;
        private System.Windows.Forms.OpenFileDialog openFileDialogOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabPage tabPageCloud;
        private System.Windows.Forms.WebBrowser webBrowserCloud;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

