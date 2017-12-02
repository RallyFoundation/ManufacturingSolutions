namespace WindowsManufacturingStudio
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
            this.metroTileInstallImages = new MetroFramework.Controls.MetroTile();
            this.metroTileBootImages = new MetroFramework.Controls.MetroTile();
            this.metroTileImageGroups = new MetroFramework.Controls.MetroTile();
            this.metroTileFFUImages = new MetroFramework.Controls.MetroTile();
            this.metroTileClientPulse = new MetroFramework.Controls.MetroTile();
            this.metroTileSettings = new MetroFramework.Controls.MetroTile();
            this.metroTileCreateBootWindowsPE = new MetroFramework.Controls.MetroTile();
            this.metroTileImageLookups = new MetroFramework.Controls.MetroTile();
            this.metroTileImageFiles = new MetroFramework.Controls.MetroTile();
            this.metroTileLogHistory = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // metroTileInstallImages
            // 
            this.metroTileInstallImages.BackColor = System.Drawing.Color.White;
            this.metroTileInstallImages.Location = new System.Drawing.Point(23, 76);
            this.metroTileInstallImages.Name = "metroTileInstallImages";
            this.metroTileInstallImages.Size = new System.Drawing.Size(120, 120);
            this.metroTileInstallImages.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTileInstallImages.TabIndex = 0;
            this.metroTileInstallImages.Text = "Install Images";
            this.metroTileInstallImages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileInstallImages.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileInstallImages.Click += new System.EventHandler(this.metroTileInstallImages_Click);
            // 
            // metroTileBootImages
            // 
            this.metroTileBootImages.BackColor = System.Drawing.Color.White;
            this.metroTileBootImages.Location = new System.Drawing.Point(149, 76);
            this.metroTileBootImages.Name = "metroTileBootImages";
            this.metroTileBootImages.Size = new System.Drawing.Size(120, 120);
            this.metroTileBootImages.Style = MetroFramework.MetroColorStyle.Yellow;
            this.metroTileBootImages.TabIndex = 1;
            this.metroTileBootImages.Text = "Boot Images";
            this.metroTileBootImages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileBootImages.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileBootImages.Click += new System.EventHandler(this.metroTileBootImages_Click);
            // 
            // metroTileImageGroups
            // 
            this.metroTileImageGroups.BackColor = System.Drawing.Color.White;
            this.metroTileImageGroups.Location = new System.Drawing.Point(275, 76);
            this.metroTileImageGroups.Name = "metroTileImageGroups";
            this.metroTileImageGroups.Size = new System.Drawing.Size(120, 120);
            this.metroTileImageGroups.Style = MetroFramework.MetroColorStyle.Pink;
            this.metroTileImageGroups.TabIndex = 2;
            this.metroTileImageGroups.Text = "Image Groups";
            this.metroTileImageGroups.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileImageGroups.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileImageGroups.Click += new System.EventHandler(this.metroTileImageGroups_Click);
            // 
            // metroTileFFUImages
            // 
            this.metroTileFFUImages.BackColor = System.Drawing.Color.White;
            this.metroTileFFUImages.Location = new System.Drawing.Point(653, 202);
            this.metroTileFFUImages.Name = "metroTileFFUImages";
            this.metroTileFFUImages.Size = new System.Drawing.Size(120, 120);
            this.metroTileFFUImages.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroTileFFUImages.TabIndex = 3;
            this.metroTileFFUImages.Text = "Full Flash Update\r\n (FFU) \r\nImages";
            this.metroTileFFUImages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileFFUImages.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileFFUImages.Visible = false;
            this.metroTileFFUImages.Click += new System.EventHandler(this.metroTileFFUImages_Click);
            // 
            // metroTileClientPulse
            // 
            this.metroTileClientPulse.BackColor = System.Drawing.Color.White;
            this.metroTileClientPulse.Location = new System.Drawing.Point(23, 202);
            this.metroTileClientPulse.Name = "metroTileClientPulse";
            this.metroTileClientPulse.Size = new System.Drawing.Size(120, 120);
            this.metroTileClientPulse.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroTileClientPulse.TabIndex = 4;
            this.metroTileClientPulse.Text = "Client Pulse";
            this.metroTileClientPulse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileClientPulse.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileClientPulse.Click += new System.EventHandler(this.metroTileClientPulse_Click);
            // 
            // metroTileSettings
            // 
            this.metroTileSettings.BackColor = System.Drawing.Color.White;
            this.metroTileSettings.Location = new System.Drawing.Point(275, 202);
            this.metroTileSettings.Name = "metroTileSettings";
            this.metroTileSettings.Size = new System.Drawing.Size(120, 120);
            this.metroTileSettings.Style = MetroFramework.MetroColorStyle.Red;
            this.metroTileSettings.TabIndex = 5;
            this.metroTileSettings.Text = "Settings";
            this.metroTileSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileSettings.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileSettings.Click += new System.EventHandler(this.metroTileSettings_Click);
            // 
            // metroTileCreateBootWindowsPE
            // 
            this.metroTileCreateBootWindowsPE.BackColor = System.Drawing.Color.White;
            this.metroTileCreateBootWindowsPE.Location = new System.Drawing.Point(653, 76);
            this.metroTileCreateBootWindowsPE.Name = "metroTileCreateBootWindowsPE";
            this.metroTileCreateBootWindowsPE.Size = new System.Drawing.Size(120, 120);
            this.metroTileCreateBootWindowsPE.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileCreateBootWindowsPE.TabIndex = 6;
            this.metroTileCreateBootWindowsPE.Text = "Create \r\nBoot\r\nWindows PE ";
            this.metroTileCreateBootWindowsPE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileCreateBootWindowsPE.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileCreateBootWindowsPE.Click += new System.EventHandler(this.metroTileCreateBootWindowsPE_Click);
            // 
            // metroTileImageLookups
            // 
            this.metroTileImageLookups.BackColor = System.Drawing.Color.White;
            this.metroTileImageLookups.Location = new System.Drawing.Point(527, 76);
            this.metroTileImageLookups.Name = "metroTileImageLookups";
            this.metroTileImageLookups.Size = new System.Drawing.Size(120, 120);
            this.metroTileImageLookups.Style = MetroFramework.MetroColorStyle.Purple;
            this.metroTileImageLookups.TabIndex = 7;
            this.metroTileImageLookups.Text = "Image Lookups";
            this.metroTileImageLookups.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileImageLookups.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileImageLookups.Click += new System.EventHandler(this.metroTileImageLookups_Click);
            // 
            // metroTileImageFiles
            // 
            this.metroTileImageFiles.BackColor = System.Drawing.Color.White;
            this.metroTileImageFiles.Location = new System.Drawing.Point(401, 76);
            this.metroTileImageFiles.Name = "metroTileImageFiles";
            this.metroTileImageFiles.Size = new System.Drawing.Size(120, 120);
            this.metroTileImageFiles.Style = MetroFramework.MetroColorStyle.Green;
            this.metroTileImageFiles.TabIndex = 8;
            this.metroTileImageFiles.Text = "Image Files";
            this.metroTileImageFiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileImageFiles.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileImageFiles.Click += new System.EventHandler(this.metroTileImageFiles_Click);
            // 
            // metroTileLogHistory
            // 
            this.metroTileLogHistory.BackColor = System.Drawing.Color.White;
            this.metroTileLogHistory.Location = new System.Drawing.Point(149, 202);
            this.metroTileLogHistory.Name = "metroTileLogHistory";
            this.metroTileLogHistory.Size = new System.Drawing.Size(120, 120);
            this.metroTileLogHistory.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileLogHistory.TabIndex = 9;
            this.metroTileLogHistory.Text = "Log / History";
            this.metroTileLogHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileLogHistory.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileLogHistory.Click += new System.EventHandler(this.metroTileLogHistory_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.metroTileLogHistory);
            this.Controls.Add(this.metroTileImageFiles);
            this.Controls.Add(this.metroTileImageLookups);
            this.Controls.Add(this.metroTileCreateBootWindowsPE);
            this.Controls.Add(this.metroTileSettings);
            this.Controls.Add(this.metroTileClientPulse);
            this.Controls.Add(this.metroTileFFUImages);
            this.Controls.Add(this.metroTileImageGroups);
            this.Controls.Add(this.metroTileBootImages);
            this.Controls.Add(this.metroTileInstallImages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Windows Manufacturing Studio";
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTile metroTileInstallImages;
        private MetroFramework.Controls.MetroTile metroTileBootImages;
        private MetroFramework.Controls.MetroTile metroTileImageGroups;
        private MetroFramework.Controls.MetroTile metroTileFFUImages;
        private MetroFramework.Controls.MetroTile metroTileClientPulse;
        private MetroFramework.Controls.MetroTile metroTileSettings;
        private MetroFramework.Controls.MetroTile metroTileCreateBootWindowsPE;
        private MetroFramework.Controls.MetroTile metroTileImageLookups;
        private MetroFramework.Controls.MetroTile metroTileImageFiles;
        private MetroFramework.Controls.MetroTile metroTileLogHistory;
    }
}

