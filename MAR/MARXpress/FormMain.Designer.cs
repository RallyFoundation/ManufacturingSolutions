namespace MARXpress
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
            this.metroTileConfigure = new MetroFramework.Controls.MetroTile();
            this.metroTileStart = new MetroFramework.Controls.MetroTile();
            this.metroTileConfigureLookup = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // metroTileConfigure
            // 
            this.metroTileConfigure.Location = new System.Drawing.Point(23, 198);
            this.metroTileConfigure.Name = "metroTileConfigure";
            this.metroTileConfigure.Size = new System.Drawing.Size(329, 111);
            this.metroTileConfigure.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileConfigure.TabIndex = 3;
            this.metroTileConfigure.Text = "Configure \r\nOA3Tool";
            this.metroTileConfigure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileConfigure.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileConfigure.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileConfigure.Click += new System.EventHandler(this.metroTileConfigure_Click);
            // 
            // metroTileStart
            // 
            this.metroTileStart.Location = new System.Drawing.Point(23, 72);
            this.metroTileStart.Name = "metroTileStart";
            this.metroTileStart.Size = new System.Drawing.Size(329, 111);
            this.metroTileStart.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileStart.TabIndex = 4;
            this.metroTileStart.Text = "Start";
            this.metroTileStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileStart.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileStart.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileStart.Click += new System.EventHandler(this.metroTileStart_Click);
            // 
            // metroTileConfigureLookup
            // 
            this.metroTileConfigureLookup.Location = new System.Drawing.Point(196, 199);
            this.metroTileConfigureLookup.Name = "metroTileConfigureLookup";
            this.metroTileConfigureLookup.Size = new System.Drawing.Size(156, 111);
            this.metroTileConfigureLookup.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroTileConfigureLookup.TabIndex = 5;
            this.metroTileConfigureLookup.Text = "Configure \r\nSKU / LPN\r\nMapping";
            this.metroTileConfigureLookup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileConfigureLookup.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileConfigureLookup.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileConfigureLookup.Visible = false;
            this.metroTileConfigureLookup.Click += new System.EventHandler(this.metroTileConfigureLookup_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 334);
            this.Controls.Add(this.metroTileConfigureLookup);
            this.Controls.Add(this.metroTileStart);
            this.Controls.Add(this.metroTileConfigure);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MAR Execution Panel";
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroTile metroTileConfigure;
        private MetroFramework.Controls.MetroTile metroTileStart;
        private MetroFramework.Controls.MetroTile metroTileConfigureLookup;
    }
}

