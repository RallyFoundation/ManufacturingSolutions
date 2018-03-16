namespace OA3Xpress
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
            this.metroTileErase = new MetroFramework.Controls.MetroTile();
            this.metroTileValidate = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // metroTileConfigure
            // 
            this.metroTileConfigure.Location = new System.Drawing.Point(23, 69);
            this.metroTileConfigure.Name = "metroTileConfigure";
            this.metroTileConfigure.Size = new System.Drawing.Size(151, 111);
            this.metroTileConfigure.Style = MetroFramework.MetroColorStyle.Orange;
            this.metroTileConfigure.TabIndex = 3;
            this.metroTileConfigure.Text = "Configure";
            this.metroTileConfigure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileConfigure.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileConfigure.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileConfigure.Click += new System.EventHandler(this.metroTileConfigure_Click);
            // 
            // metroTileStart
            // 
            this.metroTileStart.Location = new System.Drawing.Point(195, 69);
            this.metroTileStart.Name = "metroTileStart";
            this.metroTileStart.Size = new System.Drawing.Size(151, 111);
            this.metroTileStart.Style = MetroFramework.MetroColorStyle.Lime;
            this.metroTileStart.TabIndex = 4;
            this.metroTileStart.Text = "Start";
            this.metroTileStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileStart.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileStart.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileStart.Click += new System.EventHandler(this.metroTileStart_Click);
            // 
            // metroTileErase
            // 
            this.metroTileErase.Location = new System.Drawing.Point(195, 202);
            this.metroTileErase.Name = "metroTileErase";
            this.metroTileErase.Size = new System.Drawing.Size(151, 111);
            this.metroTileErase.Style = MetroFramework.MetroColorStyle.Yellow;
            this.metroTileErase.TabIndex = 6;
            this.metroTileErase.Text = "Erase";
            this.metroTileErase.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileErase.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileErase.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileErase.Click += new System.EventHandler(this.metroTileErase_Click);
            // 
            // metroTileValidate
            // 
            this.metroTileValidate.Location = new System.Drawing.Point(23, 202);
            this.metroTileValidate.Name = "metroTileValidate";
            this.metroTileValidate.Size = new System.Drawing.Size(151, 111);
            this.metroTileValidate.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTileValidate.TabIndex = 5;
            this.metroTileValidate.Text = "Validate";
            this.metroTileValidate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metroTileValidate.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTileValidate.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTileValidate.Click += new System.EventHandler(this.metroTileValidate_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 336);
            this.Controls.Add(this.metroTileErase);
            this.Controls.Add(this.metroTileValidate);
            this.Controls.Add(this.metroTileStart);
            this.Controls.Add(this.metroTileConfigure);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Resizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "OA3.0 Process Execution Panel";
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroTile metroTileConfigure;
        private MetroFramework.Controls.MetroTile metroTileStart;
        private MetroFramework.Controls.MetroTile metroTileErase;
        private MetroFramework.Controls.MetroTile metroTileValidate;
    }
}

