namespace WDSManager
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
            this.metroTileFFU = new MetroFramework.Controls.MetroTile();
            this.metroTileNetwork = new MetroFramework.Controls.MetroTile();
            this.metroTileSettings = new MetroFramework.Controls.MetroTile();
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
            // metroTileFFU
            // 
            this.metroTileFFU.BackColor = System.Drawing.Color.White;
            this.metroTileFFU.Location = new System.Drawing.Point(401, 76);
            this.metroTileFFU.Name = "metroTileFFU";
            this.metroTileFFU.Size = new System.Drawing.Size(120, 120);
            this.metroTileFFU.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileFFU.TabIndex = 3;
            this.metroTileFFU.Text = "FFU";
            this.metroTileFFU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileFFU.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            // 
            // metroTileNetwork
            // 
            this.metroTileNetwork.BackColor = System.Drawing.Color.White;
            this.metroTileNetwork.Location = new System.Drawing.Point(23, 202);
            this.metroTileNetwork.Name = "metroTileNetwork";
            this.metroTileNetwork.Size = new System.Drawing.Size(120, 120);
            this.metroTileNetwork.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroTileNetwork.TabIndex = 4;
            this.metroTileNetwork.Text = "Network";
            this.metroTileNetwork.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileNetwork.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            // 
            // metroTileSettings
            // 
            this.metroTileSettings.BackColor = System.Drawing.Color.White;
            this.metroTileSettings.Location = new System.Drawing.Point(149, 202);
            this.metroTileSettings.Name = "metroTileSettings";
            this.metroTileSettings.Size = new System.Drawing.Size(120, 120);
            this.metroTileSettings.Style = MetroFramework.MetroColorStyle.Red;
            this.metroTileSettings.TabIndex = 5;
            this.metroTileSettings.Text = "Settings";
            this.metroTileSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileSettings.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.metroTileSettings);
            this.Controls.Add(this.metroTileNetwork);
            this.Controls.Add(this.metroTileFFU);
            this.Controls.Add(this.metroTileImageGroups);
            this.Controls.Add(this.metroTileBootImages);
            this.Controls.Add(this.metroTileInstallImages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTile metroTileInstallImages;
        private MetroFramework.Controls.MetroTile metroTileBootImages;
        private MetroFramework.Controls.MetroTile metroTileImageGroups;
        private MetroFramework.Controls.MetroTile metroTileFFU;
        private MetroFramework.Controls.MetroTile metroTileNetwork;
        private MetroFramework.Controls.MetroTile metroTileSettings;
    }
}

