namespace MfgSolutionsDeploymentCenter
{
    partial class FormInstallation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInstallation));
            this.metroLinkBack = new MetroFramework.Controls.MetroLink();
            this.metroTileDISConfigurationCloud = new MetroFramework.Controls.MetroTile();
            this.metroTileCKI = new MetroFramework.Controls.MetroTile();
            this.metroTileFKI = new MetroFramework.Controls.MetroTile();
            this.metroTileFFKI = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // metroLinkBack
            // 
            this.metroLinkBack.BackgroundImage = global::MfgSolutionsDeploymentCenter.Properties.Resources.MB_0006_back;
            this.metroLinkBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.metroLinkBack.Location = new System.Drawing.Point(20, 60);
            this.metroLinkBack.Name = "metroLinkBack";
            this.metroLinkBack.Size = new System.Drawing.Size(45, 45);
            this.metroLinkBack.TabIndex = 1;
            this.metroLinkBack.UseSelectable = true;
            this.metroLinkBack.Click += new System.EventHandler(this.metroLinkBack_Click);
            // 
            // metroTileDISConfigurationCloud
            // 
            this.metroTileDISConfigurationCloud.ActiveControl = null;
            this.metroTileDISConfigurationCloud.Location = new System.Drawing.Point(23, 122);
            this.metroTileDISConfigurationCloud.Name = "metroTileDISConfigurationCloud";
            this.metroTileDISConfigurationCloud.Size = new System.Drawing.Size(250, 120);
            this.metroTileDISConfigurationCloud.Style = MetroFramework.MetroColorStyle.Silver;
            this.metroTileDISConfigurationCloud.TabIndex = 2;
            this.metroTileDISConfigurationCloud.Text = "DIS Configuration Cloud";
            this.metroTileDISConfigurationCloud.UseSelectable = true;
            this.metroTileDISConfigurationCloud.Click += new System.EventHandler(this.metroTileDISConfigurationCloud_Click);
            // 
            // metroTileCKI
            // 
            this.metroTileCKI.ActiveControl = null;
            this.metroTileCKI.Location = new System.Drawing.Point(293, 122);
            this.metroTileCKI.Name = "metroTileCKI";
            this.metroTileCKI.Size = new System.Drawing.Size(250, 120);
            this.metroTileCKI.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileCKI.TabIndex = 3;
            this.metroTileCKI.Text = "Inventory for an OEM (CKI)";
            this.metroTileCKI.UseSelectable = true;
            // 
            // metroTileFKI
            // 
            this.metroTileFKI.ActiveControl = null;
            this.metroTileFKI.Location = new System.Drawing.Point(23, 262);
            this.metroTileFKI.Name = "metroTileFKI";
            this.metroTileFKI.Size = new System.Drawing.Size(250, 120);
            this.metroTileFKI.Style = MetroFramework.MetroColorStyle.Purple;
            this.metroTileFKI.TabIndex = 4;
            this.metroTileFKI.Text = "Inventory for a TPI (FKI)";
            this.metroTileFKI.UseSelectable = true;
            // 
            // metroTileFFKI
            // 
            this.metroTileFFKI.ActiveControl = null;
            this.metroTileFFKI.Location = new System.Drawing.Point(293, 262);
            this.metroTileFFKI.Name = "metroTileFFKI";
            this.metroTileFFKI.Size = new System.Drawing.Size(250, 120);
            this.metroTileFFKI.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileFFKI.TabIndex = 5;
            this.metroTileFFKI.Text = "Inventory for a Factory Floor (FFKI)";
            this.metroTileFFKI.UseSelectable = true;
            // 
            // FormInstallation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Controls.Add(this.metroTileFFKI);
            this.Controls.Add(this.metroTileFKI);
            this.Controls.Add(this.metroTileCKI);
            this.Controls.Add(this.metroTileDISConfigurationCloud);
            this.Controls.Add(this.metroLinkBack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormInstallation";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DIS Installation";
            this.TransparencyKey = System.Drawing.Color.GhostWhite;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormInstallation_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLink metroLinkBack;
        private MetroFramework.Controls.MetroTile metroTileDISConfigurationCloud;
        private MetroFramework.Controls.MetroTile metroTileCKI;
        private MetroFramework.Controls.MetroTile metroTileFKI;
        private MetroFramework.Controls.MetroTile metroTileFFKI;


    }
}